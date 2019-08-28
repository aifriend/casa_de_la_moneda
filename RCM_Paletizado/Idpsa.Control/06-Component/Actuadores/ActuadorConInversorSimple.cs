using System;

namespace Idpsa.Control.Component
{
    [System.Serializable()]
	public class ActuadorConInversorSimple: ActuadorConInversor  
	{		
		protected IActuador Sentido1{get; private set;}
        protected IActuador Sentido2 { get; private set; }
        

        

        public ActuadorConInversorSimple(ActuadorConInversorSimple actuador)
            :base(){                        
            Sentido1 = actuador.Sentido1;
            Sentido2 = actuador.Sentido2;
            Name = actuador.Name;
            ActuatorStoped = actuador.ActuatorStoped;
        }
	
        public ActuadorConInversorSimple(IActuador  actuador1, IActuador actuador2, string name)
            :base(name)
		{
			Sentido1 = actuador1;
			Sentido2 = actuador2;			
            ActuatorStoped = true;
		}

        public ActuadorConInversorSimple(IActuador actuador1, IActuador actuador2) : this(actuador1, actuador2, "") { }

		
		
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
