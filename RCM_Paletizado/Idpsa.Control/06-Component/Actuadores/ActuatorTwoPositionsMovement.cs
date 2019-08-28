using System;
using System.Linq;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    public class ActuatorTwoPositionsMovement:ActuatorWithInversor 
    {
        private enum Zones
        {
            NotDefined,
            Pos1,
            Pos2,
            Intermediate,
            NextPos1,
            NextPos2
        }

        private Zones _zone;
        private ActuatorWithInversor _actuator;        
        private Sensor _sensorPos1;
        private Sensor _nextSensorPos1;
        private Sensor _sensorPos2;
        private Sensor _nextSensorPos2;
        private bool _moveSense1Stoped;
        private bool _moveSense2Stoped;
        private Action _slowVelocity = () => { };
        private Action _fastVelocity = () => { };

        public ActuatorTwoPositionsMovement(string name, ActuatorWithInversor actuator,
                                           Sensor sensorPos1, Sensor nextSensorPos1,
                                           Sensor sensorPos2, Sensor nextSensorPos2):base(name)
        {
            if (actuator == null)
                throw new ArgumentNullException("actuator");
            if (sensorPos1 == null)
                throw new ArgumentNullException("sensorPos1");
            if (nextSensorPos1 == null)
                throw new ArgumentNullException("nextSensorPos1");
            if (sensorPos2 == null)
                throw new ArgumentNullException("sensorPos2");
            if (nextSensorPos2 == null)
                throw new ArgumentNullException("nextSensorPos2");

            _actuator = actuator;
            _sensorPos1 = sensorPos1;
            _nextSensorPos1 = nextSensorPos1;
            _sensorPos2 = sensorPos2;
            _nextSensorPos2 = nextSensorPos2;

            ISlowFastVelocity slowfastVelocity = _actuator as ISlowFastVelocity;
            if (slowfastVelocity != null)
            {
                _slowVelocity = slowfastVelocity.SlowVelocity;
                _fastVelocity = slowfastVelocity.FastVelocity;
            }            
        }

        public override void Activate1()
        {
            Move1();
        }

        public override void Activate2()
        {
            Move2();
        }

        public override void Deactivate()
        {
            Stop();
        }

        public bool Move1()
        {
            if (_moveSense1Stoped)
            {
                Stop();
                return (_zone == Zones.Pos1);
            }
            
            if (_sensorPos1.Value())
            {
                _moveSense1Stoped = true;
                _zone = Zones.Pos1;
                Stop();
                return true;                
            }
            
            if (_nextSensorPos1.Value())
            {
                _zone = Zones.NextPos1;
            }
            else if (_nextSensorPos2.Value())
            {
                _zone = Zones.Intermediate;
            }

            if (_zone == Zones.NotDefined || _zone == Zones.NextPos1)
            {
                _slowVelocity();
            }
            else
            {
                _fastVelocity();
            }

            _moveSense2Stoped = false;
            _actuator.Activate1();

            return false;
        }

        public bool Move2()
        {

            if (_moveSense2Stoped)
            {
                Stop();
                return (_zone == Zones.Pos2);
            }
            
            if (_sensorPos2.Value())
            {
                _moveSense2Stoped = true;
                _zone = Zones.Pos2;
                Stop();
                return true;
            }
            
            if (_nextSensorPos2.Value())
            {
                _zone = Zones.NextPos2;
            }
            else if (_nextSensorPos1.Value())
            {
                _zone = Zones.Intermediate;
            }

            if (_zone == Zones.NotDefined || _zone == Zones.NextPos2)
            {
                _slowVelocity();
            }
            else
            {
                _fastVelocity();
            }

            _moveSense1Stoped = false;
            _actuator.Activate2();

            return false;
        }

        private void Stop()
        {
            _slowVelocity();
            _actuator.Deactivate();
        }

        protected override IEnumerable<Manual> GetManualRepresentationsCore()
        {
            Manual manual = base.GetManualRepresentationsCore().ElementAt(0);
            Manual manual1 = ((IManualsProvider)(_sensorPos1))
                .GetManualsRepresentations().ElementAt(0);
            Manual manual2 = ((IManualsProvider)(_sensorPos2))
                .GetManualsRepresentations().ElementAt(0);
            Manual manual3 = ((IManualsProvider)(_nextSensorPos1))
                .GetManualsRepresentations().ElementAt(0);
            Manual manual4 = ((IManualsProvider)(_nextSensorPos2))
               .GetManualsRepresentations().ElementAt(0);

            manual1.SuperGroup = manual2.SuperGroup = manual3.SuperGroup = manual4.SuperGroup = manual.Group;
            manual1.Group = manual2.Group = manual3.Group = manual4.Group = manual.Group;

            if (_actuator is ISlowFastVelocity)
            {
                return new[] { manual, manual1, manual2, manual3, manual4 };
            }
            else
            {
                return new[] { manual, manual1, manual2};
            }
        }

             
    }
}