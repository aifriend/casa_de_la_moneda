using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RCMCommonTypes;

namespace RECatalogManager
{
    [Serializable]
    public class CajaPasaportes : IEnumerable<GrupoPasaportes>
    {
        public const int NGrupos = 4;
        public const int NMaxPasaportes = NGrupos*GrupoPasaportes.NMaximo;
        private readonly GrupoPasaportes[] grupos;
        private readonly TipoPasaporte tipoPasaporte;
        private string id;

        #region cosas de peso

        public double Peso { get; set; }

        public bool IsCorrectWeight()
        {
            GrupoPasaportes g = grupos.FirstOrDefault(_ => _ != null);
            if (g == null)
                return false;

            //MDG.2010-11-26. Comprobamos que la diferencia del peso medido y el nominal de la caja es menor que el 50% de un grupo
            //Ejemplo: Medido=3560. NominalCaja =3500. Diferencia=60. DiferenciaAdmisible=5Pasaportes=5x35=175gramos
            double PesoMedido = Peso; //ej=3560
            int PesoCartonCaja = 60; //60 gramos, hemos medido 86
            double PesoNominalCaja = (g.PesoNominal()*4) + PesoCartonCaja;
            //ej:3560//Peso caja =Pesogrupo*4// PesoNominal();//ej=3500
            double DiferenciaPesoAdmisible = ((g.PesoNominal()*5)/25); //ej=175
            double DiferenciaPesoMedida = Math.Abs(PesoMedido - PesoNominalCaja); //
            bool Correcto = DiferenciaPesoMedida <= DiferenciaPesoAdmisible;
            return Correcto;
            ////////////

            //OLD.MDG.2010-11-26//return ((Math.Abs(Peso - (PesoNominal())) <= 0.8 *(g.PesoNominal())));
        }

        public bool Pesado()
        {
            return (Peso != 0) ? true : false;
        }

        private double PesoNominal()
        {
            double value = 0.0;
            foreach (GrupoPasaportes grupo in grupos)
                if (grupo != null) value += grupo.PesoNominal();
            return value;
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
            tipoPasaporte = tipo;
            Id = id;
            grupos = new GrupoPasaportes[4];
        }

        public CajaPasaportes(GrupoPasaportes group)
        {
            tipoPasaporte = group.TipoPasaporte;
            Id = group.IdCaja;
            IdLine = group.IdLine;
            grupos = new GrupoPasaportes[4];
            fechaLam = group.fechaLam; //MCR. 2011/03/03.
        }

        //MCR.2011-07-12.Para programa impresion etiquetas
        public CajaPasaportes(string idActual, TipoPasaporte type, string fechaLaminacion)
        {
            tipoPasaporte = type;
            Id = idActual;
            fechaLam = fechaLaminacion;
        }

        public IDLine IdLine { get; set; }
        public string NSerie { get; private set; }
        public int NCaja { get; private set; }

        public string CodigoBarras { get; set; }
        public string CodigoBarrasLeido { get; set; }
        public int CatalogIndex { get; set; }
        public string fechaLam { get; set; } //MCR. 2011/03/03.

        public bool Etiquetada
        {
            get { return !String.IsNullOrEmpty(CodigoBarras); }
        }

        public bool EtiquetaDuplicada { get; set; } //MDG.2011-06-16

        public TipoPasaporte TipoPasaporte
        {
            get { return tipoPasaporte; }
        }

        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                NSerie = GetNSerie(id);
                NCaja = GetNCaja(id);
            }
        }

        public IList<GrupoPasaportes> Grupos
        {
            get { return grupos; }
        }

        #region IEnumerable<GrupoPasaportes> Members

        public IEnumerator<GrupoPasaportes> GetEnumerator()
        {
            for (int i = 0; i < NGrupos; i++)
            {
                if (grupos[i] != null)
                {
                    yield return grupos[i];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

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
                grupos[index] = g;
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
    }
}