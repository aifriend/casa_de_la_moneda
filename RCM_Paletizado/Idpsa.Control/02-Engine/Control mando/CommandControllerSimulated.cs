using System;

namespace Idpsa.Control.Engine
{
    [Serializable]
    public class CommandControllerSimulated : ICommandController
    {    
        public void CommandControl(){}

        public bool ConnectionCommand
        {
            get { return true; }
        }
        public bool ConnectionCommand2
        {
            get { return true; }
        }         
    }
}