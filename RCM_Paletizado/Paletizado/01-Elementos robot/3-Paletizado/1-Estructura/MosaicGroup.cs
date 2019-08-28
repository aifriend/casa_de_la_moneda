using System;
using System.Collections.Generic;
using Idpsa.Control.Tool;

namespace Idpsa.Paletizado
{
    public class MosaicGroup
    {
        private readonly Point2D _dimensions;
        private readonly GrowSense _growSense;
        private readonly IPaletizable _item;
        private readonly Point2D _origenGrow;
        private readonly Point2D _separation;
        private readonly Spin _spin;

        public MosaicGroup(Point2D origenGrow, IPaletizable item, Spin spin, Point2D dimensions,
                           GrowSense growSense, Point2D separation)
        {
            _origenGrow = origenGrow;
            _item = item;
            _spin = spin;
            _dimensions = dimensions;
            _separation = separation;
            _growSense = growSense;
            ItemPositions = new List<PointSpin2D>();

            CalculatePositions();
            CalculateRectangle();
        }


        public List<PointSpin2D> ItemPositions { get; private set; }
        public Rectangle Rectangle { get; private set; }

        private void CalculatePositions()
        {
            double Lx = _item.LxWith(_spin);
            double Ly = _item.LyWith(_spin);

            Point2D p_d = _growSense.Transform(new Point2D(Lx, Ly)).Desplazado(_growSense.Transform(_separation));
                //;new Point2D(Lx, Ly).Transform(this._GrowSense).Desplazado(this._vectorSeparador);
            Point2D center = p_d.Div(2);

            for (int i = 0; i <= _dimensions.X - 1; i++)
            {
                for (int j = 0; j <= _dimensions.Y - 1; j++)
                {
                    Point2D point = new Point2D(p_d.X*i + center.X, p_d.Y*j + center.Y).Desplazado(_origenGrow);
                    ItemPositions.Add(new PointSpin2D(_spin, point));
                }
            }
        }

        private void CalculateRectangle()
        {
            var size = new Point2D((_item.LxWith(_spin) + Math.Abs(_separation.X))*
                                   _dimensions.X, (_item.LyWith(_spin) + Math.Abs(_separation.Y))*_dimensions.Y);

            Point2D origen = null;
            switch (_growSense)
            {
                case GrowSense.PosPos:
                    origen = _origenGrow.Desplazado(0, 0);
                    break;
                case GrowSense.NegNeg:
                    origen = _origenGrow.Desplazado(-size.X, -size.Y);
                    break;
                case GrowSense.PosNeg:
                    origen = _origenGrow.Desplazado(0, -size.Y);
                    break;
                case GrowSense.NegPos:
                    origen = _origenGrow.Desplazado(-size.X, 0);
                    break;
            }

            Rectangle = new Rectangle(origen, size);
        }
    }
}