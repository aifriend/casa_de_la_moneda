
namespace Idpsa.Control.Component
{
    [System.Serializable()]
	public class ActuadorP:Actuador
	{				
		
		public ActuadorP(IBitActivable actuador,  string name):base(actuador,name){}
        public ActuadorP(IBitActivable actuador) : base(actuador) { }

		
        public override void Activate(bool work)
        {
            try
            {
                OutPut.Activate(work);
            }
            catch {

               
            }
        }
		public override void ActivateSecure(bool work)
		{

            if ((OutPut!= null))
			{
                OutPut.Activate(work); 
			}
			
		}		

       
	}
}
