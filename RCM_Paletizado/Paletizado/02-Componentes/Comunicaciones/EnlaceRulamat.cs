using System;
using System.Collections.Generic;
using System.Net;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals;
using Idpsa.Control.Subsystem;
using Idpsa.Paletizado;
using RCMCommonTypes;

namespace Idpsa
{
    public class EnlaceRulamat : IManagerRunnable, IManualsProvider, ISocket
    {
        //Socket

        #region RobotEnlaceWorkingMode enum

        public enum RobotEnlaceWorkingMode
        {
            None = -1,
            Automatic = 0,
            Manual = 1,
            Disabled = 2
        }

        #endregion

        private static IdpsaSystemPaletizado _sys;

        [Manual(SuperGroup = "Robot Enlace", Group = "RE Inputs")] private readonly Sensor _inAlive;

        [Manual(SuperGroup = "Robot Enlace", Group = "RE Inputs")] private readonly Sensor _inBotonConfirmacion;

        [Manual(SuperGroup = "Robot Enlace", Group = "RE Inputs")] private readonly Sensor _inCentradorReposo;
        [Manual(SuperGroup = "Robot Enlace", Group = "RE Inputs")] private readonly Sensor _inEnvioGrupoPasaportes;
        [Manual(SuperGroup = "Robot Enlace", Group = "RE Inputs")] private readonly Sensor _inErrorPesoRfid;
        [Manual(SuperGroup = "Robot Enlace", Group = "RE Inputs")] private readonly Sensor _inModoFuncionamiento0;

        [Manual(SuperGroup = "Robot Enlace", Group = "RE Inputs")] private readonly Sensor _inModoFuncionamiento1;

        [Manual(SuperGroup = "Robot Enlace", Group = "RE Inputs")] private readonly Sensor _inPermisoSubirAscensor;

        private readonly IEvaluable _inPresencia;
        [Manual(SuperGroup = "Robot Enlace", Group = "RE Inputs")] private readonly Sensor _inPresenciaAbajo;
        [Manual(SuperGroup = "Robot Enlace", Group = "RE Inputs")] private readonly Sensor _inPresenciaArriba;
        private readonly SocketListener<GrupoPasaportes, GroupResume> _listener;

        //Outputs

        [Manual(SuperGroup = "Robot Enlace", Group = "RE Outputs")] private readonly Actuator _outAlive;
        [Manual(SuperGroup = "Robot Enlace", Group = "RE Outputs")] private readonly Actuator _outBotonConfirmacion;

        [Manual(SuperGroup = "Robot Enlace", Group = "RE Outputs")] private readonly Actuator _outErrorGrupo;

        [Manual(SuperGroup = "Robot Enlace", Group = "RE Outputs")] private readonly Actuator _outHabilitadoRobotEnlace;
        [Manual(SuperGroup = "Robot Enlace", Group = "RE Outputs")] private readonly Actuator _outModoAutomatico;
        [Manual(SuperGroup = "Robot Enlace", Group = "RE Outputs")] private readonly Actuator _outOkGrupo;
        [Manual(SuperGroup = "Robot Enlace", Group = "RE Outputs")] private readonly Actuator _outPermisoPinza;
        [Manual(SuperGroup = "Robot Enlace", Group = "RE Outputs")] private readonly Actuator _outPreparado;
        private bool _elevatorDown;

        public EnlaceRulamat(IPEndPoint endPoint, IdpsaSystemPaletizado sys)
        {
            _sys = sys;

            //Inputs
            _inModoFuncionamiento0 = new SensorP(_sys.Bus.In("Input56.0"), "Modo de funcionamiento 0");
            _inModoFuncionamiento1 = new SensorP(_sys.Bus.In("Input56.1"), "Modo de funcionamiento 1");
            _inPermisoSubirAscensor = new SensorP(_sys.Bus.In("Input56.2"), "Permiso para subir ascensor");
            _inEnvioGrupoPasaportes = new SensorP(_sys.Bus.In("Input56.3"), "Envio grupo pasaportes");
            _inAlive = new SensorP(_sys.Bus.In("Input56.4"), "Robot de enlace alive");
            _inErrorPesoRfid = new SensorP(_sys.Bus.In("Input56.5"), "Error de pesaje y/o lectura de chip");
            _inBotonConfirmacion = new SensorP(_sys.Bus.In("Input1.6"), "Boton de confirmacion");
            _inBotonConfirmacion.DelayToDisconnection(2000);//1000);
            _inPresenciaArriba = new SensorP(_sys.Bus.In("Input5.4"), "Presencia color arriba grupo pasaportes");
            _inPresenciaAbajo = new SensorP(_sys.Bus.In("Input5.1"), "Presencia color abajo grupo pasaportes");
            _inCentradorReposo = new SensorP(_sys.Bus.In("Input5.3"), "Centrador en reposo");
            _inPresencia = _inPresenciaAbajo.OR(_inPresenciaArriba);

            //Outputs
            //_outModoAutomatico = new ActuatorP(_sys.Bus.Out("Output50.0"), "Paletizado modo automatico");
            _outModoAutomatico = new ActuatorP(_sys.Bus.Out("Output50.0"), "Transportes Paletizado modo automatico");//MDG.2013-01-17.We check Transports in Automatic, not the paletizer
            _outPreparado = new ActuatorP(_sys.Bus.Out("Output50.1"), "Preparado para recibir pasaportes");
            _outPermisoPinza = new ActuatorP(_sys.Bus.Out("Output50.2"), "Permiso pinza en trayectoria ascensor");
            _outAlive = new ActuatorP(_sys.Bus.Out("Output50.3"), "Paletizado alive");
            _outErrorGrupo = new ActuatorP(_sys.Bus.Out("Output50.4"), "Error info grupo");
            _outHabilitadoRobotEnlace = new ActuatorP(_sys.Bus.Out("Output50.5"), "Robot de enlace habilitado");
            _outBotonConfirmacion = new ActuatorP(_sys.Bus.Out("Output50.6"), "Boton de confirmacion");
            _outOkGrupo = new ActuatorP(_sys.Bus.Out("Output50.7"), "Ok info grupo");

            //Socket
            _listener =
                new SocketListener<GrupoPasaportes, GroupResume>(endPoint.Address, endPoint.Port,
                                                                 _ => new GrupoPasaportes(_));
            _listener.DataReceived += OnReceivedData;
        }

        public bool RobotEnlaceEnabled { get; private set; }

        public RobotEnlaceWorkingMode ModoFuncionamiento
        {
            get
            {
                bool activated0 = _inModoFuncionamiento0.Value();
                bool activated1 = _inModoFuncionamiento1.Value();
                if (!activated0 & !activated1) return RobotEnlaceWorkingMode.None;
                if (activated0 & activated1) return RobotEnlaceWorkingMode.Disabled;
                return activated0 ? RobotEnlaceWorkingMode.Automatic : RobotEnlaceWorkingMode.Manual;
            }
        }

        #region IManagerRunnable Members

        IEnumerable<Action> IManagerRunnable.GetManagers()
        {
            return new Action[]
                       {
                           RobotEnlaceServer,
                           ReadInput,
                           WriteOutput,
                           RobotEnlaceScreen
                       };
        }

        #endregion

        #region ISocket Members

        void IDisposable.Dispose()
        {
            ResetOutputs();
            ((IDisposable) _listener).Dispose();
        }

        #endregion

        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            var manual = new Manual(this) {Description = "Comunicacion Robot Enlace"};
            return new[] {manual};
        }

        #endregion

        #region Miembros de IReader

        private GrupoPasaportes _groupReived;

        public bool Connected()
        {
            return _listener.IsConnected();
        }

        public bool Connect()
        {
            _listener.Connect();
            return true;
        }

        public bool Disconnect()
        {
            _listener.Disconnect();
            return true;
        }

        public bool BeginListening()
        {
            return true;
        }

        public bool EndListening()
        {
            return true;
        }

        public void Reset()
        {
            return;
        }

        public bool ReadMsg(out string code)
        {
            code = string.Empty;
            if (_hasReceivedData)
            {
                _groupReived = _receivedData;
                code = _receivedData.Id;
                return true;
            }
            return false;
        }

        public string LastMsgRead
        {
            get { return _groupReived != null ? _groupReived.Id : "Sin Lectura"; }
        }

        public string Disponibilidad()
        {
            return string.Empty;
        }

        #endregion

        public void ActivateRobotEnlace()
        {
            RobotEnlaceEnabled = true;
        }

        public void DeactivateRobotEnlace()
        {
            RobotEnlaceEnabled = false;
        }


        private void ResetOutputs()
        {
            _outModoAutomatico.Activate(false);
            _outPreparado.Activate(false);
            _outPermisoPinza.Activate(false);
            _outAlive.Activate(false);
            _outErrorGrupo.Activate(false);
            _outHabilitadoRobotEnlace.Activate(false);
            _outBotonConfirmacion.Activate(false);
            _outOkGrupo.Activate(false);
        }

        private void WriteOutput()
        {
            _outAlive.Activate(_inAlive.Value());
            //_outBotonConfirmacion.Activate(_inBotonConfirmacion.Value());
            _outBotonConfirmacion.Activate(_inBotonConfirmacion.Value() && _inPresencia.Value());//MDG.2012-11-20.Aseguramos que sólo envía el pulsado si está detectando con las fotocélulas
            _outPermisoPinza.Activate(!_inPresencia.Value() && _elevatorDown && _inCentradorReposo.Value());
            //_outModoAutomatico.Activate(_sys.Control.InActiveMode(Mode.Automatic));
            _outModoAutomatico.Activate(_sys.Control.InActiveMode2(Mode.Automatic));//MDG.2013-01-17.We check Transports in Automatic, not the paletizer
            _outHabilitadoRobotEnlace.Activate(RobotEnlaceEnabled);
            if (_sys.Lines.Line2.TransporteLinea.Elevador1 != null)
                IsReadyToReceivePassport();
            else
                _outPreparado.Activate(false);
        }

        private void ReadInput()
        {
            _elevatorDown = _sys.Lines.Line2.TransporteLinea.Elevador1.ElevatorDown();
        }

        private void RobotEnlaceScreen()
        {
            if (_outHabilitadoRobotEnlace.Value())
                ScreenPlay.DoState(ScreenPlay.Estado.PuestoKo);
            else if (!_sys.Control.InActiveMode(Mode.Automatic))
                ScreenPlay.DoState(ScreenPlay.Estado.PuestoOk);
        }

        #region Input

        public bool ElevatorAllowedMoveUp
        {
            get { return _inPermisoSubirAscensor.Value() && _inCentradorReposo.Value(); }
        }

        public bool ElevatorAllowed
        {
            get { return _inPermisoSubirAscensor.Value(); }
        }

        public bool StartSendingPassportGroup
        {
            get { return _inEnvioGrupoPasaportes.Value(); }
        }

        public bool GroupHasError
        {
            get { return _inErrorPesoRfid.Value(); }
        }

        #endregion

        #region Ouput

        public void GroupReset()
        {
            _outOkGrupo.Activate(false);
            _outErrorGrupo.Activate(false);
        }

        public void GroupAllowed()
        {
            _outOkGrupo.Activate(true);
            _outErrorGrupo.Activate(false);//MDG.2013-01-17
        }

        public void GroupNotAllowed()
        {
            _outErrorGrupo.Activate(true);
            _outOkGrupo.Activate(false);//MDG.2013-01-17
        }

        public bool IsReadyToReceivePassport()
        {
            bool state = false;
            if (_sys.Lines.Line2.TransporteLinea.Elevador1.IsRobotEnlaceInManualMode)
            {
                state = _elevatorDown && !_inCentradorReposo.Value();
            }
            else if (_sys.Lines.Line2.TransporteLinea.Elevador1.IsRobotEnlaceInAutomaticMode)
            {
                state = 
                    //!_inPresencia.Value() && //MDG.2012-11-27. Para que no se paren las cadenase auto al reiniciarlas
                    _elevatorDown && _inCentradorReposo.Value();
            }
            _outPreparado.Activate(state);
            return state;
        }

        #endregion

        #region Socket

        private bool _hasReceivedData;
        private GrupoPasaportes _receivedData;

        public GrupoPasaportes DataReceived
        {
            get { return _hasReceivedData ? _receivedData : null; }
        }

        private void OnReceivedData(object e, DataEventArgs<GrupoPasaportes> args)
        {
            _hasReceivedData = _listener.TryGetData(out _receivedData);
        }

        private void RobotEnlaceServer()
        {
            _listener.Manager();
        }

        #endregion
    }
}