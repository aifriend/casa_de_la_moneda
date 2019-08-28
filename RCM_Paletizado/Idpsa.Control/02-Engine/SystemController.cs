using System;
using System.Collections.Generic;

namespace Idpsa.Control.Engine
{
    public class SystemController
    {              
        private event SystemControlRequestHandler SystemControlRequested;

        public SystemController(IDPSASystem sys)
        {
            if (sys == null)
                throw new ArgumentNullException("sys"); 
            TieToSystemControl(sys);
        }

        public void ActiveSubsystems(params object[] subsystems)
        {
            if (subsystems == null)
                throw new ArgumentNullException("subsystems");

            RequestSystemControl(SystemControlRequest.IdRequest.ActiveSubsystems,
                subsystems);
        }     

        public void ActiveSubsystems(IEnumerable<object> subsystem)
        {
            if (subsystem == null)
                throw new ArgumentNullException("subsystems");

            RequestSystemControl(SystemControlRequest.IdRequest.ActiveSubsystems,
                subsystem);
        }


        public void DeactiveSubsystems(params object[] subsystems)
        {
            if (subsystems == null)
                throw new ArgumentNullException("subsystems");

            RequestSystemControl(SystemControlRequest.IdRequest.DeactiveSubsystems,
                subsystems);
        }

        public void DeactiveSubsystems(IEnumerable<object> subsystem)
        {
            if (subsystem == null)
                throw new ArgumentNullException("subsystems");

            RequestSystemControl(SystemControlRequest.IdRequest.DeactiveSubsystems,
                subsystem);
        }

        public void AutomaticMode()
        {
            RequestSystemControl(SystemControlRequest.IdRequest.AutomaticMode);
        }
        
        public void BackToOriginMode()
        {
            RequestSystemControl(SystemControlRequest.IdRequest.BackToOriginMode);
        }

        public void ManualMode()
        {
            RequestSystemControl(SystemControlRequest.IdRequest.ManualMode);
        }

        public void Rearmament()
        {
            RequestSystemControl(SystemControlRequest.IdRequest.Rearmament);
        }

        public void ResetMaxCycleTime()
        {
            RequestSystemControl(SystemControlRequest.IdRequest.ResetMaxCycleTime);
        }

        public void StartMode()
        {
            RequestSystemControl(SystemControlRequest.IdRequest.StartMode);
        }

        public void StartModeWithStepByStep()
        {
            RequestSystemControl(SystemControlRequest.IdRequest.StartModeWithStepByStep);
        }

        public void StopMode()
        {
            RequestSystemControl(SystemControlRequest.IdRequest.StopMode);
        }

        public void StopModeAndDeativate()
        {
            RequestSystemControl(SystemControlRequest.IdRequest.StopModeAndDeativate);
        }

        #region auto2_transportes
        public void AutomaticMode2()
        {
            RequestSystemControl(SystemControlRequest.IdRequest.AutomaticMode2);
        }

        public void BackToOriginMode2()
        {
            RequestSystemControl(SystemControlRequest.IdRequest.BackToOriginMode2);
        }

        public void ManualMode2()
        {
            RequestSystemControl(SystemControlRequest.IdRequest.ManualMode2);
        }

        public void Rearmament2()
        {
            RequestSystemControl(SystemControlRequest.IdRequest.Rearmament2);
        }

        //public void ResetMaxCycleTime2()
        //{
        //    RequestSystemControl(SystemControlRequest.IdRequest.ResetMaxCycleTime2);
        //}

        public void StartMode2()
        {
            RequestSystemControl(SystemControlRequest.IdRequest.StartMode2);
        }

        public void StartModeWithStepByStep2()
        {
            RequestSystemControl(SystemControlRequest.IdRequest.StartModeWithStepByStep2);
        }

        public void StopMode2()
        {
            RequestSystemControl(SystemControlRequest.IdRequest.StopMode2);
        }

        public void StopModeAndDeativate2()
        {
            RequestSystemControl(SystemControlRequest.IdRequest.StopModeAndDeativate2);
        }
        #endregion

        private void TieToSystemControl(IDPSASystem sys)
        {
            SystemControlRequested += sys.Control.ActionRequestedHandler;
        }      

        private void RequestSystemControl(SystemControlRequest.IdRequest idAction)
        {
            if (SystemControlRequested != null)
                SystemControlRequested(new SystemControlRequest(idAction));
        }

        private void RequestSystemControl(SystemControlRequest.IdRequest idAction, object value)
        {
            if (SystemControlRequested != null)
                SystemControlRequested(new SystemControlRequest(idAction, value));
        }       
    }
}