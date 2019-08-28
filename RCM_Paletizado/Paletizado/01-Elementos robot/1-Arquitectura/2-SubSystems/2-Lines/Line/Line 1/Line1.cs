using System;
using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Engine;
using Idpsa.Control.Subsystem;
using Idpsa.Control.Tool;
using Idpsa.Paletizado.Definitions;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    public class Line1 : Line
    {
        private const bool PaletizeDirectly = false; //Reactivacion funcionamiento normal//true;

        private const Paletizer.States PaletizerFirstState =
            PaletizeDirectly ? Paletizer.States.PutItem : Paletizer.States.PutPalet; //MDG.2011-03-11

        private readonly Bus _bus;
        private readonly IdpsaSystemPaletizado _sys;
        public bool ModoAcumulacion;
        public SourceGroupSupplier Supplier { get; private set; }

        public Line1(IdpsaSystemPaletizado sys,
                     SourceGroupSupplier supplier,
                     IItemSolicitor<GrupoPasaportes> solicitor,
                     LinesSemaphore semaphore)
            : base(IDLine.Japonesa)
        {
            _bus = sys.Bus;
            _sys = sys;
            Supplier = supplier;
            Func<string, CajaPasaportes> boxGetter = Supplier.GetBox;

            NormalPaletizer initialPaletizer = CreateInitialPaletizer(boxGetter);
            if (!PaletizeDirectly)
                AddSolicitor(initialPaletizer);

            Despaletizer despaletizer = CreateDespaletizer(boxGetter);
            if (!PaletizeDirectly)
            {
                AddSolicitor(despaletizer);
                AddSupplier(despaletizer);
            }

            EspecialPaletizer finalPaletizer = CreateFinalPaletizer(boxGetter, despaletizer);
            AddSolicitor(finalPaletizer);

            PaletStore = CreatePaletStore();
            AddSupplier(PaletStore);

            Mesas = CreateSystemaMesas(initialPaletizer, despaletizer, sys.Production);

            ZonaPaletizadoFinal = new ZonaPaletizadoFinal(
                new SensorN(_bus.In("B821"), "presencia paletizado japonesa"), finalPaletizer, sys);

            //MDG. pasmos el sourcegroup supplier para poder comparar lo recibido con lo cargado en el catalogo de grupos
            EnlaceJaponesa = CreateEnlaceJaponesa(solicitor, semaphore, Supplier);

            TransporteLinea = new TransporteLinea1(_sys, Supplier, solicitor, semaphore);

        }

        [Subsystem]
        public SistemaMesasLinea1 Mesas { get; private set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        public EnlaceJaponesa EnlaceJaponesa { get; set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        public TransporteLinea1 TransporteLinea { get; private set; }

        public bool BoxNotCatchedDespaletizing { get; set; }//MDG.2013-04-25

        private SistemaMesasLinea1 CreateSystemaMesas(NormalPaletizer initialPaletizer, Despaletizer despaletizer,
                                                      SystemProductionPaletizado production)
        {
            ICylinder centrador =
                new Cylinder1Efect
                    (
                    _bus.In("B313A"), _bus.In("B313B"),
                    _bus.Out("Y313"), "Centrador transversal"
                    );

            ICylinder escamoteable =
                new Cylinder1Efect
                    (_bus.In("B315B"), _bus.In("B315A"),
                     _bus.Out("Y315"), "Tope escamoteable")
                    .WithRestName("Bajar")
                    .WithWorkName("Subir");

            var presencia1 = new SensorP(_bus.In("B818"), "Presencia");
            var motor1 = new ActuatorWithInversorSimple(_bus.Out("K735A"), _bus.Out("K735B"));
            motor1.WithJogPosName("Adelante")
                .WithJogNegName("Atras")
                .WithInvertedJogSense(false);

            var mesa1 = new MesaRodillos1(centrador, escamoteable, presencia1, motor1, initialPaletizer);

            var presencia2 = new SensorP(_bus.In("B820"), "Presencia");
            var motor2 = new ActuatorWithInversorSimple(_bus.Out("K736A"), _bus.Out("K736B"));
            motor2.WithJogPosName("Adelante")
                .WithJogNegName("Atras")
                .WithInvertedJogSense(false);

            ICylinder escamoteable2 =
                new Cylinder1Efect1Sensor(
                    _bus.In("B316"), _bus.Out("Y316"), "Tope escamoteable")
                    .WithRestName("Subir")
                    .WithWorkName("Bajar");

            var mesa2 = new MesaRodillos2(escamoteable2, presencia2, motor2, despaletizer);

            return new SistemaMesasLinea1(mesa1, mesa2, production);
        }

        public override bool SetNewCatalog(DatosCatalogoPaletizado datosCatalogo)
        {
            PaletStore.SetNewCatalog(ProveCatalog.NInicialPalets, datosCatalogo.PaletizerDefinition.Palet);
            ZonaPaletizadoFinal.Paletizer.StartPaletizer(datosCatalogo);
            Mesas.SetNewCatalog(datosCatalogo);
            Mesas.SetNewCatalog(datosCatalogo);
            if (datosCatalogo.SotoredData != null)
            {
                //MDG.2010-12-13. Para evitar que dé error al crearse de nuevo el catalogo
                Supplier.CurrentIndex = datosCatalogo.SotoredData.GetCurrectGroupIndex();
                //MDG.2010-12-09.Lo cargo antes que los datos del palet para que no me lo sobre escriba con el sabe interno que hace
            }
            _sys.Production.SaveCatalogs();
            return true;
        }

        //private Paletizer.States _paletizerFirstState = Paletizer.States.PutItem;

        private NormalPaletizer CreateInitialPaletizer(Func<string, CajaPasaportes> boxGetter)
        {
            var settings = new PaletizerSettings(Paletizer.Action.Paletize, Paletizer.States.PutPalet)
                               {
                                   InitialState = PaletizerFirstState,
                                   MinFlatsToDo = Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos,
                                   DownSeparator = false,
                                   HasSeparator = false,
                                   PaletMustBeCenteredAfterPutted = true,
                                   NewIDPalet = true
                               };

            var Geo = new GeometriaConf(); //MCR
            CornerPoint3D cornerPoint = Geo.PosMesa1.ToConerPoint3D();

            var suppliers = new Dictionary<ElementTypes, IEnumerable<Locations>>
                                {
                                    {
                                        ElementTypes.ItemJaponesa,
                                        new List<Locations> {Locations.Reproceso, Locations.Entrada}
                                        },
                                    {
                                        ElementTypes.Palet,
                                        new List<Locations> {Locations.Paletizado2Japonesa, Locations.PaletJaponesa}
                                        }
                                };

            return new NormalPaletizer("Zona inicial de paletizado " + ToStringIdLine(), cornerPoint,
                                       Locations.Paletizado1Japonesa, settings, boxGetter, suppliers, IDLine.Japonesa);
                //MDG.2011-03-10
        }

        private Despaletizer CreateDespaletizer(Func<string, CajaPasaportes> boxGetter)
        {
            var settings = new PaletizerSettings(Paletizer.Action.Despaletize, Paletizer.States.QuitItem)
                               {
                                   InitialState = Paletizer.States.PutPaletizer,
                                   MinFlatsToDo = Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos,
                                   DownSeparator = false,
                                   HasSeparator = false
                               };

            var Geo = new GeometriaConf();//MCR
            CornerPoint3D cornerPoint = Geo.PosMesa2.ToConerPoint3D();

            var suppliers = new Dictionary<ElementTypes, IEnumerable<Locations>>
                                {
                                    {ElementTypes.Paletizer, new List<Locations> {Locations.Paletizado1Japonesa}}
                                };

            return new Despaletizer("Zona de despaletizado " + ToStringIdLine(), cornerPoint,
                                    Locations.Paletizado2Japonesa, settings, boxGetter, suppliers, IDLine.Japonesa);
        }

        private EspecialPaletizer CreateFinalPaletizer(Func<string, CajaPasaportes> boxGetter, Despaletizer despaletizer)
        {
            var settings = new PaletizerSettings(Paletizer.Action.Paletize, Paletizer.States.PutPalet)
                               {
                                   InitialState = PaletizerFirstState,
                                   HasSeparator = false,
                                   DownSeparator = false
                               };

            var Geo = new GeometriaConf();
            CornerPoint3D cornerPoint = Geo.PosZonaPaletizadoFinal1.ToConerPoint3D();//MCR

            var suppliers = new Dictionary<ElementTypes, IEnumerable<Locations>>
                                {
                                    {
                                        ElementTypes.Palet,
                                        new List<Locations> {Locations.Paletizado2Japonesa, Locations.PaletJaponesa}
                                        }
                                };

            return new EspecialPaletizer("Paletizado final " + ToStringIdLine(), cornerPoint,
                                         Locations.Paletizado3Japonesa, settings, suppliers, boxGetter, despaletizer,
                                         IDLine.Japonesa, PaletizeDirectly);
        }

        private PaletStore CreatePaletStore()
        {
            var Geo = new GeometriaConf();//MCR
            Func<bool> inAutomatic =
                () => ControlLoop<IdpsaSystemPaletizado>.Instance.Sys.Control.InActiveMode(Mode.Automatic);
            return new PaletStore("almacén de palets " + ToStringIdLine(), Locations.PaletJaponesa,
                                  Geo.PosPaletStore1.ToConerPoint3D(),
                                  _bus.In("B816").OR(
                                      Evaluable.FromFunctor(() => State != SubsystemState.Activated || !inAutomatic()).
                                          WithManualRepresentation("presencia almacén palets japonesa")));
        }

        private EnlaceJaponesa CreateEnlaceJaponesa(IItemSolicitor<GrupoPasaportes> solicitor,
                                                    LinesSemaphore semaphore,
                                                    SourceGroupSupplier supplier)
        {
            IActivable entradaJaponesaLibreSignal = _bus.Out("K981");
            IEvaluable requestJaponesa = _bus.In("Input52.4");

            //return new EnlaceJaponesa(ConfigCom.EnlaceJaponesaIpEndPoint,
            //    entradaJaponesaLibreSignal, solicitor,requestJaponesa,semaphore);
            return new EnlaceJaponesa(ConfigPaletizadoComunication.EnlaceJaponesaIpEndPoint,
                                      entradaJaponesaLibreSignal, solicitor, requestJaponesa,
                                      semaphore, supplier); //MDG.2011-07-05
        }

        public override StoreStateCatalog GetDataToStore()
        {
            return new StoredStateCatalogoLinea1
                (
                Mesas.Mesa1.Paletizer.GetDataToStore(),
                Mesas.Mesa2.Paletizer.GetDataToStore(),
                ZonaPaletizadoFinal.Paletizer.GetDataToStore()
                );
        }

        public override string ToString()
        {
            return "Máquina numerado japonesa";
        }

        public void ForceAddBox(int i)
        {
            if (i == 1)
            {
                if (Mesas.Mesa2.Paletizer.UnderLyingPaletizer != null && Mesas.Mesa2.Paletizer.UnderLyingPaletizer.Count > 0)
                    return;
                if (Mesas.Mesa1.Paletizer.UnderLyingPaletizer.Count < Mesas.Mesa1.Paletizer.UnderLyingPaletizer.TotalToDoItems())
                {
                    _sys.Lines.supplierLine1.GetNewIndexForced();
                    int ind = _sys.Lines.supplierLine1.CurrentIndex;
                    GrupoPasaportes auxP = _sys.Lines.supplierLine1.GetItem(ind + 1);
                    if (auxP != null && auxP.TipoPasaporte != null)
                    {
                        CajaPasaportes box = new CajaPasaportes(auxP);
                        for (int c = 0; c < CajaPasaportes.NGrupos; c++)
                        {
                            ind = _sys.Lines.supplierLine1.CurrentIndex++;
                            box.Add(_sys.Lines.supplierLine1.GetItem(ind + 1), CajaPasaportes.NGrupos - c - 1);
                        }
                        box = _sys.Production.CatalogManagerJaponesa.GetBox(box.Id);
                        Mesas.Mesa1.Paletizer.State = Paletizer.States.PutItem;
                        if (Mesas.Mesa1.Paletizer.UnderLyingPaletizer.Count + 1 < Mesas.Mesa1.Paletizer.UnderLyingPaletizer.TotalToDoItems())
                        {
                            Mesas.Mesa1.Paletizer.ForceElementAdded(box, i);
                            Mesas.Mesa1.Paletizer.OnChanged();
                        }
                        else
                        {
                            Mesas.Mesa1.Paletizer.ForceElementAdded(box, i);
                            Mesas.UnderlyningPaletizerTransfered();
                            //Mesas.Mesa2.Paletizer.ForceElementAdded(box, 2);
                            //Mesas.Mesa2.Paletizer.ForceElementAdded(box, 5);
                            Mesas.Mesa1.Paletizer.OnChanged();
                            Mesas.Mesa2.Paletizer.OnChanged();
                        }
                        _sys.Production.SaveCatalogs();
                    }
                }
            }
            else if (i == 2)
            {
                if (Mesas.Mesa1.Paletizer.UnderLyingPaletizer.Count + ZonaPaletizadoFinal.Paletizer.UnderLyingPaletizer.Count > 0)
                    return;
                else if (Mesas.Mesa2.Paletizer.UnderLyingPaletizer.Count <= Mesas.Mesa2.Paletizer.UnderLyingPaletizer.TotalToDoItems())
                {
                    _sys.Lines.supplierLine1.GetNewIndexForced();
                    int ind = _sys.Lines.supplierLine1.CurrentIndex;
                    GrupoPasaportes auxP = _sys.Lines.supplierLine1.GetItem(ind + 1);
                    if (auxP != null && auxP.TipoPasaporte != null)
                    {
                        CajaPasaportes box = new CajaPasaportes(auxP);
                        for (int c = 0; c < CajaPasaportes.NGrupos; c++)
                        {
                            ind = _sys.Lines.supplierLine1.CurrentIndex++;
                            box.Add(_sys.Lines.supplierLine1.GetItem(ind + 1), CajaPasaportes.NGrupos - c - 1);
                        }
                        box = _sys.Production.CatalogManagerJaponesa.GetBox(box.Id);
                        if (Mesas.Mesa2.Paletizer.UnderLyingPaletizer.Count < Mesas.Mesa2.Paletizer.UnderLyingPaletizer.TotalToDoItems())
                        {
                            Mesas.Mesa2.Paletizer.ForceElementAdded(box, i);
                            Mesas.Mesa2.Paletizer.State = Paletizer.States.QuitItem;
                            Mesas.Mesa2.Paletizer.OnChanged();
                        }
                    }
                }
                _sys.Production.SaveCatalogs();
            }
            else if (i == 3)
            {
                if (Mesas.Mesa1.Paletizer.UnderLyingPaletizer.Count > 0 && Mesas.Mesa1.Paletizer.UnderLyingPaletizer.TotalToDoItems() != ZonaPaletizadoFinal.Paletizer.UnderLyingPaletizer.TotalToDoItems())
                    return;


                if (ZonaPaletizadoFinal.Paletizer.UnderLyingPaletizer.Count < (ZonaPaletizadoFinal.Paletizer.UnderLyingPaletizer.TotalToDoItems() - Mesas.Mesa2.Paletizer.UnderLyingPaletizer.TotalToDoItems())
                    && Mesas.Mesa1.Paletizer.UnderLyingPaletizer.TotalToDoItems() != ZonaPaletizadoFinal.Paletizer.UnderLyingPaletizer.TotalToDoItems())
                {
                    _sys.Lines.supplierLine1.GetNewIndexForced();
                    int ind = _sys.Lines.supplierLine1.CurrentIndex;
                    GrupoPasaportes auxP = _sys.Lines.supplierLine1.GetItem(ind + 1);
                    if (auxP != null && auxP.TipoPasaporte != null)
                    {
                        CajaPasaportes box = new CajaPasaportes(auxP);
                        for (int c = 0; c < CajaPasaportes.NGrupos; c++)
                        {
                            ind = _sys.Lines.supplierLine1.CurrentIndex++;
                            box.Add(_sys.Lines.supplierLine1.GetItem(ind + 1), CajaPasaportes.NGrupos - c - 1);
                        }
                        box = _sys.Production.CatalogManagerJaponesa.GetBox(box.Id);
                        ZonaPaletizadoFinal.Paletizer.State = Paletizer.States.PutItem;
                        ZonaPaletizadoFinal.Paletizer.ForceElementAdded(box, i);
                        ZonaPaletizadoFinal.Paletizer.OnChanged();
                        _sys.Production.SaveCatalogs();
                    }
                }
                else if (ZonaPaletizadoFinal.Paletizer.UnderLyingPaletizer.Count < ZonaPaletizadoFinal.Paletizer.UnderLyingPaletizer.TotalToDoItems())
                {
                    _sys.Lines.supplierLine1.GetNewIndexForced();
                    int ind = _sys.Lines.supplierLine1.CurrentIndex;
                    ind = (ZonaPaletizadoFinal.Paletizer.UnderLyingPaletizer.TotalToDoItems() - ZonaPaletizadoFinal.Paletizer.UnderLyingPaletizer.Count - 1) * CajaPasaportes.NGrupos + 1;

                    GrupoPasaportes auxP = _sys.Lines.supplierLine1.GetItem(ind);
                    if (auxP != null && auxP.TipoPasaporte != null)
                    {
                        CajaPasaportes box = new CajaPasaportes(auxP);
                        for (int c = 0; c < CajaPasaportes.NGrupos; c++)
                        {
                            ind = _sys.Lines.supplierLine1.CurrentIndex++;
                            box.Add(_sys.Lines.supplierLine1.GetItem(ind + 1), CajaPasaportes.NGrupos - c - 1);
                        }
                        box = _sys.Production.CatalogManagerJaponesa.GetBox(box.Id);
                        ZonaPaletizadoFinal.Paletizer.State = Paletizer.States.PutItem;
                        ZonaPaletizadoFinal.Paletizer.ForceElementAdded(box, i);
                        ZonaPaletizadoFinal.Paletizer.OnChanged();
                        _sys.Production.SaveCatalogs();
                    }
                }
            }
            _sys.Production.SaveCatalogs();
        }
        public void ForceQuitBox(int i)
        {
            if (i == 1)
            {
                _sys.Lines.supplierLine1.GetNewIndexForced();
                //int ind = _sys.Lines.supplierLine1.CurrentIndex;
                //CajaPasaportes box = new CajaPasaportes(_sys.Lines.supplierLine1.GetItem(ind + 1));
                //for (int c = 0; c < CajaPasaportes.NGrupos; c++)
                //    box.Add(_sys.Lines.supplierLine1.GetItem(ind + 1 + c), CajaPasaportes.NGrupos - c - 1);
                //Mesas.Mesa1.Paletizer.State = Paletizer.States.QuitItem;
                Mesas.Mesa1.Paletizer.ForceElementQuitted(i);
                Mesas.Mesa1.Paletizer.State = Paletizer.States.PutItem;
                _sys.Production.SaveCatalogs();
                Mesas.Mesa1.Paletizer.OnChanged();
            }
            else if (i == 2)
            {
                if (ZonaPaletizadoFinal.Paletizer.UnderLyingPaletizer.Count + Mesas.Mesa2.Paletizer.UnderLyingPaletizer.Count <= ZonaPaletizadoFinal.Paletizer.UnderLyingPaletizer.TotalToDoItems()
                    && (Mesas.Mesa1.Paletizer.UnderLyingPaletizer.Count + ZonaPaletizadoFinal.Paletizer.UnderLyingPaletizer.Count > 0))
                    return;
                _sys.Lines.supplierLine1.GetNewIndexForced();
                //int ind = _sys.Lines.supplierLine1.CurrentIndex;
                //CajaPasaportes box = new CajaPasaportes(_sys.Lines.supplierLine1.GetItem(ind + 1));
                //for (int c = 0; c < CajaPasaportes.NGrupos; c++)
                //    box.Add(_sys.Lines.supplierLine1.GetItem(ind + 1 + c), CajaPasaportes.NGrupos - c - 1);
                Mesas.Mesa2.Paletizer.State = Paletizer.States.QuitItem;
                Mesas.Mesa2.Paletizer.ForceElementQuitted(i);
                _sys.Production.SaveCatalogs();
                Mesas.Mesa2.Paletizer.OnChanged();
            }

            else if (i == 3)
            {
                _sys.Lines.supplierLine1.GetNewIndexForced();
                //int ind = _sys.Lines.supplierLine1.CurrentIndex;
                //CajaPasaportes box = new CajaPasaportes(_sys.Lines.supplierLine1.GetItem(ind + 1));
                //for (int c = 0; c < CajaPasaportes.NGrupos; c++)
                //    box.Add(_sys.Lines.supplierLine1.GetItem(ind + 1 + c), CajaPasaportes.NGrupos - c - 1);
                ZonaPaletizadoFinal.Paletizer.State = Paletizer.States.QuitItem;
                ZonaPaletizadoFinal.Paletizer.ForceElementQuitted(i);
                ZonaPaletizadoFinal.Paletizer.State = Paletizer.States.PutItem;
                _sys.Production.SaveCatalogs();
                ZonaPaletizadoFinal.Paletizer.OnChanged();
            }

        }
        
    }
}