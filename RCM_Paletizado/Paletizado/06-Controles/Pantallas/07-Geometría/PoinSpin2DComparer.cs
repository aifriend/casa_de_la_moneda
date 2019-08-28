using System;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Paletizado.MosaicSorters;

namespace Idpsa.Paletizado
{
    internal class Point2DColum
    {
        public int Column { get; set; }
        public PointSpin2D Point { get; set; }
    }

    [Serializable]
    public enum ComparerMosaicTypes
    {
        Tipe1
    }

    public interface IMosaicSorter
    {
        ComparerMosaicTypes Type { get; }
        List<PointSpin2D> Sort(List<PointSpin2D> points);
    }

    public class MosaicSorter : IMosaicSorter
    {
        private readonly ComparerMosaicTypes _type;

        public MosaicSorter(ComparerMosaicTypes sorterType)
        {
            _type = sorterType;
        }

        #region IMosaicSorter Members

        public ComparerMosaicTypes Type
        {
            get { return _type; }
        }

        public List<PointSpin2D> Sort(List<PointSpin2D> points)
        {
            List<IGrouping<double, PointSpin2D>> groupedPoints = points.GroupBy(p => p.X).OrderBy(g => g.Key).ToList();
            return (from item in groupedPoints
                    from subItem in item
                    select new Point2DColum
                               {
                                   Column = groupedPoints.IndexOf(item),
                                   Point = subItem,
                               }).ToList().OrderBy(e => e, CreateComparer()).Select(e => e.Point).ToList();
        }

        #endregion

        private IComparer<Point2DColum> CreateComparer()
        {
            IComparer<Point2DColum> value = null;
            switch (Type)
            {
                case ComparerMosaicTypes.Tipe1:
                    value = new MosaicComparer1();
                    break;
            }

            return value;
        }
    }

    namespace MosaicSorters
    {
        internal class MosaicComparer1 : IComparer<Point2DColum>
        {
            #region IComparer<Point2DColum> Members

            public int Compare(Point2DColum p1, Point2DColum p2)
            {
                if (p1.Column > p2.Column)
                    return -1;
                else if (p1.Column < p2.Column)
                    return 1;
                else
                {
                    if (p1.Point.Y < p2.Point.Y)
                        return (((p1.Column%2) == 0) ? 1 : - 1);
                    else if (p1.Point.Y > p2.Point.Y)
                        return (((p1.Column%2) == 0) ? -1 : 1);
                    else
                        return 0;
                }
            }

            #endregion
        }
    }
}