using System;
using System.Collections.Generic;

namespace Idpsa.Control.Subsystem
{
    public interface IManagerRunnable
    {
        IEnumerable<Action> GetManagers();
    }
}