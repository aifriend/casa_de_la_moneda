using System;

namespace Idpsa.Control.Tool
{
    [Serializable]
    public class Point3D
    {
        private Double _x;
        private Double _y;
        private Double _z;

        public Point3D()
        {
            _x = 0;
            _y = 0;
            _z = 0;
        }

        public Point3D(Point3D p)
        {
            _x = p._x;
            _y = p._y;
            _z = p._z;
        }

        public Point3D(Double z, Point2D p)
        {
            _z = z;
            _x = p.X;
            _y = p.Y;
        }

        public Point3D(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public Double X
        {
            get { return _x; }
            set { _x = value; }
        }

        public Double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public Double Z
        {
            get { return _z; }
            set { _z = value; }
        }

        public static Point3D EsfToCart(double r, double teta, double fi)
        {
            const double conv = (Math.PI / 180);
            double Teta = (conv * teta);
            double Fi = (conv * fi);
            double R = Math.Abs(r);
            return new Point3D((R * (Math.Cos(Fi) * Math.Sin(Teta))), (R * (Math.Sin(Fi) * Math.Sin(Teta))),
                               (R * Math.Cos(Teta)));
        }

        public static Point3D CilToCart(double r, double fi, double z)
        {
            const double conv = (Math.PI / 180);
            double Fi = (conv * fi);
            double R = Math.Abs(r);
            return new Point3D((R * Math.Cos(Fi)), (R * Math.Sin(Fi)), z);
        }

        public Point3D Desplazado(Double despX, Double despY, Double despZ)
        {
            return new Point3D((_x + despX), (_y + despY), (_z + despZ));
        }

        public Point3D Desplazado(Point3D p)
        {
            return new Point3D((_x + p._x), (_y + p._y), (_z + p._z));
        }

        public Point3D CoorRespecto(Double x, Double y, Double z)
        {
            return new Point3D((_x - x), (_y - y), (_z - z));
        }

        public Point3D CoorRespecto(Point3D p)
        {
            return new Point3D((_x - p._x), (_y - p._y), (_z - p._z));
        }

        public Point3D Mult(Double factor)
        {
            return new Point3D((factor * _x), (factor * _y), (factor * _z));
        }

        public Point3D Div(Double factor)
        {
            Point3D p = (factor != 0) ? new Point3D((_x / factor), (_y / factor), (_z / factor)) : null;
            return p;
        }

        public Point3D CompPositivas()
        {
            return new Point3D(Math.Abs(_x), Math.Abs(_y), Math.Abs(_z));
        }

        public Point3D Unitario()
        {
            Point3D p = Modulo() != 0 ? Div(Modulo()) : null;
            return p;
        }

        public Double DistanciaTo(Point3D p)
        {
            return CoorRespecto(p).Modulo();
        }

        public Double Modulo()
        {
            return Math.Sqrt(Math.Pow(_x, 2) + Math.Pow(_y, 2) + Math.Pow(_z, 2));
        }

        public override string ToString()
        {
            return String.Format("({0:0.000}, {1:0.000}, {2:0.000})", _x, _y, _z);
        }
    }
}