using System;
using System.Collections;

namespace Idpsa.Control.Tool
{
    [Serializable]
    public class Point2DConf
    {
        private Double _x;
        private Double _y;

        public Point2DConf(Point2DConf p)
        {
            _x = p.X;
            _y = p.Y;
        }

        public Point2DConf(Double parX, Double ParY)
        {
            _x = parX;
            _y = ParY;
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

        public Point2DConf Escalar(Point2DConf factor)
        {
            return new Point2DConf(X * factor.X, Y * factor.Y);
        }

        public Point2DConf Escalar(double x, double y)
        {
            return new Point2DConf(x * X, y * Y);
        }

        public Point2DConf Desplazado(Double despX, Double despY)
        {
            return new Point2DConf(_x + despX, _y + despY);
        }

        public Point2DConf Desplazado(Point2DConf p)
        {
            return new Point2DConf(_x + p._x, _y + p._y);
        }

        public Point2DConf Mult(Double factor)
        {
            return new Point2DConf(factor * _x, factor * _y);
        }

        public Point2DConf MutlComponents(Point2DConf p)
        {
            return new Point2DConf(_x * p._x, _y * p._y);
        }

        public Point2DConf MutlComponents(double x, double y)
        {
            return new Point2DConf(x * _x, y * _y);
        }

        public Point2DConf Div(Double factor)
        {
            Point2DConf p = factor != 0 ? new Point2DConf(_x / factor, _y / factor) : null;
            return p;
        }

        public Point2DConf CoorRespecto(Double parX, Double parY)
        {
            return new Point2DConf(_x - parX, _y - parY);
        }

        public Point2DConf CoorRespecto(Point2DConf p)
        {
            return new Point2DConf(_x - p._x, _y - p._y);
        }

        public Point2DConf CompPositivas()
        {
            return new Point2DConf(Math.Abs(_x), Math.Abs(_y));
        }

        public Point2DConf Unitario()
        {
            Point2DConf p = Modulo() != 0 ? Div(Modulo()) : null;
            return p;
        }

        public Double Modulo()
        {
            return Math.Sqrt(Math.Pow(_x, 2) + Math.Pow(_y, 2));
        }

        public Double DistanciaTo(Point2DConf p)
        {
            return CoorRespecto(p).Modulo();
        }

        public override string ToString()
        {
            return String.Format("({0:0.000}; {1:0.000})", _x, _y);
        }

        public double DotProduct(Point2DConf p)
        {
            return (_x * p._x + _y * p._y);
        }

        public double AngleWith(Point2DConf p)
        {
            return Math.Acos(DotProduct(p) / (Modulo() * p.Modulo()));
        }

        #region Nested type: clsCenter

        public class clsCenter : IComparer
        {
            private readonly Point2DConf centro;

            public clsCenter(Point2DConf parCentro)
            {
                centro = new Point2DConf(parCentro);
            }

            #region IComparer Members

            int IComparer.Compare(object x, object y)
            {
                int value;
                var p1 = (Point2DConf)x;
                var p2 = (Point2DConf)y;
                if (p1.DistanciaTo(centro) < p2.DistanciaTo(centro))
                {
                    value = -1;
                }
                else value = p1.DistanciaTo(centro) > p2.DistanciaTo(centro) ? 1 : 0;
                return value;
            }

            #endregion
        }

        #endregion
    }
}