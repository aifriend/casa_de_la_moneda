namespace Idpsa.Control.Tool
{
    public class CornerPoint2D : Point2D
    {
        public CornerPoint2D(Corners corner, Point2D p)
            : base(p)
        {
            Corner = corner;
        }

        public Corners Corner { get; private set; }
    }
}