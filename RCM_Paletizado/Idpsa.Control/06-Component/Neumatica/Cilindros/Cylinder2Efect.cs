using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Cylinder2Efect : Cylinder
    {       
        private readonly IEvaluable _sensorRest;
        private readonly IEvaluable _sensorWork;
        private readonly IActivable _actuatorRest;
        private readonly IActivable _actuatorWork;

        public Cylinder2Efect(IEvaluable sensorRest, IEvaluable sensorWork, IActivable actuatorRest,
                               IActivable actuatorWork, string name) : base(name)
        {
            if (sensorRest == null)
                throw new ArgumentNullException("sensorRest");
            if (sensorWork == null)
                throw new ArgumentNullException("sensorWork");
            if (actuatorRest == null)
                throw new ArgumentNullException("actuatorRest");
            if (actuatorWork == null)
                throw new ArgumentNullException("actuatorWork");
 
            _sensorRest = sensorRest;
            _sensorWork = sensorWork;
            _actuatorRest = actuatorRest;
            _actuatorWork = actuatorWork;
        }

        public Cylinder2Efect(IEvaluable sensorRest, IEvaluable sensorWork, IActivable actuatorRest,
                               IActivable actuatorWork)
            : this(sensorRest, sensorWork, actuatorRest, actuatorWork, String.Empty){}

        public override bool InRest
        {
            get { return _sensorRest.Value() & !_sensorWork.Value(); }
        }

        public override bool InWork
        {
            get { return _sensorWork.Value() & !_sensorRest.Value(); }
        }

        public IEvaluable SensorReposo
        {
            get { return _sensorRest; }
        }

        public IEvaluable SensorTrabajo
        {
            get { return _sensorWork; }
        }

        public override void Rest()
        {
            _actuatorRest.Activate(true);
            _actuatorWork.Activate(false);
        }

        public override void Work()
        {
            _actuatorRest.Activate(false);
            _actuatorWork.Activate(true);
        }

        public override void Dead()
        {
            _actuatorRest.Activate(false);
            _actuatorWork.Activate(false);
        }

        protected override IEnumerable<Manual> GetPartialManualRepresentations()
        {
            var generalManual = new GeneralManual();

            var actuatorRest = _actuatorRest;
            var actuatorWork = _actuatorWork;
            generalManual.LimitSwithRest = _sensorRest.ToStringWithElapsedTime(ref actuatorRest);
            generalManual.LimitSwithWork = _sensorWork.ToStringWithElapsedTime(ref actuatorWork);
            generalManual.ActuatorRest = actuatorRest;
            generalManual.ActuatorWrk = actuatorWork;

            return new[] {new Manual(generalManual)};
        }
    }
}