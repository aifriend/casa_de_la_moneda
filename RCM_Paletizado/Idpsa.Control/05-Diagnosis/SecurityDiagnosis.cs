using System;

namespace Idpsa.Control.Diagnosis
{
    [Serializable]
    public abstract class SecurityDiagnosis : GeneralDiagnosis
    {
        protected SecurityDiagnosis() { }

        public abstract bool Activated();
    }
}