using System;
using System.Collections.Generic;
using Idpsa.Control.Subsystem;
using Idpsa.Control.Tool;

namespace Idpsa.Control.Component
{
    public class SynchronizedManager : IManagerRunnable
    {
        private readonly Dictionary<string, Action> _actions;
        private readonly EdgeWatcher _edgeWather;
        private readonly Func<bool> _value;
        private Action _manager;

        public SynchronizedManager(IEvaluable evaluable)
        {
            if (evaluable == null)
                throw new ArgumentNullException("evaluable");
            _value = () => evaluable.Value();
            _edgeWather = new EdgeWatcher(evaluable);
            _actions = new Dictionary<string, Action>();
        }

        public SynchronizedManager(Func<bool> logicalSignal)
        {
            if (logicalSignal == null)
                throw new ArgumentNullException("logicalSignal");
            _value = logicalSignal;
            _edgeWather = new EdgeWatcher(_value);
            _actions = new Dictionary<string, Action>();
        }

        public SynchronizedManager WithPosEdgeAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            return WithAction(EdgeWatcher.EdgeEvent.PosEdge.ToString(), action);
        }

        public SynchronizedManager WithNegEdgeAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            return WithAction(EdgeWatcher.EdgeEvent.NegEdge.ToString(), action);
        }

        public SynchronizedManager WithChangeEdgeAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            return WithAction(EdgeWatcher.EdgeEvent.EdgeChange.ToString(), action);
        }

        public SynchronizedManager WithPosAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            return WithAction(true.ToString(), action);
        }

        public SynchronizedManager WithNegAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            return WithAction(false.ToString(), action);
        }

        public void RemoveAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            Dictionary<string, Action>.ValueCollection temp = _actions.Values;
            foreach (string key in _actions.Keys)
            {
                _actions[key] -= action;
            }
        }

        private SynchronizedManager WithAction(string key, Action action)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentException("key can't be null or empty");
            if (action == null)
                throw new ArgumentException("action");    
            _actions[key] += action;
            return this;
        }

        public SynchronizedManager Construct()
        {
            if (_actions.Count == 0)            
                throw new Exception("SignalSicronicedManager does not have actions to perform");
           
            if (_actions.ContainsKey(EdgeWatcher.EdgeEvent.PosEdge.ToString()))
            {
                Action action = _actions[EdgeWatcher.EdgeEvent.PosEdge.ToString()];
                _manager += (() => { if (_edgeWather.PositiveEdge()) action(); });
            }

            if (_actions.ContainsKey(EdgeWatcher.EdgeEvent.NegEdge.ToString()))
            {
                Action action = _actions[EdgeWatcher.EdgeEvent.NegEdge.ToString()];
                _manager += (() => { if (_edgeWather.NegativeEdge()) action(); });
            }

            if (_actions.ContainsKey(EdgeWatcher.EdgeEvent.EdgeChange.ToString()))
            {
                Action action = _actions[EdgeWatcher.EdgeEvent.EdgeChange.ToString()];
                _manager += (() => { if (_edgeWather.EdgeChange()) action(); });
            }

            if (_actions.ContainsKey(true.ToString()))
            {
                Action action = _actions[true.ToString()];
                _manager += (() => { if (_value()) action(); });
            }

            if (_actions.ContainsKey(false.ToString()))
            {
                Action action = _actions[false.ToString()];
                _manager += (() => { if (!_value()) action(); });
            }

            return this;
        }

        private void Manager()
        {            
            _manager();
        }

        #region Miembros de IManagerRunnable

        public IEnumerable<Action> GetManagers()
        {
            return new Action[] {Manager};
        }

        #endregion
    }
}