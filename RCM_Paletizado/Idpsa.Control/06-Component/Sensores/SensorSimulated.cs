using System;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class SensorSimulated : Sensor
    {
        private readonly bool _value;

        public SensorSimulated(string description, bool value)
            : base(description)
        {
            _value = value;
        }
        public SensorSimulated(string description) : this(description, true) { }
        public SensorSimulated(bool value) : this(value.ToString(), value) { }
        public SensorSimulated() : this(true) { }

        public override bool Value()
        {
            return _value;
        }        
    }
}