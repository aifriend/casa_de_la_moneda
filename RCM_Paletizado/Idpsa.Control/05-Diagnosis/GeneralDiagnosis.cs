using System;

namespace Idpsa.Control.Diagnosis
{
    [Serializable]
    public abstract class GeneralDiagnosis
    {   
        public string Name { get; protected set; }
        public string FamilyName { get; protected set; }
        public DiagnosisType Type { get; protected set; }
        public string ErrorMessage { get; protected set; }

        protected GeneralDiagnosis()
        {                     
            Name = FamilyName = ErrorMessage = String.Empty;
        }

        public string Id
        {
            get { return (Name + ErrorMessage); }
        }

    }
}