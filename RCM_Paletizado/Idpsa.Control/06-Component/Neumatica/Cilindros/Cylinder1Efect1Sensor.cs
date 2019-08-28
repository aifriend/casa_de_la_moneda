using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;
using Idpsa.Control;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Cylinder1Efect1Sensor : Cylinder
    {
        private readonly IActivable _actuatorWork;
        private readonly IEvaluable _sensorWork;

        public Cylinder1Efect1Sensor(IEvaluable sensorWork, IActivable actuatorWork, string name)
            : base(name)
        {
            if (sensorWork == null)
                throw new ArgumentNullException("sensorWork");
            if (actuatorWork == null)
                throw new ArgumentNullException("actuatorWork");
            _sensorWork = sensorWork;
            _actuatorWork = actuatorWork;
        }

        public Cylinder1Efect1Sensor(IEvaluable sensorWork, IActivable actuatorWork)
            : this(sensorWork, actuatorWork, String.Empty){}

        public override bool InRest
        {
            get { return !_sensorWork.Value(); }
        }

        public override bool InWork
        {
            get { return _sensorWork.Value(); }
        }

        public IEvaluable SensorTrabajo
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
            generalManual.LimitSwithWork = _sensorWork.ToStringWithElapsedTime(ref actuatorWork);
            generalManual.ActuatorWrk = actuatorWork;

            return new[] {new Manual(generalManual)};
        }
    }
}