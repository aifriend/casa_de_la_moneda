using System;

namespace Idpsa.Control.Diagnosis
{
    [Flags]
    public enum TypeDiagnosisSignal
    {
        Seta = DiagnosisType.Event | DiagnosisType.Security | 8,
        Micro = DiagnosisType.Event | DiagnosisType.Security | 16,
        Barrera = DiagnosisType.Event | DiagnosisType.Security | 32,
        Failure = DiagnosisType.Event | 64
    }
}