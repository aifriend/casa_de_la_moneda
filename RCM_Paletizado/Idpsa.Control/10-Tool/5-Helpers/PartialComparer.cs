using System.Collections.Generic;

namespace Idpsa.Control.Tool
{
    public static class PartialComparer
    {
        public static int? Compare<T>(T first, T second)
        {
            return Compare(Comparer<T>.Default, first, second);
        }

        public static int? Compare<T>(IComparer<T> comparer, T first, T second)
        {
            int ret = comparer.Compare(first, second);
            if (ret == 0)
            {
                return null;
            }
            return ret;
        }

        public static int? ReferenceCompare<T>(T first, T second)
            where T : class
        {
            if (first == second)
            {
                return 0;
            }
            if (first == null)
            {
                return -1;
            }
            if (second == null)
            {
                return 1;
            }
            return null;
        }
    }
}