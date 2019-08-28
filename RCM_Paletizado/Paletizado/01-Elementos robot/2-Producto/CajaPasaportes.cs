using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Paletizado;
using RCMCommonTypes;

namespace Idpsa
{
    [Serializable]
    public class CajaPasaportes : IEnumerable<GrupoPasaportes>, IPaletElement, IDBMappeable
    {
        public const int NGrupos = 4;
        public const int NMaxPasaportes = NGrupos*GrupoPasaportes.NMaximo;
        private readonly GrupoPasaportes[] _grupos;
        private readonly TipoPasaporte _tipoPasaporte;
        private string _id;
        private bool ManualValidated; //MCR. 2015.03.12

        #region PESO

        public double Peso { private get; set; }

        public bool IsCorrectWeight()
        {
            GrupoPasaportes g = _grupos.FirstOrDefault(_ => _ != null);
            if (g == null)
                return false;
            if (ManualValidated)
                return true;
            //MDG.2010-11-26. Comprobamos que la diferencia del peso medido y el nominal de la caja es menor que el 50% de un grupo
            //Ejemplo: Medido=3560. NominalCaja =3500. Diferencia=60. DiferenciaAdmisible=5Pasaportes=5x35=175gramos
            double pesoMedido = Peso; //ej=3560
            const int PesoCartonCaja = 60; //60 gramos, hemos medido 86
            double pesoNominalCaja = (g.PesoNominal()*4) + PesoCartonCaja;
            //ej:3560//Peso caja =Pesogrupo*4// PesoNominal();//ej=3500
            //double DiferenciaPesoAdmisible = ((g.PesoNominal() * 5) / 25); //ej=175
            double diferenciaPesoAdmisible = ((g.PesoNominal()*13)/25); //MDG.2012-06-28
            double diferenciaPesoMedida = Math.Abs(pesoMedido - pesoNominalCaja); //
            bool correcto = diferenciaPesoMedida <= diferenciaPesoAdmisible;
            return correcto;
            ////////////

            //OLD.MDG.2010-11-26//return ((Math.Abs(Peso - (PesoNominal())) <= 0.8 *(g.PesoNominal())));
        }

        //MDG.2013-04-04.Comprobamos que se está pesando algo
        public bool IsMinimunWeight()
        {
            return (Peso > 500);
        }

        public bool Pesado()
        {
            return (Peso != 0) ? true : false;
        }

        private double PesoNominal()
        {
            return _grupos.Where(grupo => grupo != null).Sum(grupo => grupo.PesoNominal());
        }

        public string WeightError()
        {
            string msg = "";
            if (!IsCorrectWeight())
                msg = "Paquete con Id=" + Id + " tiene un peso de: " + Peso + " que sale fuera de tolerancia";

            return msg;
        }

        #endregion

        public CajaPasaportes(string id, TipoPasaporte tipo)
        {
            _tipoPasaporte = tipo;
            Id = id;
            _grupos = new GrupoPasaportes[4];
            ManualValidated = false;//MCR
        }

        public CajaPasaportes(GrupoPasaportes group)
        {
            _tipoPasaporte = group.TipoPasaporte;
            Id = group.IdCaja;
            IdLine = group.IdLine;
            _grupos = new GrupoPasaportes[4];
            fechaLam = group.FechaLam;
            ManualValidated = false;//MCR
        }

        //MCR.2011-07-12.Para programa impresion etiquetas
        public CajaPasaportes(string idActual, TipoPasaporte type, string fechaLaminacion)
        {
            _tipoPasaporte = type;
            Id = idActual;
            fechaLam = fechaLaminacion;
            ManualValidated = false;//MCR
        }

        public IDLine IdLine { get; set; }
        public string NSerie { get; private set; }
        public int NCaja { get; private set; }

        public string CodigoBarras { get; set; }
        public string CodigoBarrasLeido { get; set; }
        public int CatalogIndex { get; set; }
        public string fechaLam { get; set; }

        public bool Etiquetada
        {
            get { return !String.IsNullOrEmpty(CodigoBarras); }
        }

        public bool EtiquetaDuplicada { get; set; }

        public TipoPasaporte TipoPasaporte
        {
            get { return _tipoPasaporte; }
        }

        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                NSerie = GetNSerie(_id);
                NCaja = GetNCaja(_id);
            }
        }

        public IList<GrupoPasaportes> Grupos
        {
            get { return _grupos; }
        }

        #region IDBMappeable Members

        public DBEntity GetDBMapper()
        {
            var value = new BDCajaPasaportes
                            {
                                ID = Id,
                                IDCatalogo = _tipoPasaporte.IDCatalogo,
                                CodigoBarras = CodigoBarras,
                                FechaInicial = (DBAction == DBAction.Insert) ? DateTime.Now : (DateTime?) null,
                                FechaFinal = (DBAction == DBAction.Udpade) ? DateTime.Now : (DateTime?) null
                            };

            return value;
        }

        public DBAction DBAction { get; set; }

        #endregion

        #region IEnumerable<GrupoPasaportes> Members

        public IEnumerator<GrupoPasaportes> GetEnumerator()
        {
            for (int i = 0; i < NGrupos; i++)
            {
                if (_grupos[i] != null)
                {
                    yield return _grupos[i];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IPaletElement Members

        public ElementTypes GeneralType
        {
            get { return ElementTypes.Item; }
        }

        public string IDPalet { get; set; }
        public int Flat { get; set; }
        public int Pos { get; set; }

        #endregion

        public bool IsBarcodeReadedCorrect()
        {
            if (!Etiquetada) return false;
            if (CodigoBarras == CodigoBarrasLeido)
                return true;
            return false;
        }

        //MDG.2011-06-16
        public bool IsIdNotRepeated()
        {
            if (EtiquetaDuplicada)
                return false;
            else
                return true;
        }

        public void Add(GrupoPasaportes g, int index)
        {
            if (index < NGrupos - 1)
            {
                _grupos[index] = g;
            }
        }

        private string GetNSerie(string id)
        {
            return id.Substring(0, TipoPasaporte.NChars);
        }

        private int GetNCaja(string id)
        {
            return int.Parse(id.Substring(TipoPasaporte.NChars));
        }


        public void ValidacionManual()
        {
            ManualValidated = true; //MCR.
        }
    }
}