using RCMCommonTypes;

namespace Idpsa.PassportExtensions
{
    public static class PasaporteExtensions
    {
        public static string ConvertToString(this TipoPasaporte.TypeRfid t)
        {
            string s = null;
            switch (t)
            {
                case TipoPasaporte.TypeRfid.No:
                    s = "Sin RfId";
                    break;
                case TipoPasaporte.TypeRfid.A:
                    s = "Tipo A";
                    break;
                case TipoPasaporte.TypeRfid.B:
                    s = "Tipo B";
                    break;
                case TipoPasaporte.TypeRfid.NoDefined:
                    s = "No definido";
                    break;
            }
            return s;
        }

        public static string ConvertToString(this TipoPasaporte.Types t)
        {
            string value = "";
            if (t != TipoPasaporte.Types.Normal)
                value = t.ToString();

            return value;
        }

        public static int NumericValue(this TipoPasaporte.TypeRfid value)
        {
            int returnValue = 0;

            switch (value)
            {
                case TipoPasaporte.TypeRfid.No:
                    returnValue = 0;
                    break;
                case TipoPasaporte.TypeRfid.A:
                    returnValue = 1;
                    break;
                case TipoPasaporte.TypeRfid.B:
                    returnValue = 2;
                    break;
                case TipoPasaporte.TypeRfid.NoDefined:
                    returnValue = -1;
                    break;
            }

            return returnValue;
        }

        public static TipoPasaporte.TypeRfid NumericValueToTypeRfid(this int value)
        {
            TipoPasaporte.TypeRfid returnValue;
            switch (value)
            {
                case 0:
                    returnValue = TipoPasaporte.TypeRfid.No;
                    break;
                case 1:
                    returnValue = TipoPasaporte.TypeRfid.A;
                    break;
                case 2:
                    returnValue = TipoPasaporte.TypeRfid.B;
                    break;
                default:
                    returnValue = TipoPasaporte.TypeRfid.NoDefined;
                    break;
            }
            return returnValue;
        }


        public static int NumericValue(this TipoPasaporte.Types value)
        {
            int returnValue = 0;
            switch (value)
            {
                case TipoPasaporte.Types.Normal:
                    returnValue = 0;
                    break;
                case TipoPasaporte.Types.Consular:
                    returnValue = 1;
                    break;
                case TipoPasaporte.Types.Servicio:
                    returnValue = 2;
                    break;
                case TipoPasaporte.Types.Diplomatico:
                    returnValue = 3;
                    break;
                case TipoPasaporte.Types.Subsidiario:
                    returnValue = 4;
                    break;
                case TipoPasaporte.Types.Refugiados:
                    returnValue = 5;
                    break;
                case TipoPasaporte.Types.TituloViaje:
                    returnValue = 6;
                    break;
                case TipoPasaporte.Types.Apatridas:
                    returnValue = 7;
                    break;
                case TipoPasaporte.Types.Provisional:
                    returnValue = 8;
                    break; //MCR. 2011-04-12.
                case TipoPasaporte.Types.Oficial: //MDG.2011-04-26
                    returnValue = 9;
                    break;
            }

            return returnValue;
        }

        public static TipoPasaporte.Types NumericValueToPasaportType(this int value)
        {
            TipoPasaporte.Types returnValue;
            switch (value)
            {
                case 0:
                    returnValue = TipoPasaporte.Types.Normal;
                    break;
                case 1:
                    returnValue = TipoPasaporte.Types.Consular;
                    break;
                case 2:
                    returnValue = TipoPasaporte.Types.Servicio;
                    break;
                case 3:
                    returnValue = TipoPasaporte.Types.Diplomatico;
                    break;
                case 4:
                    returnValue = TipoPasaporte.Types.Subsidiario;
                    break;
                case 5:
                    returnValue = TipoPasaporte.Types.Refugiados;
                    break;
                case 6:
                    returnValue = TipoPasaporte.Types.TituloViaje;
                    break;
                case 7:
                    returnValue = TipoPasaporte.Types.Apatridas;
                    break;
                case 8:
                    returnValue = TipoPasaporte.Types.Provisional; //MCR. 2011-04-12.
                    break;
                case 9:
                    returnValue = TipoPasaporte.Types.Servicio; //MDG. 2011-04-26.
                    break;
                default:
                    returnValue = TipoPasaporte.Types.NotDefined;
                    break;
            }

            return returnValue;
        }
    }
}