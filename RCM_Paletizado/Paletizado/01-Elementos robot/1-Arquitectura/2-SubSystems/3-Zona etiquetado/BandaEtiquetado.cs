using System;
using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals;
using Idpsa.Control.Sequence;
using Idpsa.Control.Subsystem;
using Idpsa.Paletizado.Definitions;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    public class BandaEtiquetado : ISupplier, IContainer, IAutomaticRunnable, IManualsProvider,
                                   IItemSolicitor<CajaPasaportes>, IRi
    {
        #region States enum

        public enum States
        {
            EsperarCajaEnProdec,
            EsperarCaja,
            LlevarPosicionEtiquetado,
            Etiquetando,
            LlevarPosicionCogida,
            LeerCodigoBarras,
            EsperarCogida
        }

        #endregion

        private const Locations _location = Locations.Entrada;

        [Manual(SuperGroup = "Encajado", Group = "Banda etiquetado", Description = "Lector código barras")] private readonly IReader _barCodeReader;

        private readonly ReprocesadorManual _manualReprocesor;
        private readonly IdpsaSystemPaletizado _sys;

        public Boolean PuentePermisoEntradaCaja;
        private States _state; //Cambiado de private a public para resetearlo desde fuera

        public BandaEtiquetado(PointSpin3D position, IActivable motor, ICylinder empujadorEntrada,
                               IActivable topeEtiquetado,
                               IEvaluable sensorEntrada, IEvaluable sensorEtiquetado, IEvaluable sensorCogidaCaja,
                               IReader barCodeReader, ReprocesadorManual manualReprocesor, IdpsaSystemPaletizado sys)
        {
            Position = position;
            SEntradra = sensorEntrada.WithManualRepresentation("presencia entrada");
            SEtiquetado = sensorEtiquetado.WithManualRepresentation("presencia etiquetado");
            SCogidaCaja = sensorCogidaCaja.WithManualRepresentation("presencia cogida");
            Motor = motor;
            EmpujadorEntrada = empujadorEntrada;
            TopeEtiquetado = topeEtiquetado;
            _barCodeReader = barCodeReader;
            _manualReprocesor = manualReprocesor;
            _sys = sys; //MDG.2011-06-20

            Caja = null;
            _state = States.EsperarCajaEnProdec;
        }


        public PointSpin3D Position { get; private set; }


        [Manual(SuperGroup = "Encajado", Group = "Banda etiquetado")]
        public IEvaluable SEntradra { get; private set; }

        [Manual(SuperGroup = "Encajado", Group = "Banda etiquetado")]
        public IEvaluable SEtiquetado { get; private set; }

        [Manual(SuperGroup = "Encajado", Group = "Banda etiquetado")]
        public IEvaluable SCogidaCaja { get; private set; }

        [Manual(SuperGroup = "Encajado", Group = "Banda etiquetado")]
        public ICylinder EmpujadorEntrada { get; private set; }

        [Manual(SuperGroup = "Encajado", Group = "Banda etiquetado")]
        public IActivable TopeEtiquetado { get; private set; }

        [Manual(SuperGroup = "Encajado", Group = "Banda etiquetado")]
        public IActivable Motor { get; private set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        public Etiquetadora Etiquetadora { get; set; }

        public CajaPasaportes Caja { get; private set; }

        internal States State
        {
            get { return _state; }
            set { _state = value; }
        }


        public CajaPasaportes CajaToCatch
        {
            get { return (Caja != null && Caja.Etiquetada) ? Caja : null; }
        }

        #region IContainer Members

        public IElement ElementQuitted()
        {
            IElement value = CajaToCatch;
            Caja = null;
            return value;
        }

        public void ElementAdded(IElement element)
        {
            var caja = element as CajaPasaportes;
            Caja = caja;
        }

        #endregion

        public void SetNewCatalog()
        {
            Caja = null;
        }

        public bool BeginReadBarCode()
        {
            return _barCodeReader.BeginRead();
        }

        public bool EndReadBarCode()
        {
            if (_barCodeReader.EndRead())
            {
                Caja.CodigoBarrasLeido = _barCodeReader.LastReadedCode;
                //MDG.2011-06-17.Comprobacion etiqueta duplicada
                if (_sys.Production.IsBoxIDInCatalogs(Caja.Id)) //Etiqueta duplicada en palet
                    Caja.EtiquetaDuplicada = true;
                else
                {
                    if (_manualReprocesor.Caja != null)
                    {
                        if (Caja.Id == _manualReprocesor.Caja.Id) //Etiqueta duplicada en mesa de reproceso
                            Caja.EtiquetaDuplicada = true;
                        else
                            Caja.EtiquetaDuplicada = false;
                    }
                    else
                    {
                        Caja.EtiquetaDuplicada = false;
                    }
                }

                return true;
            }
            return false;
        }

        //MDG.2010-12-10.Metodos de salvado y carga de las cajas de pasaportes almacenadas
        public StoredDataBandaEtiquetadoBox GetDataToStore()
        {
            return new StoredDataBandaEtiquetadoBox //
                       {
                           Caja = Caja,
                           //Groups = _grupos.List,//_grupos,//Group = GrupoPasaportes,//_production.GetBoxes().Select(b => b.Id).ToList(),
                           State = _state //,
                           //Name = this.Name//,
                           //Vaciar = this.Vaciar,//MDG.2010-12-09
                           //ModoAcumulacion = this.ModoAcumulacion//MDG.2010-12-09
                       };
        }

        public void SetDataStored(StoredDataBandaEtiquetadoBox GrupoCargado)
        {
            Caja = GrupoCargado.Caja;
            //_grupos.List = GrupoCargado.Groups;
            //MDG.2011-03-01//_state = GrupoCargado.State;
            _state = GrupoCargado.State; //MDG.2011-05-31.Lo borramos con el boton de reset
            //Vaciar = GrupoCargado.Vaciar;//MDG.2010-12-09
            //ModoAcumulacion = GrupoCargado.ModoAcumulacion;//MDG.2010-12-09
        }

        #region Miembros de IRi

        void IRi.Ri()
        {
            Motor.Activate(false);
        }

        #endregion

        #region Miembros de ISupplier

        public ElementTypes? ElementSupplied()
        {
            if (_manualReprocesor.Busy())
                return null;
            if (State != States.EsperarCogida) return null;
            if (CajaToCatch == null) return null;
            if (CajaToCatch.IdLine == IDLine.Japonesa) return ElementTypes.ItemJaponesa;
            if (CajaToCatch.IdLine == IDLine.Alemana) return ElementTypes.ItemAlemana;
            return null;
        }

        public Locations Location
        {
            get { return _location; }
        }

        #endregion

        #region Miembros de IAutomaticRunnable

        IEnumerable<Chain> IAutomaticRunnable.GetAutoChains()
        {
            return new[] {new CadAutoBandaEtiquetado("Banda entrada cajas", this).AddChainControllers(Etiquetadora)};
        }

        #endregion

        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            return (this).GetManualsRepresentations();
        }

        #endregion

        #region Nested type: CadAutoBandaEtiquetado

        private class CadAutoBandaEtiquetado : StructuredChain
        {
            //IDPSASystemPaletizado _sys;//MDG.2011-06-20
            private readonly BandaEtiquetado _banda;

            public CadAutoBandaEtiquetado(string name, BandaEtiquetado banda)
                //public CadAutoBandaEtiquetado(string name, BandaEtiquetado banda, IDPSASystemPaletizado sys)
                : base(name)
            {
                _banda = banda;
                //_sys = sys;
                AddSteps();
            }

            protected override void AddSteps()
            {
                AddMain_Steps();
                EsperarCajaEnProdec_Steps();
                EsperarCaja_Steps();
                LlevarPosicionEtiquetado_Steps();
                LlevarPosicionCogida_Steps();
                LeerCodigoBarras_Steps();
                EsperarCogida_Steps();
            }

            private void AddMain_Steps()
            {
                MainChain.SetSteps(new[]
                                       {
                                           new Step("Llamar subcadena").SetTask(() => CallChain(_banda.State)),
                                           new Step("Paso final").SetTask(() => PreviousStep())
                                       });
            }

            private void EsperarCajaEnProdec_Steps()
            {
                //var boxSupplier = SourceBoxSupplier.Create(IDLine.Japonesa);
                //IEvaluable hayCajaEnProdec = _banda.SEntradra
                //             .DelayToConnection(5000)
                //             .Subscribe(v => { if (v)_banda.PutElement(boxSupplier); });

                IEvaluable hayCajaEnProdec = Evaluable.FromFunctor(() => _banda.State != States.EsperarCajaEnProdec);

                AddSubchain(States.EsperarCajaEnProdec)
                    .SetSteps(new[]
                                  {
                                      new Step("Esperar caja en encajadora lista para enviar").SetTask(() =>
                                                                                                           {
                                                                                                               _banda.
                                                                                                                   EmpujadorEntrada
                                                                                                                   .Rest
                                                                                                                   ();
                                                                                                               if (
                                                                                                                   hayCajaEnProdec
                                                                                                                       .
                                                                                                                       Value
                                                                                                                       ())
                                                                                                               {
                                                                                                                   Return
                                                                                                                       ();
                                                                                                               }
                                                                                                           })
                                  });
            }

            private void EsperarCaja_Steps()
            {
                IEvaluable sEntrada = _banda.SEntradra.DelayToConnection(2000);

                AddSubchain(States.EsperarCaja)
                    .SetSteps(new[]
                                  {
                                      new Step("Paso inicial").SetTask(() =>
                                                                           {
                                                                               sEntrada.Reset();
                                                                               NextStep();
                                                                           }),
                                      new Step("Esperar caja en cinta").SetTask(() =>
                                                                                    {
                                                                                        if (sEntrada.Value())
                                                                                        {
                                                                                            _banda.State =
                                                                                                States.
                                                                                                    LlevarPosicionEtiquetado;
                                                                                            Return();
                                                                                        }
                                                                                    })
                                  });
            }


            private void LlevarPosicionEtiquetado_Steps()
            {
                Subchain chain = AddSubchain(States.LlevarPosicionEtiquetado);
                IEvaluable sEntrada = _banda.SEntradra.DelayToDisconnection(3000);
                IEvaluable sEtiquetado = _banda.SEtiquetado.DelayToConnection(400);

                chain.Add(new Step("Paso inicial")).Task = () =>
                                                               {
                                                                   sEtiquetado.Reset();
                                                                   sEntrada.Reset();
                                                                   _banda.EmpujadorEntrada.Work();
                                                                   _banda.Motor.Activate(true);
                                                                   _banda.TopeEtiquetado.Activate(true);
                                                                   NextStep();
                                                               };

                chain.Add(new Step("Esperar empujado caja entrada cinta")).Task = () =>
                                                                                      {
                                                                                          if (!sEntrada.Value())
                                                                                          {
                                                                                              _banda.EmpujadorEntrada.
                                                                                                  Rest();
                                                                                              NextStep();
                                                                                          }
                                                                                      };

                chain.Add(new Step("Esperar detección caja en zona de etiquetado")).Task = () =>
                                                                                               {
                                                                                                   if (
                                                                                                       sEtiquetado.Value
                                                                                                           ())
                                                                                                   {
                                                                                                       _banda.Motor.
                                                                                                           Activate(
                                                                                                               false);
                                                                                                       _banda.State =
                                                                                                           States.
                                                                                                               Etiquetando;
                                                                                                       Return();
                                                                                                   }
                                                                                               };
            }

            private void LlevarPosicionCogida_Steps()
            {
                Subchain chain = AddSubchain(States.LlevarPosicionCogida);
                IEvaluable sCogida = _banda.SCogidaCaja.DelayToConnection(2000);
                var timer = new TON();

                chain.Add(new Step("Paso inicial")).Task = () =>
                                                               {
                                                                   timer.Reset();
                                                                   sCogida.Reset();
                                                                   _banda.TopeEtiquetado.Activate(false);
                                                                   NextStep();
                                                               };

                chain.Add(new Step("Arrancar motor cinta")).Task = () =>
                                                                       {
                                                                           if (timer.Timing(2000))
                                                                           {
                                                                               _banda.Motor.Activate(true);
                                                                               NextStep();
                                                                           }
                                                                       };

                chain.Add(new Step("Esperar caja en posición de cogida")).Task = () =>
                                                                                     {
                                                                                         if (sCogida.Value())
                                                                                         {
                                                                                             _banda.TopeEtiquetado.
                                                                                                 Activate(true);
                                                                                             _banda.Motor.Activate(false);
                                                                                             _banda.State =
                                                                                                 States.LeerCodigoBarras;
                                                                                             Return();
                                                                                         }
                                                                                     };
            }

            private void LeerCodigoBarras_Steps()
            {
                Subchain chain = AddSubchain(States.LeerCodigoBarras);
                var timer = new TON();

                chain.Add(new Step("Inicio leer codigo barras")).Task = () =>
                                                                            {
                                                                                if (_banda.BeginReadBarCode())
                                                                                {
                                                                                    timer.Reset();
                                                                                    NextStep();
                                                                                }
                                                                            };

                chain.Add(new Step("Esperar lectura código barras")).SetTask(() =>
                                                                                 {
                                                                                     if (_banda.EndReadBarCode())
                                                                                     {
                                                                                         _banda._state =
                                                                                             States.EsperarCogida;
                                                                                         //if(_sys.Production.
                                                                                         Return();
                                                                                     }
                                                                                     else
                                                                                     {
                                                                                         if (timer.Timing(5000))
                                                                                         {
                                                                                             _banda._state =
                                                                                                 States.EsperarCogida;
                                                                                             Return();
                                                                                         }
                                                                                     }
                                                                                 });
            }

            private void EsperarCogida_Steps()
            {
                IEvaluable cajaCogida = _banda.SCogidaCaja
                    .NOT()
                    .AND(Evaluable.FromFunctor(() => _banda.CajaToCatch == null)
                             .DelayToConnection(5000));

                Subchain chain = AddSubchain(States.EsperarCogida);

                chain.Add(new Step("Paso inicial")).Task = () =>
                                                               {
                                                                   cajaCogida.Reset();
                                                                   NextStep();
                                                               };

                chain.Add(new Step("Esperar cogida caja por griper")).Task = () =>
                                                                                 {
                                                                                     if (cajaCogida.Value())
                                                                                     {
                                                                                         _banda.State =
                                                                                             States.EsperarCajaEnProdec;
                                                                                         Return();
                                                                                     }
                                                                                 };
            }
        }

        #endregion

        #region Nested type: CadAutoBandaEtiquetadoSimulated

        private class CadAutoBandaEtiquetadoSimulated : StructuredChain
        {
            private const int _timeCreation = 5000;
            private readonly BandaEtiquetado _banda;
            private readonly IdpsaSystemPaletizado _sys;
            private TON _timerBox;

            public CadAutoBandaEtiquetadoSimulated(string name, IDPSASystem sys)
                : base(name)
            {
                _sys = (IdpsaSystemPaletizado) sys;
                _banda = _sys.Lines.BandaEtiquetado;
                AddSteps();
            }

            protected override void AddSteps()
            {
                AddMain_Steps();
            }

            private void AddMain_Steps()
            {
                int index = 0;

                MainChain.Add(new Step("Paso inicial").WithTag("first")).Task = () =>
                                                                                    {
                                                                                        ResetTimers();
                                                                                        NextStep();
                                                                                    };

                MainChain.Add(new Step("Creacion caja") {StopChain = true}).Task = () =>
                                                                                       {
                                                                                           if (TryCreateNextBox(index))
                                                                                           {
                                                                                               index++;
                                                                                               NextStep();
                                                                                           }
                                                                                       };

                MainChain.Add(new Step("Paso final de cadena")).Task = () => { GoToStep("first"); };
            }

            private bool TryCreateNextBox(int index)
            {
                bool value = false;
                if (_banda.CajaToCatch == null)
                {
                    if (_timerBox.TimingWithReset(_timeCreation, _banda.SCogidaCaja.Value()))
                    {
                        //_banda.ElementAdded(_sys.Production.GetNextBox());
                        value = true;
                    }
                }
                return value;
            }

            private void ResetTimers()
            {
                _timerBox = new TON();
            }
        }

        #endregion

        #region Miembros de IItemSolicitor<GrupoPasaportes>

        public CajaPasaportes PutElement(IItemSuplier<CajaPasaportes> suplier)
        {
            if (State == States.EsperarCajaEnProdec)
            {
                PuentePermisoEntradaCaja = false; //MDG.2011-04-26
                CajaPasaportes item = suplier.QuitItem();
                ElementAdded(item);
                State = States.EsperarCaja;
                return item;
            }
            return null;
        }

        public bool ReadyToPutElement
        {
            get
            {
                return (State == States.EsperarCajaEnProdec || State == States.EsperarCaja) &&
                       (!SEntradra.Value() || PuentePermisoEntradaCaja);
            }
        }

        #endregion
    }
}