namespace Idpsa
{
    public class DistanceIntervals
    {
        public DistanceIntervals(double centralPosition,
                                 double positiveInterval, double negativeInterval)
        {
            CentralPosition = centralPosition;
            PositiveInterval = positiveInterval;
            NegativeInterval = negativeInterval;
        }

        public double? CentralPosition { get; set; }
        public double PositiveInterval { get; set; }
        public double NegativeInterval { get; set; }

        public double PositivePosition
        {
            get { return (double) CentralPosition + PositiveInterval; }
        }

        public double NegativePosition
        {
            get { return (double) CentralPosition - NegativeInterval; }
        }
    }
}