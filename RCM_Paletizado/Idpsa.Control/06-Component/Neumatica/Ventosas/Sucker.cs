using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Sucker : IManualsProvider
    {
        private readonly IActivable _vaccumActuator;
        private readonly IEvaluable _vaccumSensor;
        private string _restName;
        private string _workName;
        private Func<bool> _manualRestEnable;
        private Func<bool> _manualWorkEnable;

        public Sucker(IEvaluable vaccumSensor, IActivable vaccumActuator, string name)
        {
            if (vaccumSensor == null)
                throw new ArgumentNullException("vaccumSensor");
            if (vaccumActuator == null)
                throw new ArgumentNullException("vaccumActuator");

            _vaccumSensor = vaccumSensor;
            _vaccumActuator = vaccumActuator;
            Name = name;

            _manualRestEnable = () => true;
            _manualWorkEnable = () => true;
            _workName = "Aspirar";
            _restName = "Parar";
        }



        public Sucker(IEvaluable vaccumSensor, IActivable vaccumActuator)
            : this(vaccumSensor, vaccumActuator, String.Empty) { }

        public string Name { get; private set; }

        public void Vacio(bool Orden)
        {
            _vaccumActuator.Activate(true);
        }

        public void VaccumOn()
        {
            _vaccumActuator.Activate(true);
        }

        public void VaccumOff()
        {
            _vaccumActuator.Activate(false);
        }

        public bool InVaccum()
        {
            return _vaccumSensor.Value();
        }

        public Sucker WithManualRestEnable(Func<bool> manualRestEnable)
        {
            if (manualRestEnable == null)
                throw new ArgumentNullException("manualRestEnable");
            _manualRestEnable = manualRestEnable;
            return this;
        }

        public Sucker WithManualWorkEnable(Func<bool> manualWorkEnable)
        {
            if (manualWorkEnable == null)
                throw new ArgumentNullException("manualWorkEnable");
            _manualWorkEnable = manualWorkEnable;
            return this;
        }

        public Sucker WithManualEnable(Func<bool> manualEnable)
        {
            if (manualEnable == null)
                throw new ArgumentNullException("manualEnable");
            _manualWorkEnable = _manualRestEnable = manualEnable;
            return this;
        }

        public Sucker WithRestName(string restName)
        {
            if (String.IsNullOrEmpty(restName))
                throw new ArgumentException("restName can't be null or empty");
            _restName = restName;
            return this;
        }

        public Sucker WithWorkName(string workName)
        {
            if (String.IsNullOrEmpty(workName))
                throw new ArgumentException("workName can't be null or empty");
            this._workName = workName;
            return this;
        }

        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            var generalManual = new GeneralManual();
            generalManual.ActuatorWrk = _vaccumActuator;

            if (!(_vaccumSensor is SensorSimulated))
            {
                generalManual.LimitSwithWork = _vaccumSensor;
            }

            generalManual.RestEnable = _manualRestEnable;
            generalManual.WorkEnable = _manualWorkEnable;
            generalManual.RestName = _restName;
            generalManual.WorkName = _workName;
           
            var manual = new Manual(generalManual);
            if (String.IsNullOrEmpty(manual.Description))
                manual.Description = Name;

            return new[] {manual};
        }

        #endregion
    }
}