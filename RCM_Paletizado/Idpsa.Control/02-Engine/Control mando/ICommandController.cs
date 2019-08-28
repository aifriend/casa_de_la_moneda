namespace Idpsa.Control.Engine
{    
    public interface ICommandController
    {
        void CommandControl();
        bool ConnectionCommand { get; }
        bool ConnectionCommand2 { get; }  
    }
}