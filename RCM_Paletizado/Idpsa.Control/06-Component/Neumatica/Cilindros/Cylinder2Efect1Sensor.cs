using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;


namespace Idpsa.Control.Component
{
    [Serializable]
    public class Cylinder2Efect1Sensor : Cylinder
    {       
        private readonly IEvaluable _sensorWork;
        private readonly IActivable _actuatorRest;
        private readonly IActivable _actuatorWork;

        public Cylinder2Efect1Sensor(IEvaluable sensorWork, IActivable actuatorRest, IActivable actuatorWork,
                                      string name)
            : base(name)
        {
            if (sensorWork == null)
                throw new ArgumentNullException("sensorWork");
            if (actuatorRest == null)
                throw new ArgumentNullException("actuatorRest");
            if (actuatorWork == null)
                throw new ArgumentNullException("actuatorWork");
            
            _sensorWork = sensorWork;
            _actuatorRest = actuatorRest;
            _actuatorWork = actuatorWork;
        }

        public Cylinder2Efect1Sensor(IEvaluable sensorWork, IActivable actuatorRest, IActivable actuatorWork)
            : this(sensorWork, actuatorRest, actuatorWork, String.Empty){}

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

            var actuatorWork = _actuatorWork;
            generalManual.LimitSwithWork = _sensorWork.ToStringWithElapsedTime(ref actuatorWork);
            generalManual.ActuatorRest = _actuatorRest;
            generalManual.ActuatorWrk = actuatorWork;

            return new[] {new Manual(generalManual)};
        }
    }
}