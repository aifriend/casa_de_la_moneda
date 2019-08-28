using System;
using System.Collections.Generic;
using Idpsa.Control.Timing;

namespace Idpsa.Control.Sequence
{
    [Serializable]
    public class Step
    {
        private bool _activated;
        private bool _diagnosis;
        [NonSerialized]
        private Action _diagnosisTask;
        private Func<string> _dynamicComment;
        private readonly List<string> _commentDiagnosis;
        private readonly TimerCounter _myCounter;
        [NonSerialized]
        private Action _task;
        private Func<int> _timeOutFunc;
        public string Tag { get; private set; }
        internal string SubchainName{get;set;}

        public Step(string comment, int timeOut, bool stopChain, bool stopStepByStep)
        {
            if (comment == null)
                throw new ArgumentNullException("comment");
            if (timeOut < 0)
                throw new ArgumentException("a step time out can't be negative");

            _timeOutFunc = () => timeOut;
            _dynamicComment = () => comment;
            StopStepByStep = stopStepByStep;
            StopChain = stopChain;
            _commentDiagnosis = new List<string>();
            _myCounter = new TimerCounter();
        }

        public Step(Func<string> dynamicComment, int timeOut, bool stopChain, bool stopStepByStep)
        {
            if (dynamicComment == null)
                throw new ArgumentNullException("dynamicComment");
            if (timeOut < 0)
                throw new ArgumentException("a step time out can't be negative");

            _timeOutFunc = () => timeOut;
            _dynamicComment = dynamicComment;
            StopStepByStep = stopStepByStep;
            StopChain = stopChain;
            _commentDiagnosis = new List<string>();
            _myCounter = new TimerCounter();
        }

        public Step(string comment, int timeOut, bool stopChain)
            : this(comment, timeOut, stopChain, true) { }

        public Step(Func<string> dynamicComment, int timeOut, bool stopChain)
            : this(dynamicComment, timeOut, stopChain, true) { }

        public Step(string comment, int timeOut)
            : this(comment, timeOut, false, true) { }

        public Step(Func<string> dynamicComment, int timeOut)
            : this(dynamicComment, timeOut, false, true) { }

        public Step(string comment)
            : this(comment, 0, false, true) { }

        public Step(Func<string> dynamicComment)
            : this(dynamicComment, 0, false, true) { }

        public Step()
            : this(String.Empty, 0, false, true) { }

        public Step WithTag(string tag)
        {
            if (String.IsNullOrEmpty(tag))
                throw new ArgumentException("an step tag can't be null or empty");

            if (SubchainName!=null)
                throw new Exception(String.Format("tag : '{0}' was set after been added to the subChain : '{1}'",tag,SubchainName));

            Tag = tag;
            return this;
        }

        public bool StopChain { get; set; }

        public int TimeOut
        {
            get { return _timeOutFunc(); }
        }

        public string Comment
        {
            get { return _dynamicComment(); }
        }

        public bool Activated
        {
            get { return _activated; }
            set
            {
                if (!_activated & value)
                {
                    _myCounter.Reset();
                    _myCounter.Start();
                }
                if (!value)
                {
                    _diagnosis = false;
                    _myCounter.Reset();
                }
                _activated = value;
            }
        }

        public bool StopStepByStep { get; set; }

        public Action Task
        {
            get { return _task; }
            set { SetTask(value); }
        }

        public Action DiagnosisTask
        {
            get { return _diagnosisTask; }
            set { SetDiagnosisTask(value); }
        }

        public bool Diagnosis
        {
            get
            {
                if (Activated)
                {
                    if (TimeOut > 0)
                    {
                        if (_myCounter.Value > TimeOut)
                        {
                            _diagnosis = true;
                            _myCounter.Reset();
                        }
                    }
                }
                return _diagnosis;
            }
        }

        public void SetDynamicBehaviour(Func<DynamicStepBody> dynamicBodyFunc)
        {
            if (dynamicBodyFunc == null)
                throw new ArgumentException("dynamicBodyFunc");
            SetDynamicBehaviour(dynamicBodyFunc, () => { });
        }

        public void SetDynamicBehaviour(Func<DynamicStepBody> dynamicBodyFunc, Action additionalAction)
        {
            if (dynamicBodyFunc == null)
                throw new ArgumentNullException("dynamicBodyFunc");
            if (additionalAction == null)
                throw new ArgumentNullException("additionalAction");

            _timeOutFunc = (() => dynamicBodyFunc().TimeOut);
            _dynamicComment = (() => dynamicBodyFunc().DynamicComment());
            _task = () =>
                        {
                            if (dynamicBodyFunc().Predicate())
                            {
                                dynamicBodyFunc().Action();
                                if (dynamicBodyFunc().AdditionalAction)
                                    additionalAction();
                            }
                        };

            _diagnosisTask = () => dynamicBodyFunc().DiagnosisAction();
        }

        public Step SetTask(Action task)
        {
            if (task == null)
                throw new ArgumentNullException("task");
            _task = task;
            return this;
        }

        public void SetDiagnosisTask(Action diagnosisTask)
        {
            if (diagnosisTask == null)
                throw new ArgumentNullException("diagnosisTask");
            if (_task == null)
                throw new Exception("A normal step task must be set before the set up of the diagnosis task");
           
            _diagnosisTask = diagnosisTask;
        }

        public List<String> GetDiagnosis()
        {
            return _commentDiagnosis;
        }

        public void AddDiagnosis(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            _diagnosis = true;
            _commentDiagnosis.Add(text);
        }

        public void AddDiagnosis(List<string> texts)
        {
            if (texts == null)
                throw new ArgumentNullException("texts");
            _diagnosis = true;
            foreach (string text in texts)
                AddDiagnosis(texts);
        }

        public void ClearDiagnosis()
        {
            _commentDiagnosis.Clear();
        }
       
    }
}