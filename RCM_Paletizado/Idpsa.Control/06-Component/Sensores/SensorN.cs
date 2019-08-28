using System;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class SensorN : Sensor
    {
        public SensorN(Input input) : base(input) { }
        public SensorN(IEvaluable input, string description) : base(input, description){}
        public SensorN(IEvaluable input) : base(input){}

        public override bool Value()
        {
            return (!In.Value());
        }
       
    }
}