namespace Idpsa.Control.Tool
{
    public class Pair<T, U>
    {
        public Pair()
        {
            Value1 = default(T);
            Value2 = default(U);
        }

        public Pair(T value1, U value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public T Value1 { get; set; }
        public U Value2 { get; set; }
    }
}