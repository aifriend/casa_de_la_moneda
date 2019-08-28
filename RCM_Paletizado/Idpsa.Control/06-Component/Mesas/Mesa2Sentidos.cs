using System;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Mesa2Sentidos
    {
        private readonly ActuadorConInversor _motor;
        private readonly ISensor _sensor;

        public Mesa2Sentidos(ISensor sensor, ActuadorConInversor motor, string name)
        {
            _sensor = sensor;
            _motor = motor;
            Name = name;

            Producto = false;
        }

        public Mesa2Sentidos(ISensor sensor, ActuadorConInversor motor) : this(sensor, motor, "")
        {
        }

        public string Name { get; set; }
        public bool Producto { get; set; }

        public bool ISensor()
        {
            return _sensor.Value();
        }

        public void Sentido1()
        {
            _motor.Activate1();
        }

        public void Sentido2()
        {
            _motor.Activate2();
        }

        public void Parar()
        {
            _motor.Deactivate();
        }
    }
}