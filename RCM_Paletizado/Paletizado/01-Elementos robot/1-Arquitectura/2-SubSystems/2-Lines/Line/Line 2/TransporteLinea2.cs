using System;//MDG.2013-03-26
using System.Collections.Generic;//MDG.2013-02-06
using Idpsa.Control;//MDG.2013-02-06
using Idpsa.Control.Component;
using Idpsa.Control.Subsystem;
using Idpsa.Control.Tool;
using Idpsa.Control.Diagnosis;//MDG.2013-02-06

namespace Idpsa.Paletizado
{
    //public class TransporteLinea2
    public class TransporteLinea2 : IDiagnosisOwner/*MDG.2013-02-06*/ , IManagerRunnable

    {
        private readonly Bus _bus;
        private readonly IdpsaSystemPaletizado _sys;

        public TransporteLinea2(IdpsaSystemPaletizado sys,
                                SourceGroupSupplier supplier,
                                IItemSolicitor<GrupoPasaportes> solicitor,
                                LinesSemaphore semaphore)
        {
            _sys = sys;
            _bus = sys.Bus;
            InitializeControlAire();//MDG.2012-07-23
            InitializeElevador2(solicitor, semaphore);
            InitializeTramo2();
            InitializeTramo1();
            InitializeElevador1(supplier);
            //Inicializamos otra vez tramo 1 para que pueda coger la suscripcion del elevador 1
            SubscribeTramo1EnElevador1();
        }

        [Subsystem(Filter = SubsystemFilter.None)]
        public Elevador1 Elevador1 { get; private set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        public TramoTransporteGruposPasaportes Tramo1 { get; set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        public TramoTransporteGruposPasaportes Tramo2 { get; set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        public Elevador2 Elevador2 { get; private set; }
        
        [Subsystem(Filter = SubsystemFilter.None)]
        public ControlAireTransportes controlAireTransportes { get; set; }//MDG.2012-07-23
        

        private void InitializeElevador1(SourceGroupSupplier supplier)
        {
            var actuador =
                new ActuatorWithInversorSimple
                    (
                    _bus.Out("K741A"),//MDG.2012-07-19//_bus.Out("Output52.2"), //subir//MDG.2012-04-12//_bus.Out("Output51.2"),//subir
                    _bus.Out("K741B")//MDG.2012-07-19//_bus.Out("Output52.3") //MDG.2012-04-12//bajar//_bus.Out("Output51.3")//bajar
                    );

            Cylinder centrador =
                new Cylinder2Efect1Sensor
                    (
                    _bus.In("Input5.3"),
                    _bus.Out("Output4.1"),//MDG.2012-07-19//_bus.Out("Output2.7"),
                    _bus.Out("Output4.2"),//MDG.2012-07-19//_bus.Out("Output2.6"),
                    "Centrador grupo pasaportes"
                    )
                    .WithRestName("Recoger")
                    .WithWorkName("Extender");

            var twoPositionsActuator = (ActuatorTwoPositionsMovementMiddleStop)
                                       new ActuatorTwoPositionsMovementMiddleStop
                                           (
                                           "Motor",
                                           actuador,
                                           new SensorP(_bus.In("Input1.3"), "Posición superior"),
                                           new SensorP(_bus.In("A901"), "Posición proxima superior"),
                                           new SensorP(_bus.In("Input1.1"), "Posición inferior 1"),
                                           new SensorP(_bus.In("Input5.7"), "Posición inferior 2"),//MDG.2012-10-30//new SensorSimulated("Posición próxima inferior", false)
                                           new ActuatorP(_bus.Out("K743"), "Velocidad alta")
                                           )
                                           .WithJogPosName("Subir")
                                           .WithJogNegName("Bajar")
                                           .WithEnableJogNeg(() => (!_bus.In("Input1.1").Value() && centrador.InWork))
                                           .WithEnableJogPos(() => (!_bus.In("Input1.3").Value() && centrador.InWork)); 

            var actuadorGiro =
                new GyreActuator
                    (
                    _bus.In("Input4.7"), //Posicion 0
                    _bus.In("Input1.4"), //Posicion +90
                    _bus.In("Input1.0"), //Posicion -90
                    new ActuatorWithInversorSimple(_bus.Out("Output3.5"), _bus.Out("Output3.5")), //Posicion 0
                    new ActuatorWithInversorSimple(_bus.Out("Output3.3"), _bus.Out("Output3.6")), //Posicion +90 -90
                    "Giro pinza"
                    );

            Cylinder empujador =
                new Cylinder1Efect
                    (
                    _bus.In("Input1.7"),
                    _bus.In("Input1.5"),
                    _bus.Out("Output3.1"),
                    "Empujador grupo pasaportes"
                    )
                    .WithRestName("Recoger")
                    .WithWorkName("Empujar")
                    .WithManualWorkEnable(() => actuadorGiro.InGyre(Spin.S270) || actuadorGiro.InGyre(Spin.S90));

            Sensor paradaSeguridadAbajo = new SensorP(_bus.In("Input5.7"), "Posición inferior 2");
            Sensor botonConfirmacion = new SensorP(_bus.In("Input1.6"), "Botón confirmacion grupo pasaportes");
            //botonConfirmacion.DelayToDisconnection(2000);
            Sensor presenciaAbajoUp = new SensorP(_bus.In("Input5.4"),
                                                  "Presencia color arriba grupo pasaportes en entrada Ascensor 1");
            Sensor presenciaAbajo = new SensorP(_bus.In("Input5.1"),
                                                "Presencia color abajo grupo pasaportes en entrada Ascensor 1");

            IItemSolicitor<GrupoPasaportes> solicitor = Tramo1;

            //Añadimos detector de presencia de grupo de pasaportes abajo, a la entraa del ascensor
            Elevador1 = new Elevador1(_sys,
                                      twoPositionsActuator, //Actuador
                                      actuadorGiro, //Actuador de giro
                                      empujador, //Empujador
                                      centrador, //Centrador
                                      botonConfirmacion, //Confirmacion
                                      paradaSeguridadAbajo, presenciaAbajoUp, presenciaAbajo, //Sensores
                                      supplier, solicitor //Supplier, Solicitor
                );
        }

        private void InitializeTramo1()
        {
            Actuator motor =
                new ActuatorP
                    (
                    //_bus.Out("Output52.4"), //MDG.2012-04-12//_bus.Out("Output51.4"),
                    _bus.Out("K742"),//MDG.2012-07-19
                    "Motor tramo transporte 1"
                    );

            ICylinder empujador =
                new Cylinder1Efect
                    (
                    _bus.In("Input5.2"),
                    _bus.In("Input5.0"),
                    _bus.Out("Output3.4"),
                    "Empujador tramo 1"
                    )
                    .WithRestName("Recoger")
                    .WithWorkName("Empujar");

            //IItemSuplier<GrupoPasaportes> supplier = Elevador1;//MDG.2011-03-01
            IItemSolicitor<GrupoPasaportes> solicitor = Tramo2;

            Tramo1 =
                new TramoTransporteGruposPasaportes
                    //MDG.2011-03-01. Nueva llamada, diferente a la del tramo 2, que incluye suscripcion al elevador 1
                    (
                    _sys,
                    "Tramo 1",
                    20,
                    //MDG.2010-11-29.En el primer tramo entran 20 grupos en modo normal y 100 en modo acumulacion//2,//20,
                    motor,
                    new SensorP(_bus.In("Input1.2"), "Presencia entrada tramo transporte 1"),
                    new SensorP(_bus.In("Input5.6"), "Presencia salida tramo transporte 1"),
                    empujador,
                    solicitor,
                    Elevador1 //MDG.2011-03-01.suscripcion al elevador 1
                    );
        }

        private void SubscribeTramo1EnElevador1()
        {
            Tramo1.SubscribeElevador1(Elevador1);
        }

        private void InitializeTramo2()
        {
            Actuator motor = new ActuatorP
                (
                _bus.Out("K733"),//MDG.2012-07-19//_bus.Out("Output51.0"), //MDG.2012-04-12//_bus.Out("Output50.0"),
                "Motor tramo transporte 2"
                );

            ICylinder empujador =
                new Cylinder1Efect
                    (
                    _bus.In("Input0.6"),
                    _bus.In("Input0.2"),
                    _bus.Out("Output3.2"),
                    "Empujador tramo 2"
                    )
                    .WithRestName("Recoger")
                    .WithWorkName("Empujar");

            IItemSolicitor<GrupoPasaportes> solicitor = Elevador2;

            Tramo2 =
                new TramoTransporteGruposPasaportes
                    (
                    _sys,
                    "Tramo 2",
                    5,
                    //MDG.2010-11-29.En el segundo tramo entran 5 grupos en modo normal y 20 en modo acumulacion//2,//5,
                    motor,
                    new SensorSimulated("Presencia entrada tramo transporte 2", false),
                    new SensorP(_bus.In("Input0.5"), "Presencia salida tramo transporte 2"),
                    empujador,
                    solicitor
                    );
        }

        private void InitializeElevador2(IItemSolicitor<GrupoPasaportes> solicitor, LinesSemaphore semaphore)
        {
            var actuador =
                new ActuatorWithInversorSimple
                    (
                    _bus.Out("K740B"),//MDG.2012-07-19//_bus.Out("Output52.1"), //MDG.2012-04-12//_bus.Out("Output51.1"),
                    _bus.Out("K740A")//MDG.2012-07-19//_bus.Out("Output52.0") //MDG.2012-04-12//_bus.Out("Output51.0")
                    );

            ICylinder empujador = null;

            var twoPositionsActuator = (ActuatorTwoPositionsMovement)
                                       new ActuatorTwoPositionsMovement
                                           (
                                           "Motor",
                                           actuador,
                                           new SensorP(_bus.In("Input0.3"), "Posición superior"),
                                           new SensorSimulated("Posición próxima superior", false),
                                           new SensorP(_bus.In("Input2.7"), "Posición inferior"),
                                           new SensorSimulated("Posición próxima inferior", false)
                                           )
                                           .WithJogPosName("Subir")
                                           .WithJogNegName("Bajar")
                                           .WithEnableJogNeg(() => (empujador.InRest&&!_bus.In("Input2.7").Value()))
                                           .WithEnableJogPos(() => (empujador.InRest && !_bus.In("Input0.3").Value()));

            empujador =
                new Cylinder1Efect
                    (
                    _bus.In("Input2.0"),
                    _bus.In("Input0.4"),
                    _bus.Out("Output3.0"),
                    "Empujador grupo pasaportes"
                    )
                    .WithRestName("Recoger")
                    .WithWorkName("Empujar");

            Elevador2 = new Elevador2(twoPositionsActuator, empujador, solicitor, semaphore);
        }

        public bool IsEmpty()
        {
            return Elevador1.PassportGroup == null &&
                   Tramo1.Grupos.Count <= 0 &&
                   Tramo2.Grupos.Count <= 0 &&
                   Elevador2.GrupoPasaportes == null;
        }

        private void InitializeControlAire()//MDG.2012-07-23
        {
            //IEvaluable botonConexionMando2 = _bus.In("Input56.6").OR((_bus.In("Input4.5")));
            controlAireTransportes =
                new ControlAireTransportes
                    (
                    _bus.In("Input56.6"),
                    _bus.In("Input56.7"),
                    _bus.Out("Output4.0")//,
                    //_bus.Out("K744")
                    );

        }

        #region Miembros de IDiagnosisOwner

        IEnumerable<SecurityDiagnosis> IDiagnosisOwner.GetSecurityDiagnosis()
        {
            IEvaluable diagnosisSignal =
                Evaluable.FromFunctor(
                    () =>
                    !_sys.Control.InActiveMode2(Mode.Automatic)
                    && (_sys.ManualFeedRuhlamat.State == SubsystemState.Deactivated)
                    );
                    //.DelayToConnection(100)
                    //.DelayToDisconnection(100);

            return new[]
                       {
                           new SecurityDiagnosisCondition
                               ("Transportes aéreos no en modo Automatico",
                                "Arranque el modo automático de los transportes",
                                DiagnosisType.Step,
                                diagnosisSignal.Value)
                       };
        }

        #endregion

        #region Miembros de IManagerRunnable

        private TON _timerIsEmpty = new TON();//MDG.2013-03-26
        public bool longTimeEmpty;

        public IEnumerable<Action> GetManagers()
        {
            return new List<Action> { Gestor };
        }

        private void Gestor()
        {
            //MDG.2013-03-26.Para que se pare al cabo de 2 minutos
            if(_timerIsEmpty.Timing(120000, IsEmpty()))
            {
                longTimeEmpty = true;
            }
            else if (!IsEmpty())
            {
                longTimeEmpty = false;
                _timerIsEmpty.Reset();
            }
        }

        #endregion

    }
}