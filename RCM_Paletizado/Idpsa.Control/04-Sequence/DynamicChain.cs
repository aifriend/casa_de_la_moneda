using System;

namespace Idpsa.Control.Sequence
{
    public class DynamicChain : StructuredChain, IRun
    {
        private bool _run;
        private Func<bool> _runCondition;
        private DynamicStepBody _stepBody;

        public event Action Finalized;

        public DynamicChain(string name, IChainControllersOwner controller)
            : base(name)
        {
            if (controller == null)
                throw new ArgumentNullException("controller");

            _runCondition = () => true;
            AddChainControllers(controller);
            AddSteps();
        }

        public DynamicChain WithRunCondition(Func<bool> runCondition)
        {
            if (runCondition == null)
                throw new ArgumentNullException("runCondition");

            _runCondition = runCondition;
            return this;
        }

        public DynamicChain WithStepBody(DynamicStepBody stepBody)
        {
            if (stepBody == null)
                throw new ArgumentNullException("stepBody");

            _stepBody = stepBody;
            return this;
        }

        public sealed override void Run()
        {
            Init = !(_run && _runCondition());
            if (!Init && !Error)
            {
                foreach (ExecutionLine line in ActiveLines())
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

        protected sealed override void AddSteps()
        {
            MainChain.Add(new Step("Paso inicial")).SetTask(() => { if (_run) NextStep(); });
            MainChain.Add(new Step("Ejecución paso dinámico")).SetDynamicBehaviour(() => _stepBody);
            MainChain.Add(new Step("Final ejecución paso dinámico"))
                .SetTask(() =>{_run = false; OnFinalized();});
        }

        private void OnFinalized()
        {
            if (Finalized != null)
                Finalized();
        }

        public void Start()
        {
            if (_stepBody == null)
                throw new NullReferenceException("stepBody");

            if (_run == false)
            {               
                Init = true;
                _run = true;
            }
        }

        public void Stop()
        {
            Init = true;
            _run = false;
        }
    }
}