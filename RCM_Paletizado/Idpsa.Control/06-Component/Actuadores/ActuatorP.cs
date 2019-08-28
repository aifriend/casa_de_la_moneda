
namespace Idpsa.Control.Component
{
    [System.Serializable()]
	public class ActuatorP : Actuator
	{				
		
		public ActuatorP(IActivable actuator,  string name):base(actuator,name){}
        public ActuatorP(IActivable actuator) : base(actuator) { }
		
        public override void Activate(bool work)
        {           
            Out.Activate(work);            
        }

	
       
	}
}
