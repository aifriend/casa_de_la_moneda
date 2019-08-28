namespace Idpsa.Control.Engine
{
    public interface IProtectionsController
    {
        void ProtectionsControl(SystemControl control);
        bool ProtectionsOK { get; }
        bool ProtectionsCanceled { get; }
        bool ProtectionsOK2 { get; }
        bool ProtectionsCanceled2 { get; }
    }
}