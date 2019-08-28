using System;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals;

namespace Idpsa.Paletizado
{
    public class ActuatorTwoPositionsMovementMiddleStop : ActuatorWithInversor
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
        private bool _move2SlowSpeedBreak;//MDG.2012-10-30
        private int _countCyclesFastMove2Slow;//MDG.2012-10-30
        private Zones _zone;
        private ActuatorP _vSel;

        public ActuatorTwoPositionsMovementMiddleStop(string name, ActuatorWithInversorSimple actuator,
                                            Sensor sensorPos1, Sensor nextSensorPos1,
                                            Sensor sensorPos2, Sensor nextSensorPos2,
                                            ActuatorP parVSel) : base(name)
        {
            _actuator = actuator;
            _sensorPos1 = sensorPos1;
            _nextSensorPos1 = nextSensorPos1;
            _sensorPos2 = sensorPos2;
            _nextSensorPos2 = nextSensorPos2;
			_vSel = parVSel;

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
                SlowVelocity();
            }
            else
            {
                _fastVelocity();
                FastVelocity();
            }


           // FastVelocity();//MDG.2012-11-15
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
                _countCyclesFastMove2Slow = 0;//MDG.2012-10-30
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

            ////MDG.2012-10-30. Para conseguir velocidad lenta
            //if (_zone == Zones.NotDefined || _zone == Zones.NextPos2)
            //{
            //    SlowVelocity();

            //    _countCyclesFastMove2Slow += 1;
            //    if (_countCyclesFastMove2Slow>5)//25)//3)//25)
            //    {
            //        _move2SlowSpeedBreak = false;
            //        if (_countCyclesFastMove2Slow > 8)//26)//50)//26)//avanzamos durante 1 ciclos
            //        {
            //            _countCyclesFastMove2Slow = 0;
            //        }
            //    }
            //    else//paramos durante 4 ciclos
            //    {
            //        _move2SlowSpeedBreak = true;
            //    }

            //    //_move2SlowSpeedBreak = !_move2SlowSpeedBreak;
            //    if (_move2SlowSpeedBreak)
            //    {
            //        _moveSense1Stoped = false;
            //        Stop();
            //    }
            //    else
            //    {
            //        _moveSense1Stoped = false;
            //        _actuator.Activate2();
            //    }

            //}
            //else
            //{
            //    _countCyclesFastMove2Slow = 0;//MDG.2012-10-30
            //    FastVelocity();
            //    _moveSense1Stoped = false;
            //    _actuator.Activate2();
            //}

            //MDG.2013-01-14.Movimientos originales.Sin parada durante un ciclo al detectar sensor previo pos 2
            /////////////////
            if (
                //_zone == Zones.NotDefined || 
                _zone == Zones.NextPos2)
            {
                _slowVelocity();
                SlowVelocity();//MDG.2013-01-14.Siempre fast velocity excepto detectando con detector proximo.
            }
            else
            {
                _fastVelocity();
                FastVelocity();//MDG.2013-01-14.Siempre fast velocity excepto detectando con detector proximo.
            }
            //FastVelocity();//MDG.2013-01-14.Siempre fast velocity. Ya no se usa slow ni detector previo porque no hace falta y daban error en variador
            _moveSense1Stoped = false;
            _actuator.Activate2();
            /////////////////
             
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


        #region Miembros de ISlowFastVelocity

        public void SlowVelocity()
        {
            _vSel.Activate(false);
        }

        public void FastVelocity()
        {
            _vSel.Activate(true);
        }
        #endregion

    }
}