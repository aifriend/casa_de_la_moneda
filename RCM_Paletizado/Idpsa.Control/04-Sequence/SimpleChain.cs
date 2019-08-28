using System;
using System.Linq;
using Idpsa.Control.Diagnosis;

namespace Idpsa.Control.Sequence
{
    [Serializable]
    public abstract class SimpleChain : Chain
    {
        private readonly ExecutionPoint _exPoint;
        private bool _init;
        private bool _quitError;

        protected SimpleChain(string name)
            : base(name)
        {
            _exPoint = new ExecutionPoint(new Subchain(name), 0);
            Steps = _exPoint.Subchain;
        }
        protected Subchain Steps
        {
            get { return _exPoint.Subchain; }
            private set { _exPoint.Subchain = value; }
        }

        internal override bool Init
        {
            get { return _init; }
            set
            {
                if (value)
                {
                    SetCurrentStepIndex(0);
                    Error = false;
                }
                _init = value;
            }
        }
        internal override bool QuitError
        {
            get { return _quitError; }
            set
            {
                _quitError = value;
                if (_quitError)
                {
                    Error = false;
                    _quitError = false;
                }
            }
        }
      
        public override Step CurrentStep
        {
            get { return GetStep(CurrentStepIndex); }
        }

        public override int CurrentStepIndex
        {
            get { return _exPoint.StepIndex; }
        }

        public override string CurrentExecutionSubchainName { get { return Name;} }

        public override string CurrentStepName
        {
            get { return (Name + "/" + CurrentStepIndex); }
        }

        public override void Run()
        {
            if (Init || Error) return;

            if (StepByStep.StepByStepManager(CurrentStep.StopStepByStep) && !Stoped)
                ChainSteps();

            if (CurrentStep.Diagnosis)
                TreatDiagnosis();
            else
                CurrentStep.Activated = true;
        }

        protected internal void TreatDiagnosis()
        {
            Step step = CurrentStep;
            step.Activated = false;
            Error = true;

            if (CurrentStep.DiagnosisTask != null)
                CurrentStep.DiagnosisTask();

            if (step.GetDiagnosis().Count > 0)
            {                
                foreach (string description in step.GetDiagnosis())
                {
                    DiagnosisManager.Instance.Add(new DiagnosisStep(CurrentStepName, description, StepsDiagnosisFamily));                                                                    
                }
                step.ClearDiagnosis();
            }
            else
            {
                DiagnosisManager.Instance.Add(new DiagnosisStep(CurrentStepName,step.Comment, StepsDiagnosisFamily));
            }
        }

        protected override void NextStep()
        {
            CurrentStep.Activated = false;
            SetCurrentStepIndex(NextStepIndex(CurrentStepIndex));
        }

        protected override void PreviousStep()
        {
            CurrentStep.Activated = false;
            SetCurrentStepIndex(PreviousStepIndex(CurrentStepIndex));
        }

        protected override void GoToStep(string stepTag)
        {
            if (String.IsNullOrEmpty(stepTag))
            {
                throw new ArgumentException("stepTag can't be null or empty");
            }
            _exPoint.StepIndex =
                _exPoint.Subchain.GetStepIndex(stepTag);

            CurrentStep.Activated = false;
        }

        protected internal override bool InOrigin(bool backToOriginChain, Mode operationMode, bool activeMode)
        {           
            if (backToOriginChain)
            {
                if (operationMode == Mode.BackToOrigin && activeMode)
                {
                    if (!IsLastStep(CurrentStepIndex))
                        return false;
                }
                else
                {
                    if (!IsFirstStep(CurrentStepIndex))
                        return false;
                }
            }
            else
            {
                if (IsFirstStep(CurrentStepIndex))               
                    return true;                
            }

            return false;
        }
        
        private Step GetStep(int stepIndex)
        {
            return _exPoint.Subchain[stepIndex]; 
        }
        private void SetCurrentStepIndex(int step)
        {
            _exPoint.StepIndex = step;
            if (!_init)
            {
                CurrentStep.Activated = true;
            }
        }
        private bool IsFirstStep(int stepIndex)
        {
            return (stepIndex == 0);
        }
        private bool IsLastStep(int stepIndex)
        {
            return (stepIndex == _exPoint.Subchain.Count - 1);
        }
        private int NextStepIndex(int stepIndex)
        {
            if (stepIndex < _exPoint.Subchain.Count - 1)
                return stepIndex + 1;
            else
                throw new ArgumentOutOfRangeException("exPoint.StepIndex");       
        }
        private int PreviousStepIndex(int stepIndex)
        {
            if (stepIndex > 0)
                return stepIndex - 1;
            else
                throw new ArgumentOutOfRangeException("exPoint.StepIndex");  
        }      
        protected override void ChainSteps()
        {
            GlobalCurrentExecutionPoint = _exPoint;
            CurrentStep.Task();
        }
    }
}