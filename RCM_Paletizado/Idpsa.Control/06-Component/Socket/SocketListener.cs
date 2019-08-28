using System;
using System.Net.Sockets;
using System.Net;

namespace Idpsa.Control.Component
{
    public sealed class SocketListener<TSource, TUSend> : IDisposable
        where TSource : class
        where TUSend : class
    {
        private readonly object _lock = new Object();
        private Socket _listener;
        private readonly IPAddress _ipAddress;
        private readonly int _port;
        private readonly int _bufferedConexions;
        private TSource _lastDataReceived;
        private readonly Func<TUSend, TSource> _transformation;
        private volatile bool _waitToRead;
        public bool WaitForAcceptedSocket { get; private set; }
        private Socket _acceptedSocket;
        public event EventHandler<DataEventArgs<TSource>> DataReceived;

        private SocketListener(IPAddress ipAddress, int port,
            Func<TUSend, TSource> transformation, int bufferedConexions)
        {
            if (ipAddress == null)
                throw new ArgumentNullException("ipAddress");
            if (port < 0)
                throw new ArgumentException("port number can't be negative");
            if (transformation == null)
                throw new ArgumentException("transformation");

            if (bufferedConexions <= 0)
                throw new ArgumentNullException("conexionsBuffered must be greather than zero");

            _transformation = transformation;
            _bufferedConexions = bufferedConexions;
            _ipAddress = ipAddress;
            _port = port;
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(new IPEndPoint(_ipAddress, _port));
            _listener.Listen(_bufferedConexions);
        }

        public SocketListener(IPAddress ipAddress, int port, Func<TUSend, TSource> transformation)
            : this(ipAddress, port, transformation, 5) { }

        public bool TryGetData(out TSource data)
        {
            data = null;
            lock (_lock)
            {
                if (_lastDataReceived != null)
                {
                    data = _lastDataReceived;
                    _lastDataReceived = null;
                    return true;
                }
                return false;
            }
        }

        private void OnDataReceived(TSource data)
        {
            if (DataReceived != null)
                DataReceived(this, new DataEventArgs<TSource>(data));
        }

        public bool IsConnected()
        {
            try
            {
                return
                    !(_serializer != null &&
                      _serializer.Socket.Poll(1, SelectMode.SelectRead) &&
                      _serializer.Socket.Available == 0);
            }
            catch (SocketException) { return false; }
        }

        private SocketSerializer<TUSend> _serializer;

        /// <summary>
        /// Server needs to keep alive in order to listen for connections.
        /// </summary>
        public void Manager()
        {
            // Has client
            if (_acceptedSocket == null && !WaitForAcceptedSocket)
            {
                WaitForAcceptedSocket = true;
                _listener.BeginAccept(Accept, _listener);
            }

            // Wait for data to receive
            if (_acceptedSocket != null && !WaitForAcceptedSocket && !_waitToRead)
            {
                _waitToRead = true;
                try
                {
                    _serializer = new SocketSerializer<TUSend> { Socket = _acceptedSocket };
                    _serializer.Socket.BeginReceive(_serializer.Buffer,
                                                    0,
                                                    _serializer.Buffer.Length,
                                                    SocketFlags.None,
                                                    Receive,
                                                    _serializer);
                }
                catch
                {
                    _waitToRead = false;
                    _acceptedSocket = null;
                }
            }

            // Check if connected
            if (!_waitToRead || IsConnected()) return;
            _waitToRead = false;
            _acceptedSocket = null;
            WaitForAcceptedSocket = false;
        }

        /// <summary>
        /// Starts when an incomming connection was requested
        /// </summary>            
        private void Accept(IAsyncResult result)
        {
            try
            {
                var serializer = new SocketSerializer<TUSend>();
                _acceptedSocket = serializer.Socket = ((Socket)result.AsyncState).EndAccept(result);
                WaitForAcceptedSocket = false;
            }
            catch
            {
                _waitToRead = false;
            }
        }

        /// <summary>
        /// Receives the data, puts it in a buffer and checks if we need to receive again.
        /// </summary>            
        private void Receive(IAsyncResult result)
        {
            try
            {
                 var serializer = (SocketSerializer<TUSend>)result.AsyncState;
                var read = serializer.Socket.EndReceive(result);
                if (read > 0)
                {
                    for (var i = 0; i < read; i++)
                    {
                        serializer.TransmissionBuffer.Add(serializer.Buffer[i]);
                    }
                }
                Done(serializer);
            }
            catch (Exception ex)
            {
                _waitToRead = false;
            }
        }

        /// <summary>
        /// Deserializes and outputs the received object
        /// </summary>            
        private void Done(SocketSerializer<TUSend> data)
        {
            if (data.TransmissionBuffer.Count > 0)
            {
                var send = data.DeSerialize();
                var g = _transformation(send);
                SetLastReceivedData(g);
            }
            _waitToRead = false;
        }

        private void SetLastReceivedData(TSource data)
        {
            lock (_lock)
            {
                _lastDataReceived = data;
            }
            OnDataReceived(data);
        }

        public void Connect()
        {
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(new IPEndPoint(_ipAddress, _port));
            _listener.Listen(_bufferedConexions);
            _waitToRead = false;
            _acceptedSocket = null;
            WaitForAcceptedSocket = false;
        }

        public void Disconnect()
        {
            _listener.Close();
        }

        #region Miembros de IDisposable

        void IDisposable.Dispose()
        {
            if (_listener != null)
            {
                _listener.Close();
            }
        }

        #endregion
    }
}