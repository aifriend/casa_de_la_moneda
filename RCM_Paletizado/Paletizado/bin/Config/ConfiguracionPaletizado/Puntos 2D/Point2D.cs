using System;
using System.Collections;

namespace Idpsa.Control.Tool
{
    [Serializable]
    public class Point2D
    {
        private Double _x;
        private Double _y;

        public Point2D(Point2D p)
        {
            _x = p.X;
            _y = p.Y;
        }

        public Point2D(Double parX, Double ParY)
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

        public Point2D Escalar(Point2D factor)
        {
            return new Point2D(X * factor.X, Y * factor.Y);
        }

        public Point2D Escalar(double x, double y)
        {
            return new Point2D(x * X, y * Y);
        }

        public Point2D Desplazado(Double despX, Double despY)
        {
            return new Point2D(_x + despX, _y + despY);
        }

        public Point2D Desplazado(Point2D p)
        {
            return new Point2D(_x + p._x, _y + p._y);
        }

        public Point2D Mult(Double factor)
        {
            return new Point2D(factor * _x, factor * _y);
        }

        public Point2D MutlComponents(Point2D p)
        {
            return new Point2D(_x * p._x, _y * p._y);
        }

        public Point2D MutlComponents(double x, double y)
        {
            return new Point2D(x * _x, y * _y);
        }

        public Point2D Div(Double factor)
        {
            Point2D p = factor != 0 ? new Point2D(_x / factor, _y / factor) : null;
            return p;
        }

        public Point2D CoorRespecto(Double parX, Double parY)
        {
            return new Point2D(_x - parX, _y - parY);
        }

        public Point2D CoorRespecto(Point2D p)
        {
            return new Point2D(_x - p._x, _y - p._y);
        }

        public Point2D CompPositivas()
        {
            return new Point2D(Math.Abs(_x), Math.Abs(_y));
        }

        public Point2D Unitario()
        {
            Point2D p = Modulo() != 0 ? Div(Modulo()) : null;
            return p;
        }

        public Double Modulo()
        {
            return Math.Sqrt(Math.Pow(_x, 2) + Math.Pow(_y, 2));
        }

        public Double DistanciaTo(Point2D p)
        {
            return CoorRespecto(p).Modulo();
        }

        public override string ToString()
        {
            return String.Format("({0:0.000}; {1:0.000})", _x, _y);
        }

        public double DotProduct(Point2D p)
        {
            return (_x * p._x + _y * p._y);
        }

        public double AngleWith(Point2D p)
        {
            return Math.Acos(DotProduct(p) / (Modulo() * p.Modulo()));
        }

        #region Nested type: clsCenter

        public class clsCenter : IComparer
        {
            private readonly Point2D centro;

            public clsCenter(Point2D parCentro)
            {
                centro = new Point2D(parCentro);
            }

            #region IComparer Members

            int IComparer.Compare(object x, object y)
            {
                int value;
                var p1 = (Point2D)x;
                var p2 = (Point2D)y;
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