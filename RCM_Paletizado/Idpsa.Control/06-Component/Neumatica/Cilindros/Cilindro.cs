using System;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [Serializable]
    public abstract class Cilindro : ICilindro, IManualsProvider
    {
        #region Posicion enum

        public enum Posicion
        {
            SinPosicion,
            Reposo,
            Trabajo
        }

        #endregion

        private string _basName;

        private Func<bool> _manualBasEnable;
        private Func<bool> _manualWorkEnable;
        private string _workName;

        protected Cilindro(string name)
        {
            Name = name;
            _manualBasEnable = () => true;
            _manualWorkEnable = () => true;
            _workName = "Trabajo";
            _basName = "Reposo";
        }

        #region ICilindro Members

        public string Name { get; private set; }

        public abstract bool EnReposo { get; }
        public abstract bool EnTrabajo { get; }

        public abstract void Reposo();
        public abstract void Trabajo();
        public abstract void Muerto();

        #endregion

        public Cilindro WithManualBasEnable(Func<bool> manualBasEnable)
        {
            _manualBasEnable = manualBasEnable;
            return this;
        }

        public Cilindro WithManualWorkEnable(Func<bool> manualWorkEnable)
        {
            _manualWorkEnable = manualWorkEnable;
            return this;
        }

        public Cilindro WithManualEnable(Func<bool> manualEnable)
        {
            _manualWorkEnable = _manualBasEnable = manualEnable;
            return this;
        }

        public Cilindro WithBasName(string basName)
        {
            _basName = basName;
            return this;
        }

        public Cilindro WithWorkName(string workName)
        {
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
                    generalManual.BasEnable = _manualBasEnable;
                    generalManual.WorkEnable = _manualWorkEnable;
                    generalManual.BasName = _basName;
                    generalManual.WorkName = _workName;
                    if (String.IsNullOrEmpty(manual.Descripcion))
                    {
                        manual.Descripcion = Name;
                    }
                }

                int i = 0;
                if (manuals.Count() > 1)
                {
                    foreach (Manual manual in manuals)
                    {
                        i++;
                        manual.Descripcion += " " + i;
                    }
                }
            }
            return manuals;
        }

        #endregion
    }
}