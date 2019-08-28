using System.Collections.Generic;
using Idpsa.Control.Tool;

namespace Idpsa.Paletizado
{
    public partial class Mosaic
    {
        private MosaicGroup AddMosaicGroupCentered(Rectangle rec, Spin spin, Point2D dims,
                                                   GrowSense growSense, Point2D separation)
        {
            var cero = new Point2D(0, 0);
            var grupo = new MosaicGroup(cero, Item, spin, dims, growSense, separation);
            Point2D centro = grupo.Rectangle.OrigenCentradoSobre(rec);
            return AddMosaicGroup(centro, spin, dims, GrowSense.PosPos, separation);
        }

        private MosaicGroup AddMosaicGroupCenteredX(double origenY, Rectangle rec
                                                    , Spin spin, Point2D dims, GrowSense growSense, Point2D separation)
        {
            var cero = new Point2D(0, 0);
            var grupo = new MosaicGroup(cero, Item, spin, dims, growSense, separation);
            Point2D centro = grupo.Rectangle.OrigenCentradoSobre(rec);
            centro = new Point2D(centro.X, origenY);
            return AddMosaicGroup(centro, spin, dims, GrowSense.PosPos, separation);
        }

        private MosaicGroup AddMosaicGroupCenteredY(double origenX, Rectangle rec,
                                                    Spin spin, Point2D dims, GrowSense growSense, Point2D separation)
        {
            var cero = new Point2D(0, 0);
            var grupo = new MosaicGroup(cero, Item, spin, dims, growSense, separation);
            Point2D centro = grupo.Rectangle.OrigenCentradoSobre(rec);
            centro = new Point2D(origenX, centro.Y);
            return AddMosaicGroup(centro, spin, dims, GrowSense.PosPos, separation);
        }

        private MosaicGroup AddMosaicGroupCentered(Rectangle rec, Rectangle.ZonaCentrado Fzona, Point2D dims, Spin spin,
                                                   GrowSense growSense, Point2D separation)
        {
            Rectangle zonaCentrado = Fzona(rec);
            var cero = new Point2D(0, 0);
            var grupo = new MosaicGroup(cero, Item, spin, dims, growSense, separation);
            Point2D centro = grupo.Rectangle.OrigenCentradoSobre(zonaCentrado);
            return AddMosaicGroup(centro, spin, dims, GrowSense.PosPos, separation);
        }


        private MosaicGroup AddMosaicGroup(Point2D origen, Spin spin, Point2D dims,
                                           GrowSense growSense, Point2D separation)
        {
            var grupo = new MosaicGroup(origen, Item, spin, dims, growSense, separation);
            ItemPositions.AddRange(grupo.ItemPositions);
            return grupo;
        }


        private void Sort(IComparer<PointSpin2D> comparer)
        {
            ItemPositions.Sort(comparer);
        }

        private Rectangle Rectangle()
        {
            PointSpin2D pi = ItemPositions[0];
            double dX = 0.5*Item.LxWith(pi.Spin);
            double dY = 0.5*Item.LyWith(pi.Spin);

            double maxX = pi.X + dX;
            double maxY = pi.Y + dY;
            double minX = pi.X - dX;
            double minY = pi.Y - dY;


            foreach (PointSpin2D p in ItemPositions)
            {
                dX = 0.5*Item.LxWith(p.Spin);
                dY = 0.5*Item.LyWith(p.Spin);

                if ((p.X + dX) > maxX)
                {
                    maxX = p.X + dX;
                }
                if ((p.Y + dY) > maxY)
                {
                    maxY = p.Y + dY;
                }
                if ((p.X - dX) < minX)
                {
                    minX = p.X - dX;
                }
                if ((p.Y - dY) < minY)
                {
                    minY = p.Y - dY;
                }
            }

            var po = new Point2D(minX, minY);
            var l = new Point2D(maxX - minX, maxY - minY);

            return new Rectangle(po, l);
        }

        private void CenterMosaic()
        {
            Rectangle rM = Rectangle();
            Point2D pMC = rM.OrigenCentradoSobre(Palet.Base);
            Point2D desp = rM.Origen.CoorRespecto(pMC);
            foreach (PointSpin2D p in ItemPositions)
            {
                p.X -= desp.X;
                p.Y -= desp.Y;
            }
        }

        private void MosaicXReflexion()
        {
            double xCenter = Rectangle().Center.X;
            foreach (PointSpin2D p in ItemPositions)
            {
                p.X += -2*(p.X - xCenter);
                if (p.Spin == Spin.S0)
                    p.Spin = Spin.S180;
                else if (p.Spin == Spin.S180)
                    p.Spin = Spin.S0;
            }
        }

        private void MosaicYReflexion()
        {
            double yCenter = Rectangle().Center.Y;
            foreach (PointSpin2D p in ItemPositions)
            {
                p.Y += -2*(p.Y - yCenter);
                if (p.Spin == Spin.S90)
                    p.Spin = Spin.S270;
                else if (p.Spin == Spin.S270)
                    p.Spin = Spin.S90;
            }
        }


        private void CenterMosaicX()
        {
            Rectangle rM = Rectangle();
            Point2D pMC = rM.OrigenCentradoSobre(Palet.Base);
            Point2D desp = rM.Origen.CoorRespecto(pMC);
            foreach (PointSpin2D p in ItemPositions)
            {
                p.X -= desp.X;
            }
        }

        private void CenterMosaicY()
        {
            Rectangle rM = Rectangle();
            Point2D pMC = rM.OrigenCentradoSobre(Palet.Base);
            Point2D desp = rM.Origen.CoorRespecto(pMC);
            foreach (PointSpin2D p in ItemPositions)
            {
                p.Y -= desp.Y;
            }
        }

        private void DesplazarMosaico(Point2D desp)
        {
            foreach (PointSpin2D p in ItemPositions)
            {
                p.X += desp.X;
                p.Y += desp.Y;
            }
        }

        public Point2D MosaicDimensions()
        {
            Rectangle r = Rectangle();
            Point2D l = r.Lados;
            return new Point2D(l.X, l.Y);
        }
    }
}