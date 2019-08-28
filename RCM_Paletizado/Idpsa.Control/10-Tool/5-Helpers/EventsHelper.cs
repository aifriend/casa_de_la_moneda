using System;

namespace Idpsa.Control.Tool
{
    public static class AsyncEvents
    {
        public static void FireAsync(Delegate del, params object[] args)
        {
            if (del == null)
            {
                return;
            }
            Delegate[] delegates = del.GetInvocationList();
            AsyncFire asyncFire = InvokeDelegate;
            foreach (Delegate sink in delegates)
            {
                asyncFire.BeginInvoke(sink, args, null, null);
            }
        }

        private static void InvokeDelegate(Delegate del, object[] args)
        {
            del.DynamicInvoke(args);
        }

        public static void FireAsync(this EventHandler del, object sender, EventArgs e)
        {
            FireAsync(del, sender, e);
        }

        public static void FireAsync<E>(this EventHandler<E> del, object sender, E e) where E : EventArgs
        {
            FireAsync(del, sender, e);
        }

        public static void FireAsync(this Action del)
        {
            FireAsync(del);
        }

        public static void FireAsync<T>(this Action<T> del, T t)
        {
            FireAsync(del, t);
        }

        public static void FireAsync<T, U>(this Action<T, U> del, T t, U u)
        {
            FireAsync(del, t, u);
        }

        public static void FireAsync<T, U, V>(this Action<T, U, V> del, T t, U u, V v)
        {
            FireAsync(del, t, u, v);
        }

        #region Nested type: AsyncFire

        private delegate void AsyncFire(Delegate del, object[] args);

        #endregion
    }
}