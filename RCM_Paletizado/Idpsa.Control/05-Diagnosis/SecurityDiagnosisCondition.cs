using System;

namespace Idpsa.Control.Diagnosis
{
    [Serializable]
    public class SecurityDiagnosisCondition : SecurityDiagnosis
    {       
      
        private readonly Func<bool> _securityCondition;
       
        public SecurityDiagnosisCondition(string name, string errorMessage, DiagnosisType type,
                                          Func<bool> securityCondition,string diagnosisFamily)
            
        {                    
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("name can't be null or empty");

            if (errorMessage ==null)
                throw new ArgumentNullException("errorMessage");

            if (securityCondition == null)
                throw new ArgumentNullException("securityCondition");

            if (diagnosisFamily == null)
                throw new ArgumentNullException("diagnosisFamily");

            Name = name;
            Type = type;
            ErrorMessage = errorMessage;
            _securityCondition = securityCondition;
        }
        
        public SecurityDiagnosisCondition(string name, string errorMessage, DiagnosisType type,
                                          Func<bool> securityCondition)
            : this(name,errorMessage,type,securityCondition,String.Empty){}
            

        public override bool Activated()
        {
            return _securityCondition();
        }
    }
}