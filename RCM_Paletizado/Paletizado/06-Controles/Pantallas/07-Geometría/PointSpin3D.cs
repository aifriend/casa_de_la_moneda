using Idpsa.Control.Tool;

namespace Idpsa
{
    public class PointSpin3D : Point3D
    {
        public PointSpin3D()
        {
            Spin = Spin.Any;
            X = Y = Z = 0;
        }

        public PointSpin3D(Spin spin, Point3D p)
            : base(p)
        {
            Spin = spin;
        }

        public PointSpin3D(Spin spin, double x, double y, double z)
            : base(x, y, z)
        {
            Spin = spin;
        }

        public PointSpin3D(double z, PointSpin2D p2d)
        {
            X = p2d.X;
            Y = p2d.Y;
            Z = z;
            Spin = p2d.Spin;
        }

        public Spin Spin { get; protected set; }
    }
}