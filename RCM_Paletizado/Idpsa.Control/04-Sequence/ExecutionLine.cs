using System;
using System.Collections.Generic;

namespace Idpsa.Control.Sequence
{
    [Serializable]
    public class ExecutionLine
    {
        #region States enum

        [Flags]
        public enum States
        {
            NotStarted = 1,
            Running = 2,
            Stoped = 4,
            Finished = 8,
            AbortRequested = 16 | Running
        }

        #endregion

        public ExecutionLine(Subchain chain, int step)
        {
            Id = chain.Name;
            ActivePoint = new ExecutionPoint(chain, step);
            CallBackPoints = new Stack<ExecutionPoint>();
        }

        public ExecutionLine(ExecutionPoint executionPoint)
        {
            State = States.NotStarted;
            ActivePoint = executionPoint;
            CallBackPoints = new Stack<ExecutionPoint>();
        }

        public string Id { get; private set; }
        public ExecutionPoint ActivePoint { get; set; }
        public Stack<ExecutionPoint> CallBackPoints { get; private set; }
        public States State { get; set; }

        public void ToOrigin()
        {
            foreach (ExecutionPoint executionPoint in CallBackPoints)
                executionPoint.Subchain.ClearParamValues(executionPoint);

            ActivePoint.StepIndex = 0;
            CallBackPoints.Clear();
        }

        public void Abort()
        {
            State = States.Finished;
            ToOrigin();
        }

        public override string ToString()
        {
            return String.Format("{0} / {1}", ActivePoint.Subchain.Name, ActivePoint.StepIndex);
        }

        public bool InState(States state)
        {
            return (State & state) != 0;
        }
    }
}