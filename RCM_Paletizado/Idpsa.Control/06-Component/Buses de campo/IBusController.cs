namespace Idpsa.Control.Component
{
    public interface IBusController
    {
        void WakeUpDevice();
        void RunDevice();
        void ResetOutputs();
        bool IsBusOK();
    }
}