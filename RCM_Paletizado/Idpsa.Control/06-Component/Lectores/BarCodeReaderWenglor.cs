using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using Idpsa.Control.Engine;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class BarCodeReaderWenglor : IRestorable, IReader, IDataNotifierProvider<string>, IManualsProvider
    {
        
        private readonly ASCIIEncoding _ascii = new ASCIIEncoding();
        private readonly DataNotifier<string> _dataNotifier;

        private readonly Predicate<string> _validationRule;
        private string _endMesage;
        [field : NonSerialized] private SerialPort _serialPort;
        private bool _waitingRead;
        public bool Habilitado { get; set; }//MCR. 2015.03.13

        public BarCodeReaderWenglor(string name, int port, Predicate<string> validationRule)
        {
            Name = name;
            _port = port;
            Initialize();
            _dataNotifier = new DataNotifier<string>(name);
            TimeOut = 30;
            BarCode = String.Empty;
            if (validationRule == null)
                validationRule = str => true;
            _validationRule = validationRule;
            Habilitado = true;//MCR. 2015.03.13
        }

        private int _port { get; set; }

        public string Name { get; private set; }
        public string BarCode { get; private set; }


        public int TimeOut { get; private set; }

        #region IReader Members

        public void Dispose()
        {
            _serialPort.Dispose();
        }

        public bool Connect()
        {
            bool value = false;
            try
            {
                if (!_serialPort.IsOpen)
                    _serialPort.Open();
                value =_serialPort.IsOpen;
            }
            catch { }
            return value;
        }

        public bool Disconnect()
        {
            if (_serialPort.IsOpen)
                _serialPort.Close();

            return !_serialPort.IsOpen;
        }

        public bool Connected()
        {
            bool value = false;
            try
            {
                value = _serialPort.IsOpen;
            }
            catch { }
            return value;
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
            var manual = new Manual(this) {Description = _dataNotifier.NotifierName};

            return new[] {manual};
        }

        #endregion

        #region Miembros de IReader

        bool IReader.Connected()
        {
            return Connected();
        }

        bool IReader.Connect()
        {
            return Connect();
        }

        bool IReader.Disconnect()
        {
            return Disconnect();
        }

        void IReader.Reset()
        {
            BarCode = String.Empty;
            if (Connected())
                Disconnect();
            if (!Connected())
                Connect();
        }

        bool IReader.ReadCode(out string code)
        {
            code = null;
            if (MesureBarCode())
            {
                code = BarCode;
                return true;
            }
            return false;
        }

        string IReader.LastReadedCode
        {
            get { return BarCode; }
        }

        #endregion

        public event Action<string> BarCodeObtained;
        public event Action<string> DataObtained;

        private void Initialize()
        {
            _waitingRead = false;
            _endMesage = _ascii.GetString(new byte[] {13, 10});
            _serialPort = new SerialPort
                              {
                                  BaudRate = 9600,
                                  Parity = Parity.Even,
                                  DataBits = 8,
                                  StopBits = StopBits.One,
                                  Handshake = Handshake.None,
                                  Encoding = Encoding.ASCII
                              };            
            _serialPort.DataReceived += DataRecived;
            _serialPort.ReadBufferSize = 50;
            _serialPort.WriteBufferSize = 30;
            _serialPort.ReceivedBytesThreshold = 20;
            var portNames = new List<string>(SerialPort.GetPortNames());
            if (portNames.Contains("COM" + _port))
            {
                _serialPort.PortName = "COM" + _port;
            }
        }


        public bool MesureBarCode()
        {
            BeginRead();
            var timer = new TON();

            while (!timer.Timing(TimeOut))
            {
                if (!EndRead() && !String.IsNullOrEmpty(BarCode))
                {
                    return true;
                }
            }

            return false;
        }


        public bool BeginRead()
        {
            bool value = false;
            try
            {               
                if (!_serialPort.IsOpen)
                    _serialPort.Open();

                if (_serialPort.IsOpen)
                {
                    _serialPort.DiscardInBuffer();
                    _serialPort.Write("<a>");
                    _waitingRead = true;
                    value = true;
                }
            }
            catch { };

            return value;
        }

        public bool EndRead()
        {
            return !_waitingRead;
        }

        public void DataRecived(object sender, SerialDataReceivedEventArgs e)
        {
            string msg = _serialPort.ReadExisting();
            OnDataObtained(msg);

            if (msg.EndsWith(_endMesage))
            {
                msg = msg.Substring(0, msg.Length - _endMesage.Length);
                if (_validationRule(msg))
                {
                    BarCode = msg;
                    OnBarCodeObtained();

                    if (_waitingRead)
                    {
                        _waitingRead = false;
                    }
                }
            }
        }

        public void OnDataObtained(string data)
        {
            if (DataObtained != null)
                DataObtained(data);
        }

        private void OnBarCodeObtained()
        {
            if (BarCodeObtained != null)
            {
                BarCodeObtained(BarCode);
                _dataNotifier.Notify(BarCode);
            }
        }

        public string GetInBuffer()
        {
            return _serialPort.ReadExisting();
        }

        public BarCodeReaderWenglor WithTimeOut(int timeOut)
        {
            TimeOut = timeOut;
            return this;
        }
    }
}