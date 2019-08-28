using System.Collections.Generic;

namespace Idpsa.Control.Sequence
{
    public interface IChainControllersOwner
    {
        IEnumerable<IChainController> GetChainControllers();
    }
}