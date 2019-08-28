using System;

namespace Idpsa.Control.Tool
{
    [Serializable]
    public class CornerPoint3D : Point3D
    {
        public CornerPoint3D(Corners corner, Point3D p)
            : base(p)
        {
            Corner = corner;
        }

        public Corners Corner { get; private set; }

        public CornerPoint2D ToCornerPoint2D()
        {
            return new CornerPoint2D(Corner, new Point2D(X, Y));
        }
    }
}