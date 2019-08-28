using System;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class MesaConTope : Mesa
    {
        private readonly ICilindro _tope;

        public MesaConTope(ISensor sensor, IActuador motor, ICilindro tope, string name) : base(sensor, motor, name)
        {
            _tope = tope;
        }

        public MesaConTope(ISensor sensor, IActuador motor, ICilindro tope) : this(sensor, motor, tope, "")
        {
        }

        public ICilindro Tope
        {
            get { return _tope; }
        }
    }
}