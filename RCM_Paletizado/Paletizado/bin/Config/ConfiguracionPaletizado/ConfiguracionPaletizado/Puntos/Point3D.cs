using System;

namespace Idpsa.Control.Tool
{
    [Serializable]
    public class Point3DConf
    {
        private Double _x;
        private Double _y;
        private Double _z;

        public Point3DConf()
        {
            _x = 0;
            _y = 0;
            _z = 0;
        }

        public Point3DConf(Point3DConf p)
        {
            _x = p._x;
            _y = p._y;
            _z = p._z;
        }

        public Point3DConf(Double z, Point2DConf p)
        {
            _z = z;
            _x = p.X;
            _y = p.Y;
        }

        public Point3DConf(double x, double y, double z)
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

        public static Point3DConf EsfToCart(double r, double teta, double fi)
        {
            const double conv = (Math.PI / 180);
            double Teta = (conv * teta);
            double Fi = (conv * fi);
            double R = Math.Abs(r);
            return new Point3DConf((R * (Math.Cos(Fi) * Math.Sin(Teta))), (R * (Math.Sin(Fi) * Math.Sin(Teta))),
                               (R * Math.Cos(Teta)));
        }

        public static Point3DConf CilToCart(double r, double fi, double z)
        {
            const double conv = (Math.PI / 180);
            double Fi = (conv * fi);
            double R = Math.Abs(r);
            return new Point3DConf((R * Math.Cos(Fi)), (R * Math.Sin(Fi)), z);
        }

        public Point3DConf Desplazado(Double despX, Double despY, Double despZ)
        {
            return new Point3DConf((_x + despX), (_y + despY), (_z + despZ));
        }

        public Point3DConf Desplazado(Point3DConf p)
        {
            return new Point3DConf((_x + p._x), (_y + p._y), (_z + p._z));
        }

        public Point3DConf CoorRespecto(Double x, Double y, Double z)
        {
            return new Point3DConf((_x - x), (_y - y), (_z - z));
        }

        public Point3DConf CoorRespecto(Point3DConf p)
        {
            return new Point3DConf((_x - p._x), (_y - p._y), (_z - p._z));
        }

        public Point3DConf Mult(Double factor)
        {
            return new Point3DConf((factor * _x), (factor * _y), (factor * _z));
        }

        public Point3DConf Div(Double factor)
        {
            Point3DConf p = (factor != 0) ? new Point3DConf((_x / factor), (_y / factor), (_z / factor)) : null;
            return p;
        }

        public Point3DConf CompPositivas()
        {
            return new Point3DConf(Math.Abs(_x), Math.Abs(_y), Math.Abs(_z));
        }

        public Point3DConf Unitario()
        {
            Point3DConf p = Modulo() != 0 ? Div(Modulo()) : null;
            return p;
        }

        public Double DistanciaTo(Point3DConf p)
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