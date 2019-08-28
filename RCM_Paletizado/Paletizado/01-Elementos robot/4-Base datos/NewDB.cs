using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.IO;

namespace Idpsa
{
   public enum Turno
    {
        mañana,
        tarde,
        noche
    }

   public enum Entrada
   {
       ManualProdec,
       ManualRobotEnlace,
       Ruhlamat,
       Unomatic
   }

       [Serializable]
       public class StoredDBCatalog
       {
           //grupos propios de la zona externa
           public string _catalogoID, _firstPassport, _lastPassport; //datos del catálogo que se está produciendo.
           public string _idFirstPassportProduced, _idLastPassportProduced;
           public DateTime _fecha;
           public Turno _turno;
           public int NChars;
           public string _idFirstBoxProduced, _idLastBoxProduced;
           public List<PaletizerDB> ZonaPaletizado;
           public Entrada _entrada;
           public List<GrupoPasaportes> _aereos;

           public StoredDBCatalog(CatalogoDB catalago)
           {
               _catalogoID = catalago.catalogoID; //MDG.2011-11-18//new Pasaporte();
               _idFirstPassportProduced = catalago.idFirstPassportProduced;
               _idLastPassportProduced = catalago.idLastPassportProduced;
               _firstPassport=catalago.firstPassport;
               _lastPassport = catalago.lastPassport;
               _idFirstBoxProduced = catalago.idFirstBoxProduced;
               _idLastBoxProduced = catalago.idLastBoxProduced;
               _fecha = catalago.getfecha;
               _turno = catalago.turnoTurno;
               NChars = catalago.NChars;
               _entrada = catalago.getEntrada();
               ZonaPaletizado = new List<PaletizerDB>();
               if (_entrada != Entrada.Unomatic)
                   _aereos = new List<GrupoPasaportes>();
               if (catalago._aereos != null && catalago._aereos.Count > 0)
                   foreach (var grup in catalago._aereos)
                   {
                       _aereos.Add(grup);
                   }
               if (catalago.ZonaPaletizado!=null)    
               foreach (PaletizerDB p in catalago.ZonaPaletizado)
               {
                   if (p != null)
                   {
                       PaletizerDB pAux = new PaletizerDB(p);
                       ZonaPaletizado.Add(pAux);
                   }
               }
           }
       }

       [Serializable]
       public class PaletizerDB
       {
           private string _idFirstBoxPaletized, _idLastBoxPaletized, _name;
           public int NChars;
           public PaletizerDB(string Nam, int NcharsCat)
           {
               _name = Nam;
               _idFirstBoxPaletized = "";
               _idLastBoxPaletized = "";
               NChars = NcharsCat;
           }
           public PaletizerDB(PaletizerDB p)
           {
               _name = p.Name;
               _idFirstBoxPaletized = p.idFirstBoxPaletized;
               _idLastBoxPaletized = p.idLastBoxPaletized;
               NChars = p.NChars;
           }
           public PaletizerDB()
           {
           }
           public string idLastBoxPaletized
           {
               get
               {
                   return _idLastBoxPaletized;
               }
               set
               {
                   _idLastBoxPaletized = value;
               }
           }
           public string idFirstBoxPaletized
           {
               get
               {
                   return _idFirstBoxPaletized;
               }
               set
               {
                   _idFirstBoxPaletized = value;
               }
           }
           public int cantidadCajasPaletizadas
           {
               get
               {
                   int f, l;
                   if (idFirstBoxPaletized == "")
                       return 0;
                   f = GetNPasaporte(idFirstBoxPaletized);
                   l = GetNPasaporte(idLastBoxPaletized);
                   return (l - f);
               }
           }

           public string Name
           {
               get
               {
                   return _name;
               }
           }
           public int GetNPasaporte(string idPasaporte)
           {
               if (idPasaporte != "")
                   return int.Parse(idPasaporte.Substring(NChars));
               else
                   return 0;
           }
       }
    [Serializable]
   public class CatalogoDB
   {
       private string _catalogoID, _firstPassport, _lastPassport; //datos del catálogo que se está produciendo.
       private string _idFirstPassportProduced, _idLastPassportProduced;
       private string _idFirstBoxProduced, _idLastBoxProduced;
       private DateTime _fecha;
       private Turno _turno;
       public int NChars;
       public bool producing;
       private Entrada _entrada;
       public List<GrupoPasaportes> _aereos;
       private List<PaletizerDB> _zonaPaletizado;
       //private int _cantidad;

       public CatalogoDB(DatosCatalogo catalogo, Entrada entry)
       {
           _catalogoID = catalogo.IdPasaporteIni;
           _firstPassport = catalogo.IdPasaporteIni;
           _lastPassport = catalogo.IdPasaporteEnd;
           _idLastPassportProduced = "";
           _idFirstPassportProduced = "";
           _idFirstBoxProduced="";
           _idLastBoxProduced="";
           producing = false;
           NChars = catalogo.TipoPasaporte.NChars;
           _fecha = DateTime.Today;
           _turno = getTurno();
           string filePath = Path.Combine(ConfigPaletizadoFiles.DBCatalogs, fileName());
           _entrada = entry;
           if (entry == Entrada.Unomatic)
           {
               _zonaPaletizado = new List<PaletizerDB>();
               PaletizerDB palet = new PaletizerDB("Mesa 1",NChars);
               _zonaPaletizado.Add(palet);
               palet=new PaletizerDB("Mesa 2", NChars);
               _zonaPaletizado.Add(palet);
               palet = new PaletizerDB("Paletizado final Linea 1", NChars);
               _zonaPaletizado.Add(palet);
           }
           else
           {

               _aereos = new List<GrupoPasaportes>();
               _zonaPaletizado = new List<PaletizerDB>();
               PaletizerDB palet = new PaletizerDB("Paletizado Final Linea 2",NChars);
               _zonaPaletizado.Add(palet);
           }
           if (File.Exists(filePath))
           {
               using (var readFile = new FileStream(filePath, FileMode.Open, FileAccess.Read))
               {
                   var BFormatter = new BinaryFormatter();
                   var catalog = (StoredDBCatalog)BFormatter.Deserialize(readFile);
                   _idLastPassportProduced = catalog._idLastPassportProduced;
                   _idFirstPassportProduced = catalog._idFirstPassportProduced;
                   _idFirstBoxProduced = catalog._idFirstBoxProduced;
                   _idLastBoxProduced = catalog._idLastBoxProduced;
                   _zonaPaletizado= catalog.ZonaPaletizado;
                   if (catalog._aereos!=null)
                   foreach (var grup in catalog._aereos)
                   {
                       _aereos.Add(grup);
                   }

               }
           }

       }
       public CatalogoDB(StoredDBCatalog catalogo)
       {
           _catalogoID = catalogo._catalogoID;
           _idLastPassportProduced = catalogo._idLastPassportProduced;
           _idFirstPassportProduced = catalogo._idFirstPassportProduced;
           _firstPassport = catalogo._firstPassport;
           _lastPassport = catalogo._lastPassport;
           _zonaPaletizado = catalogo.ZonaPaletizado;
           _entrada = catalogo._entrada;
           _idFirstBoxProduced = catalogo._idFirstBoxProduced;
           _idLastBoxProduced = catalogo._idLastBoxProduced;
           NChars = catalogo.NChars;
           _fecha = catalogo._fecha;
           _turno = catalogo._turno;
           if (_entrada!=Entrada.Unomatic)
               _aereos=new List<GrupoPasaportes>();
           if (catalogo._aereos != null && catalogo._aereos.Count > 0)
           foreach (var grup in catalogo._aereos)
           {
               _aereos.Add(grup);
           }
       }
       public CatalogoDB(CatalogoDB catalogo)
       {
           _catalogoID = catalogo._catalogoID;
           _idLastPassportProduced = catalogo._idLastPassportProduced;
           _idFirstPassportProduced = catalogo._idFirstPassportProduced;
           _firstPassport = catalogo._firstPassport;
           _lastPassport = catalogo._lastPassport;
           _idFirstBoxProduced = catalogo._idFirstBoxProduced;
           _idLastBoxProduced = catalogo._idLastBoxProduced;
           _zonaPaletizado = catalogo.ZonaPaletizado;
           _entrada = catalogo.getEntrada();
           NChars = catalogo.NChars;
           _fecha = DateTime.Today;
           _turno = getTurno();
           if (_entrada != Entrada.Unomatic)
               _aereos = new List<GrupoPasaportes>();
           if (catalogo._aereos != null && catalogo._aereos.Count > 0)
               foreach (var grup in catalogo._aereos)
               {
                   _aereos.Add(grup);
               }
       }
       public bool changeLastPassportProduced(string LastID)
       {
           _idLastPassportProduced=LastID;
           if (_idFirstPassportProduced =="")
           {
               _idFirstPassportProduced = LastID;
               producing = true;
           }
           else
           {
               int f = GetNPasaporte(idFirstPassportProduced);
               int l = GetNPasaporte(idLastPassportProduced);

               if (l < f)
               {
                   l = l - 24;
                   _idLastPassportProduced =idLastPassportProduced.Substring(0,NChars)+l.ToString();
               }
               else
               {
                   if (f%5==0)
                   {
                       f = f - 24;
                       _idFirstPassportProduced = idFirstPassportProduced.Substring(0, NChars) + f.ToString();

                   }
               }
               
           }
           return (ActualizarDBCat());
       }

       public bool changeLastBoxProduced(string LastID)
       {
           _idLastBoxProduced = LastID;
           if (_idFirstBoxProduced=="")
           {
               _idFirstBoxProduced = LastID;
           }
           return (ActualizarDBCat());
       }

       public bool changeLastBoxPaletized(string LastID, int r)
       {
           int i=r-1;
           if (r != 2)
           {
               if (r == 4)
                   i = 0;
               ZonaPaletizado[i].idLastBoxPaletized = LastID;
               if (ZonaPaletizado[i].idFirstBoxPaletized == "")
               {
                   ZonaPaletizado[i].idFirstBoxPaletized = LastID;
               }
           }
           else
           {
               if (ZonaPaletizado[i].idFirstBoxPaletized == "")
               {
                   ZonaPaletizado[i].idFirstBoxPaletized = ZonaPaletizado[1].idFirstBoxPaletized;
                   ZonaPaletizado[i].idLastBoxPaletized = ZonaPaletizado[1].idLastBoxPaletized;
               }
               else
                   ZonaPaletizado[i].idLastBoxPaletized = LastID;

           }
           return (ActualizarDBCat());
       }
       public string catalogoID
        {
            get {
                return _catalogoID;
            }
        }
       public string firstPassport        
       {
            get {
                return _firstPassport;
            }
        } 
       public string lastPassport        
        {
            get {
                return _lastPassport;
            }
        } //datos del catálogo que se está produciendo.
       public string idFirstPassportProduced
       {
           get
           {
               return _idFirstPassportProduced;
           }
       }
       public string idLastPassportProduced
       {
           get
           {
               return _idLastPassportProduced;
           }
           set
           {
               _idLastPassportProduced = value;
           }
       }
       public string idFirstBoxProduced
       {
           get
           {
               if (_idFirstBoxProduced!=null)
                   return _idFirstBoxProduced;
               else
               {
                   return ("");
               }
           }
       }
       public string idLastBoxProduced
       {
           get
           {
               if (_idFirstBoxProduced != null)
                   return _idLastBoxProduced;
               else
               {
                   return ("");
               }
           }
           set
           {
               _idLastPassportProduced = value;
           }
       }

       public DateTime getfecha
       {
           get
           {
               return _fecha;
           }
       }
       public String fecha
       {           
           get
           {
               String mes = _fecha.Month.ToString();
               if (_fecha.Month <= 9)
                   mes = "0" + _fecha.Month.ToString();
               String dia = _fecha.Day.ToString();
               if (_fecha.Day <= 9)
                   dia = "0" + _fecha.Day.ToString();
               String aux1 = dia+"/" + mes +"/"+ _fecha.Year.ToString();
               return aux1;
           }
       }
       public Turno turnoTurno
       {
           get
           {
               return _turno;
           }
       }
       public String turno
       {
           get
           {
               if (_turno==Turno.mañana)
                   return ("Mañana");
               if (_turno == Turno.tarde)
                   return ("Tarde");
               if (_turno == Turno.noche)
                   return ("Noche");
               else return ("");
               
           }
       }
       public int cantidadPasaportes
       {
           get
           {
               int f, l;
               if (idFirstPassportProduced == "")
                   return 0;
               f = GetNPasaporte(idFirstPassportProduced);
               l = GetNPasaporte(idLastPassportProduced);

               if (l < f)
               {
                   return (f - l+1);
               }
               return (l - f+1);
           }
       }

        public int cantidadCajasProducidas
       {
           get
           {
               int f, l;
               if (idFirstBoxProduced == "")
                   return 0;
               f = GetNPasaporte(idFirstBoxProduced)/100;
               l = GetNPasaporte(idLastBoxProduced)/100;
               if (l < f)
               {
                   return (f - l+1);
               }
               return (l - f);
           }
       }
        public int GetNPasaporte(string idPasaporte)
       {
           if (idPasaporte != "")
               return int.Parse(idPasaporte.Substring(NChars));
           else
               return 0;
       }
        private void SaveDB(StoredDBCatalog dataCat)
       {
           string filePath = Path.Combine(ConfigPaletizadoFiles.DBCatalogs, fileName());

           using (var writeFile = new FileStream(filePath, FileMode.Create, FileAccess.Write))
           {
               var BFormatter = new BinaryFormatter();
               BFormatter.Serialize(writeFile, dataCat);
           }
       }
        public bool ActualizarDBCat()
       {
           if (turnoTurno!=getTurno())
           {
               return false;
           }
           SaveDB(new StoredDBCatalog(this));
           return true;
       }
        public String fileName()
       {
           String mes = _fecha.Month.ToString();
           if (_fecha.Month<=9)
               mes="0"+_fecha.Month.ToString();
           String dia = _fecha.Day.ToString();
           if (_fecha.Day <= 9)
               dia = "0"+_fecha.Day.ToString();
           String aux1 = _fecha.Year.ToString() + mes+ dia;
           String aux2 = "M";
           if (_turno == Turno.noche)
               aux2 = "N";
           if (_turno == Turno.tarde)
               aux2 = "T";
            String aux3 = "R";
            if (_entrada==Entrada.ManualProdec)
            {
                aux3 = "M";
            }
            else if(_entrada==Entrada.Unomatic)
            {
                aux3 = "U";
            }
           return (aux1 + aux2+aux3+catalogoID);
       }
        public Turno getTurno()
       {
           if (DateTime.Now.Hour > 7 && DateTime.Now.Hour < 15)
               return (Turno.mañana);
           else if (DateTime.Now.Hour >= 15 && DateTime.Now.Hour < 22)
               return (Turno.tarde);
           else
           {
               if (DateTime.Now.Hour <= 23)
                   _fecha.AddDays(0.25);
               return (Turno.noche);
           }

       }

        public Entrada getEntrada()
        {
            return _entrada;
        }
        public String sEntrada
        {
            get
            {
                if (_entrada==Entrada.ManualProdec)
                    return ("Manual Prodec");
                if (_entrada == Entrada.ManualRobotEnlace)
                    return ("Manual Ascensor");
                if (_entrada == Entrada.Ruhlamat)
                    return ("Ruhlamat");
                if (_entrada == Entrada.Unomatic)
                    return ("Casilleros");
                else return ("");

            }
        }
        public String sLinea
        {
            get
            {
                if (_entrada == Entrada.Unomatic)
                    return ("Unomatic");
                else return ("Ruhlamat");

            }
        }
        public List<PaletizerDB> ZonaPaletizado
        {
            get 
            { 
                return _zonaPaletizado; 
            }
        }
        public string idFirstCajaPaletizado1
        {
            get
            {
                return(ZonaPaletizado[0].idFirstBoxPaletized);
            }
        }
        public string idLastCajaPaletizado1
        {
            get
            {
                return(ZonaPaletizado[0].idLastBoxPaletized);
            }
        }
        public string NamePaletizado1
        {
            get
            {
                return(ZonaPaletizado[0].Name);
            }
        }
        public string cantidadCajasPaletizadas1
        {
            get
            {
                int f, l;
                if (idFirstCajaPaletizado1 == "")
                    return "";
                f = GetNPasaporte(ZonaPaletizado[0].idFirstBoxPaletized)/100;
                l = GetNPasaporte(ZonaPaletizado[0].idLastBoxPaletized)/100;
                if (l < f)
                {
                    return (f - l + 1).ToString();
                }
                return (l - f+1).ToString();
            }
        }
        public string idFirstCajaPaletizado2
        {
            get
            {
                if (ZonaPaletizado.Count() < 2)
                    return ("");
                return (ZonaPaletizado[1].idFirstBoxPaletized);
            }
        }
        public string idLastCajaPaletizado2
        {
            get
            {
                if (ZonaPaletizado.Count() < 2)
                    return ("");
                return (ZonaPaletizado[1].idLastBoxPaletized);
            }
        }
        public string NamePaletizado2
        {
            get
            {
                if (ZonaPaletizado.Count() < 2)
                    return ("");
                return (ZonaPaletizado[1].Name);
            }
        }
        public string cantidadCajasPaletizadas2
        {
            get
            {
                if (ZonaPaletizado.Count() < 2)
                    return ("");
                int f, l;
                if (idFirstCajaPaletizado2 == "")
                    return "";
                f = GetNPasaporte(ZonaPaletizado[1].idFirstBoxPaletized) / 100;
                l = GetNPasaporte(ZonaPaletizado[1].idLastBoxPaletized) / 100;

                if (l < f)
                {
                    return (f - l + 1).ToString();
                }
                return (l - f + 1).ToString();
            }
        }
        public string idFirstCajaPaletizado3
        {
            get
            {
                if (ZonaPaletizado.Count() < 2)
                    if (_aereos != null)
                    {
                        if (_aereos.Count() > 0)
                            return (_aereos[0].IdsPasaportes(4).First());
                    }
                    else
                        return ("");
                       
                return (ZonaPaletizado[2].idFirstBoxPaletized);
            }
        }
        public string idLastCajaPaletizado3
        {
            get
            {
                if (ZonaPaletizado.Count() < 2)
                    if (_aereos != null)
                    {
                        if (_aereos.Count() > 0)
                            return (_aereos[0].IdsPasaportes(4).First());
                    }
                    else
                            return ("");
                return (ZonaPaletizado[2].idLastBoxPaletized);
            }
        }
        public string NamePaletizado3
        {
            get
            {
                if (ZonaPaletizado.Count() < 2)
                    if (_aereos != null)
                    {
                        if (_aereos.Count() > 0)
                            return ("Transportes Aereos");
                    }
                    else
                        return ("");
                return (ZonaPaletizado[2].Name);
            
            }
        }
        public string cantidadCajasPaletizadas3
        {
            get
            {
                int f, l;
                if (ZonaPaletizado.Count() < 2)
                    if (_aereos != null)
                        return _aereos.Count().ToString();
                    //if (_aereos.Count() > 0)
                    //{

                    //    //f = GetNPasaporte(_aereos[0].Last());
                    //    //l = GetNPasaporte(idLastCajaPaletizado3);
                    //    //return (l - f + 1).ToString();
                    //}
                    else
                        return ("");
                if (idFirstCajaPaletizado3 == "")
                    return "";
                f = GetNPasaporte(ZonaPaletizado[2].idFirstBoxPaletized) / 100;
                l = GetNPasaporte(ZonaPaletizado[2].idLastBoxPaletized) / 100;

                if (l < f)
                {
                    return (f - l + 1).ToString();
                }
                return (l - f + 1).ToString();
            }
        }
   }

   
    public class datosTurno
    {
       private DateTime _fecha;
       private Turno _turno;
       private List<CatalogoDB> _CatalogArray;
       private int _numeroPasaportesProducidos;

        public datosTurno(DatosCatalogo catalogo, Entrada entry)
        {
            _fecha = DateTime.Today;
            _turno = getTurno();
            if (_CatalogArray.Count > 0)
                foreach (CatalogoDB element in _CatalogArray)
                    _CatalogArray.Remove(element);
            _CatalogArray.Add(new CatalogoDB(catalogo, entry));
        }
        
        public datosTurno()
        {
            _fecha = DateTime.Today;
            _turno = getTurno();
            _CatalogArray = new List<CatalogoDB>();
            if(_CatalogArray.Count>0)
                foreach (CatalogoDB element in _CatalogArray)
                    _CatalogArray.Remove(element);
        }

        public datosTurno(datosTurno newData)
        {
            _fecha = newData._fecha;
            _turno = newData._turno;
            if (_CatalogArray.Count > 0)
                foreach (CatalogoDB element in _CatalogArray)
                    _CatalogArray.Remove(element);
            foreach (CatalogoDB element in newData._CatalogArray)
            {
                _CatalogArray.Add(element);
            }
        }

        public Turno getTurno()
        {
            if (DateTime.Now.Hour > 8 && DateTime.Now.Hour < 15)
                return (Turno.mañana);
            else if (DateTime.Now.Hour >= 15 && DateTime.Now.Hour < 22)
                return (Turno.tarde);
            else
                return (Turno.noche);
        }

        public void newCatalog(DatosCatalogo catalogo, Entrada entry)
        {
            _CatalogArray.Add(new CatalogoDB(catalogo, entry));
        }

        public void newCatalog(CatalogoDB catalogo)
        {
            _CatalogArray.Add(catalogo);
        }

        public void lastPassport(string LastProduced)
        {
            CatalogoDB aux= _CatalogArray.Last();
            aux.idLastPassportProduced = LastProduced;
            CatalogoDB aux2 = _CatalogArray.Last();
            _CatalogArray.Remove(aux2);
            _CatalogArray.Add(aux);
        }

        public String fileName()
        {
            String aux1=_fecha.Year.ToString()+_fecha.Month.ToString()+_fecha.Day.ToString();
            String aux2 = "mañana";
            if (_turno == Turno.noche)
                aux2 = "noche";
            if (_turno == Turno.tarde)
                aux2 = "tarde";
            return (aux1 + aux2);
        }

        public DateTime fecha
        {
            get {
            return _fecha;
            }
        }
        public Turno turno
        {
            get
            {
                return _turno;
            }
        }
        public String sturno
        {
            get
            {
                if (_turno == Turno.mañana)
                    return ("M");
                if (_turno == Turno.tarde)
                    return ("T");
                if (_turno == Turno.noche)
                    return ("N");
                else return ("");

            }
        }
        public List<CatalogoDB> CatalogArray
        {
            get
            {
                return _CatalogArray;
            }
        }
        public List<CatalogoDB> getCatalogs()
        {
            
                return _CatalogArray;
            
        }
        public int numeroPasaportesProducidos
        {
            get
            {
                return _numeroPasaportesProducidos;
            }
        }

        public StoredDBCatalog LoadDatosCatalogo(string fileName)
        {
            string filePath = Path.Combine(ConfigPaletizadoFiles.DBCatalogs, fileName);

            using (var readFile = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var BFormatter = new BinaryFormatter();
                var catalog = (StoredDBCatalog)BFormatter.Deserialize(readFile);
                return catalog;
            }
        }

        public bool SearchDB(string turn, string fech, string idcat)
        {
            string actualFile = "";
            if (fech == "" && idcat == "")
                return false;
            try
            {
                var directory = new DirectoryInfo(ConfigPaletizadoFiles.DBCatalogs);
                FileInfo[] files = directory.GetFiles();
                var bFormatter = new BinaryFormatter();
                CatalogArray.Clear();

                foreach (FileInfo file in files)
                {
                    actualFile = file.Name;
                    bool b = ReadCatalogFromFile(actualFile, turn, fech,idcat);
                    if (b)
                        CatalogArray.Add(new CatalogoDB(LoadDatosCatalogo(actualFile)));
                }
                if (CatalogArray.Count() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool ReadCatalogFromFile(String actualFile, String turn,String fech,String idcat)
        {
            String auxTurn=actualFile.Substring(8,1);
            String Auxfech = actualFile.Substring(0,8);
            String auxId = actualFile.Substring(10, idcat.Length);
            if (turn == "")
            {
                if (fech == "")
                {
                    if (idcat == auxId)
                        return true;
                }
                else if (Auxfech == fech)
                    if (idcat == "" || idcat == auxId)
                        return true;
            }
            else if (auxTurn == turn)
                if (fech == "")
                {
                    if (idcat == auxId)
                        return true;
                }
                else if (Auxfech == fech)
                    if (idcat == "" || idcat == auxId)
                        return true;
            return false;
        }

        public void LoadDatos()
        {
            String mes = _fecha.Month.ToString();
            if (_fecha.Month <= 9)
                mes = "0" + _fecha.Month.ToString();
            String dia = _fecha.Day.ToString();
            if (_fecha.Day <= 9)
                dia = "0" + _fecha.Day.ToString();
            String aux1 = _fecha.Year.ToString() + mes + dia;
            SearchDB(sturno, aux1, "");

        }

    }

    public class NewDB
    {
        public List<datosTurno> DBdata;
        public int numeroPasaportesTotal;
        
        public NewDB(datosTurno newData)
        {
            DBdata.Add(new datosTurno(newData));
            StoreTurno();
            
        }

        public List<datosTurno> GetTurnos()
        {
            return DBdata;
        }

        public void StoreTurno()
        {
            if (DBdata == null)
                return;

            //Catalog.EstadoCatalogoAlmacenado = new DatosCatalogo.EstadoCatalogo(this);

            //string filePath = Path.Combine(ConfigFiles.Catalogs, Catalog.Name);

            //using (var writeFile = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            //{
            //    var BFormatter = new BinaryFormatter();
            //    BFormatter.Serialize(writeFile, Catalog);
            //}
        }
        private void LoadTurno()
        {
            //string actualFile = "";
            //try
            //{
            //    var directory = new DirectoryInfo(ConfigFiles.Catalogs);
            //    FileInfo[] files = directory.GetFiles();
            //    var bFormatter = new BinaryFormatter();
            //    _catalogs.Clear();

            //    foreach (FileInfo file in files)
            //    {
            //        actualFile = file.FullName;
            //        ReadCatalogFromFile(actualFile);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ReadCatalogFromFile(actualFile);
            //}
        }
        private static void DeleteTurno()
        {
            //var directory = new DirectoryInfo(ConfigFiles.Catalogs);
            //FileInfo[] files = directory.GetFiles();
            //if (files.Count() <= 9999) return;
            //FileInfo file = files.OrderBy(f => f.CreationTime).Last();
            //if (file == null) return;
            //if (file.Exists)
            //{
            //    file.Delete();
            //}
        }
        private static void ModifyTurno()
        {

        }        
    }
}
