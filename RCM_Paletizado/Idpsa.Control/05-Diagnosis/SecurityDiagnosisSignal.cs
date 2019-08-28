using System;
using Idpsa.Control.Component;

namespace Idpsa.Control.Diagnosis
{
    [Serializable]
    public class SecurityDiagnosisSignal : SecurityDiagnosis
    {        
        private readonly Input _signal;        
        private readonly TypeDiagnosisSignal _typeSignal;

        public SecurityDiagnosisSignal(Input signal, TypeDiagnosisSignal typeSignal, string errorMessage)
        {
            if (signal == null)
                throw new ArgumentNullException("signal");
            
            if (errorMessage == null)
                throw new ArgumentNullException("errorMessage");

            _signal = signal;
            _typeSignal = typeSignal;
            Type =
                (DiagnosisType)
                Enum.Parse(typeof (DiagnosisType), (((int)(typeSignal)) & ((int)(DiagnosisType.All))).ToString());
            ErrorMessage = errorMessage;
            if (ErrorMessage == String.Empty)
            {
                ErrorMessage = signal.Description;
            }
            Name = _signal.Symbol;
        }

        public SecurityDiagnosisSignal(Input signal, TypeDiagnosisSignal typeSinal) : this(signal, typeSinal, ""){}
      
        public TypeDiagnosisSignal TypeSignal
        {
            get { return _typeSignal; }
        }

        public override bool Activated()
        {
            bool result = false;
            switch (TypeSignal)
            {
                case TypeDiagnosisSignal.Seta:
                case TypeDiagnosisSignal.Micro:
                    result = _signal.Value();
                    break;
                case TypeDiagnosisSignal.Failure:
                case TypeDiagnosisSignal.Barrera:
                    result = !_signal.Value();
                    break;
            }
            return result;
        }
    }
}