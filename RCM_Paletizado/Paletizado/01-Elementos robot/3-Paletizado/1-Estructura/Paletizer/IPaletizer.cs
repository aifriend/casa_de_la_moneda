using System.Collections.Generic;
using Idpsa.Control.Tool;

namespace Idpsa.Paletizado
{
    public interface IPaletizer : IContainer
    {
        string Name { get; set; }
        int CurrentFlat { get; }
        int CurrentMosaicIndex { get; }
        IEnumerable<Mosaic> Mosaics { get; }
        Mosaic CurrentMosaic { get; }
        IPaletizable Item { get; }
        IPalet Palet { get; }
        PaletizerSettings Settings { get; set; }
        Paletizer.States State { get; set; }
        Point3D CenterOverPalet();
        Point3D CenterOverSurface();
        void ElementAdded(IElement element);
        void PaletCentered();
        IElement ElementQuitted();
        bool EmptyPalet();
        PointSpin3D PositionItem();
        PointSpin3D PositionPalet();
        PointSpin3D PositionPutSeparator();
        int RemainingItems();
        int TotalItems();
        int TotalToDoItems();
    }
}