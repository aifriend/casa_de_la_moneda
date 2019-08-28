using System;
using Idpsa.Control.Tool;

namespace Idpsa.Control.Component
{
    public class Wago_750 : ICounter
    {
        private const int backwardsModeBit = 1;
        private const int CounterValueLenth = 16;
        private const int CounterValueShift = 8;

        private const int lockBit = 4;
        private const int minValue = 0;
        private const int overflowBit = 0;
        private const int setBit = 5;
        private const int setValueLenth = 16;
        private const int setValueShift = 8;

        private readonly int bitImput;
        private readonly int bitOutput;
        private readonly IOCollection dICollection;
        private readonly IOCollection dOCollection;
        private readonly int maxValue = (int)Math.Pow(2, 16) - 1;

        public Wago_750(Address imputAddress, Address outputAddress, Bus bus, string name)
        {
            bitImput = imputAddress.BitAddress();
            bitOutput = outputAddress.BitAddress();
            dICollection = bus.InCollection;
            dOCollection = bus.OutCollection;
        }

        public Wago_750(Address imputAddress, Address outputAddress, Bus bus)
            : this(imputAddress, outputAddress, bus, "")
        {
        }

        public int Name { get; private set; }

        public bool CircularCount
        {
            get { return ReadStatusBit(overflowBit); }

            set { WriteControlBit(overflowBit, value); }
        }

        #region ICounter Members

        public int GetValue()
        {
            int counterValue = 0;

            for (int i = 0; i < CounterValueLenth; i++)
            {
                if (dICollection[bitImput + CounterValueShift + i].Value)
                    counterValue += 1 << i;
            }

            return counterValue;
        }


        public bool SetValue(int counterValue)
        {
            bool flag = ReadStatusBit(setBit);

            if (!flag)
            {
                WriteSetCounterValue(counterValue);
                WriteControlBit(setBit, true);
            }
            else
            {
                WriteControlBit(setBit, false);
            }

            return ((GetValue() == counterValue) && flag);
        }

        public bool StopCounter
        {
            get { return ReadStatusBit(lockBit); }

            set { WriteControlBit(lockBit, value); }
        }

        public bool BackWardsMode
        {
            get { return ReadStatusBit(backwardsModeBit); }

            set { WriteControlBit(backwardsModeBit, value); }
        }

        public int MaxValue
        {
            get { return maxValue; }
        }

        public int MinValue
        {
            get { return minValue; }
        }

        public bool Reset()
        {
            return SetValue(0);
        }

        #endregion

        private bool ReadStatusBit(int bitNumber)
        {
            return dICollection[bitImput + bitNumber].Value;
        }

        private void WriteControlBit(int bitNumber, bool value)
        {
            dOCollection[bitOutput + bitNumber].Value = value;
        }

        private void WriteSetCounterValue(int value)
        {
            for (int i = 0; i < setValueLenth; i++)
            {
                dOCollection[bitOutput + setValueShift + i].Value = (value & (1 << i)).ToBool();
            }
        }
    }
}