using System;

namespace Idpsa.Control.Diagnosis
{
    [Flags]
    public enum DiagnosisType
    {
        Event = 1,
        Step = 2,
        Security = 4,
        All = Event | Step | Security
    }
}