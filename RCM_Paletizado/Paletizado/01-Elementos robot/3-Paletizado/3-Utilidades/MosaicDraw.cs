using System;
using System.Collections.Generic;
using System.Drawing;
using Idpsa.Control.Tool;

namespace Idpsa.Paletizado
{
    public static class MosaicDraw
    {
        public static void Erase(System.Windows.Forms.Control region)
        {
            using (Graphics G = region.CreateGraphics())
            {
                G.PageUnit = GraphicsUnit.Pixel;
                G.Clear(Color.White);
            }
        }

        public static void DrawMosaic(this Mosaic mosaic, System.Windows.Forms.Control region, Spin rotation)
        {
            var palet = (IPalet) mosaic.Palet.Clone();
            var item = (IPaletizable) mosaic.Item.Clone();
            MosaicType mosaicType = mosaic.Type;
            DrawMosaic(region, item, palet, mosaicType, rotation);
        }

        public static void DrawMosaicDynamic(this Mosaic mosaic, bool addingElements,
                                             System.Windows.Forms.Control region, Spin rotation)
        {
            var palet = (IPalet) mosaic.Palet.Clone();
            var item = (IPaletizable) mosaic.Item.Clone();
            MosaicType mosaicType = mosaic.Type;
            int pos = mosaic.CurrentIndexItem();
            DrawMosaicDynamic(region, item, palet, mosaicType, pos, Spin.S270,false);
        }

        public static void DrawMosaic(System.Windows.Forms.Control region, IPaletizable item,
                                      IPalet palet, MosaicType mosaicType, Spin rotation)
        {
            Graphics G = region.CreateGraphics();
            G.PageUnit = GraphicsUnit.Pixel;
            G.Clear(Color.White);
            G.ScaleTransform(region.Size.Width/1400.0F, region.Size.Height/1400.0F);


            if (rotation == Spin.S270)
            {
                G.RotateTransform(-90);
                G.TranslateTransform(-1400, 0);
            }
            else if (rotation == Spin.S90)
            {
                throw new NotImplementedException();
            }
            else if (rotation == Spin.S180)
            {
                throw new NotImplementedException();
            }


            palet.Base.Origen = new Point2D((1400 - palet.Base.Lados.X)/2, (1400 - palet.Base.Lados.Y)/2);
            var M = new Mosaic(item, palet, mosaicType, MosaicInitialPosition.Start);
            var rec = new RectangleF[M.TotalItems()];
            var lines = new List<Pair<Point2D, Point2D>>();

            int i = 0;
            foreach (PointSpin2D point in M)
            {
                rec[i++] = new RectangleF((float) ((point.X - (item.LxWith(point.Spin)/2))),
                                          (float) ((point.Y - (item.LyWith(point.Spin)/2))),
                                          (float) (item.LxWith(point.Spin)), (float) (item.LyWith(point.Spin)));
                lines.Add(CalculateLabelPoints(point, item));
            }

            Brush brocha = null;
            Pen lapiz = null;

            //Palet  
            lapiz = Pens.LightBlue;
            brocha = Brushes.BurlyWood;
            G.FillRectangle(brocha, (float) (palet.Base.Origen.X), (float) (palet.Base.Origen.Y),
                            (float) (palet.Base.Lados.X), (float) (palet.Base.Lados.Y));

            //Cajas
            brocha = Brushes.DarkBlue;
            G.FillRectangles(brocha, rec);

            //Etiqueta            
            lapiz = new Pen(Color.Red, 2);
            foreach (var v in lines)
            {
                G.DrawLine(lapiz, (float) v.Value1.X, (float) v.Value1.Y, (float) v.Value2.X, (float) v.Value2.Y);
            }
        }


        public static void DrawMosaicDynamic(System.Windows.Forms.Control region, IPaletizable item,
                                             IPalet palet, MosaicType mosaicType, int pos, Spin rotation, bool reverse)
        {
            using (Graphics G = region.CreateGraphics())
            {
                Brush brushFirstElements = Brushes.Blue;
                Brush brushLastElements = Brushes.Black;
                Brush brushPalet = Brushes.BurlyWood;

                G.PageUnit = GraphicsUnit.Pixel;
                G.Clear(Color.White);
                G.ScaleTransform(region.Size.Width/1400.0F, region.Size.Height/1400.0F);

                if (rotation == Spin.S270)
                {
                    G.RotateTransform(-90);
                    G.TranslateTransform(-1400, 0);
                }
                else if (rotation == Spin.S90)
                {
                    throw new NotImplementedException();
                }
                else if (rotation == Spin.S180)
                {
                    throw new NotImplementedException();
                }

                palet.Base.Origen = new Point2D((1400 - palet.Base.Lados.X)/2, (1400 - palet.Base.Lados.Y)/2);
                var M = new Mosaic(item, palet, mosaicType, MosaicInitialPosition.Start);
                var rec = new RectangleF[M.TotalItems()];
                var lines = new List<Pair<Point2D, Point2D>>();

                int i = 0;
                if (reverse)
                {
                    foreach (PointSpin2D point in M)
                    {
                        i++;
                        rec[M.TotalItems()-i] = new RectangleF((float)((point.X - (item.LxWith(point.Spin) / 2))),
                          (float)((point.Y - (item.LyWith(point.Spin) / 2))),
                          (float)(item.LxWith(point.Spin)), (float)(item.LyWith(point.Spin)));
                        lines.Add(CalculateLabelPoints(point, item));
                    }
                    lines.Reverse();
                }
                else
                foreach (PointSpin2D point in M)
                {
                    rec[i++] = new RectangleF((float) ((point.X - (item.LxWith(point.Spin)/2))),
                                              (float) ((point.Y - (item.LyWith(point.Spin)/2))),
                                              (float) (item.LxWith(point.Spin)), (float) (item.LyWith(point.Spin)));
                    lines.Add(CalculateLabelPoints(point, item));
                }


                var paletRectangle = new RectangleF((float) (palet.Base.Origen.X), (float) (palet.Base.Origen.Y),
                                                    (float) (palet.Base.Lados.X), (float) (palet.Base.Lados.Y));
                G.FillRectangle(brushPalet, paletRectangle);

                //Cajas
                if (pos > 0)
                {
                    G.FillRectangles(brushFirstElements, rec.ToArray(0, pos + 1));
                }

                G.FillRectangles(Brushes.Gold, rec.ToArray(pos, 1));
                int totalItems = M.TotalItems();
                if (pos < totalItems - 1)
                {
                    G.FillRectangles(brushLastElements, rec.ToArray(pos + 1, totalItems - pos - 1));
                }

                //Etiqueta
                using (var lapiz = new Pen(Color.Red, 2))
                {
                    foreach (var v in lines)
                    {
                        G.DrawLine(lapiz, (float) v.Value1.X, (float) v.Value1.Y, (float) v.Value2.X, (float) v.Value2.Y);
                    }
                }
            }
        }


        private static Pair<Point2D, Point2D> CalculateLabelPoints(PointSpin2D p,
                                                                   IPaletizable item)
        {
            p = new PointSpin2D(p.Spin, p);
            Point2D direction = p.Direction().Escalar(-1, 1);
            double translateModul = 0.9*item.Dimensions.Length/2;
            Point2D translation = direction.Mult(translateModul);
            Point2D equidistant = p.Desplazado(translation);
            var value = new Pair<Point2D, Point2D>();
            double distance = 0.7*item.Dimensions.Width/2;
            Point2D normalDirection = p.NormalDirection();
            Point2D direction1 = normalDirection.Mult(distance);
            Point2D direction2 = normalDirection.Mult(-distance);
            value.Value1 = equidistant.Desplazado(direction1);
            value.Value2 = equidistant.Desplazado(direction2);
            return value;
        }
    }
}