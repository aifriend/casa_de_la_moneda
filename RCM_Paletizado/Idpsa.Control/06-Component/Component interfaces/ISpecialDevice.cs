namespace Idpsa.Control.Component
{
    public interface ISpecialDevice
    {
        bool InError();
        void OnErrorAction();
    }
}