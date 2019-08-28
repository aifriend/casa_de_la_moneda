using Idpsa.Control.Tool;
using System;

namespace ConfiguracionPaletizado
{
    internal static class Geometry
    {
        //Las coordenadas X e Y se han 
        //tomado con respecto a la esquina inferior
        //izquierda y la coordenada z respecto a la parte
        //inferior del brazo de la pinza

        private const double LargoPlanoAspirante = 220;
        private const double AnchoPlanoAspirante = 120;

        public static readonly Point2DConf correcion =
            new Point2DConf(-AnchoPlanoAspirante / 2, LargoPlanoAspirante / 2);
    }

    public class ConfSavedConf
    {
        public string Name;
        public Point3DConf boxMeasures; //= new Point3D(192, 135, 225);
        public Point2DConf PosSeparatorStore; //= new Point2D(4, 740).Desplazado(Geometria.correcion);
        public Point2DConf PosPaletStore1; // = new Point2D(1638, 570).Desplazado(Geometria.correcion);
        public Point2DConf PosPaletStore2; //= new Point2D(2997, 520).Desplazado(Geometria.correcion);
        public Point2DConf PosZonaPaletizadoFinal1; // = new Point2D(4542 - 65, 1703 + 100).Desplazado(Geometria.correcion);
        public Point2DConf PosZonaPaletizadoFinal2; //= new Point2D(4531 - 65 - 25, 625 + 60 + 30 + 4 - 10).Desplazado(Geometria.correcion);
        public Point2DConf PosMesa1; // = new Point2D(2875, 1717).Desplazado(Geometria.correcion);
        public Point2DConf PosMesa2; // = new Point2D(4222, 1717).Desplazado(Geometria.correcion);
        public Point2DConf PosBandaEntrada; // =new Point2D(808, 1368);
        public Point2DConf PosCogerBandaReproceso; // = new Point2D (801, 1090);
        public Point2DConf SeparatorSize; // =  new Point2D(1000, 1000);

        public ConfSavedConf()
        {
                Name = "";
                boxMeasures = new Point3DConf();
                PosSeparatorStore = new Point2DConf(0,0);
                PosPaletStore1 = new Point2DConf(0, 0);
                PosPaletStore2 = new Point2DConf(0, 0);
                PosZonaPaletizadoFinal1 = new Point2DConf(0, 0);
                PosZonaPaletizadoFinal2 = new Point2DConf(0, 0);
                PosMesa1 = new Point2DConf(0, 0);
                PosMesa2 = new Point2DConf(0, 0);
                PosBandaEntrada = new Point2DConf(0, 0);
                PosCogerBandaReproceso = new Point2DConf(0, 0);
                SeparatorSize = new Point2DConf(0, 0);
            
        }
        public ConfSavedConf(string name, double boxX, double boxY, double boxZ, double sepStorX, double sepStorY, double palStor1X, double palStor1Y,
            double palStor2X, double palStor2Y, double zonaPal1X, double zonPal1Y, double zonaPal2X, double zonaPal2Y, double posMesa1X,
            double posMesa1Y, double posMesa2X, double posMesa2Y, double posBandaEntX, double posBandaEntY, double posCogerBandaReprX,
            double posCogerBandaReprY, double separatorSizeX, double separatorSizeY )
        {
            Name = name;
            boxMeasures = new Point3DConf(boxX, boxY, boxZ);
            PosSeparatorStore = new Point2DConf(sepStorX, sepStorY);
            PosPaletStore1 = new Point2DConf(palStor1X, palStor1Y);
            PosPaletStore2 = new Point2DConf(palStor2X, palStor2Y);
            PosZonaPaletizadoFinal1 = new Point2DConf(zonaPal1X, zonPal1Y);
            PosZonaPaletizadoFinal2 = new Point2DConf(zonaPal2X, zonaPal2Y);
            PosMesa1 = new Point2DConf(posMesa1X, posMesa1Y);
            PosMesa2 = new Point2DConf(posMesa2X, posMesa2Y);
            PosBandaEntrada = new Point2DConf(posBandaEntX, posBandaEntY);
            PosCogerBandaReproceso = new Point2DConf(posCogerBandaReprX, posCogerBandaReprY);
            SeparatorSize = new Point2DConf(separatorSizeX, separatorSizeY);
        }
        public ConfSavedConf(safeConf s)
        {
            Name = s.name;
            boxMeasures = new Point3DConf(s.boxX, s.boxY, s.boxZ);
            PosSeparatorStore = new Point2DConf(s.sepStorX, s.sepStorY);
            PosPaletStore1 = new Point2DConf(s.palStor1X, s.palStor1Y);
            PosPaletStore2 = new Point2DConf(s.palStor2X, s.palStor2Y);
            PosZonaPaletizadoFinal1 = new Point2DConf(s.zonaPal1X, s.zonPal1Y);
            PosZonaPaletizadoFinal2 = new Point2DConf(s.zonaPal2X, s.zonaPal2Y);
            PosMesa1 = new Point2DConf(s.posMesa1X, s.posMesa1Y);
            PosMesa2 = new Point2DConf(s.posMesa2X, s.posMesa2Y);
            PosBandaEntrada = new Point2DConf(s.posBandaEntX, s.posBandaEntY);
            PosCogerBandaReproceso = new Point2DConf(s.posCogerBandaReprX, s.posCogerBandaReprY);
            SeparatorSize = new Point2DConf(s.separatorSizeX, s.separatorSizeY);
        }

        public ConfSavedConf(string d)
        {
            if (d == "default")
            {
                Name = "Default";
                boxMeasures = new Point3DConf(192, 135, 195);
                PosSeparatorStore = new Point2DConf(4, 740).Desplazado(Geometry.correcion);
                PosPaletStore1 = new Point2DConf(1638, 570).Desplazado(Geometry.correcion);
                PosPaletStore2 = new Point2DConf(2997, 520).Desplazado(Geometry.correcion);
                PosZonaPaletizadoFinal1 = new Point2DConf(4600-65, 1703 + 115).Desplazado(Geometry.correcion);
                PosZonaPaletizadoFinal2 = new Point2DConf(4531 - 65 - 25, 625 + 60 + 30 + 4 - 10).Desplazado(Geometry.correcion);
                PosMesa1 = new Point2DConf(2875, 1717).Desplazado(Geometry.correcion);
                PosMesa2 = new Point2DConf(4222, 1717).Desplazado(Geometry.correcion);
                PosBandaEntrada = new Point2DConf(808, 1368);
                PosCogerBandaReproceso = new Point2DConf(801, 1090);
                SeparatorSize = new Point2DConf(1000, 1000);
            }
        }
    }
}