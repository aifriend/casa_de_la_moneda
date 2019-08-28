using Idpsa.Control;//MDG.2013-02-06
using Idpsa.Control.Component;
using Idpsa.Control.Diagnosis;//MDG.2013-02-06
using Idpsa.Control.Subsystem;
using Idpsa.Control.Tool;
using System;//MDG.2013-03-26
using System.Collections.Generic;//MDG.2013-02-06

namespace Idpsa.Paletizado
{
    //public class TransporteLinea1
    public class TransporteLinea1 : IDiagnosisOwner/*MDG.2013-02-06*/ , IManagerRunnable

    {
        private readonly Bus _bus;
        private readonly IdpsaSystemPaletizado _sys;

        public TransporteLinea1(IdpsaSystemPaletizado sys,
                                SourceGroupSupplier supplier,
                                IItemSolicitor<GrupoPasaportes> solicitor,
                                LinesSemaphore semaphore)
        {
            _sys = sys;
            _bus = sys.Bus;
            InitializeControlAire();//MDG.2012-07-23
            InitializeTramo1();
        }

        [Subsystem(Filter = SubsystemFilter.None)]
        public Elevador1 Elevador1 { get; private set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        public TramoTransporteGruposPasaportes Tramo1 { get; set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        public ControlAireTransportes controlAireTransportes { get; set; }//MDG.2012-07-23


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

            IItemSolicitor<GrupoPasaportes> solicitor = Tramo1;

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

        public bool IsEmpty()
        {
            return Tramo1.Grupos.Count <= 0;
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
            if (_timerIsEmpty.Timing(120000, IsEmpty()))
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