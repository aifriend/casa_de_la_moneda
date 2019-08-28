using System;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control;


namespace Idpsa.Control.Engine
{
    [Serializable]
    public class CommandController : ICommandController
    {
        public bool ConnectionCommand { get; private set; }
        public bool ConnectionCommand2 { get; private set; }

        protected IEvaluable EmergencyOkIn { get; private set; }
        protected IEvaluable CommandOkIn { get; private set; }
        protected IEvaluable ConnectCommandIn { get; private set; }
        protected IEvaluable DisconnectCommandIn { get; private set; }

        protected IActivable EmergencyOkOut { get; private set; }
        protected IActivable CommandOkOut { get; private set; }

        private IEvaluable _pulseEmergencyOk;


        protected IEvaluable EmergencyOkIn2 { get; private set; }
        protected IEvaluable CommandOkIn2 { get; private set; }

        public CommandController(){}        

        public void CommandControl()
        {            
            EmergencyOkOut.Activate(_pulseEmergencyOk.Value());

            if (ConnectCommandIn.Value())
            {
                if (EmergencyOkIn.Value() && !CommandOkIn.Value())
                    CommandOkOut.Activate(true);
                else
                    CommandOkOut.Activate(false);
            }
            else
            {
                CommandOkOut.Activate(false);
            }

            ConnectionCommand = CommandOkIn.Value();
            ConnectionCommand2 = CommandOkIn2.Value();//MDG.2012-07-24            
        }
        public CommandController WithInputs(IEvaluable emergencyOk, IEvaluable commandOk,
                                            IEvaluable connectCommand, IEvaluable disconnectCommand,
                                            IEvaluable emergencyOk2, IEvaluable commandOk2
                                            )
        {
            if (emergencyOk == null) throw new ArgumentNullException("emergencyOk");
            if (commandOk == null) throw new ArgumentNullException("commandOk");
            if (connectCommand == null) throw new ArgumentNullException("connectCommandIn");
            if (disconnectCommand == null) throw new ArgumentNullException("disconnectCommand");
            if (emergencyOk2 == null) throw new ArgumentNullException("emergencyOk2");
            if (commandOk2 == null) throw new ArgumentNullException("commandOk2");
            

            EmergencyOkIn = emergencyOk;
            CommandOkIn = commandOk;
            ConnectCommandIn = connectCommand;
            DisconnectCommandIn = disconnectCommand;

            EmergencyOkIn2 = emergencyOk2;
            CommandOkIn2 = commandOk2;

            ContructPulseEmergencyOk();

            return this;
        }
        public CommandController WithOutputs(IActivable emergencyOk, IActivable commandOk)
        {
            if (emergencyOk == null) throw new ArgumentNullException("emergencyOk");
            if (commandOk == null) throw new ArgumentNullException("commandOk");

            EmergencyOkOut = emergencyOk;
            CommandOkOut = commandOk;

            return this;
        }

        private void ContructPulseEmergencyOk()
        {           
            _pulseEmergencyOk = DisconnectCommandIn.NOT()
                                                    .AND(EmergencyOkIn.NOT())
                                                    .EnableOfClock(1000, 200);
        }
        
    }
}