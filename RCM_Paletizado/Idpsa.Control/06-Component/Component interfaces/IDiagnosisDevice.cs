using Idpsa.Control.Sequence;

namespace Idpsa.Control.Component

{
    public interface IDiagnosisDevice
    {
        int Time { get; set; }
        bool ActivateDiagnonis(bool actDiagnosis);
        string GetErrorMessage();
        void SetErrorMessage(string str);
        void RiseDiagnosis(Chain cad, string tipoError);
    }
}