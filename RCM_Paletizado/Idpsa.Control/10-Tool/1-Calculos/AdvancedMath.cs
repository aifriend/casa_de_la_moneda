using System;
using System.Globalization;

namespace Idpsa.Control.Tool
{
    public static class AdvancedMath
    {
        public static double Format(double value, string pformat)
        {
            if ((!double.IsNaN(value) & !double.IsInfinity(value)))
            {
                string valueString = value.ToString(pformat);
                return double.Parse(valueString);
            }
            return value;
        }

        public static string Format(string value, string pformat)
        {
            double number = double.Parse(value, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
            return number.ToString(pformat);
        }

        public static int WeightBit(bool bit, int weight)
        {
            return Convert.ToInt32(Convert.ToDouble(bit) * Math.Pow(Convert.ToDouble(2), Convert.ToDouble(weight)));
        }
    }
}