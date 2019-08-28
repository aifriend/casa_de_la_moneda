using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using Idpsa.Control.Component;
using Idpsa.Control.Engine;
using Idpsa.Control.Subsystem;

namespace Idpsa.Control
{
    [Synchronization]
    [Serializable]
    public abstract class IDPSASystem : IRi, IOriginDefiner, IDisposable 
    {
        private readonly ICollection<IRestorable> _restorableSubSystems; 
        private Action _subsystemsDispose;

        protected IDPSASystem()
        {
            InitializeSystem();
            InitializeSubSystems();
            IntegrateSubsystems();            
            ActiveAllSubsystems();           
            _restorableSubSystems = new List<IRestorable>();            
        }

        protected IDPSASystem(ICollection<IRestorable> _restorableSubsystems):this()
        {
            if (_restorableSubsystems == null)
                throw new ArgumentNullException("restorableSubsystems"); 
            
            this._restorableSubSystems = _restorableSubsystems;
        }

        #region IDPSASystem Members

        internal SubsystemsFacade Subsystems { get; private set; }

        public Bus Bus { get; private set; }
        public SystemControl Control { get; private set; }
        public SystemGeneralSignals Signals { get; private set; }  
        public SystemChains Chains { get { return Subsystems.Chains; } }
        public SpecialDevicesManager SpecialDevices { get { return Subsystems.SpecialDevices; } }

        #endregion

        public void Ri()
        {
            if (Control.RiCondition())
            {
                Control.Ri();
                Subsystems.Ri();
            }
            if (Control.RiCondition2())
            {
                Control.Ri2();
                Subsystems.Ri2();
            }
        }

        public bool InOrigin()
        {
            bool value = Control.InOrigin() &&
                         Chains.InOrigin() &&
                         Subsystems.InOrigin();

            return value;
        }

        public bool InOrigin2()
        {
            bool value = Control.InOrigin2();// &&
                         Chains.InOrigin2();// &&
                         //Subsystems.InOrigin2();

            return value;
        }
       

        private void InitializeSystem()
        {
            Bus = ConstructBus();
            Control = ConstructSystemControl();
            //Control2 = ConstructSystemControl2();
            Signals = ConstructSystemGeneralSignals();
        }

        protected abstract Bus ConstructBus();
        protected abstract SystemControl ConstructSystemControl();
        //protected abstract SystemControl2 ConstructSystemControl2();
        protected abstract SystemGeneralSignals ConstructSystemGeneralSignals();
        protected abstract void InitializeSubSystems();

        private void IntegrateSubsystems()
        {
            Subsystems = new SubsystemsFacade(this);            
            _subsystemsDispose = new Action(((IDisposable)Subsystems).Dispose);
        }

        private void ActiveAllSubsystems()
        {
            Subsystems.SetAllSubsystemStates(SubsystemState.Activated);
            Signals.SetSecurityDiagnosis((IDiagnosisOwner)Subsystems);            
        }        
              
        public void RestoreSubsystems()
        {
            foreach (IRestorable subSystem in _restorableSubSystems)
            {
                subSystem.Restore();
            }
        }

        #region Miembros de IDisposable

        protected virtual void DisposeCore()
        {
            _subsystemsDispose();
        }

        public void Dispose()
        {
            DisposeCore();
        }

        #endregion
    }
  
}

