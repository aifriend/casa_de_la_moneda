using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Idpsa.Control.Sequence
{
    [Serializable]
    public class Subchain
    {
        private List<Step> _steps;
        private readonly Dictionary<string, Dictionary<ExecutionPoint, object>> _callerParameters;
        private Dictionary<string, object> _parameters;
        private Dictionary<string, int> _stepTags_stepIndex;

        public string Name { get; private set; }
         
        public Subchain(string name)
        {
            if (String.IsNullOrEmpty(name))           
                throw new ArgumentException("The name of a subchain can't be null or empty");
            
            Name = name;
            _parameters = new Dictionary<string, object>();
            _steps = new List<Step>();
            _callerParameters = new Dictionary<string, Dictionary<ExecutionPoint, object>>();
        }
             
        public Subchain WithParam(string name, object value)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("A parameter name can't be null or empty");
            if (!_callerParameters.ContainsKey(name))
                throw new Exception(String.Format("In the subchain {0} the paremeter {1} hasn't been defined",
                                                  Name, name));

            _callerParameters[name][NextCaller] = value;
            return this;
        }
        public Subchain WithParams(Dictionary<string, object> values)
        {
            foreach (var value in values)
                WithParam(value.Key, value.Value);

            return this;
        }
        public void AddParam(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("A parameter name can't be null or empty");
            _callerParameters[name] = new Dictionary<ExecutionPoint, object>();
        }
        public void AddParams(params string[] names)
        {
            foreach (string name in names)
                AddParam(name);
        }
        public object Param(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("A parameter name can't be null or empty");
            return _callerParameters[name][Caller];
        }
        public T Param<T>(string name)
        {
            return (T)_callerParameters[name][Caller];
        }
        public T TryParam<T>(string name)
        {
            if(_callerParameters[name].ContainsKey(Caller))
                return (T)_callerParameters[name][Caller];
            return default(T);
        }
        public void ClearParamValues(ExecutionPoint caller)
        {
            for (int i = 0; i < _parameters.Count; i++)
            {
                _callerParameters[_parameters.Keys.ElementAt(i)].Remove(caller);
            }
        }
        public Step Add(Step step)
        {
            if (step == null)
                throw new ArgumentNullException("step");
            
            _steps.Add(step);
            step.SubchainName = Name;

            if (!String.IsNullOrEmpty(step.Tag))
            {
                if (_stepTags_stepIndex == null)
                {
                    _stepTags_stepIndex = new Dictionary<string, int>();
                }
                else
                {
                    if (_stepTags_stepIndex.ContainsKey(step.Tag))
                        throw new ArgumentException
                            (String.Format("The step tag {0} already exists in the subchain {1}", step.Tag, Name));
                }
                _stepTags_stepIndex[step.Tag] = _steps.Count - 1;
            }
            
            return step;
        }

        public Subchain SetSteps(IList<Step> steps)
        {
            if (steps == null)
                throw new ArgumentNullException("steps");

            foreach (var step in steps)
                Add(step);

            return this;
        }

        public int GetStepIndex(string stepTag)
        {
            if (!_stepTags_stepIndex.ContainsKey(stepTag))           
                throw new ArgumentException
                    (String.Format("There's no step in the subchain {0} with the tag {1}",Name,stepTag));
            
            return _stepTags_stepIndex[stepTag];
        }    
        public int Count
        {
            get
            {
                return _steps.Count;
            }
        }

        internal Step this[int index]
        {
            get { return _steps[index]; }
        }
        private static ExecutionPoint Caller
        {
            get { return Chain.GlobalCurrentExecutionPoint; }
        }
        private static ExecutionPoint NextCaller
        {
            get { return Chain.GlobalNextExecutionPoint; }
        }
     
    }
}