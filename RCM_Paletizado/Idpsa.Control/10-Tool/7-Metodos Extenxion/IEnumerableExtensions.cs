using System;
using System.Collections.Generic;
using System.Linq;

namespace Idpsa.Control.Tool
{
    public static class IEnumerableExtensions
    {      
        public static TElement MaxElement<TElement, TData>(
            this IEnumerable<TElement> source,
            Func<TElement, TData> selector)
            where TData : IComparable<TData>
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");
                       
            TElement result = default(TElement);
            TData maxValue = default(TData);
            foreach (TElement element in source)
            {
                TData candidate = selector(element);
                if (candidate.CompareTo(maxValue) > 0)
                {                    
                    maxValue = candidate;
                    result = element;
                }
            }
            return result;
        }

        public static TElement MinElement<TElement, TData>(
            this IEnumerable<TElement> source,
            Func<TElement, TData> selector)
            where TData : IComparable<TData>
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            bool firstElement = true;

            TElement result = default(TElement);
            TData minValue = default(TData);
            foreach (TElement element in source)
            {
                TData candidate = selector(element);
                if (firstElement || candidate.CompareTo(minValue) < 0)
                {
                    firstElement = false;
                    minValue = candidate;
                    result = element;
                }
            }
            return result;
        }

        public static void ForEach<T>(
            this IEnumerable<T> source,
            Action<T> action)
        {
            foreach (T item in source)
                action(item);
        }

        public static T[] ToArray<T>(this IEnumerable<T> source, int pos, int Lenght)
        {
            var result = new T[Lenght];
            for (int i = 0; i < Lenght; i++)
                result[i] = source.ElementAt(i + pos);

            return result;
        }
    }
}
