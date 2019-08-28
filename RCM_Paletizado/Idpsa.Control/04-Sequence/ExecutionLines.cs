using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Idpsa.Control.Sequence
{
    [Serializable]
    public class ExecutionLines : IEnumerable<ExecutionLine>
    {
        private List<ExecutionLine> _linesToAdd;

        public ExecutionLines(Subchain originChain)
        {
            Lines = new List<ExecutionLine>();
            _linesToAdd = new List<ExecutionLine>();
            MainLine = new ExecutionLine(originChain, 0) {State = ExecutionLine.States.Running};
            Lines.Add(MainLine);
            ActiveLine = MainLine;
        }

        public List<ExecutionLine> Lines { get; private set; }        
        public ExecutionLine ActiveLine { get; set; }
        public ExecutionLine MainLine { get; private set; }

        #region IEnumerable<ExecutionLine> Members

        public IEnumerator<ExecutionLine> GetEnumerator()
        {
            foreach (ExecutionLine line in Lines)
                yield return line;
        }

        #endregion

        public ExecutionLine Add(ExecutionLine line)
        {
            ExecutionLine value = Lines.FirstOrDefault(l => l.Id.Equals(line.Id));

            if (value != null)
            {
                if (value.InState(ExecutionLine.States.Finished))
                    value.State = ExecutionLine.States.Running;
                else
                    throw new Exception(
                        "No puede haber dos flujos concurrentes\n ejecutándose sobre la misma subcadena");
            }
            else
            {
                _linesToAdd.Add(line);
            }

            return line;
        }

        public void CheckNewParallelChains()
        {
            if (_linesToAdd.Count > 0)
            {
                Lines.AddRange(_linesToAdd);
                _linesToAdd.Clear();
            }
        }

        public ExecutionLine GetExecutionLine(object nameChain)
        {
            return Lines.FirstOrDefault(l => l.Id.Equals(nameChain.ToString()));
        }

        public void Reset()
        {
            foreach (ExecutionLine line in Lines)
                line.Abort();
        }

        #region Miembros de IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}