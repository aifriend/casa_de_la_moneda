namespace Idpsa.Control.Component
{
    [System.Serializable()]
    public class ActuatorSimulated : Actuator
    {
        public ActuatorSimulated(string name) : base(name) { }
        public ActuatorSimulated() : base(string.Empty) { }

        public override void Activate(bool work){}    
        public override bool Value() { return false; }
    }
}
