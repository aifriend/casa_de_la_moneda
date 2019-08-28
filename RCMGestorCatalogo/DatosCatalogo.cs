using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using RCMCommonTypes;

namespace RECatalogManager
{
    [Serializable]
    public class DatosCatalogo
    {
        private static Dictionary<string, Pasaporte> _pasaportes;
        private string _idPasaporteEnd;
        private string _idPasaporteIni;
        private TipoPasaporte _tipoPasaporte;

        public DatosCatalogo(TipoPasaporte tipoPasaporte, string idPasaporteIni, string idPasaporteEnd, string FechaLam)
            //MCR 2011/03/02. Introduzco como variable la fecha de laminacion para pas�rsela a las etiquetas
        {
            _tipoPasaporte = tipoPasaporte;
            _idPasaporteIni = idPasaporteIni;
            IdPasaporteEnd = idPasaporteEnd;
            NSerie = idPasaporteIni.Substring(0, tipoPasaporte.NChars);
            _tipoPasaporte.IDCatalogo = ID;
            FechLaminacion = FechaLam;
            //MCR 2011/03/02. Introduzco como variable la fecha de laminacion para pas�rsela a las etiquetas. 
        }

        public string Name
        {
            get { return ToString(); }
        }

        public string ID
        {
            get { return (_tipoPasaporte.Country.Name + _tipoPasaporte.Type + IdPasaporteIni); }
        }

        public string NSerie { get; private set; }
        public string FechLaminacion { get; set; }
        //MCR 2011/03/03. Introduzco como variable la fecha de laminacion para pas�rsela a las etiquetas.


        public TipoPasaporte TipoPasaporte
        {
            get { return _tipoPasaporte; }
            set { _tipoPasaporte = value; }
        }

        public string IdPasaporteIni
        {
            get { return _idPasaporteIni; }
            set { _idPasaporteIni = value; }
        }

        public string IdPasaporteEnd
        {
            get { return _idPasaporteEnd; }
            set { _idPasaporteEnd = value; }
        }

        public int NPasaportes()
        {
            return NPasaportes(_idPasaporteIni, _idPasaporteEnd, TipoPasaporte);
        }

        public int NGrupos()
        {
            return NPasaportes()/GrupoPasaportes.NMaximo;
        }

        public int NCajas()
        {
            return NPasaportes()/CajaPasaportes.NMaxPasaportes;
        }

        public static int NPasaportes(string parIdPasaporteIni, string parIdPasaporteEnd, TipoPasaporte parTipo)
        {
            return (Pasaporte.GetNPasaporte(parIdPasaporteEnd, parTipo) -
                    Pasaporte.GetNPasaporte(parIdPasaporteIni, parTipo) + 1);
        }

        public static bool IsIdPasaporteIniCorrect(out string errorMessage, string parIdPasaporteIni,
                                                   TipoPasaporte parTipo)
        {
            bool value = false;
            errorMessage = "";
            if ((value = Pasaporte.IsIdValid(parIdPasaporteIni, parTipo)) == false)
                errorMessage = string.Format("El pasaporte incial de cat�logo: {0} no es v�lido", parIdPasaporteIni);
            else if (((Pasaporte.GetNPasaporte(parIdPasaporteIni, parTipo) - 1)%100) != 0)
            {
                errorMessage =
                    string.Format(
                        "El pasaporte incial de cat�logo: {0} debe tener n�mero de pasporte finalizado en 01",
                        parIdPasaporteIni);
                value = false;
            }

            return value;
        }

        public static bool IsIdPasaporteEndCorrect(out string errorMessage, string parIdPasaporteEnd,
                                                   string parIdPasaporteIni, TipoPasaporte parTipo)
        {
            bool value = false;
            errorMessage = "";
            if ((value = Pasaporte.IsIdValid(parIdPasaporteEnd, parTipo)) == false)
                errorMessage = string.Format("El n�mero de pasaporte final de cat�logo: {0} no es v�lido",
                                             parIdPasaporteEnd);
            else if (parIdPasaporteEnd.Substring(parTipo.NChars) != parIdPasaporteIni.Substring(parTipo.NChars))
            {
                errorMessage = "Los n�meros de pasaporte inicial y final deben tener las mismas letras";
            }
            else if ((Pasaporte.GetNPasaporte(parIdPasaporteEnd, parTipo)%100) != 0)
            {
                errorMessage =
                    string.Format("El pasaporte final de cat�logo: {0} debe tener n�mero de pasporte finalizado en 00",
                                  parIdPasaporteEnd);
                value = false;
            }

            return value;
        }

        public static bool IsNPasaportesCorrect(out string errorMessage, string parIdPasaporteIni,
                                                string parIdPasaporteEnd, TipoPasaporte parTipo)
        {
            bool value = true;
            errorMessage = "";
            int n = NPasaportes(parIdPasaporteIni, parIdPasaporteEnd, parTipo);
            if (n < 0)
            {
                errorMessage = "El pasaporte final tiene una numeraci�n mayor que el inicial";
                value = false;
            }
            else if (n == 0)
            {
                errorMessage = "El pasaporte inicial tiene la misma numeraci�n que el final";
                value = false;
            }
            else if ((n%100) != 0)
            {
                errorMessage = "El n�mero de pasaportes del cat�logo ha de ser m�ltiplo de 100";
                value = false;
            }

            return value;
        }


        public static DatosCatalogo TryLoad(string file)
        {
            DatosCatalogo catalog = null;
            if (File.Exists(file))
            {
                using (var readFile = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    //try
                    //{                    
                    var BFormatter = new BinaryFormatter();
                    catalog = (DatosCatalogo) BFormatter.Deserialize(readFile);

                    //}
                    //catch {

                    //}
                }
            }


            return catalog;
        }


        public bool TrySave(string file)
        {
            bool well = true;


            using (var writeFile = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    var BFormatter = new BinaryFormatter();
                    BFormatter.Serialize(writeFile, this);
                }
                catch (Exception ex)
                {
                    well = false;
                }
            }
            return well;
        }

        public override string ToString()
        {
            return String.Format("{0}__{1}_{2}", _tipoPasaporte.Country.Name, _idPasaporteIni, _idPasaporteEnd);
        }
    }
}