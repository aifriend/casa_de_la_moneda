using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [System.Serializable()]
    public abstract class Actuator : IActivable, IManualsProvider 
    {        
        public string Name{get;private set;}
        public string Description { get; private set; }
        protected IActivable Out{get;private set;}

        private Func<bool> _manualRestEnable;
        private Func<bool> _manualWorkEnable;
        private string _workName;
        private string _restName;
       
        private IEvaluable _activatedSignal{get;set;}    


        public Actuator(IActivable actuator, string description)
        {
            if (actuator == null)
                throw new ArgumentNullException("actuator");
            if (description == null)
                throw new ArgumentNullException("description");
            
            Out = actuator;
            Description = description;
            _manualRestEnable = () => true;
            _manualWorkEnable = () => true;
            _workName = "Arrancar";
            _restName = "Parar";
            Name = actuator.ToString() ?? String.Empty;
        }

        public Actuator(IActivable actuator) : this(actuator, String.Empty) { }

        public Actuator(string description)
        {
            if (description == null)
                throw new ArgumentNullException("description");
            Description = description;
            Out = Output.Simulated;
            Name = String.Empty;
        }

        public abstract void Activate(bool work);
     
        public virtual bool Value()
        {
            return Out.Value();
        }

        public override string ToString()
        {
            return Name;
        }

        public Actuator WithActivatedFeedbackSignal(IEvaluable activatedSignal)
        {
            if (activatedSignal == null)
                throw new ArgumentNullException("activatedSignal");
            _activatedSignal = activatedSignal;
            return this;
        }

        public Actuator WithManualRestEnable(Func<bool> manualRestEnable)
        {
            if (manualRestEnable == null)
                throw new ArgumentNullException("manualRestEnable");
            _manualRestEnable = manualRestEnable;
            return this;
        }

        public Actuator WithManualWorkEnable(Func<bool> manualWorkEnable)
        {
            if (manualWorkEnable == null)
                throw new ArgumentNullException("manualWorkEnable");   
            _manualWorkEnable = manualWorkEnable;
            return this;
        }

        public Actuator WithManualEnable(Func<bool> manualEnable)
        {
            if (manualEnable == null)
                throw new ArgumentNullException("manualEnable");    
            _manualWorkEnable = _manualRestEnable = manualEnable;
            return this;
        }

        public Actuator WithRestName(string restName)
        {
            if (String.IsNullOrEmpty(restName))
                throw new ArgumentException("restName can't be null or empty");
            _restName = restName;
            return this;
        }

        public Actuator WithWorkName(string workName)
        {
            if (String.IsNullOrEmpty(workName))
                throw new ArgumentException("workName can't be null or empty");
            this._workName = workName;
            return this;
        }
             

        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            return GetManualsRepresentationsCore();
        }

        protected virtual IEnumerable<Manual> GetManualsRepresentationsCore()
        {
            var generalManual = new GeneralManual
                                    {
                                        LimitSwithWork = _activatedSignal,
                                        ActuatorWrk = this,
                                        RestEnable = _manualRestEnable,
                                        WorkEnable = _manualWorkEnable,
                                        RestName = _restName,
                                        WorkName = _workName
                                    };
            var manual = new Manual(generalManual) {Description = Description};
            return new[] { manual };
        }

        #endregion
             
        
    }
}
