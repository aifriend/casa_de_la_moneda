using System;
using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Diagnosis;
using Idpsa.Control.Manuals;
using Idpsa.Control.Sequence;
using Idpsa.Control.Standard;
using Idpsa.Control.Subsystem;
using IdpsaControl;
using MonitorWrapper;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    public class RuhlamatManualFeed : ISubsystemStateAware, IAutomaticRunnable, IItemSuplier<GrupoPasaportes>,
                                      IDiagnosisOwner
    {
        private const short ShowScreenTime = 3000;
        private const short PushBlockTimeOut = 1500;

        private const short WaitWeightTimeOut = 5000;
        private const int ReadWeightMaxTries = 10;
        public const double MaxErrorAllowed = 0.5;

        private const short WaitChipTimeOut = 5000;
        private const int ReadChipMaxTries = 10;
        private static short _pushBlockTimeOut;
        private readonly Sensor _presenciaEntradaBanda;
        private readonly LinesSemaphore _semaphore;
        private readonly IItemSolicitor<GrupoPasaportes> _solicitor;
        private readonly SourceGroupSupplier _supplier;

        private GrupoPasaportes _groupReceived;
        private double _groupWeight;
        private double _onReceivedWeight;
        private string _passportRfid;
        private int _readChipTriesWithError;
        private int _readWeightTriesWithError;
        private States _state;

        public RuhlamatManualFeed(ICylinder empujador,
                                  Sensor presenciaEntradaBanda,
                                  Sensor cajonCerrado,
                                  BasculaHbm bascula,
                                  RfidReader rfid,
                                  IItemSolicitor<GrupoPasaportes> solicitor,
                                  LinesSemaphore semaphore,
                                  SourceGroupSupplier supplier)
        {
            _stateAware = new SubsystemStateAware();
            _presenciaEntradaBanda = presenciaEntradaBanda;
            _cajonCerrado = cajonCerrado;
            _empujador = empujador;
            _solicitor = solicitor;
            _semaphore = semaphore;
            _supplier = supplier;

            //Initial state
            _state = States.LLenar;

            //Reset rfid
            _passportRfid = String.Empty;
            _rfid = rfid;
            Bascula = bascula;
            Bascula.WeightObtained += WeightObtainedHandler;

            //Reset bascula
            MaxPassportWeight = 0;
            ResetWeight();
        }

        public double MaxPassportWeight { get; set; }
        public double GroupNominalWeight { get; set; }

        public SubsystemState State
        {
            get { return _stateAware.State; }
        }

        #region IAutomaticRunnable Members

        IEnumerable<Chain> IAutomaticRunnable.GetAutoChains()
        {
            return new[] {new AutoChainEntradaManualRulamat("Entrada PRODEC", this)};
        }

        #endregion

        #region IDiagnosisOwner Members

        public IEnumerable<SecurityDiagnosis> GetSecurityDiagnosis()
        {
            return new[]
                       {
                           new SecurityDiagnosisCondition
                               ("DETECTADO FALLO DEL LECTOR DE CHIPS",
                                "REINICIAR EL PC DE CONTROL Y VOLVER A CARGAR EL CATALOGO",
                                DiagnosisType.Step,
                                Evaluable.FromFunctor(() => !_rfid.Connected()).Value)
                       };
        }

        #endregion

        private void WeightObtainedHandler(double weigth)
        {
            _onReceivedWeight = weigth;
        }

        public void Activate()
        {
            _stateAware.Activate();
        }

        public void Deactivate()
        {
            _stateAware.Deactivate();
        }

        public bool IsEmpty()
        {
            return _groupReceived == null;
        }

        private bool GetActualGroupFromCatalogue()
        {
            _groupReceived = null;
            if (_supplier != null)
            {
                _groupReceived = _supplier.GetItem();
                if (_groupReceived != null)
                {
                    MaxPassportWeight = _groupReceived.TipoPasaporte.Weight;
                    GroupNominalWeight = _groupReceived.PesoNominal();
                }
            }
            return _groupReceived != null;
        }

        private void QuitActualGroupFromCatalogue()
        {
            if (_supplier != null)
                _supplier.QuitItem();
        }

        /// <summary>
        /// Check if weight is in passport nominal weight
        /// </summary>
        private bool CheckWeightInRange()
        {
            double weightMargin = (MaxPassportWeight*MaxErrorAllowed);
            double actualWeightDiff = Math.Abs(_groupWeight - GroupNominalWeight);
            return actualWeightDiff < weightMargin;
        }

        /// <summary>
        /// Check if RFID is in expected range of passport groups
        /// </summary>
        /// <returns></returns>
        private bool CheckRfidInRange()
        {
            if (!String.IsNullOrEmpty(_passportRfid))
            {
                int esperadoMin = Int32.Parse(
                    _groupReceived.Id.Substring(
                        _groupReceived.TipoPasaporte.NChars)) + 1;
                int esperadoMax = (esperadoMin + _groupReceived.NPasaportes());
                int rfidLeido = Int32.Parse(_passportRfid.Substring(_groupReceived.TipoPasaporte.NChars));
                return rfidLeido <= esperadoMax && rfidLeido >= esperadoMin;
            }
            return false;
        }

        /// <summary>
        /// Check if passport rfid readed is in catalogue
        /// </summary>
        /// <returns></returns>
        private bool CompareActualAndReadedPassport()
        {
            if (_supplier != null)
                if (!_supplier.ContainsGroupId(_groupReceived.Id))
                    return false;
            return true;
        }

        private bool DoEmpujadorRest()
        {
            if (!_empujador.InRest)
                _empujador.Rest();
            return _empujador.InRest;
        }

        private bool IsBoxOpen()
        {
            return !_cajonCerrado.Value();
        }

        private bool IsBoxClosed()
        {
            return _cajonCerrado.Value();
        }

        private static bool ShowScreenPlay(int state, TON timer)
        {
            ScreenPlay.DoState(state);
            return timer.TimingWithReset(ShowScreenTime, true);
        }

        private bool ResetWeight()
        {
            bool connected = Bascula.Configurate();
            _readWeightTriesWithError = 0;
            _readChipTriesWithError = 0;
            _groupWeight = 0;
            return connected;
        }

        #region Miembros de IItemSuplier<GrupoPasaportes>

        public GrupoPasaportes QuitItem()
        {
            GrupoPasaportes group = _groupReceived;
            _groupReceived = null;
            return group;
        }

        #endregion

        #region Monitor wrapper

        private static bool IsMonitorConnected
        {
            get { return CheckPoint.IsInstalled(); }
        }

        #endregion

        #region Nested type: AutoChainEntradaManualRulamat

        private class AutoChainEntradaManualRulamat : StructuredChain
        {
            private readonly RuhlamatManualFeed _manualFeed;
            private TON _timer;

            public AutoChainEntradaManualRulamat(string name, RuhlamatManualFeed manualFeed)
                : base(name)
            {
                _manualFeed = manualFeed;
                AddSteps();
            }

            protected override void AddSteps()
            {
                Main_Steps();
                LLenar_Steps(); // Cajon retirado de la posicion de vaciado
                CerrarCajon_Steps(); // Cajon en posicion de pesado y medida
                Check_Steps(); // Comprobacion de pesado y medida: OK - KO
                Empujar_Steps(); // Empuje del grupo en banda a prodec previo al vaciado
                Vaciar_Steps(); // Entrada grupo en Linea 2 regulada por semaforo
                Reproceso_Steps(); // Grupo retirado de la posicion de pesado y lectura de chip
            }

            private void ResetTimers()
            {
                _timer = new TON();
            }

            private void Main_Steps()
            {
                MainChain.Add(new Step("Paso inicial")).Task = NextStep;

                MainChain.Add(new Step("Inicializar monitor externo"))
                    .Task = () =>
                                {
                                    if (!IsMonitorConnected) return;
                                    NextStep();
                                };

                MainChain.Add(new Step("Comprobar accionador de entrada en reposo"))
                    .Task = () =>
                                {
                                    if (!_manualFeed.DoEmpujadorRest()) return;
                                    NextStep();
                                };

                MainChain.Add(new Step("Elegir cadena a ejecutar") {StopChain = true})
                    .Task = () =>
                                {
                                    ResetTimers();
                                    CallChain(_manualFeed._state);
                                };

                MainChain.Add(new Step("Paso final")).Task = PreviousStep;
            }

            private void LLenar_Steps()
            {
                Subchain chain = AddSubchain(States.LLenar);

                chain.Add(new Step("Comprobar accionador de entrada en reposo"))
                    .Task = () =>
                                {
                                    if (!_manualFeed.DoEmpujadorRest()) return;
                                    NextStep();
                                };

                chain.Add(new Step("Esperar cajon abierto en entrada manual") {StopChain = true})
                    .Task = () =>
                                {
                                    if (_manualFeed.IsBoxOpen())
                                        NextStep();
                                    else
                                    {
                                        ScreenPlay.DoState(ScreenPlay.Estado.OpenBox);
                                        PreviousStep();
                                    }
                                };

                chain.Add(new Step("Paso final")).Task = () =>
                                                             {
                                                                 _manualFeed._state = States.Cerrar;
                                                                 Return();
                                                             };
            }

            private void CerrarCajon_Steps()
            {
                Subchain chain = AddSubchain(States.Cerrar);

                chain.Add(new Step("Comprobar accionador de entrada en reposo"))
                    .Task = () =>
                                {
                                    if (!_manualFeed.DoEmpujadorRest()) return;
                                    NextStep();
                                };

                chain.Add(new Step("Cargar siguiente grupo de passaportes desde catalogo"))
                    .Task = () =>
                                {
                                    if (_manualFeed.GetActualGroupFromCatalogue())
                                        NextStep();
                                };

                chain.Add(new Step("Mostramos siguiente ID de grupo a introducir"))
                    .Task = () =>
                                {
                                    ScreenPlay.UpdateIntroGrupo(_manualFeed._groupReceived.IdPlus1);
                                    ScreenPlay.DoState(ScreenPlay.Estado.IntroGrupo);
                                    if (_manualFeed.IsBoxClosed())
                                        NextStep();
                                };

                chain.Add(new Step("Paso final")).Task = () =>
                                                             {
                                                                 _manualFeed._state = States.Check;
                                                                 Return();
                                                             };
            }

            private void Check_Steps()
            {
                Subchain chain = AddSubchain(States.Check);

                #region RFID

                chain.Add(new Step("Bypass lector de chip"))
                    .Task = NextStep;

                chain.Add(new Step("Comprobacion de intentos de lectura de chip").WithTag("CountChipTries"))
                    .Task = () =>
                                {
                                    if (_manualFeed._readChipTriesWithError < ReadChipMaxTries)
                                    {
                                        ScreenPlay.DoState(ScreenPlay.Estado.ComprobarGrupo);
                                        NextStep();
                                    }
                                    else
                                    {
                                        ScreenPlay.DoState(ScreenPlay.Estado.ErrorDeLectura);
                                        if (!_manualFeed.IsBoxOpen()) return;
                                        _manualFeed._state = States.Reproceso;
                                        Return();
                                        return;
                                    }
                                };

                chain.Add(new Step("Conexión con lector RfId en la entrada manual"))
                    .Task = () =>
                                {
                                    if (!_manualFeed.IsBoxClosed())
                                    {
                                        if (!ShowScreenPlay(ScreenPlay.Estado.ErrProceso, _timer)) return;
                                        _manualFeed._state = States.LLenar;
                                        Return();
                                        return;
                                    }

                                    ScreenPlay.DoState(ScreenPlay.Estado.ComprobarGrupo);
                                    if (_manualFeed._rfid.WiseReaderConnect())
                                        NextStep();
                                    else //Error on connecting with RFID device
                                    {
                                        _manualFeed._readChipTriesWithError++;
                                        GoToStep("CountChipTries");
                                    }
                                };

                chain.Add(new Step("Lectura RfId del grupo de pasaportes"))
                    .Task = () =>
                                {
                                    if (_timer.Timing(WaitChipTimeOut))
                                    {
                                        //Error: "No data readed"
                                        NextStep();
                                    }
                                    string code;
                                    if (!_manualFeed._rfid.ReadCode(out code) ||
                                        code == RfidReaderDecipheIt.PassportType.None) return;
                                    _manualFeed._passportRfid = code;
                                    GoToStep("CodigoRfidOk");
                                };

                chain.Add(new Step("Codigo de lectura RfId incorrecto"))
                    .Task = () =>
                                {
                                    ScreenPlay.DoState(ScreenPlay.Estado.CodigoRfidIncorrecto);
                                    if (!_manualFeed.IsBoxOpen()) return;
                                    _manualFeed._state = States.Reproceso;
                                    Return();
                                    return;
                                };

                chain.Add(new Step("Comprobamos si el Rfid leido pertenece al catalogo").WithTag("CodigoRfidOk"))
                    .Task = () =>
                                {
                                    if (!_manualFeed.CheckRfidInRange())
                                    {
                                        ScreenPlay.DoState(ScreenPlay.Estado.CodigoRfidIncorrecto);
                                        if (!_manualFeed.IsBoxOpen()) return;
                                        _manualFeed._state = States.Reproceso;
                                        Return();
                                        return;
                                    }
                                    ScreenPlay.DoState(ScreenPlay.Estado.ComprobarGrupo);
                                    NextStep();
                                };

                #endregion

                #region PESADO

                chain.Add(new Step("Inicializacion de bascula de pesado").WithTag("InitWeight"))
                    .Task = () =>
                                {
                                    if (!_manualFeed.ResetWeight()) return;
                                    NextStep();
                                };

                chain.Add(new Step("Comprobacion de intentos de pesado de grupo").WithTag("CountWeightTries"))
                    .Task = () =>
                                {
                                    if (_manualFeed._readWeightTriesWithError < ReadWeightMaxTries)
                                    {
                                        ScreenPlay.DoState(ScreenPlay.Estado.ComprobarGrupo);
                                        NextStep();
                                    }
                                    else
                                    {
                                        ScreenPlay.DoState(ScreenPlay.Estado.ErrorDePesaje);
                                        if (!_manualFeed.IsBoxOpen()) return;
                                        _manualFeed._state = States.Reproceso;
                                        Return();
                                        return;
                                    }
                                };

                chain.Add(new Step("Pesaje del grupo de pasaportes: Configuracion"))
                    .Task = () =>
                                {
                                    if (!_manualFeed.IsBoxClosed())
                                    {
                                        if (!ShowScreenPlay(ScreenPlay.Estado.ErrProceso, _timer)) return;
                                        _manualFeed._state = States.LLenar;
                                        Return();
                                        return;
                                    }
                                    ScreenPlay.DoState(ScreenPlay.Estado.ComprobarGrupo);
                                    if (_manualFeed.Bascula.Configurate())
                                        NextStep();
                                    else //Error: "Opening serial port to read"
                                    {
                                        _manualFeed._readWeightTriesWithError++;
                                        GoToStep("CountWeightTries");
                                    }
                                };

                chain.Add(new Step("Pesaje del grupo de pasaportes: Inicio de conexion"))
                    .Task = () =>
                                {
                                    if (!_manualFeed.Bascula.BeginRead()) return;
                                    NextStep();
                                };

                chain.Add(new Step("Pesaje del grupo de pasaportes: Lectura de la bascula"))
                    .Task = () =>
                                {
                                    if (_timer.Timing(WaitWeightTimeOut))
                                    {
                                        //Error: "No data received"
                                        _manualFeed._readWeightTriesWithError++;
                                        GoToStep("CountWeightTries");
                                    }
                                    else
                                    {
                                        if (_manualFeed._onReceivedWeight <= 0)
                                        {
                                            //Error: "Cero value weight received"
                                            _manualFeed._readWeightTriesWithError++;
                                            GoToStep("CountWeightTries");
                                        }
                                        else
                                        {
                                            _manualFeed._groupWeight = _manualFeed._onReceivedWeight;
                                            _manualFeed._onReceivedWeight = 0D;
                                            NextStep();
                                        }
                                    }
                                };

                chain.Add(new Step("Comprobacion pesado grupo pasaportes"))
                    .Task = () =>
                                {
                                    if (!_manualFeed.CheckWeightInRange())
                                    {
                                        ScreenPlay.DoState(ScreenPlay.Estado.PesoMedidoIncorrecto);
                                        if (ScreenPlay.IsOk())
                                            NextStep();
                                        else
                                        {
                                            if (!_manualFeed.IsBoxOpen()) return;
                                            _manualFeed._state = States.Reproceso;
                                            Return();
                                            return;
                                        }
                                    }
                                    else
                                        NextStep();
                                };

                #endregion

                chain.Add(new Step("Comprobar si el grupo de pasaportes esta dentro de catalogo").WithTag("FinalCheck"))
                    .Task = () =>
                                {
                                    if (!_manualFeed.CompareActualAndReadedPassport())
                                    {
                                        if (!ShowScreenPlay(ScreenPlay.Estado.SiEntrada, _timer)) return;
                                        _manualFeed._state = States.Reproceso;
                                        Return();
                                        return;
                                    }
                                    ScreenPlay.DoState(ScreenPlay.Estado.GrupoOk);
                                    NextStep();
                                };

                chain.Add(new Step("Paso final"))
                    .Task = () =>
                                {
                                    _manualFeed._state = States.Empujar;
                                    Return();
                                };
            }


            private void Empujar_Steps()
            {
                Subchain chain = AddSubchain(States.Empujar);

                chain.Add(new Step("Comprobar accionador de entrada en reposo"))
                    .Task = () =>
                                {
                                    if (!_manualFeed.IsBoxClosed())
                                    {
                                        if (!ShowScreenPlay(ScreenPlay.Estado.ErrProceso, _timer)) return;
                                        _manualFeed._state = States.LLenar;
                                        Return();
                                        return;
                                    }
                                    ScreenPlay.DoState(ScreenPlay.Estado.GrupoOk);
                                    if (!_manualFeed.DoEmpujadorRest()) return;
                                    NextStep();
                                };

                chain.Add(new Step("Solicitamos permiso de entrada en transportador") {StopChain = true})
                    .Task = () =>
                                {
                                    if (!_manualFeed.IsBoxClosed())
                                    {
                                        if (!ShowScreenPlay(ScreenPlay.Estado.ErrProceso, _timer)) return;
                                        _manualFeed._state = States.LLenar;
                                        Return();
                                        return;
                                    }
                                    ScreenPlay.DoState(ScreenPlay.Estado.GrupoOk);
                                    _manualFeed._semaphore.Request(IDLine.Alemana);
                                    NextStep();
                                };

                chain.Add(new Step("Esperar posición libre en transportador") {StopChain = true})
                    .Task = () =>
                                {
                                    if (!_manualFeed.IsBoxClosed())
                                    {
                                        if (!ShowScreenPlay(ScreenPlay.Estado.ErrProceso, _timer)) return;
                                        _manualFeed._state = States.LLenar;
                                        Return();
                                        return;
                                    }
                                    ScreenPlay.DoState(ScreenPlay.Estado.GrupoOk);
                                    if (_manualFeed._presenciaEntradaBanda.Value()) return;
                                    if (!_manualFeed._solicitor.ReadyToPutElement) return;
                                    NextStep();
                                };

                chain.Add(new Step("Accionar compuerta de entrada manual") {StopChain = true}.WithTag("PushGroup"))
                    .Task = () =>
                                {
                                    if (!_manualFeed.IsBoxClosed())
                                    {
                                        if (!ShowScreenPlay(ScreenPlay.Estado.ErrProceso, _timer)) return;
                                        _manualFeed._state = States.LLenar;
                                        Return();
                                        return;
                                    }
                                    _pushBlockTimeOut = PushBlockTimeOut;
                                    _manualFeed._empujador.Work();
                                    NextStep();
                                };

                chain.Add(new Step("Comprobar atasco"))
                    .Task = () =>
                                {
                                    if (_timer.Timing(_pushBlockTimeOut))
                                    {
                                        _pushBlockTimeOut = 0;
                                        ScreenPlay.DoState(ScreenPlay.Estado.ErrorDeAtasco);
                                        if (!_manualFeed.DoEmpujadorRest()) return;
                                        if (!_manualFeed.IsBoxOpen()) return;
                                        _manualFeed._state = States.Reproceso;
                                        Return();
                                        return;
                                    }
                                    NextStep();
                                };

                chain.Add(new Step("Introducir grupo"))
                    .Task = () =>
                                {
                                    if (!_manualFeed._empujador.InWork)
                                        GoToStep("PushGroup");
                                    else
                                        NextStep();
                                };

                chain.Add(new Step("Paso final")).Task = () =>
                                                             {
                                                                 _manualFeed._state = States.Vaciar;
                                                                 Return();
                                                             };
            }

            private void Vaciar_Steps()
            {
                Subchain chain = AddSubchain(States.Vaciar);

                chain.Add(new Step("Enviar grupo a banda prodec") {StopChain = true})
                    .Task = () =>
                                {
                                    if (!_manualFeed.IsBoxClosed())
                                    {
                                        if (!ShowScreenPlay(ScreenPlay.Estado.ErrProceso, _timer)) return;
                                        _manualFeed._state = States.LLenar;
                                        Return();
                                        return;
                                    }
                                    ScreenPlay.DoState(ScreenPlay.Estado.GrupoOk);
                                    if (!_manualFeed.DoEmpujadorRest()) return;
                                    _manualFeed._semaphore.QuitRequest(IDLine.Alemana);
                                    _manualFeed._solicitor.PutElement(_manualFeed);
                                    _manualFeed.QuitActualGroupFromCatalogue();
                                    NextStep();
                                };

                chain.Add(new Step("Paso final")).Task = () =>
                                                             {
                                                                 _manualFeed._state = States.LLenar;
                                                                 Return();
                                                             };
            }

            private void Reproceso_Steps()
            {
                Subchain chain = AddSubchain(States.Reproceso);

                chain.Add(new Step("Comprobar accionador de entrada en reposo"))
                    .Task = () =>
                                {
                                    if (!_manualFeed.IsBoxClosed())
                                    {
                                        _manualFeed._state = States.LLenar;
                                        Return();
                                        return;
                                    }
                                    if (!_manualFeed.DoEmpujadorRest()) return;
                                    NextStep();
                                };

                chain.Add(new Step("Paso final")).Task = () =>
                                                             {
                                                                 _manualFeed._state = States.LLenar;
                                                                 Return();
                                                             };
            }
        }

        #endregion

        #region Miembros de ISubsystemStateAware

        private readonly SubsystemStateAware _stateAware;

        public ISubsystemStateObserver SetSubsystemStateController(ISubsystemStateController value)
        {
            return ((ISubsystemStateAware) _stateAware).SetSubsystemStateController(value);
        }

        #endregion

        #region Nested type: States

        private enum States
        {
            Cerrar,
            Check,
            LLenar,
            Vaciar,
            Empujar,
            Reproceso
        }

        #endregion

        #region Manuales

        [Manual(SuperGroup = "Encajado", Group = "Entrada PRODEC")] private readonly Sensor _cajonCerrado;

        [Manual(SuperGroup = "Encajado", Group = "Entrada PRODEC")] private readonly ICylinder _empujador;

        [Manual(SuperGroup = "Encajado", Group = "Entrada PRODEC")] private readonly RfidReader _rfid;

        [Manual(SuperGroup = "Encajado", Group = "Entrada PRODEC")]
        public BasculaHbm Bascula { get; private set; }

        #endregion
    }
}