using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using RCMCommonTypes;

namespace Idpsa
{
    [Serializable]
    public class DatosCatalogo : IDBMappeable
    {
        private static Dictionary<string, Pasaporte> _pasaportes;
        private string _idPasaporteEnd;
        private string _idPasaporteIni;
        private TipoPasaporte _tipoPasaporte;

        public DatosCatalogo(TipoPasaporte tipoPasaporte, string idPasaporteIni, string idPasaporteEnd, string FechaLam)
            //MCR 2011/03/02. Introduzco como variable la fecha de laminacion para pasársela a las etiquetas
        {
            _tipoPasaporte = tipoPasaporte;
            _idPasaporteIni = idPasaporteIni;
            IdPasaporteEnd = idPasaporteEnd;
            NSerie = idPasaporteIni.Substring(0, tipoPasaporte.NChars);
            _tipoPasaporte.IDCatalogo = ID;
            FechLaminacion = FechaLam;
            //MCR 2011/03/02. Introduzco como variable la fecha de laminacion para pasársela a las etiquetas. 
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
        //MCR 2011/03/03. Introduzco como variable la fecha de laminacion para pasársela a las etiquetas.


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

        #region IDBMappeable Members

        public DBEntity GetDBMapper()
        {
            return new BDDatosCatalogo
                       {
                           ID = ID,
                           Nacionalidad = _tipoPasaporte.Country.Name,
                           TipoPasaporte = _tipoPasaporte.Type.ToString(),
                           TipoRfid = _tipoPasaporte.RfIdType.ToString(),
                           PasaporteInicial = IdPasaporteIni,
                           NumeroPasaportes = NPasaportes(),
                           PesoPasaporte = (int) _tipoPasaporte.Weight,
                           FechaInicial = (DBAction == DBAction.Insert) ? DateTime.Now : (DateTime?) null,
                           FechaFinal = (DBAction == DBAction.Udpade) ? DateTime.Now : (DateTime?) null
                       };
        }

        public DBAction DBAction { get; set; }

        #endregion

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
                errorMessage = string.Format("El pasaporte incial de catálogo: {0} no es válido", parIdPasaporteIni);
            else if (((Pasaporte.GetNPasaporte(parIdPasaporteIni, parTipo) - 1)%100) != 0)
            {
                errorMessage =
                    string.Format(
                        "El pasaporte incial de catálogo: {0} debe tener número de pasporte finalizado en 01",
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
            String ini = parIdPasaporteIni.Substring(0,parTipo.NChars);
            String end = parIdPasaporteEnd.Substring(0,parTipo.NChars);
            if ((value = Pasaporte.IsIdValid(parIdPasaporteEnd, parTipo)) == false)
                errorMessage = string.Format("El número de pasaporte final de catálogo: {0} no es válido",
                                             parIdPasaporteEnd);
            else if (!end.Equals(ini))
            {
                errorMessage = "Los números de pasaporte inicial y final deben tener las mismas letras";
                value = false;
            }
            else if ((Pasaporte.GetNPasaporte(parIdPasaporteEnd, parTipo)%100) != 0)
            {
                errorMessage =
                    string.Format("El pasaporte final de catálogo: {0} debe tener número de pasporte finalizado en 00",
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
                errorMessage = "El pasaporte final tiene una numeración mayor que el inicial";
                value = false;
            }
            else if (n == 0)
            {
                errorMessage = "El pasaporte inicial tiene la misma numeración que el final";
                value = false;
            }
            else if ((n%100) != 0)
            {
                errorMessage = "El número de pasaportes del catálogo ha de ser múltiplo de 100";
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

        public static bool ValidarTipoyN(string idPasaporteIni, TipoPasaporte parTipo)//MCR. 2016. Modificación destinatario.
        {
            var a = parTipo.Type.ToString();
            var b = TipoN(idPasaporteIni, parTipo).ToString();
            return (parTipo.Type.ToString().Equals(TipoN(idPasaporteIni, parTipo).ToString()));
        }
        private static TipoPasaporte.Types TipoN(string idPasaporteIni, TipoPasaporte parTipo)//MCR. 2016. Modificación destinatario.
        {
            if ((idPasaporteIni.StartsWith("P") && parTipo.NChars == 3))
                return (TipoPasaporte.Types.Normal);
            else if (idPasaporteIni.Contains("ZAB"))
                return TipoPasaporte.Types.Normal;
            else if (idPasaporteIni.Contains("DVI"))
                return TipoPasaporte.Types.TituloViaje;
            else if (idPasaporteIni.Contains("ZVI"))
                return TipoPasaporte.Types.TituloViaje;
            else if (idPasaporteIni.Contains("DVR"))
                return TipoPasaporte.Types.Refugiados;
            else if (idPasaporteIni.Contains("ZVR"))
                return TipoPasaporte.Types.Refugiados;
            else if (idPasaporteIni.Contains("DVA"))
                return TipoPasaporte.Types.Apatridas;
            else if (idPasaporteIni.Contains("ZVA"))
                return TipoPasaporte.Types.Apatridas;
            else if (idPasaporteIni.Contains("DVP"))
                return TipoPasaporte.Types.Subsidiario;
            else if (idPasaporteIni.Contains("ZVP"))
                return TipoPasaporte.Types.Subsidiario;
            else if (idPasaporteIni.Contains(" LM") || idPasaporteIni.Substring(0, parTipo.NChars).Equals("LM"))
                return TipoPasaporte.Types.Maritimo;
            else if ((idPasaporteIni.StartsWith("XD") && parTipo.NChars == 3))
                return TipoPasaporte.Types.Consular;
            else if (idPasaporteIni.Contains("ZZB"))
                return TipoPasaporte.Types.Consular;
            else if (idPasaporteIni.Contains(" PA") || idPasaporteIni.Substring(0, parTipo.NChars).Equals("PA"))
                return TipoPasaporte.Types.Provisional;
            else if (idPasaporteIni.Substring(0, 2).Equals("XF"))
                return TipoPasaporte.Types.Diplomatico;
            else if (idPasaporteIni.Substring(0, 2).Equals("XG"))
                return TipoPasaporte.Types.Servicio;
            return TipoPasaporte.Types.NotDefined;
        }

    }
}