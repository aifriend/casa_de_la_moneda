using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using Idpsa.Control.Subsystem;

namespace Idpsa.Control.Component
{   
    public class SocketSender<TSource,USend>: IManagerRunnable,IDisposable
        where TSource: class
        where USend : class 
    {        
        private readonly object _lock = new Object();
        private readonly Socket _sender;        
        private readonly IPAddress _ipAddress;
        private readonly int _port;
        private readonly int _bufferCapacity;
        private volatile bool _wait;
        private SocketSerializer<USend> _serializer;
        private Func<TSource,USend> _transformation;
        public event EventHandler<DataEventArgs<USend>> DataSend;
        
        public SocketSender(IPAddress ipAddress,int port,Func<TSource,USend> transformation, int bufferCapacity)
        {
            if (ipAddress == null)
                throw new ArgumentNullException("ipAddress");
            if (port < 0)
                throw new ArgumentException("port number can't be negative");
            if (transformation == null)
                throw new ArgumentException("transformation");

            if (bufferCapacity <= 0)
                throw new ArgumentNullException("bufferCapacity must be greather than zero"); 

            _sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _ipAddress = ipAddress;
            _port = port;
            _transformation = transformation;
            _bufferCapacity = bufferCapacity;
        }

        public SocketSender(IPAddress ipAddress, int port, Func<TSource, USend> transformation)
            : this(ipAddress, port, transformation, 2 * 2048) { }
                      
        public bool TrySetDataToSend(TSource data)
        {
            var value = _transformation(data);
            lock (_lock)
            {
                if (!HasDataToSend)
                {
                    _serializer = new SocketSerializer<USend>(value,_bufferCapacity);
                    return true;
                }
            }
            return false;
        }

        protected virtual void OnDataSend(USend data)
        {
            var temp = DataSend;
            if (temp != null)
            {
                temp(this, new DataEventArgs<USend>(data));
            }
        }

        public bool HasDataToSend
        {
            get
            {
                lock (_lock)
                {
                    return _serializer != null;
                }
            }
        }

        private void ClearData()
        {
            lock(_lock)
            {
                _serializer = null;
            }
        }
        
        /// <summary>
        /// Starts the client and attempts to send an object to the server
        /// </summary>
        private void Manager()
        {
            if (HasDataToSend && !_wait)
            {
                _wait = true;
                _sender.BeginConnect(new IPEndPoint(_ipAddress, _port), BeginSend, _sender);
            }
        }

        /// <summary>
        /// Starts when the connection was accepted by the remote hosts and prepares to send data
        /// </summary>        
        private void BeginSend(IAsyncResult result)
        {
            _serializer.Socket = (Socket)result.AsyncState; 
            _serializer.Socket.EndConnect(result);            
            byte[] buffer = _serializer.Serialize(); //fills the buffer with data
            _serializer.Socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, Send, _serializer);
        }
                      
        private void Send(IAsyncResult result)
        {
            var serializer = (SocketSerializer<USend>)result.AsyncState;
            int size = serializer.Socket.EndSend(result);
            OnDataSend(_serializer.Data);
            ClearData();
            serializer.Socket.Disconnect(true);
            _wait = false;
        }

        #region Miembros de IManagerRunable

        IEnumerable<Action> IManagerRunnable.GetManagers()
        {
            return new Action[]{Manager};
        }

        #endregion

        #region Miembros de IDisposable

        void IDisposable.Dispose()
        {
            if (_sender != null)            
               _sender.Close();            
        }

        #endregion
    }

}
