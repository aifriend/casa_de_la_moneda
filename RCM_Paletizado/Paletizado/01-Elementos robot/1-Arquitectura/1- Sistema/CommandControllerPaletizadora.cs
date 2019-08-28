using System;

namespace Idpsa.Control.Engine
{
    [Serializable]
    public class CommandControllerPaletizadora : ICommandController
    {
        protected IEvaluable EmergencyOkIn { get; private set; }
        protected IEvaluable CommandOkIn { get; private set; }
        protected IEvaluable ConnectionCommandIn { get; private set; }
        protected IEvaluable DisconnectionCommandIn { get; private set; }
        protected IEvaluable PararCasilleros { get; private set; }

        protected IActivable EmergencyOkOut { get; private set; }
        protected IActivable CommandOkOut { get; private set; }
        protected IActivable StopCasilleros { get; private set; }

        protected IEvaluable EmergencyOkIn2 { get; private set; }
        protected IEvaluable CommandOkIn2 { get; private set; }
        protected IEvaluable ConnectionCommandIn2 { get; private set; }
        protected IActivable CommandOkOut2 { get; private set; } //MCR.2016. mod. rearme Aereos.
        protected IActivable CommandMandoRele2 { get; private set; }

        #region ICommandController Members

        public bool ConnectionCommand { get; private set; }
        public bool ConnectionCommand2 { get; private set; }

        public void CommandControl()
        {
            if (PararCasilleros.Value())
                StopCasilleros.Activate(true);
            else
                StopCasilleros.Activate(false);

            if (ConnectionCommandIn.Value())
            {
                EmergencyOkOut.Activate(!EmergencyOkIn.Value());
                CommandOkOut.Activate(!CommandOkIn.Value());
            }
            else
            {
                EmergencyOkOut.Activate(false);
                CommandOkOut.Activate(false);
            }

            if (ConnectionCommandIn2.Value()) //MCR. 2016. Rearme Aereos.
            {
                CommandOkOut2.Activate(CommandOkIn2.Value());
                CommandMandoRele2.Activate(true);
            }
            else
            {
                CommandOkOut2.Activate(CommandOkIn2.Value());
                CommandMandoRele2.Activate(false);
            }

            ConnectionCommand = CommandOkIn.Value();
            ConnectionCommand2 = CommandOkIn2.Value();//MDG.2012-07-23
        }


        #endregion

        public CommandControllerPaletizadora WithInputs(IEvaluable emergencyOk, IEvaluable commandOk,
                                                        IEvaluable connectionCommand, IEvaluable disconnectionCommand,
                                                        IEvaluable pararCasilleros,
                                                        IEvaluable emergencyOk2, IEvaluable commandOk2,//MDG.2012-07-23
                                                        IEvaluable connectionCommand2)
        {
            EmergencyOkIn = emergencyOk;
            CommandOkIn = commandOk;
            ConnectionCommandIn = connectionCommand;
            DisconnectionCommandIn = disconnectionCommand;
            PararCasilleros = pararCasilleros;
            EmergencyOkIn2 = emergencyOk2;
            CommandOkIn2 = commandOk2;
            ConnectionCommandIn2 = connectionCommand2;//MCR.2016. Mod. Rearme Aereos.

            return this;
        }

        public CommandControllerPaletizadora WithOutputs(IActivable emergencyOk, IActivable commandOk,
                                                         IActivable stopCasilleros, IActivable commandOk2,
                                                           IActivable commandOkIn2Out)//MCR.2016. Mod. Rearme Aereos.)
        {
            EmergencyOkOut = emergencyOk;
            CommandOkOut = commandOk;
            StopCasilleros = stopCasilleros;
            CommandOkOut2 = commandOk2; //MCR.2016. Mod. Rearme Aereos. Luz armario
            CommandMandoRele2 = commandOkIn2Out; //MCR. Relé NA.

            return this;
        }
    }
}