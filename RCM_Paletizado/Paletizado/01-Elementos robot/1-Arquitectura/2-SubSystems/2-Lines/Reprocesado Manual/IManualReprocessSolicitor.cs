namespace Idpsa.Paletizado
{
    public interface IManualReprocessSolicitor
    {
        bool WaitingReprocess { get; }
        CajaPasaportes GetBoxToReproccess();
        void OnReprocess();
        void AttachToReprocessor(IManualReprocessor reprocessor);
    }
}