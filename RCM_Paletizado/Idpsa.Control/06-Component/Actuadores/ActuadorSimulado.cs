
namespace Idpsa.Control.Component
{
    [System.Serializable()]
    public class ActuadorSimulado : Actuador
    {

        public ActuadorSimulado(string name) : base(null, name) { }
        public ActuadorSimulado() : base(null, "") { }

        public override void Activate(bool work){}
        public override void ActivateSecure(bool work) {}

        public override bool Value() { return false; }
    }
}
