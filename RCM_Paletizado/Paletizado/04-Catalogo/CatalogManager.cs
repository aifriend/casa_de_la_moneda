using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Idpsa.Control;
using Idpsa.Control.Engine;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    public enum CatalogManagerStates
    {
        NoIniciado,
        EnProceso,
        UltimoGrupo, //MCR. 2016
        Finalizado
    }

    public enum CatalogManagerId
    {
        Id1,
        Id2
    }

    public class CatalogManager
    {
        private readonly Dictionary<string, CajaPasaportes> boxes;
        private readonly Dictionary<string, GrupoPasaportes> groups;
        private readonly Dictionary<string, Pasaporte> passPorts;

        private Dictionary<string, CajaPasaportes> cajasRealizadas;
        private int count;
        private CatalogManagerStates estado;

        public CatalogManager(DatosCatalogoPaletizado catalog)
        {
            Catalog = catalog;
            passPorts = new Dictionary<string, Pasaporte>();
            groups = new Dictionary<string, GrupoPasaportes>();
            boxes = new Dictionary<string, CajaPasaportes>();
            cajasRealizadas = new Dictionary<string, CajaPasaportes>();
            CreatePassPortsCatalog();
            CreateGroupsCatalog();
            CreateBoxesCatalog();
        }

        public CatalogManagerStates Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        public DatosCatalogoPaletizado Catalog { get; private set; }

        public int PassportsCount
        {
            get { return passPorts.Count; }
        }

        public int GroupsCount
        {
            get { return groups.Count; }
        }

        public int BoxesCount
        {
            get { return boxes.Count; }
        }

        #region Para pruebas

        private GrupoPasaportes CreateGroup(int index)
        {
            var g = new GrupoPasaportes();
            Pasaporte p;
            bool firstTimeReaded;
            if (!TryGetReadedPassport(GetPassPortId(index*GrupoPasaportes.NMaximo), out p, out firstTimeReaded))
                return null;

            g.CopyPassportIntrinsicInformation(p);

            for (int i = 0; i < GrupoPasaportes.NMaximo; i++)
            {
                TryGetPassPort(GetPassPortId(index*GrupoPasaportes.NMaximo + i), out p);
                g.Add(p);
            }
            g.IdLine = Catalog.IDLine;
            g.CatalogIndex = index;
            g.FechaLam = Catalog.FechLaminacion; //MCR. 2011/03/03.
            return g;
        }

        public GrupoPasaportes GetGroup(int index)
        {
            return index >= groups.Count ? null : groups.ElementAt(index).Value;
        }


        private GrupoPasaportes GetGroup(string idFirstPassPort)
        {
            var g = new GrupoPasaportes();
            Pasaporte p;
            TryGetPassPort(idFirstPassPort, out p);
            g.CopyPassportIntrinsicInformation(p);
            int nPasaporteAux = p.NPasaporte;
            for (int i = 0; i < GrupoPasaportes.NMaximo; i++)
            {
                string idPassPortAux = Pasaporte.GetId(g.NSerie, nPasaporteAux, Catalog.TipoPasaporte);
                g.Add(passPorts[idPassPortAux]);
                nPasaporteAux++;
            }
            g.IdLine = Catalog.IDLine;
            g.FechaLam = Catalog.FechLaminacion; //MCR. 2011/03/03.
            return g;
        }

        //MDG.2011-07-05. Cogemos grupo usando su Id
        public GrupoPasaportes GetGroupFromId(string idGroup)
        {
            if (groups.ContainsKey(idGroup))
                return groups[idGroup];
            return null;
        }


        private CajaPasaportes CreateBox(int boxIndex)
        {
            var grupos = new List<GrupoPasaportes>();

            for (int i = 0; i < CajaPasaportes.NGrupos; i++)
            {
                GrupoPasaportes group = GetGroup(boxIndex*CajaPasaportes.NGrupos);
                if (group == null) return null;
                grupos.Add(group);
            }
            CajaPasaportes caja = null;
            caja = new CajaPasaportes(grupos.ElementAt(0));
            for (int i = 0; i < grupos.Count; i++)
            {
                caja.Add(grupos.ElementAt(i), CajaPasaportes.NGrupos - i - 1);
            }
            //caja.Grupos[0].LastOfBox = true;
            caja.IdLine = Catalog.IDLine;
            if (Catalog.IDLine == IDLine.Alemana)
                //MDg.2010-12-15.Para que el cuarto grupo introducido por la Alemana sea el que cierre la caja
                caja.Grupos[0].LastOfBox = true; //no funciona//caja.Grupos[3].LastOfBox = true;
            else
                caja.Grupos[0].LastOfBox = true;

            caja.CatalogIndex = boxIndex;

            return caja;
        }

        public CajaPasaportes GetBox(int index)
        {
            if (index >= boxes.Count || index < 0)
                return null;
            else return boxes.ElementAt(index).Value;
        }

        public CajaPasaportes GetBox(string idCaja)
        {
            if (boxes.ContainsKey(idCaja))
                return boxes[idCaja];
            return null;
        }

        #endregion

        public bool NuevoCatalogoPermitido()
        {
            return (estado == CatalogManagerStates.NoIniciado ||
                    estado == CatalogManagerStates.Finalizado);
        }

        private void CreatePassPortsCatalog()
        {
            passPorts.Clear();
            string NSerie = Pasaporte.GetNSerie(Catalog.IdPasaporteIni, Catalog.TipoPasaporte);
            int NPasaporteIni = Pasaporte.GetNPasaporte(Catalog.IdPasaporteIni, Catalog.TipoPasaporte);
            int NPasaporteEnd = Pasaporte.GetNPasaporte(Catalog.IdPasaporteEnd, Catalog.TipoPasaporte);

            for (int i = NPasaporteIni; i <= NPasaporteEnd; i++)
            {
                string id = Pasaporte.GetId(NSerie, i, Catalog.TipoPasaporte);
                var p = new Pasaporte(Catalog.TipoPasaporte, id);
                p.Rfid1 = p.NPasaporte.ToString();
                passPorts.Add(id, p);
            }
        }

        private void CreateGroupsCatalog()
        {
            groups.Clear();
            int index = 0;
            GrupoPasaportes grupo = null;
            while ((grupo = CreateGroup(index++)) != null)
            {
                groups.Add(grupo.Id, grupo);
            }
        }

        private void CreateBoxesCatalog()
        {
            boxes.Clear();
            int index = 0;
            CajaPasaportes caja;
            while ((caja = CreateBox(index++)) != null)
            {
                boxes.Add(caja.Id, caja);
            }
        }

        public void SaveCatalog(StoreStateCatalog storeStateCatalog)
        {
            string path = Path.Combine(ConfigPaletizadoFiles.CatalogsPaletizado, Catalog.Name);
            Catalog.SotoredData = storeStateCatalog;

            using (var writeFile = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var BFormatter = new BinaryFormatter();
                BFormatter.Serialize(writeFile, Catalog);
            }
        }

        //MDG.2010-12-02.Salvado de los datos de los dos ascensores y los 2 transportadores elevados de la linea 2

        //ASCENSOR 1
        public void SaveElevator1Data(StoredDataElevator1Group storeStateElevatorData)
        {
            string path = Path.Combine(ConfigFiles.Groups, storeStateElevatorData.Name); //"Elevator1");//Catalog.Name);

            using (var writeFile = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var BFormatter = new BinaryFormatter();
                BFormatter.Serialize(writeFile, storeStateElevatorData); //Catalog);
            }
        }

        public StoredDataElevator1Group LoadElevator1Data()
        {
            string path = Path.Combine(ConfigFiles.Groups, "Elevator1");

            using (var readFile = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var BFormat = new BinaryFormatter();
                var catalog = (StoredDataElevator1Group) BFormat.Deserialize(readFile);
                return catalog;
            }
        }

        //TRAMOS DE TRANSPORTE 1 y 2
        public void SaveTramoTransporteData(StoredDataTransportGroups storeStateTransportData)
        {
            string path = Path.Combine(ConfigFiles.Groups, storeStateTransportData.Name);
            //"Elevator1");//Catalog.Name);

            using (var writeFile = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var BFormatter = new BinaryFormatter();
                BFormatter.Serialize(writeFile, storeStateTransportData); //Catalog);
            }
        }

        public StoredDataTransportGroups LoadTramoTransporteData(string TramoName)
        {
            string path = Path.Combine(ConfigFiles.Groups, TramoName); //"Tramo1");

            using (var readFile = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var BFormat = new BinaryFormatter();
                var catalog = (StoredDataTransportGroups) BFormat.Deserialize(readFile);
                return catalog;
            }
        }

        //ASCENSOR 2
        public void SaveElevator2Data(StoredDataElevator2Group storeStateElevatorData)
        {
            string path = Path.Combine(ConfigFiles.Groups, storeStateElevatorData.Name); //"Elevator2");//Catalog.Name);

            using (var writeFile = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var BFormatter = new BinaryFormatter();
                BFormatter.Serialize(writeFile, storeStateElevatorData); //Catalog);
            }
        }

        public StoredDataElevator2Group LoadElevator2Data()
        {
            string path = Path.Combine(ConfigFiles.Groups, "Elevator2");

            using (var readFile = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var BFormat = new BinaryFormatter();
                var catalog = (StoredDataElevator2Group) BFormat.Deserialize(readFile);
                return catalog;
            }
        }

        //MDG.2010-12-10.Salvado de los datos de las banda comunes a las dos lineas

        //BANDA SALIDA ENFAJADORA
        public void SaveBandaSalidaEnfajadoraData(StoredDataBandaSalidaEnfajadoraGroups storeStateTransportData)
        {
            string path = Path.Combine(ConfigFiles.Groups, "BandaSalidaEnfajadora"); //"Elevator1");//Catalog.Name);

            using (var writeFile = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var BFormatter = new BinaryFormatter();
                BFormatter.Serialize(writeFile, storeStateTransportData); //Catalog);
            }
        }

        public StoredDataBandaSalidaEnfajadoraGroups LoadBandaSalidaEnfajadoraData()
        {
            string path = Path.Combine(ConfigFiles.Groups, "BandaSalidaEnfajadora"); //"Tramo1");

            using (var readFile = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var BFormat = new BinaryFormatter();
                var catalog = (StoredDataBandaSalidaEnfajadoraGroups) BFormat.Deserialize(readFile);
                return catalog;
            }
        }

        //PRODEC
        public void SaveProdecData(StoredDataProdecGroups storeStateTransportData)
        {
            string path = Path.Combine(ConfigFiles.Groups, "Prodec"); //"Elevator1");//Catalog.Name);

            using (var writeFile = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var BFormatter = new BinaryFormatter();
                BFormatter.Serialize(writeFile, storeStateTransportData); //Catalog);
            }
        }

        public StoredDataProdecGroups LoadProdecData()
        {
            string path = Path.Combine(ConfigFiles.Groups, "Prodec"); //"Tramo1");

            using (var readFile = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var BFormat = new BinaryFormatter();
                var catalog = (StoredDataProdecGroups) BFormat.Deserialize(readFile);
                return catalog;
            }
        }

        //BANDA ETIQUETADO
        public void SaveBandaEtiquetadoData(StoredDataBandaEtiquetadoBox storeStateTransportData)
        {
            string path = Path.Combine(ConfigFiles.Groups, "BandaEtiquetado"); //"Elevator1");//Catalog.Name);

            using (var writeFile = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var BFormatter = new BinaryFormatter();
                BFormatter.Serialize(writeFile, storeStateTransportData); //Catalog);
            }
        }

        public StoredDataBandaEtiquetadoBox LoadBandaEtiquetadoData()
        {
            string path = Path.Combine(ConfigFiles.Groups, "BandaEtiquetado"); //"Tramo1");

            using (var readFile = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var BFormat = new BinaryFormatter();
                var catalog = (StoredDataBandaEtiquetadoBox) BFormat.Deserialize(readFile);
                return catalog;
            }
        }

        //BANDA REPROCESO
        public void SaveBandaReprocesoData(StoredDataBandaReprocesoBox storeStateTransportData)
        {
            string path = Path.Combine(ConfigFiles.Groups, "BandaReproceso"); //"Elevator1");//Catalog.Name);

            using (var writeFile = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var BFormatter = new BinaryFormatter();
                BFormatter.Serialize(writeFile, storeStateTransportData); //Catalog);
            }
        }

        public StoredDataBandaReprocesoBox LoadBandaReprocesoData()
        {
            string path = Path.Combine(ConfigFiles.Groups, "BandaReproceso"); //"Tramo1");

            using (var readFile = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var BFormat = new BinaryFormatter();
                var catalog = (StoredDataBandaReprocesoBox) BFormat.Deserialize(readFile);
                return catalog;
            }
        }


        private bool TryGetReadedPassport(string id, out Pasaporte p, out bool firstTimeReaded)
        {
            bool value = false;
            firstTimeReaded = false;
            if (TryGetPassPort(id, out p))
            {
                value = true;

                firstTimeReaded = !p.IdLeido;
                p.IdLeido = true;

                if (Estado == CatalogManagerStates.NoIniciado)
                {
                    Estado = CatalogManagerStates.EnProceso;
                    Catalog.DBAction = DBAction.Insert;
                    ControlLoop<IdpsaSystemPaletizado>.Instance.Sys.Db.ProcessElement(Catalog);
                }

                if ((p.NPasaporte%GrupoPasaportes.NMaximo) == 0)
                {
                    p.CierraGrupo = true;
                }
            }

            return value;
        }

        public bool TryGetPassPort(string idpassPort, out Pasaporte p)
        {
            bool value = false;
            p = null;
            if (passPorts.ContainsKey(idpassPort))
            {
                value = true;
                p = passPorts[idpassPort];
            }
            return value;
        }

        private bool TryGetGroup(string idGrupo, out GrupoPasaportes grupo)
        {
            bool value = false;
            grupo = null;
            string idFirstPassport = idGrupo.Substring(0, idGrupo.Length - 1) +
                                     (int.Parse(idGrupo[idGrupo.Length - 1].ToString()) + 1);

            Pasaporte p;
            if (TryGetPassPort(idFirstPassport, out p))
            {
                if ((grupo = GetGroup(idFirstPassport)) != null)
                {
                    value = true;
                }
            }

            return value;
        }

        private string getNextIdPassPort()
        {
            string id = null;
            if (count < Catalog.NPasaportes())
            {
                string NSerie = Pasaporte.GetNSerie(Catalog.IdPasaporteIni, Catalog.TipoPasaporte);
                int NPasaporteIni = Pasaporte.GetNPasaporte(Catalog.IdPasaporteIni, Catalog.TipoPasaporte);
                id = Pasaporte.GetId(NSerie, NPasaporteIni + count, Catalog.TipoPasaporte);
                count++;
            }
            return id;
        }

        private string GetPassPortId(int orden)
        {
            string NSerie = Pasaporte.GetNSerie(Catalog.IdPasaporteIni, Catalog.TipoPasaporte);
            int NPasaporteIni = Pasaporte.GetNPasaporte(Catalog.IdPasaporteIni, Catalog.TipoPasaporte);
            string id = Pasaporte.GetId(NSerie, NPasaporteIni + orden, Catalog.TipoPasaporte);
            return id;
        }
    }
}