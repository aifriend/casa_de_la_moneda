namespace Idpsa.Control.Subsystem
{
    public interface ISubsystemStateObserver
    {
        void OnStateChanged(SubsystemState state);
    }
}