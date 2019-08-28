using System.Collections.Generic;
using Idpsa.Control.Sequence;

namespace Idpsa.Control.Subsystem
{
    public interface IFreeRunnable
    {
        IEnumerable<Chain> GetFreeChains();
    }
}