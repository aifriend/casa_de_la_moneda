using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic; 
using Idpsa.Control.Manuals;
using Idpsa.Control.Engine;

namespace Idpsa.Control
{
    public static class Evaluable
    {
        public static IEvaluable NOT(this IEvaluable source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            return new _NOT(source);
        }
        public static IEvaluable AND(this IEvaluable source, IEvaluable signal)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (signal == null)
                throw new ArgumentNullException("signal");
            return new _AND(source,signal);
        }
        public static IEvaluable OR(this IEvaluable source, IEvaluable signal)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (signal == null)
                throw new ArgumentNullException("signal");
            return new _OR(source, signal);
        }
        public static IEvaluable XOR(this IEvaluable source, IEvaluable signal)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (signal == null)
                throw new ArgumentNullException("signal");
            return new _XOR(source, signal);
        }
        public static IEvaluable XNOR(this IEvaluable source, IEvaluable signal)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (signal == null)
                throw new ArgumentNullException("signal");
            return new _XNOR(source, signal);
        }

        public static IEvaluable Clock(int timeOn, int timeOff)
        {            
            if (timeOn < 0)
                throw new ArgumentException("timeOn can't be negative");
            if (timeOff < 0)
                throw new ArgumentException("timeOff cant't be negative");

            return new _Clock(timeOn, timeOff, "CLK");           
        }      
        public static IEvaluable EnableOfClock(this IEvaluable enable, int timeOn, int timeOff)
        {
            if (enable == null)
                throw new ArgumentNullException("enable");
            if (timeOn < 0)
                throw new ArgumentException("timeOn can't be negative");
            if (timeOff < 0)
                throw new ArgumentException("timeOff cant't be negative");

            return new _EnableOfClock(enable, timeOn, timeOff,"CLK");
        }      

        public static IEvaluable ConnectionOnEdge(this IEvaluable source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            return new _ConnectionOnEdge(source);            
        }      
        public static IEvaluable ConnectionOnPositiveEdge(this IEvaluable source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            return new _ConnectionOnPositiveEdge(source);
        }       
        public static IEvaluable ConnectionOnNegativeEdge(this IEvaluable source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            return new _ConnectionOnNegativeEdge(source);
        }      
               
        public static IEvaluable DelayToConnection(this IEvaluable source, int time)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (time < 0)
                throw new ArgumentOutOfRangeException("time can't be negative");
            return new _DelayToConnection(source, time);
        }
        public static IEvaluable DelayToDisconnection(this IEvaluable source, int time)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (time < 0)
                throw new ArgumentOutOfRangeException("time can't be negative");
            return new _DelayToDisconnection(source, time);
        }

        public static IEvaluable DelayToConnectionAccumulated(this IEvaluable source, int time)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (time < 0)
                throw new ArgumentOutOfRangeException("time can't be negative");
            return new _DelayToConnectionAccumulated(source, time);
        }
        public static IEvaluable DelayToDisconnectionAccumulated(this IEvaluable source, int time)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (time < 0)
                throw new ArgumentOutOfRangeException("time can't be negative");
            return new _DelayToDisconnectionAccumulated(source, time);
        }
             
        public static IEvaluable WithToString(this IEvaluable source, string name)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("name can't be null or empty");

            return _WithToString.Create(source,()=>name);
        }
        public static IEvaluable WithToString(this IEvaluable source, Func<string> name)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (name==null)
                throw new ArgumentNullException("name");

            return _WithToString.Create(source, name);
        }

        public static IEvaluable WithReset(this IEvaluable source, Action reset)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (reset == null)
                throw new ArgumentNullException("reset");

            return _WithReset.Create(source, reset);
        }
               
        public static IEvaluable WithManualRepresentation(this IEvaluable source, string description)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (String.IsNullOrEmpty(description))
                throw new ArgumentException("description can't be null or empty");
            return new _WithManualRepresentation(source,description);
        }                      
        
        public static IEvaluable Subscribe(this IEvaluable source, Action<bool> action)
        {            
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            return _Subscribe.Instance.Subscribe(source, action);

        }
        public static IEvaluable Subscribe(this IEvaluable source, Action<bool> action, out IDisposable unsubscribe)
        {            
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            return _Subscribe.Instance.Subscribe(source, action,out unsubscribe);            
        }

        public static IEvaluable OnDelayRespectTo(this IEvaluable source, ref IEvaluable signal, Action<bool, bool, int> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (signal == null)
                throw new ArgumentNullException("signal");
            if (action == null)
                throw new ArgumentNullException("action");

            return _OnDelayRespectTo.Subscribe(source, ref signal, action);
        }
        public static IEvaluable OnDelayRespectTo(this IEvaluable source, ref IEvaluable signal, Action<bool, bool, int> action, out IDisposable unsubscribe)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (signal == null)
                throw new ArgumentNullException("signal");
            if (action == null)
                throw new ArgumentNullException("action");

            return _OnDelayRespectTo.Subscribe(source, ref signal, action,out unsubscribe);
        }
        public static IEvaluable OnDelayRespectTo(this IEvaluable source, ref IActivable signal, Action<bool, bool, int> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (signal == null)
                throw new ArgumentNullException("signal");
            if (action == null)
                throw new ArgumentNullException("action");

            return _OnDelayRespectTo.Subscribe(source, ref signal, action);
        }
        public static IEvaluable OnDelayRespectTo(this IEvaluable source, ref IActivable signal, Action<bool, bool, int> action, out IDisposable unsubscribe)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (signal == null)
                throw new ArgumentNullException("signal");
            if (action == null)
                throw new ArgumentNullException("action");

            return _OnDelayRespectTo.Subscribe(source, ref signal, action, out unsubscribe);
        }

        public static IEvaluable ToStringWithElapsedTime(this IEvaluable source, ref IActivable signal)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (signal == null)
                throw new ArgumentNullException("signal");

            return _ToStringWithElapsedTime.ToStringWithElapsedTime(source, ref signal);
        }
        public static IEvaluable ToStringWithElapsedTime(this IEvaluable source, ref IEvaluable signal)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (signal == null)
                throw new ArgumentNullException("signal");

            return _ToStringWithElapsedTime.ToStringWithElapsedTime(source, ref signal);
        }

        public static void Reset(this IEvaluable source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            _Reset.Instance.Reset(source)();
        }

        public static IEvaluable FromFunctor(Func<bool> value)
        {            
            return new _FromFunctor(value);
        }


        private class _NOT : IEvaluable
        {
            private IEvaluable _evaluable;

            public _NOT(IEvaluable evaluable)
            {
                _evaluable = evaluable;
            }

            #region Miembros de IEvaluable

            public bool Value()
            {
                return !_evaluable.Value();
            }           

            #endregion

            public override string ToString()
            {
                return "NOT "+_evaluable.ToString();
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_evaluable);
                    _reset = _reset ?? new Action(() => { });
                }
                _reset();
            }
        }
        private class _AND : IEvaluable
        {
            private IEvaluable _evaluable1;
            private IEvaluable _evaluable2;

            #region Miembros de IEvaluable


            public _AND(IEvaluable evaluable1, IEvaluable evaluable2)
            {
                _evaluable1 = evaluable1;
                _evaluable2 = evaluable2; 
            }

            public bool Value()
            {
                return _evaluable1.Value() & _evaluable2.Value(); 
            }

            #endregion

            public override string ToString()
            {
                return "(" + _evaluable1.ToString() + " AND " + _evaluable2.ToString() + ")";
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_evaluable1);
                    _reset += _Reflection.GetReset(_evaluable2);
                    _reset = _reset ?? new Action(() => { });
                }
                _reset();
            }
        }
        private class _OR : IEvaluable
        {
            private IEvaluable _evaluable1;
            private IEvaluable _evaluable2;         

            #region Miembros de IEvaluable


            public _OR(IEvaluable evaluable1, IEvaluable evaluable2)
            {
                _evaluable1 = evaluable1;
                _evaluable2 = evaluable2;
            }

            public bool Value()
            {
                return _evaluable1.Value() | _evaluable2.Value();
            }

            #endregion

            public override string ToString()
            {
                return _evaluable1.ToString() + " OR " + _evaluable2.Value();
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_evaluable1);
                    _reset += _Reflection.GetReset(_evaluable2);
                    _reset = _reset ?? new Action(() => { });
                }
                _reset();
            }
        }
        private class _XOR : IEvaluable
        {
            private IEvaluable _evaluable1;
            private IEvaluable _evaluable2;

            #region Miembros de IEvaluable


            public _XOR(IEvaluable evaluable1, IEvaluable evaluable2)
            {
                _evaluable1 = evaluable1;
                _evaluable2 = evaluable2;
            }

            public bool Value()
            {
                return _evaluable1.Value() ^ _evaluable2.Value();
            }

            #endregion

            public override string ToString()
            {
                return "(" + _evaluable1.ToString() + " XOR " + _evaluable2.Value() + ")";
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_evaluable1);
                    _reset += _Reflection.GetReset(_evaluable2);
                    _reset = _reset ?? new Action(() => { });
                }
                _reset();
            }
        }
        private class _XNOR : IEvaluable
        {
            private IEvaluable _evaluable1;
            private IEvaluable _evaluable2;

            #region Miembros de IEvaluable


            public _XNOR(IEvaluable evaluable1, IEvaluable evaluable2)
            {
                _evaluable1 = evaluable1;
                _evaluable2 = evaluable2;
            }

            public bool Value()
            {
                return !(_evaluable1.Value() ^ _evaluable2.Value());
            }

            #endregion

            public override string ToString()
            {
                return "(" + _evaluable1.ToString() + " XNOR " + _evaluable2.Value() + ")";
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_evaluable1);
                    _reset += _Reflection.GetReset(_evaluable2);
                    _reset = _reset ?? new Action(() => { });
                }
                _reset();
            }
        }

        private class _Clock : IEvaluable
        {
            private int _timeON;
            private int _timeOff;
            private TON _timerON;
            private TON _timerOff;
            private string _name;
            private bool _value;

            public _Clock(int timeON, int timeOff, string name)
            {
                _timeON = timeON;
                _timeOff = timeOff;
                _name = name;
                _timerON = new TON();
                _timerOff = new TON();
            }

            #region Miembros de IEvaluable

            public bool Value()
            {
                if (_timerON.TimingWithReset(_timeON, _value))
                    _value = false;

                if (_timerOff.TimingWithReset(_timeOff, !_value))
                    _value = true;

                return _value;
            }

            #endregion

            public void ResetClock()
            {
                _timerON.Reset();
                _timerOff.Reset();
                _value = false;
            }

            public override string ToString()
            {
                return _name;
            }
        }
        private class _EnableOfClock : IEvaluable
        {
            private IEvaluable _enable;
            private _Clock _clock;
            private string _name;

            public _EnableOfClock(IEvaluable enable, int timeON, int timeOff, string name)
            {
                _enable = enable;
                _clock = (_Clock)Clock(timeON, timeOff);
                _name = name;
            }

            #region Miembros de IEvaluable

            public bool Value()
            {
                if (!_enable.Value())
                {
                    _clock.ResetClock();
                    return false;
                }
                else
                {
                    return _clock.Value();
                }
            }

            #endregion

            public override string ToString()
            {
                return String.Format("{0}(E->{1})", _name, _enable);
            }
            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_enable);
                    _reset += ()=>_clock.ResetClock();                    
                }
                _reset();
            }
        }

        private class _ConnectionOnEdge : IEvaluable
        {
            private IEvaluable _evaluable;            
            private bool _value;
            private bool _lastValue;
            private bool _lastClock;
                       
            public _ConnectionOnEdge(IEvaluable evaluable)
            {             
                _evaluable = evaluable;
                _lastValue = evaluable.Value();              
            }

            #region Miembros de IEvaluable

            public bool Value()
            {
                if (_lastClock != CLOCK.Instance.CLK)
                {   
                    _lastClock = ! _lastClock;
                    if (_evaluable.Value() != _lastValue)
                    {
                        _value = true;
                        _lastValue = !_lastValue;
                    }
                    else
                    {
                        _value = false;
                    }
                }
                return _value;
            }

            #endregion

            public override string ToString()
            {
                return _evaluable.ToString();
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_evaluable);
                    _reset += (Action)(() => { _value = false; _lastValue = _evaluable.Value(); });                    
                }
                _reset();
            }
             
        }
        private class _ConnectionOnPositiveEdge : IEvaluable
        {
            private IEvaluable _evaluable;
            private bool _value;
            private bool _lastValue;
            private bool _lastClok;

            public _ConnectionOnPositiveEdge(IEvaluable evaluable)
            {
                _evaluable = evaluable;
                _lastValue = evaluable.Value();
            }

            #region Miembros de IEvaluable

            public bool Value()
            {
                if (_lastClok != CLOCK.Instance.CLK)
                {
                    _lastClok = !_lastClok;
                    if (_evaluable.Value() != _lastValue)
                    {
                        _lastValue = !_lastValue;
                        if(_lastValue) _value = true;                        
                    }
                    else
                    {
                        _value = false;
                    }
                }
                return _value;
            }

            #endregion

            public override string ToString()
            {
                return _evaluable.ToString();
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_evaluable);
                    _reset += (Action)(() => { _value = false; _lastValue = _evaluable.Value(); });   
                }
                _reset();
            }
        }

        private class _ConnectionOnNegativeEdge : IEvaluable
        {
            private IEvaluable _evaluable;
            private bool _value;
            private bool _lastValue;
            private bool _lastClok;

            public _ConnectionOnNegativeEdge(IEvaluable evaluable)
            {
                _evaluable = evaluable;
                _lastValue = evaluable.Value();
            }

            #region Miembros de IEvaluable

            public bool Value()
            {
                if (_lastClok != CLOCK.Instance.CLK)
                {
                    _lastClok = !_lastClok;
                    if (_evaluable.Value() != _lastValue)
                    {
                        _lastValue = !_lastValue;
                        if (!_lastValue) _value = true;                        
                    }
                    else
                    {
                        _value = false;
                    }
                }
                return _value;
            }

            #endregion

            public override string ToString()
            {
                return _evaluable.ToString();
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_evaluable);
                    _reset += (Action)(() => { _value = false; _lastValue = _evaluable.Value(); });
                }
                _reset();
            }
        }
        private class _DelayToConnection : IEvaluable
        {
            private readonly TON _timer;
            private readonly int _time;
            private readonly IEvaluable _evaluable;
            private bool _value;
            public _DelayToConnection(IEvaluable evaluable, int time)
            {
                _evaluable = evaluable;
                _time = time;
                _timer = new TON();
            }

            #region Miembros de IEvaluable

            public bool Value()
            {
                var value = _evaluable.Value();
                if (_timer.TimingWithReset(_time, value))
                {
                    _value = true;
                    return true;
                }
                if (!value)
                {
                    _value = false;
                    return false;
                }

                return _value;
            }

            #endregion

            public override string ToString()
            {
                return _evaluable.ToString();
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_evaluable);
                    _reset += (Action)(() => { _value = false; _timer.Reset(); });
                }
                _reset();
            }
        }
        private class _DelayToDisconnection : IEvaluable
        {
            private readonly TON _timer;
            private readonly int _time;
            private readonly IEvaluable _evaluable;
            private bool _value;
            public _DelayToDisconnection(IEvaluable evaluable, int time)
            {
                _evaluable = evaluable;
                _time = time;
                _timer = new TON();
            }

            #region Miembros de IEvaluable

            public bool Value()
            {
                var value = _evaluable.Value();
                if (_timer.TimingWithReset(_time, !value))
                {
                    _value = false;
                    return false;
                }
                if (value)
                {
                    _value = true;
                    return true;
                }

                return _value;
            }

            #endregion

            public override string ToString()
            {
                return _evaluable.ToString();
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_evaluable);
                    _reset += (Action)(() => { _value = false; _timer.Reset(); });
                }
                _reset();
            }
        }

        private class _DelayToConnectionAccumulated : IEvaluable
        {
            private TON _timer;
            private int _time;
            private IEvaluable _evaluable;
            private bool _value;
            public _DelayToConnectionAccumulated(IEvaluable evaluable, int time)
            {
                _evaluable = evaluable;
                _time = time;
                _timer = new TON();
            }
            #region Miembros de IEvaluable

            public bool Value()
            {
                var value = _evaluable.Value();
                if (_timer.TimingWithStop(_time, value))
                {
                    _value = true;
                    return true;
                }
                if (!value)
                {
                    _value = false;
                    return false;
                }

                return _value;
            }

            #endregion

            public override string ToString()
            {
                return _evaluable.ToString();
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_evaluable);
                    _reset += (Action)(() => { _value = false; _timer.Reset(); });
                }
                _reset();
            }
        }
        private class _DelayToDisconnectionAccumulated : IEvaluable
        {
            private TON _timer;
            private int _time;
            private IEvaluable _evaluable;
            private bool _value;
            public _DelayToDisconnectionAccumulated(IEvaluable evaluable, int time)
            {
                _evaluable = evaluable;
                _time = time;
                _timer = new TON();
            }
            #region Miembros de IEvaluable

            public bool Value()
            {
                var value = _evaluable.Value();
                if (_timer.TimingWithStop(_time, !value))
                {
                    _value = false;
                    return false;
                }
                if (value)
                {
                    _value = true;
                    return true;
                }

                return _value;
            }

            #endregion

            public override string ToString()
            {
                return _evaluable.ToString();
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_evaluable);
                    _reset += (Action)(() => { _value = false; _timer.Reset(); });
                }
                _reset();
            }
        }

        private class _WithToString : IEvaluable 
        {
            public static IEvaluable Create(IEvaluable evaluable, Func<string> name)
            {
                var manualsProvider = evaluable as IManualsProvider;
                if(manualsProvider!=null)
                    return new _WithToStringWithManual(evaluable,name,manualsProvider);
                return new _WithToString(evaluable,name); 
            }

            private IEvaluable _evaluable; 
            private Func<string> _name;
            private _WithToString(IEvaluable evaluable, Func<string> name)
            {
                _evaluable = evaluable;
                _name = name;
            }

            #region Miembros de IEvaluable

            public bool Value()
            {
                return _evaluable.Value();
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
                    _reset += _Reflection.GetReset(_evaluable);
                    _reset += _reset ?? new Action(() => { });
                }
                _reset();
            }

            private class _WithToStringWithManual : _WithToString, IManualsProvider
            {
                private string _description;

                public _WithToStringWithManual(IEvaluable evaluable, Func<string> name, IManualsProvider manualsProvider)
                    : base(evaluable, name)
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

        private class _WithReset : IEvaluable
        {
            public static IEvaluable Create(IEvaluable evaluable, Action reset)
            {
                var manualsProvider = evaluable as IManualsProvider;
                if (manualsProvider != null)
                    return new _WithResetWithManual(evaluable, reset, manualsProvider);
                return new _WithReset(evaluable, reset);
            }

            private IEvaluable _evaluable;
            private Action _reset;

            private _WithReset(IEvaluable evaluable, Action reset)
            {
                _evaluable = evaluable;
                _reset = reset;
                _reset += _Reflection.GetReset(_evaluable);  
            }

            #region Miembros de IEvaluable

            public bool Value()
            {
                return _evaluable.Value();
            }

            #endregion

            protected internal void Reset_()
            {               
                _reset();
            }

            private class _WithResetWithManual : _WithReset  
            {
                private string _description;

                public _WithResetWithManual(IEvaluable evaluable, Action reset, IManualsProvider manualsProvider)
                    : base(evaluable, reset)
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

        private class _WithManualRepresentation : IEvaluable ,IManualsProvider
        {
            private IEvaluable _evaluable;
            private string _description;

            public _WithManualRepresentation(IEvaluable evaluable, string description)
            {
                _evaluable = evaluable;
                _description = description;
            }

            #region Miembros de IEvaluable

            public bool Value()
            {
                return _evaluable.Value();
            }

            #endregion

            #region Miembros de IManualsProvider

            public IEnumerable<Manual> GetManualsRepresentations()
            {
                var manual = new Manual(_evaluable);

                if (!String.IsNullOrEmpty(_description))
                    manual.Description = _description;

                return new[] { manual };
            }

            #endregion

            public override string ToString()
            {
                return _evaluable.ToString();
            }

            private Action _reset;
            protected internal void Reset_()
            {
                if (_reset == null)
                {
                    _reset += _Reflection.GetReset(_evaluable);
                    _reset += _reset ?? new Action(() => { });
                }
                _reset();
            }

        }        

        private class _Subscribe
        {
            private readonly Dictionary<IEvaluable, _Observable> _sourcesAndObservables;        

            private static readonly _Subscribe _instance = new _Subscribe();

            public static _Subscribe Instance { get { return _instance; } }

            public _Subscribe()
            {
                _sourcesAndObservables = new Dictionary<IEvaluable, _Observable>();                
            }
                       
            public IEvaluable Subscribe(IEvaluable value, Action<bool> action)
            {
                var observable = GetObservable(value);
                observable.OnChanged += action;
                return observable;
            }
            public IEvaluable Subscribe(IEvaluable value, Action<bool> action, out IDisposable unsubscribe)
            {                
                var observable = GetObservable(value);
                observable.OnChanged += action;
                unsubscribe = new _Unsubscribe(() => observable.OnChanged -= action);
                return observable;
            }

            private _Observable GetObservable(IEvaluable value)
            {
                var observable = value as _Observable;
                if (observable != null)                
                    return observable;
                
                if (!_sourcesAndObservables.ContainsKey(value))                
                    _sourcesAndObservables[value] = _Observable.Create(value);
                
                return _sourcesAndObservables[value];                            
            }

            private class _Observable : IEvaluable 
            {
                public static _Observable Create(IEvaluable evaluable)
                {
                    var manualProvider = evaluable as IManualsProvider;
                    if (manualProvider == null)
                        return new _Observable(evaluable);
                    else
                        return new _ObservableWithManual(evaluable, manualProvider);
                }

                public IEvaluable _evaluable;
                private bool _lastValue;
                private bool _lastClockValue;
                public event Action<bool> OnChanged;              

                private _Observable(IEvaluable evaluable)
                {
                    _evaluable = evaluable;
                    _lastValue = evaluable.Value();                    
                }

                #region Miembros de IEvaluable

                public bool Value()
                {                    
                    if (_lastClockValue != CLOCK.Instance.CLK)
                    {
                        _lastClockValue = !_lastClockValue;
                        if (_evaluable.Value() != _lastValue)
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
                    return _evaluable.ToString();
                }

                private Action _reset;
                protected internal void Reset_()
                {
                    if (_reset == null)
                    {
                        _reset += _Reflection.GetReset(_evaluable);
                        _reset += _reset ?? new Action(() => { });
                    }
                    _reset();
                }

                private class _ObservableWithManual : _Observable, IManualsProvider
                {
                    private string _description;

                    public _ObservableWithManual(IEvaluable evaluable, IManualsProvider manualsProvider)
                        : base(evaluable)
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
            public static IEvaluable Subscribe(IEvaluable signal, ref IEvaluable originSignal, Action<bool, bool, int> action)
            {            
                return new _OnDelayRespectTo().SetTimeSpan(signal, ref originSignal, action);               
            }
            public static IEvaluable Subscribe(IEvaluable signal, ref IEvaluable originSignal, Action<bool, bool, int> action, out IDisposable unsubscribe)
            {
                return new _OnDelayRespectTo().SetTimeSpan(signal, ref originSignal, action, out unsubscribe);
            }
            public static IEvaluable Subscribe(IEvaluable signal, ref IActivable originSignal, Action<bool, bool, int> action)
            {            
                return new _OnDelayRespectTo().SetTimeSpan(signal, ref originSignal, action);               
            }
            public static IEvaluable Subscribe(IEvaluable signal, ref IActivable originSignal, Action<bool, bool, int> action, out IDisposable unsubscribe)
            {
                return new _OnDelayRespectTo().SetTimeSpan(signal, ref originSignal, action, out unsubscribe);
            }                    

            private _OnDelayRespectTo(){}

            private IEvaluable SetTimeSpan(IEvaluable signal, ref IEvaluable originSignal, Action<bool, bool, int> action)
            {
                var notifier = new _TimeSpanNotifier(signal, originSignal, action);
                originSignal = originSignal.Subscribe(notifier.TimeSpanNotifierOriginSignal);
                return signal.Subscribe(notifier.TimeSpanNotifierSignal);
            }
            private IEvaluable SetTimeSpan(IEvaluable signal, ref IEvaluable originSignal, Action<bool, bool, int> action, out IDisposable unsubscribe)
            {
                var notifier = new _TimeSpanNotifier(signal, originSignal, action);
                IDisposable unsubscriveSignal = null;
                IDisposable unsubscriveOriginSignal = null;
                originSignal = originSignal.Subscribe(notifier.TimeSpanNotifierOriginSignal, out unsubscriveOriginSignal);
                var value = signal.Subscribe(notifier.TimeSpanNotifierSignal, out unsubscriveSignal);
                unsubscribe = new _Unsubscribe(() => { unsubscriveOriginSignal.Dispose(); unsubscriveSignal.Dispose(); });
                return value;
            }
            private IEvaluable SetTimeSpan(IEvaluable signal, ref IActivable originSignal, Action<bool, bool, int> action)
            {
                var notifier = new _TimeSpanNotifier(signal, originSignal, action);
                originSignal = originSignal.Subscribe(notifier.TimeSpanNotifierOriginSignal);
                return signal.Subscribe(notifier.TimeSpanNotifierSignal);
            }
            private IEvaluable SetTimeSpan(IEvaluable signal, ref IActivable originSignal, Action<bool, bool, int> action, out IDisposable unsubscribe)
            {
                var notifier = new _TimeSpanNotifier(signal, originSignal, action);
                IDisposable unsubscriveSignal = null;
                IDisposable unsubscriveOriginSignal = null;
                originSignal = originSignal.Subscribe(notifier.TimeSpanNotifierSignal, out unsubscriveOriginSignal);
                var value = signal.Subscribe(notifier.TimeSpanNotifierOriginSignal, out unsubscriveSignal);
                unsubscribe = new _Unsubscribe(() => { unsubscriveOriginSignal.Dispose(); unsubscriveSignal.Dispose(); });
                return value;
            }

            private class _TimeSpanNotifier
            {
                private IEvaluable _signal;
                private IEvaluable _originSignal;
                public Action<bool, bool, int> _action;

                private int _timeSignal=-1;
                private int _timeOriginSignal=-1;

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
            public static IEvaluable ToStringWithElapsedTime(IEvaluable signal, ref IActivable originSignal)
            {
                TON timer = new TON();
                int elapsedTime = 0;
                return signal.OnDelayRespectTo(ref originSignal, (v, vo, t) => { if (v) { timer.Start(); elapsedTime = t; } })
                             .WithToString(() => { return (timer.Started && timer.ElapsedTime < 2000) ? elapsedTime.ToString() : signal.ToString(); });
            }
            public static IEvaluable ToStringWithElapsedTime(IEvaluable signal, ref IEvaluable originSignal)
            {
                TON timer = new TON();
                int elapsedTime = 0;
                return signal.OnDelayRespectTo(ref originSignal, (v, vo, t) => { if (v) { timer.Start(); elapsedTime = t; } })
                             .WithToString(() => { return (timer.Started && timer.ElapsedTime < 2000) ? elapsedTime.ToString() : signal.ToString(); });
            }
        }

        private class _FromFunctor : IEvaluable
        {
            private Func<bool> _value;
            private Func<string> _name;

            public _FromFunctor(Func<bool> value)
            {
                _value = value;
                var name = value.ToString();
                _name = () => name;
            }

            public _FromFunctor(Func<bool> value, Func<string> name)
            {
                _value = value;
                _name = name;
            }

            #region Miembros de IEvaluable

            public bool Value()
            {
                return _value();
            }

            #endregion

            public override string ToString()
            {
                return _name();
            }
        }

        private class _Reset
        {
            private Dictionary<IEvaluable, Action> _evaluableActions;
            private static readonly _Reset _instance = new _Reset();

            private _Reset()
            {
                _evaluableActions = new Dictionary<IEvaluable, Action>();
            }

            public static _Reset Instance
            {
                get { return _instance; }
            }

            public Action Reset(IEvaluable evaluable)
            {
                if (!_evaluableActions.ContainsKey(evaluable))
                {
                    var value = _Reflection.GetReset(evaluable);
                    _evaluableActions[evaluable] = value ?? new Action(() => { });
                }

                return _evaluableActions[evaluable];
            }
        }

        private static class _Reflection
        {
            private static Delegate GetMethod(IEvaluable evaluable,Type delegateType,string methodName)
            {
                var method = evaluable.GetType().GetMethod(methodName, BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.FlattenHierarchy);
                if (method == null) return null;              
                return Delegate.CreateDelegate(delegateType , evaluable, methodName);
            }

            public static Action GetReset(IEvaluable evaluable)
            {
                return (Action)GetMethod(evaluable,typeof(Action),"Reset_");
            }          
        }
    }
}