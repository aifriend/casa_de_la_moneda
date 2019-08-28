using System;

namespace Idpsa.Control.Component
{
    public interface IReader : IDisposable
    {       
        bool Connected();
        bool Connect();
        bool Disconnect();
        bool BeginRead();
        bool EndRead();
        void Reset();
        bool ReadCode(out string code);
        string LastReadedCode { get; }
        bool Habilitado { get; set; }//MCR. 2015.03.13
    }
}