
namespace Idpsa.Control.Component
{
    [System.Serializable()]
    public class ActuadorConInvFinalCarrera:ActuadorConInversorSimple
    {
        private ISensor _sensor1;
        private ISensor _sensor2;

        public ActuadorConInvFinalCarrera(ActuadorConInvFinalCarrera actuador):base(actuador)
        {
            _sensor1 = actuador._sensor1;
            _sensor2 = actuador._sensor2;
        }

        public ActuadorConInvFinalCarrera(IActuador actuador1, IActuador actuador2,ISensor sensor1,ISensor sensor2, string name)
        :base(actuador1,actuador2,name){
            _sensor1 = sensor1;
            _sensor2 = sensor2; 
        }

        public ActuadorConInvFinalCarrera(IActuador actuador1, IActuador actuador2, ISensor sensor1, ISensor sensor2) : this(actuador1, actuador2,sensor1,sensor2, "") { }

      

        public override void Activate1()
        {
            if (!_sensor1.Value()) base.Activate1();
            else Deactivate();            
        }

        public override void Activate2()
        {
            if (!_sensor2.Value()) base.Activate2();
            else Deactivate();  
        }
       

    }
}
