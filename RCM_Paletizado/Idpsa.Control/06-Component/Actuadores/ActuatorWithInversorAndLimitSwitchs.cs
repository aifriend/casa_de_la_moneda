using System;

namespace Idpsa.Control.Component
{
    [System.Serializable()]
    public class ActuatorWithInversorAndLimitSwitchs :ActuatorWithInversorSimple
    {
        private IEvaluable _sensor1;
        private IEvaluable _sensor2;

        public ActuatorWithInversorAndLimitSwitchs(ActuatorWithInversorAndLimitSwitchs actuator)
            : base(actuator)
        {
            if (actuator == null)
                throw new ArgumentNullException("actuator");

            _sensor1 = actuator._sensor1;
            _sensor2 = actuator._sensor2;
        }

        public ActuatorWithInversorAndLimitSwitchs(IActivable actuator1, IActivable actuator2,IEvaluable sensor1,IEvaluable sensor2, string name)
        :base(actuator1,actuator2,name)
        {
            if (sensor1 == null)
                throw new ArgumentNullException("sensor1");
            if (sensor2 == null)
                throw new ArgumentNullException("sensor2"); 

            _sensor1 = sensor1;
            _sensor2 = sensor2; 
        }

        public ActuatorWithInversorAndLimitSwitchs(IActivable actuator1, IActivable actuator2, IEvaluable sensor1, IEvaluable sensor2) : this(actuator1, actuator2, sensor1, sensor2, "") { }
              

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
