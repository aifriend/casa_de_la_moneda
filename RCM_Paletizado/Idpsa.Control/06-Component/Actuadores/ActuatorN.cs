
namespace Idpsa.Control.Component
{
    [System.Serializable()]
	public class ActuatorN : Actuator
	{

        public ActuatorN(IActivable actuator, string description) : base(actuator, description) { }
        public ActuatorN(IActivable actuator) : base(actuator) { }
        		
        public override void Activate(bool work)
        {            
            Out.Activate(!work);            
        }
	}
}
