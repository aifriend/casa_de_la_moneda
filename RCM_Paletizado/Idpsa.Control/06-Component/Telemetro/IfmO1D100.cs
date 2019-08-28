using System;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class IfmO1D100
    {
        private const decimal Factor = 2143 / 27030;
        private readonly int _bitInputs;
        private readonly IOCollection _dICollection = new IOCollection();
        private readonly Address _inputs;
        private decimal _distancia;


        public IfmO1D100(Address inputs, IOCollection dICollection, string name)
        {
            Name = name;
            _inputs = inputs;
            _dICollection = dICollection;
            _bitInputs = (_inputs.Byte * 8) + _inputs.Bit;
        }

        public IfmO1D100(Address inputs, IOCollection dICollection) : this(inputs, dICollection, "")
        {
        }
            
        public string Name { get; private set; }
        
        public int Distance()
        {
            int value1 = 0;
            int value2 = 0;
            int i;
            for (i = 0; i <= 7; i++)
            {
                if (_dICollection[8 + i].Value)
                {
                    value1 += (int)Math.Pow(2, i);
                }
                if (_dICollection[16 + i].Value)
                {
                    value2 += (int)Math.Pow(2, i);
                }
            }
            value1 = (int)(value1 * Math.Pow(2, 8) + value2);
            value1 = (int)(Factor * value1);
            if (value1 != 0)
            {
                _distancia = value1;
            }
            return (int)_distancia;
        }
    }
}