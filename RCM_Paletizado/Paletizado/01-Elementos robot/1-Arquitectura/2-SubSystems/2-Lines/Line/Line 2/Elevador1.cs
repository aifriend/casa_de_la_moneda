using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals;
using Idpsa.Control.Sequence;
using Idpsa.Control.Subsystem;
using Idpsa.Control.Tool;

namespace Idpsa.Paletizado
{
    public class Elevador1 : IAutomaticRunnable2, IItemSuplier<GrupoPasaportes>, IRi, IRi2
    {
        #region Pusher enum

        public enum Pusher
        {
            Extend,
            Retract,
            Any
        }

        #endregion

        #region States enum

        public enum States
        {
            Subir,
            Bajar,
            Llenar,
            LlenarAuto,
            LlenarManual,
            Vaciar
        }

        #endregion

        private const Spin SpinDefault = Spin.S90;
        private static Spin _lastSpinState;
        private static SpinDetectionState _actualSpinDetection;

        [Manual(SuperGroup = "Transporte", Group = "Elevador 1")]
        private readonly ActuatorTwoPositionsMovementMiddleStop _ascensor;

        [Manual(SuperGroup = "Transporte", Group = "Elevador 1")] private readonly ICylinder _centrador;

        [Manual(SuperGroup = "Transporte", Group = "Elevador 1")] private readonly IEvaluable _confirmacionManual;
        [Manual(SuperGroup = "Transporte", Group = "Elevador 1")] private readonly ICylinder _empujador;
        [Manual(SuperGroup = "Transporte", Group = "Elevador 1")] private readonly GyreActuator _girador;
        private readonly IEvaluable _presenciaAbajo;


        [Manual(SuperGroup = "Transporte", Group = "Elevador 1")] private readonly IEvaluable _paradaSeguridadAbajo;
        [Manual(SuperGroup = "Transporte", Group = "Elevador 1")] private readonly IEvaluable _presenciaAbajoDown;
        [Manual(SuperGroup = "Transporte", Group = "Elevador 1")] private readonly IEvaluable _presenciaAbajoUp;

        private readonly IItemSolicitor<GrupoPasaportes> _solicitor;
        private readonly SourceGroupSupplier _supplier;
        private readonly IdpsaSystemPaletizado _sys;
        public States State;
        public bool StateEmpujando;
        private ElevatorState _elevatorState;
        private GrupoPasaportes _passportGroupExpected;
        private GrupoPasaportes _passportGroupReceived;

        public Elevador1(IdpsaSystemPaletizado sys,
                         ActuatorTwoPositionsMovementMiddleStop ascensor,//ActuatorTwoPositionsMovement ascensor,
                         GyreActuator actuadorGiro,
                         ICylinder empujador,
                         ICylinder centrador,
                         Sensor botonConfirmacionManual,
                         Sensor paradaSeguridadAbajo,
                         Sensor presenciaAbajoUp,
                         Sensor presenciaAbajoDown,
                         SourceGroupSupplier supplier,
                         IItemSolicitor<GrupoPasaportes> solicitor)
        {
            _sys = sys;

            _ascensor = ascensor;
            _girador = actuadorGiro;
            _empujador = empujador;
            _centrador = centrador;

            //Stop security on movin donwn
            _paradaSeguridadAbajo = paradaSeguridadAbajo.WithManualRepresentation("deteccion de seguridad en base elevador 1"); 

            //Passport group reception logic at elevator basement
            _presenciaAbajoDown =
                presenciaAbajoDown.WithManualRepresentation("presencia blanco abajo en grupo base elevador1"); 
            _presenciaAbajoUp =
                presenciaAbajoUp.WithManualRepresentation("presencia blanco arriba en grupo base elevador1");
            _presenciaAbajo = _presenciaAbajoUp.OR(_presenciaAbajoDown);
            _confirmacionManual = botonConfirmacionManual;//.Value(); // botonConfirmacionManual.EnableOfClock(0, 200);//(100, 200);
            _confirmacionManual.DelayToDisconnection(2000);
            //Añadimos detector de presencia de grupo de pasaportes abajo, a la entrada del ascensor
            _solicitor = solicitor;
            _supplier = supplier;
            _elevatorState = ElevatorState.None;
            _lastSpinState = SpinDefault;
            State = States.Bajar;
            StateEmpujando = false;
        }

        public bool IsRobotEnlaceInManualMode
        {
            get
            {
                return _sys.Rulhamat.ModoFuncionamiento ==
                       EnlaceRulamat.RobotEnlaceWorkingMode.Manual;
            }
        }

        public bool IsRobotEnlaceInAutomaticMode
        {
            get
            {
                return _sys.Rulhamat.ModoFuncionamiento ==
                       EnlaceRulamat.RobotEnlaceWorkingMode.Automatic;
            }
        }

        public GrupoPasaportes PassportGroup { get; private set; }
        
        private bool IsEmpty()//MDG.2013-03-26
        {
            return (PassportGroup==null);
        }

        public bool ElevatorSecurityDown()
        {
            return _paradaSeguridadAbajo.Value();
        }

        public bool ElevatorDown()
        {
            return _ascensor.IsDown;
        }

        private bool ElevatorUp()
        {
            return _ascensor.IsUp;
        }

        private bool MoveUp()
        {
            return _ascensor.Move1();
        }

        private bool MoveDown()
        {
            return _ascensor.Move2();
        }

        private void StopMove()
        {
            _ascensor.StopMove();
        }

        private bool InSpin(Spin spin)
        {
            bool value = false;
            switch (spin)
            {
                case Spin.S0:
                    value = _girador.InGyre(Spin.S0);
                    break;
                case Spin.S90:
                    value = _girador.InGyre(Spin.S90);
                    break;
                case Spin.S270:
                    value = _girador.InGyre(Spin.S270);
                    break;
                case Spin.Any:
                    value = true;
                    break;
            }
            return value;
        }

        private void SetSpin(Spin spin)
        {
            switch (spin)
            {
                case Spin.S0:
                    _girador.Gyrate(Spin.S0);
                    break;
                case Spin.S90:
                    _girador.Gyrate(Spin.S90);
                    break;
                case Spin.S270:
                    _girador.Gyrate(Spin.S270);
                    break;
            }
        }

        //Para consultar estando en modo acumulacion y abvanzar la banda a la vez que se empuja
        public bool IsPusher(Pusher push)
        {
            bool value = false;
            switch (push)
            {
                case Pusher.Retract:
                    value = _empujador.InRest;
                    break;
                case Pusher.Extend:
                    value = _empujador.InWork;
                    break;
                case Pusher.Any:
                    value = true;
                    break;
            }
            return value;
        }

        private void SetPusher(Pusher push)
        {
            switch (push)
            {
                case Pusher.Retract:
                    _empujador.Rest();
                    StateEmpujando = false;
                    break;
                case Pusher.Extend:
                    _empujador.Work();
                    StateEmpujando = true;
                    break;
            }
        }

        private static string PushToString(Pusher push)
        {
            string value = "comprobar empujador elevador 1";
            switch (push)
            {
                case Pusher.Retract:
                    value += " en reposo";
                    break;
                case Pusher.Extend:
                    value += " en trabajo";
                    break;
                case Pusher.Any:
                    value += " en cualquier posición";
                    break;
            }
            return value;
        }

        private static string SpinToString(Spin spin)
        {
            string value = "comprobar girador elevador 1";
            switch (spin)
            {
                case Spin.S0:
                    value += " en giro de recepción";
                    break;
                case Spin.S90:
                    value += " en giro de empuje -90";
                    break;
                case Spin.S270:
                    value += " en giro de empuje +90";
                    break;
                case Spin.Any:
                    value += " en cualquier posición";
                    break;
            }
            return value;
        }

        private ElevatorState GetRobotEnlaceWorkingMode()
        {
            ElevatorState elevatorState;
            if (IsRobotEnlaceInAutomaticMode)
                elevatorState = ElevatorState.Automatic;
            else if (IsRobotEnlaceInManualMode)
                elevatorState = ElevatorState.Manual;
            else
                elevatorState = ElevatorState.None;
            return elevatorState;
        }

        private bool GetGrupoPasaportesRulamat()
        {
            //Recibo grupo desde la rulamat
            GrupoPasaportes dataReceived = _sys.Rulhamat.DataReceived;
            if (dataReceived != null)
            {
                _passportGroupReceived = dataReceived;
                return true;
            }
            return false;
        }

        private bool PassportSequenceChecker()
        {
            _passportGroupExpected = _supplier.GetItem();
            if (_passportGroupExpected != null)
            {
                if (!string.IsNullOrEmpty(_passportGroupReceived.Id) &&
                    !string.IsNullOrEmpty(_passportGroupExpected.Id))
                {
                    if (_passportGroupReceived.Id == _passportGroupExpected.Id)
                    {
                        PassportGroup = _supplier.QuitItem();
                        _passportGroupExpected = PassportGroup;
                        return true;
                    }
                }
            }
            return false;
        }

        private void CheckSpinDetection()
        {
            if (_presenciaAbajoUp.Value() && !_presenciaAbajoDown.Value())
            {
                _actualSpinDetection = SpinDetectionState.OddUp;
            }
            if (!_presenciaAbajoUp.Value() && _presenciaAbajoDown.Value())
            {
                _actualSpinDetection = SpinDetectionState.OddDown;
            }
            if (_presenciaAbajoUp.Value() && _presenciaAbajoDown.Value())
            {
                _actualSpinDetection = SpinDetectionState.Even;
            }
            if (_presenciaAbajoUp.Value() && _presenciaAbajoDown.Value())
            {
                _actualSpinDetection = SpinDetectionState.None;
            }

            if (_actualSpinDetection == SpinDetectionState.OddUp || _actualSpinDetection == SpinDetectionState.OddDown)
            {
                if (IsRobotEnlaceInAutomaticMode)
                {
                    switch (_actualSpinDetection)
                    {
                        case SpinDetectionState.OddUp:
                            _lastSpinState = Spin.S270; //izquierda
                            break;
                        case SpinDetectionState.OddDown:
                            _lastSpinState = Spin.S90; //derecha
                            break;
                    }
                }
                else if (IsRobotEnlaceInManualMode)
                {
                    _lastSpinState = SpinDefault; //izquierda
                }
            }
            else
            {
                switch (_lastSpinState)
                {
                    case Spin.S90:
                        _lastSpinState = Spin.S270;
                        break;
                    case Spin.S270:
                        _lastSpinState = Spin.S90;
                        break;
                    default:
                        _lastSpinState = Spin.S90;
                        break;
                }
            }
        }

        //Metodos de salvado y carga
        public StoredDataElevator1Group GetDataToStore()
        {
            return new StoredDataElevator1Group //
                       {
                           Group = PassportGroup,
                           //_production.GetBoxes().Select(b => b.Id).ToList(),
                           State = State
                       };
        }

        public void SetDataStored(StoredDataElevator1Group grupoCargado)
        {
            PassportGroup = grupoCargado.Group;
            State = grupoCargado.State;
        }

        #region Miembros de IItemSuplier<GrupoPasaportes>

        public GrupoPasaportes QuitItem()
        {
            GrupoPasaportes value = PassportGroup;
            PassportGroup = null;
            return value;
        }

        #endregion

        #region Miembros de IRi

        public void Ri()
        {
            ;// _ascensor.Deactivate();
        }

        public void Ri2()
        {
            _ascensor.Deactivate();
        }

        #endregion

        #region Nested type: AutoChainElevador1

        private class AutoChainElevador1 : StructuredChain
        {
            private readonly Elevador1 _elevador;
            private TON _timer;

            public AutoChainElevador1(string name, Elevador1 elevador)
                : base(name)
            {
                _elevador = elevador;
                AddSteps();
            }

            protected override void AddSteps()
            {
                MainSteps();
                SubirSteps();
                BajarSteps();
                VaciarSteps();
                LLenar();
                LLenarAutoSteps();
                LLenarManualSteps();
                SetCheckSteps();
            }

            private void ResetTimers()
            {
                _timer = new TON();
            }

            private void MainSteps()
            {
                MainChain.Add(new Step("Paso inicial")).Task = NextStep;

                MainChain.Add(new Step("Elegir cadena a ejecutar") {StopChain = true})
                    .Task = () =>
                                {
                                    ResetTimers();
                                    CallChain(_elevador.State);
                                };

                MainChain.Add(new Step("Paso final")).Task = PreviousStep;
            }

            private DynamicStepBody SetCheckStep(Pusher push, Spin spin)
            {
                return new DynamicStepBody(() => "Poner empujador y girador elevador 1 en posición",
                                           () => SetCheckChain(push, spin)) {AdditionalAction = false};
            }

            private void SubirSteps()
            {
                Subchain chain = AddSubchain(States.Subir);

                chain.Add(new Step()).SetDynamicBehaviour(() => SetCheckStep(Pusher.Retract, Spin.S0));

                chain.Add(new Step("Comprobar grupo de subida en elevador 1")).Task = 
                    () =>
                    {
                        _elevador._sys.Rulhamat.GroupAllowed();//MDG.2013-01-17.We continue telling that the group is received ok
                        if (_elevador.ElevatorDown() && !_elevador._presenciaAbajo.Value()) return;
                        NextStep();
                    };

                chain.Add(new Step("Subir elevador 1")).Task = 
                    () =>
                    {
                        _elevador._sys.Rulhamat.GroupAllowed();//MDG.2013-01-17.We continue telling that the group is received ok
                        if (_elevador.ElevatorUp())
                        {
                            _elevador.StopMove();
                            NextStep();
                        }
                        else
                        {
                            if (!_elevador._sys.Rulhamat.ElevatorAllowedMoveUp)
                            {
                                _elevador.StopMove();
                                return;
                            }

                            //Verificacion de que hay nada debajo antes de subir
                            if (!_elevador.ElevatorUp())
                            {
                                //Reduccion Tiempos cadena Ascensor1
                                if (_timer.TimingWithReset(200, _elevador.MoveUp()))
                                    NextStep();
                            }
                            else
                            {
                                if (_timer.TimingWithReset(200, !_elevador._presenciaAbajo.Value()))
                                    _elevador.StopMove();
                                //Parar ascensor ya que no hay ningun grupo que subir
                            }
                        }
                    };

                chain.Add(new Step("Paso final"))
                    .Task = () =>
                                {
                                    _elevador.State = States.Vaciar;
                                    Return();
                                };
            }

            private void BajarSteps()
            {
                Subchain chain = AddSubchain(States.Bajar);

                chain.Add(new Step()).SetDynamicBehaviour(() => SetCheckStep(Pusher.Retract, Spin.S0));

                MainChain.Add(new Step("Comprobamos retirada del centrador de grupos en elevador 1")).Task = 
                    () =>
                    {
                        _elevador._sys.Rulhamat.GroupReset();//MDG.2013-01-17.We reset the comunication that the group is received ok
                        //Comprobamos permiso para bajada del centrador
                        if (!_elevador._sys.Rulhamat.ElevatorAllowed) return;

                        if (!_elevador._centrador.InRest)
                        {
                            _elevador._centrador.Rest();

                            //Reduccion tiempos cadena Ascensor1
                            if (_timer.TimingWithReset(200, !_elevador._centrador.InRest))
                                NextStep();
                        }
                        else
                            NextStep();
                    };

                chain.Add(new Step("Bajamos de forma segura el elevador 1")).Task = 
                    () =>
                    {
                        if (_elevador.ElevatorDown())
                        {
                            _elevador.StopMove();
                            GoToStep("Final");
                        }
                        else
                        {
                            if (!_elevador._sys.Rulhamat.ElevatorAllowedMoveUp)
                            {
                                _elevador.StopMove();
                                return;
                            }

                            //Verificacion de que no hay nada debajo antes de bajar
                            if (_elevador._presenciaAbajo.Value() == false)
                            {
                                //Reduccion Tiempos cadena Ascensor1
                                if (_timer.TimingWithReset(200, _elevador.MoveDown()))
                                {
                                    GoToStep("Final");
                                }
                                //else if (_elevador.ElevatorSecurityDown())
                                //{
                                //    _elevador.StopMove();
                                //    GoToStep("SecurityStop");
                                //}
                            }
                            else
                            {
                                if (_timer.TimingWithReset(200, _elevador._presenciaAbajo.Value()))
                                    _elevador.StopMove();
                                //Parar ascensor
                            }
                        }
                    };

                //chain.Add(new Step("PARADA 2: Bajamos de forma segura el elevador 1").WithTag("SecurityStop"))
                //    .Task = () =>
                //                {
                //                    if (_elevador.ElevatorDown())
                //                    {
                //                        _elevador.StopMove();
                //                        GoToStep("Final");
                //                    }
                //                    else
                //                    {
                //                        if (!_elevador._sys.Rulhamat.ElevatorAllowedMoveUp)
                //                        {
                //                            _elevador.StopMove();
                //                            return;
                //                        }

                //                        //Verificacion de que no hay nada debajo antes de bajar
                //                        if (_elevador._presenciaAbajo.Value() == false)
                //                        {
                //                            //Reduccion Tiempos cadena Ascensor1
                //                            var downDualDetection = _elevador.MoveDown() &&
                //                                                    _elevador.ElevatorSecurityDown();
                //                            if (_timer.TimingWithReset(200, downDualDetection))
                //                                GoToStep("Final");
                //                        }
                //                        else
                //                        {
                //                            if (_timer.TimingWithReset(200, _elevador._presenciaAbajo.Value()))
                //                                _elevador.StopMove();
                //                            //Parar ascensor
                //                        }
                //                    }
                //                };

                chain.Add(new Step("Paso final").WithTag("Final"))
                    .Task = () =>
                                {
                                    _elevador.State = States.Llenar;
                                    Return();
                                };
            }

            private void LLenar()
            {
                Subchain chain = AddSubchain(States.Llenar);

                chain.Add(new Step("Comprobar modo de funcionamiento") {StopChain = true}).Task = 
                    () =>
                    {
                        _elevador._elevatorState = _elevador.GetRobotEnlaceWorkingMode();
                        switch (_elevador._elevatorState)
                        {
                            case ElevatorState.Automatic:
                                _elevador._centrador.Rest();
                                _elevador.State = States.LlenarAuto;
                                Return();
                                break;
                            case ElevatorState.Manual:
                                _elevador._centrador.Work();
                                _elevador.State = States.LlenarManual;
                                Return();
                                break;
                        }
                    };
            }

            private void LLenarAutoSteps()
            {
                Subchain chain = AddSubchain(States.LlenarAuto);

                chain.Add(new Step()).SetDynamicBehaviour(() => SetCheckStep(Pusher.Retract, Spin.S0));

                chain.Add(new Step("Comprobar modo de funcionamiento") {StopChain = true}).Task = 
                    () =>
                    {
                        _elevador._sys.Rulhamat.GroupReset();
                        NextStep();
                    };

                chain.Add(new Step("Comprobar permiso de acceso al elevador 1") {StopChain = true}).Task =
                    () =>
                    {
                        _elevador._elevatorState = _elevador.GetRobotEnlaceWorkingMode();
                        if (_elevador._elevatorState == ElevatorState.Manual)
                        {
                            _elevador.State = States.Llenar;
                            Return();
                            return;
                        }
                        if (!_elevador._sys.Rulhamat.IsReadyToReceivePassport()) return;
                        NextStep();
                    };

                chain.Add(new Step("Esperando envio de grupo de pasaporte desde el robot de enlace") {StopChain = true}).Task = 
                    () =>
                    {
                        _elevador._elevatorState = _elevador.GetRobotEnlaceWorkingMode();
                        if (_elevador._elevatorState == ElevatorState.Manual)
                        {
                            _elevador.State = States.Llenar;
                            Return();
                            return;
                        }
                        if (_elevador._sys.Rulhamat.GroupHasError)
                        {
                            Return();
                            return;
                        }
                        _elevador._sys.Rulhamat.GroupReset();
                        if (!_elevador._sys.Rulhamat.StartSendingPassportGroup) return;
                        NextStep();
                    };

                chain.Add(new Step("Esperando recepcion de grupo de pasaporte desde el robot de enlace")
                  {StopChain = true}).Task = 
                  () =>
                    {
                        _elevador._elevatorState = _elevador.GetRobotEnlaceWorkingMode();
                        //Salimos de la subcadena de carga automatica si el modo de funcionamiento ha cambiado a carga manual
                        if (_elevador._elevatorState == ElevatorState.Manual)
                        {
                            _elevador.State = States.Llenar;
                            Return();
                            return;
                        }
                        if (!_elevador.GetGrupoPasaportesRulamat()) return;
                        NextStep();
                    };

                chain.Add(new Step("Comprobar secuencia de entrada del grupo de pasaportes")).Task = 
                    () =>
                    {
                        if (!_elevador.PassportSequenceChecker())
                        {
                            _elevador._sys.Rulhamat.GroupNotAllowed();
                            //MDG.2013-01-17.No mostramos incidencias en este programa. Las mostramos en el robot de enlace
                            //SetStepDiagnosis("Introduzca el grupo de pasaportes correspondiente");
                            Return();
                            return;
                        }
                        _elevador._sys.Rulhamat.GroupAllowed();
                        NextStep();
                    };

                chain.Add(new Step("Solo permitimos la subida del elevador 1 si robot de enlace da permiso")).Task = 
                    () =>
                    {
                        _elevador._sys.Rulhamat.GroupAllowed();//MDG.2013-01-17.We continue telling that the group is received ok
                        _elevador._elevatorState = _elevador.GetRobotEnlaceWorkingMode();
                        //Salimos de la subcadena de carga automatica si el modo de funcionamiento ha cambiado a carga manual
                        if (_elevador._elevatorState == ElevatorState.Manual)
                        {
                            _elevador.State = States.Llenar;
                            Return();
                            return;
                        }
                        if (!_elevador._sys.Rulhamat.ElevatorAllowedMoveUp) return;
                        //_elevador._sys.Rulhamat.GroupReset();//MDG.2013-01-17. We don't reset the communication still because maybe Robot Enlace didn't notice it already
                        _elevador.CheckSpinDetection();
                        _elevador.State = States.Subir;
                        Return();
                    };
            }

            private void LLenarManualSteps()
            {
                Subchain chain = AddSubchain(States.LlenarManual);

                chain.Add(new Step()).SetDynamicBehaviour(() => SetCheckStep(Pusher.Retract, Spin.S0));

                chain.Add(new Step("Comprobar modo de funcionamiento") {StopChain = true}).Task = 
                    () =>
                    {
                        _elevador._sys.Rulhamat.GroupReset();
                        NextStep();
                    };

                chain.Add(new Step("Comprobamos accionamiento del centrador de grupos en elevador 1")).Task =
                    () =>
                        {
                            _elevador._elevatorState = _elevador.GetRobotEnlaceWorkingMode();
                            if (_elevador._elevatorState == ElevatorState.Automatic)
                            {
                                _elevador.State = States.Llenar;
                                Return();
                                return;
                            }

                            //Comprobamos permiso para bajada del centrador
                            if (!_elevador._sys.Rulhamat.ElevatorAllowed) return;

                            _elevador._centrador.Work();

                            //Reduccion tiempos cadena Ascensor1
                            if (_timer.TimingWithReset(200, !_elevador._centrador.InWork))
                                NextStep();
                        };

                chain.Add(new Step("Comprobar permiso de acceso al elevador 1") {StopChain = true}).Task =
                    () =>
                        {
                            _elevador._elevatorState = _elevador.GetRobotEnlaceWorkingMode();
                            if (_elevador._elevatorState == ElevatorState.Automatic)
                            {
                                _elevador.State = States.Llenar;
                                Return();
                                return;
                            }
                            if (!_elevador._sys.Rulhamat.IsReadyToReceivePassport()) return;
                            NextStep();
                        };

                chain.Add(new Step("Esperar boton de confirmacion con grupo en elevador 1") {StopChain = true}).Task =
                    () =>
                        {
                            _elevador._elevatorState = _elevador.GetRobotEnlaceWorkingMode();
                            if (_elevador._elevatorState == ElevatorState.Automatic)
                            {
                                _elevador.State = States.Llenar;
                                Return();
                                return;
                            }
                            if (!_elevador._confirmacionManual.Value() || !_elevador._presenciaAbajo.Value()) return;
                            _elevador.CheckSpinDetection();
                            NextStep();
                        };

                chain.Add(new Step("Retiramos centrador de grupos en elevador 1")).Task =
                    () =>
                        {
                            //Extendemos centrador
                            _elevador._centrador.Rest();

                            //Reduccion tiempos cadena Ascensor1
                            if (_timer.TimingWithReset(200, _elevador._centrador.InWork))
                                NextStep();
                        };

                chain.Add(new Step("Esperando envio de grupo de pasaporte desde el robot de enlace") {StopChain = true}).Task = 
                    () =>
                    {
                        _elevador._elevatorState = _elevador.GetRobotEnlaceWorkingMode();
                        if (_elevador._elevatorState == ElevatorState.Automatic)
                        {
                            _elevador.State = States.Llenar;
                            Return();
                            return;
                        }
                        if (_elevador._sys.Rulhamat.GroupHasError)
                        {
                            Return();
                            return;
                        }
                        _elevador._sys.Rulhamat.GroupReset();
                        if (!_elevador._sys.Rulhamat.StartSendingPassportGroup) return;
                        NextStep();
                    };

                chain.Add(new Step("Esperando recepcion de grupo de pasaporte desde el robot de enlace"){StopChain = true}).Task = 
                    () =>
                                {
                                    _elevador._elevatorState = _elevador.GetRobotEnlaceWorkingMode();
                                    if (_elevador._elevatorState == ElevatorState.Automatic)
                                    {
                                        _elevador.State = States.Llenar;
                                        Return();
                                        return;
                                    }
                                    if (!_elevador.GetGrupoPasaportesRulamat()) return;
                                    NextStep();
                                };

                chain.Add(new Step("Comprobar secuencia de entrada del grupo de pasaportes"))
                    .Task = () =>
                                {
                                    if (!_elevador.PassportSequenceChecker())
                                    {
                                        _elevador._sys.Rulhamat.GroupNotAllowed();
                                        //MDG.2013-01-17.No mostramos incidencias en este programa. Las mostramos en el robot de enlace//
                                        //SetStepDiagnosis("Introduzca el grupo de pasaportes correspondiente");
                                        Return();
                                        return;
                                    }
                                    _elevador._sys.Rulhamat.GroupAllowed();
                                    NextStep();
                                };

                chain.Add(new Step("Solo permitimos la subida del elevador 1 si robot de enlace da permiso"))
                    .Task = () =>
                                {
                                    _elevador._elevatorState = _elevador.GetRobotEnlaceWorkingMode();
                                    if (_elevador._elevatorState == ElevatorState.Automatic)
                                    {
                                        _elevador.State = States.Llenar;
                                        Return();
                                    }
                                    if (!_elevador._sys.Rulhamat.ElevatorAllowedMoveUp) return;
                                    _elevador._sys.Rulhamat.GroupReset();
                                    _elevador.State = States.Subir;
                                    Return();
                                };
            }

            private void VaciarSteps()
            {
                Subchain chain = AddSubchain(States.Vaciar);

                chain.Add(new Step()).SetDynamicBehaviour(() => SetCheckStep(Pusher.Retract, _lastSpinState));

                chain.Add(new Step("Esperar posición libre en transportador") {StopChain = true})
                    .Task = () =>
                                {
                                    if (_elevador._solicitor.ReadyToPutElement)
                                        NextStep();
                                };

                chain.Add(new Step()).SetDynamicBehaviour(() => SetCheckStep(Pusher.Extend, default(Spin)));

                chain.Add(new Step("Paso final")).Task = () =>
                                                             {
                                                                 _elevador.MoveDown();
                                                                 _elevador._solicitor.PutElement(_elevador);
                                                                 _elevador.State = States.Bajar;
                                                                 Return();
                                                             };
            }

            private void SetCheckChain(Pusher push, Spin spin)
            {
                CallChain(AuxChains.SetCheck)
                    .WithParam("pusher", push)
                    .WithParam("spin", spin);
            }

            private void SetCheckSteps()
            {
                Subchain chain = AddSubchain(AuxChains.SetCheck);
                chain.AddParams("pusher", "spin");
                Spin spin = default(Spin);
                Pusher push = default(Pusher);
                TON timer = null;

                chain.Add(new Step("Paso inicial"))
                    .Task = () =>
                                {
                                    push = chain.Param<Pusher>("pusher");
                                    spin = chain.Param<Spin>("spin");
                                    timer = new TON();
                                    NextStep();
                                };

                chain.Add(new Step(() => PushToString(push), 4000))
                    .Task = () =>
                                {
                                    _elevador.SetPusher(push);
                                    if (timer.TimingWithReset(10, _elevador.IsPusher(push)))
                                        NextStep();
                                };

                chain.Add(new Step(() => SpinToString(spin), 4000))
                    .Task = () =>
                                {
                                    _elevador.SetSpin(spin);
                                    if (timer.TimingWithReset(200, _elevador.InSpin(spin)))
                                        Return();
                                };

                chain.Add(new Step(("Retraemos el centrador a la posicion de seguridad"), 2000))
                    .Task = () =>
                                {
                                    _elevador._centrador.Rest();
                                    if (timer.TimingWithReset(10, _elevador._centrador.InWork))
                                        NextStep();
                                };
            }
        }

        #endregion

        #region Miembros de IAutomaticRunnable2

        IEnumerable<Chain> IAutomaticRunnable2.GetAutoChains2()
        {
            return new StructuredChain[]
                       {
                           new AutoChainElevador1("Elevador 1", this)
                       };
        }

        #endregion

        #region Nested type: AuxChains

        private enum AuxChains
        {
            SetCheck
        }

        #endregion

        #region Nested type: ElevatorState

        private enum ElevatorState
        {
            None,
            Automatic,
            Manual
        }

        #endregion

        #region Nested type: SpinDetectionState

        private enum SpinDetectionState
        {
            None,
            OddUp,
            OddDown,
            Even,
        }

        #endregion

    }
}