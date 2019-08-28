using System;

namespace Idpsa.Control.Component
{
    public interface IDataNotifier<T>
    {
        string NotifierName { get; }
        void Subscribe(EventHandler<DataEventArgs<T>> newDataHandler);
    }
}