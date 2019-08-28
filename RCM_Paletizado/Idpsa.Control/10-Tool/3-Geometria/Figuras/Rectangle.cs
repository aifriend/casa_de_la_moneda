using System;

namespace Idpsa.Control.Tool
{
    public enum Corners
    {
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }


    [Serializable]
    public class Rectangle
    {
        #region Delegates

        public delegate Rectangle ZonaCentrado(Rectangle rec);

        #endregion

        private Point2D _lados;
        private Point2D _origen;


        public Rectangle(Point2D parOrigen, Point2D parLados)
        {
            _lados = new Point2D(parLados);
            _origen = new Point2D(parOrigen);
        }

        public Rectangle(Point2D parOrigen, double parLx, double parLy)
        {
            _lados = new Point2D(parLx, parLy);
            _origen = new Point2D(parOrigen);
        }

        public Rectangle(double origenX, double origenY, double parLx, double parLy)
        {
            _origen = new Point2D(origenX, origenY);
            _lados = new Point2D(parLx, parLy);
        }

        public Rectangle(double parLx, double parLy)
        {
            _origen = new Point2D(0, 0);
            _lados = new Point2D(parLx, parLy);
        }

        public Rectangle(CornerPoint2D cornerPoint, Point2D parLados)
        {
            _lados = new Point2D(parLados);
            _origen = GetOriginCorner(cornerPoint, _lados);
        }

        public Rectangle(CornerPoint2D cornerPoint, double parLx, double parLy)
        {
            _lados = new Point2D(parLx, parLy);
            _origen = GetOriginCorner(cornerPoint, _lados);
        }

        public Point2D Origen
        {
            get { return new Point2D(_origen); }
            set { _origen = value; }
        }

        public Point2D Center
        {
            get { return _origen.Desplazado(_lados.X / 2, _lados.Y / 2); }
        }

        public Point2D Lados
        {
            get { return new Point2D(_lados); }
            set { _lados = value; }
        }

        public Point2D CornerUpL()
        {
            return new Point2D(_origen);
        }

        public Point2D CornerUpR()
        {
            return new Point2D(_origen.X + _lados.X, _origen.Y);
        }

        public Point2D CornerDownL()
        {
            return new Point2D(_origen.X, _origen.Y + _lados.Y);
        }

        public Point2D CornerDownR()
        {
            return new Point2D(_origen.X + _lados.X, _origen.Y + _lados.Y);
        }

        public Rectangle ZoneUp(Rectangle rec)
        {
            var Orig = new Point2D(Origen.X, rec.Origen.Y);
            var Opuesta = new Point2D(CornerUpR());
            Point2D Ls = Opuesta.CoorRespecto(Orig);
            return new Rectangle(Orig, Ls);
        }

        public Rectangle ZoneUpL(Rectangle rec)
        {
            var Orig = new Point2D(rec.Origen);
            var Opuesta = new Point2D(CornerUpL());
            Point2D Ls = Opuesta.CoorRespecto(Orig);
            return new Rectangle(Orig, Ls);
        }

        public Rectangle ZoneUpR(Rectangle rec)
        {
            var Orig = new Point2D(CornerUpR().X, rec.Origen.Y);
            var Opuesta = new Point2D(rec.CornerUpR().X, CornerUpR().Y);
            Point2D Ls = Opuesta.CoorRespecto(Orig);
            return new Rectangle(Orig, Ls);
        }

        public Rectangle ZoneL(Rectangle rec)
        {
            var Orig = new Point2D(rec.Origen.X, Origen.Y);
            var Opuesta = new Point2D(CornerDownL());
            Point2D Ls = Opuesta.CoorRespecto(Orig);
            return new Rectangle(Orig, Ls);
        }

        public Rectangle ZoneR(Rectangle rec)
        {
            var Orig = new Point2D(CornerUpR());
            var Opuesta = new Point2D(rec.CornerUpR().X, CornerDownR().Y);
            Point2D Ls = Opuesta.CoorRespecto(Orig);
            return new Rectangle(Orig, Ls);
        }

        public Rectangle ZoneDown(Rectangle rec)
        {
            var Orig = new Point2D(CornerDownL());
            var Opuesta = new Point2D(CornerDownR().X, rec.CornerDownR().Y);
            Point2D Ls = Opuesta.CoorRespecto(Orig);
            return new Rectangle(Orig, Ls);
        }

        public Rectangle ZoneDownL(Rectangle rec)
        {
            var Orig = new Point2D(rec.Origen.X, CornerDownL().Y);
            var Opuesta = new Point2D(CornerDownL().X, rec.CornerDownL().Y);
            Point2D Ls = Opuesta.CoorRespecto(Orig);
            return new Rectangle(Orig, Ls);
        }

        public Rectangle ZoneDownR(Rectangle rec)
        {
            var Orig = new Point2D(CornerDownR());
            var Opuesta = new Point2D(CornerDownR().X, rec.CornerDownR().Y);
            Point2D Ls = Opuesta.CoorRespecto(Orig);
            return new Rectangle(Orig, Ls);
        }

        public Rectangle Expandido(Point2D Dist)
        {
            var l = new Point2D(Lados.X + Dist.X, Lados.Y + Dist.Y);
            var rec = new Rectangle(new Point2D(0, 0), l);
            Point2D Orig = rec.OrigenCentradoSobre(this);
            return new Rectangle(Orig, l);
        }

        public Rectangle Reducido(Point2D Dist)
        {
            var l = new Point2D(Lados.X - Dist.X, Lados.Y - Dist.Y);
            var rec = new Rectangle(new Point2D(0, 0), l);
            Point2D Orig = rec.OrigenCentradoSobre(this);
            return new Rectangle(Orig, l);
        }

        public Point2D OrigenCentradoSobre(Rectangle rec)
        {
            double DespX = (rec.Lados.X - Lados.X) / 2;
            double DespY = (rec.Lados.Y - Lados.Y) / 2;
            var value = new Point2D(rec.Origen.X + DespX, rec.Origen.Y + DespY);
            return value;
        }

        public Rectangle CentrarEn(Point2D point)
        {
            var origin = new Point2D(point.X - Lados.X / 2, point.Y - Lados.Y);
            return new Rectangle(origin, Lados);
        }

        public bool IntersectaCon(Rectangle rec)
        {
            bool value;
            Rectangle MinXRec;
            Rectangle NoMinXRec;
            Rectangle NoMinYRec;
            if (Origen.X <= rec.Origen.X)
            {
                MinXRec = this;
                NoMinXRec = rec;
            }
            else
            {
                MinXRec = rec;
                NoMinXRec = this;
            }
            NoMinYRec = Origen.Y <= rec.Origen.Y ? rec : this;
            if ((NoMinXRec.Origen.X <= MinXRec.Origen.X + MinXRec.Lados.X) &&
                (NoMinYRec.Origen.Y <= MinXRec.Origen.Y + MinXRec.Lados.Y))
            {
                value = true;
            }
            else
            {
                value = false;
            }
            return value;
        }

        public Rectangle Escalar(Point2D Escala)
        {
            return new Rectangle(Origen.Escalar(Escala), Lados.Escalar(Escala));
        }

        public bool ContainsPoint(Point2D p)
        {
            bool value = false;
            if (p.X >= Origen.X && p.X <= Origen.X + Lados.X && p.Y >= Origen.Y && p.Y <= Origen.Y + Lados.Y)
            {
                value = true;
            }
            return value;
        }

        public Rectangle GetCopy()
        {
            return new Rectangle(_origen.X, _origen.Y, _lados.X, _lados.Y);
        }

        protected static Point2D GetOriginCorner(CornerPoint2D cornerPoint, Point2D parLados)
        {
            Point2D p = null;


            switch (cornerPoint.Corner)
            {
                case Corners.UpLeft:
                    p = cornerPoint.Desplazado(0, 0);
                    break;
                case Corners.UpRight:
                    p = cornerPoint.Desplazado(-parLados.X, 0);
                    break;
                case Corners.DownLeft:
                    p = cornerPoint.Desplazado(0, -parLados.Y);
                    break;
                case Corners.DownRight:
                    p = cornerPoint.Desplazado(-parLados.X, -parLados.Y);
                    break;
            }

            return p;
        }
    }
}