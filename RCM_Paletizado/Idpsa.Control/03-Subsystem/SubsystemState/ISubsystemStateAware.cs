namespace Idpsa.Control.Subsystem
{
    public interface ISubsystemStateAware
    {
        ISubsystemStateObserver SetSubsystemStateController(ISubsystemStateController value);
    }
}