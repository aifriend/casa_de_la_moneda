using System;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Engine;
using Idpsa.Control.Manuals;
using Idpsa.Control.Subsystem;
using Idpsa.Paletizado.Definitions;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    public class Lines : IOriginDefiner
    {
        private readonly Mediator _mediator;
        private readonly IdpsaSystemPaletizado _sys;

        public Lines(IdpsaSystemPaletizado sys)
        {
            _sys = sys;

            ManualReprocesor = new ReprocesadorManual(sys.Bus);

            BandaEtiquetado = ConstructBandaEtiquetado(true, sys.Bus, ManualReprocesor);
            BandaEtiquetado.Etiquetadora = ContructEtiquetadora(sys.Bus, BandaEtiquetado);

            Encajadora = ConstructEncajadora(true, sys.Bus, BandaEtiquetado);

            Semaphore = new LinesSemaphore();

            BandaSalidaEnfajadora = ConstructBandaSalidaEnfajadora(true, Semaphore, Encajadora);

            supplierLine1 = SourceGroupSupplier.Create(IDLine.Japonesa);

            Line1 = new Line1(sys, supplierLine1, BandaSalidaEnfajadora.SolicitorJaponesa, Semaphore);

            SupplierLine2 = SourceGroupSupplier.Create(IDLine.Alemana);

            Line2 = new Line2(sys, SupplierLine2, BandaSalidaEnfajadora.SolicitorAlemana, Semaphore);

            Beacon = ConstructBeacon();

            SeparatorStore = new SeparatorStore("almacén de cartones", _sys.Geo.PosSeparatorStore.ToConerPoint3D(),
                                                new SensorP(sys.Bus.In("B822"), "Presencia cartones"));

            IEnumerable<ISolicitor> commonSolicitors = new List<ISolicitor>();
            IEnumerable<ISupplier> commonSuppliers = new List<ISupplier>
                                                         {BandaEtiquetado, ManualReprocesor, SeparatorStore};
            _mediator = new Mediator(commonSolicitors, commonSuppliers);
        }

        public LinesSemaphore Semaphore { get; private set; }
        public SourceGroupSupplier supplierLine1 { get; private set; }
        public SourceGroupSupplier SupplierLine2 { get; private set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        public BandaSalidaEnfajadora BandaSalidaEnfajadora { get; set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        public Prodec Encajadora { get; set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        [Manual(SuperGroup = "Estado", Group = "Socitors/Supliers")]
        public BandaEtiquetado BandaEtiquetado { get; set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        [Manual(SuperGroup = "Estado", Group = "Socitors/Supliers")]
        public ReprocesadorManual ManualReprocesor { get; set; }

        [Subsystem(Filter = SubsystemFilter.All)]
        [Manual(SuperGroup = "Estado", Group = "Socitors/Supliers")]
        public SeparatorStore SeparatorStore { get; set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        public Beacon Beacon { get; set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        public Line1 Line1 { get; set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        public Line2 Line2 { get; set; }

        public IEnumerable<ISolicitor> GetSolicitors()
        {
            return _mediator.GetSolicitors();
        }

        public IEnumerable<ISupplier> GetSuppliers()
        {
            return _mediator.GetSuppliers();
        }

        public IEnumerable<ISupplier> GetSuppliers(IEnumerable<Locations> locations)
        {
            if (locations == null)
                return new ISupplier[] {};

            return _mediator.GetSuppliers()
                .Where((s) => locations.Any((location) => location == s.Location))
                .DefaultIfEmpty();
        }

        public ISupplier GetSupplier(Locations location)
        {
            return _mediator.GetSuppliers().FirstOrDefault(s => s.Location == location);
        }

        public PaletStore GetPaletSotore(Locations location)
        {
            if (location == Locations.PaletJaponesa)
                return Line1.PaletStore;
            return location == Locations.PaletAlemana ? Line2.PaletStore : null;
        }

        public IPaletizer GetPaletizer(Locations location)
        {
            IPaletizer paletizer = null;
            switch (location)
            {
                case Locations.Paletizado1Japonesa:
                    paletizer = Line1.Mesas.Mesa1.Paletizer;
                    break;
                case Locations.Paletizado2Japonesa:
                    paletizer = Line1.Mesas.Mesa2.Paletizer;
                    break;
                case Locations.Paletizado3Japonesa:
                    paletizer = Line1.ZonaPaletizadoFinal.Paletizer;
                    break;
                case Locations.PaletizadoAlemana:
                    paletizer = (Line2).Paletizer;
                    break;
                default:
                    throw new Exception("Fail in GetPaletizer method of class Lines, parameter location: " + location +
                                        ",line1");
            }

            return paletizer;
        }

        public SeparatorStore GetSeparatorStore()
        {
            return SeparatorStore;
        }

        public void DeactivateLines()
        {
            Line1.Deactivate();
            Line2.Deactivate();
        }

        public void ActivateLines()
        {
            Line1.Activate();
            Line2.Activate();
        }

        public void SetNewCatalog(DatosCatalogoPaletizado datosCatalogo)
        {
            BandaEtiquetado.SetNewCatalog();
            SeparatorStore.SetNewCatalog(datosCatalogo);

            Line line = GetLine(datosCatalogo.IDLine);

            line.SetNewCatalog(datosCatalogo);
            _mediator.AddSuppliersAndSolicitors(line);

            if (line.State != SubsystemState.Activated)
            {
                line.Activate();
            }
        }

        public Line GetLine(IDLine idLine)
        {
            return (idLine == IDLine.Japonesa) ? Line1 : (Line) Line2;
        }

        private Line GetComplementLine(IDLine idLine)
        {
            return (idLine == IDLine.Japonesa) ? Line2 : (Line) Line1;
        }

        private Line GetComplementLine(Line line)
        {
            return GetComplementLine(line.Id);
        }

        private BandaSalidaEnfajadora ConstructBandaSalidaEnfajadora(bool construct, LinesSemaphore semaphore,
                                                                     IItemSolicitor<GrupoPasaportes> solicitor)
        {
            if (!construct) return null;

            Bus bus = _sys.Bus;
            Actuator motor = new ActuatorP(bus.Out("K732"), "Motor cinta");
            Input sEntradaJaponesa = bus.In("Input52.3");
            Input sEntradaAlemana = bus.In("Input52.3");
            Input sSalida = bus.In("Input52.2");
            Sensor sEntradaAlemanaManual = new SensorP(bus.In("Input54.4"), "Sensor entrada manual PRODEC");
            Sensor sEmpujadorAlemanaManual = new SensorP(bus.In("Input54.2"), "Sensor empujador manual PRODEC");

            return new BandaSalidaEnfajadora(motor, sEntradaJaponesa,
                                             sEntradaAlemana, sEntradaAlemanaManual, sEmpujadorAlemanaManual,
                                             sSalida, semaphore, _sys.Control, solicitor, _sys.Production);
        }

        private Prodec ConstructEncajadora(bool construct, Bus bus,
                                           IItemSolicitor<CajaPasaportes> solicitor)
        {
            if (!construct) return null;

            return new Prodec(bus.In("ProdecEntrada"), bus.In("ProdecEmergencia"), bus.In("ProdecMarcha"),
                              bus.In("ProdecFallo"), bus.In("ProdecRechazar"), bus.Out("ProdecMensaje"),
                              bus.Out("ProdecSalidaCajas"), solicitor, _sys.Control);
        }

        private BandaEtiquetado ConstructBandaEtiquetado(bool construct, Bus bus, ReprocesadorManual manualReprocesor)
        {
            if (!construct) return null;

            Actuator motor = new ActuatorP(bus.Out("K750"), "Motor cinta");
            Actuator topeEntrada =
                new ActuatorN(bus.Out("Output55.6"), "Tope caja etiquetado").WithRestName("Recoger").WithWorkName(
                    "Extender");
            Input sEntrada = bus.In("B850");
            Input sEtiquetado = bus.In("B851");
            Input sSalida = bus.In("Input52.1");

            var barcodeReader = new BarCodeReaderWenglor("Lector código barras",
                                                         ConfigPaletizadoComunication.BarcodePort, (s) => true);

            ICylinder cilindroEntrada = new Cylinder1EfectTimed(2000, 2000, bus.Out("Output55.4"), "Empujador");

            return new BandaEtiquetado(_sys.Geo.PosBandaEntrada, motor, cilindroEntrada, topeEntrada,
                                       sEntrada, sEtiquetado, sSalida, barcodeReader, manualReprocesor, _sys);
        }

        private Etiquetadora ContructEtiquetadora(Bus bus, BandaEtiquetado banda)
        {
            Zebra_CajasPrinter zebraPrinter = new Zebra_CajasPrinter_10_100_PrintServer("Etiquetadora automática",
                                                                                        "ZebraAutomatico");
            var ventosa = new Sucker(new SensorSimulated("B300A"), bus.Out("V300"), "Aspirador etiquetas");

            ICylinder retirador = new Cylinder1Efect(bus.In("B330B"), bus.In("B330A"),
                                                     new ActuatorN(bus.Out("Y330")), "Retirador etiquetadora");

            ICylinder extensor = new Cylinder1Efect(bus.In("B230B"), bus.In("B230A"),
                                                    bus.Out("Y230"), "Extensor etiquetadora")
                .WithManualEnable(() => retirador.InWork)
                .WithRestName("Recoger")
                .WithWorkName("Extender");

            ICylinder girador = new Cylinder1EfectTimed(2500, 2500, bus.Out("Y510"), "Girador etiquetadora")
                .WithManualEnable(() => retirador.InWork && extensor.InRest)
                .WithRestName("0º")
                .WithWorkName("90º");

            return new Etiquetadora(zebraPrinter, ventosa, extensor, retirador, girador, banda);
        }


        private Beacon ConstructBeacon()
        {
            Bus bus = _sys.Bus;
            SystemControl control = _sys.Control;
            IEvaluable orangeDelayed = BandaEtiquetado.SCogidaCaja.DelayToDisconnection(4*60*1000);
            //IEvaluable soundDelayed = BandaEtiquetado.SCogidaCaja.DelayToDisconnection(4 * 60 * 1000);

            return new Beacon()
            .WihtLightOnWhen(bus.Out("H990_1"), () => control.InActiveMode2(Mode.Automatic)) //blue
            .WihtLightOnWhen(bus.Out("H990_2"), () => control.InActiveMode(Mode.Automatic)) //green
            //.WithBlinkWhen(bus.Out("H990_3"), () => !orangeDelayed.Value() && control.InActiveMode(Mode.Automatic), 700) //orange 
            .WithBlinkWhen(bus.Out("H990_3"), () => (control.OperationMode == Mode.Automatic) && !control.InActiveMode(Mode.Automatic) && control.SystemOK && control.BootSystem && control.StartingAuto, 500) //orange
            .WihtLightOnWhen(bus.Out("H990_4"), () => control.Diagnosis) //red
            .WithBlinkWhen(bus.Out("H990_5"), () => (control.OperationMode == Mode.Automatic) && !control.InActiveMode(Mode.Automatic) && control.SystemOK && control.BootSystem && control.StartingAuto, 500); //Baliza sonora 
            
                //.WithBlinkWhen(bus.Out("Output1.4"),
                //               () => !orangeDelayed.Value() && control.InActiveMode(Mode.Automatic), 700) //orange 
                ////.WihtLightOnWhen(bus.Out("Output1.0"), () => control.InActiveMode(Mode.BackToOrigin)) //blue
                //.WihtLightOnWhen(bus.Out("Output1.0"), () => control.InActiveMode2(Mode.Automatic)) //blue//MDG.2012-07-25
                //.WihtLightOnWhen(bus.Out("Output1.2"), () => control.InActiveMode(Mode.Automatic)) //green
                //.WihtLightOnWhen(bus.Out("Output1.6"), () => control.Diagnosis); //red
        }

        #region Miembros de IOriginDefiner

        bool IOriginDefiner.InOrigin()
        {
            return (Line1.State == SubsystemState.Activated) || (Line2.State == SubsystemState.Activated);
        }

        #endregion
    }
}