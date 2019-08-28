using System;
using System.Collections.Generic;

namespace Idpsa.Control.Diagnosis
{
    internal class DiagnosisFamilyLog
    {
        private readonly Dictionary<string, int> _diagnosisFamilyLog;
        private readonly Object _lockObject = new Object();

        public DiagnosisFamilyLog()
        {
            _diagnosisFamilyLog = new Dictionary<string, int>();
        }

        public void AddToLog(GeneralDiagnosis d)
        {
            lock (_lockObject)
            {
                if (!String.IsNullOrEmpty(d.FamilyName))
                {
                    if (!_diagnosisFamilyLog.ContainsKey(d.FamilyName))
                        _diagnosisFamilyLog.Add(d.FamilyName, 0);

                    _diagnosisFamilyLog[d.FamilyName]++;
                }
            }
        }

        public void RemoveFromLog(GeneralDiagnosis d)
        {
            lock (_lockObject)
            {
                if (!String.IsNullOrEmpty(d.FamilyName))
                {
                    if (_diagnosisFamilyLog.ContainsKey(d.FamilyName))
                        _diagnosisFamilyLog[d.FamilyName]--;
                }
            }
        }

        public void Clear()
        {
            lock (_lockObject)
            {
                _diagnosisFamilyLog.Clear();
            }
        }

        public bool DiagnosisFamilyActive(string diagnosisFamilyName)
        {
            lock (_lockObject)
            {
                return ((_diagnosisFamilyLog.ContainsKey(diagnosisFamilyName) &&
                         _diagnosisFamilyLog[diagnosisFamilyName] > 0));
            }
        }
    }
}