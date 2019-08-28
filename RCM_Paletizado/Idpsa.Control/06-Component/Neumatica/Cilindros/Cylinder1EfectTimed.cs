using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Cylinder1EfectTimed : Cylinder
    {
        private readonly IActivable _actuatorWork;
        private readonly int _timeRest;
        private readonly TON _timerRest;
        private readonly TON _timerWork;
        private readonly int _timeWork;

        public Cylinder1EfectTimed(int timeRest, int timeWork, IActivable actuatorWork, string name)
            : base(name)
        {
            if (timeRest < 0)            
                throw new ArgumentException("timeRest can't be negative");
            if (timeWork < 0)
                throw new ArgumentException("timeWork can't be negative");
            if (actuatorWork == null)
                throw new ArgumentNullException("actuatorWork"); 
           
            _timeRest = timeRest;
            _timeWork = timeWork;
            _actuatorWork = actuatorWork;
            _timerRest = new TON();
            _timerWork = new TON();
        }

        public Cylinder1EfectTimed(int timeRest, int timeWork, IActivable actuatorWork)
            : this(timeRest, timeWork, actuatorWork, String.Empty)
        {
        }

        public override bool InRest
        {
            get { return _timerRest.Timing(_timeRest); }
        }

        public override bool InWork
        {
            get { return _timerWork.Timing(_timeWork); }
        }

        public override void Rest()
        {
            _actuatorWork.Activate(false);
            _timerRest.Reset();
        }

        public override void Work()
        {
            _actuatorWork.Activate(true);
            _timerWork.Reset();
        }

        public override void Dead()
        {
            _actuatorWork.Activate(false); 
        }


        protected override IEnumerable<Manual> GetPartialManualRepresentations()
        {           
            var generalManual = new GeneralManual();
            
            generalManual.ActuatorWrk = _actuatorWork;

            return new[] {new Manual(generalManual)};
        }
    }
}