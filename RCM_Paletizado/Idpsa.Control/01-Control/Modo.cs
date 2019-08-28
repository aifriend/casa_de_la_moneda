namespace Idpsa.Control
{
    public struct Modo
    {
        public const int Automatico = 30;
        public const int Especial = 40;
        public const int Manual = 20;
        public const int SinModo = 0;
        public const int VueltaOrigen = 50;

        public static string ToString(int modo)
        {
            string s;
            switch (modo)
            {
                case SinModo:
                    s = "Sin Modo";
                    break;
                case Manual:
                    s = "Manual";
                    break;
                case Automatico:
                    s = "Automatico";
                    break;
                case VueltaOrigen:
                    s = "Vuelta Origen";
                    break;
                case Especial:
                    s = "Especial";
                    break;
                default:
                    s = "Sin Modo";
                    break;
            }
            return s;
        }
    }
}