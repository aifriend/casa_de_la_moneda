using System;
using Idpsa.Control.Timing;

namespace Idpsa.Control
{
    [Serializable]
    public class TON
    {
        private readonly TimerCounter _counter;
        private bool _enableAnt;
        private bool _input;        
        private bool _Q;
        private int _timeBeforeStop;
        private int _timeToPass;

        public TON()
        {
            _counter = new TimerCounter();
        }

        public bool Started { get { return _counter.Started; } }

        public bool Input
        {
            get { return _input; }
            set
            {
                if (value & !_input)
                {
                    _counter.Reset();
                    _counter.Start();
                }
                _input = value;
            }
        }

        public int TimeToPass
        {
            get { return _timeToPass; }
            set { _timeToPass = value; }
        }

        public bool Q
        {
            get
            {
                _Q = _counter.Value >= _timeToPass;
                return _Q;
            }
        }

        public int ElapsedTime
        {
            get { return (int)_counter.Value; }
        }
       
        public void Reset()
        {
            _enableAnt = false;
            _input=false;       
            _Q=false;
            _timeBeforeStop = 0;
            _timeToPass =0;
            _counter.Reset();
        }

        public void Start()
        {
            Reset();
            Input = true;
        }

        public bool Timing(int Time)
        {
            return Timing(Time, true);
        }

        public bool Timing(int Time, bool enable)
        {
            Input = enable;
            TimeToPass = Time;
            bool value = Q;
            if (value)
            {
                Input = false;
            }
            return value;
        }
        
        public bool TimingWithReset(int Time, bool enable)
        {
            if (enable && _input == false)
            {
                _counter.Reset();
                _counter.Start();
            }
            else if (enable == false)
            {
                _counter.Reset();
                _counter.Start();
            }
            _input = enable;
            TimeToPass = Time;
            bool value = Q;
            if ((value && enable))
            {
                Input = false;
            }
            return (value & enable);
        }

        public bool TimingWithStop(int Time, bool enable)
        {
            if (enable == false & _enableAnt)
            {
                _timeBeforeStop = _timeBeforeStop + ElapsedTime;
            }
            _enableAnt = enable;
            Input = enable;
            if (enable)
            {
                TimeToPass = Time - _timeBeforeStop;
            }
            bool value = Q;
            if ((value & enable))
            {
                Input = false;
                _timeBeforeStop = 0;
                _enableAnt = false;
            }
            return value & enable;
        }
    }
}