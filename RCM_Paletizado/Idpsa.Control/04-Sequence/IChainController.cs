using System.Collections.Generic;

namespace Idpsa.Control.Sequence
{
    public interface IChainController
    {
        void SetCaller(string idControlledChain);
        void SetChainCallers(IChainControllable chainControllable);
        IEnumerable<Subchain> GetSubchains();
    }
}