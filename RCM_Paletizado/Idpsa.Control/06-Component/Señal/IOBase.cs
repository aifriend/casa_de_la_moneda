using System;

namespace Idpsa.Control.Component
{
    [Serializable]
    public abstract class IOBase
    {
        #region TypeIO enum

        public enum TypeIO
        {
            Input,
            Output
        }

        #endregion

        public Address Address { get; set; }
        public IOSignal IOSignal { get; set; }
        public TypeIO Type { get; protected set; }
        public string Symbol { get; set; }
        public string Description { get; set; }

        public abstract bool Value();
        public abstract void PutSet();
        public abstract void PutReset();
    }
}