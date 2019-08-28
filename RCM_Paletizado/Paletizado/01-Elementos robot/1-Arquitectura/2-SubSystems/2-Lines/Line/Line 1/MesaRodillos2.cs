using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Diagnosis;
using Idpsa.Control.Manuals;
using Idpsa.Control.Sequence;
using Idpsa.Control.Subsystem;

namespace Idpsa.Paletizado
{
    public class MesaRodillos2 : IChainControllersOwner, IDiagnosisOwner, IRi
    {
        private const string _name = "mesa rodillos 2";

        private readonly ChainController _chainController;

        [Manual(SuperGroup = "Mesas", Group = "Mesa 2", Description = "Tope escamoteable")] private readonly ICylinder
            _escamoteable;

        [Manual(SuperGroup = "Mesas", Group = "Mesa 2", Description = "Motor")] private readonly
            ActuatorWithInversorSimple _motor;

        [Manual(SuperGroup = "Estado", Group = "Socitors/Supliers")] private readonly Despaletizer _paletizer;

        [Manual(SuperGroup = "Mesas", Group = "Mesa 2")] private readonly IEvaluable _presencia;
        private TON _timer;


        public MesaRodillos2(ICylinder escamoteable, IEvaluable presencia, ActuatorWithInversorSimple motor,
                             Despaletizer paletizer)
        {
            _escamoteable = escamoteable;
            _presencia = presencia;
            _motor = motor;
            _paletizer = paletizer;
            _paletizer.WithRequestElementAllowed(e => (e == ElementTypes.Paletizer) ? !_presencia.Value() : true);
            _chainController = new ChainController();
            PutPaletizerChain_Steps();
        }

        [Subsystem]
        public Despaletizer Paletizer
        {
            get { return _paletizer; }
        }

        #region Miembros de IChainControllersOwner

        public IEnumerable<IChainController> GetChainControllers()
        {
            return new IChainController[] {_chainController};
        }

        #endregion

        #region Miembros de IRi

        public void Ri()
        {
            _motor.Deactivate();
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
                               ("Elemento detectado en mesa despaletizado",
                                "Quite elemento de mesa de despaletizado",
                                DiagnosisType.Step,
                                () => diagnosisSignal.Value())
                       };
        }

        #endregion

        public void SetNewCatalog(DatosCatalogoPaletizado datosCatalogo)
        {
            PaletizerDefinition auxPaletizerDef = new PaletizerDefinition(datosCatalogo.PaletizerDefinition);
            int NMosaics = datosCatalogo.PaletizerDefinition.MosaicTypes.Count;
            if (NMosaics > Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos)
            {
                IPaletizable item = PaletizadoElements.Create(PaletizableTypes.box);
                Mosaic penulM = new Mosaic(item, datosCatalogo.PaletizerDefinition.Palet, auxPaletizerDef.MosaicTypes[Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos - 1]);
                Mosaic ulM = new Mosaic(item, datosCatalogo.PaletizerDefinition.Palet, auxPaletizerDef.MosaicTypes[Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos]);
                if (penulM.TotalItems() != ulM.TotalItems())
                    for (int i = 0; i < Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos; i++)
                    {
                        auxPaletizerDef.MosaicTypes[i] = auxPaletizerDef.MosaicTypes[i + 1];
                    }
            }
            DatosCatalogoPaletizado auxDatos = new DatosCatalogoPaletizado(datosCatalogo, auxPaletizerDef);
            Paletizer.StartPaletizer(auxDatos);
        }


        public void PutPaletizerPChain()
        {
            _chainController.CallParallelChain(Chains.PutPaletizer + " " + _name);
        }

        public bool PaletizerPutted()
        {
            return _chainController.Join(Chains.PutPaletizer + " " + _name);
        }

        private void PutPaletizerChain_Steps()
        {
            Subchain chain = _chainController.AddChain(Chains.PutPaletizer + " " + _name);

            //chain.Add(new Step("Subir tope escamoteable " + _name)).Task = () =>
            chain.Add(new Step("Inicio timer")).Task = () =>
                                                                               {
                                                                                   _timer = new TON();
                                                                                   //_escamoteable.Rest();
                                                                                   //_escamoteable.Work();//MDG.2013-04-25. Ya no hay tope
                                                                                   //MDG.2011-03-22
                                                                                   _chainController.NextStep();
                                                                               };

            //chain.Add(new Step("Comprobar tope escamoteable " + _name + "subido")).Task = () =>
            //MDG.2013-04-25. Ya nohay tope escamoteable
            chain.Add(new Step("Avanzar palet")).Task = () =>
                                                          {
                                                              //if (_escamoteable.InRest)
                                                              //if (
                                                              //    _escamoteable.
                                                              //        InWork)
                                                              //    //MDG.2011-03-22
                                                              //{
                                                                  _motor.Activate1();
                                                                  _chainController.
                                                                      NextStep();
                                                              //}
                                                          };

            chain.Add(new Step("Detectar presencia palet")).Task = () =>
                                                                       {
                                                                           if (_timer.TimingWithReset( /*20000*/
                                                                               2000, /*true*/_presencia.Value()))
                                                                           {
                                                                               _motor.Deactivate();
                                                                               _chainController.Return();
                                                                           }
                                                                       };
        }

        #region Nested type: Chains

        private enum Chains
        {
            PutPaletizer,
            PutAwayPalet
        }

        #endregion
    }
}