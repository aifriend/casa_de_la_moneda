using System;
using System.Collections.Generic;
using RCMCommonTypes;

namespace Idpsa
{
    [Serializable]
    public class Pasaporte : IDBMappeable
    {
        #region Incidencias enum

        [Flags]
        public enum Incidencias
        {
            None = 0,
            Id = 1,
            Rfid1 = 2,
            Rfid2 = 4,
            All = Id | Rfid1 | Rfid2
        }

        #endregion

        #region Propiedades

        private string id;

        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                if (IsIdValid(id, Tipo))
                {
                    NSerie = GetNSerie(id, Tipo);
                    NPasaporte = GetNPasaporte(id, Tipo);
                    IdGrupo = GetIdGrupo(id, Tipo);
                }
            }
        }

        public TipoPasaporte Tipo { get; private set; }
        public bool IdLeido { get; set; }
        public string NSerie { get; private set; }
        public int NPasaporte { get; private set; }
        public string IdGrupo { get; private set; }
        public string Rfid1 { get; set; }
        public string Rfid2 { get; set; }
        public bool CierraGrupo { get; set; }

        #endregion

        #region Constructores

        public Pasaporte(TipoPasaporte tipo, string id)
        {
            IdLeido = true;
            Tipo = tipo;
            Id = id;
            Rfid1 = "";
            Rfid2 = "";
        }


        public Pasaporte(Pasaporte p)
        {
            Tipo = p.Tipo;
            Id = p.id;
            Rfid1 = p.Rfid1;
            Rfid2 = p.Rfid2;
            CierraGrupo = p.CierraGrupo;
        }

        #endregion

        #region Determinación de prodiedades a través del Id

        public static string GetNSerie(string idPasaporte, TipoPasaporte parTipo)
        {
            string s = idPasaporte.Substring(0, parTipo.NChars);
            if (s == null) s = "";
            return s;
        }

        public static int GetNPasaporte(string idPasaporte, TipoPasaporte parTipo)
        {
            return int.Parse(idPasaporte.Substring(parTipo.NChars));
        }

        public static string GetIdGrupo(string idPasaporte, TipoPasaporte parTipo)
        {
            string aux = (GetNPasaporte(idPasaporte, parTipo) - 1).ToString().PadLeft(parTipo.NDigits, '0');
            aux = GetNSerie(idPasaporte, parTipo) + aux;
            int value = GrupoPasaportes.NMaximo*
                        (int)
                        Math.Floor(((double) (int.Parse((aux.Substring(parTipo.Length - 2))))/GrupoPasaportes.NMaximo));
            return (aux.Substring(0, parTipo.Length - 2) + value.ToString().PadLeft(2, '0'));
        }

        public static string GetId(string NSerie, int NPasaporte, TipoPasaporte Tipo)
        {
            return (NSerie + NPasaporte.ToString().PadLeft(Tipo.NDigits, '0'));
        }

        #endregion

        #region Comprobaciones de formato

        public static bool IsIdValid(string id, TipoPasaporte parTipo)
        {
            bool value = true;
            int nSerie;


            if (id.Length == parTipo.Length)
            {
                for (int i = 0; i < parTipo.NChars; i++)
                {
                    if (!Char.IsLetterOrDigit(id[i]))
                    {
                        value = false;
                        break;
                    }
                }

                if (!int.TryParse(id.Substring(parTipo.NChars), out nSerie))
                    value = false;
            }
            else
                value = false;


            return value;
        }

        public bool IsIdValid()
        {
            return IsIdValid(id, Tipo);
        }

        public static bool IsRfidValid(string rfid)
        {
            return true;
        }

        public bool IsRfid1Valid()
        {
            return IsRfidValid(Rfid1);
        }

        public bool IsRfid2Valid()
        {
            return IsRfidValid(Rfid2);
        }

        #endregion

        #region Obtención de incidencias y mensajes de error

        public Dictionary<Incidencias, String> GetIncidences(Incidencias flag)
        {
            Type t = GetType();
            var L = new Dictionary<Incidencias, string>();
            foreach (Incidencias valor in Enum.GetValues(typeof (Incidencias)))
                if (valor != Incidencias.None || valor != Incidencias.All)
                    if ((flag & valor) != 0)
                    {
                        var strError =
                            (string) t.GetMethod(Enum.GetName(typeof (Incidencias), valor) + "Error").Invoke(this, null);
                        if (strError.Trim() != "")
                            L.Add(valor, strError);
                    }
            return L;
        }

        private string IdError()
        {
            int nSerie;
            string msg = "";

            if (id.Length == Tipo.Length)
            {
                for (int i = 0; i < Tipo.NChars; i++)
                {
                    if (!Char.IsLetterOrDigit(id[i]))
                    {
                        msg += "Número serie: " + id.Substring(0, Tipo.NChars) + "incorrecto";
                        return msg;
                    }
                }


                if (!int.TryParse(id.Substring(Tipo.NChars), out nSerie))
                    msg = ", Número pasaporte" + id.Substring(Tipo.NChars) + "incorrecto";
            }
            else
            {
                msg = "Pasaporte incorrectamente leído, lectura: " + id;
            }


            return msg;
        }

        private string Rfid1Error()
        {
            string msg = "";
            if (!IsRfid1Valid())
                msg = "Pasaporte con Id =" + id + " se ha leído Rfid =" + Rfid1 + " en el primer puesto de lectura";

            return msg;
        }

        private string Rfid2Error()
        {
            string msg = "";
            if (!IsRfid2Valid())
                msg = "Pasaporte con Id =" + id + " se ha leído Rfid =" + Rfid2 + " en el segundo puesto de lectura";

            return msg;
        }

        #endregion

        #region BDMapper

        #endregion

        #region Igualdad

        public static bool operator ==(Pasaporte p1, Pasaporte p2)
        {
            if (Equals(p1, null) && Equals(p2, null))
                return true;
            else if (Equals(p1, null) ^ Equals(p2, null))
                return false;
            else
                return (p1.Id == p2.Id);
        }

        public static bool operator !=(Pasaporte p1, Pasaporte p2)
        {
            return !(p1 == p2);
        }

        public bool Esquals(Pasaporte p)
        {
            bool value;
            if (!(p == null))
            {
                if (p.GetType() == typeof (Pasaporte))
                {
                    Pasaporte pas = p;
                    if (p.NPasaporte == NPasaporte)
                    {
                        value = true;
                    }
                    else
                    {
                        value = false;
                    }
                }
                else
                {
                    value = false;
                }
            }
            else
            {
                value = false;
            }
            return value;
        }

        #endregion

        #region TipoGiro enum

        public enum TipoGiro
        {
            Cero,
            Noventa,
            CientoOchenta,
            MenosNoventa
        }

        #endregion

        #region IDBMappeable Members

        public DBEntity GetDBMapper()
        {
            return new BDPasaporte {ID = Id, IDCatalogo = Tipo.IDCatalogo, IDGrupo = IdGrupo, RfID = Rfid1};
        }

        public DBAction DBAction { get; set; }

        #endregion
    }
}