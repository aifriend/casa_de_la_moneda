using System;

namespace Idpsa.Control.Component
{
    public interface ISocket : IDisposable
    {       
        bool Connected();
        bool Connect();
        bool Disconnect();
        bool BeginListening();
        bool EndListening();
        void Reset();
        bool ReadMsg(out string code);
        string LastMsgRead { get; }
    }
}