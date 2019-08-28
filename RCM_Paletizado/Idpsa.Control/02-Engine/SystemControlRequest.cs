using System;

namespace Idpsa.Control.Engine
{
    [Serializable]
    internal class SystemControlRequest
    {
        #region IdRequest enum

        internal enum IdRequest
        {
            None,
            StartMode,
            StartModeWithStepByStep,
            StopMode,
            StopModeAndDeativate,
            ManualMode,
            AutomaticMode,
            BackToOriginMode,
            ActiveSubsystems,
            DeactiveSubsystems,
            Rearmament,
            ResetMaxCycleTime,
            StartMode2,
            StartModeWithStepByStep2,
            StopMode2,
            StopModeAndDeativate2,
            ManualMode2,
            AutomaticMode2,
            BackToOriginMode2,
            Rearmament2   
        }

        #endregion

        public SystemControlRequest(IdRequest id)
        {            
            Id = id;
        }

        public SystemControlRequest(IdRequest id, object value)
        {
            Id = id;
            Value = value;
        }

        public IdRequest Id { get; private set; }
        public object Value { get; private set; }
    }
}