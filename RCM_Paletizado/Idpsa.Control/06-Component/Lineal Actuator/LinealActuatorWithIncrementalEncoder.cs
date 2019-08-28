namespace Idpsa.Control.Component
{
    public class LinealActuatorWithIncrementalEncoder
    {
        private readonly ActuatorWithInversor _actuator;

        public LinealActuatorWithIncrementalEncoder(ActuatorWithInversor actuator, IncrementalEncoder encoder)
        {
            _actuator = actuator;
            Encoder = encoder;
        }

        public IncrementalEncoder Encoder { get; private set; }

        public bool Stoped
        {
            get { return _actuator.ActuatorStoped; }
        }

        public bool Referenced
        {
            get { return Encoder.Referenced; }
        }

        public void Activate1()
        {
            if (!Encoder.PulseCounter.BackWardsMode)
                _actuator.Activate1();
            else
                Encoder.PulseCounter.BackWardsMode = false;
        }

        public void Activate2()
        {
            if (Encoder.PulseCounter.BackWardsMode)
                _actuator.Activate2();
            else
                Encoder.PulseCounter.BackWardsMode = true;
        }

        public void Deactivate()
        {
            _actuator.Deactivate();
        }

        public bool Reference(double position, double countPercentaje)
        {
            return Encoder.SetReference(position, countPercentaje);
        }


        public double Position()
        {
            return Encoder.GetPosition();
        }
    }
}