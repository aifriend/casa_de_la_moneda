namespace Idpsa.Control.Component
{
    public interface IDataNotifierProvider<T>
    {
        IDataNotifier<T> GetDataNotifier();
    }
}