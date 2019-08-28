using System;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class SensorSimulado : Sensor
    {
        private readonly bool _value;
        public SensorSimulado(string name, bool value)
            : base(null, name)
        {
            _value = value;
        }

        public SensorSimulado(string name) : this(name, true) { }

        public SensorSimulado(bool value) : this("Simulado", value) { }

        public SensorSimulado() : this(true) { }



        public override bool Value()
        {
            return _value;
        }
        public override bool SecureValue()
        {
            return _value;
        }

    }
}