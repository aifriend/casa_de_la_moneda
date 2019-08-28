using Idpsa.Control;
using Idpsa.Control.Component;
using IdpsaControl;

namespace Idpsa.Paletizado
{
    public partial class IdpsaSystemPaletizado
    {
        protected override void InitializeSubSystems()
        {
            Geo = new GeometriaConf(); //MCR

            BotoneraBarrera = ContructBotoneraBarrera();
            Production = new SystemProductionPaletizado(this);
            Lines = new Lines(this);
            Gripper = ConstructGripper();
            Db = new DataBase();
            GripperContext.GetInstance().SetLines(Lines);
            ManualFeedRuhlamat = CreateEntradaManualRulamat(Lines);
            Rulhamat = new EnlaceRulamat(ConfigPaletizadoComunication.EnlaceRulamatIpEndPoint, this);
            barreraRearme = CreateBarreraRearme();
        }

        private BotoneraBarrera ContructBotoneraBarrera()
        {
            Sensor buttonStopRequest = new SensorP(Bus.In("Input3.6"), "Petición Stop");
            Sensor buttonResumeRequest = new SensorP(Bus.In("Input3.5"), "Continuar automático");

            return new BotoneraBarrera(buttonStopRequest, buttonResumeRequest, this);
        }

        private RuhlamatManualFeed CreateEntradaManualRulamat(Lines lines)
        {
            IItemSolicitor<GrupoPasaportes> solicitor = lines.BandaSalidaEnfajadora.SolicitorAlemana;
            LinesSemaphore semaphore = Lines.Semaphore;
            SourceGroupSupplier supplier = lines.SupplierLine2;

            //Weighter and RFID passport checker
            var bascula = new BasculaHbm("Bascula Entrada Manual",
                                         ConfigPaletizadoComunication.BasculaEntradaManualRulamatPort);
            //var rfid = new RfidReaderDecipheIt("Rfid Entrada Manual",
            //                                   ConfigPaletizadoComunication.RfidEntradaManualRulamatPort);
            //MDG.2013-02-04.Busca el lecor en dos puertos
            var rfid = new RfidReaderDecipheIt("Rfid Entrada Manual",
                                               ConfigPaletizadoComunication.RfidEntradaManualRulamatPort,
                                               ConfigPaletizadoComunication.RfidEntradaManualRulamatPort_alternativo,
                                               ConfigPaletizadoComunication.RfidEntradaManualRulamatPort_alternativo2
                                               );
            

            
            //Manual entry components
            Sensor cajonCerrado = new SensorP(Bus.In("Input54.1"), "Cajon cerrado");
            Sensor entradaManualOcupada = new SensorP(Bus.In("Input54.4"), "Sensor entrada manual PRODEC");
            ICylinder empujador = new Cylinder1Efect
                (
                Bus.In("Input54.2"),
                Bus.In("Input54.3"),
                Bus.Out("Output54.2"),
                "Empujador Entrada Manual"
                )
                .WithRestName("Retener")
                .WithWorkName("Introducir");

            return new RuhlamatManualFeed(empujador, entradaManualOcupada, cajonCerrado, bascula, rfid, solicitor,
                                          semaphore, supplier);
        }

        private Gripper ConstructGripper()
        {
            CompaxC3I20T11 EjeX = new CompaxC3I20T11(new Address(6, 0), new Address(5, 0), Bus, "Eje X")
                .WithJojNegEnable(() => true)
                .WithJojPosName("Adelante")
                .WithJojNegName("Atras")
                .WithInvertedJogSense(false);

            CompaxC3I20T11 EjeY = new CompaxC3I20T11(new Address(28, 0), new Address(27, 0), Bus, "Eje Y")
                .WithJojEnable(() => true)
                .WithJojPosName("Derecha")
                .WithJojNegName("Izquierda")
                .WithInvertedJogSense(false);

            //var EjeZ = new CompaxC3I20T11(new Address(56, 0), new Address(56, 0), Bus, "Eje Z")
            //CompaxC3I20T11 EjeZ = new CompaxC3I20T11(new Address(57, 0), new Address(56, 0), Bus, "Eje Z")
                //MDG.2012-04-12.Desplazamiento por nueva tarjeta de entradas digitales en la direccion 56
            CompaxC3I20T11 EjeZ = new CompaxC3I20T11(new Address(57, 0), new Address(57, 0), Bus, "Eje Z")
                //MDG.2012-07-19.Desplazamiento por nueva tarjeta de salidas digitales en la direccion 56
                .WithJojEnable(() => true)
                .WithJojPosName("Abajo")
                .WithJojNegName("Arriba")
                .WithInvertedJogSense(false);

            var gantry = new Gantry3_C3I20T11_ChainController(EjeX, EjeY, EjeZ);

            var actuadorGiro = new GyreActuatorSommerSFM(
                Bus.In("B504B"),
                Bus.In("B504A"),
                Bus.In("B504C"),
                new ActuatorWithInversorSimple(Bus.Out("Y504B"), Bus.Out("Y504A")),
                new ActuatorWithInversorSimple(Bus.Out("Y505B"), Bus.Out("Y505A")), "Giro pinza");

            var aspirator = new Sucker(new SensorSimulated(), Bus.Out("Y401"));
            var extensor = new Cylinder2Efect1Sensor(Bus.In("B311"), Bus.Out("Y311B"), Bus.Out("Y311A"));
            var arms = new Cylinders2EfectBypassed(Bus.In("B403B"), Bus.In("B403A"), Bus.In("B310B"), Bus.In("B310A"),
                                                   Bus.Out("Y310B"), Bus.Out("Y310A"));
            var tilts = new ScaleHBM("Bascula", ConfigPaletizadoComunication.TiltPort);

            return new Gripper(gantry, actuadorGiro, aspirator, extensor, arms, tilts, this);
        }

        private AuthorizedReset CreateBarreraRearme()
        {
            RfidReaderEmployee rfid = new RfidReaderEmployee("RfID_Rearme", ConfigPaletizadoComunication.RfidRearme, ConfigPaletizadoComunication.RfidRearme_alt);
            SensorP boton = new SensorP(Bus.In("Input3.4"));
            Output luz = Bus.Out("Output0.7");
            Output sirena = Bus.Out("H990_5");
            Input SysOKPos = Bus.In("K922B");
            Input SysOKNeg = Bus.In("K921D");

            return new AuthorizedReset(rfid, boton, luz,SysOKPos,SysOKNeg, sirena);
        }
    }
}