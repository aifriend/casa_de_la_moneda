using System;
using Idpsa.Control.Engine;

namespace Idpsa.Control.Tool
{
    public class EdgeWatcher
    {
        #region EdgeEvent enum

        [Flags]
        public enum EdgeEvent
        {
            None = 0,
            EdgeChange = 1,
            PosEdge = 2 | EdgeChange,
            NegEdge = 4 | EdgeChange
        }

        #endregion

        private readonly Func<bool> _value;
        private bool _lastClock;
        private bool _lastReadedValue;
        private EdgeEvent _currentEdgeEvent;  

        public EdgeWatcher(IEvaluable evaluable)
        {
            _value = () => evaluable.Value();
        }

        public EdgeWatcher(Func<bool> value)
        {
            _value = value;
        }

        public bool PositiveEdge()
        {
            SetCurrentEdgeEvent();
            return EdgeEvent.PosEdge.Equals(_currentEdgeEvent);
        }

        public bool NegativeEdge()
        {
            SetCurrentEdgeEvent();
            return EdgeEvent.NegEdge.Equals(_currentEdgeEvent);
        }

        public bool EdgeChange()
        {
            SetCurrentEdgeEvent();
            return (EdgeEvent.EdgeChange & _currentEdgeEvent) != 0;
        }

        public EdgeEvent GetEdgeEvent()
        {
            SetCurrentEdgeEvent();
            return _currentEdgeEvent;
        }
              

        private void SetCurrentEdgeEvent()
        {
            EdgeEvent edgeEvent = EdgeEvent.None; 
            if(CLOCK.Instance.CLK != _lastClock)
            {
                _lastClock = !_lastClock;               
                
                if (_lastReadedValue != _value())
                {                    
                    _lastReadedValue = !_lastReadedValue;
                    _currentEdgeEvent = _lastReadedValue ? EdgeEvent.PosEdge : EdgeEvent.NegEdge;
                   
                }             
            }       
            
        }

    }
}