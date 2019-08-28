using System;

namespace Idpsa.Control.Sequence
{
    [Serializable]
    public class ExecutionPoint
    {
        public ExecutionPoint(Subchain subChain, int stepIndex)
        {
            Subchain = subChain;
            StepIndex = stepIndex;
        }

        public Subchain Subchain { get; set; }
        public int StepIndex { get; set; }

        public override string ToString()
        {
            return String.Format("{0} / {1}", Subchain.Name, StepIndex);
        }
    }
}