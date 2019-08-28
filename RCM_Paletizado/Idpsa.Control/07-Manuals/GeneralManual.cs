using System;

namespace Idpsa.Control.Manuals
{
    public class GeneralManual
    {     
        public IEvaluable LimitSwithRest { get; set; }
        public IEvaluable LimitSwithWork { get; set; }
        public IActivable ActuatorRest { get; set; }
        public IActivable ActuatorWrk { get; set; }

        public GeneralManual()
        {          
            WorkEnable = () => true;
            RestEnable = () => true;
        }

        public Func<bool> WorkEnable { get; set; }
        public Func<bool> RestEnable { get; set; }
        public string WorkName { get; set; }
        public string RestName { get; set; }
        
    }
}