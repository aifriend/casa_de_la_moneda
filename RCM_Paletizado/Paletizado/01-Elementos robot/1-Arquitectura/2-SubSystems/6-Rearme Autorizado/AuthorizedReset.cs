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
    public class AuthorizedReset : IFreeRunnable
    {
        private const short WaitChipTimeOut = 5000;
        private const int ReadChipMaxTries = 10;
        private static short _pushBlockTimeOut;

        public string _code;
        private int _readChipTriesWithError;
        private States _state;
        public SensorP btRearme;
        public Output luz;
        public Idpsa.EstadoRearme rearmeState;
        public Output sirena;

        public event CodeChangeEvent CodeChange;
        public delegate void CodeChangeEvent(string rubrica);
        public event CodeChangeEvent stateChange;
        public delegate void RearmandoEvent();
        public event RearmandoEvent rearmeChange;
        public delegate void caidaEvent(Boolean b);
        public event caidaEvent barreraCaidaEvent;
        public delegate void alarmaRFID(Boolean b);
        public event alarmaRFID alarmaRFIDEvent;
        public bool barreraDesarmada;
        public bool maquinaIniciada;
        private Input sysokpos;
        private Input sysokneg;

        public AuthorizedReset(RfidReaderEmployee rfid, SensorP boton, Output luzRearme, Input sysOkPos, Input sysOkNeg, Output siren)
        {
            _code = String.Empty;
            btRearme = boton;
            _rfid = rfid;
            luz = luzRearme;
            sysokpos = sysOkPos;
            sysokneg = sysOkNeg;
            barreraDesarmada = false;
            sirena = siren;

        }

        #region IFreeRunnable Members

        IEnumerable<Chain> IFreeRunnable.GetFreeChains()
        {
            return new[] { new AutoChainEntradaAutorizada("Entrada Autorizada", this) };
        }

        #endregion

        #region IDiagnosisOwner Members

        public void GetAlarma()
        {
            alarmaRFIDEvent(!_rfid.Connected());
        }

        #endregion



        /// <summary>
        /// Check if RFID is in expected range of passport groups
        /// </summary>
        /// <returns></returns>



        #region Nested type: AutoChainEntradaAutorizada

        private class AutoChainEntradaAutorizada : StructuredChain
        {
            private readonly AuthorizedReset resetBarrera;
            private TON _timer;

            public AutoChainEntradaAutorizada(string name, AuthorizedReset rearme)
                : base(name)
            {
                resetBarrera = rearme;
                AddSteps();
            }

            protected override void AddSteps()
            {
                Main_Steps();
                Check_Steps();
                Stopped_Steps();
            }

            private void ResetTimers()
            {
                _timer = new TON();
            }

            private void Main_Steps()
            {
                MainChain.Add(new Step("Paso inicial")).Task = NextStep;

                MainChain.Add(new Step("Elegir cadena a ejecutar") {StopChain = true})
                    .Task = () =>
                                {
                                    resetBarrera.GetAlarma();
                                    resetBarrera.barreraDesarmada = false;
                                    if(!resetBarrera.maquinaIniciada)
                                        PreviousStep();
                                    resetBarrera._state = States.Stopped;
                                    NextStep();
                                };

                MainChain.Add(new Step("Elegir cadena a ejecutar") { StopChain = true })
                    .Task = () =>
                                {
                                    ResetTimers();
                                    CallChain(resetBarrera._state);
                                    NextStep();
                                };

                MainChain.Add(new Step("Paso final")).Task = PreviousStep;
            }

            private void Stopped_Steps()
            {
                Subchain chain = AddSubchain(States.Stopped);

                chain.Add(new Step("Bypass lector de chip"))
                    .Task = NextStep;

                chain.Add(new Step("Conexión con lector RfId en barrera"))
                    .Task = () =>
                                {
                                     resetBarrera.checkEstadoRearme();
                                    //resetBarrera._rfid.Reset();
                                    if (resetBarrera.btRearme.Value())
                                        NextStep();

                                };
                chain.Add(new Step("Conexión con lector RfId en barrera"))
                    .Task = () =>
                    {
                        resetBarrera.rearmeState = EstadoRearme.Entrando;
                        if (_timer.Timing(300))
                            NextStep();
                    };
                chain.Add(new Step("Conexión con lector RfId en barrera"))
                   .Task = () =>
                   {
                       if (resetBarrera.btRearme.Value())
                       {
                           resetBarrera.rearmeState = EstadoRearme.Rearmando;
                           resetBarrera.sirena.Activate(true);
                       }
                       NextStep();
                   };
                chain.Add(new Step("retorno")).Task = () =>
                                                          {
                                                              resetBarrera.sirena.Activate(false);
                                                              if (resetBarrera.luz.Value())
                                                                  resetBarrera.luz.Activate(false);
                                                              resetBarrera._state = States.Check;
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
                                     resetBarrera.checkEstadoRearme();
                                    if (resetBarrera._readChipTriesWithError < ReadChipMaxTries)
                                    {

                                        resetBarrera.stateChangeHandler("Conectando");
                                        NextStep();
                                    }
                                    else
                                    {
                                        resetBarrera.stateChangeHandler("Error de lectura");
                                        //if (!_manualFeed.IsBoxOpen()) return;
                                        resetBarrera._state = States.Stopped;
                                        Return();
                                        return;
                                    }
                                };

                chain.Add(new Step("Conexión con lector RfId en la entrada manual"))
                    .Task = () =>
                                {
                                    resetBarrera.checkEstadoRearme(); 
                                    if (resetBarrera._rfid.WiseReaderConnect())
                                        NextStep();
                                    else //Error on connecting with RFID device
                                    {
                                        resetBarrera._readChipTriesWithError++;
                                        GoToStep("CountChipTries");
                                    }
                                };

                chain.Add(new Step("Lectura RfId del grupo de pasaportes"))
                    .Task = () =>
                                {

                                    resetBarrera.checkEstadoRearme();
                                    if (_timer.Timing(WaitChipTimeOut))
                                    {
                                        resetBarrera.stateChangeHandler("No data readed");
                                        NextStep();
                                    }
                                    string code;
                                    if (!resetBarrera._rfid.ReadCode(out code) ||
                                        code == RfidReaderEmployee.PassportType.None) return;
                                    resetBarrera._code = code;
                                    GoToStep("CodigoRfidOk");
                                };

                chain.Add(new Step("Codigo de lectura RfId incorrecto"))
                    .Task = () =>
                                {

                                    resetBarrera.checkEstadoRearme();

                                    resetBarrera.checkEstadoRearme();
                                    resetBarrera.stateChangeHandler("RFID incorrecto"); 
                                    resetBarrera._state = States.Stopped;
                                    Return();
                                    return;
                                };

                chain.Add(new Step("Comprobamos si el Rfid leido pertenece al catalogo").WithTag("CodigoRfidOk"))
                    .Task = () =>
                                {

                                    resetBarrera.checkEstadoRearme();
                                    if (!resetBarrera.CheckRfidInRange())
                                    {
                                        resetBarrera.stateChangeHandler("RFID incorrecto");
                                        resetBarrera._state = States.Stopped;
                                        Return();
                                        return;
                                    }
                                    resetBarrera.CodeChangeHandler(resetBarrera._code);
                                    resetBarrera.stateChangeHandler("Rúbrica Leida:"+ resetBarrera._code);
                                    resetBarrera.sirena.Activate(true);
                                    NextStep();
                                };

                #endregion

                chain.Add(new Step("Paso final"))
                    .Task = () =>
                                {
                                    resetBarrera.sirena.Activate(false);
                                    resetBarrera.checkEstadoRearme();
                                    resetBarrera._state = States.Stopped;
                                    Return();
                                };
            }
        }


        #endregion

        //#region Miembros de ISubsystemStateAware

        ////private readonly SubsystemStateAware _stateAware;

        ////public ISubsystemStateObserver SetSubsystemStateController(ISubsystemStateController value)
        ////{
        ////    return ((ISubsystemStateAware) _stateAware).SetSubsystemStateController(value);
        ////}

        //#endregion

        #region Nested type: States
        public void checkEstadoRearme()
        {
            GetAlarma();
            if (barreraDesarmada)
            {
                if (luz.Value())
                    rearmeState = EstadoRearme.Rearmado;
                else
                {
                     this.rearmandoEventHandler();
                }

            }
            else
                if (!sysokpos.Value() && !sysokneg.Value()&&maquinaIniciada)
                {
                    barreraCaidaEventHandler((true));
                    luz.Activate(false);
                    barreraDesarmada = true;
                }

        }

        private enum States
        {
            Stopped,
            Check
        }

        #endregion


        private void CodeChangeHandler(string codeCard)
        {
            CodeChange(codeCard);
        }
        private void stateChangeHandler(string chainState)
        {
            stateChange(rearmeState.ToString()+": " +chainState);
        }
        private void rearmandoEventHandler()
        {
            rearmeChange();
        }
        private void barreraCaidaEventHandler(bool b)
        {
            barreraCaidaEvent(b);
        }

        private bool CheckRfidInRange()
        {
            if (!String.IsNullOrEmpty(_code))
            {
                return _code.Length==4;
            }
            return false;
        }
        #region Manuales

        [Manual(SuperGroup = "General", Group = "Rearme Barrera")] 
        public readonly RfidReaderEmployee _rfid;


        #endregion
    }
}