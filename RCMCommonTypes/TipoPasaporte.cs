using System;

namespace RCMCommonTypes
{  
    [Serializable()]
    public class TipoPasaporte
    {
        public enum Types
        {
            NotDefined,
            Normal,
            Consular,
            Servicio,
            Diplomatico,
            Subsidiario,
            Refugiados,
            TituloViaje,
            Apatridas,
            Provisional, //MCR.
            Oficial,//MDG.2011-04-26.Para el pasaporte Rep. Dominicana Oficial
            Maritimo //MCR.2016
        }
        public enum TypeRfid
        {
            NoDefined,
            No,
            A,
            B
        }
        public enum Thicknesses
        {
            Grueso25Pasaportes,
            Delgado50Pasaportes
        }

        public Country Country { get; private set; }
        public Types Type { get; private set; }
        public int NChars { get; private set; }
        public int NDigits { get; private set; }
        public TypeRfid RfIdType { get; private set; }
        public Thicknesses Thickness { get; private set; }
        public double Weight { get; private set; }
        public string IDCatalogo { get; set; }
        public string Destinatario { get; private set; }
        public string Responsable { get; private set; }
        public bool TieneFechaDeLaminacion { get;  set; }
        public string NombrePasaporte { get; private set; } //MDG.2011-03-17

        public TipoPasaporte(Country country, Types type, int nChars, int nDigits, TypeRfid rfIdType, Thicknesses thickness,
            double weight,string destinatario, string responsable, bool tieneFechaDeLaminacion)
        {
            Country = country;
            Type = type;
            NChars = nChars;
            NDigits = nDigits;
            RfIdType = rfIdType;
            Thickness = thickness;
            Destinatario = destinatario;
            Responsable = responsable;
            TieneFechaDeLaminacion = tieneFechaDeLaminacion;
            Weight = weight;
            NombrePasaporte = ""; //MDG.2011-03-17
        }

        public TipoPasaporte(Country country, Types type, int nChars, int nDigits, TypeRfid rfIdType, Thicknesses thickness,
            double weight, string destinatario, string responsable, bool tieneFechaDeLaminacion, string nombrePasaporte)
        {
            Country = country;
            Type = type;
            NChars = nChars;
            NDigits = nDigits;
            RfIdType = rfIdType;
            Thickness = thickness;
            Destinatario = destinatario;
            Responsable = responsable;
            TieneFechaDeLaminacion = tieneFechaDeLaminacion;
            Weight = weight;
            NombrePasaporte = nombrePasaporte;//MDG.2011-03-17
        }

        public static readonly TipoPasaporte Español = new TipoPasaporte(Country.España, Types.Normal, 2, 6, TypeRfid.A, Thicknesses.Grueso25Pasaportes,35,"MINISTERIO DEL INTERIOR","DIRECIÓN GENERAL DE LA POLICIA",true);
        public static readonly TipoPasaporte EspañolConsular = new TipoPasaporte(Country.España, Types.Consular, 2, 6, TypeRfid.A, Thicknesses.Grueso25Pasaportes, 35, "MINISTERIO DE ASUNTOS EXTERIORES", "", true);
        public static readonly TipoPasaporte Panama = new TipoPasaporte(Country.Panama, Types.Normal, 1, 7, TypeRfid.No, Thicknesses.Delgado50Pasaportes, 16.6,"REPÚBLICA DE PANAMÁ","AUTORIDAD MARITIMA DE PANAMA",false);
        public static readonly TipoPasaporte RepDominicana = new TipoPasaporte(Country.RepDominicana, Types.Normal, 1, 7, TypeRfid.No, Thicknesses.Grueso25Pasaportes, 35, "REPÚBLICA DOMINICANA", "DIRECCIÓN GENERAL DE PASAPORTES", false);
        public static readonly TipoPasaporte RepDominicanaConsular = new TipoPasaporte(Country.RepDominicana, Types.Consular, 2, 6, TypeRfid.No, Thicknesses.Grueso25Pasaportes, 36.5, "REPÚBLICA DOMINICANA", "DIRECCIÓN GENERAL DE PASAPORTES", false, "");//MDG.2011-03-17
        
        public int Length
        {
            get
            {
                return (NChars + NDigits);
            }
        }

        public bool HasRfid
        {
            get
            {
                return !(RfIdType == TypeRfid.No||RfIdType == TypeRfid.NoDefined);
            }
        }

        public override string ToString()
        {
            var label = String.Format("País: {0}; Categoría: {1}; Chip: {2}", Country.Name, Type, RfIdType);
            if (RfIdType != TypeRfid.A && RfIdType != TypeRfid.B)
                label = String.Format("País: {0}; Categoría: {1}; Chip: {2}", Country.Name, Type, "No");
            return label;
        }

        public string ToStringBd()
        {
            return String.Format("{0}", Country.Name);
        }

        public override bool Equals(object obj)
        {
            bool value = false;
            if (obj != null)
            {
                if (obj is TipoPasaporte)
                {
                    TipoPasaporte t = (TipoPasaporte)obj;
                    if (ToString() == obj.ToString())
                    {
                        value = true;
                    }
                }

            }
            return value;
        }

        public bool NewWeight(double weight)//MDG.2012-11-27. Para poder asignarlo a mitad de proceso
        {
            if (weight > 0)
            {
                Weight = weight;
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}