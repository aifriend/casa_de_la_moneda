using Idpsa.Control.Tool;

namespace Idpsa
{
    public class SpinRectangle : Rectangle
    {
        public SpinRectangle(Point2D parOrigen, Point2D parLados)
            : base(parOrigen, parOrigen)
        {
        }

        public SpinRectangle(Point2D parOrigen, double parLx, double parLy)
            : base(parOrigen, parLx, parLy)
        {
        }

        public SpinRectangle(double origenX, double origenY, double parLx, double parLy)
            : base(origenX, origenY, parLx, parLy)
        {
        }

        public void SwapSides()
        {
            double tempX = Lados.X;
            Lados.X = Lados.Y;
            Lados.Y = tempX;
        }

        public void SwapSides(Spin rotation)
        {
            if (rotation == Spin.S180 || rotation == Spin.S270)
            {
                SwapSides();
            }
        }


        public Rectangle GetSwappedCopy()
        {
            return new Rectangle(Origen.X, Origen.Y, Lados.Y, Lados.X);
        }

        public Rectangle GetSwappedCopy(Spin rotation)
        {
            Rectangle r = null;

            if (rotation == Spin.S180 || rotation == Spin.S270)
                r = GetSwappedCopy();
            else
                r = GetCopy();

            return r;
        }
    }
}