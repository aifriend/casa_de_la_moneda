using System;
using Idpsa.Control.Tool;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Parker
    {
        private readonly int _bitInputs;
        private readonly int _bitOutputs;
        private readonly IOCollection _dICollection = new IOCollection();
        private readonly IOCollection _dOCollection = new IOCollection();
        private readonly string _name;

        private readonly TOF _timerOff = new TOF();
        private readonly TON _timerOn = new TON();
        private double _aceleracion;
        //15 
        private bool _aliveLocal;
        private bool _aliveParker;
        private bool _alivePC;
        private bool _conectado;
        private bool _deshabilitarMotor;
        private bool _entradaParada;
        private bool _habilitarMotor;
        //14
        //1 / 1
        private bool _inicio;
        private Address _inputs;
        private bool _jog;
        private bool _motorError;
        private Address _outputs;
        //0 / 0
        private bool _parada;
        private double _posicion;
        private bool _punto;
        private bool _readyMotor;
        private bool _referenciarMotor;
        //2 / 2
        private bool _reset;
        private bool _resetMotor;
        private bool _salidaParada;
        //48 W / 48
        private double _velocidad;
        //64 W  /64


        public Parker(Address parInputs, Address parOutputs, IOCollection pardICollection, IOCollection pardOCollection,
                      string parName, int _timeErrorWatch)
        {
            TimeErrorWatch = _timeErrorWatch;
            _name = parName;
            _inputs = parInputs;
            _outputs = parOutputs;
            _dICollection = pardICollection;
            _dOCollection = pardOCollection;
            _bitInputs = (_inputs.Byte * 8) + _inputs.Bit;
            _bitOutputs = (_outputs.Byte * 8) + _outputs.Bit;
        }

        public Parker(Address parInputs, Address parOutputs, IOCollection pardICollection, IOCollection pardOCollection,
                      string parName) : this(parInputs, parOutputs, pardICollection, pardOCollection, parName, 0)
        {
        }

        public Parker(Address parInputs, Address parOutputs, IOCollection pardICollection, IOCollection pardOCollection)
            : this(parInputs, parOutputs, pardICollection, pardOCollection, "", 0)
        {
        }

        public int TimeErrorWatch { get; set; }


        public string Name
        {
            get { return _name; }
        }

        public Address Inputs
        {
            get { return _inputs; }
            set { _inputs = value; }
        }

        public Address Outputs
        {
            get { return _outputs; }
            set { _outputs = value; }
        }

        public bool Alive
        {
            get
            {
                _aliveLocal = _dICollection[_bitInputs + 15].Value;
                return _aliveLocal;
            }
            set
            {
                _aliveLocal = value;
                _dOCollection[_bitOutputs + 15].Value = _aliveLocal;
            }
        }

        public bool HabilitarMotor
        {
            get
            {
                _habilitarMotor = _dICollection[_bitInputs + 11].Value;
                return _habilitarMotor;
            }
            set
            {
                _habilitarMotor = value;
                _dOCollection[_bitOutputs + 11].Value = _habilitarMotor;
            }
        }

        public bool DeshabilitarMotor
        {
            get
            {
                _deshabilitarMotor = _dICollection[_bitInputs + 12].Value;
                return _deshabilitarMotor;
            }
            set
            {
                _deshabilitarMotor = value;
                _dOCollection[_bitOutputs + 12].Value = _deshabilitarMotor;
            }
        }

        public bool ReferenciarMotor
        {
            get
            {
                _referenciarMotor = _dICollection[_bitInputs + 10].Value;
                return _referenciarMotor;
            }
            set
            {
                _referenciarMotor = value;
                _dOCollection[_bitOutputs + 10].Value = _referenciarMotor;
            }
        }

        public bool ResetMotor
        {
            get
            {
                _resetMotor = _dICollection[_bitInputs + 7].Value;
                return _resetMotor;
            }
            set
            {
                _resetMotor = value;
                _dOCollection[_bitOutputs + 7].Value = _resetMotor;
            }
        }

        public bool Inicio
        {
            get
            {
                _inicio = _dICollection[_bitInputs + 1].Value;
                return _inicio;
            }
            set
            {
                _inicio = value;
                _dOCollection[_bitOutputs + 1].Value = _inicio;
            }
        }

        public bool SalidaParada
        {
            get
            {
                _salidaParada = _dOCollection[_bitOutputs + 0].Value;
                return _salidaParada;
            }
            set
            {
                _salidaParada = value;
                _dOCollection[_bitOutputs + 0].Value = _salidaParada;
            }
        }

        public bool EntradaParada
        {
            get
            {
                _entradaParada = _dOCollection[_bitInputs + 0].Value;
                return _entradaParada;
            }
        }

        public bool Parada
        {
            get
            {
                _parada = _dICollection[_bitInputs + 0].Value;
                return _parada;
            }
            set
            {
                _parada = value;
                _dOCollection[_bitOutputs + 0].Value = _parada;
            }
        }

        public bool Reset
        {
            get
            {
                _reset = _dICollection[_bitInputs + 2].Value;
                return _reset;
            }
            set
            {
                _reset = value;
                _dOCollection[_bitOutputs + 2].Value = _reset;
            }
        }

        public bool MotorError
        {
            get
            {
                _motorError = _dICollection[_bitInputs + 13].Value;
                return _motorError;
            }
        }

        //Public ReadOnly Property FinStart() As Boolean
        //    Get
        //        FinStartLocal = dICollectionLocal(BitInputs + 1).Value
        //        Return FinStartLocal
        //    End Get
        //End Property
        //Public ReadOnly Property FinReset() As Boolean
        //    Get
        //        FinResetLocal = dICollectionLocal(BitInputs + 2).Value
        //        Return FinResetLocal
        //    End Get
        //End Property
        //Public ReadOnly Property FinParada() As Boolean
        //    Get
        //        FinParadaLocal = dICollectionLocal(BitInputs + 0).Value
        //        Return FinParadaLocal
        //    End Get
        //End Property
        public bool Jog
        {
            get
            {
                _jog = _dICollection[_bitInputs + 8].Value;
                return _jog;
            }
            set
            {
                _jog = value;
                _dOCollection[_bitOutputs + 8].Value = _jog;
            }
        }

        public bool Punto
        {
            get
            {
                _punto = _dICollection[_bitInputs + 9].Value;
                return _punto;
            }
            set
            {
                _punto = value;
                _dOCollection[_bitOutputs + 9].Value = _punto;
            }
        }

        public double Posicion
        {
            get
            {
                _posicion = ReadDWord(_bitInputs + 16);
                return _posicion;
            }
            set
            {
                if (value >= -2147483647 & value <= 2147483647)
                {
                    _posicion = value;
                }
                else
                {
                    _posicion = 0;
                }
                WriteDWord(_bitOutputs + 16, (int)_posicion);
            }
        }

        public double Velocidad
        {
            get
            {
                _velocidad = ReadWord(_bitInputs + 48);
                return _velocidad;
            }
            set
            {
                if (value >= -32767 & value <= 32767)
                {
                    _velocidad = value;
                }
                else
                {
                    _velocidad = 0;
                }
                WriteWord(_bitOutputs + 48, (int)_velocidad);
            }
        }

        public double Aceleracion
        {
            get
            {
                _aceleracion = ReadWord(_bitInputs + 64);
                return _aceleracion;
            }
            set
            {
                if (value >= -32767 & value <= 32767)
                {
                    _aceleracion = value;
                }
                else
                {
                    _aceleracion = 0;
                }
                WriteWord(_bitOutputs + 64, (int)_aceleracion);
            }
        }

        public int LimiteSuperior { get; set; }

        public int LimiteInferior { get; set; }

        public bool Conectado
        {
            get { return _conectado; }
        }

        public bool ReadyMotor
        {
            get
            {
                _readyMotor = _dICollection[_bitInputs + 14].Value;
                return _readyMotor;
            }
        }

        //Protected Function ReadByte(ByVal parOffset As Integer) As Byte
        //    Dim i As Integer
        //    Dim lByte As Byte
        //    lByte = 0
        //    For i = 0 To 7
        //        lByte = lByte + (dICollectionLocal(parOffset + i).Value * Math.Pow(Convert.ToDouble(2), Convert.ToDouble(i)))
        //    Next
        //    Return lByte
        //End Function
        protected int ReadWord(int parOffset)
        {
            int i;
            int lInteger;
            long lLong = 0;
            for (i = 0; i <= 14; i++)
            {
                lLong =
                    (long)
                    (lLong +
                     (Math.Abs(_dICollection[parOffset + i].Value.ToInt()) *
                      Math.Pow(Convert.ToDouble(2), Convert.ToDouble(i))));
            }
            if (_dICollection[parOffset + 15].Value == false)
            {
                lInteger = (int)lLong;
            }
            else
            {
                lInteger = (int)(lLong - 32768);
            }
            return lInteger;
        }

        protected int ReadDWord(int parOffset)
        {
            int i;
            int lInteger;
            long lLong = 0;
            for (i = 0; i <= 30; i++)
            {
                lLong =
                    (long)
                    (lLong +
                     (Math.Abs(_dICollection[parOffset + i].Value.ToInt()) *
                      Math.Pow(Convert.ToDouble(2), Convert.ToDouble(i))));
            }
            if (_dICollection[parOffset + 31].Value == false)
            {
                lInteger = (int)lLong;
            }
            else
            {
                lInteger = (int)(lLong - 2147483648L);
            }
            return lInteger;
        }

        //Protected Sub WriteByte(ByVal parOffset As Integer, ByVal parByte As Integer)
        //    Dim i As Integer
        //    Dim lInteger As Integer
        //    Dim Cociente As Integer
        //    Dim Resto As Integer
        //    lInteger = parByte
        //    For i = 7 To 0 Step -1
        //        Cociente = Math.DivRem(lInteger, Convert.ToInt32(Math.Pow(Convert.ToDouble(2), Convert.ToDouble(i))), Resto)
        //        If Cociente = 1 Then
        //            dOCollectionLocal(parOffset + i).Value = True
        //        Else
        //            dOCollectionLocal(parOffset + i).Value = False
        //        End If
        //        lInteger = Resto
        //    Next
        //End Sub
        protected void WriteWord(int parOffset, int parWord)
        {
            int i;
            int lInteger;
            if (parWord > 0)
            {
                lInteger = parWord;
            }
            else
            {
                lInteger = 65536 + parWord;
            }
            for (i = 15; i >= 0; i += -1)
            {
                int Resto;
                int Cociente = Math.DivRem(lInteger, Convert.ToInt32(Math.Pow(Convert.ToDouble(2), Convert.ToDouble(i))),
                                           out Resto);
                _dOCollection[parOffset + i].Value = Cociente == 1;
                lInteger = Resto;
            }
        }

        protected void WriteDWord(int parOffset, int parDWord)
        {
            int i;
            long lInteger;
            if (parDWord > 0)
            {
                lInteger = parDWord;
            }
            else
            {
                lInteger = 4294967296L + parDWord;
            }
            for (i = 31; i >= 0; i += -1)
            {
                int Resto;
                int Cociente =
                    Math.DivRem((int)lInteger,
                                (int)Convert.ToInt64(Math.Pow(Convert.ToDouble(2), Convert.ToDouble(i))), out Resto);
                _dOCollection[parOffset + i].Value = Cociente == 1;
                lInteger = Resto;
            }
        }

        public void Run()
        {
            _conectado = lFnReady();
        }

        private bool lFnReady()
        {
            bool BolReturn = false;
            _aliveParker = Alive;
            _alivePC = !_aliveParker;
            _timerOn.Input = _alivePC;
            _timerOn.TimeToPass = 1000;
            _timerOff.Input = _alivePC;
            _timerOff.TimeToPass = 1000;
            if (_timerOn.Q | _timerOff.Q)
            {
            }
            else
            {
                BolReturn = true;
            }
            Alive = _alivePC;
            return BolReturn;
        }

        public bool Origen_MotorError()
        {
            Reset = false;
            Punto = false;
            ReferenciarMotor = false;
            Jog = false;
            Parada = false;
            Inicio = false;
            ResetMotor = false;
            HabilitarMotor = false;
            DeshabilitarMotor = false;
            return MotorError;
        }

        //Si false saltamos a ReadyMotor
        public bool InicioReset()
        {
            bool value = !Inicio;
            if (Inicio == false)
            {
                ResetMotor = true;
                Inicio = true;
            }
            return value;
        }

        public bool FinalReset()
        {
            if (Inicio)
            {
                ResetMotor = false;
                Inicio = false;
            }
            return Inicio;
        }

        //Si true saltamos a InicioMovimiento
        public bool InicioHabilitar()
        {
            bool value = !Inicio;
            if (Inicio == false)
            {
                HabilitarMotor = true;
                Inicio = true;
            }
            return value;
        }

        public bool FinalHabilitar()
        {
            bool value = Inicio;
            if (Inicio)
            {
                HabilitarMotor = false;
                Inicio = false;
            }
            return value;
        }

        public bool StartMov(int pos, int v, int a)
        {
            bool NoInicio = !Inicio;
            if (NoInicio)
            {
                Posicion = pos;
                Punto = true;
                Velocidad = v < 1 ? 1 : v;
                Aceleracion = a < 1 ? 1 : a;
                Inicio = true;
            }
            return NoInicio;
        }

        public bool EndMov()
        {
            bool value = Inicio;
            if (value)
            {
                Punto = false;
                Inicio = false;
            }
            return value;
        }

        //    Public Property ReactivateState() As Boolean
        //        Get
        //            Return _ReactivateState
        //        End Get
        //        Set(ByVal Value As Boolean)
        //            _ReactivateState = Value
        //        End Set
        //    End Property

        public bool EnOrigen(double posOrigen)
        {
            bool enOrigen;
            if ((MotorError == false) & ReadyMotor & (Parada == false) & (Math.Abs(Posicion - posOrigen) <= 1))
            {
                enOrigen = true;
            }
            else
            {
                enOrigen = false;
            }
            return enOrigen;
        }
    }
}