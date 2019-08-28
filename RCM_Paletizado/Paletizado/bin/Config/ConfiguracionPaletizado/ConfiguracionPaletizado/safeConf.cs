using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfiguracionPaletizado
{
    [Serializable]
    public class safeConf
    {
        public string name;
        public double boxX, boxY, boxZ;
        public double sepStorX;
        public double sepStorY;
        public double palStor1X;
        public double palStor1Y;
        public double palStor2X;
        public double palStor2Y;
        public double zonaPal1X;
        public double zonPal1Y;
        public double zonaPal2X;
        public double zonaPal2Y;
        public double posMesa1X;
        public double posMesa1Y;
        public double posMesa2X;
        public double posMesa2Y;
        public double posBandaEntX;
        public double posBandaEntY;
        public double posCogerBandaReprX;
        public double posCogerBandaReprY;
        public double separatorSizeX;
        public double separatorSizeY;

        public safeConf(ConfSavedConf conf)
        {
            name = conf.Name;
            boxX = conf.boxMeasures.X; boxY = conf.boxMeasures.Y; boxZ = conf.boxMeasures.Z;
            sepStorX = conf.PosSeparatorStore.X;
            sepStorY = conf.PosSeparatorStore.Y;
            palStor1X = conf.PosPaletStore1.X;
            palStor1Y = conf.PosPaletStore1.Y;
            palStor2X = conf.PosPaletStore2.X;
            palStor2Y = conf.PosPaletStore2.Y;
            zonaPal1X = conf.PosZonaPaletizadoFinal1.X;
            zonPal1Y = conf.PosZonaPaletizadoFinal1.Y;
            zonaPal2X = conf.PosZonaPaletizadoFinal2.X;
            zonaPal2Y = conf.PosZonaPaletizadoFinal2.Y;
            posMesa1X = conf.PosMesa1.X;
            posMesa1Y = conf.PosMesa1.Y;
            posMesa2X = conf.PosMesa2.X;
            posMesa2Y = conf.PosMesa2.Y;
            posBandaEntX = conf.PosBandaEntrada.X;
            posBandaEntY = conf.PosBandaEntrada.Y;
            posCogerBandaReprX = conf.PosCogerBandaReproceso.X;
            posCogerBandaReprY = conf.PosCogerBandaReproceso.Y;
            separatorSizeX = conf.SeparatorSize.X;
            separatorSizeY = conf.SeparatorSize.Y;
        }
    }
}
