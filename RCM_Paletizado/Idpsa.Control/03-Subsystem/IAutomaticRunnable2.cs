using System.Collections.Generic;
using Idpsa.Control.Sequence;

namespace Idpsa.Control.Subsystem
{
    public interface IAutomaticRunnable2//MDG.2012-07-23
    {
        IEnumerable<Chain> GetAutoChains2();
    }
}