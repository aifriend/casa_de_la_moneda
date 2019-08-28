using System;

namespace Idpsa.Control.Sequence
{
    public interface IChainControllable
    {
        string IdControlledChain { get; }
        Func<object, Subchain> GetSubchainCaller();
        Action GetNextStepCaller();
        Action GetPreviousStepCaller();
        Action GetReturnCaller();
        Action<string> GetGoToStepCaller();
        Func<object, Subchain> GetSubchainParallelCaller();
        Func<object, bool> GetJoinCaller();
    }
}