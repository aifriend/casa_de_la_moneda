using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;
using URMAPI;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class RfidReaderFake : RfidReader, IManualsProvider
    {
        public RfidReaderFake(string name)
            : base(name)
        {
        }       

        public override void Dispose()
        {
            if (Connected())
                Disconnect();
        }

        public override bool Connect()
        {
            return true;
        }

        public override bool Disconnect()
        {
            return true;
        }

        public override bool ReadCode(out string code)
        {
            code = "FAKE1234";
            return true;
        }

        public override void Reset()
        {
            if (Connected())
                Disconnect();
            if (Disconnect())
                Connect();
        }

        public override bool Connected()
        {
            return true;
        }

        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            var manual = new Manual(this) {Description = Name};
            return new[] {manual};
        }

        #endregion
    }
}