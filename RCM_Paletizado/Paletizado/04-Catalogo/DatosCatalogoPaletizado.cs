using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Paletizado.Definitions;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    [Serializable]
    public class PaletizerDefinition
    {
        public PaletizerDefinition()
        {
            MosaicTypes = new List<MosaicType>();
        }

        public PaletizerDefinition(string name)
        {
            Name = name;
            MosaicTypes = new List<MosaicType>();
        }

        public PaletizerDefinition(PaletizerDefinition palDef)
        {
            MosaicType[] MosaicAux = new MosaicType[6];
            MosaicTypes = new List<MosaicType>();
            foreach (MosaicType M in palDef.MosaicTypes)
                palDef.MosaicTypes.CopyTo(MosaicAux);
            for (int i=0; i<palDef.MosaicTypes.Count();i++)
                MosaicTypes.Add(MosaicAux[i]);
            Item = palDef.Item;
            Palet = palDef.Palet;
            Separator = palDef.Separator;
            CoparerType = palDef.CoparerType;
        }

        public string Name { get; private set; }
        public IPaletizable Item { get; set; }
        public IPalet Palet { get; set; }
        public ISeparator Separator { get; set; }
        public List<MosaicType> MosaicTypes { get; set; }
        public ComparerMosaicTypes CoparerType { get; set; }
    }

    [Serializable]
    public class DatosCatalogoPaletizado : DatosCatalogo
    {
        public DatosCatalogoPaletizado(TipoPasaporte tipo,
                                       string idPasaporteIni, string idPasaporteEnd,
                                       PaletizerDefinition paletizerDefinition, IDLine idLine, string FechaLam)
            // //MCR. 2011/03/03. Feha Laminacion
            : base(tipo, idPasaporteIni, idPasaporteEnd, FechaLam)
        {
            PaletizerDefinition = paletizerDefinition;
            IDLine = idLine;
        }
        public DatosCatalogoPaletizado(DatosCatalogoPaletizado datosCatalogo,
                                       PaletizerDefinition paletizerDefinition)
            // //MCR. 2011/03/03. Feha Laminacion
            : base(datosCatalogo.TipoPasaporte, datosCatalogo.IdPasaporteIni, datosCatalogo.IdPasaporteEnd, datosCatalogo.FechLaminacion)
        {
            PaletizerDefinition = paletizerDefinition;
            IDLine = datosCatalogo.IDLine;
            this.SotoredData = datosCatalogo.SotoredData;
            this.DBAction = datosCatalogo.DBAction;
        }

        public PaletizerDefinition PaletizerDefinition { get; set; }
        public IDLine IDLine { get; set; }
        public StoreStateCatalog SotoredData { get; set; }
    }

    [Serializable]
    public class StoredDataPaletizado
    {
        public IList<string> Boxes;
        //public int CurrentBoxIndex;//MDG.2010-12-09.Almacenamos el indice del ultimo grupo de pasaportes que ha entrado en esta linea
        public int CurrentGroupIndex;
        //MDG.2010-12-09.Almacenamos el indice del ultimo grupo de pasaportes que ha entrado en esta linea

        public Paletizer.States State;

        public StoredDataPaletizado()
        {
            Boxes = new List<string>();
            //CurrentBoxIndex = -1;//MDG.2010-12-09.Almacenamos el indice del ultimo grupo de pasaportes que ha entrado en esta linea
            CurrentGroupIndex = -1;
            //MDG.2010-12-09.Almacenamos el indice del ultimo grupo de pasaportes que ha entrado en esta linea
        }
    }

    [Serializable]
    public abstract class StoreStateCatalog
    {
        public abstract StoredDataPaletizado GetDataPaletizer(Locations location);
        public abstract IEnumerable<string> GetAllBoxes();
        public abstract int GetCurrectGroupIndex(); //MDG.2010-12-09
    }

    [Serializable]
    public class StoredStateCatalogoLinea1 : StoreStateCatalog, IEnumerable<StoredDataPaletizado>
    {
        private readonly StoredDataPaletizado _paletizado1JaponesaData;
        private readonly StoredDataPaletizado _paletizado2JaponesaData;
        private readonly StoredDataPaletizado _paletizado3JaponesaData;

        public StoredStateCatalogoLinea1
            (
            StoredDataPaletizado paletizado1,
            StoredDataPaletizado paletizado2,
            StoredDataPaletizado paletizado3)
        {
            _paletizado1JaponesaData = paletizado1;
            _paletizado2JaponesaData = paletizado2;
            _paletizado3JaponesaData = paletizado3;
        }

        public override StoredDataPaletizado GetDataPaletizer(Locations location)
        {
            switch (location)
            {
                case Locations.Paletizado1Japonesa:
                    return _paletizado1JaponesaData;
                case Locations.Paletizado2Japonesa:
                    return _paletizado2JaponesaData;
                case Locations.Paletizado3Japonesa:
                    return _paletizado3JaponesaData;
                default:
                    return null;
            }
        }

        public override IEnumerable<string> GetAllBoxes()
        {
            IEnumerable<string> boxes = Enumerable.Empty<string>();
            foreach (StoredDataPaletizado data in this)
            {
                if (data.Boxes != null)
                    boxes = boxes.Concat(data.Boxes);
            }

            return boxes;
        }

        public override int GetCurrectGroupIndex() //MDG.2010-12-09
        {
            return -1;
        }

        #region Miembros de IEnumerable<StoredDataCatalogoLinea2>

        public IEnumerator<StoredDataPaletizado> GetEnumerator()
        {
            yield return _paletizado1JaponesaData;
            yield return _paletizado2JaponesaData;
            yield return _paletizado3JaponesaData;
        }

        #endregion

        #region Miembros de IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

    [Serializable]
    public class StoredDataCatalogoLinea2 : StoreStateCatalog
    {
        private readonly StoredDataPaletizado _paletizadoAlemanaData;

        public StoredDataCatalogoLinea2(StoredDataPaletizado _paletizado)
        {
            _paletizadoAlemanaData = _paletizado;
            //_paletizado.CurrentGroupIndex;
        }

        public override StoredDataPaletizado GetDataPaletizer(Locations location)
        {
            if (location == Locations.PaletizadoAlemana)
                return _paletizadoAlemanaData;
            return null;
        }

        public override IEnumerable<string> GetAllBoxes()
        {
            return _paletizadoAlemanaData.Boxes;
        }

        public override int GetCurrectGroupIndex() //MDG.2010-12-09
        {
            return _paletizadoAlemanaData.CurrentGroupIndex;
        }
    }

    //MDG.2010-12-02
    [Serializable]
    public class StoredDataElevator1Group
    {
        public GrupoPasaportes Group; //=new GrupoPasaportes();
        public string Name; // = "Elevator1";
        public Elevador1.States State; //=Elevador1.States.Bajar;

        public StoredDataElevator1Group()
        {
            Group = new GrupoPasaportes();
            State = Elevador1.States.Bajar;
            Name = "Elevator1";
        }
    }

    [Serializable]
    public class StoredDataElevator2Group
    {
        public GrupoPasaportes Group; //=new GrupoPasaportes();
        public string Name; // = "Elevator2";
        public Elevador2.States State; //=Elevador2.States.Bajar;

        public StoredDataElevator2Group()
        {
            Group = new GrupoPasaportes();
            State = Elevador2.States.Bajar;
            Name = "Elevator2";
        }
    }


    [Serializable]
    public class StoredDataTransportGroups
    {
        //public IdpsaControl.Tool.FifoList<GrupoPasaportes> Groups; // IList<string> Groups;
        public List<GrupoPasaportes> Groups; // IList<string> Groups;
        public bool ModoAcumulacion; //MDG.2010-12-09
        public string Name; // = "TramoTransporte1";
        public TramoTransporteGruposPasaportes.States State;
        public bool Vaciar; //MDG.2010-12-09

        public StoredDataTransportGroups()
        {
            //Groups = new IdpsaControl.Tool.FifoList<GrupoPasaportes>();//List<string>();
            Groups = new List<GrupoPasaportes>(); //List<string>();
            //Name = "Tramo 1";
            Vaciar = false;
            ModoAcumulacion = false;
        }
    }

    //MDG.2010-12-10
    [Serializable]
    public class StoredDataBandaSalidaEnfajadoraGroups
    {
        //public IdpsaControl.Tool.FifoList<GrupoPasaportes> Groups; // IList<string> Groups;
        public List<GrupoPasaportes> Groups; // IList<string> Groups;
        //public TramoTransporteGruposPasaportes.States State;
        public string Name; // = "TramoTransporte1";

        public StoredDataBandaSalidaEnfajadoraGroups()
        {
            //Groups = new IdpsaControl.Tool.FifoList<GrupoPasaportes>();//List<string>();
            Groups = new List<GrupoPasaportes>(); //List<string>();
            //Name = "Tramo 1";
            //Vaciar = false;
            //ModoAcumulacion = false;
        }

        //public bool Vaciar;//MDG.2010-12-09
        //public bool ModoAcumulacion;//MDG.2010-12-09
    }

    [Serializable]
    public class StoredDataProdecGroups
    {
        //public IdpsaControl.Tool.FifoList<GrupoPasaportes> Groups; // IList<string> Groups;
        public List<GrupoPasaportes> Groups; // IList<string> Groups;
        //public TramoTransporteGruposPasaportes.States State;
        public string Name; // = "TramoTransporte1";

        public StoredDataProdecGroups()
        {
            //Groups = new IdpsaControl.Tool.FifoList<GrupoPasaportes>();//List<string>();
            Groups = new List<GrupoPasaportes>(); //List<string>();
            //Name = "Tramo 1";
            //Vaciar = false;
            //ModoAcumulacion = false;
        }

        //public bool Vaciar;//MDG.2010-12-09
        //public bool ModoAcumulacion;//MDG.2010-12-09
    }

    [Serializable]
    public class StoredDataBandaEtiquetadoBox
    {
        //public IdpsaControl.Tool.FifoList<GrupoPasaportes> Groups; // IList<string> Groups;
        //public List<GrupoPasaportes> Groups; // IList<string> Groups;
        public CajaPasaportes Caja;
        public string Name; // = "TramoTransporte1";
        public BandaEtiquetado.States State;

        public StoredDataBandaEtiquetadoBox()
        {
            //Groups = new IdpsaControl.Tool.FifoList<GrupoPasaportes>();//List<string>();
            //Groups = new List<GrupoPasaportes>();//List<string>();
            Name = "BandaEtiquetado";
            //Vaciar = false;
            //ModoAcumulacion = false;
            Caja = null; // new CajaPasaportes();
        }

        //public bool Vaciar;//MDG.2010-12-09
        //public bool ModoAcumulacion;//MDG.2010-12-09
    }

    [Serializable]
    public class StoredDataBandaReprocesoBox
    {
        //public IdpsaControl.Tool.FifoList<GrupoPasaportes> Groups; // IList<string> Groups;
        //public List<GrupoPasaportes> Groups; // IList<string> Groups;
        public CajaPasaportes Caja;
        public string Name; // = "TramoTransporte1";
        public ReprocesadorManual.States State;

        public StoredDataBandaReprocesoBox()
        {
            //Groups = new IdpsaControl.Tool.FifoList<GrupoPasaportes>();//List<string>();
            //Groups = new List<GrupoPasaportes>();//List<string>();
            Name = "BandaReproceso";
            //Vaciar = false;
            //ModoAcumulacion = false;
            Caja = null; // new CajaPasaportes();
        }

        //public bool Vaciar;//MDG.2010-12-09
        //public bool ModoAcumulacion;//MDG.2010-12-09
    }
}