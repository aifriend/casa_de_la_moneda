using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Idpsa.Control.Component
{
    public class ServerSocket
    {
        #region private vars.

        private static readonly ASCIIEncoding _aSCIIEncoding = new ASCIIEncoding();
        private readonly bool _exit;
        private readonly TcpListener _server;
        private readonly Thread _thread;
        private TcpClient _client;
        private NetworkStream _stream;

        #endregion

        #region delegados

        public delegate void DataReceivedHandler(object data);

        #endregion

        #region eventos

        public event DataReceivedHandler DataReceived;

        #endregion

        /// <summary>
        /// Constructor parametrizado de la clase. Inicializa los atributos y lanza
        /// el hilo de conexion.
        /// </summary>
        public ServerSocket(bool _exit)
        {
            try
            {
                // Get DNS host information.
                IPHostEntry hostInfo = Dns.GetHostEntry(Environment.MachineName);
                // Get the DNS IP addresses associated with the host.
                IPAddress[] IPaddresses = hostInfo.AddressList;

                _server = new TcpListener(IPaddresses[0], 6969);
                _server.Start();

                //Lanzamos tantos hilos como puertos hay a la escucha
                _thread = new Thread(doConexion);
                _thread.Start();
            }
            catch (Exception)
            {
            } //cambiar
            this._exit = _exit;
        }

        /// <summary>
        /// Acepta conexiones de entrada
        /// </summary>
        private void doConexion()
        {
            while (true)
            {
                try
                {
                    _client = _server.AcceptTcpClient();
                    _stream = _client.GetStream();

                    const string data = "Conectado.";
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    _stream.Write(msg, 0, msg.Length);
                    Listener();
                }
                catch (Exception)
                {
                    Thread.Sleep(500); //si hay un error el el establecimiento de la conexión esperamos
                }
            }
        }

        /// <summary>
        /// Hilo de escucha. Recibe datos via socket
        /// </summary>
        private void Listener()
        {
            string inMsg;
            while (!_exit)
            {
                try
                {
                    var inStream = new Byte[256];
                    try
                    {
                        _stream.Read(inStream, 0, inStream.Length);
                        inMsg = _aSCIIEncoding.GetString(inStream, 0, inStream.Length);
                        inMsg = inMsg.Substring(0, inMsg.IndexOf('\0'));
                        Console.WriteLine("Datos recibidos: " + inMsg);
                        OnDataReceived(inMsg);
                    }
                    catch (Exception) //no puede leer por lo que se ha perdido la conexión
                    {
                        Console.WriteLine("Se ha perdido la conexion.");
                        break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Fallo en la conexión.");
                }
            }
        }

        /// <summary>
        /// Envia datos via sockets
        /// </summary>
        /// <param name="data">Cadena de datos que se envia</param>
        public bool SendData(string data)
        {
            byte[] text = Encoding.ASCII.GetBytes(data);
            try
            {
                _stream.Flush();
                _stream.Write(text, 0, text.Length);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Cierra los sockects abiertos y elimina los hilos de escucha
        /// </summary>
        public void CloseSockets()
        {
            if (_client != null)
                _client.Close();
            _server.Stop();
            _thread.Abort();
        }

        /// <summary>
        /// Evento generado por la clase al recibir datos por el socket.
        /// </summary>
        /// <param name="data">Los datos leidos.</param>
        private void OnDataReceived(object data)
        {
            if (DataReceived != null)
            {
                DataReceived(data);
            }
        }
    }
}