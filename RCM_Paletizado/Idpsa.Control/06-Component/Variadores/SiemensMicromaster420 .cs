using System;
using System.Collections.Generic;
using Idpsa.Control.Tool;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class SiemensMicromaster420
    {
        private const int MaxValueF = 85;
        private readonly int _bitInputs;
        private readonly int _bitOutputs;
        private readonly decimal _defaultVelocity;
        private readonly IOCollection _dICollection = new IOCollection();
        private readonly IOCollection _dOCollection = new IOCollection();
        private readonly decimal _frecuenceVelocityRate;
        private readonly Address _inputAddress;
        private readonly Address _ouputAddress;
        private readonly Dictionary<string, int> _statusWord1;
        private readonly Dictionary<string, int> _controlWord1;
        private string _lastState;


        public SiemensMicromaster420(Address inputs, Address outputs, IOCollection iCollection,
                                     IOCollection dOCollection, string name, decimal frecuenceVelocityRate,
                                     int defaultVelocity)
        {
            Name = name;
            _defaultVelocity = defaultVelocity;
            _frecuenceVelocityRate = frecuenceVelocityRate;
            _inputAddress = inputs;
            _ouputAddress = outputs;
            _dICollection = iCollection;
            _dOCollection = dOCollection;
            _bitInputs = (_inputAddress.Byte * 8) + _inputAddress.Bit;
            _bitOutputs = (_ouputAddress.Byte * 8) + _ouputAddress.Bit;
            _lastState = "Inactivo";
            _controlWord1 = new Dictionary<string, int>
                              {
                                  {"ListoAdelante", 32260},
                                  {"ListoAtras", 32268},
                                  {"Adelante", 32516},
                                  {"Atras", 32524},
                                  {"Actualizar Error", 65028}
                              };
            _statusWord1 = new Dictionary<string, int>
                               {
                                   {"Adelante Parada", 12794},
                                   {"Atras Parada", 12730},
                                   {"Adelante", 13563},
                                   {"Atras", 13499},
                                   {"Desconectado", 14586},
                                   {"Listo", 12731}
                               };
            //W1Estado.Add(" Parada", &H31FB)
        }

        public SiemensMicromaster420(Address inputs, Address outputs, IOCollection dICollection,
                                     IOCollection dOCollection, string name, decimal rfv)
            : this(inputs, outputs, dICollection, dOCollection, name, rfv, 1)
        {
        }

        public SiemensMicromaster420(Address inputs, Address outputs, IOCollection dICollection,
                                     IOCollection dOCollection, string name)
            : this(inputs, outputs, dICollection, dOCollection, name, 1, 1)
        {
        }

        public SiemensMicromaster420(Address inputs, Address outputs, IOCollection dICollection,
                                     IOCollection pardOCollection)
            : this(inputs, outputs, dICollection, pardOCollection, "", 1, 1)
        {
        }

        public string Name { get; set; }


        public void Mando(string orden)
        {
            Mando(orden, -1);
        }

        public void Mando(string orden, decimal v)
        {
            int Mando1 = _controlWord1[orden];
            int i;
            for (i = 0; i <= 15; i++)
            {
                _dOCollection[_bitOutputs + i].Value = ((Mando1 & (1 << i)) != 0) ? true : false;
            }
            if ((orden == "Adelante" | orden == "Atras"))
            {
                if (v != -1)
                {
                    Velocidad(v);
                }
                else
                {
                    Velocidad(_defaultVelocity);
                }
            }
        }

        public void Parada()
        {
            _dOCollection[_bitOutputs + 8].Value = false;
        }

        public void ParadaEmergencia()
        {
            _dOCollection[_bitOutputs + 10].Value = false;
        }

        public bool EnEstado(string ParEstado)
        {
            return (ParEstado == ConocerEstado());
        }

        public string ConocerEstado()
        {
            int valor1 = LeerEstado();
            foreach (var e in _statusWord1)
            {
                int valor2 = e.Value;
                if ((valor1 == valor2))
                {
                    _lastState = e.Key;
                    if ((_lastState.IndexOf("Parada") != -1))
                    {
                        _lastState = "Parada";
                    }
                }
            }
            return _lastState;
        }

        public int LeerEstado()
        {
            int valor = 0;

            for (int i = 0; i <= 15; i++)
            {
                if (_dICollection[_bitInputs + i].Value)
                {
                    valor += (1 << i);
                }
            }
            return valor;
        }

        public void Velocidad(decimal v)
        {
            var f = (int)(_frecuenceVelocityRate * v);
            Frecuencia(f);
        }

        public void Frecuencia(int f)
        {
            int i;
            if (f > MaxValueF)
            {
                f = MaxValueF;
            }
            f = (int)(f * 327.68);
            for (i = 0; i <= 7; i++)
            {
                _dOCollection[_bitOutputs + 24 + i].Value = (f & (1 << i)).ToBool();
            }
            for (i = 8; i <= 15; i++)
            {
                _dOCollection[_bitOutputs + 8 + i].Value = (f & (1 << i)).ToBool();
            }
        }

        public decimal LeerVelocidad()
        {
            return (LeerFrecuencia() / _frecuenceVelocityRate);
        }

        public int LeerFrecuencia()
        {
            int f = 0;
            int i;
            for (i = 0; i <= 7; i++)
            {
                if (_dICollection[_bitInputs + 24 + i].Value)
                {
                    f += 1 << i;
                }
            }
            for (i = 8; i <= 15; i++)
            {
                if (_dICollection[_bitInputs + 8 + i].Value)
                {
                    f += 1 << i;
                }
            }
            f = (int)(f / 327.68);
            if ((f >= (200 - MaxValueF)))
            {
                f -= 200;
            }
            return f;
        }

        public bool Fallo()
        {
            return ((_dICollection[_bitInputs + 11].Value || (!_dICollection[_bitInputs + 3].Value) ||
                     (!_dICollection[_bitInputs + 5].Value) || (!_dICollection[_bitInputs + 7].Value)));
        }

        public bool GiroDerechas()
        {
            return _dICollection[_bitInputs + 6].Value;
        }

        public bool VelocidadAlcanzada()
        {
            return _dICollection[_bitInputs].Value;
        }
    }
}