using System;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Diagnosis;
using Idpsa.Control.Engine.Status;
using Idpsa.Control.Sequence;
using Idpsa.Control.Subsystem;

namespace Idpsa.Control.Engine
{
    [Serializable]
    public class SystemControl : IOriginDefiner, IRi
    {
        #region Campos
        private readonly object _lockObject = new Object();
        private readonly IDPSASystem _sys;
        private readonly List<SystemControlRequest> _actionsRequested;
        private readonly Dictionary<string, AddedStatus> _addedStatus;
        private readonly Dictionary<string, AddedStatus> _addedStatus2;
        
        private bool _bootSystem;
        private bool _busOk;
        private bool _connectionCommand;
        private bool _globalAddedStatusOk;  
        private bool _origin;  
        private bool _diagnosis;                     
        private Mode _operationMode;
        private Mode _operationModeSelected;
        private bool _operationModeChange;
        private ControlModeStatus _modeStatus;
        private bool _protectionsOK; 
        private bool _protectionsCanceled;
        private bool _startingAuto;//MDG.2013-03-26
        private IDictionary<object, SubsystemState> _subsystemsAndStates;
        private ControlStopRequest _stopRequested;
        private double _maxCycleTime;
        //MDG.2012-07-23
        private bool _bootSystem2;
        private bool _connectionCommand2;
        private bool _globalAddedStatusOk2;  
        private bool _origin2;
        private bool _diagnosis2;
        private Mode _operationMode2;
        private Mode _operationModeSelected2;
        private bool _operationModeChange2;
        private ControlModeStatus _modeStatus2;
        private bool _protectionsOK2;
        private bool _protectionsCanceled2;
        private ControlStopRequest _stopRequested2;       
        #endregion        
        [field: NonSerialized]
        public event EventHandler<EventNotificationArgs> NewNotification;

        public bool BootSystem
        {
            get { return _bootSystem; }
        }
        public bool BootSystem2
        {
            get { return _bootSystem2; }
        }

        public bool SystemOK
        {
            get
            {
                return (ConnectionCommand && GlobalAddedStatusOk && ProtectionsOK && BusOk && OperationMode != Mode.WithoutMode);
            }
        }
        public bool SystemOK2
        {
            get
            {
                return (
                    ConnectionCommand2 &&
                    GlobalAddedStatusOk2 &&
                    //ProtectionsOK2 &&
                    BusOk &&
                    OperationMode2 != Mode.WithoutMode
                    );
            }
        }
        public bool BusOk
        {
            get { return _busOk; }
            internal set
            {
                if (value != _busOk)
                {
                    _busOk = value;
                    OnNewNotification
                        (new EventNotificationArgs(IdNotification.BusOk, _busOk));                    
                }
            }
        }
        public bool ConnectionCommand
        {
            get { return _connectionCommand; } 
            internal set
            {
                if (value != _connectionCommand)
                {
                    _connectionCommand = value;
                    OnNewNotification
                        (new EventNotificationArgs(IdNotification.ConnectionCommand, _connectionCommand));                    
                }
            }
        }

        public bool ConnectionCommand2
        {
            get { return _connectionCommand2; }
            internal set
            {
                if (value != _connectionCommand2)
                {
                    _connectionCommand2 = value;
                    OnNewNotification
                        (new EventNotificationArgs(IdNotification.ConnectionCommand2, _connectionCommand2));
                }
            }
        }

        public bool GlobalAddedStatusOk
        {
            get { return _globalAddedStatusOk; }
            private set
            {
                if (value != _globalAddedStatusOk)
                {
                    _globalAddedStatusOk = value;
                    OnNewNotification
                        (new EventNotificationArgs(IdNotification.GlobalAddedStatusOk, _globalAddedStatusOk));                    
                }
            }
        }

        public bool GlobalAddedStatusOk2
        {
            get { return _globalAddedStatusOk2; }
            private set
            {
                if (value != _globalAddedStatusOk2)
                {
                    _globalAddedStatusOk2 = value;
                    OnNewNotification
                        (new EventNotificationArgs(IdNotification.GlobalAddedStatusOk2, _globalAddedStatusOk2));
                }
            }
        }
        #region control_paletizado
        public bool Origin
        {
            get { return _origin; }
            private set
            {
                if (value != _origin)
                {
                    _origin = value;
                    OnNewNotification
                        (new EventNotificationArgs(IdNotification.Origin, _origin));                    
                    if (_origin && OperationMode == Mode.BackToOrigin)
                    {
                        ModeStatus = ControlModeStatus.Deactivated;
                    }                    
                }
            }
        }
        public bool Diagnosis
        {
            get { return _diagnosis; }
            set
            {
                if (value != _diagnosis)
                {
                    _diagnosis = value;
                    OnNewNotification
                        (new EventNotificationArgs(IdNotification.Diagnosis, _diagnosis));                    
                }
            }
        }

        public Mode OperationMode
        {
            get { return _operationMode; }
            private set
            {
                if (value != _operationMode)
                {
                    _operationMode = value;
                    _operationModeChange = true;
                    OnNewNotification
                        (new EventNotificationArgs(IdNotification.OperationMode, _operationMode)); 
                    ModeStatus = ControlModeStatus.Deactivated;
                }
                else
                {
                    _operationModeChange = false;
                }
            }
        }
        public ControlModeStatus ModeStatus
        {
            get { return _modeStatus; }
            internal set
            {
                if (value != _modeStatus)
                {
                    if (IsModeStatusValid(value))
                    {
                        _modeStatus = value;
                        OnNewNotification
                       (new EventNotificationArgs(IdNotification.ModeStatus, _modeStatus));                       
                    }
                }
            }
        }    
        public bool ActiveMode
        {
            get
            {
                return _modeStatus != ControlModeStatus.Deactivated;
            }
        }
        public bool ProtectionsOK
        {
            get { return _protectionsOK; }
            internal set
            {
                if (value != _protectionsOK)
                {
                    _protectionsOK = value;
                    OnNewNotification
                        (new EventNotificationArgs(IdNotification.ProtectionsOK, _protectionsOK));                    
                }
            }
        }
        public bool ProtectionsCanceled
        {
            get { return _protectionsCanceled; }
            internal set
            {
                if (value != _protectionsCanceled)
                {
                    _protectionsCanceled = value;
                    OnNewNotification
                        (new EventNotificationArgs(IdNotification.ProtectionsCanceled, _protectionsCanceled));                    
                }
            }
        }
               
        public bool StartingAuto
        {
            get { return _startingAuto; }
            set
            {
                _startingAuto=value;
            }
        }

        #endregion

        #region control_transportes
        public bool Origin2
        {
            get { return _origin2; }
            private set
            {
                if (value != _origin2)
                {
                    _origin2 = value;
                    OnNewNotification
                        (new EventNotificationArgs(IdNotification.Origin2, _origin2));
                    if (_origin2 && OperationMode2 == Mode.BackToOrigin)
                    {
                        ModeStatus2 = ControlModeStatus.Deactivated;
                    }
                }
            }
        }
        public bool Diagnosis2
        {
            get { return _diagnosis2; }
            set
            {
                if (value != _diagnosis2)
                {
                    _diagnosis2 = value;
                    OnNewNotification
                        (new EventNotificationArgs(IdNotification.Diagnosis2, _diagnosis2));
                }
            }
        }
        public Mode OperationMode2
        {
            get { return _operationMode2; }
            private set
            {
                if (value != _operationMode2)
                {
                    _operationMode2 = value;
                    _operationModeChange2 = true;
                    OnNewNotification
                        (new EventNotificationArgs(IdNotification.OperationMode2, _operationMode2));
                    ModeStatus2 = ControlModeStatus.Deactivated;
                }
                else
                {
                    _operationModeChange2 = false;
                }
            }
        }
        public ControlModeStatus ModeStatus2
        {
            get { return _modeStatus2; }
            internal set
            {
                if (value != _modeStatus2)
                {
                    if (IsModeStatusValid2(value))
                    {
                        _modeStatus2 = value;
                        OnNewNotification
                       (new EventNotificationArgs(IdNotification.ModeStatus2, _modeStatus2));
                    }
                }
            }
        }
        public bool ActiveMode2
        {
            get
            {
                return _modeStatus2 != ControlModeStatus.Deactivated;
            }
        }
        public bool ProtectionsOK2
        {
            get { return _protectionsOK2; }
            internal set
            {
                if (value != _protectionsOK2)
                {
                    _protectionsOK2 = value;
                    OnNewNotification
                        (new EventNotificationArgs(IdNotification.ProtectionsOK2, _protectionsOK2));
                }
            }
        }
        public bool ProtectionsCanceled2
        {
            get { return _protectionsCanceled2; }
            internal set
            {
                if (value != _protectionsCanceled2)
                {
                    _protectionsCanceled2 = value;
                    OnNewNotification
                        (new EventNotificationArgs(IdNotification.ProtectionsCanceled2, _protectionsCanceled2));
                }
            }
        }
        #endregion

        public IEnumerable<AddedStatus> AddedStatus
        {
            get { return _addedStatus.Values; }
        }
        public IDictionary<object, SubsystemState> SubsystemsAndStates
        {
            get { return _subsystemsAndStates; }

            internal set
            {
                _subsystemsAndStates = value;
                OnNewNotification
                          (new EventNotificationArgs(IdNotification.SubsystemsAndStates, _subsystemsAndStates));
            }
        }
        public ControlStopRequest StopRequest
        {
            get
            {
                return _stopRequested;
            }
            private set
            {
                _stopRequested = value;
            }
        }
        public ControlStopRequest StopRequest2
        {
            get
            {
                return _stopRequested2;
            }
            private set
            {
                _stopRequested2 = value;
            }
        }
        public double MaxCycleTime
        {
            get { return _maxCycleTime; }

            internal set
            {
                if (value > _maxCycleTime)
                {
                    _maxCycleTime = value;
                    OnNewNotification
                        (new EventNotificationArgs(IdNotification.MaxCycleTime, _maxCycleTime));                    
                }
            }
        }
        
        public SystemControl(IDPSASystem idpsaSystem)
        {
            _sys = idpsaSystem;
            _addedStatus = new Dictionary<string, AddedStatus>();
            _addedStatus2 = new Dictionary<string, AddedStatus>();
            _subsystemsAndStates = new Dictionary<object, SubsystemState>();
            _actionsRequested = new List<SystemControlRequest>();
        }

        public SystemControl WithAddedStatus(AddedStatus status)
        {
            if (status == null)
                throw new ArgumentNullException("status");
            if (_addedStatus.ContainsKey(status.Name))
            {
                throw new ArgumentException(
                    String.Format(
                    "A added status with the same Name ({0}), has been already added to the system control", status.Name));
            }
            _addedStatus.Add(status.Name, status);
            return this;
        }
        public SystemControl WithAddedStatus2(AddedStatus status)
        {
            if (status == null)
                throw new ArgumentNullException("status2");
            if (_addedStatus2.ContainsKey(status.Name))
            {
                throw new ArgumentException(
                    String.Format(
                    "A added status with the same Name ({0}), has been already added to the system control", status.Name));
            }
            _addedStatus2.Add(status.Name, status);
            return this;
        }
        public bool InActiveMode(Mode workingMode)
        {
            return (OperationMode == workingMode) && ActiveMode;
        }
        public bool InActiveMode2(Mode workingMode)
        {
            return (OperationMode2 == workingMode) && ActiveMode2;
        }
        public bool InOrigin()
        {
            return !RiCondition();
        }
        public bool InOrigin2()
        {
            return !RiCondition2();
        }
        public void Ri()
        {
            ModeStatus = ControlModeStatus.Deactivated;
            StopRequest = ControlStopRequest.None;
        }
        public void Ri2()
        {
            ModeStatus2 = ControlModeStatus.Deactivated;
            StopRequest2 = ControlStopRequest.None;
        }

        #region Miembros no publicos
        #region Procesamiento cícliclo

        internal void CheckSetOrigin()
        {
            Origin = _sys.InOrigin();
            Origin2 = _sys.InOrigin2();
        }
        
        internal void OperationModeManager()
        {
            CheckBootSystem();
            CheckBootSystem2();//MDG.2012-07-23
            ProccessActionRequested();
            SetGlobalAddedStatus();
            SetGlobalAddedStatus2();            
            SetOperationMode();
            SetOperationMode2();    
        }


        #endregion     

        #region Atención de peticiones

        internal void ActionRequestedHandler(SystemControlRequest action)
        {
            lock (_lockObject)
            {
                _actionsRequested.Add(action);
            }
        }
        private void ProccessActionRequested()
        {
            List<SystemControlRequest> _actionsTemp = null;
            lock (_lockObject)
            {
                if (_actionsRequested.Count == 0)
                    return;
                _actionsTemp = _actionsRequested.ToList();
                _actionsRequested.Clear();
            }

            foreach (var actionRequested in _actionsTemp)
            {
                switch (actionRequested.Id)
                {
                    case SystemControlRequest.IdRequest.StartMode:
                        if ((_operationModeSelected == Mode.Automatic) ||
                            (_operationModeSelected == Mode.BackToOrigin))
                        {
                            Chain.StepByStep.StepByStepMode = false;
                            StopRequest = ControlStopRequest.None;
                            ModeStatus = ControlModeStatus.Activated;
                        }
                        break;

                    case SystemControlRequest.IdRequest.StartModeWithStepByStep:
                        if ((_operationModeSelected == Mode.Automatic) ||
                            (_operationModeSelected == Mode.BackToOrigin))
                        {
                            Chain.StepByStep.StepByStepMode = true;
                            StopRequest = ControlStopRequest.None;
                            ModeStatus = ControlModeStatus.Activated;
                        }
                        break;

                    case SystemControlRequest.IdRequest.StopMode:
                        StopRequest = ControlStopRequest.Stop;
                        break;

                    case SystemControlRequest.IdRequest.StopModeAndDeativate:
                        StopRequest = ControlStopRequest.StopAndDeactive;
                        break;

                    case SystemControlRequest.IdRequest.AutomaticMode:
                        if (OperationMode != Mode.Automatic)
                        {
                            _operationModeSelected = Mode.Automatic;
                            ModeStatus = ControlModeStatus.Deactivated;
                        }
                        break;

                    case SystemControlRequest.IdRequest.BackToOriginMode:
                        if (OperationMode != Mode.BackToOrigin)
                        {
                            _operationModeSelected = Mode.BackToOrigin;
                            ModeStatus = ControlModeStatus.Deactivated;
                        }
                        break;

                    case SystemControlRequest.IdRequest.ManualMode:
                        if (OperationMode != Mode.Manual)
                        {
                            _operationModeSelected = Mode.Manual;
                            ModeStatus = ControlModeStatus.Deactivated;
                        }
                        break;

                    case SystemControlRequest.IdRequest.ActiveSubsystems:
                        var subSystems1 = actionRequested.Value as IEnumerable<object>;
                        try
                        {
                            SetSubsystemsState(subSystems1, SubsystemState.Activated);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException("subsystem can't be activated", ex);
                        }
                        break;

                    case SystemControlRequest.IdRequest.DeactiveSubsystems:
                        var subSystems2 = actionRequested.Value as IEnumerable<object>;
                        try
                        {
                            SetSubsystemsState(subSystems2, SubsystemState.Deactivated);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException("subsystem can't be deactivated", ex);
                        }
                        break;

                    case SystemControlRequest.IdRequest.Rearmament:
                        Diagnosis = false;
                        AlarmsHistoricalManager.Instance.AddStepsDiagnosis();
                        DiagnosisManager.Instance.Clear();
                        _sys.Chains.AutomaticChains.QuitError();
                        _sys.Chains.BackToOgiginChains.QuitError();
                        break;
                    case SystemControlRequest.IdRequest.ResetMaxCycleTime:
                        _maxCycleTime = 0;
                        break;

                        //MDG.2012-07-23
                    case SystemControlRequest.IdRequest.StartMode2:
                        if ((_operationModeSelected2 == Mode.Automatic) ||
                            (_operationModeSelected2 == Mode.BackToOrigin))
                        {
                            Chain.StepByStep.StepByStepMode = false;//*
                            StopRequest2 = ControlStopRequest.None;
                            ModeStatus2 = ControlModeStatus.Activated;
                        }
                        break;

                    case SystemControlRequest.IdRequest.StartModeWithStepByStep2:
                        if ((_operationModeSelected2 == Mode.Automatic) ||
                            (_operationModeSelected2 == Mode.BackToOrigin))
                        {
                            Chain.StepByStep.StepByStepMode = true;//*
                            StopRequest2 = ControlStopRequest.None;
                            ModeStatus2 = ControlModeStatus.Activated;
                        }
                        break;

                    case SystemControlRequest.IdRequest.StopMode2:
                        StopRequest2 = ControlStopRequest.Stop;
                        break;

                    case SystemControlRequest.IdRequest.StopModeAndDeativate2:
                        StopRequest2 = ControlStopRequest.StopAndDeactive;
                        break;

                    case SystemControlRequest.IdRequest.AutomaticMode2:
                        if (OperationMode2 != Mode.Automatic)
                        {
                            _operationModeSelected2 = Mode.Automatic;
                            ModeStatus2 = ControlModeStatus.Deactivated;
                        }
                        break;

                    case SystemControlRequest.IdRequest.BackToOriginMode2:
                        if (OperationMode2 != Mode.BackToOrigin)
                        {
                            _operationModeSelected2 = Mode.BackToOrigin;
                            ModeStatus2 = ControlModeStatus.Deactivated;
                        }
                        break;

                    case SystemControlRequest.IdRequest.ManualMode2:
                        if (OperationMode2 != Mode.Manual)
                        {
                            _operationModeSelected2 = Mode.Manual;
                            ModeStatus2 = ControlModeStatus.Deactivated;
                        }
                        break;
                    case SystemControlRequest.IdRequest.Rearmament2:
                        Diagnosis2 = false;
                        AlarmsHistoricalManager.Instance.AddStepsDiagnosis();
                        DiagnosisManager.Instance.Clear();
                        _sys.Chains.AutomaticChains2.QuitError();
                        //_sys.Chains.BackToOgiginChains2.QuitError();
                        break;
                }
            }
        }
        protected void OnNewNotification(EventNotificationArgs e)
        {
            if (NewNotification != null)
                NewNotification(this, e);
        }    
        #endregion        
       
        internal bool RiCondition()
        {
            bool result = false;
            if (_operationModeChange || !SystemOK)
            {
                result = true;
            }
            return result;
        }

        internal bool RiCondition2()
        {
            bool result = false;
            if (_operationModeChange2 || !SystemOK2)
            {
                result = true;
            }
            return result;
        }
        
        internal void NotifyInitialState()
        {
            foreach (IdNotification notification in Enum.GetValues(typeof(IdNotification)))
                OnNewNotification(
                    new EventNotificationArgs(
                        notification,
                        GetType().GetProperty(notification.ToString()).GetValue(this, null)
                        )
                    );
            foreach (AddedStatus status in _addedStatus.Values)
            {
                status.NotifyStatus();
                status.NotifyStatus2();
            }
        }

        private void CheckBootSystem()
        {           
                bool value = SystemOK && !_diagnosis && !_protectionsCanceled && _origin;
                if (value != _bootSystem)
                {
                    OnNewNotification
                        (new EventNotificationArgs(IdNotification.BootSystem, value));
                    _bootSystem = value;
                }
        }

        private void CheckBootSystem2()
        {
            bool value = SystemOK2 && !_diagnosis2 && !_protectionsCanceled2 && _origin2;
            if (value != _bootSystem2)
            {
                OnNewNotification
                    (new EventNotificationArgs(IdNotification.BootSystem2, value));
                _bootSystem2 = value;
            }
        }

        private bool IsModeStatusValid(ControlModeStatus modeStatus)
        {
            if (modeStatus == ControlModeStatus.Activated)
            {
                if (RiCondition())
                    return false;

                if (OperationMode == Mode.Automatic && !BootSystem)
                    return false;
            }            
            return true;
        }
        private bool IsModeStatusValid2(ControlModeStatus modeStatus2)
        {
            if (modeStatus2 == ControlModeStatus.Activated)
            {
                if (RiCondition2())
                    return false;

                if (OperationMode2 == Mode.Automatic && !BootSystem2)
                    return false;
            }
            return true;
        }
        private void SetOperationMode()
        {
            if (!GlobalAddedStatusOk)
            {
                OperationMode = _operationModeSelected = Mode.WithoutMode;
                return;
            }
            OperationMode = _operationModeSelected;
        }
        private void SetOperationMode2()
        {
            if (!GlobalAddedStatusOk2)
            {
                OperationMode2 = _operationModeSelected2 = Mode.WithoutMode;
                return;
            }
            OperationMode2 = _operationModeSelected2;
        }
        private void SetGlobalAddedStatus()
        {
            GlobalAddedStatusOk =
                (_addedStatus.Values.Any((aS) => aS.Status == StatusValue.Abort)) ? false : true;
        }
        private void SetGlobalAddedStatus2()
        {
            GlobalAddedStatusOk2 =
                (_addedStatus2.Values.Any((aS) => aS.Status2 == StatusValue.Abort)) ? false : true;
        }
        private void SetSubsystemsState(IEnumerable<object> subsystems, SubsystemState value)
        {
            if (subsystems == null)
                throw new ArgumentNullException("subsystems");
            try
            {
                _sys.Subsystems.SetSubsystemsState(subsystems, value);
            }
            catch
            {
                throw;
            }
            _sys.Signals.SetSecurityDiagnosis(_sys.Subsystems);
        }
        #endregion

        #region IdNotification enum

        public enum IdNotification
        {
            BootSystem,
            BusOk,
            ConnectionCommand,
            GlobalAddedStatusOk,
            Origin,
            Diagnosis,
            OperationMode,
            ModeStatus,
            ProtectionsOK,
            ProtectionsCanceled,
            SubsystemsAndStates,
            MaxCycleTime,
            //MDG.2012-07-23
            BootSystem2,
            ConnectionCommand2,
            GlobalAddedStatusOk2,
            Origin2,
            Diagnosis2,
            OperationMode2,
            ModeStatus2,
            ProtectionsOK2,
            ProtectionsCanceled2
        }

        #endregion

        #region Nested type: EventNotificationArgs

        public class EventNotificationArgs : EventArgs
        {
            public static readonly new EventNotificationArgs Empty;

            public EventNotificationArgs(IdNotification idNotification, object value)
            {
                IdNotification = idNotification;
                Value = value;
            }

            public IdNotification IdNotification { get; private set; }
            public object Value { get; private set; }
        }

        #endregion
    }
}