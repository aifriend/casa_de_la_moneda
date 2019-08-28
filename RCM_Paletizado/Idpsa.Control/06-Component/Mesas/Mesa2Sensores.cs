using System;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Mesa2Sensores : Mesa
    {
        private readonly ISensor _sensor2;

        public Mesa2Sensores(ISensor sensor, ISensor sensor2, IActuador motor, string name) : base(sensor, motor, name)
        {
            _sensor2 = sensor2;
        }

        public Mesa2Sensores(ISensor sensor, ISensor sensor2, IActuador motor) : this(sensor, sensor2, motor, "")
        {
        }

        public bool Sensor2()
        {
            return _sensor2.Value();
        }
    }
}