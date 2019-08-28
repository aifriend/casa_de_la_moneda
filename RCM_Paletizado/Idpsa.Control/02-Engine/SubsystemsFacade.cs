using System;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Tool;
using Idpsa.Control.Subsystem;
using Idpsa.Control.Component;
using Idpsa.Control.Diagnosis;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Engine
{
    internal class SubsystemsFacade:IDiagnosisOwner ,ISpecialDeviceOwner,IManualsProvider ,IDisposable
    {
        private SystemControl _control;
        private SubsystemsAnalizer _subsystemsAnalizer;
        private bool _groupStatesActualized;
        internal Action Ri { get; private set; }
        internal Action Ri2 { get; private set; }
        internal Action Manager { get; private set; }

        public SystemChains Chains { get; private set; }
        public SpecialDevicesManager SpecialDevices { get; private set; }
        public Func<bool> InOrigin { get; private set; }

        public SubsystemsFacade(IDPSASystem system)
        {
            _control = system.Control;
            _subsystemsAnalizer = new SubsystemsAnalizer(system);
            Chains = new SystemChains(_control);
            SpecialDevices = new SpecialDevicesManager(_control, GetAllSpecialDevices());            
        }     

        internal void SetSubsystemsState(IEnumerable<object> subsystems, SubsystemState state)
        {
            if (subsystems == null)
                throw new ArgumentNullException("subsystems");
            
            try
            {
                foreach (var subSys in subsystems)
                {
                    PutSubsystemState(GetAnalizerNode(subSys), state);
                }
                SetActiveSubsystems();
            }
            catch { throw; } 
           
        }      
        internal void SetAllSubsystemStates(SubsystemState state)
        {
            foreach (var element in _subsystemsAnalizer.Roots)
            {
                PutSubsystemState(element, state);               
            }            
            
            SetActiveSubsystems();           
        }

        private TreeNode<SubsystemsAnalizer.AnalizerElement> GetAnalizerNode(object subSystem)
        {
            if (subSystem == null)
                throw new ArgumentNullException("subSystem");

            var value = _subsystemsAnalizer.Nodes.FirstOrDefault(n => n.Value.SubSystem == subSystem);

            if (value == null)
                throw new ArgumentException("the subsystem isn't defined");
            return value;
        }
        private void PutSubsystemState(TreeNode<SubsystemsAnalizer.AnalizerElement> element, SubsystemState state)
        {

            if (state == SubsystemState.Activated && element.Parent.Value.State == SubsystemState.Deactivated)
                return;

            element.Execute(
                v =>
                {
                    if (v.State != state)
                    {
                        v.State = state;
                        _groupStatesActualized = false;
                    }
                }
            );          
        }    
        private void SetActiveSubsystems()
        {            
            if (!_groupStatesActualized)
            {
                try
                {                                    
                    Chains.SetSubsystems(_subsystemsAnalizer);
                    SpecialDevices.SetSpecialDevices(_subsystemsAnalizer);
                    _subsystemsAnalizer.ConstructFunctors();
                    Ri = _subsystemsAnalizer.Ri;
                    Ri2 = _subsystemsAnalizer.Ri2;
                    InOrigin = _subsystemsAnalizer.InOrigin;
                    Manager = null;
                    _subsystemsAnalizer
                        .GetManagers()
                        .ForEach(manager => Manager += manager);
                    _groupStatesActualized = true;
                    SetSystemControlSubsystemsState();
                }
                catch
                {
                    throw;
                }
            }
        }
        private void SetSystemControlSubsystemsState()
        {
            _control.SubsystemsAndStates = _subsystemsAnalizer.ActiveSubystems();                
        }

        #region Miembros de IDisposable

        void IDisposable.Dispose()
        {
            _subsystemsAnalizer.Dispose();
        }

        #endregion

        #region Miembros de ISpecialDeviceOwner

        IEnumerable<ISpecialDevice> ISpecialDeviceOwner.GetSpecialDevices()
        {
            return _subsystemsAnalizer.GetSpecialDevices();
        }

        private IEnumerable<ISpecialDevice> GetAllSpecialDevices()
        {
            return _subsystemsAnalizer.GetAllSpecialDevices();
        }

        #endregion

        #region Miembros de ISecurityDiagnosisOwner

        IEnumerable<SecurityDiagnosis> IDiagnosisOwner.GetSecurityDiagnosis()
        {
            return _subsystemsAnalizer.GetSecurityDiagnosis();
        }

        #endregion
        
        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            return _subsystemsAnalizer.GetManualsRepresentations();
        }

        #endregion
    }
}