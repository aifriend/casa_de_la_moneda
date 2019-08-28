using System;

namespace Idpsa.Control.Sequence
{
    [Serializable]
    public class ChainComunicator
    {
        private bool _busy;
        private bool _ready;
        private bool _start;

        public void Reset()
        {
            _start = false;
            _ready = false;
            _busy = false;
        }

        public bool SendTask()
        {
            bool value = _ready && !_busy;
            if (value) _start = true;
            return value;
        }

        public bool ReceiveTask()
        {
            _ready = true;
            _busy = false;
            if (_start)
            {
                _ready = false;
                _busy = true;
            }
            return _start;
        }

        public bool ReceiveAckTask()
        {
            return (!_ready && _busy);
        }

        public bool ReceiveAckTaskAndGo()
        {
            _start = false;
            return (!_ready && _busy);
        }

        public bool SendFinished()
        {
            _ready = false;
            _busy = false;
            return true;
        }

        public bool ReceiveFinished()
        {
            bool value = !_ready && !_busy;
            if (value) _start = false;
            return value;
        }

        public bool ReceiveAckFinished()
        {
            return !_start;
        }
    }
}