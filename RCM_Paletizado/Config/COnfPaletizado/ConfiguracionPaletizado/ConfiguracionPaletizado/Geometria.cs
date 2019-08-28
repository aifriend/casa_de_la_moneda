using Idpsa.Control.Tool;
using System;

namespace ConfiguracionPaletizado
{
    internal static class Geometria
    {
        //Las coordenadas X e Y se han 
        //tomado con respecto a la esquina inferior
        //izquierda y la coordenada z respecto a la parte
        //inferior del brazo de la pinza

        private const double LargoPlanoAspirante = 220;
        private const double AnchoPlanoAspirante = 120;

        public static readonly Point2D correcion =
            new Point2D(-AnchoPlanoAspirante / 2, LargoPlanoAspirante / 2);
    }

    [Serializable]
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
                PosSeparatorStore = new Point2D(0,0);
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
        public ConfSaved(string name, double boxX, double boxY, double boxZ, double sepStorX, double sepStorY, double palStor1X, double palStor1Y,
            double palStor2X, double palStor2Y, double zonaPal1X, double zonPal1Y, double zonaPal2X, double zonaPal2Y, double posMesa1X,
            double posMesa1Y, double posMesa2X, double posMesa2Y, double posBandaEntX, double posBandaEntY, double posCogerBandaReprX,
            double posCogerBandaReprY, double separatorSizeX, double separatorSizeY )
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
}