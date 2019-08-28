using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [System.Serializable()]
    public abstract class Actuador : IActuador,IManualsProvider 
    {        
        public string Name{get;private set;}
        protected IBitActivable OutPut{get;private set;}

        private Func<bool> _manualBasEnable;
        private Func<bool> _manualWorkEnable;
        private string _workName;
        private string _basName;   
        
        private IBitEvaluable _activatedSignal{get;set;}
     


        public Actuador(IBitActivable actuador, string name)
        {
            OutPut = actuador;
            Name = name;
            _manualBasEnable = () => true;
            _manualWorkEnable = () => true;
            _workName = "Arrancar";
            _basName = "Parar";
        }
        public Actuador(IBitActivable actuador) : this(actuador, (actuador!=null)? actuador.ToString():"") { }

        

        public abstract void Activate(bool work);
      
        public abstract void ActivateSecure(bool work);
        
        public virtual bool Value()
        {
            return OutPut.Value();
        }

        public virtual bool SecureValue()
        {
            bool value = false;
            if (OutPut == null)
                value = false;
            else
                value = Value();

            return Value();
        }

        public override string ToString()
        {
            return Name;
        }

        public Actuador WithActivatedFeedbackSignal(IBitEvaluable activatedSignal)
        {
            if (activatedSignal == null)
                throw new ArgumentNullException("activatedSignal");
            _activatedSignal = activatedSignal;
            return this;
        }


        public Actuador WithManualBasEnable(Func<bool> manualBasEnable)
        {
            _manualBasEnable = manualBasEnable;
            return this;
        }

        public Actuador WithManualWorkEnable(Func<bool> manualWorkEnable)
        {
            _manualWorkEnable = manualWorkEnable;
            return this;
        }

        public Actuador WithManualEnable(Func<bool> manualEnable)
        {
            _manualWorkEnable = _manualBasEnable = manualEnable;
            return this;
        }

        public Actuador WithBasName(string basName)
        {
            _basName = basName;
            return this;
        }

        public Actuador WithWorkName(string workName)
        {
            _workName = workName;
            return this;
        }

        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            return GetManualsRepresentationsCore();
        }

        protected virtual IEnumerable<Manual> GetManualsRepresentationsCore()
        {
            var generalManual = new GeneralManual();
            generalManual.FinalesCarreraWrk[0] = _activatedSignal;
            generalManual.ActuadoresWrk[0] = (IBitActivable)this;
            generalManual.BasEnable = _manualBasEnable;
            generalManual.WorkEnable = _manualWorkEnable;
            generalManual.BasName = _basName;
            generalManual.WorkName = _workName;
            var manual = new Manual(generalManual);
            if (String.IsNullOrEmpty(manual.Descripcion))
            {
                manual.Descripcion = Name;
            }
            return new[] { manual };
        }

        #endregion
    }
}
