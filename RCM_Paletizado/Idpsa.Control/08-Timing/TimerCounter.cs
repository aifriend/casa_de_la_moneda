using System;

namespace Idpsa.Control.Timing
{
    [Serializable]
    public class TimerCounter
    {
        private double _dblCounterEnd;
        private double _dblCounterIni = -1;
        private double _dblCounterValue;

        public bool Started { get; set; }

        public double Value
        {
            get
            {
                if (_dblCounterIni == -1)
                    return 0.0;

                _dblCounterValue = 0;
                _dblCounterEnd = Environment.TickCount;
                if (_dblCounterEnd < _dblCounterIni)
                {
                    _dblCounterEnd = _dblCounterEnd + Int32.MaxValue;
                }
                _dblCounterValue = _dblCounterEnd - _dblCounterIni;
                return _dblCounterValue;
            }
        }

        public void Reset()
        {
            _dblCounterIni = -1;
            _dblCounterValue = 0;
            _dblCounterEnd = 0;
            Started = false;
        }

        public void Start()
        {
            _dblCounterIni = Environment.TickCount;
            Started = true;
        }
    }
}