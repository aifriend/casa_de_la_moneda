using System;

namespace Idpsa.Control.Subsystem
{
    public class SubsystemStateAware : ISubsystemStateAware, ISubsystemStateController
    {        
        private ISubsystemStateController _stateSolicitor;
        private Action<SubsystemState> _onStateChanged;

        public SubsystemState State { get; private set; }
        
        public SubsystemStateAware()
        {
            _onStateChanged = state => State = state;
        }

        public SubsystemStateAware(Action<SubsystemState> onStateChanged)
        {
            if (onStateChanged == null)
                throw new ArgumentNullException("onStateChanged");
            _onStateChanged = state => State = state;
            _onStateChanged += onStateChanged; 
        }

        public void Activate()
        {
            _stateSolicitor.Activate();
        }

        public void Deactivate()
        {
            _stateSolicitor.Deactivate();
        }

        ISubsystemStateObserver ISubsystemStateAware.SetSubsystemStateController(ISubsystemStateController value)
        {
            _stateSolicitor = value;
            return new SubsystemStateOveserver(_onStateChanged);
        }

        private class SubsystemStateOveserver : ISubsystemStateObserver
        {
            private Action<SubsystemState> _onStateChanged;
            public SubsystemStateOveserver(Action<SubsystemState> onStateChanged)
            {              
                _onStateChanged = onStateChanged;
            }

            public void OnStateChanged(SubsystemState state)
            {
                _onStateChanged(state);
            }
        }
        
    }

}