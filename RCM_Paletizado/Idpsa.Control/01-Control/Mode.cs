namespace Idpsa.Control
{
    public struct Mode
    {
        private readonly int _value;

        private Mode(int value)
        {
            _value = value;
        }
        
        public static readonly Mode Special = new Mode(40);
        public static readonly Mode Automatic = new Mode(30);
        public static readonly Mode BackToOrigin = new Mode(20);
        public static readonly Mode Manual = new Mode(10);
        public static readonly Mode WithoutMode = new Mode(0);        

        public override string  ToString()
        {
            if (this == WithoutMode)
                return "Sin modo";
            else if (this == Manual)
                return "Manual";
            else if (this == Automatic)
                return "Automatico";
            else if (this == BackToOrigin)
                return "Vuelta Origen";
            else if (this == Special)
                return "Especial";
            else 
                return "Desconocido";
        }

        public static bool operator ==(Mode left, Mode rigth)
        {
            return (left._value == rigth._value);
        }

        public static bool operator !=(Mode left, Mode rigth)
        {
            return !(left == rigth);
        }

        public override bool Equals(object obj)
        {          
            if (object.ReferenceEquals(obj,null))
                return false;                

            if (!(obj is Mode))
                return false;

            return (this == (Mode)obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

    }
}