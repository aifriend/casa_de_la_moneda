using System;

namespace Idpsa.Control.Sequence
{
    [Serializable]
    public class DynamicStepBody
    {
        public DynamicStepBody(Func<string> dynamicComment, Func<bool> predicate, Action action,
                               Action diagnosisAction)
        {
            if (dynamicComment == null)
                throw new ArgumentNullException("dynamicComment");
            if (predicate == null)
                throw new ArgumentNullException("predicate");
            if (action == null)
                throw new ArgumentNullException("action");
            
            Predicate = predicate;
            Action = action;
            DynamicComment = dynamicComment;
            DiagnosisAction = diagnosisAction;
            AdditionalAction = true;
        }

        public DynamicStepBody(Func<string> dynamicComment, Action action, Action diagnosisAction)
            : this(dynamicComment, () => true, action, null){}

        public DynamicStepBody(Func<string> dynamicComment, Action action)
            : this(dynamicComment, () => true, action, null){}

        public DynamicStepBody(Func<string> dynamicComment, Func<bool> predicate, Action action)
            : this(dynamicComment, predicate, action, null){}

        public DynamicStepBody()
            : this(() => String.Empty, () => true, () => { }, null){}
        
        public Func<bool> Predicate { get; private set; }
        public Action Action { get; private set; }
        public bool AdditionalAction { get; set; }
        public int TimeOut { get; private set; }
        public Action DiagnosisAction { get; private set; }
        public Func<string> DynamicComment { get; private set; }


        public DynamicStepBody WithAdditionalAction(bool additionalAction)
        {
            AdditionalAction = additionalAction;
            return this;
        }

        public DynamicStepBody WithTimeOut(int timeOut)
        {
            if (timeOut < 0)
                throw new ArgumentException("a dinamicStepBody timeOut can't be negative");

            TimeOut = timeOut;
            return this;
        }       
    }
}