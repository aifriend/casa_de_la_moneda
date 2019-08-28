using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Idpsa.Control.Tool;
using System;
using ConfiguracionPaletizado;

namespace Idpsa.Paletizado
{
    internal static class Geometria
    {
        //Las coordenadas X e Y se han 
        //tomado con respecto a la esquina inferior
        //izquierda y la coordenada z respecto a la parte
        //inferior del brazo de la pinza

        private const double LargoPlanoAspirante = 220;
        private const double AnchoPlanoAspirante = 120;

        public const int ZSuelo_brazo = 1340 + 100; //90;
        public const int ZTecho_brazo = 44;
        public const int Zbrazo_mesasRodillos = 982 + 110;//100;
        public const int Zbrazo_mesaCarton = 769 + 73;
        public static Point3D boxMeasures = new Point3D(192, 135, 225);


        public const int ZTechoPortico = 22;
        //public const int ZSuelo = 1305;
        public const int ZSuelo = 1335; //MDG.2011-03-16.ParaQueAlcance a recoger Palet Vikex

        public const int MaxNumberFlatsPaletizadoEnMesaRodillos = 4;

        public static readonly Point2D correcion =
            new Point2D(-AnchoPlanoAspirante/2, LargoPlanoAspirante/2);

        public static readonly Point3DGripperHelper PosSeparatorStore =
            new Point3DGripperHelper(Corners.DownLeft, Spin.S90, Zbrazo_mesaCarton,
                                     new Point2D(4, 740).Desplazado(correcion));

        public static readonly Point3DGripperHelper PosPaletStore1 =
            new Point3DGripperHelper(Corners.DownLeft, Spin.S90, ZSuelo_brazo,
                                     new Point2D(1638, 570).Desplazado(correcion));

        public static readonly Point3DGripperHelper PosPaletStore2 =
            new Point3DGripperHelper(Corners.DownLeft, Spin.S90 //Spin.S270//Spin.S90
                                    , ZSuelo_brazo,
                                     //new Point2D(2997, 560).Desplazado(correcion));
                                     //new Point2D(2997, 580).Desplazado(correcion));//MDG.2013-05-13
                                     //new Point2D(2997, 540).Desplazado(correcion));//MDG.2013-05-13
                                     new Point2D(2997, 520).Desplazado(correcion));//MDG.2013-05-13

        //public static readonly Point3DGripperHelper PosZonaPaletizadoFinal1 =
        //  new Point3DGripperHelper(Corners.DownLeft ,ZSuelo_brazo,
        //                           new Point2D(4542 - 65, 1703).Desplazado(correcion));

        //MDG.2011-03-16.Desplazamos 100 mm a la derecha para que quepa bien palet Vikex
        public static readonly Point3DGripperHelper PosZonaPaletizadoFinal1 =
            new Point3DGripperHelper(Corners.DownLeft, ZSuelo_brazo,
                                     new Point2D(4542 - 65, 1703 + 100).Desplazado(correcion));

        //public static readonly Point3DGripperHelper PosZonaPaletizadoFinal2 =
        //  new Point3DGripperHelper(Corners.DownLeft ,ZSuelo_brazo,
        //                           new Point2D(4531 - 65, 625).Desplazado(correcion));

        //MDG.2010-11-23.Desplazamos 60 mm a la derecha
        //public static readonly Point3DGripperHelper PosZonaPaletizadoFinal2 =
        //          new Point3DGripperHelper(Corners.DownLeft, ZSuelo_brazo,
        //                                   new Point2D(4531 - 65, 625+60).Desplazado(correcion));
        //MDG.2010-11-23.Desplazamos otros 30 mm a la derecha
        public static readonly Point3DGripperHelper PosZonaPaletizadoFinal2 =
            new Point3DGripperHelper(Corners.DownLeft, ZSuelo_brazo,
                                     //new Point2D(4531 - 65, 625 + 60 + 30).Desplazado(correcion));
                                     //new Point2D(4531 - 65-25, 625 + 60 + 30+4).Desplazado(correcion));//MDG.2013-05-13
                                     //new Point2D(4531 - 65 - 25, 625 + 60 + 30 + 4 +20).Desplazado(correcion));//MDG.2013-05-13
                                     new Point2D(4531 - 65 - 25, 625 + 60 + 30 + 4 -10).Desplazado(correcion));//MDG.2013-05-13

        public static readonly Point3DGripperHelper PosMesa1 =
            new Point3DGripperHelper(Corners.DownRight, Zbrazo_mesasRodillos,
                                     //new Point2D(2855, 1717).Desplazado(correcion));
                                     new Point2D(2875, 1717).Desplazado(correcion));//MDG.2013-05-09. 10 mm más al fondo en X

        public static readonly Point3DGripperHelper PosMesa2 =
            new Point3DGripperHelper(Corners.DownRight, Zbrazo_mesasRodillos,
                                     new Point2D(4222, 1717).Desplazado(correcion));

        public static readonly PointSpin3D PosBandaEntrada =
            //new PointSpin3D(Spin.S0,775, 1370, 140);
            //new PointSpin3D(Spin.S0, 780, 1370, 140);
            //new PointSpin3D(Spin.S0, 815, 1367, 140);//MDG.2013-01-24.AjustePinza y Mesa Etiquetado
            //new PointSpin3D(Spin.S0, 808, 1362, 146);//MDG.2013-01-24.AjustePinza y Mesa Etiquetado
            //new PointSpin3D(Spin.S0, 808, 1362, 151);//MDG.2013-01-28.Bajamos 5 mm porque a veces no llega a coger las cajas
            //new PointSpin3D(Spin.S0, 803, 1362, 151);//MDG.2013-01-28.Hacia atras 5 mm para que no las coja por la zona en la que estan mal por el fallo del precintado
            new PointSpin3D(Spin.S0, 808, 1368, 149);//MDG.2013-05-09.6mm derecha porque movieron guia y 5mm hacia adelante para volver a coger cajas centradas. Ya no falla precintado

        public static readonly PointSpin3D PosCogerBandaReproceso =
            //new PointSpin3D(Spin.S0, 810, 1093, 146);
            //new PointSpin3D(Spin.S0, 810, 1093, 153);//MDG.2013-01-24.Ajuste Pinza
            new PointSpin3D(Spin.S0, 801, 1090, 153);//MDG.2013-01-24.Ajuste Pinza

        public static readonly Point2D SeparatorSize =
            new Point2D(1000, 1000);

        //MDG.2011-06-15
        public static readonly PointSpin3D PosOrigenBandaEntrada =
            //new PointSpin3D(Spin.S0, 775, 1370, Gripper.NegativeZLimit);
            //new PointSpin3D(Spin.S0, 780, 1370, Gripper.NegativeZLimit);//MDG.2011-07-06.Paraque coja las cajas centradas
            //new PointSpin3D(Spin.S0, 815, 1367, Gripper.NegativeZLimit);//MDG.2013-01-24.Recolocacion mesa entrada
            //new PointSpin3D(Spin.S0, 808, 1362, Gripper.NegativeZLimit);//MDG.2013-01-28
            //new PointSpin3D(Spin.S0, 803, 1362, Gripper.NegativeZLimit);//MDG.2013-01-28
            new PointSpin3D(Spin.S0, 808, 1368, Gripper.NegativeZLimit);//MDG.2013-05-09
    }

    public class Point3DGripperHelper : PointSpin3D
    {
        private const int Zbrazo_planoAspiranteRectracted = 83; //36;
        private const int Zbrazo_planoAspiranteExtended = 0; //47;

        public Point3DGripperHelper(Corners corner, double x, double y, double z)
            : this(corner, Spin.S0, x, y, z)
        {
        }

        public Point3DGripperHelper(Corners corner, Spin spin, double x, double y, double z)
            : base(spin, x, y, z)
        {
            Corner = corner;
        }

        public Point3DGripperHelper(Corners corner, double z, Point2D p)
            : this(corner, Spin.S0, z, p)
        {
        }

        public Point3DGripperHelper(Corners corner, Spin spin, double z, Point2D p)
        {
            Corner = corner;
            Spin = spin;
            Z = z;
            X = p.X;
            Y = p.Y;
        }

        public Point3DGripperHelper(double z, PointSpin2D p)
            : base(z, p)
        {
        }

        public Corners Corner { get; private set; }

        public PointSpin3D GetGripperTransformed(Gripper.Extensor extensor)
        {
            double z = 0;
            switch (extensor)
            {
                case Gripper.Extensor.Extend:
                    z = Z + Zbrazo_planoAspiranteExtended;
                    break;
                case Gripper.Extensor.Retract:
                    z = Z - Zbrazo_planoAspiranteRectracted;
                    break;
            }
            return new PointSpin3D(Spin, X, Y, z);
        }

        public CornerPoint3D ToConerPoint3D()
        {
            return new CornerPoint3D(Corner, this);
        }

        public static PointSpin3D TransFormToGripperMorfology(Gripper.Extensor extensor, PointSpin3D point)
        {
            double zValue = point.Z;
            switch (extensor)
            {
                case Gripper.Extensor.Extend:
                    zValue = zValue - Zbrazo_planoAspiranteExtended;
                    break;
                case Gripper.Extensor.Retract:
                    zValue = zValue + Zbrazo_planoAspiranteRectracted;
                    break;
                case Gripper.Extensor.Dead:
                    zValue = zValue - Zbrazo_planoAspiranteExtended/2;
                    break;
            }
            return new PointSpin3D(point.Spin, point.X, point.Y, zValue);
        }
    }

    public class ConfSaved
    {
        public string Name;
        public Point3D boxMeasures; //= new Point3D(192, 135, 225);
        public Point2D PosSeparatorStore; //= new Point2D(4, 740).Desplazado(Geometria.correcion);
        public Point2D PosPaletStore1; // = new Point2D(1638, 570).Desplazado(Geometria.correcion);
        public Point2D PosPaletStore2; //= new Point2D(2997, 520).Desplazado(Geometria.correcion);
        public Point2D PosZonaPaletizadoFinal1; // = new Point2D(4542 - 65, 1703 + 100).Desplazado(Geometria.correcion);
        public Point2D PosZonaPaletizadoFinal2; //= new Point2D(4531 - 65 - 25, 625 + 60 + 30 + 4 - 10).Desplazado(Geometria.correcion);
        public Point2D PosMesa1; // = new Point2D(2875, 1717).Desplazado(Geometria.correcion);
        public Point2D PosMesa2; // = new Point2D(4222, 1717).Desplazado(Geometria.correcion);
        public Point2D PosBandaEntrada; // =new Point2D(808, 1368);
        public Point2D PosCogerBandaReproceso; // = new Point2D (801, 1090);
        public Point2D SeparatorSize; // =  new Point2D(1000, 1000);

        public ConfSaved()
        {
            Name = "";
            boxMeasures = new Point3D();
            PosSeparatorStore = new Point2D(0, 0);
            PosPaletStore1 = new Point2D(0, 0);
            PosPaletStore2 = new Point2D(0, 0);
            PosZonaPaletizadoFinal1 = new Point2D(0, 0);
            PosZonaPaletizadoFinal2 = new Point2D(0, 0);
            PosMesa1 = new Point2D(0, 0);
            PosMesa2 = new Point2D(0, 0);
            PosBandaEntrada = new Point2D(0, 0);
            PosCogerBandaReproceso = new Point2D(0, 0);
            SeparatorSize = new Point2D(0, 0);

        }

        public ConfSaved(ConfiguracionPaletizado.safeConf s)
        {
            Name = s.name;
            boxMeasures = new Point3D(s.boxX, s.boxY, s.boxZ);
            PosSeparatorStore = new Point2D(s.sepStorX, s.sepStorY);
            PosPaletStore1 = new Point2D(s.palStor1X, s.palStor1Y);
            PosPaletStore2 = new Point2D(s.palStor2X, s.palStor2Y);
            PosZonaPaletizadoFinal1 = new Point2D(s.zonaPal1X, s.zonPal1Y);
            PosZonaPaletizadoFinal2 = new Point2D(s.zonaPal2X, s.zonaPal2Y);
            PosMesa1 = new Point2D(s.posMesa1X, s.posMesa1Y);
            PosMesa2 = new Point2D(s.posMesa2X, s.posMesa2Y);
            PosBandaEntrada = new Point2D(s.posBandaEntX, s.posBandaEntY);
            PosCogerBandaReproceso = new Point2D(s.posCogerBandaReprX, s.posCogerBandaReprY);
            SeparatorSize = new Point2D(s.separatorSizeX, s.separatorSizeY);
        }

        public ConfSaved(string name, double boxX, double boxY, double boxZ, double sepStorX, double sepStorY, double palStor1X, double palStor1Y,
            double palStor2X, double palStor2Y, double zonaPal1X, double zonPal1Y, double zonaPal2X, double zonaPal2Y, double posMesa1X,
            double posMesa1Y, double posMesa2X, double posMesa2Y, double posBandaEntX, double posBandaEntY, double posCogerBandaReprX,
            double posCogerBandaReprY, double separatorSizeX, double separatorSizeY)
        {
            Name = name;
            boxMeasures = new Point3D(boxX, boxY, boxZ);
            PosSeparatorStore = new Point2D(sepStorX, sepStorY);
            PosPaletStore1 = new Point2D(palStor1X, palStor1Y);
            PosPaletStore2 = new Point2D(palStor2X, palStor2Y);
            PosZonaPaletizadoFinal1 = new Point2D(zonaPal1X, zonPal1Y);
            PosZonaPaletizadoFinal2 = new Point2D(zonaPal2X, zonaPal2Y);
            PosMesa1 = new Point2D(posMesa1X, posMesa1Y);
            PosMesa2 = new Point2D(posMesa2X, posMesa2Y);
            PosBandaEntrada = new Point2D(posBandaEntX, posBandaEntY);
            PosCogerBandaReproceso = new Point2D(posCogerBandaReprX, posCogerBandaReprY);
            SeparatorSize = new Point2D(separatorSizeX, separatorSizeY);
        }


        public ConfSaved(string d)
        {
            if (d == "default")
            {
                Name = "Default";
                boxMeasures = new Point3D(192, 135, 225);
                PosSeparatorStore = new Point2D(4, 740).Desplazado(Geometria.correcion);
                PosPaletStore1 = new Point2D(1638, 570).Desplazado(Geometria.correcion);
                PosPaletStore2 = new Point2D(2997, 520).Desplazado(Geometria.correcion);
                PosZonaPaletizadoFinal1 = new Point2D(4542 - 65, 1703 + 100).Desplazado(Geometria.correcion);
                PosZonaPaletizadoFinal2 = new Point2D(4531 - 65 - 25, 625 + 60 + 30 + 4 - 10).Desplazado(Geometria.correcion);
                PosMesa1 = new Point2D(2875, 1717).Desplazado(Geometria.correcion);
                PosMesa2 = new Point2D(4222, 1717).Desplazado(Geometria.correcion);
                PosBandaEntrada = new Point2D(808, 1368);
                PosCogerBandaReproceso = new Point2D(801, 1090);
                SeparatorSize = new Point2D(1000, 1000);
            }
        }
    }

    public class GeometriaConf
    {
        public ConfSaved conf;
        private const int ZSuelo_brazo = Geometria.ZSuelo_brazo;
        private const int ZTecho_brazo = Geometria.ZTecho_brazo;
        private const int Zbrazo_mesasRodillos = Geometria.Zbrazo_mesasRodillos;
        private const int Zbrazo_mesaCarton = Geometria.Zbrazo_mesaCarton; 
        public const int ZTechoPortico = 22;
        public const int ZSuelo = 1335; //MDG.2011-03-16.ParaQueAlcance a recoger Palet Vikex

        public const int MaxNumberFlatsPaletizadoEnMesaRodillos = 4;
        public Point3DGripperHelper PosSeparatorStore;
        public Point3DGripperHelper PosPaletStore1;
        public Point3DGripperHelper PosPaletStore2;
        public Point3DGripperHelper PosZonaPaletizadoFinal1;
        public Point3DGripperHelper PosZonaPaletizadoFinal2;
        public Point3DGripperHelper PosMesa1;
        public Point3DGripperHelper PosMesa2;
        public PointSpin3D PosBandaEntrada;
        public PointSpin3D PosCogerBandaReproceso;
        public Point2D SeparatorSize;
        public PointSpin3D PosOrigenBandaEntrada;

        public GeometriaConf(string fail)
        {
            PosSeparatorStore = Geometria.PosSeparatorStore;
            PosPaletStore1 = Geometria.PosPaletStore1;
            PosPaletStore2 = Geometria.PosPaletStore2;
            PosZonaPaletizadoFinal1 = Geometria.PosZonaPaletizadoFinal1;
            PosZonaPaletizadoFinal2 = Geometria.PosZonaPaletizadoFinal2;
            PosMesa1 = Geometria.PosMesa1;
            PosMesa2 = Geometria.PosMesa2;
            PosBandaEntrada = Geometria.PosBandaEntrada;
            PosCogerBandaReproceso = Geometria.PosCogerBandaReproceso;
            SeparatorSize = Geometria.SeparatorSize;
            PosOrigenBandaEntrada = Geometria.PosOrigenBandaEntrada;
        }

        public GeometriaConf()
        {
            conf = LoadDatosConf();
            PosSeparatorStore = new Point3DGripperHelper(Corners.DownLeft, Spin.S90, Zbrazo_mesaCarton,
                                                         conf.PosSeparatorStore);
            PosPaletStore1 = new Point3DGripperHelper(Corners.DownLeft, Spin.S90, ZSuelo_brazo,
                                         conf.PosPaletStore1);

            PosPaletStore2 = new Point3DGripperHelper(Corners.DownLeft, Spin.S90, ZSuelo_brazo,
                                         conf.PosPaletStore2);
            PosZonaPaletizadoFinal1 = new Point3DGripperHelper(Corners.DownLeft, ZSuelo_brazo,
                                         conf.PosZonaPaletizadoFinal1);
            PosZonaPaletizadoFinal2 = new Point3DGripperHelper(Corners.DownLeft, ZSuelo_brazo,
                                         conf.PosZonaPaletizadoFinal2);
            PosMesa1 = new Point3DGripperHelper(Corners.DownRight, Zbrazo_mesasRodillos,
                                         conf.PosMesa1);
            PosMesa2 = new Point3DGripperHelper(Corners.DownRight, Zbrazo_mesasRodillos,
                                         conf.PosMesa2);
            PosBandaEntrada = new PointSpin3D(Spin.S0, conf.PosBandaEntrada.X, conf.PosBandaEntrada.Y, 149 + Geometria.boxMeasures.Z-conf.boxMeasures.Z);
            PosCogerBandaReproceso = new PointSpin3D(Spin.S0, conf.PosCogerBandaReproceso.X, conf.PosCogerBandaReproceso.Y, 153+Geometria.boxMeasures.Z-conf.boxMeasures.Z);
            SeparatorSize = conf.SeparatorSize;
            PosOrigenBandaEntrada = new PointSpin3D(Spin.S0, conf.PosBandaEntrada.X, conf.PosBandaEntrada.Y, Gripper.NegativeZLimit);
        }


        public ConfSaved LoadDatosConf()
        {
            string filePath = ConfigPaletizadoFiles.ConfActual;
            try
            {
                using (var readFile = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var bFormatter = new BinaryFormatter();
                    var aux = (safeConf) bFormatter.Deserialize(readFile);
                    ConfSaved catalog = new ConfSaved(aux);
                    return catalog;
                }
            }
            catch(Exception exception)
            {
                MessageBox.Show("No se ha podido mostrar la configuración pedida");
                return (new ConfSaved("default"));
            }
        }
    }
}