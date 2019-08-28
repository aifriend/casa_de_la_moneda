using System;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [Serializable]
    public abstract class Cylinder : ICylinder, IManualsProvider
    {        
        private string _restName;
        private string _workName;

        private Func<bool> _manualRestEnable;
        private Func<bool> _manualWorkEnable;        

        protected Cylinder(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name"); 
            Name = name;
            _manualRestEnable = () => true;
            _manualWorkEnable = () => true;
            _workName = "Trabajo";
            _restName = "Reposo";
        }

        #region ICilindro Members

        public string Name { get; private set; }

        public abstract bool InRest { get; }
        public abstract bool InWork { get; }

        public abstract void Rest();
        public abstract void Work();
        public abstract void Dead();

        #endregion

        public Cylinder WithManualRestEnable(Func<bool> manualRestEnable)
        {
            if (manualRestEnable == null)
                throw new ArgumentNullException("manualRestEnable"); 
            _manualRestEnable = manualRestEnable;
            return this;
        }

        public Cylinder WithManualWorkEnable(Func<bool> manualWorkEnable)
        {
            if (manualWorkEnable == null)
                throw new ArgumentNullException("manualWorkEnable");
            _manualWorkEnable = manualWorkEnable;
            return this;
        }

        public Cylinder WithManualEnable(Func<bool> manualEnable)
        {
            if (manualEnable == null)
                throw new ArgumentNullException("manualEnable");
            _manualWorkEnable = _manualRestEnable = manualEnable;
            return this;
        }

        public Cylinder WithRestName(string restName)
        {
            if (restName == null)
                throw new ArgumentNullException("restName");
            _restName = restName;
            return this;
        }

        public Cylinder WithWorkName(string workName)
        {
            if (workName == null)
                throw new ArgumentNullException("workName");
            _workName = workName;
            return this;
        }

        protected abstract IEnumerable<Manual> GetPartialManualRepresentations();

        #region Miembros de IManualProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            IEnumerable<Manual> manuals = GetPartialManualRepresentations();
            if (manuals != null)
            {
                foreach (Manual manual in manuals)
                {
                    var generalManual = (GeneralManual)manual.RepresentedInstance;
                    generalManual.RestEnable = _manualRestEnable;
                    generalManual.WorkEnable = _manualWorkEnable;
                    generalManual.RestName = _restName;
                    generalManual.WorkName = _workName;
                    if (String.IsNullOrEmpty(manual.Description))
                    {
                        manual.Description = Name;
                    }
                }

                int i = 0;
                if (manuals.Count() > 1)
                {
                    foreach (Manual manual in manuals)
                    {
                        i++;
                        manual.Description += " " + i;
                    }
                }
            }
            return manuals;
        }

        #endregion
    }
}