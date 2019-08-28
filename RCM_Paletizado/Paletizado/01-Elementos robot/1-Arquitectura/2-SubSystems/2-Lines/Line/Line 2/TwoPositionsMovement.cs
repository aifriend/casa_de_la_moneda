using System;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals;

namespace Idpsa.Paletizado
{
    public class ActuatorTwoPositionsMovement : ActuatorWithInversor
    {
        private readonly ActuatorWithInversor _actuator;
        private readonly Action _fastVelocity = () => { };
        private readonly Sensor _nextSensorPos1;
        private readonly Sensor _nextSensorPos2;
        private readonly Sensor _sensorPos1;
        private readonly Sensor _sensorPos2;
        private readonly Action _slowVelocity = () => { };
        private bool _moveSense1Stoped;
        private bool _moveSense2Stoped;
        private Zones _zone;

        public ActuatorTwoPositionsMovement(string name, ActuatorWithInversorSimple actuator,
                                            Sensor sensorPos1, Sensor nextSensorPos1,
                                            Sensor sensorPos2, Sensor nextSensorPos2) : base(name)
        {
            _actuator = actuator;
            _sensorPos1 = sensorPos1;
            _nextSensorPos1 = nextSensorPos1;
            _sensorPos2 = sensorPos2;
            _nextSensorPos2 = nextSensorPos2;

            var slowfastVelocity = _actuator as ISlowFastVelocity;
            if (slowfastVelocity != null)
            {
                _slowVelocity = slowfastVelocity.SlowVelocity;
                _fastVelocity = slowfastVelocity.FastVelocity;
            }
        }

        public bool IsDown
        {
            get { return _sensorPos2.Value(); }
        }

        public bool IsUp
        {
            get { return _sensorPos1.Value(); }
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
            else if (_nextSensorPos1.Value())
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
            else if (_nextSensorPos2.Value())
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

        //MDG.2011-06-02.Orden de parada externa para poder parar ascensor a conveniencia, no sólo con move1 o move1
        public void StopMove()
        {
            Stop();
        }

        private void Stop()
        {
            _slowVelocity();
            _actuator.Deactivate();
        }

        protected override IEnumerable<Manual> GetManualRepresentationsCore()
        {
            Manual manual = base.GetManualRepresentationsCore().ElementAt(0);
            Manual manual1 = ((IManualsProvider) (_sensorPos1))
                .GetManualsRepresentations().ElementAt(0);
            Manual manual2 = ((IManualsProvider) (_sensorPos2))
                .GetManualsRepresentations().ElementAt(0);
            Manual manual3 = ((IManualsProvider) (_nextSensorPos1))
                .GetManualsRepresentations().ElementAt(0);
            Manual manual4 = ((IManualsProvider) (_nextSensorPos2))
                .GetManualsRepresentations().ElementAt(0);

            manual1.SuperGroup = manual2.SuperGroup = manual3.SuperGroup = manual4.SuperGroup = manual.Group;
            manual1.Group = manual2.Group = manual3.Group = manual4.Group = manual.Group;

            if (_actuator is ISlowFastVelocity)
            {
                return new[] {manual, manual1, manual2, manual3, manual4};
            }
            else
            {
                return new[] {manual, manual1, manual2};
            }
        }

        #region Nested type: Zones

        private enum Zones
        {
            NotDefined,
            Pos1,
            Pos2,
            Intermediate,
            NextPos1,
            NextPos2
        }

        #endregion
    }
}