using System.Collections.Generic;
using Idpsa.Control.Sequence;

namespace Idpsa.Control.Subsystem
{
    public interface IBackToOriginRunnable
    {
        IEnumerable<Chain> GetBackToOriginChains();
    }
}