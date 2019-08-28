using System;

namespace Idpsa.Control.Tool
{  
    public static class PrimitiveExtensions
    {
        public static string Chars(this string s, int index)
        {
            return s.Substring(index, 1);
        }

        public static int ToInt(this bool b)
        {
            return (b ? 1 : 0);
        }

        public static bool ToBool(this int b)
        {
            return ((b != 0) ? true : false);
        }
        public static void Times(this int upperBound, Action<int> action)
        {
            for (int index = 0; index < upperBound; index++)
                action.Invoke(index);
        }
    }
   
}