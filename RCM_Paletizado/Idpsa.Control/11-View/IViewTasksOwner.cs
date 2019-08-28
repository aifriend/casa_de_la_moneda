using System;
using System.Collections.Generic;

namespace Idpsa.Control.View
{
    public interface IViewTasksOwner
    {
        IEnumerable<Action> GetViewTasks();
    }
}