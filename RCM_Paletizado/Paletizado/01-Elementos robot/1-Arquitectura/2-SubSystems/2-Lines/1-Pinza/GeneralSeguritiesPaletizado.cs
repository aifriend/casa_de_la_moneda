using System.Collections.Generic;
using Idpsa.Control.Component;
using Idpsa.Control.Diagnosis;
using Idpsa.Control.Subsystem;

namespace Idpsa.Paletizado
{
    internal class GeneralSecuritiesPaletizado : IDiagnosisOwner
    {
        private readonly Bus _bus;

        public GeneralSecuritiesPaletizado(Bus bus)
        {
            _bus = bus;
        }

        #region IDiagnosisOwner Members

        public IEnumerable<SecurityDiagnosis> GetSecurityDiagnosis()
        {
            return new SecurityDiagnosis[]
                       {
                           new SecurityDiagnosisSignal(_bus.In("A020_S902"), TypeDiagnosisSignal.Seta),
                           new SecurityDiagnosisSignal(_bus.In("A601"), TypeDiagnosisSignal.Seta),
                           new SecurityDiagnosisSignal(_bus.In("A602"), TypeDiagnosisSignal.Seta),
                           //MDG.2011-06-20. Añadida notificacion seta zona ascensor de subida
                           new SecurityDiagnosisSignal(_bus.In("Q733"), TypeDiagnosisSignal.Failure),
                           //MDG.2011-06-20. Añadida notificacion error térmicos              
                           new SecurityDiagnosisSignal(_bus.In("Q734"), TypeDiagnosisSignal.Failure),
                           new SecurityDiagnosisSignal(_bus.In("Q735"), TypeDiagnosisSignal.Failure),
                           new SecurityDiagnosisSignal(_bus.In("Q736"), TypeDiagnosisSignal.Failure),
                           new SecurityDiagnosisSignal(_bus.In("Q740"), TypeDiagnosisSignal.Failure),
                           //new SecurityDiagnosisSignal(_bus.In("Q741"), TypeDiagnosisSignal.Failure),//MDG.2012-11-19.Este fallo se va a monitorizar junto con el mando de los transportes
                           new SecurityDiagnosisSignal(_bus.In("Q742"), TypeDiagnosisSignal.Failure),
                           new SecurityDiagnosisSignal(_bus.In("Q743"), TypeDiagnosisSignal.Failure)//,
                           //new SecurityDiagnosisSignal(_bus.In("Q741"), TypeDiagnosisSignal.Failure)
                       };
        }

        #endregion
    }
}