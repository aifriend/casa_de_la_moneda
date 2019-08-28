using System;

namespace Idpsa.Control
{
    public class DataEventArgs<T> : EventArgs
    {
        public DataEventArgs(T data)
        {
            Data = data;
        }
        public T Data { get; protected set; }      
    }
}