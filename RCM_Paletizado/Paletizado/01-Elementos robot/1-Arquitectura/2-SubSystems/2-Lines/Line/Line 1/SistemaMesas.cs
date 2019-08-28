using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Sequence;
using Idpsa.Control.Subsystem;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    public class SistemaMesasLinea1 : IChainControllersOwner, IAutomaticRunnable
    {
        #region Chains enum

        public enum Chains
        {
            TransferPaletizer
        };

        #endregion

        private readonly ChainController _chainController;
        private readonly SystemProductionPaletizado _production;


        public SistemaMesasLinea1(MesaRodillos1 mesa1, MesaRodillos2 mesa2, SystemProductionPaletizado production)
        {
            Mesa1 = mesa1;
            Mesa2 = mesa2;
            _production = production;
            _chainController = new ChainController();
            TransferPaletizer_Steps();
        }

        [Subsystem]
        public MesaRodillos1 Mesa1 { get; set; }

        [Subsystem]
        public MesaRodillos2 Mesa2 { get; set; }

        public bool SetNewCatalog(DatosCatalogoPaletizado datosCatalogo)
        {
            Mesa1.SetNewCatalog(datosCatalogo);
            Mesa2.SetNewCatalog(datosCatalogo);
            return true;
        }

        public void UnderlyningPaletizerTransfered()
        {
            Mesa2.Paletizer.UnderLyingPaletizer =
                (Mesa1.Paletizer).UnderLyingPaletizer;

            if (_production.IsCatalogReady(IDLine.Japonesa))
            {
                DatosCatalogoPaletizado catalog = _production.GetCatalog(IDLine.Japonesa);
                //Mesa1.Paletizer.StartPaletizer(catalog);
                Mesa1.Paletizer.StartNewPaletizerB(catalog);
                Mesa2.Paletizer.StartPaletizerB(catalog);
                Mesa2.Paletizer.State = Paletizer.States.QuitItem;
                Mesa1.Paletizer.State = Paletizer.States.PutPalet;

                bool b = Mesa1.Paletizer.Mosaics.Count() > Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos;
                if (b)
                    Mesa1.Paletizer.LowPriority = true;
                //MDG.2011-06-21.Para obligarle aponer el siguiente palet
            }
        }

        public void TransferPaletizerChain()
        {
            _chainController.CallChain(Chains.TransferPaletizer);
        }

        private void TransferPaletizer_Steps()
        {
            Subchain chain = _chainController.AddChain(Chains.TransferPaletizer);

            chain.Add(new Step("Ejecutar cadenas para transferencia de paletizado entre mesas de rodillos")).Task =
                () =>
                    {
                        Mesa1.QuitPaletizerPChain();
                        Mesa2.PutPaletizerPChain();
                        _chainController.NextStep();
                    };

            chain.Add(new Step("Esperar finalización transferencia de paletizado entre mesas de rodillos")).Task = () =>
                                                                                                                       {
                                                                                                                           if
                                                                                                                               (
                                                                                                                               Mesa1
                                                                                                                                   .
                                                                                                                                   PaletizerQuitted
                                                                                                                                   () &&
                                                                                                                               Mesa2
                                                                                                                                   .
                                                                                                                                   PaletizerPutted
                                                                                                                                   ())
                                                                                                                           {
                                                                                                                               UnderlyningPaletizerTransfered
                                                                                                                                   ();
                                                                                                                               _chainController
                                                                                                                                   .
                                                                                                                                   Return
                                                                                                                                   ();
                                                                                                                           }
                                                                                                                       };
        }

        #region Miembros de IChainControllersOwner

        public IEnumerable<IChainController> GetChainControllers()
        {
            return
                new IChainController[] {_chainController}.Concat(Mesa1.GetChainControllers()).Concat(
                    Mesa2.GetChainControllers());
        }

        #endregion

        #region Miembros de IAutomaticRunnable

        public IEnumerable<Chain> GetAutoChains()
        {
            return new[] {new CadAutoMesasLinea1("Mesas de rodillos", this)};
        }

        private class CadAutoMesasLinea1 : StructuredChain
        {
            private readonly SistemaMesasLinea1 _mesas;

            public CadAutoMesasLinea1(string name, SistemaMesasLinea1 mesas)
                : base(name)
            {
                _mesas = mesas;
                AddSteps();
                AddChainControllers(mesas);
            }

            protected override void AddSteps()
            {
                Main_Steps();
            }

            private void Main_Steps()
            {
                DynamicStepBody dynamicBody = null;

                MainChain.Add(new Step("Paso libre")).Task = () => NextStep();

                MainChain.Add(new Step("Seleccionar subrama a ejecutar") {StopChain = true}.WithTag("loop")).Task =
                    () =>
                        {
                            if (_mesas.Mesa1.InWaitPosition() == false)
                            {
                                dynamicBody = new DynamicStepBody(
                                    () => "Poner mesa rodillos 1 en posición de espera",
                                    () => _mesas.Mesa1.PutWaitPositionChain());
                                NextStep();
                            }

                            else if (_mesas.Mesa1.Paletizer.State == Paletizer.States.CenterPalet)
                            {
                                dynamicBody = new DynamicStepBody(
                                    () => "Centrar palet en mesa de rodillos 1",
                                    () => _mesas.Mesa1.CenterPaletChain());
                                NextStep();
                            }

                            else if (_mesas.Mesa1.Paletizer.State == Paletizer.States.QuitPaletizer &&
                                     _mesas.Mesa2.Paletizer.State == Paletizer.States.PutPaletizer)
                            {
                                dynamicBody = new DynamicStepBody(
                                    () => "Transferir paletizado entre mesas de rodillos",
                                    () => _mesas.TransferPaletizerChain());
                                NextStep();
                            }
                        };

                MainChain.Add(new Step()).SetDynamicBehaviour(() => dynamicBody);

                MainChain.Add(new Step("Paso final de cadena")).Task = () => { GoToStep("loop"); };
            }
        }

        #endregion
    }
}