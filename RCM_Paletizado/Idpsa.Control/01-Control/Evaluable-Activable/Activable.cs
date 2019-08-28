using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Idpsa.Control.Engine;
using Idpsa.Control.Manuals;

namespace Idpsa.Control
{
    public static class Activable
    {
        public static IActivable NOT(this IActivable source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new _NOT(source); 
        }

        public static IActivable Join(this IActivable source, IActivable signal)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (signal == null)
                throw new ArgumentNullException("signal");

            return new _Join(source, signal);
        }
      
        public static IActivable DelayToConnection(this IActivable source, int time)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (time < 0)
                throw new ArgumentOutOfRangeException("time can't be negative");
            return new _DelayToConnection(source, time);
        }
        public static IActivable DelayToDisconnection(this IActivable source, int time)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (time < 0)
                throw new ArgumentOutOfRangeException("time can't be negative");
            return new _DelayToDisconnection(source, time);
        }
       
        public static IActivable DelayToConnectionAccumulated(this IActivable source, int time)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (time < 0)
                throw new ArgumentOutOfRangeException("time can't be negative");
            return new _DelayToConnectionAccumulated(source, time);
        }
        public static IActivable DelayToDisconnectionAccumulated(this IActivable source, int time)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (time < 0)
                throw new ArgumentOutOfRangeException("time can't be negative");
            return new _DelayToDisconnectionAccumulated(source, time);
        }
        
        public static IActivable WithValue(this IActivable source, Func<bool> value)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (value == null)
                throw new ArgumentNullException("value");

            return _WithValue.Create(source, value);
        }
        public static IActivable WithValue(this IActivable source, Func<bool, bool> workToValue)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (workToValue == null)
                throw new ArgumentNullException("workToValue");

            return _WithValue.Create(source, workToValue);
        }

        public static IActivable WithToString(this IActivable source, string name)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("name can't be null or empty");

            return _WithToString.Create(source, () => name);  
        }
        public static IActivable WithToString(this IActivable source, Func<string> name)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (name == null)
                throw new ArgumentNullException("name");

            return _WithToString.Create(source, name); 
        }       
        
        public static IActivable WithManualRepresentation(this IActivable source, string description)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (String.IsNullOrEmpty(description))
                throw new ArgumentException("description can't be null or empty");
            return new _WithManualRepresentation(source,description);
        }        

        public static IActivable Subscribe(this IActivable source, Action<bool> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            return _Subscribe.Instance.Subscribe(source, action);

        }
        public static IActivable Subscribe(this IActivable source, Action<bool> action, out IDisposable unsubscribe)
        {            
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            return _Subscribe.Instance.Subscribe(source, action, out unsubscribe);
        }

        public static IActivable OnDelayRespectTo(this IActivable source, ref IEvaluable signal, Action<bool, bool, int> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (signal == null)
                throw new ArgumentNullException("signal");
            if (action == null)
                throw new ArgumentNullException("action");

            return _OnDelayRespectTo.Subscribe(source, ref signal, action);
        }
        public static IActivable OnDelayRespectTo(this IActivable source, ref IEvaluable signal, Action<bool, bool, int> action, out IDisposable unsubscribe)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (signal == null)
                throw new ArgumentNullException("signal");
            if (action == null)
                throw new ArgumentNullException("action");

            return _OnDelayRespectTo.Subscribe(source, ref signal, action,out unsubscribe);
        }
        public static IActivable OnDelayRespectTo(this IActivable source, ref IActivable signal, Action<bool, bool, int> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (signal == null)
                throw new ArgumentNullException("signal");
            if (action == null)
                throw new ArgumentNullException("action");

            return _OnDelayRespectTo.Subscribe(source, ref signal, action);
        }
        public static IActivable OnDelayRespectTo(this IActivable source, ref IActivable signal, Action<bool, bool, int> action, out IDisposable unsubscribe)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (signal == null)
                throw new ArgumentNullException("signal");
            if (action == null)
                throw new ArgumentNullException("action");

            return _OnDelayRespectTo.Subscribe(source, ref signal, action, out unsubscribe);
        }

        public static IActivable ToStringWithElapsedTime(this IActivable source, ref IActivable signal)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (signal == null)
                throw new ArgumentNullException("signal");

            return _ToStringWithElapsedTime.ToStringWithElapsedTime(source, ref signal);
        }
        public static IEvaluable ToStringWithElapsedTime(this IActivable source, ref IEvaluable signal)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (signal == null)
                throw new ArgumentNullException("signal");

            return _ToStringWithElapsedTime.ToStringWithElapsedTime(source, ref signal);
        }

        public static void Reset(this IActivable source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            _Reset.Instance.Reset(source)();
        }

        public static IActivable FromFunctor(Action<bool> activate)
        {           
            return new _FromFunctor(activate);
        }


        private class _NOT : IActivable
        {
            private IActivable _activable;

            public _NOT(IActivable activable)
            {
                _activable = activable;
            }

            #region Miembros de IActivable

            public void Activate(bool work)
            {
                _activable.Activate(!work);
            }

            #endregion

            #region Miembros de IEvaluable

            public bool Value()
            {
                return _activable.Value();
            }

            #endregion

            public override string ToString()
            {
                return "NOT " + _activable.ToString();
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_activable);
                    _reset += _reset ?? new Action(() => {});
                }
                _reset();
            }
        }
        
        private class _Join : IActivable
        {
            private IActivable _activable1;
            private IActivable _activable2;
            Func<string> _toString;
            private bool _value;

            public _Join(IActivable activable1, IActivable activable2)
            {
                _activable1 = activable1;
                _activable2 = activable2;
                _toString = () => _activable1.ToString() + " JOIN " + _activable2.ToString();
            }

            public _Join(IActivable activable1, IActivable activable2, string name)
            {
                _activable1 = activable1;
                _activable2 = activable2;
                _toString = () => name;
            }

            #region Miembros de IActivable

            public void Activate(bool work)
            {
                _activable1.Activate(work);
                _activable2.Activate(work);
                _value = work;
            }

            #endregion

            #region Miembros de IEvaluable

            public bool Value()
            {
                return _value;
            }

            #endregion

            public override string ToString()
            {
                return _toString();
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_activable1);
                    _reset += _Reflection.GetReset(_activable2);
                    _reset += _reset ?? new Action(() => {});
                }
                _reset();
            }
        }
        
        private class _DelayToConnection : IActivable
        {
            private TON _timer;
            private int _time;
            private IActivable _activable;
            private bool _value;

            public _DelayToConnection(IActivable activable, int time)
            {
                _activable = activable;
                _time = time;
                _timer = new TON();
                _value = false;
            }

            #region Miembros de IActivable

            public void Activate(bool work)
            {
                if (_timer.TimingWithReset(_time, work))
                {
                    _value = true;
                    _activable.Activate(true);
                    return;
                }
                if (!work)
                {
                    _value = false;
                    _activable.Activate(false);
                    return;
                }
                _activable.Activate(_value);
            }

            #endregion

            #region Miembros de IEvaluable

            public bool Value()
            {
                return _activable.Value();
            }

            #endregion

            public override string ToString()
            {
                return _activable.ToString();
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_activable);
                    _reset += _reset ?? new Action(() => { _value = false; _timer.Reset(); });
                }
                _reset();
            }
        }
        private class _DelayToDisconnection : IActivable
        {
            private TON _timer;
            private int _time;
            private IActivable _activable;
            private bool _value;
            public _DelayToDisconnection(IActivable activable, int time)
            {
                _activable = activable;
                _time = time;
                _timer = new TON();
                _value = true;
            }

            #region Miembros de IActivable

            public void Activate(bool work)
            {                
                if (_timer.TimingWithReset(_time, !work))
                {
                    _value = false;
                    _activable.Activate(false);
                    return;
                }
                if (work)
                {
                    _value = true;
                    _activable.Activate(true);
                    return;
                }
                _activable.Activate(_value);
            }

            #endregion

            #region Miembros de IEvaluable

            public bool Value()
            {
                return _activable.Value();
            }

            #endregion

            public override string ToString()
            {
                return _activable.ToString();
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_activable);
                    _reset += _reset ?? new Action(() => { _value = false; _timer.Reset(); });
                }
                _reset();
            }
        }

        private class _DelayToConnectionAccumulated : IActivable
        {
            private TON _timer;
            private int _time;
            private IActivable _activable;
            private bool _value;

            public _DelayToConnectionAccumulated(IActivable activable, int time)
            {
                _activable = activable;
                _time = time;
                _timer = new TON();
                _value = false;
            }

            #region Miembros de IActivable

            public void Activate(bool work)
            {
                if (_timer.TimingWithStop(_time, work))
                {
                    _value = true;
                    _activable.Activate(true);
                    return;
                }
                if (!work)
                {
                    _value = false;
                    _activable.Activate(false);
                    return;
                }
                _activable.Activate(_value);
            }

            #endregion

            #region Miembros de IEvaluable

            public bool Value()
            {
                return _activable.Value();
            }

            #endregion

            public override string ToString()
            {
                return _activable.ToString();
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_activable);
                    _reset += _reset ?? new Action(() => { _value = false; _timer.Reset(); });
                }
                _reset();
            }
        }
        private class _DelayToDisconnectionAccumulated : IActivable
        {
            private TON _timer;
            private int _time;
            private IActivable _activable;
            private bool _value;

            public _DelayToDisconnectionAccumulated(IActivable activable, int time)
            {
                _activable = activable;
                _time = time;
                _timer = new TON();
                _value = false;
            }

            #region Miembros de IActivable

            public void Activate(bool work)
            {
                if (_timer.TimingWithStop(_time, !work))
                {
                    _value = false;
                    _activable.Activate(false);
                    return;
                }
                if (work)
                {
                    _value = true;
                    _activable.Activate(true);
                    return;
                }
                _activable.Activate(_value);
            }

            #endregion

            #region Miembros de IEvaluable

            public bool Value()
            {
                return _activable.Value();
            }

            #endregion

            public override string ToString()
            {
                return _activable.ToString();
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_activable);
                    _reset += _reset ?? new Action(() => { _value = false; _timer.Reset(); });
                }
                _reset();
            }
        }
       
        private class _WithValue : IActivable
        {
            public static IActivable Create(IActivable bitActivale, Func<bool> value)
            {
                var manualsProvider = bitActivale as IManualsProvider;
                if (manualsProvider != null)
                    return new _WithValueWithManual(bitActivale, value, manualsProvider);
                return new _WithValue(bitActivale, value);
            }

            public static IActivable Create(IActivable bitActivale, Func<bool, bool> workToValue)
            {
                var manualsProvider = bitActivale as IManualsProvider;
                if (manualsProvider != null)
                    return new _WithValueWithManual(bitActivale, workToValue, manualsProvider);
                return new _WithValue(bitActivale, workToValue);
            }

            private IActivable _activable;            
            private Func<bool> _value;
            private bool _lastWorkValue;

            private _WithValue(IActivable bitActivale, Func<bool> value)
            {
                _activable = bitActivale;
                _value = value;                
            }

            private _WithValue(IActivable bitActivale, Func<bool, bool> workToValue)
            {
                _activable = bitActivale;                
                _value = () => workToValue(_lastWorkValue); 
                _lastWorkValue = false;
            }
            #region Miembros de IActivable

            public void Activate(bool work)
            {
                _activable.Activate(work);
                _lastWorkValue = work;
            }

            #endregion

            #region Miembros de IEvaluable

            public bool Value()
            {
                return _value();
            }

            #endregion

            public override string ToString()
            {
                return _activable.ToString();
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_activable);
                    _reset = _reset ?? new Action(() => { });
                }
                _reset();
            }

            private class _WithValueWithManual : _WithValue, IManualsProvider
            {
                private string _description;

                public _WithValueWithManual(IActivable bitActivale, Func<bool> value, IManualsProvider manualsProvider)
                    : base(bitActivale,value)
                {
                    _description = manualsProvider.GetManualsRepresentations().ElementAt(0).Description;
                }

                public _WithValueWithManual(IActivable bitActivale, Func<bool, bool> workToValue, IManualsProvider manualsProvider)
                    : base(bitActivale, workToValue)
                {
                    _description = manualsProvider.GetManualsRepresentations().ElementAt(0).Description;
                }

                #region Miembros de IManualsProvider

                public IEnumerable<Manual> GetManualsRepresentations()
                {
                    var manual = new Manual(this);

                    if (!String.IsNullOrEmpty(_description))
                        manual.Description = _description;

                    return new[] { manual };
                }

                #endregion
            }
        }

        private class _WithToString : IActivable
        {
            public static IActivable Create(IActivable activable, Func<string> name)
            {
                var manualsProvider = activable as IManualsProvider;
                if (manualsProvider != null)
                    return new _WithToStringWithManual(activable, name, manualsProvider);
                return new _WithToString(activable, name);
            }

            private IActivable _activable;
            private Func<string> _name;

            private _WithToString(IActivable activable, Func<string> name)
            {
                _activable = activable;
                _name = name;
            }

            #region Miembros de IActivable

            public void Activate(bool work)
            {
                _activable.Activate(work);
            }

            #endregion

            #region Miembros de IEvaluable

            public bool Value()
            {
                return _activable.Value();
            }

            #endregion

            public override string ToString()
            {
                return _name();
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_activable);
                    _reset = _reset ?? new Action(() => { });
                }
                _reset();
            }

            private class _WithToStringWithManual : _WithToString, IManualsProvider
            {
                private string _description;

                public _WithToStringWithManual(IActivable activable, Func<string> name, IManualsProvider manualsProvider)
                    : base(activable,name)
                {
                    _description = manualsProvider.GetManualsRepresentations().ElementAt(0).Description;
                }

                #region Miembros de IManualsProvider

                public IEnumerable<Manual> GetManualsRepresentations()
                {
                    var manual = new Manual(this);

                    if (!String.IsNullOrEmpty(_description))
                        manual.Description = _description;

                    return new[] { manual };
                }

                #endregion

            }
        }

        private class _WithReset : IActivable
        {
            public static IActivable Create(IActivable activable, Action reset)
            {
                var manualsProvider = activable as IManualsProvider;
                if (manualsProvider != null)
                    return new _WithResetWithManual(activable, reset, manualsProvider);
                return new _WithReset(activable, reset);
            }

            private IActivable _activable;
            private Action _reset;

            private _WithReset(IActivable activable, Action reset)
            {
                _activable = activable;
                _reset = reset;
                _reset += _Reflection.GetReset(_activable);
            }

            #region Miembros de IActivable

            public void Activate(bool work)
            {
                _activable.Activate(work);                
            }

            #endregion

            #region Miembros de IEvaluable

            public bool Value()
            {
                return _activable.Value();
            }

            #endregion           

            protected internal void Reset_()
            {
                _reset();
            }

            private class _WithResetWithManual : _WithReset
            {
                private string _description;

                public _WithResetWithManual(IActivable activable, Action reset, IManualsProvider manualsProvider)
                    : base(activable, reset)
                {
                    _description = manualsProvider.GetManualsRepresentations().ElementAt(0).Description;
                }               

                #region Miembros de IManualsProvider

                public IEnumerable<Manual> GetManualsRepresentations()
                {
                    var manual = new Manual(this);

                    if (!String.IsNullOrEmpty(_description))
                        manual.Description = _description;

                    return new[] { manual };
                }

                #endregion
            }          
        }

        private class _WithManualRepresentation : IActivable, IManualsProvider
        {
            private IActivable _activable;
            private string _description;

            public _WithManualRepresentation(IActivable activable, string description)
            {
                _activable = activable;
                _description = description;
            }

            #region Miembros de IActivable

            public void Activate(bool work)
            {
                _activable.Activate(work);
            }

            #endregion

            #region Miembros de IEvaluable

            public bool Value()
            {
                return _activable.Value();
            }

            #endregion

            #region Miembros de IManualsProvider

            public IEnumerable<Manual> GetManualsRepresentations()
            {
                var manual = new Manual(this);

                if (!String.IsNullOrEmpty(_description))
                    manual.Description = _description;

                return new[] { manual };
            }

            #endregion

            public override string ToString()
            {
                return _activable.ToString();
            }
        }        

        private class _Subscribe
        {
            private readonly Dictionary<IActivable, _Observable> _sourcesAndObservables;

            private static readonly _Subscribe _instance = new _Subscribe();

            public static _Subscribe Instance { get { return _instance; } }

            public _Subscribe()
            {
                _sourcesAndObservables = new Dictionary<IActivable, _Observable>();
            }

            public IActivable Subscribe(IActivable value, Action<bool> action)
            {
                var observable = GetObservable(value);
                observable.OnChanged += action;
                return observable;
            }
            public IActivable Subscribe(IActivable value, Action<bool> action, out IDisposable unsubscribe)
            {
                var observable = GetObservable(value);
                observable.OnChanged += action;
                unsubscribe = new _Unsubscribe(() => observable.OnChanged -= action);
                return observable;
            }

            private _Observable GetObservable(IActivable value)
            {
                var observable = value as _Observable;
                if (observable != null)
                    return observable;

                if (!_sourcesAndObservables.ContainsKey(value))
                    _sourcesAndObservables[value] = _Observable.Create(value);

                return _sourcesAndObservables[value];
            }

            private class _Observable : IActivable
            {
                public static _Observable Create(IActivable activable)
                {
                    var manualProvider = activable as IManualsProvider;
                    if (manualProvider != null)
                        return new _ObservableWithManual(activable, manualProvider);
                    
                    return new _Observable(activable);
                }

                private IActivable _activable;
                private bool _lastValue;
                private bool _lastClockValue;
                public event Action<bool> OnChanged;

                private _Observable(IActivable evaluable)
                {
                    _activable = evaluable;
                    _lastValue = evaluable.Value();
                }

                #region Miembros de IActivable

                public void Activate(bool work)
                {
                    _activable.Activate(work);
                    Value();                    
                }

                #endregion

                #region Miembros de IEvaluable

                public bool Value()
                {
                    if (_lastClockValue != CLOCK.Instance.CLK)
                    {
                        _lastClockValue = !_lastClockValue;
                        if (_activable.Value() != _lastValue)
                        {
                            _lastValue = !_lastValue;
                            var temp = OnChanged;
                            if (temp != null)
                            {
                                OnChanged(_lastValue);
                            }
                        }
                    }
                    return _lastValue;
                }

                #endregion

                public override string ToString()
                {
                    return _activable.ToString();
                }

                private class _ObservableWithManual : _Observable, IManualsProvider
                {
                    private string _description;

                    public _ObservableWithManual(IActivable activable, IManualsProvider manualsProvider)
                        : base(activable)
                    {
                        _description = manualsProvider.GetManualsRepresentations().ElementAt(0).Description;
                    }

                    #region Miembros de IManualsProvider

                    public IEnumerable<Manual> GetManualsRepresentations()
                    {
                        var manual = new Manual(this);

                        if (!String.IsNullOrEmpty(_description))
                            manual.Description = _description;

                        return new[] { manual };
                    }

                    #endregion
                }
               
            }
            private class _Unsubscribe : IDisposable
            {
                #region Miembros de IDisposable

                private Action _dispose;

                public _Unsubscribe(Action dispose)
                {
                    _dispose = dispose;
                }

                public void Dispose()
                {
                    _dispose();
                }

                #endregion
            }
        }

        private class _OnDelayRespectTo
        {
            public static IActivable Subscribe(IActivable signal, ref IActivable originSignal, Action<bool, bool, int> action)
            {              
                return new _OnDelayRespectTo().SetTimeSpan(signal, ref originSignal, action);                
            }
            public static IActivable Subscribe(IActivable signal, ref IActivable originSignal, Action<bool, bool, int> action, out IDisposable unsubscribe)
            {
                return new _OnDelayRespectTo().SetTimeSpan(signal, ref originSignal, action, out unsubscribe);
            }
            public static IActivable Subscribe(IActivable signal, ref IEvaluable originSignal, Action<bool, bool, int> action)
            {                
                return new _OnDelayRespectTo().SetTimeSpan(signal, ref originSignal, action);
            }
            public static IActivable Subscribe(IActivable signal, ref IEvaluable originSignal, Action<bool, bool, int> action, out IDisposable unsubscribe)
            {
                return new _OnDelayRespectTo().SetTimeSpan(signal, ref originSignal, action, out unsubscribe);
            }
            
            private _OnDelayRespectTo() { }

            private IActivable SetTimeSpan(IActivable signal, ref IActivable originSignal, Action<bool, bool, int> action)
            {
                var notifier = new _TimeSpanNotifier(signal, originSignal, action);
                originSignal = originSignal.Subscribe(notifier.TimeSpanNotifierOriginSignal);
                return signal.Subscribe(notifier.TimeSpanNotifierSignal);
            }
            private IActivable SetTimeSpan(IActivable signal, ref IActivable originSignal, Action<bool, bool, int> action, out IDisposable unsubscribe)
            {
                var notifier = new _TimeSpanNotifier(signal, originSignal, action);
                IDisposable unsubscriveSignal = null;
                IDisposable unsubscriveOriginSignal = null;
                originSignal = originSignal.Subscribe(notifier.TimeSpanNotifierOriginSignal, out unsubscriveOriginSignal);
                var value = signal.Subscribe(notifier.TimeSpanNotifierSignal, out unsubscriveSignal);
                unsubscribe = new _Unsubscribe(() => { unsubscriveOriginSignal.Dispose(); unsubscriveSignal.Dispose(); });
                return value;
            }
            private IActivable SetTimeSpan(IActivable signal, ref IEvaluable originSignal, Action<bool, bool, int> action)
            {
                var notifier = new _TimeSpanNotifier(signal, originSignal, action);
                originSignal = originSignal.Subscribe(notifier.TimeSpanNotifierOriginSignal);
                return signal.Subscribe(notifier.TimeSpanNotifierSignal);
            }
            private IActivable SetTimeSpan(IActivable signal, ref IEvaluable originSignal, Action<bool, bool, int> action, out IDisposable unsubscribe)
            {
                var notifier = new _TimeSpanNotifier(signal, originSignal, action);
                IDisposable unsubscriveSignal = null;
                IDisposable unsubscriveOriginSignal = null;
                originSignal = originSignal.Subscribe(notifier.TimeSpanNotifierOriginSignal, out unsubscriveOriginSignal);
                var value = signal.Subscribe(notifier.TimeSpanNotifierSignal, out unsubscriveSignal);
                unsubscribe = new _Unsubscribe(() => { unsubscriveOriginSignal.Dispose(); unsubscriveSignal.Dispose(); });
                return value;
            }

            private class _TimeSpanNotifier
            {
                private IEvaluable _signal;
                private IEvaluable _originSignal;
                public Action<bool, bool, int> _action;

                private int _timeSignal = -1;
                private int _timeOriginSignal = -1;

                public _TimeSpanNotifier(IEvaluable signal, IEvaluable originSignal, Action<bool, bool, int> action)
                {
                    _signal = signal;
                    _originSignal = originSignal;
                    _action = action;
                }

                public void TimeSpanNotifierOriginSignal(bool value)
                {
                    _timeOriginSignal = Environment.TickCount;
                    _timeSignal = -1;
                }

                public void TimeSpanNotifierSignal(bool value)
                {
                    _timeSignal = Environment.TickCount;
                    if (_timeSignal != -1 && _timeOriginSignal != -1)
                    {
                        int timeSpan = _timeSignal - _timeOriginSignal;
                        _timeOriginSignal = _timeSignal = -1;
                        _action(_signal.Value(), _originSignal.Value(), timeSpan);
                    }
                }
            }           

            private class _Unsubscribe : IDisposable
            {
                #region Miembros de IDisposable

                private Action _dispose;

                public _Unsubscribe(Action dispose)
                {
                    _dispose = dispose;
                }

                public void Dispose()
                {
                    _dispose();
                }

                #endregion
            }    
        }

        private class _ToStringWithElapsedTime
        {
            public static IActivable ToStringWithElapsedTime(IActivable signal, ref IActivable originSignal)
            {
                TON timer = new TON();
                int elapsedTime = 0;
                return signal.OnDelayRespectTo(ref originSignal, (v, vo, t) => { if (v) { timer.Start(); elapsedTime = t; } })
                             .WithToString(() => { return (timer.Started && timer.ElapsedTime < 2000) ? elapsedTime.ToString() : signal.ToString(); });
            }
            public static IActivable ToStringWithElapsedTime(IActivable signal, ref IEvaluable originSignal)
            {
                TON timer = new TON();
                int elapsedTime = 0;
                return signal.OnDelayRespectTo(ref originSignal, (v, vo, t) => { if (v) { timer.Start(); elapsedTime = t; } })
                             .WithToString(() => { return (timer.Started && timer.ElapsedTime < 2000) ? elapsedTime.ToString() : signal.ToString(); });
            }
        }

        private class _FromFunctor : IActivable
        {
            private Action<bool> _activate;
            private string _name;
            private bool _lastWorkValue;

            public _FromFunctor(Action<bool> activate)
            {
                _activate = activate;
                _name = activate.ToString();
            }

            #region Miembros de IActivable

            public void Activate(bool work)
            {
                _activate(work);
                _lastWorkValue = work;
            }

            #endregion

            #region Miembros de IEvaluable

            public bool Value()
            {
                return _lastWorkValue;
            }

            #endregion

            public override string ToString()
            {
                return _name;
            }
        }

        private class _Reset
        {
            private Dictionary<IActivable, Action> _activableActions;
            private static readonly _Reset _instance = new _Reset();

            private _Reset()
            {
                _activableActions = new Dictionary<IActivable, Action>();
            }

            public static _Reset Instance
            {
                get { return _instance; }
            }

            public Action Reset(IActivable activable)
            {
                if (!_activableActions.ContainsKey(activable))
                {
                    var value = _Reflection.GetReset(activable);
                    _activableActions[activable] = value ?? new Action(() => { });
                }

                return _activableActions[activable];
            }
        }

        private static class _Reflection
        {
            private static Delegate GetMethod(IActivable activable, Type delegateType, string methodName)
            {
                var method = activable.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                if (method == null) return null;
                return Delegate.CreateDelegate(delegateType, activable, methodName);
            }

            public static Action GetReset(IActivable evaluable)
            {
                return (Action)GetMethod(evaluable, typeof(Action), "Reset_");
            }
        }
    }
}