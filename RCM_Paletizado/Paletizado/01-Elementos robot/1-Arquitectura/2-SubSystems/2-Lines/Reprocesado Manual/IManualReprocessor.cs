namespace Idpsa.Paletizado
{
    public interface IManualReprocessor
    {
        void AttachToSolicitor(IManualReprocessSolicitor solicitor);
        void OnNewRequest();
    }
}