namespace Idpsa.Control.Tool
{
    public class Singleton<T> where T : class, new()
    {
        private Singleton()
        {
        }

        public static T Instance
        {
            get { return SingletonCreator.instance; }
        }

        #region Nested type: SingletonCreator

        private class SingletonCreator
        {
            // Private object instantiated with private constructor
            internal static readonly T instance = new T();
        }

        #endregion
    }
}