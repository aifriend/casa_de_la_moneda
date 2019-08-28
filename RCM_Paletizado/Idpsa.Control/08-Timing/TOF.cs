using System;
using Idpsa.Control;
using Idpsa.Control.Timing;

namespace Idpsa.Control
{
    [Serializable]
    public class TOF
    {
        //Condition
        private TimerCounter _counter = new TimerCounter();
        private bool _input;
        //Tiempo a transcurrir
        // Tiempo cumplido
        private bool _QLocal;
        private int _timeToPass;

        public bool Started { get { return _counter.Started; } }

        public bool Input
        {
            get { return _input; }
            set
            {
                if ((value == false) & _input)
                {
                    _counter.Reset();
                    _counter.Start();
                    _input = false;
                }
                else
                {
                    if (value)
                    {
                        _input = true;
                    }
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
                _QLocal = _counter.Value >= _timeToPass;
                return _QLocal;
            }
        }

        public int ElapsedTime
        {
            get { return (int)_counter.Value; }
        }

        public bool Timing(int Time)
        {
            return Temporization(Time, false);
        }

        public bool Temporization(int Time, bool enable)
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
    }
}