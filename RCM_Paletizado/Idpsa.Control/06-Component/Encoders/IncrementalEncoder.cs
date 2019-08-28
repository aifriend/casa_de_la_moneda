namespace Idpsa.Control.Component
{
    public class IncrementalEncoder
    {
        private readonly double _distancePulseFactor;
        private int _counterOrigin;
        private int _lastCount;
        private double distanceOrigin;

        public IncrementalEncoder(ICounter pulseCounter, double pulsesPerRevolution,
                                  double linealDistancePerRevolution, double reductionFactor)
        {
            PulseCounter = pulseCounter;
            PulsesPerRevolution = pulsesPerRevolution;
            DistancePerRevolution = linealDistancePerRevolution;
            ReductionFactor = reductionFactor;
            _distancePulseFactor = DistancePerRevolution / (ReductionFactor * PulsesPerRevolution);
        }

        public bool Referenced { get; private set; }
        public double LinealPosition { get; private set; }
        public double PulsesPerRevolution { get; private set; }
        public double ReductionFactor { get; private set; }
        public double DistancePerRevolution { get; set; }
        public ICounter PulseCounter { get; private set; }

        public int PulseIncrement()
        {
            int value = _lastCount;
            _lastCount = PulseCounter.GetValue();
            return (_lastCount - value);
        }


        public bool SetReference(double position, double countPercentaje)
        {
            Referenced = false;

            int counterValue = PulseCounter.MinValue +
                               (int)((PulseCounter.MaxValue - PulseCounter.MinValue) * countPercentaje);

            if (PulseCounter.SetValue(counterValue))
            {
                LinealPosition = distanceOrigin = position;
                _counterOrigin = counterValue;
                Referenced = true;
                _lastCount = counterValue;
            }

            return Referenced;
        }

        public double GetPosition()
        {
            RefreshPosition();
            return LinealPosition;
        }

        public void RefreshPosition()
        {
            LinealPosition = _distancePulseFactor * (PulseCounter.GetValue() - _counterOrigin) + distanceOrigin;
        }
    }
}