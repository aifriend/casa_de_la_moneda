using System;

namespace Idpsa.Control.Diagnosis
{
    public class DiagnosisStep : GeneralDiagnosis
    {
        public DiagnosisStep(string name, string errorMessage)
        {
            ErrorMessage = errorMessage;
            Construct(name, errorMessage, String.Empty);
        }

        public DiagnosisStep(string name, string errorMessage, string familyName)
        {
            Construct(name, errorMessage, familyName); 
        }

        private void Construct(string name,string errorMessage,string familyName)
        {           
            if (familyName==null)
                throw new ArgumentNullException("familyName");

            Type = DiagnosisType.Step;
            ErrorMessage = errorMessage;
            FamilyName = familyName;
        }
       
    }
}