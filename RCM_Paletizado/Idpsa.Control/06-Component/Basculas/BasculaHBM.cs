using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Engine;
using Idpsa.Control.Manuals;

namespace IdpsaControl
{
    [Serializable]
    public class BasculaHbm : IRestorable, IReader, IDataNotifierProvider<string>, IManualsProvider
    {
        #region Delegates

        public delegate void DataObtainedHandler(string data);

        public delegate void WeightObtainedHandler(double weigth);

        #endregion

        private readonly ASCIIEncoding _ascii = new ASCIIEncoding();
        private readonly DataNotifier<string> _dataNotifier;

        [field: NonSerialized] private SerialPort _serialPort;
        private bool _waitingWeight;
        public bool Habilitado { get; set; }//MCR. 2015.03.13

        public BasculaHbm(string name, int port)
        {
            _name = name;
            _port = port;
            Initialize();
            _dataNotifier = new DataNotifier<string>(name);
            TimeOut = 20;
            Habilitado = true;//MCR. 2015.03.13
        }

        private readonly int _port;
        private string _endMesage;
        private string _name;
        private double _weight;
        private bool Configurated { get; set; }
        private int TimeOut { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
            _serialPort.Dispose();
        }

        #endregion

        #region IRestorable Members

        public void Restore()
        {
            Initialize();
        }

        #endregion

        #region Miembros de IDataNotifierProvider<string>

        public IDataNotifier<string> GetDataNotifier()
        {
            return _dataNotifier;
        }

        #endregion

        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            var manual = new Manual(this) { Description = _dataNotifier.NotifierName };

            return new[] {manual};
                //Añade la báscula a los dispositivos que se pueden controlar manualmente por pantalla. No cambio,sólo comentario.  2011-03-08.
        }

        #endregion

        #region Miembros de IReader

        public bool Connected()
        {
            return _serialPort.IsOpen;
        }

        public bool Connect()
        {
            if (!_serialPort.IsOpen)
                _serialPort.Open();

            return _serialPort.IsOpen;
        }

        public bool Disconnect()
        {
            if (_serialPort.IsOpen)
                _serialPort.Close();

            return !_serialPort.IsOpen;
        }

        public void Reset()
        {
            if (_serialPort.IsOpen)
                _serialPort.Close();
            else if (!_serialPort.IsOpen)
                _serialPort.Open();
        }

        public bool BeginRead()
        {
            var value = false;

            if (!_serialPort.IsOpen)
                _serialPort.Open();

            if (_serialPort.IsOpen)
            {
                _serialPort.DiscardInBuffer();
                _serialPort.ReceivedBytesThreshold = 13;
                _serialPort.Write("P\r\n"); // ("MSV?;");
                _waitingWeight = true;
                _weight = 0D;
                value = true;
            }

            return value;            
        }

        public bool EndRead()
        {
            return !_waitingWeight;
        }

        public bool ReadCode(out string code)
        {
            Configurate();
            code = null;
            if (MesureWeight())
            {
                code = _weight.ToString();
                return true;
            }
            return false;
        }

        string IReader.LastReadedCode
        {
            get { return _weight.ToString(); }
        }

        #endregion

        public event WeightObtainedHandler WeightObtained;
        public event DataObtainedHandler DataObtained;

        private void Initialize()
        {
            _waitingWeight = false;
            _endMesage = _ascii.GetString(new byte[] {13, 10});
            _serialPort = new SerialPort
                              {
                                  BaudRate = 19200,
                                  Parity = Parity.None,
                                  DataBits = 8,
                                  StopBits = StopBits.One,
                                  Handshake = Handshake.None,
                                  Encoding = Encoding.ASCII
                              };
            _serialPort.DataReceived += DataRecived;
            _serialPort.ReadBufferSize = 20;
            _serialPort.WriteBufferSize = 20;
            var portNames = new List<string>(SerialPort.GetPortNames());
            try
            {
                if (portNames.Contains("COM" + _port))
                {
                    _serialPort.PortName = "COM" + _port;
                }
            } 
            catch (Exception ex)
            {
                throw new Exception(String.Format("El puerto {0}, no está disponible", _port));
            }
        }

        private void SetToZero()
        {
            if (!_serialPort.IsOpen)
                _serialPort.Open();

            _serialPort.Write("Z\r\n"); // ("CDL?;");
        }

        private bool BeginConfiguration()
        {
            Configurated = false;
            if (!_serialPort.IsOpen)
                _serialPort.Open();

            if (_serialPort.IsOpen)
            {
                _serialPort.ReceivedBytesThreshold = 6;
                //sP.DiscardInBuffer();
                //ALV _serialPort.Write("COF3;");
                Configurated = true;
            }

            return Configurated;
        }

        private bool EndConfiguration()
        {
            return !Configurated;
        }

        public bool Configurate()
        {
            BeginConfiguration();
            var timer = new TON();
            while (!timer.Timing(TimeOut))
            {
                if (!EndConfiguration())
                    return true;
            }
            return false;
        }

        private bool MesureWeight()
        {
            BeginRead();
            var timer = new TON();
            while (!timer.Timing(1000)) ;
            return EndRead();
        }

        private void DataRecived(object sender, SerialDataReceivedEventArgs e)
        {
            string msg = _serialPort.ReadExisting();

            OnDataObtained(msg);
            //ALV Temporal: a ver que pasa
            //if (msg.Contains("0\r\n0\r\n")) {
            //    Configurated = false;
            //    msg = msg.Replace("0\r\n0\r\n", "");
            //}

            if (msg.EndsWith(_endMesage))
            {
                if (!msg.Contains("Error"))
                {
                    var pos = 0;
                    if ((pos = msg.LastIndexOf(_endMesage)) != -1)
                    {
                        if (pos >= 12) //(pos >= 8)
                        {
                            double weight;
                            if (double.TryParse(msg.Substring(pos - 10, 7), out weight))
                            {
                                _weight = weight/10;
                                if (msg[pos - 11] == '-') _weight = -_weight;
                                if (_waitingWeight)
                                {
                                    _waitingWeight = false;
                                    OnWeightObtained();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void OnDataObtained(string data)
        {
            if (DataObtained != null)
                DataObtained(data);
        }

        private void OnWeightObtained()
        {
            if (WeightObtained == null) return;
            WeightObtained(_weight);
            _dataNotifier.Notify(_weight.ToString());
        }

        public string GetBasculaInBuffer()
        {
            return _serialPort.ReadExisting();
        }

        public BasculaHbm WithTimeOut(int timeOut)
        {
            TimeOut = timeOut;
            return this;
        }
    }
}