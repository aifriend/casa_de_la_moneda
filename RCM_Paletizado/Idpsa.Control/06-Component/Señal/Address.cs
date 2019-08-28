using System;

namespace Idpsa.Control
{
    [Serializable]
    public struct Address
    {
        private readonly int _byte;   
        private readonly int _bit;  

        public Address(int @byte, int bit)
        {
            if (@byte < 0)
                    throw new ArgumentException("@byte, A Address byte direction can't be negative");
            if (bit < 0 || bit > 7)
                throw new ArgumentOutOfRangeException("bit, a Address bit it's only defined in [0,7]");
            _byte = @byte;
            _bit  = bit;
        }

        public Address(int @byte) 
            : this(@byte, 0){}

        public Address(Address value)
            : this(value.Byte, value.Bit) { }

        public int Byte
        {
            get { return _byte; }           
        } 

        public int Bit
        {
            get { return _bit; }           
        }      

        public int BitAddress()
        {
            return (8 * Byte + Bit);
        }

        public static bool operator ==(Address left, Address right)
        {
            return left.BitAddress() == right.BitAddress();
        }
        public static bool operator !=(Address left, Address right)
        {
            return !(left == right);
        }
        public static bool operator >(Address left, Address right)
        {
            return left.BitAddress() > right.BitAddress(); 
        }
        public static bool operator <(Address left, Address right)
        {
            return left.BitAddress() < right.BitAddress(); 
        }
        public static bool operator >=(Address left, Address right)
        {
            return left.BitAddress() >= right.BitAddress();
        }
        public static bool operator <=(Address left, Address right)
        {
            return left.BitAddress() <= right.BitAddress();
        }
        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj,null))
                return false;
            if (!(obj is Address))
                return false;

            return (((Address)obj).BitAddress() == this.BitAddress());
        }
        public override int GetHashCode()
        {
            return BitAddress().GetHashCode(); 
        }
    }
}