using System.Collections.Generic;

namespace Idpsa.Control.Tool
{
    public class PairList<T, U> : List<Pair<T, U>>
    {
        public void Add(T v1, U v2)
        {
            Add(new Pair<T, U>(v1, v2));
        }
    }
}