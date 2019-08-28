using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using Idpsa.Control.Engine;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class ScaleHBM : IRestorable, IReader, IDataNotifierProvider<string>, IManualsProvider
    {
        #region Delegates

        public delegate void DataObtainedHandler(string data);

        public delegate void WeightObtainedHandler(double weigth);

        public bool Habilitado { get; set; }//MCR. 2015.03.13

        #endregion

        private readonly ASCIIEncoding _ascii = new ASCIIEncoding();
        private readonly DataNotifier<string> _dataNotifier;
        private readonly Func<double, double> _weightTransformation;

        private string _endMesage;
        [field : NonSerialized] private SerialPort _serialPort;
        private bool _waitingWeight;

       
        public ScaleHBM(string name, int port)
            :this(name,port,x=>x)
        {
            Name = name;
            _port = port;
            Initialize();
            _dataNotifier = new DataNotifier<string>(name);
            TimeOut = 20;
            Habilitado = true;//MCR. 2015.03.13
        }

        public ScaleHBM(string name, int port, Func<double, double> weightTransformation)
        {
            if(String.IsNullOrEmpty(name))
                throw new ArgumentException("name can't be null or empty");
            if(port <0)
                throw new ArgumentOutOfRangeException("port");
            if(weightTransformation == null)
                throw new ArgumentNullException("weightTransformation");
          
            Name = name;
            _port = port;
            Initialize();
            _dataNotifier = new DataNotifier<string>(name);
            TimeOut = 20;
            _weightTransformation = weightTransformation;
        }

        private int _port { get; set; }

        public string Name { get; private set; }
        public double Weight { get; private set; }

        public bool Configurated { get; private set; }
        public int TimeOut { get; private set; }

        #region IReader Members

        public void Dispose()
        {
            _serialPort.Dispose();
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

        public bool Connected()
        {
            return _serialPort.IsOpen;
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
            if (Connected())
                Disconnect();
            if (!Connected())
                Connect();
        }

        bool IReader.ReadCode(out string code)
        {
            Configurate();
            code = null;
            if (MesureWeight())
            {
                code = Weight.ToString();
                return true;
            }
            return false;
        }

        string IReader.LastReadedCode
        {
            get { return Weight.ToString(); }
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
                                  BaudRate = 9600,
                                  Parity = Parity.Even,
                                  DataBits = 8,
                                  StopBits = StopBits.One,
                                  Handshake = Handshake.None,
                                  Encoding = Encoding.ASCII
                              };
            _serialPort.DataReceived += DataRecived;            
            _serialPort.ReadBufferSize = 20;
            _serialPort.WriteBufferSize = 20;
            var portNames = new List<string>(SerialPort.GetPortNames());
            if (portNames.Contains("COM" + _port))
            {
                _serialPort.PortName = "COM" + _port;
            }
        }

        public bool BeginConfiguration()
        {
            Configurated = false;
            if (!_serialPort.IsOpen)
                _serialPort.Open();

            if (_serialPort.IsOpen)
            {
                _serialPort.ReceivedBytesThreshold = 6;
                //sP.DiscardInBuffer();
                _serialPort.Write("COF3;");
                Configurated = true;
            }

            return Configurated;
        }

        public bool EndConfiguration()
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

        public bool MesureWeight()
        {
            BeginWeight();
            var timer = new TON();

            while (!timer.Timing(TimeOut))
            {
                if (!EndWeight())
                    return true;
            }

            return false;
        }


        public bool BeginWeight()
        {
            bool value = false;

            if (!_serialPort.IsOpen)
                _serialPort.Open();

            if (_serialPort.IsOpen)
            {
                _serialPort.DiscardInBuffer();
                _serialPort.ReceivedBytesThreshold = 10;
                _serialPort.Write("MSV?;");
                _waitingWeight = true;
                value = true;
            }

            return value;
        }

        public bool EndWeight()
        {
            return !_waitingWeight;
        }

        public void DataRecived(object sender, SerialDataReceivedEventArgs e)
        {
            string msg = _serialPort.ReadExisting();

            OnDataObtained(msg);
            if (msg.Contains("0\r\n0\r\n"))
            {
                Configurated = false;
                msg = msg.Replace("0\r\n0\r\n", "");
            }

            if (msg.EndsWith(_endMesage))
                if (!msg.Contains("Error"))
                {
                    int pos;
                    if ((pos = msg.LastIndexOf(_endMesage)) != -1)
                    {
                        if (pos >= 8)
                        {
                            double weight;
                            if (double.TryParse(msg.Substring(pos - 5, 5), out weight))
                            {                                
                                if (msg[pos - 8] == '-') 
                                    weight = -weight;
                                Weight = _weightTransformation(weight);
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

        public void OnDataObtained(string data)
        {
            if (DataObtained != null)
                DataObtained(data);
        }

        private void OnWeightObtained()
        {
            if (WeightObtained != null)
            {
                WeightObtained(Weight);
                _dataNotifier.Notify(Weight.ToString());
            }
        }

        public string GetBasculaInBuffer()
        {
            return _serialPort.ReadExisting();
        }

        public ScaleHBM WithTimeOut(int timeOut)
        {
            TimeOut = timeOut;
            return this;
        }

        #region Miembros de IReader


        public bool BeginRead()
        {
            return BeginWeight();
        }

        public bool EndRead()
        {
            return EndWeight();
        }

        #endregion
    }
}