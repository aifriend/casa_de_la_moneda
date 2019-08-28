using System;
using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals;
using Idpsa.Control.Sequence;

namespace Idpsa.Paletizado
{
    [Serializable]
    public class Etiquetadora : IChainControllersOwner
    {
        private readonly BandaEtiquetado _banda;
        private readonly ChainController _chainController;
        private bool _cajaEnSalida;
        private bool _entradaLibre;
        private bool _entrandoCaja;
        private bool _etiquetando;

        public Etiquetadora(Zebra_CajasPrinter zebraPrinter,
                            Sucker ventosa, ICylinder extensor,
                            ICylinder retirador, ICylinder girador, BandaEtiquetado banda)
        {
            ZebraPrinter = zebraPrinter;
            Ventosa = ventosa;
            Extensor = extensor;
            Retirador = retirador;
            Girador = girador;
            _banda = banda;
            _chainController = new ChainController();
            Etiquetando_Steps();
        }

        [Manual(SuperGroup = "Encajado", Group = "Etiquetado", Description = "Aspiracion")]
        public Sucker Ventosa { get; private set; }

        [Manual(SuperGroup = "Encajado", Group = "Etiquetado", Description = "Extensor")]
        public ICylinder Extensor { get; private set; }

        [Manual(SuperGroup = "Encajado", Group = "Etiquetado", Description = "Retirador")]
        public ICylinder Retirador { get; private set; }

        [Manual(SuperGroup = "Encajado", Group = "Etiquetado", Description = "Giro")]
        public ICylinder Girador { get; private set; }

        public Zebra_CajasPrinter ZebraPrinter { get; private set; }

        #region IChainControllersOwner Members

        public IEnumerable<IChainController> GetChainControllers()
        {
            return new IChainController[] {_chainController};
        }

        #endregion

        public void Etiquetando_Steps()
        {
            Subchain chain = _chainController.AddChain(BandaEtiquetado.States.Etiquetando);

            var timerImpresora = new TON();
            var timerAspirado = new TON();
            var timerPegado = new TON();
            CajaPasaportes _cajaEtiquetado = null;


            chain.Add(new Step("Preparar elementos para girar a 0º maquina etiquetadora")).Task = () =>
                                                                                                      {
                                                                                                          timerImpresora
                                                                                                              .Reset();
                                                                                                          timerAspirado.
                                                                                                              Reset();
                                                                                                          timerPegado.
                                                                                                              Reset();
                                                                                                          Ventosa.
                                                                                                              VaccumOff();
                                                                                                          Extensor.Rest();
                                                                                                          _chainController
                                                                                                              .NextStep();
                                                                                                      };


            chain.Add(new Step("Comprobar cilindro extensor maquina etiquetadora recogido", 5000)).Task = () =>
                                                                                                              {
                                                                                                                  if (
                                                                                                                      Extensor
                                                                                                                          .
                                                                                                                          InRest)
                                                                                                                  {
                                                                                                                      Retirador
                                                                                                                          .
                                                                                                                          Work
                                                                                                                          ();
                                                                                                                      _chainController
                                                                                                                          .
                                                                                                                          NextStep
                                                                                                                          ();
                                                                                                                  }
                                                                                                              };

            chain.Add(new Step("Comprobar cilindro retirador maquina etiquetadora recogido", 5000)).Task = () =>
                                                                                                               {
                                                                                                                   if (
                                                                                                                       Retirador
                                                                                                                           .
                                                                                                                           InWork)
                                                                                                                   {
                                                                                                                       _chainController
                                                                                                                           .
                                                                                                                           NextStep
                                                                                                                           ();
                                                                                                                   }
                                                                                                               };


            chain.Add(new Step("Cilindro girador maquina etiquetadora a posición vertical")).Task = () =>
                                                                                                        {
                                                                                                            Girador.Rest
                                                                                                                ();
                                                                                                            _chainController
                                                                                                                .
                                                                                                                NextStep
                                                                                                                ();
                                                                                                        };


            chain.Add(new Step("Comprobar cilindro girador maquina etiquetadora en posición vertical", 5000)).Task =
                () =>
                    {
                        if (Girador.InRest)
                        {
                            _chainController.NextStep();
                        }
                    };

            chain.Add(new Step("Estirado cilindro retirador maquina etiquetadora")).Task = () =>
                                                                                               {
                                                                                                   Retirador.Rest();
                                                                                                   _chainController.
                                                                                                       NextStep();
                                                                                               };

            chain.Add(new Step("Comprobar cilindro retirador maquina etiquetadora estirado", 5000)).Task = () =>
                                                                                                               {
                                                                                                                   if (
                                                                                                                       Retirador
                                                                                                                           .
                                                                                                                           InRest)
                                                                                                                   {
                                                                                                                       _chainController
                                                                                                                           .
                                                                                                                           NextStep
                                                                                                                           ();
                                                                                                                   }
                                                                                                               };

            chain.Add(new Step("Esperar caja para imprimir etiqueta de pasaportes")).Task = () =>
                                                                                                {
                                                                                                    if (_banda.State ==
                                                                                                        BandaEtiquetado.
                                                                                                            States.
                                                                                                            Etiquetando)
                                                                                                    {
                                                                                                        _cajaEtiquetado
                                                                                                            =
                                                                                                            _banda.Caja;

                                                                                                        if (
                                                                                                            _cajaEtiquetado !=
                                                                                                            null)
                                                                                                        {
                                                                                                            Ventosa.
                                                                                                                VaccumOn
                                                                                                                ();
                                                                                                            ZebraPrinter
                                                                                                                .Print(
                                                                                                                    _cajaEtiquetado,false);
                                                                                                            timerImpresora
                                                                                                                .Reset();
                                                                                                            _chainController
                                                                                                                .
                                                                                                                NextStep
                                                                                                                ();
                                                                                                        }
                                                                                                    }
                                                                                                };

            chain.Add(new Step("Esperar salida de etiqueta")).Task = () =>
                                                                         {
                                                                             if (timerImpresora.Timing(6000))
                                                                                 _chainController.NextStep();
                                                                         };

            chain.Add(new Step("Preparar elementos para girar a 90º maquina etiquetadora")).Task = () =>
                                                                                                       {
                                                                                                           Retirador.
                                                                                                               Work();
                                                                                                           Extensor.Rest
                                                                                                               ();
                                                                                                           _chainController
                                                                                                               .NextStep
                                                                                                               ();
                                                                                                       };

            chain.Add(new Step("Comprobar cilindro retirador maquina etiquetadora recogido", 5000)).Task = () =>
                                                                                                               {
                                                                                                                   if (
                                                                                                                       Retirador
                                                                                                                           .
                                                                                                                           InWork)
                                                                                                                       _chainController
                                                                                                                           .
                                                                                                                           NextStep
                                                                                                                           ();
                                                                                                               };

            chain.Add(new Step("Comprobar cilindro extensor maquina etiquetadora recogido", 5000)).Task = () =>
                                                                                                              {
                                                                                                                  if (
                                                                                                                      Extensor
                                                                                                                          .
                                                                                                                          InRest)
                                                                                                                  {
                                                                                                                      Girador
                                                                                                                          .
                                                                                                                          Work
                                                                                                                          ();
                                                                                                                      _chainController
                                                                                                                          .
                                                                                                                          NextStep
                                                                                                                          ();
                                                                                                                  }
                                                                                                              };

            chain.Add(new Step("Comprobar cilindro girador maquina etiquetadora en posición horizontal", 5000)).Task =
                () =>
                    {
                        if (Girador.InWork)
                        {
                            Extensor.Work();
                            _chainController.NextStep();
                        }
                    };

            chain.Add(new Step("Comprobar cilindro extensor maquina etiquetadora estirado", 5000)).Task = () =>
                                                                                                              {
                                                                                                                  if (
                                                                                                                      Extensor
                                                                                                                          .
                                                                                                                          InWork)
                                                                                                                      _chainController
                                                                                                                          .
                                                                                                                          NextStep
                                                                                                                          ();
                                                                                                              };

            chain.Add(new Step("Temporizar pegado de etiqueta en caja")).Task = () =>
                                                                                    {
                                                                                        if (timerPegado.Timing(2000))
                                                                                        {
                                                                                            Ventosa.VaccumOff();
                                                                                            _chainController.NextStep();
                                                                                        }
                                                                                    };

            chain.Add(new Step("Temporizar desativación de ventosa")).Task = () =>
                                                                                 {
                                                                                     if (timerPegado.Timing(2000))
                                                                                         _chainController.NextStep();
                                                                                 };

            chain.Add(new Step("Estirar cilindro extensor maquina etiquetadora")).Task = () =>
                                                                                             {
                                                                                                 Extensor.Rest();
                                                                                                 _chainController.
                                                                                                     NextStep();
                                                                                             };

            chain.Add(new Step("Comprobar cilindro extensor maquina etiquetadora estirado", 5000)).Task = () =>
                                                                                                              {
                                                                                                                  if (
                                                                                                                      Extensor
                                                                                                                          .
                                                                                                                          InRest)
                                                                                                                  {
                                                                                                                      _cajaEtiquetado
                                                                                                                          .
                                                                                                                          CodigoBarras
                                                                                                                          =
                                                                                                                          ZebraPrinter
                                                                                                                              .
                                                                                                                              CalculateBarcode
                                                                                                                              (_cajaEtiquetado);
                                                                                                                      Girador
                                                                                                                          .
                                                                                                                          Rest
                                                                                                                          ();
                                                                                                                      _banda
                                                                                                                          .
                                                                                                                          State
                                                                                                                          =
                                                                                                                          BandaEtiquetado
                                                                                                                              .
                                                                                                                              States
                                                                                                                              .
                                                                                                                              LlevarPosicionCogida;
                                                                                                                      _chainController
                                                                                                                          .
                                                                                                                          Return
                                                                                                                          ();
                                                                                                                  }
                                                                                                              };
        }
    }
}