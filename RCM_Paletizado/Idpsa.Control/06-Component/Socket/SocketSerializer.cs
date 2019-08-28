using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Idpsa.Control.Component
{
    internal class SocketSerializer<T> where T : class
    {        
        public Socket Socket;        
        public readonly List<byte> TransmissionBuffer = new List<byte>();        
        public readonly byte[] Buffer;
        public T Data { get; private set; }

        public SocketSerializer(T data, int bufferCapacity)
        {           
            if (bufferCapacity <= 0)
                throw new ArgumentException("bufferCapacity must be greather than zero");
            Data = data;
            Buffer = new byte[bufferCapacity];
        }

        public SocketSerializer(T data)
            : this(data, 2 * 2048) { }

        public SocketSerializer(int bufferCapacity)
            : this(null, bufferCapacity) { }

        public SocketSerializer()
            : this(null) { }

        public byte[] Serialize()
        {
            var bin = new BinaryFormatter();
            var mem = new MemoryStream();
            bin.Serialize(mem, Data);
            return mem.GetBuffer();
        }

        public T DeSerialize()
        {
            var dataBuffer = TransmissionBuffer.ToArray();
            var bin = new BinaryFormatter();
            var mem = new MemoryStream();
            mem.Write(dataBuffer, 0, dataBuffer.Length);
            mem.Seek(0, 0);
            return (T)bin.Deserialize(mem);
        }
    }
}