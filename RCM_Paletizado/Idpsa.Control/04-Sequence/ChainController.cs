using System;
using System.Collections.Generic;

namespace Idpsa.Control.Sequence
{
    public class ChainController : IChainController
    {
        private readonly Dictionary<string, ControllerDelegates> _controllerDelegates;
        private readonly List<Subchain> _subChains;
        private string _idControlledChain;

        public ChainController()
        {
            _subChains = new List<Subchain>();
            _controllerDelegates = new Dictionary<string, ControllerDelegates>();
        }

        #region IChainController Members

        public void SetCaller(string idControlledChain)
        {
            _idControlledChain = idControlledChain;
        }

        public void SetChainCallers(IChainControllable chainControllable)
        {
            if (!_controllerDelegates.ContainsKey(chainControllable.IdControlledChain))
            {
                _controllerDelegates[chainControllable.IdControlledChain] = 
                    new ControllerDelegates()
                        {
                            CallChain = chainControllable.GetSubchainCaller(),
                            Return = chainControllable.GetReturnCaller(),
                            NextStep = chainControllable.GetNextStepCaller(),
                            PreviousStep = chainControllable.GetPreviousStepCaller(),
                            GoToStep = chainControllable.GetGoToStepCaller(),
                            CallParallelChain = chainControllable.GetSubchainParallelCaller(),
                            Join = chainControllable.GetJoinCaller()
                        };
            }
        }

        public IEnumerable<Subchain> GetSubchains()
        {
            return _subChains;
        }

        #endregion

        public Subchain AddChain(object name)
        {
            var value = new Subchain(name.ToString());
            _subChains.Add(value);
            return value;
        }

        public Subchain CallChain(Object nameChain)
        {
            return _controllerDelegates[_idControlledChain].CallChain(nameChain);
        }

        public Subchain CallParallelChain(Object nameChain)
        {
            return _controllerDelegates[_idControlledChain].CallParallelChain(nameChain);
        }

        public void Return()
        {
            _controllerDelegates[_idControlledChain].Return();
        }

        public void NextStep()
        {
            _controllerDelegates[_idControlledChain].NextStep();
        }

        public void PreviousStep()
        {
            _controllerDelegates[_idControlledChain].PreviousStep();
        }

        public void GoToStep(string stepTag)
        {
            _controllerDelegates[_idControlledChain].GoToStep(stepTag);
        }

        public bool Join(params object[] chainNames)
        {
            foreach (object chainName in chainNames)
                if (!_controllerDelegates[_idControlledChain].Join(chainName))
                    return false;

            return true;
        }

        protected void AddDynamicSubChain(object subChainName, DynamicStepBody stepBody)
        {
            var chain = new Subchain(subChainName.ToString());
            chain.Add(new Step()).SetDynamicBehaviour(() => stepBody, NextStep);
            chain.Add(new Step("Final ejecución " + chain.Name)).SetTask(Return);
        }

        #region Nested type: ControllerDelegates

        private struct ControllerDelegates
        {
            public Func<object, Subchain> CallChain;
            public Func<object, Subchain> CallParallelChain;
            public Action<string> GoToStep;
            public Func<object, bool> Join;
            public Action NextStep;
            public Action PreviousStep;
            public Action Return;
        }

        #endregion
    }
}