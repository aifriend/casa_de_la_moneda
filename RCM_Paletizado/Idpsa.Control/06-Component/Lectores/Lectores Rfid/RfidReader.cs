using System;

namespace Idpsa.Control.Component
{
    [Serializable]
    public abstract class RfidReader : IReader 
    {
        protected RfidReader(string name)
        {
            Name = name;
            Habilitado = true;//MCR. 2015.03.13
        }

        protected string Name { get; private set; }
        public bool Habilitado { get; set; }//MCR. 2015.03.13

        #region IReader Members

        public string LastReadedCode { get; protected set; }

        public abstract bool Connected();

        public abstract bool Connect();

        public abstract bool Disconnect();

        /// <summary>
        /// Lee el codigo RFID.
        /// </summary>
        /// <param name="code">Parametro de salida. Contiene el codigo leido.</param>
        /// <returns>TRUE: si ha leido codigo RFID. 
        ///          FALSE: en caso contrario</returns>
        public abstract bool ReadCode(out string code);

        public abstract void Dispose();

        public abstract void Reset();

        #endregion

        public virtual void ResetLastErrorCode()
        {
            LastReadedCode = "";
        }


        public bool BeginRead()
        {
            throw new NotImplementedException();
        }

        public bool EndRead()
        {
            throw new NotImplementedException();
        }

    }
}