using System;
using System.Collections.Generic;

namespace Idpsa.Control.Sequence
{
    [Serializable]
    public abstract class Chain : IRun
    {
        protected internal static StepByStep StepByStep = new StepByStep();

        public static void ActivateStepInStepByStep(bool value)
        {
            StepByStep.ActivateStep = value;
        }

        protected internal static ExecutionPoint GlobalCurrentExecutionPoint { get; protected set; }
        protected internal static ExecutionPoint GlobalNextExecutionPoint { get; protected set; }
        
        protected Chain(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("name");
            Name = name;
            StepsDiagnosisFamily = String.Empty; 
        }

        public string Name { get; private set; }        
        public bool Error { get; set; }
        public bool Stoped { get; set; }
        public string StepsDiagnosisFamily { get; protected set; }
             
        public abstract Step CurrentStep { get; }
        public abstract string CurrentExecutionSubchainName { get; }
        public abstract int CurrentStepIndex { get; }
        public abstract string CurrentStepName { get; }        
        public abstract void Run();

        internal abstract bool Init { get; set; }
        internal abstract bool QuitError { get; set; } 
      
        protected abstract void ChainSteps();
        protected abstract void NextStep();
        protected abstract void PreviousStep();
        protected abstract void GoToStep(string setpTag);
        protected internal abstract bool InOrigin(bool backToOriginChain, Mode operationMode, bool activeMode);

        protected virtual void SetStepDiagnosis(string diagnosisText)
        {
            CurrentStep.AddDiagnosis(diagnosisText);
        }
        protected virtual void SetStepDiagnosis(List<string> diagnosisTexts)
        {
            CurrentStep.AddDiagnosis(diagnosisTexts);
        }
    }
}