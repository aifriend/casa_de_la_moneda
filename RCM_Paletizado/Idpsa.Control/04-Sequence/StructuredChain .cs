using System;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Diagnosis;

namespace Idpsa.Control.Sequence
{
    [Serializable]
    public abstract class StructuredChain : Chain, IChainControllable
    {
        private bool _init;
        private bool _quitError;

        protected StructuredChain(string name)
            : base(name)
        {
            MainChain = new Subchain("Principal");
            Subchains = new Dictionary<string, Subchain> {{MainChain.Name, MainChain}};
            Lines = new ExecutionLines(MainChain);            
        }

        public ExecutionLines Lines { get; private set; }
        protected Subchain MainChain { get; private set; }
        protected internal Dictionary<string, Subchain> Subchains { get; private set; }

        public override string CurrentExecutionSubchainName
        {
            get { return (Name + ", subcadena: " + Lines.MainLine.ActivePoint.Subchain.Name); }          
        }

        protected Subchain this[object chainName]
        {
            get { return Subchain(chainName); }
        }

        internal override bool Init
        {
            get { return _init; }
            set
            {
                if (value)
                {
                    Lines.Reset();
                    Lines.MainLine.ActivePoint.Subchain = MainChain;
                    Lines.MainLine.ActivePoint.StepIndex = 0;
                    GetStep(Lines.MainLine.ActivePoint).Activated = false;
                    Lines.MainLine.State = ExecutionLine.States.Running;
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

        public override int CurrentStepIndex
        {
            get { return Lines.MainLine.ActivePoint.StepIndex; }
        }

        public override string CurrentStepName
        {
            get
            {
                return (Name + ", subcadena: " + Lines.ActiveLine.ActivePoint.Subchain.Name + "/" +
                        Lines.ActiveLine.ActivePoint.StepIndex);
            }
        }

        public IEnumerable<Step> CurrentSteps
        {
            get { return ActiveLines().Select(l => GetStep(l.ActivePoint)); }
        }

        public override Step CurrentStep
        {
            get { return GetStep(Lines.MainLine.ActivePoint); }
        }

        public Step ActiveCurrentStep
        {
            get { return GetStep(Lines.ActiveLine.ActivePoint); }
        }

        #region IChainControllable Members

        public string IdControlledChain
        {
            get { return Name; }
        }

        public Func<object, Subchain> GetSubchainCaller()
        {
            return CallExternChain;
        }

        public Func<object, Subchain> GetSubchainParallelCaller()
        {
            return CallParallelChain;
        }

        public Action GetNextStepCaller()
        {
            return NextStep;
        }

        public Action GetPreviousStepCaller()
        {
            return PreviousStep;
        }

        public Action GetReturnCaller()
        {
            return Return;
        }

        public Action<string> GetGoToStepCaller()
        {
            return GoToStep;
        }

        public Func<object, bool> GetJoinCaller()
        {
            return Join;
        }

        #endregion

        protected internal event Action<string> ChainActive = delegate { };

        protected Subchain Subchain(object chainName)
        {
            return Subchains[chainName.ToString()];
        }

        protected Subchain Subchain(string chainName)
        {
            return Subchains[chainName];
        }

        public StructuredChain AddChainController(IChainController chainController)
        {
            if (chainController == null)
                throw new ArgumentNullException("chainController");
 
            chainController.SetChainCallers(this);
            ChainActive += chainController.SetCaller;
            foreach (Subchain subChain in chainController.GetSubchains())
            {
                if (!Subchains.ContainsKey(subChain.Name))
                {
                    Subchains.Add(subChain.Name, subChain);
                }
            }
            return this;
        }

        public StructuredChain AddChainControllers(IChainControllersOwner chainControllerOwner)
        {           
            foreach (IChainController chainController in chainControllerOwner.GetChainControllers())
            {
                AddChainController(chainController);
            }
            return this;
        }

        protected internal Subchain CallExternChain(object mameChain)
        {
            return CallChain(mameChain.ToString());
        }

        protected Subchain AddSubchain(object mameChain)
        {
            if (mameChain == null)            
                throw new ArgumentNullException("name"); 
           
            var value = new Subchain(mameChain.ToString());
            Subchains.Add(mameChain.ToString(), value);
            return value;
        }

        protected override void NextStep()
        {
            if(IsLastSubchainStep(Lines.ActiveLine.ActivePoint))            
                throw new Exception(String.Format("NextStep() was used in the final step of the subchain {0}"
                                              , Lines.ActiveLine.ActivePoint.Subchain.Name));  
           
            Lines.ActiveLine.ActivePoint.StepIndex = NextStepIndex(Lines.ActiveLine.ActivePoint);
            ActiveCurrentStep.Activated = false;
        }

        protected override void PreviousStep()
        {
            if (IsFirstSubchainStep(Lines.ActiveLine.ActivePoint))
                throw new Exception(String.Format("PreviousStep() was used in the initial step of the subchain {0}"
                                              , Lines.ActiveLine.ActivePoint.Subchain.Name)); 

            Lines.ActiveLine.ActivePoint.StepIndex = PreviousStepIndex(Lines.ActiveLine.ActivePoint);
            ActiveCurrentStep.Activated = false;
        }

        protected override void GoToStep(string stepTag)
        {
            if (String.IsNullOrEmpty(stepTag))            
                throw new ArgumentException("StepTag can't be null or empty"); 
            
            Lines.ActiveLine.ActivePoint.StepIndex =
                Lines.ActiveLine.ActivePoint.Subchain.GetStepIndex(stepTag);

            ActiveCurrentStep.Activated = false;
        }

        protected IEnumerable<ExecutionLine> ActiveLines()
        {
            return Lines.Where(l => l.InState(ExecutionLine.States.Running));
        }

        protected internal override bool InOrigin(bool backToOriginChain, Mode operationMode, bool activeMode)
        {            
            var mainActivePoint = Lines.MainLine.ActivePoint;

            if ((ActiveLines().Count() > 1))
                return false;

            if (!Lines.MainLine.InState(ExecutionLine.States.Running))
                return false;

            if (backToOriginChain)
            {
                if (operationMode == Mode.BackToOrigin && activeMode)
                {
                    if (!IsLastSubchainStep(mainActivePoint))
                        return false;
                }
                else
                {
                    if (!IsFirstSubchainStep(mainActivePoint))
                        return false;
                }
            }
            else
            {
                if (!IsFirstSubchainStep(mainActivePoint))
                    return false;
            }

            return true;
        }

        private static Step GetStep(ExecutionPoint value)
        {
            return value.Subchain[value.StepIndex];            
        }

        private bool IsFirstSubchainStep(ExecutionPoint value)
        {
            return (value.StepIndex == 0);
        }
        
        private bool IsLastSubchainStep(ExecutionPoint value)
        {
            return ( value.StepIndex == value.Subchain.Count - 1);
        }

        private int NextStepIndex(ExecutionPoint value)
        {            
            if (value.StepIndex < value.Subchain.Count - 1)
                return value.StepIndex + 1;           
            else
                throw new ArgumentOutOfRangeException("StepIndex");             
        }

        private int PreviousStepIndex(ExecutionPoint value)
        {
            if (value.StepIndex > 0)
                return value.StepIndex - 1;            
            else
                throw new ArgumentOutOfRangeException("StepIndex");  
        }

        private ExecutionPoint MoveNextStep(ExecutionPoint p)
        {           
            p.StepIndex = NextStepIndex(p);
            return p;
        }       

        protected override void ChainSteps()
        {
            ActiveCurrentStep.Task();
        }

        protected Subchain CallChain(object nameChain)
        {
            Lines.ActiveLine.CallBackPoints.Push(MoveNextStep(Lines.ActiveLine.ActivePoint));
            Lines.ActiveLine.ActivePoint = GlobalNextExecutionPoint = new ExecutionPoint(Subchains[nameChain.ToString()], 0);
            ActiveCurrentStep.Activated = false;
            return Subchains[nameChain.ToString()];
        }

        protected Subchain CallParallelChain(object nameChain)
        {
            var line =
                Lines.Add(new ExecutionLine(Subchains[nameChain.ToString()], 0) {State = ExecutionLine.States.Running});
            GlobalNextExecutionPoint = line.ActivePoint;
            GetStep(line.ActivePoint).Activated = false;
            return Subchains[nameChain.ToString()];
        }

        protected bool Join(object nameChain)
        {
            return !ParallelChainInState(nameChain, ExecutionLine.States.Running);
        }

        protected bool Join(params object[] nameChains)
        {
            foreach (object nameChain in nameChains)
                if (!Join(nameChain))
                    return false;
            return true;
        }

        private bool ChangeExecutionLineState(object nameChain, ExecutionLine.States newState)
        {
            var line = Lines.GetExecutionLine(nameChain);
            if (line != null)
            {
                line.State = newState;
                return true;
            }
            return false;
        }

        protected bool Stop(object nameChain)
        {
            return ChangeExecutionLineState(nameChain, ExecutionLine.States.Stoped);
        }

        protected bool Resume(object nameChain)
        {
            return ChangeExecutionLineState(nameChain, ExecutionLine.States.Running);
        }

        protected bool Abort(object nameChain)
        {
            var line = Lines.GetExecutionLine(nameChain);
            if (line != null)
            {
                line.Abort();
                return true;
            }
            return false;
        }

        protected bool RequestAbort(object nameChain)
        {
            return ChangeExecutionLineState(nameChain, ExecutionLine.States.AbortRequested);
        }

        protected bool CheckAbortRequested()
        {
            if (Lines.ActiveLine.State == ExecutionLine.States.AbortRequested)
            {
                Lines.ActiveLine.Abort();
                return true;
            }
            return false;
        }

        protected bool ParallelChainInState(object nameChain, ExecutionLine.States state)
        {
            bool value = false;
            var line = Lines.GetExecutionLine(nameChain);
            if (line != null)
                value = line.InState(state);

            return value;
        }

        protected virtual void Return()
        {
            ActiveCurrentStep.Activated = false;
            Lines.ActiveLine.ActivePoint.Subchain.ClearParamValues(Lines.ActiveLine.ActivePoint);
            if (Lines.ActiveLine.CallBackPoints.Count > 0)
            {
                Lines.ActiveLine.ActivePoint = Lines.ActiveLine.CallBackPoints.Pop();
                ActiveCurrentStep.Activated = false;
            }
            else
            {
                Lines.ActiveLine.CallBackPoints.Clear();
                Lines.ActiveLine.ActivePoint.StepIndex = 0;
                Lines.ActiveLine.State = ExecutionLine.States.Finished;
            }
        }

        public override void Run()
        {
            if (Init || Error) return;

            if (StepByStep.StepByStepManager(CurrentStep.StopStepByStep) && !Stoped)
            {
                foreach (var line in ActiveLines())
                {
                    Lines.ActiveLine = line;
                    GlobalCurrentExecutionPoint = line.ActivePoint;
                    OnChainActive();
                    TreatStep();
                    ChainSteps();
                }
                Lines.CheckNewParallelChains();
            }
        }

        protected virtual void OnChainActive()
        {
            ChainActive(IdControlledChain);
        }

        protected void TreatStep()
        {
            if (ActiveCurrentStep.Diagnosis)
                TreatDiagnosis();
            else
                ActiveCurrentStep.Activated = true;
        }

        protected internal void TreatDiagnosis()
        {
            Step step = ActiveCurrentStep;
            step.Activated = false;
            Error = true;

            if (step.DiagnosisTask != null) step.DiagnosisTask();

            if (step.GetDiagnosis().Count > 0)
            {                
                foreach (string description in step.GetDiagnosis())
                    DiagnosisManager.Instance.Add(new DiagnosisStep(CurrentStepName, description,StepsDiagnosisFamily));

                step.ClearDiagnosis();
            }
            else
            {
                DiagnosisManager.Instance.Add(new DiagnosisStep(CurrentStepName, step.Comment, StepsDiagnosisFamily));
            }
        }

        protected void AddDynamicSubChain(object subChainName, DynamicStepBody stepBody)
        {
            var chain = AddSubchain(subChainName.ToString());
            chain.Add(new Step()).SetDynamicBehaviour(() => stepBody, NextStep);
            chain.Add(new Step("Final ejecución de subcadena " + chain.Name)).SetTask(Return);
        }

        protected abstract void AddSteps();

    }
}