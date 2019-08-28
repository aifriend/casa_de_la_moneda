using System;
using System.Collections;
using System.Collections.Generic;
using RCMCommonTypes;

namespace RECatalogManager
{
    [Serializable]
    public class GrupoPasaportes : IEnumerable<Pasaporte>
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
        public const int NMaximo = 25; //

        public const int NMaximoDelgado = 50;
        public const int NMaximoGrueso = 25;
        private const int nMinimoCogidaGantry = 18;

        #region Propiedades

        private readonly List<Pasaporte> pasaportes;
        private string id;
        public TipoPasaporte TipoPasaporte { get; set; }
        public IDLine IdLine { get; set; }
        public int CatalogIndex { get; set; }

        public string Id
        {
            get { return id; }
            set
            {
                id = value;

                if (IsIdValid(id, TipoPasaporte))
                {
                    NSerie = GetNSerie(id);
                    NGrupo = GetNGrupo(id);
                    IdCaja = GetIdCaja();
                }
            }
        }

        public string NSerie { get; private set; }
        public int NGrupo { get; private set; }
        public string IdCaja { get; private set; }
        public string fechaLam { get; set; } //MCR. 2011/03/03.

        public bool Fajado { get; set; }
        public double Peso { get; set; }
        public bool LastOfBox { get; set; }
        public bool Transferido { set; get; }
        public Incidencias Incidencia { get; private set; }

        public string IdPlus1
        {
            get { return GetIdGrupoPlus1(); }
            set { ; }
        }

        #endregion

        #region Constructores

        public GrupoPasaportes()
        {
            pasaportes = new List<Pasaporte>();
        }

        public GrupoPasaportes(Pasaporte p)
        {
            pasaportes = new List<Pasaporte>();
            TipoPasaporte = p.Tipo;
            Id = p.IdGrupo;
        }

        //sobrecarga utilizada para recibir el grupo de pasaportes externo de la linea 1 (Unomatic)
        public GrupoPasaportes(GroupResume g)
        {
            TipoPasaporte = g.TipoPasaporte;
            //this.TipoPasaporte.NombrePasaporte=
            Id = g.Id;
            Peso = g.Weight;
            LastOfBox = g.LastOfBox;
            pasaportes = new List<Pasaporte>();
            //this.NMaximo = g.TipoPasaporte.Thickness == TipoPasaporte.Thicknesses.Delgado50Pasaportes ? NMaximoDelgado : NMaximoGrueso;//2010-12-10 
            foreach (string str in IdsPasaportes(g.PassportsNumber))
                pasaportes.Add(new Pasaporte(TipoPasaporte, str));
        }

        public GrupoPasaportes(GrupoPasaportes g)
        {
            if (g != null)
            {
                TipoPasaporte = g.TipoPasaporte;
                Id = g.Id;
                Fajado = g.Fajado;
                Peso = g.Peso;
                LastOfBox = g.LastOfBox;
                Transferido = g.Transferido;
                Incidencia = g.Incidencia;
                pasaportes = new List<Pasaporte>();
                CatalogIndex = g.CatalogIndex;
                foreach (Pasaporte p in g)
                    pasaportes.Add(new Pasaporte(p));
            }
        }

        public void AddPasaportesGrupo(GrupoPasaportes Gp)
        {
            if (pasaportes.Count == 0 && Gp.NPasaportes() > 0)
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
                pasaportes.Add(Gp[i]);
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
            //if (TipoPasaporte == TipoPasaporte.Español)//MDG.2010-12-12
            //{
            //MDG.2010-07-14.Nuevo calculo ID caja
            string NSerie = GetNSerie(id);
            int NGrupo = GetNGrupo(id);
            string IdGrupoAux = (NGrupo + 100).ToString().PadLeft(TipoPasaporte.NDigits, '0');
            string IdCajaAux = ((NSerie + IdGrupoAux).Substring(0, id.Length - 2) + "00");
            return IdCajaAux; //MDG.2010-07-14
            //}
            //else
            //{
            //    return (id.Substring(0,id.Length-2) + "00");//MDG.2010-07-14.OLD
            //}
        }

        private string GetIdGrupoPlus1()
        {
            //MDG.2010-07-14. ID grupo para mostrar en pantalla. e.g.: ...01,26,51,76
            string NSerie = GetNSerie(id);
            int NGrupo = GetNGrupo(id);
            string IdGrupoAux = (NGrupo + 1).ToString().PadLeft(TipoPasaporte.NDigits, '0');
            string IdGrupoPlus1 = NSerie + IdGrupoAux;
            return IdGrupoPlus1;
        }

        #endregion

        #region Manejo de pasaportes

        public Pasaporte this[int index]
        {
            get { return pasaportes[index]; }
            set { pasaportes[index] = value; }
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
            pasaportes.Add(p);
        }

        public void InsertPassPort(int pos, Pasaporte p)
        {
            pasaportes.Insert(pos, p);
        }


        public int NPasaportes()
        {
            return pasaportes.Count;
        }

        public Pasaporte GetPasaporte(string idPasaporte)
        {
            Pasaporte value = null;
            foreach (Pasaporte pasaporte in pasaportes)
            {
                if (pasaporte.Id == idPasaporte)
                {
                    value = pasaporte;
                    break;
                }
            }
            return value;
        }

        public bool IsContainerOf(Pasaporte p)
        {
            bool value = false;
            if (Id == p.IdGrupo)
                value = true;

            return value;
        }

        public bool Existe(Pasaporte p)
        {
            bool value = false;
            foreach (Pasaporte pasaporte in pasaportes)
            {
                if (pasaporte.Id == p.Id)
                {
                    value = true;
                    break;
                }
            }
            return value;
        }

        public bool Existe(string IdPasaporte)
        {
            bool value = false;
            foreach (Pasaporte pasaporte in pasaportes)
            {
                if (pasaporte.Id == IdPasaporte)
                {
                    value = true;
                    break;
                }
            }
            return value;
        }

        public int Position(Pasaporte p)
        {
            int pos = -1;

            if (IsContainerOf(p))
                pos = p.NPasaporte - NGrupo - 1;

            return pos;
        }

        public List<string> IdsPasaportes(int nPasaportes)
        {
            var values = new List<string>();

            for (int i = 1; i <= nPasaportes; i++)
            {
                values.Add(NSerie + (NGrupo + i).ToString().PadLeft(TipoPasaporte.NDigits, '0'));
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
                    int pos = 0;
                    for (int i = 0; i < NMaximo; i++)
                    {
                        if (pasaportes[i] != null)
                        {
                            pos = pasaportes[i].NPasaporte - NGrupo - 1;
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

                    pasaportes.Insert(posAux, p);
                    value = true;
                }
            }
            return value;
        }


        public void Remove(string idPasaporte)
        {
            for (int i = 0; i <= pasaportes.Count - 1; i++)
            {
                if (this[i].Id == idPasaporte)
                {
                    pasaportes.RemoveAt(i);
                    break;
                }
            }
        }

        public void Remove(Pasaporte Pasaporte)
        {
            for (int i = 0; i <= pasaportes.Count - 1; i++)
            {
                if (this[i].Id == Pasaporte.Id)
                {
                    pasaportes.RemoveAt(i);
                    break;
                }
            }
        }

        public void RemoveAt(int index)
        {
            pasaportes.RemoveAt(index);
        }

        public GrupoPasaportes ClearRfi2Values()
        {
            foreach (Pasaporte p in pasaportes)
            {
                p.Rfid2 = "";
            }
            return this;
        }

        #endregion

        #region Métodos de acceso

        public bool Empty()
        {
            bool value = false;
            if (pasaportes.Count == 0)
            {
                value = true;
            }
            return value;
        }

        public bool Completo()
        {
            bool Value = false;
            if (pasaportes.Count == NMaximo)
            {
                Value = true;
            }
            return Value;
        }

        public bool NMinimoCogidaGantry()
        {
            return (pasaportes.Count >= nMinimoCogidaGantry);
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
                else
                    return (NMaximoDelgado*TipoPasaporte.Weight);
            }
            else
            {
                return 0; //peso nominal es 0, por lo que rechazará la caja y la revisaran
            }
        }

        #endregion

        #region Comprobaciones de formato

        public bool IsIdValid()
        {
            return IsIdValid(id, TipoPasaporte);
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
                    else if (!(nSerie%25 == 0))
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
            foreach (Pasaporte p in pasaportes)
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
            foreach (Pasaporte p in pasaportes)
                if (!p.IsIdValid())
                    value = false;

            return value;
        }

        private bool AreRfid1Correct()
        {
            bool value = true;
            foreach (Pasaporte p in pasaportes)
                if (!p.IsRfid1Valid())
                    value = false;

            return value;
        }

        private bool AreRfid2Correct()
        {
            bool value = true;
            foreach (Pasaporte p in pasaportes)
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
                msg = "Grupo pasaportes con Id=" + Id + " tiene: " + pasaportes.Count + " pasaportes";

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
            return pasaportes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return pasaportes.GetEnumerator();
        }

        #endregion

        #region Métodos experimentales

        public void Clear()
        {
            TipoPasaporte = null;
            Id = null;
            NSerie = null;
            NGrupo = 0;
            pasaportes.Clear();
            Fajado = false;
            Peso = 0.0;
            Transferido = false;
            Incidencia = Incidencias.None;
        }


        public static GrupoPasaportes NewSoftCopy(GrupoPasaportes g)
        {
            var gCopy = new GrupoPasaportes();
            ;
            if (g != null)
            {
                gCopy.TipoPasaporte = g.TipoPasaporte;
                gCopy.Id = g.Id;
                gCopy.Fajado = g.Fajado;
                gCopy.Peso = g.Peso;
                gCopy.Transferido = g.Transferido;
                gCopy.Incidencia = g.Incidencia;
                gCopy.pasaportes.Clear();
                foreach (Pasaporte p in g)
                    gCopy.pasaportes.Add(p);
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
                pasaportes.Clear();
                foreach (Pasaporte p in g)
                    pasaportes.Add(p);
            }
        }

        #endregion
    }
}