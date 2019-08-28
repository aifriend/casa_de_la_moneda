using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;


namespace Idpsa.Control.Component
{
    [Serializable]
    public class Cylinders1EfectBypassed : Cylinder
    {
        private readonly IActivable _actuatorWork;
        private readonly IEvaluable _sensorRest1;
        private readonly IEvaluable _sensorRest2;
        private readonly IEvaluable _sensorWork1;
        private readonly IEvaluable _sensorWork2;

        public Cylinders1EfectBypassed(IEvaluable sensorRest1, IEvaluable sensorWork1, IEvaluable sensorRest2,
                                          IEvaluable sensorWork2, IActivable actuatorWork, string name) : base(name)
        {
            if (sensorRest1 == null)
                throw new ArgumentNullException("sensorRest1");
            if (sensorWork1 == null)
                throw new ArgumentNullException("sensorWork1");
            if (sensorRest2 == null)
                throw new ArgumentNullException("sensorRest2");
            if (sensorWork2 == null)
                throw new ArgumentNullException("sensorWork2");
 
            _sensorRest1 = sensorRest1;
            _sensorWork1 = sensorWork1;
            _sensorRest2 = sensorRest2;
            _sensorWork2 = sensorWork2;
            _actuatorWork = actuatorWork;
        }

        public Cylinders1EfectBypassed(IEvaluable sensorRest1, IEvaluable sensorWork1, IEvaluable sensorRest2,
                                          IEvaluable sensorWork2, IActivable actuatorWork)
            : this(sensorRest1, sensorWork1, sensorRest2, sensorWork2, actuatorWork, String.Empty){}
        
        public override bool InRest
        {
            get
            {
                return (_sensorRest1.Value() & !_sensorWork2.Value()) &&
                       (_sensorRest2.Value() & !_sensorWork2.Value());
            }
        }

        public override bool InWork
        {
            get
            {
                return (!_sensorRest1.Value() & _sensorWork2.Value()) &&
                       (!_sensorRest2.Value() & _sensorWork2.Value());
            }
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
            var generalManual1 = new GeneralManual();
            var generalManual2 = new GeneralManual();

            var actuatorWork = _actuatorWork;

            generalManual1.LimitSwithRest = _sensorRest1.ToStringWithElapsedTime(ref actuatorWork);
            generalManual1.LimitSwithWork = _sensorWork1.ToStringWithElapsedTime(ref actuatorWork);
            generalManual2.LimitSwithRest = _sensorRest2.ToStringWithElapsedTime(ref actuatorWork);
            generalManual2.LimitSwithWork = _sensorWork2.ToStringWithElapsedTime(ref actuatorWork);
            generalManual1.ActuatorWrk = actuatorWork;

            return new[] {new Manual(generalManual1), new Manual(generalManual2)};
        }
    }
}