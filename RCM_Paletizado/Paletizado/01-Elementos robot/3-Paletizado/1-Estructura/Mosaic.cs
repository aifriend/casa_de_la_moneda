using System;
using System.Collections;
using System.Collections.Generic;
using Idpsa.Control.Tool;

namespace Idpsa.Paletizado
{
    [Serializable]
    public enum MosaicType
    {
        Mosaico1,
        Mosaico1Cruzado,
        MosaicoPrueba,
        MosaicoPruebaCruzado,
        Mosaico25,
        Mosaico20,
        Mosaico10,
        Mosaico20Girado, //MDG.2011-03-14
        Mosaico10Girado, //MDG.2011-03-14
        MosaicoPrueba1Caja, //MDG.2011-03-22
        MosaicoPrueba2Cajas, //MDG.2011-03-24
        Mosaico23, //MDG.2011-04-07
        Mosaico23Cruzado, //MDG.2011-04-07
        Mosaico8 //MDG.2011-04-07
    }

    [Serializable]
    public enum MosaicInitialPosition
    {
        Start,
        End
    }

    [Serializable]
    public partial class Mosaic : IEnumerable<PointSpin2D>
    {
        public int pos;

        private Mosaic()
        {
            pos = 0;
            ItemPositions = new List<PointSpin2D>();
        }

        public Mosaic(IPaletizable item, IPalet palet, MosaicType type)
            : this()
        {
            NewMosaic(item, palet, type, MosaicInitialPosition.Start);
        }

        public Mosaic(IPaletizable item, IPalet palet, MosaicType type, MosaicInitialPosition initialPositon)
            : this()
        {
            NewMosaic(item, palet, type, initialPositon);
        }

        public IPaletizable Item { get; private set; }
        public IPalet Palet { get; private set; }
        public MosaicType Type { get; private set; }

        public List<PointSpin2D> ItemPositions { get; private set; }

        #region IEnumerable<PointSpin2D> Members

        public IEnumerator<PointSpin2D> GetEnumerator()
        {
            foreach (PointSpin2D p in ItemPositions)
                yield return p;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public static MosaicType StringToTipoMosaico(string name)
        {
            return (MosaicType) Enum.Parse(typeof (MosaicType), name.Trim(), true);
        }

        public void NewMosaic(IPaletizable item, IPalet palet, MosaicType type, MosaicInitialPosition initialPositon)
        {
            Item = item;
            Palet = palet;
            Type = type;
            ItemPositions.Clear();
            CreateMosaic();
            switch (initialPositon)
            {
                case MosaicInitialPosition.Start:
                    pos = 0;
                    break;
                case MosaicInitialPosition.End:
                    pos = ItemPositions.Count - 1;
                    break;
            }
        }

        private void CreateMosaic()
        {
            var comparerType = (ComparerMosaicTypes) (GetType().GetMethod(Type.ToString()).Invoke(this, null));
            IMosaicSorter mosaicSorter = new MosaicSorter(comparerType);
            ItemPositions = mosaicSorter.Sort(ItemPositions);
        }

        public List<PointSpin2D> Positions()
        {
            return ItemPositions;
        }


        public PointSpin2D Position()
        {
            return ItemPositions[pos];
        }

        public PointSpin2D PreviousPosition()
        {
            if (pos > 0)
                return ItemPositions[pos - 1];
            else
                return null;
        }

        public PointSpin2D NextPosition()
        {
            if (pos < TotalItems() - 1)
            {
                return ItemPositions[pos + 1];
            }
            return null;
        }

        public int CurrentIndexItem()
        {
            return pos;
        }

        public Spin CurrentItemSpin()
        {
            return (ItemPositions[pos].Spin);
        }

        public bool CheckNextElement()
        {
            return pos < ItemPositions.Count - 1;
        }

        public bool NextElement()
        {
            bool value = pos < ItemPositions.Count - 1;
            if (value)
            {
                pos++;
            }
            else
            {
                pos = 0;
            }

            return value;
        }

        public bool NextElementExist()
        {
            bool value = pos < ItemPositions.Count - 1;

            return value;
        }

        public bool PreviousElement()
        {
            bool value = pos >= 1;
            if (value)
            {
                pos--;
            }
            else
            {
                pos = ItemPositions.Count - 1;
            }

            return value;
        }


        public int TotalItems()
        {
            return ItemPositions.Count;
        }

        public int RemainingItems()
        {
            return ItemPositions.Count - pos;
        }

        public ComparerMosaicTypes Mosaico1()
        {
            MosaicGroup g = AddMosaicGroupCenteredY(0.0, Palet.Base, Spin.S180, new Point2D(2, 5), GrowSense.PosPos,
                                                    new Point2D(4, 26));

            var p = new Point2D(g.Rectangle.Origen.X, g.Rectangle.Center.Y);


            AddMosaicGroup(p, Spin.S90, new Point2D(5, 2), GrowSense.NegPos, new Point2D(4, 4));

            AddMosaicGroup(p, Spin.S270, new Point2D(5, 2), GrowSense.NegNeg, new Point2D(4, 4));

            CenterMosaic();

            return ComparerMosaicTypes.Tipe1;
        }

        public ComparerMosaicTypes Mosaico1Cruzado()
        {
            ComparerMosaicTypes comparer = Mosaico1();
            MosaicXReflexion();
            return comparer;
        }

        public ComparerMosaicTypes MosaicoPrueba()
        {
            MosaicGroup g = AddMosaicGroupCenteredX(0.0, Palet.Base, Spin.S0, new Point2D(2, 2), GrowSense.PosPos,
                                                    new Point2D(4, 4));
            //Point2D p = g.Rectangle.CornerUpR();
            //AddMosaicGroup(p, Spin.S90, new Point2D(2, 2), GrowSense.PosPos, new Point2D(4, 4));
            CenterMosaic();
            return ComparerMosaicTypes.Tipe1;
        }

        public ComparerMosaicTypes MosaicoPruebaCruzado()
        {
            ComparerMosaicTypes comparer = MosaicoPrueba();
            //MosaicXReflexion();
            return comparer;
        }

        public ComparerMosaicTypes Mosaico25()
        {
            MosaicGroup g = AddMosaicGroupCenteredY(0.0, Palet.Base, Spin.S180, new Point2D(1, 5), GrowSense.PosPos,
                                                    new Point2D(4, 26));

            var p = new Point2D(g.Rectangle.Origen.X, g.Rectangle.Center.Y);


            AddMosaicGroup(p, Spin.S90, new Point2D(5, 2), GrowSense.NegPos, new Point2D(4, 4));

            AddMosaicGroup(p, Spin.S270, new Point2D(5, 2), GrowSense.NegNeg, new Point2D(4, 4));

            CenterMosaic();

            MosaicXReflexion();
            return ComparerMosaicTypes.Tipe1;
        }

        public ComparerMosaicTypes Mosaico20()
        {
            MosaicGroup g = AddMosaicGroupCenteredY(0.0, Palet.Base, Spin.S180, new Point2D(1, 5), GrowSense.PosPos,
                                                    new Point2D(5, 5));

            Point2D p1 = g.Rectangle.CornerUpR();

            g = AddMosaicGroup(p1.Desplazado(0, 0), Spin.S180, new Point2D(1, 5), GrowSense.PosPos, new Point2D(5, 5));

            Point2D p2 = g.Rectangle.CornerUpR();

            g = AddMosaicGroup(p2.Desplazado(0, 0), Spin.S180, new Point2D(1, 5), GrowSense.PosPos, new Point2D(5, 5));

            Point2D p3 = g.Rectangle.CornerUpR();

            AddMosaicGroup(p3.Desplazado(0, 0), Spin.S180, new Point2D(1, 5), GrowSense.PosPos, new Point2D(5, 5));

            CenterMosaic();

            MosaicXReflexion();
            return ComparerMosaicTypes.Tipe1;
        }

        //MDG.2011-06-02.Sindespalazmiento de 10 entre filas para que quede más apelmazado

        //public ComparerMosaicTypes Mosaico20()
        //{
        //    MosaicGroup g = AddMosaicGroupCenteredY(0.0, Palet.Base, Spin.S180, new Point2D(1, 5), GrowSense.PosPos,
        //      new Point2D(5, 5));

        //    Point2D p1 = g.Rectangle.CornerUpR();

        //    g = AddMosaicGroup(p1.Desplazado(10,0), Spin.S180, new Point2D(1, 5), GrowSense.PosPos, new Point2D(5, 5));

        //    Point2D p2 = g.Rectangle.CornerUpR();

        //    g = AddMosaicGroup(p2.Desplazado(10, 0), Spin.S180, new Point2D(1, 5), GrowSense.PosPos, new Point2D(5, 5));

        //    Point2D p3 = g.Rectangle.CornerUpR();

        //    AddMosaicGroup(p3.Desplazado(10, 0), Spin.S180, new Point2D(1, 5), GrowSense.PosPos, new Point2D(5, 5));

        //    CenterMosaic();

        //    MosaicXReflexion();
        //    return ComparerMosaicTypes.Tipe1;
        //}

        public ComparerMosaicTypes Mosaico10()
        {
            MosaicGroup g1 = AddMosaicGroupCenteredX(0.0, Palet.Base, Spin.S270, new Point2D(5, 1), GrowSense.PosPos,
                                                     new Point2D(5, 5));

            Point2D p1 = g1.Rectangle.CornerDownL();

            MosaicGroup g2 = AddMosaicGroup(p1.Desplazado(0, 10), Spin.S90, new Point2D(5, 1), GrowSense.PosPos,
                                            new Point2D(5, 5));

            CenterMosaic();

            return ComparerMosaicTypes.Tipe1;
        }


        public ComparerMosaicTypes Mosaico20Girado() //MDG.2011-03-14.Mosaico para capas palet Vikex
        {
            //MosaicGroup g = AddMosaicGroupCenteredY(0.0, Palet.Base, Spin.S180, new Point2D(2, 5), GrowSense.PosPos,
            //   new Point2D(4, 26));

            //Point2D p = new Point2D(g.Rectangle.Origen.X, g.Rectangle.Center.Y);


            //AddMosaicGroup(p, Spin.S90, new Point2D(5, 2), GrowSense.NegPos, new Point2D(4, 4));

            MosaicGroup g1 = AddMosaicGroupCenteredX(0.0, Palet.Base, Spin.S90, new Point2D(5, 2), GrowSense.NegPos,
                                                     new Point2D(4, 4));

            //Point2D p1 = g1.Rectangle.CornerDownL();
            Point2D p1 = g1.Rectangle.CornerUpR();

            AddMosaicGroup(p1, Spin.S270, new Point2D(5, 2), GrowSense.NegNeg, new Point2D(4, 4));


            //MosaicGroup g1 = AddMosaicGroupCenteredX(0.0, Palet.Base, Spin.S270, new Point2D(5, 1), GrowSense.PosPos,
            //  new Point2D(5, 5));

            //Point2D p1 = g1.Rectangle.CornerDownL();

            //MosaicGroup g2 = AddMosaicGroup(p1.Desplazado(0, 10), Spin.S90, new Point2D(5, 1), GrowSense.PosPos, new Point2D(5, 5));

            CenterMosaic();

            return ComparerMosaicTypes.Tipe1;

            ////MosaicGroup g = AddMosaicGroupCenteredX(0.0, Palet.Base, Spin.S90, new Point2D(1, 4), GrowSense.PosPos,
            ////  new Point2D(5, 5));

            ////Point2D p1 = g.Rectangle.CornerUpR();

            ////g = AddMosaicGroup(p1.Desplazado(10, 0), Spin.S90, new Point2D(1, 4), GrowSense.PosPos, new Point2D(5, 5));

            ////Point2D p2 = g.Rectangle.CornerUpR();

            ////g = AddMosaicGroup(p2.Desplazado(10, 0), Spin.S90, new Point2D(1, 4), GrowSense.PosPos, new Point2D(5, 5));

            ////Point2D p3 = g.Rectangle.CornerUpR();

            ////AddMosaicGroup(p3.Desplazado(10, 0), Spin.S90, new Point2D(1, 4), GrowSense.PosPos, new Point2D(5, 5));

            ////Point2D p4 = g.Rectangle.CornerUpR();

            ////AddMosaicGroup(p4.Desplazado(10, 0), Spin.S90, new Point2D(1, 4), GrowSense.PosPos, new Point2D(5, 5));


            ////CenterMosaic();

            ////MosaicXReflexion();
            ////return ComparerMosaicTypes.Tipe1;

            //MosaicGroup g = AddMosaicGroupCenteredY(0.0, Palet.Base, Spin.S90, new Point2D(2, 4), GrowSense.PosPos,
            //    new Point2D(5, 5));


            ////Point2D p = new Point2D(g.Rectangle.Origen.X, g.Rectangle.Center.Y);

            //Point2D p1 = g.Rectangle.CornerUpR();

            //AddMosaicGroup(p1, Spin.S270, new Point2D(3, 4), GrowSense.PosPos, new Point2D(4, 4));

            ////AddMosaicGroup(p, Spin.S90, new Point2D(5, 2), GrowSense.NegPos, new Point2D(4, 4));

            ////AddMosaicGroup(p, Spin.S270, new Point2D(5, 2), GrowSense.NegNeg, new Point2D(4, 4));

            //CenterMosaic();

            //return ComparerMosaicTypes.Tipe1;
        }

        public ComparerMosaicTypes Mosaico10Girado() //MDG.2011-03-14.Mosaico para capas palet Vikex
        {
            //MosaicGroup g1 = AddMosaicGroupCenteredX(0.0, Palet.Base, Spin.S180, new Point2D(1, 5), GrowSense.PosPos,
            //  new Point2D(5, 5));

            //Point2D p1 = g1.Rectangle.CornerDownL();

            //MosaicGroup g2 = AddMosaicGroup(p1.Desplazado(0, 10), Spin.S0, new Point2D(1, 5), GrowSense.PosPos, new Point2D(5, 5));

            //CenterMosaic();

            //return ComparerMosaicTypes.Tipe1;

            //MDG.2011-03-18.Mosaico 10 girado debe tener angulo de 0 grados, no de 180, porque la pinza no alcanza este giro
            //MosaicGroup g = AddMosaicGroupCenteredY(0.0, Palet.Base, Spin.S0, new Point2D(2, 5), GrowSense.PosPos,
            //    new Point2D(5, 5));


            //MosaicGroup g = AddMosaicGroupCenteredY(0.0, Palet.Base, Spin.S180, new Point2D(2, 5), GrowSense.PosPos,
            //    new Point2D(5, 5));

            //Point2D p = new Point2D(g.Rectangle.Origen.X, g.Rectangle.Center.Y);


            //AddMosaicGroup(p, Spin.S90, new Point2D(5, 2), GrowSense.NegPos, new Point2D(4, 4));

            //AddMosaicGroup(p, Spin.S270, new Point2D(5, 2), GrowSense.NegNeg, new Point2D(4, 4));

            //MDG.2011-03-24.Con 180 grados. Como Mosaico1 Preparado para mosaic.XReflexion
            MosaicGroup g = AddMosaicGroupCenteredY(0.0, Palet.Base, Spin.S180, new Point2D(1, 5), GrowSense.PosPos,
                                                    new Point2D(5, 5));

            Point2D p1 = g.Rectangle.CornerUpR();

            g = AddMosaicGroup(p1.Desplazado(10, 0), Spin.S180, new Point2D(1, 5), GrowSense.PosPos, new Point2D(5, 5));

            CenterMosaic();

            MosaicXReflexion(); //MDG.2011-03-24.Para que gire los 180 grados

            return ComparerMosaicTypes.Tipe1;
        }

        //MDG.2011-03-22.Mosaico para prueba deposicion solo 1 caja
        public ComparerMosaicTypes MosaicoPrueba1Caja()
        {
            MosaicGroup g = AddMosaicGroupCenteredY(0.0, Palet.Base, Spin.S0, new Point2D(1, 1), GrowSense.PosPos,
                                                    new Point2D(5, 5));

            CenterMosaic();

            return ComparerMosaicTypes.Tipe1;
        }

        //MDG.2011-03-24.Mosaico para prueba deposicion solo 2 cajas
        public ComparerMosaicTypes MosaicoPrueba2Cajas()
        {
            MosaicGroup g = AddMosaicGroupCenteredY(0.0, Palet.Base, Spin.S0, new Point2D(1, 2), GrowSense.PosPos,
                                                    new Point2D(5, 5));

            CenterMosaic();

            return ComparerMosaicTypes.Tipe1;
        }

        //MDG.2011-07-04.Para nuevo paletizado 100CajasPaletConsularContrapeado
        public ComparerMosaicTypes Mosaico23()
        {
            //Grupo de 1 x 3 (un grupo de 1x2 y un grupo de 1)

            MosaicGroup g = AddMosaicGroupCenteredX(0.0, Palet.Base, Spin.S90, new Point2D(1, 1), GrowSense.NegPos,
                                                    new Point2D(0, 0));

            Point2D p1 = g.Rectangle.CornerUpR();

            //g=AddMosaicGroup(p1.Desplazado(0,-30), Spin.S270, new Point2D(1, 2), GrowSense.NegNeg, new Point2D(0, 55));
            g = AddMosaicGroup(p1.Desplazado(0, -30), Spin.S270, new Point2D(1, 2), GrowSense.NegNeg, new Point2D(0, 65));

            Point2D p2 = g.Rectangle.CornerUpR();

            //Grupo de 4 x 5 (4 grupos de 1x5)

            //g = AddMosaicGroup(p2.Desplazado(5, 30), Spin.S180, new Point2D(1, 5), GrowSense.PosPos, new Point2D(5, 5));
            g = AddMosaicGroup(p2.Desplazado(0, 30), Spin.S180, new Point2D(1, 5), GrowSense.PosPos, new Point2D(5, 5));

            Point2D p3 = g.Rectangle.CornerUpR();

            g = AddMosaicGroup(p3.Desplazado(0, 0), Spin.S180, new Point2D(1, 5), GrowSense.PosPos, new Point2D(5, 5));

            Point2D p4 = g.Rectangle.CornerUpR();

            g = AddMosaicGroup(p4.Desplazado(0, 0), Spin.S180, new Point2D(1, 5), GrowSense.PosPos, new Point2D(5, 5));

            Point2D p5 = g.Rectangle.CornerUpR();

            g = AddMosaicGroup(p5.Desplazado(0, 0), Spin.S180, new Point2D(1, 5), GrowSense.PosPos, new Point2D(5, 5));

            CenterMosaic();

            MosaicXReflexion();
            return ComparerMosaicTypes.Tipe1;
        }

        //MDG.2011-07-04.Para nuevo paletizado 100CajasPaletConsularContrapeado
        public ComparerMosaicTypes Mosaico23Cruzado()
        {
            //Grupo de 1 x 3 (un grupo de 1x2 y un grupo de 1)

            MosaicGroup g = AddMosaicGroupCenteredX(0.0, Palet.Base, Spin.S90, new Point2D(1, 1), GrowSense.NegPos,
                                                    new Point2D(0, 0));

            Point2D p1 = g.Rectangle.CornerUpR();

            //g = AddMosaicGroup(p1.Desplazado(0, -30), Spin.S270, new Point2D(1, 2), GrowSense.NegNeg, new Point2D(0, 55));
            g = AddMosaicGroup(p1.Desplazado(0, -30), Spin.S270, new Point2D(1, 2), GrowSense.NegNeg, new Point2D(0, 65));

            Point2D p2 = g.Rectangle.CornerUpR();

            //Grupo de 4 x 5 (4 grupos de 1x5)

            //g = AddMosaicGroup(p2.Desplazado(5, 30), Spin.S0, new Point2D(1, 5), GrowSense.PosPos, new Point2D(5, 5));
            g = AddMosaicGroup(p2.Desplazado(0, 30), Spin.S0, new Point2D(1, 5), GrowSense.PosPos, new Point2D(5, 5));
            //MDG.2011-07-06

            Point2D p3 = g.Rectangle.CornerUpR();

            g = AddMosaicGroup(p3.Desplazado(0, 0), Spin.S0, new Point2D(1, 5), GrowSense.PosPos, new Point2D(5, 5));

            Point2D p4 = g.Rectangle.CornerUpR();

            g = AddMosaicGroup(p4.Desplazado(0, 0), Spin.S0, new Point2D(1, 5), GrowSense.PosPos, new Point2D(5, 5));

            Point2D p5 = g.Rectangle.CornerUpR();

            g = AddMosaicGroup(p5.Desplazado(0, 0), Spin.S0, new Point2D(1, 5), GrowSense.PosPos, new Point2D(5, 5));

            CenterMosaic();

            //MosaicXReflexion();
            return ComparerMosaicTypes.Tipe1;
        }

        //MDG.2011-07-04.Para nuevo paletizado 100CajasPaletConsularContrapeado
        public ComparerMosaicTypes Mosaico8()
        {
            MosaicGroup g1 = AddMosaicGroupCenteredX(0.0, Palet.Base, Spin.S90, new Point2D(4, 1), GrowSense.NegPos,
                                                     new Point2D(4, 4));

            Point2D p1 = g1.Rectangle.CornerUpR();

            AddMosaicGroup(p1, Spin.S270, new Point2D(4, 1), GrowSense.NegNeg, new Point2D(4, 4));


            CenterMosaic();

            return ComparerMosaicTypes.Tipe1;
        }

        public void ForcedFirstItem()
        {
            pos = 0;
        }
        public void ForcedLastPosition()
        {
            pos = Positions().Count-1;
        }
    }
}