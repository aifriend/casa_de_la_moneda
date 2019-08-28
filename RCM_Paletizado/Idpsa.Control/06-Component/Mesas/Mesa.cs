using System;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Mesa
    {
        private readonly IActuador _motor;
        private readonly ISensor _sensor;

        public Mesa(ISensor sensor, IActuador motor, string name)
        {
            _sensor = sensor;
            _motor = motor;
            Name = name;
            Producto = false;
        }

        public Mesa(ISensor sensor, IActuador motor) : this(sensor, motor, "")
        {
        }

        public string Name { get; set; }
        public bool Producto { get; set; }

        public bool Sensor()
        {
            return _sensor.Value();
        }

        public void Motor(bool work)
        {
            _motor.Activate(work);
        }
    }
}