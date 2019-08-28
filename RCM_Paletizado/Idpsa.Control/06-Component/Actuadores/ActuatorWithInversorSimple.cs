using System;

namespace Idpsa.Control.Component
{
    [System.Serializable()]
	public class ActuatorWithInversorSimple: ActuatorWithInversor  
	{		
		protected IActivable Sentido1 { get; private set; }
        protected IActivable Sentido2 { get; private set; }
        
        public ActuatorWithInversorSimple(ActuatorWithInversorSimple actuator)
            :base()
        {
            if (actuator == null)
                throw new ArgumentNullException("actuator"); 
            Sentido1 = actuator.Sentido1;
            Sentido2 = actuator.Sentido2;
            Name = actuator.Name;
            ActuatorStoped = actuator.ActuatorStoped;
        }
	
        public ActuatorWithInversorSimple(IActivable  actuator1, IActivable actuator2, string name)
            :base(name)
		{
            if (actuator1 == null)
                throw new ArgumentNullException("actuator1");
            if (actuator2 == null)
                throw new ArgumentNullException("actuator2");
            
			Sentido1 = actuator1;
			Sentido2 = actuator2;			
            ActuatorStoped = true;
		}

        public ActuatorWithInversorSimple(IActivable actuator1, IActivable actuator2) : this(actuator1, actuator2, String.Empty) { }
		
		public override void Activate1()
		{
            Sentido1.Activate(true);
            Sentido2.Activate(false);
            ActuatorStoped = false;
		}

		public override void Activate2()
		{
            Sentido1.Activate(false);
			Sentido2.Activate(true);
            ActuatorStoped = false;
		}

		public override void Deactivate()
		{
			Sentido1.Activate(false);
			Sentido2.Activate(false);
            ActuatorStoped = true;
		}

    }
}