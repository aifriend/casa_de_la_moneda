using System;
using System.Collections.Generic;
using Idpsa.Control.Engine;

namespace Idpsa.Control.Engine
{
    public interface IDelegateTasksOwner
    {
        IEnumerable<Action> GetDelegateTasks();
    }
}