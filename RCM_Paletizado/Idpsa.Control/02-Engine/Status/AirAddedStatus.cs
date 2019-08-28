using System;

namespace Idpsa.Control.Engine.Status
{
    public class AirAddedStatus : AddedStatus
    {
        protected IEvaluable ValveAirIn { get; set; }
        protected IActivable ValveAirOut { get; set; }
        protected IEvaluable ValveAirIn2 { get; set; }
        protected IActivable ValveAirOut2 { get; set; }
        protected IEvaluable ValveAirIn3 { get; set; }
        protected IActivable ValveAirOut3 { get; set; }

        public AirAddedStatus(string name, IEvaluable valveAirIn, IActivable valveAirOut, IEvaluable valveAirIn2, IActivable valveAirOut2, IEvaluable valveAirIn3, IActivable valveAirOut3)
            : base(name)
        {
            if (valveAirIn == null) throw new ArgumentNullException("valveAirIn");
            if (valveAirOut == null) throw new ArgumentNullException("valveAirOut");
            if (valveAirIn2 == null) throw new ArgumentNullException("valveAirIn2");
            if (valveAirOut2 == null) throw new ArgumentNullException("valveAirOut2");
            if (valveAirIn2 == null) throw new ArgumentNullException("valveAirIn3");
            if (valveAirOut2 == null) throw new ArgumentNullException("valveAirOut3");

            ValveAirIn = valveAirIn;
            ValveAirOut = valveAirOut;
            ValveAirIn2 = valveAirIn2;
            ValveAirOut2 = valveAirOut2;
            ValveAirIn3 = valveAirIn3;
            ValveAirOut3 = valveAirOut3;
        }

        protected override StatusValue StatusControlCore(SystemControl control)
        {           
            ValveAirOut.Activate(control.ConnectionCommand);
            return ValveAirIn.Value() ? StatusValue.OK : StatusValue.Abort;
        }
        protected override StatusValue StatusControlCore2(SystemControl control)
        {
            ValveAirOut2.Activate(control.ConnectionCommand2);
            return ValveAirIn2.Value() ? StatusValue.OK : StatusValue.Abort;
        }
        protected override StatusValue StatusControlCore3(SystemControl control)
        {
            ValveAirOut3.Activate(control.ConnectionCommand2);
            return ValveAirIn3.Value() ? StatusValue.OK : StatusValue.Abort;
        }
    }
}