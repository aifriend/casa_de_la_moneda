using System;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Input : IOBase, IEvaluable

    {
        private bool _fResetValue;
        private bool _fSetValue;
        private bool _value;

        private static readonly Input _simulated
            = new Input {Symbol = "Simulated",IOSignal = new IOSignal()};

        public static Input Simulated
        {
            get { return _simulated; }
        } 

        public Input()
        {
            Type = TypeIO.Input;
        }

        public bool FSet
        {
            get { return _fSetValue; }
            set
            {
                if (FReset)
                {
                    FReset = false;
                }
                _fSetValue = value;
            }
        }

        public bool FReset
        {
            get { return _fResetValue; }
            set
            {
                if (FSet)
                {
                    FSet = false;
                }
                _fResetValue = value;
            }
        }

        #region IEvaluable Members

        public override bool Value()
        {
            if (!FSet & !FReset)
            {
                _value = IOSignal.Value;
            }
            else
            {
                if (FSet)
                {
                    _value = true;
                }
                else if (FReset)
                {
                    _value = false;
                }
            }
            return _value;
        }

        #endregion

        public override void PutSet()
        {
            throw new NotSupportedException("the function PutSet can't be used with an input");            
        }

        public override void PutReset()
        {
            throw new NotSupportedException("the function PutReset can't be used with an input");            
        }

        public override string ToString()
        {
            return Symbol;
        }
    }
}