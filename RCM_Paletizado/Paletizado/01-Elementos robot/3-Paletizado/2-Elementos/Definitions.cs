using System;
using Idpsa.Control.Tool;

namespace Idpsa.Paletizado
{

    #region Estructuras

    [Serializable]
    public struct Dimensions
    {
        public double Height;
        public double Length;
        public double Width;

        public Dimensions(double lenght, double width, double height)
        {
            Length = lenght;
            Width = width;
            Height = height;
        }
    }

    #endregion

    #region Enumeraciones

    [Serializable]
    public enum ElementTypes
    {
        Palet,
        Item,
        ItemJaponesa,
        ItemAlemana,
        Separator,
        Paletizer
    }

    [Serializable]
    public enum PaletizableTypes
    {
        box
    }

    [Serializable]
    public enum PaletTypes
    {
        EuroPalet,
        PaletConsular,
        PaletVikex
    }

    //MDG.2011-03-14. Introducido palet Vikex
    [Serializable]
    public enum SeparatorTypes
    {
        Carton
    }

    [Serializable]
    public enum GrowSense
    {
        PosPos,
        NegNeg,
        PosNeg,
        NegPos
    }

    #endregion

    #region Interfaces

    public interface IElement
    {
        ElementTypes GeneralType { get; }
    }

    public interface IPaletElement : IElement
    {
        string IDPalet { get; set; }
        int Flat { get; set; }
        int Pos { get; set; }
    }

    public interface IPaletizable : ICloneable
    {
        PaletizableTypes Type { get; }
        Dimensions Dimensions { get; }

        double LxWith(Spin spin);
        double LyWith(Spin spin);
    }


    public interface IPalet : IElement, ICloneable
    {
        PaletTypes Type { get; }
        double Height { get; }
        Rectangle Base { get; }
    }

    public interface ISeparator : IElement, ICloneable
    {
        SeparatorTypes Type { get; }
        double Thickness { get; }
        Rectangle Base { get; }
    }

    [Serializable]
    public class PaletizadoElements
    {
        private static GeometriaConf geo = new GeometriaConf(); //MCR

        public static IPaletizable Create(PaletizableTypes paletizableType)
        {
            Paletizable value = null;
            switch (paletizableType)
            {
                case PaletizableTypes.box:
                    value = new Paletizable(paletizableType, geo.conf.boxMeasures.X, geo.conf.boxMeasures.Y, geo.conf.boxMeasures.Z);
                    break;
            }

            return value;
        }

        public static IPalet Create(PaletTypes paletType)
        {
            Palet value = null;
            switch (paletType)
            {
                case PaletTypes.EuroPalet:
                    value = new Palet(paletType, 1200, 800, 140);
                    break;
                case PaletTypes.PaletConsular:
                    value = new Palet(paletType, 890, 700, 148); //MDG.2012-06-07.Los palets son mas bajos//155);
                    break;
                case PaletTypes.PaletVikex:
                    //value = new Palet(paletType, 700, 780, 105);//MDG.2011-03-14.Dimensiones. Pueden ser 740x800x105
                    value = new Palet(paletType, 740, 800, 105); //MDG.2011-03-14.Dimensiones. Pueden ser 740x800x105
                    break;
            }

            return value;
        }

        public static ISeparator Create(SeparatorTypes separatorType)
        {
            Separator value = null;
            switch (separatorType)
            {
                case SeparatorTypes.Carton:
                    value = new Separator(separatorType, 0, 0, 0);
                    break;
            }

            return value;
        }

        #region Nested type: Palet

        [Serializable]
        private class Palet : IPalet
        {
            private readonly Rectangle _base;
            private readonly double _height;
            private readonly PaletTypes _type;

            public Palet(PaletTypes type, double lx, double ly, double height)
            {
                _type = type;
                _height = height;
                _base = new Rectangle(lx, ly);
            }

            #region Miembros de ICloneable

            public object Clone()
            {
                return new Palet(_type, _base.Lados.X, _base.Lados.Y, _height);
            }

            #endregion

            #region IPalet Members

            public ElementTypes GeneralType
            {
                get { return ElementTypes.Palet; }
            }

            public PaletTypes Type
            {
                get { return _type; }
            }

            public double Height
            {
                get { return _height; }
            }

            public Rectangle Base
            {
                get { return _base; }
            }

            #endregion
        }

        #endregion

        #region Nested type: Paletizable

        [Serializable]
        private class Paletizable : IPaletizable
        {
            private readonly Dimensions _dimensions;
            private readonly PaletizableTypes _type;

            public Paletizable(PaletizableTypes type, Dimensions dimensions)
            {
                _type = type;
                _dimensions = dimensions;
            }

            public Paletizable(PaletizableTypes type
                               , double lenght, double width, double height)
            {
                _type = type;
                _dimensions = new Dimensions(lenght, width, height);
            }

            #region IPaletizable Members

            public PaletizableTypes Type
            {
                get { return _type; }
            }

            public Dimensions Dimensions
            {
                get { return _dimensions; }
            }

            public double LxWith(Spin spin)
            {
                double value;
                if (spin == Spin.S0 || spin == Spin.S180)
                    value = _dimensions.Length;
                else
                    value = _dimensions.Width;

                return value;
            }

            public double LyWith(Spin spin)
            {
                double value;
                if (spin == Spin.S90 || spin == Spin.S270)
                    value = _dimensions.Length;
                else
                    value = _dimensions.Width;

                return value;
            }

            #endregion

            #region Miembros de ICloneable

            public object Clone()
            {
                return new Paletizable(_type, _dimensions);
            }

            #endregion
        }

        #endregion

        #region Nested type: Separator

        [Serializable]
        private class Separator : ISeparator
        {
            private readonly Rectangle _base;
            private readonly double thickness;
            private readonly SeparatorTypes type;

            public Separator(SeparatorTypes type, double lx, double ly, double thickness)
            {
                this.type = type;
                this.thickness = thickness;
                _base = new Rectangle(lx, ly);
            }

            #region Miembros de ICloneable

            public object Clone()
            {
                return new Separator(type, _base.Lados.X, _base.Lados.Y, thickness);
            }

            #endregion

            #region ISeparator Members

            public ElementTypes GeneralType
            {
                get { return ElementTypes.Separator; }
            }

            public SeparatorTypes Type
            {
                get { return type; }
            }

            public double Thickness
            {
                get { return thickness; }
            }

            public Rectangle Base
            {
                get { return _base; }
            }

            #endregion
        }

        #endregion
    }

    #endregion
}