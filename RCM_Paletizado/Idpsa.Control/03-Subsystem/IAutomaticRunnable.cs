using System.Collections.Generic;
using Idpsa.Control.Sequence;

namespace Idpsa.Control.Subsystem
{
    public interface IAutomaticRunnable
    {
        IEnumerable<Chain> GetAutoChains();
    }
}