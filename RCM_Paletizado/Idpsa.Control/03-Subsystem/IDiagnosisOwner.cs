using System.Collections.Generic;
using Idpsa.Control.Diagnosis;

namespace Idpsa.Control.Subsystem
{
    public interface IDiagnosisOwner
    {
        IEnumerable<SecurityDiagnosis> GetSecurityDiagnosis();
    }
}