using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class SimulatedReader : IReader, IManualsProvider
    {
        private bool _connected;
        private Func<string> _read;

        public SimulatedReader(string name)
        :this(name,()=>""){}
        public bool Habilitado { get; set; }//MCR. 2015.03.13

        public SimulatedReader(string name, Func<string> read)
        {
            Habilitado = true;//MCR. 2015.03.13
            if (String.IsNullOrEmpty(name))            
                throw new ArgumentException("name can't be null or empty");

            if (read == null)
                throw new ArgumentNullException("name");
            
            Name = name;
            _read = read;
            _connected = false;
        }

        public string Name { get; private set; }

        #region IReader Members

        public bool Connect()
        {
            _connected = true;
            return true;
        }

        public bool Disconnect()
        {
            _connected = false;
            return true;
        }

        public bool ReadCode(out string code)
        {
            code = "1234";
            return true;
        }

        public string LastReadedCode { get; private set; }

        public bool Connected()
        {
            return _connected;
        }

        public void Dispose()
        {
        }

        public void Reset()
        {
        }

        #endregion

        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            var manual = new Manual(this) {Description = Name};
            return new[] {manual};
        }

        #endregion

        #region Miembros de IReader


        public bool BeginRead()
        {
            return true;
        }

        public bool EndRead()
        {
            LastReadedCode = _read();
            return true;
        }

        #endregion
    }
}