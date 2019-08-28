using System;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Piloto
    {
        private readonly Input _boton;
        private readonly Output _luz;

        public Piloto(Input parBoton, Output parLuz, string parName)
        {
            _boton = parBoton;
            _luz = parLuz;
            Name = parName;
        }

        public Piloto(Input parBoton, Output parLuz) : this(parBoton, parLuz, "")
        {
        }

        public string Name { get; set; }

        public bool Pulsado()
        {
            return _boton.Value();
        }

        public void LuzOn()
        {
            _luz.PutSet();
        }

        public void LuzOff()
        {
            _luz.PutReset();
        }

        public bool Encendido()
        {
            return _luz.Value();
        }
    }
}