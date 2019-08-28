using System;

namespace Idpsa.Control.Sequence
{
    [Serializable]
    public class StepByStep
    {
        public bool ActivateStep { get; set; }        
        public bool StepByStepMode { get; set; }

        public StepByStep(){}      

        public bool StepByStepManager(bool stopStep)
        {
            if (StepByStepMode && !ActivateStep && stopStep)            
                return false;
           
            return true;
        }
    }
}