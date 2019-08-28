using System;

namespace Idpsa.Control.Timing
{
    [Serializable]
    public class R_TRIG
    {
        private bool _cLK;
        private bool _M;
        private bool _Q;

        public bool CLK
        {
            get { return _cLK; }
            set { _cLK = value; }
        }

        public bool Q
        {
            get
            {
                _Q = _cLK & !_M;
                _M = _cLK;
                return _Q;
            }
        }
    }
}