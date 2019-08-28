using System;
using System.Collections.Generic;
using Idpsa.Control.Tool;

namespace Idpsa.Control.Component
{
    public class SynchronizedActions
    {
        private readonly List<Action> _actions;
        private readonly EdgeWatcher _edgeWather;
        private readonly Func<bool> _value;        

        public SynchronizedActions(IEvaluable evaluable)
        {
            if (evaluable == null)
                throw new ArgumentNullException("evaluable");
            _value = () => evaluable.Value();
            _edgeWather = new EdgeWatcher(evaluable);
            _actions = new List<Action>();
        }

        public SynchronizedActions(Func<bool> logicalSignal)
        {
            if (logicalSignal == null)
                throw new ArgumentNullException("logicalSignal");
            _value = logicalSignal;
            _edgeWather = new EdgeWatcher(_value);
            _actions = new List<Action>();
        }

        public SynchronizedActions WithPosEdgeAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action"); 
            return WithAction(() => { if (_edgeWather.PositiveEdge()) action(); });
        }

        public SynchronizedActions WithNegEdgeAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            return WithAction(() => { if (_edgeWather.NegativeEdge()) action(); });
        }

        public SynchronizedActions WithChangeEdgeAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            return WithAction(() => { if (_edgeWather.EdgeChange()) action(); });
        }

        public SynchronizedActions WithPosAction(Action action)
        {
            if (action == null) 
                throw new ArgumentNullException("action");
            return WithAction(() => { if (_value()) action(); });
        }

        public SynchronizedActions WithNegAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            return WithAction(() => { if (!_value()) action(); });
        }       

        private SynchronizedActions WithAction(Action action)
        {
            if (action == null)
                throw new ArgumentException("action");
            _actions.Add(action);
            return this;
        }

        public void DoActions()
        {           
            foreach (Action action in _actions)
                action();
        }
    }
}