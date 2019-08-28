using System;
using System.Collections.Generic;
using RCMCommonTypes;

namespace RECatalogManager
{
    [Serializable]
    public class DatosCatalogoRobotEnlace : DatosCatalogo
    {
        public DatosCatalogoRobotEnlace(TipoPasaporte tipo,
                                        string idPasaporteIni,
                                        string idPasaporteEnd,
                                        string FechaLam)
            : base(tipo, idPasaporteIni, idPasaporteEnd, FechaLam)
        {
            IdLine = IDLine.Alemana;
        }

        public IDLine IdLine { get; set; }
        public StoreStateCatalog SotoredData { get; set; }
    }

    [Serializable]
    public class StoredDataRobotEnlace
    {
        public IList<string> Boxes;
        public int CurrentGroupIndex;

        public StoredDataRobotEnlace()
        {
            Boxes = new List<string>();
            CurrentGroupIndex = -1;
        }
    }

    [Serializable]
    public abstract class StoreStateCatalog
    {
        public abstract StoredDataRobotEnlace GetDataPaletizer();
        public abstract IEnumerable<string> GetAllBoxes();
        public abstract int GetCurrectGroupIndex();
    }

    [Serializable]
    public class StoredDataCatalogoLinea : StoreStateCatalog
    {
        private readonly StoredDataRobotEnlace _paletizadoAlemanaData;

        public StoredDataCatalogoLinea(StoredDataRobotEnlace _paletizado)
        {
            _paletizadoAlemanaData = _paletizado;
            //_paletizado.CurrentGroupIndex;
        }

        public override StoredDataRobotEnlace GetDataPaletizer()
        {
            return _paletizadoAlemanaData;
        }

        public override IEnumerable<string> GetAllBoxes()
        {
            return _paletizadoAlemanaData.Boxes;
        }

        public override int GetCurrectGroupIndex()
        {
            return _paletizadoAlemanaData.CurrentGroupIndex;
        }
    }
}