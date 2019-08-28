using System;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Output : IOBase, IActivable
    {
        private bool _fResetValue;
        private bool _fSetValue;
        private bool _value;

        private static readonly Output _simulated
           = new Output { Symbol = "Simulated",IOSignal = new IOSignal()};

        public static Output Simulated
        {
            get { return _simulated; }
        } 

        public Output()
        {
            Type = TypeIO.Output;
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
                if (value)
                {
                    IOSignal.Value = true;
                }
                else
                {
                    IOSignal.Value = false;
                    _value = false;
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
                    FSet = false;

                IOSignal.Value = false;

                _fResetValue = value;
            }
        }

        #region IActivable Members

        public override bool Value()
        {
            if (FSet != true & FReset != true)
            {
                _value = IOSignal.Value;
            }
            else
            {
                if (FSet)
                {
                    _value = true;
                }
                if (FReset)
                {
                    _value = false;
                }
            }
            return _value;
        }

        public void Activate(bool work)
        {
            if (work)
            {
                PutSet();
            }
            else
            {
                PutReset();
            }
        }

        #endregion

        public override void PutSet()
        {
            //lanzar una señal a la tarjeta
            if (FSet != true & FReset != true)
            {
                IOSignal.Value = true;
                _value = IOSignal.Value;
            }
            else
            {
                _value = false;
            }
        }

        public override void PutReset()
        {
            if (FSet != true & FReset != true)
            {
                IOSignal.Value = false;
                _value = IOSignal.Value;
            }
            else
            {
                _value = false;
            }
        }

        public override string ToString()
        {
            return Symbol;
        }
    }
}