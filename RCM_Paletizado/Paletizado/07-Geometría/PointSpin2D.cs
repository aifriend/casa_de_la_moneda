using System;
using Idpsa.Control.Tool;

namespace Idpsa
{
    [Serializable]
    public class PointSpin2D : Point2D
    {
        public PointSpin2D(Spin spin, Point2D p)
            : base(p)
        {
            Spin = spin;
        }

        public PointSpin2D(Spin spin, double x, double y)
            : base(x, y)
        {
            Spin = spin;
        }

        public Spin Spin { get; set; }

        public Point2D Direction()
        {
            Point2D value = null;

            switch (Spin)
            {
                case Spin.S0:
                    value = new Point2D(1, 0);
                    break;
                case Spin.S90:
                    value = new Point2D(0, 1);
                    break;
                case Spin.S180:
                    value = new Point2D(-1, 0);
                    break;
                case Spin.S270:
                    value = new Point2D(0, -1);
                    break;
            }
            return value;
        }

        public Point2D NormalDirection()
        {
            Point2D direction = Direction();
            return new Point2D(direction.Y, direction.X);
        }

        public bool IsHorizontalOriented()
        {
            return (Spin == Spin.S0 || Spin == Spin.S180);
        }

        public bool IsVerticalOriented()
        {
            return !IsHorizontalOriented();
        }
    }
}