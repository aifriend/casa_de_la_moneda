using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Diagnosis;
using Idpsa.Control.Manuals;
using Idpsa.Control.Sequence;
using Idpsa.Control.Subsystem;

namespace Idpsa.Paletizado
{
    public class MesaRodillos1 : IChainControllersOwner, IRi, IDiagnosisOwner
    {
        #region Miembros de IDiagnosisOwner

        private const string _name = "mesa rodillos 1";
        [Manual(SuperGroup = "Mesas", Group = "Mesa 1")] private readonly ICylinder _centrador;
        private readonly ChainController _chainController;
        [Manual(SuperGroup = "Mesas", Group = "Mesa 1")] private readonly ICylinder _escamoteable;

        [Manual(SuperGroup = "Mesas", Group = "Mesa 1", Description = "Motor")] private readonly
            ActuatorWithInversorSimple _motor;

        [Manual(SuperGroup = "Estado", Group = "Socitors/Supliers")] private readonly NormalPaletizer _paletizer;
        [Manual(SuperGroup = "Mesas", Group = "Mesa 1")] private readonly IEvaluable _presencia;
        private TON _timer;

        public MesaRodillos1(ICylinder centrador, ICylinder escamoteable, IEvaluable presencia,
                             ActuatorWithInversorSimple motor, NormalPaletizer paletizer)
        {
            _centrador = centrador;
            _escamoteable = escamoteable;
            _presencia = presencia;
            _motor = motor;
            _paletizer = paletizer;
            _paletizer.WithRequestElementAllowed(e => (e == ElementTypes.Palet) ? !_presencia.Value() : true);
            _chainController = new ChainController();
            PutWaitPosition_Steps();
            CenterPaletChain_Steps();
            QuitPaletizer_Steps();
        }

        #region Miembros de IMesaRodillos

        [Subsystem]
        public NormalPaletizer Paletizer
        {
            get { return _paletizer; }
        }

        public void PutWaitPositionChain()
        {
            _chainController.CallChain(Chains.PutWaitPosition + " " + _name);
        }

        private void PutWaitPosition_Steps()
        {
            Subchain chain = _chainController.AddChain(Chains.PutWaitPosition + " " + _name);

            chain.Add(new Step("Reposo escamoteable y retirar centrador mesa rodillos"))
                .Task = () =>
                            {
                                _escamoteable.Rest();
                                _centrador.Rest();
                                _chainController.NextStep();
                            };

            chain.Add(new Step("Comprobar cilindro escamoteable mesa rodillos 1 en reposo", 5000))
                .Task = () =>
                            {
                                if (_escamoteable.InRest)
                                    _chainController.NextStep();
                            };

            chain.Add(new Step("Comprobar centrador mesa retirado", 5000))
                .Task = () =>
                            {
                                if (_centrador.InRest)
                                {
                                    _chainController.Return();
                                }
                            };
        }

        public bool InWaitPosition()
        {
            return (_centrador.InRest && _escamoteable.InRest);
        }

        public void CenterPaletChain()
        {
            _chainController.CallChain(Chains.CenterPalet + " " + _name);
        }

        private void CenterPaletChain_Steps()
        {
            Subchain chain = _chainController.AddChain(Chains.CenterPalet + " " + _name);
            _timer = new TON();

            chain.Add(new Step("Preparar mesa rodillos 1 para centrage")).Task = () =>
                                                                                     {
                                                                                         _timer.Reset();
                                                                                         _centrador.Rest();
                                                                                         _escamoteable.Work();
                                                                                         _chainController.NextStep();
                                                                                     };

            chain.Add(new Step("Comprobar cilindro escamoteable mesa rodillos 1 en trabajo", 5000)).Task = () =>
                                                                                                               {
                                                                                                                   if (
                                                                                                                       _escamoteable
                                                                                                                           .
                                                                                                                           InWork)
                                                                                                                       _chainController
                                                                                                                           .
                                                                                                                           NextStep
                                                                                                                           ();
                                                                                                               };

            chain.Add(new Step("Comprobar centrador mesa rodillos 1 retirado", 5000)).Task = () =>
                                                                                                 {
                                                                                                     if (
                                                                                                         _centrador.
                                                                                                             InRest)
                                                                                                         _chainController
                                                                                                             .NextStep();
                                                                                                 };

            chain.Add(new Step("Poner en marcha motor mesa rodillos 1")).Task = () =>
                                                                                    {
                                                                                        _motor.Activate1();
                                                                                        _chainController.NextStep();
                                                                                    };

            chain.Add(new Step("Detectar palet en mesa rodillos 1", 10000)).Task = () =>
                                                                                       {
                                                                                           if (
                                                                                               _timer.TimingWithReset(
                                                                                                   1000, /*true*/
                                                                                                   _presencia.Value()))
                                                                                           {
                                                                                               _motor.Deactivate();
                                                                                               _chainController.NextStep
                                                                                                   ();
                                                                                           }
                                                                                       };

            chain.Add(new Step("Comprobar centrador mesa rodillos 1 en trabajo", 5000)).Task = () =>
                                                                                                   {
                                                                                                       _centrador.Work();
                                                                                                       if (
                                                                                                           _timer.Timing
                                                                                                               (2000))
                                                                                                       {
                                                                                                           PutWaitPositionChain
                                                                                                               ();
                                                                                                       }
                                                                                                   };

            chain.Add(new Step("Actualizar estado", 5000)).Task = () =>
                                                                      {
                                                                          Paletizer.PaletCentered();
                                                                          _chainController.Return();
                                                                      };
        }

        public void QuitPaletizerPChain()
        {
            _chainController.CallParallelChain(Chains.QuitPaletizer + " " + _name);
        }

        public bool PaletizerQuitted()
        {
            return _chainController.Join(Chains.QuitPaletizer + " " + _name);
        }

        private void QuitPaletizer_Steps()
        {
            Subchain chain = _chainController.AddChain(Chains.QuitPaletizer + " " + _name);

            chain.Add(new Step("Poner en posicion de reposo")).Task = () =>
                                                                          {
                                                                              _timer = new TON();
                                                                              PutWaitPositionChain();
                                                                          };

            chain.Add(new Step("Activar motor mesa rodillos")).Task = () =>
                                                                          {
                                                                              _motor.Activate1();
                                                                              _chainController.NextStep();
                                                                          };

            chain.Add(new Step("Temporizar salida palet mesa rodillos 1")).Task = () =>
                                                                                      {
                                                                                          if (
                                                                                              _timer.TimingWithReset( /*10000*/
                                                                                                  8000, /*true*/
                                                                                                  !_presencia.Value()))
                                                                                          {
                                                                                              _motor.Deactivate();
                                                                                              _chainController.Return();
                                                                                          }
                                                                                      };
        }

        #endregion

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

        #region IDiagnosisOwner Members

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
                               ("Elemento detectado en mesa de paletizado",
                                "Quite elemento de mesa de paletizado",
                                DiagnosisType.Step,
                                diagnosisSignal.Value)
                       };
        }

        #endregion

        public void SetNewCatalog(DatosCatalogoPaletizado datosCatalogo)
        {
            PaletizerDefinition auxPaletizerDef = new PaletizerDefinition(datosCatalogo.PaletizerDefinition);
            int NMosaics=datosCatalogo.PaletizerDefinition.MosaicTypes.Count;
            if (NMosaics > Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos)
            {
                IPaletizable item = PaletizadoElements.Create(PaletizableTypes.box);
                Mosaic penulM = new Mosaic(item, datosCatalogo.PaletizerDefinition.Palet, auxPaletizerDef.MosaicTypes[Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos-1]);
                Mosaic ulM = new Mosaic(item, datosCatalogo.PaletizerDefinition.Palet, auxPaletizerDef.MosaicTypes[Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos]);
                if(penulM.TotalItems()!=ulM.TotalItems())
                    for (int i = 0; i < Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos; i++)
                    {
                        auxPaletizerDef.MosaicTypes[i] = auxPaletizerDef.MosaicTypes[i + 1];
                    }
            }
            DatosCatalogoPaletizado auxDatos = new DatosCatalogoPaletizado(datosCatalogo, auxPaletizerDef);
            Paletizer.StartPaletizer(auxDatos);
            bool b = NMosaics > Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos && datosCatalogo.SotoredData != null;
            if (b && datosCatalogo.SotoredData.GetDataPaletizer(Idpsa.Paletizado.Definitions.Locations.Paletizado3Japonesa).Boxes.Count +
                            datosCatalogo.SotoredData.GetDataPaletizer(Idpsa.Paletizado.Definitions.Locations.Paletizado2Japonesa).Boxes.Count > 0)
                Paletizer.LowPriority = true;
            else
                Paletizer.LowPriority = false;
        }

        #region Nested type: Chains

        private enum Chains
        {
            CenterPalet,
            QuitPaletizer,
            PutWaitPosition
        }

        #endregion
    }

    #endregion
}