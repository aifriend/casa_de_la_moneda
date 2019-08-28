using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;


namespace Idpsa.Control.Component
{
    [Serializable]
    public class Cylinder1Efect : Cylinder
    {        
        private readonly IEvaluable _sensorRest;
        private readonly IEvaluable _sensorWork;
        private readonly IActivable _actuatorWork;

        public Cylinder1Efect(IEvaluable sensorRest, IEvaluable sensorWork, IActivable actuatorWork, string name)
            : base(name)
        {
            if (sensorRest == null)
                throw new ArgumentNullException("sensorRest");
            if (sensorWork == null)
                throw new ArgumentNullException("sensorWork");
            if (actuatorWork == null)
                throw new ArgumentNullException("actuatorWork");

            _sensorRest = sensorRest;
            _sensorWork = sensorWork;
            _actuatorWork = actuatorWork;
        }

        public Cylinder1Efect(IEvaluable sensorRest, IEvaluable sensorWork, IActivable actuatorWork)
            : this(sensorRest, sensorWork, actuatorWork, "") { }

        public override bool InRest
        {
            get { return !_sensorWork.Value() & _sensorRest.Value(); }
        }

        public override bool InWork
        {
            get { return _sensorWork.Value() & !_sensorRest.Value(); }
        }

        public IEvaluable SensorRest
        {
            get { return _sensorRest; }
        }

        public IEvaluable SensorWork
        {
            get { return _sensorWork; }
        }

        public override void Rest()
        {
            _actuatorWork.Activate(false);
        }

        public override void Work()
        {
            _actuatorWork.Activate(true);
        }

        public override void Dead()
        {
            _actuatorWork.Activate(false);
        }

        protected override IEnumerable<Manual> GetPartialManualRepresentations()
        {
            var generalManual = new GeneralManual();

            var actuatorWork = _actuatorWork; 
            generalManual.LimitSwithRest = _sensorRest.ToStringWithElapsedTime(ref actuatorWork);
            generalManual.LimitSwithWork = _sensorWork.ToStringWithElapsedTime(ref actuatorWork);
            generalManual.ActuatorWrk = actuatorWork;

            return new[] { new Manual(generalManual) };
        }

    }
}