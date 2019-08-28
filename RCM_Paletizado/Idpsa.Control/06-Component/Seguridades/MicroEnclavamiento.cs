using System;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class MicroEnclavamiento
    {
        private readonly Input _input;
        private readonly Output _output;

        public MicroEnclavamiento(Input input, Output output, string name)
        {
            _input = input;
            _output = output;
            Name = name;
        }

        public MicroEnclavamiento(Input input, Output output) : this(input, output, "")
        {
        }

        public string Name { get; set; }

        public void Enclavar(bool Value)
        {
            if (Value)
                _output.PutSet();
            else
                _output.PutReset();
        }

        public bool Cerrado()
        {
            return _input.Value();
        }

        public bool Abierto()
        {
            return _input.Value();
        }
    }
}