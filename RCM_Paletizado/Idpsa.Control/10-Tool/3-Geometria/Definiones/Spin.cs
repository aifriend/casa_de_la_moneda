using System;

namespace Idpsa.Control.Tool
{
    [Serializable]
    public enum Spin 
    { 
        Any,
        S0,
        S90,
        S180,
        S270
    }

    public static class SpinExtensions
    {
        public static string ToStringImage(this Spin spin)
        {
            string value = null;
            switch (spin)
            {
                case Spin.S0:
                    value = "0 grados";
                    break;
                case Spin.S90:
                    value = "90 grados";
                    break;
                case Spin.S180:
                    value = "180 grados";
                    break;
                case Spin.S270:
                    value = "-90 grados";
                    break;
                case Spin.Any:
                    value = "Cualquier giro";
                    break;
            }
            return value;

        }
    }
}