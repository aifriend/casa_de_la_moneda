using System;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class SensorP : Sensor
    {
        public SensorP(Input input) : base(input) { }
        public SensorP(IEvaluable input, string description) : base(input, description){}
        public SensorP(IEvaluable input) : base(input){}

        public override bool Value()
        {
            return In.Value();
        }
    }
}