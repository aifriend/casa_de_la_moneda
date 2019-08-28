using System;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Component;
using Idpsa.Control.Diagnosis;
using Idpsa.Control.Subsystem;
using Idpsa.Control.Engine.Status;

namespace Idpsa.Control.Engine
{
    [Serializable]
    public class SystemGeneralSignals
    {
        private IActivable _busOkOut;
        private Bus _bus;
        private SystemControl _control;
        private ICommandController _commandController;
        private IProtectionsController _protectionsController;
        private SecuritySignalCollection _securities;
        private IDiagnosisOwner _generalSecurities;
        private readonly List<IActivable> _emergencyActivatedSignals;
        private readonly List<IActivable> _commandActivatedSignals;
          
        public SystemGeneralSignals(IActivable busOkOut, Bus bus, SystemControl control,
            ICommandController commandController, IDiagnosisOwner generalSecurities)
        {
            if (busOkOut == null) throw new ArgumentNullException("busOkOut");
            if (bus == null) throw new ArgumentNullException("bus");
            if (control == null) throw new ArgumentNullException("control");
            if (commandController == null) throw new ArgumentNullException("commandController");
            
            _control = control;
            _bus = bus;
            _busOkOut = busOkOut;
            _commandController = commandController;
            _protectionsController = new ProtectionsControllerSimulated();
            _securities = new SecuritySignalCollection();
            _commandActivatedSignals = new List<IActivable>();
            _emergencyActivatedSignals = new List<IActivable>();

            if (generalSecurities != null)
                _generalSecurities = generalSecurities;
        }

        public SystemGeneralSignals WithProtectionsController(IProtectionsController protectionsController)
        {
            if (protectionsController == null)
                throw new ArgumentNullException("protectionsController");

            _protectionsController = protectionsController;
            return this;
        }
        public SystemGeneralSignals WithCommandActivatedSignals(params IActivable[] signals)
        {
            if (signals == null) throw new ArgumentNullException("signals");
            foreach (var signal in signals)
                if (signal == null) throw new ArgumentNullException("signals can't provide a null signal");

            _commandActivatedSignals.AddRange(signals);
            return this;
        }
        public SystemGeneralSignals WithEmergencyActivatedSignals(params IActivable[] signals)
        {
            if (signals == null) throw new ArgumentNullException("signals");
            foreach (var signal in signals)
                if (signal == null) throw new ArgumentNullException("signals can't provide a null signal");

            _emergencyActivatedSignals.AddRange(signals);
            return this;
        }

        public bool EmergencyActuated()
        {
            bool value = false;
            foreach (SecurityDiagnosis securidad in _securities.Actuated())
            {
                if (securidad.Type == DiagnosisType.Security)
                {
                    value = true;
                    break;
                }
            }
            return value;
        }
        public IEnumerable<SecurityDiagnosis> ActuatedSecurities()
        {
            return _securities.Actuated();
        }
        public IEnumerable<SecurityDiagnosis> NotActuatedSecurities()
        {
            return _securities.NotActuated();
        }

                  
        internal void AddedStatusControl()
        {
            foreach (AddedStatus status in _control.AddedStatus)
                status.StatusControl(_control);
        }
        internal void SecurityDiagnosisManager()
        {
            try
            {
                foreach (SecurityDiagnosis s in ActuatedSecurities())
                {
                    if (!DiagnosisManager.Instance.Exist(s.Name))
                    {
                        DiagnosisManager.Instance.Add(s);
                    }
                }

                foreach (SecurityDiagnosis s in NotActuatedSecurities())
                {
                    if (DiagnosisManager.Instance.Exist(s.Name))
                    {
                        AlarmsHistoricalManager.Instance.AddEventDiagnosis(s);
                        DiagnosisManager.Instance.Remove(s);
                    }
                }
            }
            catch
            {
            }
        }     
        internal void SetSecurityDiagnosis(IDiagnosisOwner iSecurityDiagnosisOwner)
        {
            _securities.Clear();

            if (_generalSecurities != null)
            {
                _securities.AddRange(_generalSecurities.GetSecurityDiagnosis());
            }
            if (iSecurityDiagnosisOwner != null)
            {
                _securities.AddRange(iSecurityDiagnosisOwner.GetSecurityDiagnosis());
            }
        }
        internal void SignalsControl()
        {
            CommandControl();
            ProtectionsControl();
            BusControl();
            CommandActivatedSignalsControl();
            EmergencyActivatedSignalsControl();
        }

        private void CommandControl()
        {
            _commandController.CommandControl(); 
            _control.ConnectionCommand = _commandController.ConnectionCommand;
            _control.ConnectionCommand2 = _commandController.ConnectionCommand2;
        }
        private void ProtectionsControl()
        {
            _protectionsController.ProtectionsControl(_control);
            _control.ProtectionsOK = _protectionsController.ProtectionsOK;
            _control.ProtectionsCanceled = _protectionsController.ProtectionsCanceled;
            _control.ProtectionsOK2 = _protectionsController.ProtectionsOK2;
            _control.ProtectionsCanceled2 = _protectionsController.ProtectionsCanceled2; 
        }
        private void BusControl()
        {            
            _control.BusOk = _bus.IsBusOK();
            _busOkOut.Activate(_control.BusOk);           
        }
        private void CommandActivatedSignalsControl()
        {
            foreach (IActivable act in _commandActivatedSignals)
                act.Activate(_control.ConnectionCommand);
        }
        private void EmergencyActivatedSignalsControl()
        {          
            foreach (var signal in _emergencyActivatedSignals)
                signal.Activate(EmergencyActuated());
        }     
    }
}