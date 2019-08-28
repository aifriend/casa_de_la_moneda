using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Globalization;
using System.Linq;
using RCMCommonTypes;

namespace Idpsa
{
    [Serializable]
    public class GrupoPasaportes : IEnumerable<Pasaporte>, IDBMappeable
    {
        #region Casilleros enum

        [Flags]
        public enum Casilleros
        {
            Left,
            Right,
        }

        #endregion

        #region Incidencias enum

        /// <summary>
        /// Incidencias asociadas a 
        /// un grupo de pasaportes
        /// </summary>
        public enum Incidencias
        {
            None = 0,
            IdPasaporte = 1,
            Weight = 2,
            Order = 4,
            OrderInverted = 8,
            NPasaportes = 16,
            Rfid1 = 32,
            Rfid2 = 64,
            AllError = Weight | Order | OrderInverted | NPasaportes | Rfid1 | Rfid2,
        }

        #endregion

        public const int NContrapeados = 5;
        public const int NMaximo = 25;

        private const int NMaximoDelgado = 50;
        private const int NMaximoGrueso = 25;
        private const int nMinimoCogidaGantry = 18;

        #region Propiedades

        private readonly List<Pasaporte> _pasaportes;
        private string _id;
        public TipoPasaporte TipoPasaporte { get; private set; }
        public IDLine IdLine { get; set; }
        public int CatalogIndex { private get; set; }

        public string Id
        {
            get { return _id; }
            private set
            {
                _id = value;
                if (!IsIdValid(_id, TipoPasaporte)) return;
                NSerie = GetNSerie(_id);
                NGrupo = GetNGrupo(_id);
                IdCaja = GetIdCaja();
            }
        }

        public string NSerie { get; private set; }
        public int NGrupo { get; private set; }
        public string IdCaja { get; private set; }
        public string FechaLam { get; set; }

        public bool Fajado { get; set; }
        public double Peso { get; set; }
        public bool LastOfBox { get; set; }
        public bool Transferido { set; get; }
        public Incidencias Incidencia { get; private set; }

        public string IdPlus1
        {
            get { return GetIdGrupoPlus1(); }
        }

        #endregion

        #region Constructores

        public GrupoPasaportes()
        {
            _pasaportes = new List<Pasaporte>();
        }

        public GrupoPasaportes(Pasaporte p)
        {
            _pasaportes = new List<Pasaporte>();
            TipoPasaporte = p.Tipo;
            Id = p.IdGrupo;
        }

        //sobrecarga utilizada para recibir el grupo de pasaportes externo de la linea 1 (Unomatic)
        public GrupoPasaportes(GroupResume g)
        {
            TipoPasaporte = g.TipoPasaporte;
            Id = g.Id;
            Peso = g.Weight;
            LastOfBox = g.LastOfBox;
            _pasaportes = new List<Pasaporte>();
            foreach (string str in IdsPasaportes(g.PassportsNumber))
                _pasaportes.Add(new Pasaporte(TipoPasaporte, str));
        }

        public GrupoPasaportes(GrupoPasaportes g)
        {
            if (g == null) return;
            TipoPasaporte = g.TipoPasaporte;
            Id = g.Id;
            Fajado = g.Fajado;
            Peso = g.Peso;
            LastOfBox = g.LastOfBox;
            Transferido = g.Transferido;
            Incidencia = g.Incidencia;
            _pasaportes = new List<Pasaporte>();
            CatalogIndex = g.CatalogIndex;
            foreach (Pasaporte p in g)
                _pasaportes.Add(new Pasaporte(p));
        }

        public void AddPasaportesGrupo(GrupoPasaportes Gp)
        {
            if (_pasaportes.Count == 0 && Gp.NPasaportes() > 0)
            {
                TipoPasaporte = Gp[0].Tipo;
                Id = Gp[0].IdGrupo;
            }
            for (int i = 0; i < Gp.NPasaportes(); i++)
            {
                if (Gp[i].CierraGrupo)
                {
                    Gp[i].CierraGrupo = false;

                    Transferido = true;
                }
                _pasaportes.Add(Gp[i]);
            }
        }

        #endregion

        #region Determinación de propiedades a través del Id

        private string GetNSerie(string idPasaporte)
        {
            return idPasaporte.Substring(0, TipoPasaporte.NChars);
        }

        private int GetNGrupo(string idPasaporte)
        {
            return int.Parse(idPasaporte.Substring(TipoPasaporte.NChars));
        }

        private string GetIdCaja()
        {
            string idGrupoAux = (GetNGrupo(_id) + 100).ToString(CultureInfo.InvariantCulture).PadLeft(TipoPasaporte.NDigits, '0');
            string idCajaAux = ((GetNSerie(_id) + idGrupoAux).Substring(0, _id.Length - 2) + "00");
            return idCajaAux;
        }

        private string GetIdGrupoPlus1()
        {
            string idGrupoAux = (GetNGrupo(_id) + 1).ToString(CultureInfo.InvariantCulture).PadLeft(TipoPasaporte.NDigits, '0');
            string idGrupoPlus1 = GetNSerie(_id) + idGrupoAux;
            return idGrupoPlus1;
        }

        #endregion

        #region Manejo de pasaportes

        public Pasaporte this[int index]
        {
            get { return _pasaportes[index]; }
        }

        public void CopyIntrinsicInformation(GrupoPasaportes g)
        {
            Clear();
            TipoPasaporte = g.TipoPasaporte;
            Id = g.Id;
            Incidencia = g.Incidencia;
            Fajado = g.Fajado;
            Peso = g.Peso;
            LastOfBox = g.LastOfBox;
        }


        public void CopyPassportIntrinsicInformation(Pasaporte p)
        {
            Clear();
            TipoPasaporte = p.Tipo;
            Id = p.IdGrupo;
        }

        public void Add(Pasaporte p)
        {
            _pasaportes.Add(p);
        }

        public void InsertPassPort(int pos, Pasaporte p)
        {
            _pasaportes.Insert(pos, p);
        }


        public int NPasaportes()
        {
            return _pasaportes.Count;
        }

        public Pasaporte GetPasaporte(string idPasaporte)
        {
            return _pasaportes.FirstOrDefault(pasaporte => pasaporte.Id == idPasaporte);
        }

        public bool IsContainerOf(Pasaporte p)
        {
            bool value = Id == p.IdGrupo;
            return value;
        }

        public bool Existe(Pasaporte p)
        {
            return _pasaportes.Any(pasaporte => pasaporte.Id == p.Id);
        }

        public bool Existe(string idPasaporte)
        {
            return _pasaportes.Any(pasaporte => pasaporte.Id == idPasaporte);
        }

        public int Position(Pasaporte p)
        {
            int pos = -1;

            if (IsContainerOf(p))
                pos = p.NPasaporte - NGrupo - 1;

            return pos;
        }

        public IEnumerable<string> IdsPasaportes(int nPasaportes)
        {
            var values = new List<string>();

            for (int i = 1; i <= nPasaportes; i++)
            {
                values.Add(NSerie + (NGrupo + i).ToString(CultureInfo.InvariantCulture).PadLeft(TipoPasaporte.NDigits, '0'));
            }

            return values;
        }

        public bool InsertPassPort(Pasaporte p)
        {
            bool value = false;
            if (IsContainerOf(p))
            {
                if (!Existe(p))
                {
                    int posAux = p.NPasaporte - NGrupo - 1;
                    for (int i = 0; i < NMaximo; i++)
                    {
                        if (_pasaportes[i] != null)
                        {
                            int pos = _pasaportes[i].NPasaporte - NGrupo - 1;
                            if (posAux < pos)
                            {
                                posAux = i;
                                break;
                            }
                        }
                        else
                        {
                            posAux = i;
                            break;
                        }
                    }

                    _pasaportes.Insert(posAux, p);
                    value = true;
                }
            }
            return value;
        }


        public void Remove(string idPasaporte)
        {
            for (int i = 0; i <= _pasaportes.Count - 1; i++)
            {
                if (this[i].Id == idPasaporte)
                {
                    _pasaportes.RemoveAt(i);
                    break;
                }
            }
        }

        public void Remove(Pasaporte Pasaporte)
        {
            for (int i = 0; i <= _pasaportes.Count - 1; i++)
            {
                if (this[i].Id == Pasaporte.Id)
                {
                    _pasaportes.RemoveAt(i);
                    break;
                }
            }
        }

        public void RemoveAt(int index)
        {
            _pasaportes.RemoveAt(index);
        }

        public GrupoPasaportes ClearRfi2Values()
        {
            foreach (Pasaporte p in _pasaportes)
            {
                p.Rfid2 = "";
            }
            return this;
        }

        #endregion

        #region Métodos de acceso

        public bool Empty()
        {
            bool value = _pasaportes.Count == 0;
            return value;
        }

        public bool Completo()
        {
            bool Value = _pasaportes.Count == NMaximo;
            return Value;
        }

        public bool NMinimoCogidaGantry()
        {
            return (_pasaportes.Count >= nMinimoCogidaGantry);
        }

        public bool Pesado()
        {
            return (Peso != 0) ? true : false;
        }

        public double PesoNominal()
        {
            //return (NMaximo * TipoPasaporte.Weight);
            if (TipoPasaporte != null)
            {
                if (TipoPasaporte.Thickness == TipoPasaporte.Thicknesses.Grueso25Pasaportes)
                    return (NMaximoGrueso*TipoPasaporte.Weight);
                return (NMaximoDelgado*TipoPasaporte.Weight);
            }
            return 0; //peso nominal es 0, por lo que rechazará la caja y la revisaran
        }

        #endregion

        #region Comprobaciones de formato

        public bool IsIdValid()
        {
            return IsIdValid(_id, TipoPasaporte);
        }

        public static bool IsIdValid(string id, TipoPasaporte parTipoPasaporte)
        {
            bool value = true;
            int nSerie;

            if (id != null)
            {
                if (id.Length == parTipoPasaporte.Length)
                {
                    for (int i = 0; i < parTipoPasaporte.NChars; i++)
                    {
                        if (!Char.IsLetterOrDigit(id[i]))
                        {
                            return false;
                        }
                    }


                    if (!int.TryParse(id.Substring(parTipoPasaporte.NChars), out nSerie))
                        value = false;
                    else if (nSerie%25 != 0)
                        value = false;
                }


                else
                    value = false;
            }
            else
                value = false;

            return value;
        }

        public bool IsCorrectWeight()
        {
            return ((Math.Abs(Peso - (PesoNominal())) <= (0.5*TipoPasaporte.Weight)));
        }

        public bool IsOrderInverted()
        {
            bool value = true;
            value = false; //cambiar importante
            //for (int i = 0; i < pasaportes.Count; i++)
            //{
            //    if (pasaportes[i].Rfid1 != pasaportes[pasaportes.Count - (i + 1)].Rfid2)
            //    {
            //        value = false;
            //        break;
            //    }
            //}
            return value;
        }

        public bool IsOrderCorrect()
        {
            bool value = true;
            foreach (Pasaporte p in _pasaportes)
            {
                if (p.Tipo.HasRfid)
                {
                    if (p.Rfid1 != p.Rfid2)
                    {
                        value = false;
                        break;
                    }
                }
            }
            return (value || true);
        }

        private bool AreIdPasaportesFormatsCorrect()
        {
            bool value = true;
            foreach (Pasaporte p in _pasaportes)
                if (!p.IsIdValid())
                    value = false;

            return value;
        }

        private bool AreRfid1Correct()
        {
            bool value = true;
            foreach (Pasaporte p in _pasaportes)
                if (!p.IsRfid1Valid())
                    value = false;

            return value;
        }

        private bool AreRfid2Correct()
        {
            bool value = true;
            foreach (Pasaporte p in _pasaportes)
                if (!p.IsRfid2Valid())
                    value = false;

            return value;
        }

        #endregion

        #region Comprobación de incidencias y mensajes de error

        public Dictionary<Incidencias, String> GetIncidences(Incidencias flag)
        {
            Type t = GetType();
            var L = new Dictionary<Incidencias, string>();
            foreach (Incidencias valor in Enum.GetValues(typeof (Incidencias)))
                if (valor != Incidencias.None && valor != Incidencias.AllError)
                    if ((flag & valor) != 0)
                    {
                        var strError =
                            (string)
                            t.GetMethod(Enum.GetName(typeof (Incidencias), valor) + "Error").Invoke(this, null);
                        if (strError.Trim() != "")
                            L.Add(valor, strError);
                    }
            return L;
        }

        private Dictionary<Incidencias, String> GetIncidences()
        {
            Incidencias flag = Incidencia;
            Type t = GetType();
            var L = new Dictionary<Incidencias, string>();
            foreach (Incidencias valor in Enum.GetValues(typeof (Incidencias)))
                if (valor != Incidencias.None && valor != Incidencias.AllError)
                    if ((flag & valor) != 0)
                    {
                        var strError =
                            (string)
                            t.GetMethod(Enum.GetName(typeof (Incidencias), valor) + "Error").Invoke(this, null);
                        if (strError.Trim() != "")
                            L.Add(valor, strError);
                    }
            return L;
        }

        public string WeightError()
        {
            string msg = "";
            if (!IsCorrectWeight())
                msg = "Paquete con Id=" + Id + " tiene un peso de: " + Peso + " que sale fuera de tolerancia";

            return msg;
        }

        public string NPasaportesError()
        {
            string msg = "";
            if (!Completo())
                msg = "Grupo pasaportes con Id=" + Id + " tiene: " + _pasaportes.Count + " pasaportes";

            return msg;
        }

        public string OrderError()
        {
            string msg = "";
            if (!IsOrderInverted() && !IsOrderCorrect())
                msg = "Grupo pasaportes con Id= " + Id + " tiene pasaportes en desorden";

            return msg;
        }

        public string OrderInvertedError()
        {
            string msg = "";
            if (IsOrderInverted())
                msg = "Grupo pasaportes con Id= " + Id + " no se ha girado";

            return msg;
        }

        public string IdPasaporteError()
        {
            string msg = "";
            if (!AreIdPasaportesFormatsCorrect())
                msg = "Paquete con Id" + Id + " tiene pasaporte(s) con error(es) en la lectura del número de pasaporte";
            return msg;
        }

        public string Rfid1Error()
        {
            string msg = "";
            if (!AreRfid1Correct())
                msg = "Paquete con Id=" + Id + " tiene pasaporte(s) con error(es) rfid en el primer puesto de lectura";
            return msg;
        }

        public string Rfid2Error()
        {
            string msg = "";
            if (!AreRfid2Correct())
                msg = "Paquete con Id=" + Id + " tiene pasaporte(s) con error(es) rfid en el segundo puesto de lectura";
            return msg;
        }

        public bool Reprocesar()
        {
            bool value = false;
            if (NMinimoCogidaGantry() && AreIdPasaportesFormatsCorrect() && AreRfid1Correct())
                value = false;

            return value;
        }

        #endregion

        #region Interfaz IEnumerable

        public IEnumerator<Pasaporte> GetEnumerator()
        {
            return _pasaportes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _pasaportes.GetEnumerator();
        }

        #endregion

        #region Métodos experimentales

        public void Clear()
        {
            TipoPasaporte = null;
            Id = null;
            NSerie = null;
            NGrupo = 0;
            _pasaportes.Clear();
            Fajado = false;
            Peso = 0.0;
            Transferido = false;
            Incidencia = Incidencias.None;
        }


        public static GrupoPasaportes NewSoftCopy(GrupoPasaportes g)
        {
            var gCopy = new GrupoPasaportes();
            if (g != null)
            {
                gCopy.TipoPasaporte = g.TipoPasaporte;
                gCopy.Id = g.Id;
                gCopy.Fajado = g.Fajado;
                gCopy.Peso = g.Peso;
                gCopy.Transferido = g.Transferido;
                gCopy.Incidencia = g.Incidencia;
                gCopy._pasaportes.Clear();
                foreach (Pasaporte p in g)
                    gCopy._pasaportes.Add(p);
            }
            return gCopy;
        }


        public void SoftCopy(GrupoPasaportes g)
        {
            Clear();

            if (g != null)
            {
                TipoPasaporte = g.TipoPasaporte;
                Id = g.Id;
                Fajado = g.Fajado;
                Peso = g.Peso;
                Transferido = g.Transferido;
                Incidencia = g.Incidencia;
                _pasaportes.Clear();
                foreach (Pasaporte p in g)
                    _pasaportes.Add(p);
            }
        }

        #endregion

        #region IDBMappeable Members

        public DBEntity GetDBMapper()
        {
            var value = new BDGrupoPasaportes
                            {
                                ID = Id,
                                IDCatalogo = TipoPasaporte.IDCatalogo,
                                IDCaja = IdCaja,
                                Fajado = Fajado,
                                FechaInicial = (DBAction == DBAction.Insert) ? DateTime.Now : (DateTime?) null,
                                FechaFinal = (DBAction == DBAction.Udpade) ? DateTime.Now : (DateTime?) null
                            };

            value.Pasaportes = new EntitySet<BDPasaporte>();
            foreach (Pasaporte p in _pasaportes)
            {
                value.Pasaportes.Add((BDPasaporte) p.GetDBMapper());
            }

            return value;
        }

        public DBAction DBAction { get; set; }

        #endregion
    }
}