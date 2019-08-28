using System;
using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Engine;
using Idpsa.Control.Manuals;
using Idpsa.Control.Subsystem;

namespace Idpsa.Paletizado
{
    public class BotoneraBarrera : IManagerRunnable
    {
        [Manual(SuperGroup = "General", Group = "Barrera")] private readonly IEvaluable _buttonResumeRequest;
        [Manual(SuperGroup = "General", Group = "Barrera")] private readonly IEvaluable _buttonStopRequest;

        private readonly SystemControl _control;
        private readonly SystemController _systemController;

        public BotoneraBarrera(Sensor buttonStopRequest,
                               Sensor buttonResumeRequest,
                               IDPSASystem sys)
        {
            _buttonStopRequest = buttonStopRequest;
            _buttonResumeRequest = buttonResumeRequest;
            _systemController = new SystemController(sys);
            _control = sys.Control;
        }


        private void Manager()
        {
            if (_control.OperationMode == Mode.Automatic)
            {
                if (_buttonStopRequest.Value() && _control.ActiveMode)
                {
                    _systemController.StopMode();
                }
                else if (_buttonResumeRequest.Value())
                {
                    _systemController.StartMode();
                }
            }
        }

        #region Miembros de IManagerRunnable

        IEnumerable<Action> IManagerRunnable.GetManagers()
        {
            return new Action[] {Manager};
        }

        #endregion
    }
}