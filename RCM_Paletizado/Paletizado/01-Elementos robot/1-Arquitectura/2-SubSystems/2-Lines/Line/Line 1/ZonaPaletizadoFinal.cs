using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Diagnosis;
using Idpsa.Control.Manuals;
using Idpsa.Control.Sequence;
using Idpsa.Control.Subsystem;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    public class ZonaPaletizadoFinal : IAutomaticRunnable, IDiagnosisOwner
    {
        [Manual(SuperGroup = "Estado", Group = "Socitors/Supliers")] private readonly EspecialPaletizer _paletizer;
        [Manual(SuperGroup = "General", Group = "Sensores")] private readonly IEvaluable _presencia;
        private readonly IdpsaSystemPaletizado _sys;

        public ZonaPaletizadoFinal(IEvaluable presencia, EspecialPaletizer paletizer, IdpsaSystemPaletizado sys)
        {
            _sys = sys;
            _presencia = presencia;
            _paletizer = paletizer;
            paletizer.WithRequestElementAllowed(e => (e == ElementTypes.Palet) ? !_presencia.Value() : true);
        }

        [Subsystem]
        public EspecialPaletizer Paletizer
        {
            get { return _paletizer; }
        }

        public bool SetNewCatalog(DatosCatalogoPaletizado datosCatalogo)
        {
            return true;
        }

        #region Nested type: CadAutoZonaPaletizadoFinal

        private class CadAutoZonaPaletizadoFinal : StructuredChain
        {
            private const int _timeQuitPalet = 10000;
            private readonly EspecialPaletizer _paletizer;
            private readonly SystemProductionPaletizado _production;
            private IEvaluable _presencia;
            private TON _timer;

            public CadAutoZonaPaletizadoFinal(string name, ZonaPaletizadoFinal zonaPaletizadoFinal) : base(name)
            {
                _paletizer = zonaPaletizadoFinal._paletizer;
                _presencia = zonaPaletizadoFinal._presencia;
                _production = zonaPaletizadoFinal._sys.Production;
                AddSteps();
            }

            protected override void AddSteps()
            {
                Main_Steps();
            }

            private void Main_Steps()
            {
                MainChain.Add(new Step("Paso inicial")).Task = () =>
                                                                   {
                                                                       ResetTimers();
                                                                       NextStep();
                                                                   };

                MainChain.Add(new Step("Comprobar paletizado retirado linea 1") {StopChain = true}).Task = () =>
                                                                                                               {
                                                                                                                   if (
                                                                                                                       _timer
                                                                                                                           .
                                                                                                                           TimingWithReset
                                                                                                                           (_timeQuitPalet,
                                                                                                                            /*!_presencia.Value()*/
                                                                                                                            true &&
                                                                                                                            _paletizer
                                                                                                                                .
                                                                                                                                State ==
                                                                                                                            Paletizado
                                                                                                                                .
                                                                                                                                Paletizer
                                                                                                                                .
                                                                                                                                States
                                                                                                                                .
                                                                                                                                QuitPaletizer))
                                                                                                                   {
                                                                                                                       if
                                                                                                                           (
                                                                                                                           _production
                                                                                                                               .
                                                                                                                               IsCatalogReady
                                                                                                                               (IDLine
                                                                                                                                    .
                                                                                                                                    Japonesa))
                                                                                                                       {
                                                                                                                           //_paletizer.StartPaletizer(_production.GetCatalog(IDLine.Japonesa));
                                                                                                                           _paletizer
                                                                                                                               .
                                                                                                                               StartNewPaletizer
                                                                                                                               (_production
                                                                                                                                    .
                                                                                                                                    GetCatalog
                                                                                                                                    (IDLine
                                                                                                                                         .
                                                                                                                                         Japonesa));
                                                                                                                           //MDG.2011-06-28
                                                                                                                           NextStep
                                                                                                                               ();
                                                                                                                       }
                                                                                                                   }
                                                                                                               };

                MainChain.Add(new Step("Paso final de cadena")).Task = () =>
                                                                           {
                                                                               ResetTimers();
                                                                               PreviousStep();
                                                                           };
            }

            private void ResetTimers()
            {
                _timer = new TON();
            }
        }

        #endregion

        #region Miembros de IAutomaticRunnable

        IEnumerable<Chain> IAutomaticRunnable.GetAutoChains()
        {
            return new[] {new CadAutoZonaPaletizadoFinal(_paletizer.Name, this)};
        }

        #endregion

        #region Miembros de IDiagnosisOwner

        IEnumerable<SecurityDiagnosis> IDiagnosisOwner.GetSecurityDiagnosis()
        {
            IEvaluable diagnosisSignal =
                Evaluable.FromFunctor(
                    () =>
                    _paletizer.State == Paletizado.Paletizer.States.PutPalet &&
                    _presencia.Value()
                    )
                    .DelayToConnection(6000)
                    .DelayToDisconnection(6000);

            return new[]
                       {
                           new SecurityDiagnosisCondition
                               ("Elemento detectado en zona de paletizado final japonesa",
                                "Quite elemento de zona de paletizado",
                                DiagnosisType.Step,
                                diagnosisSignal.Value)
                       };
        }

        #endregion
    }
}