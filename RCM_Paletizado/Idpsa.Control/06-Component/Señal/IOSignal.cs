using System;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class IOSignal
    {
        public bool Value { get; set; }

        public int Board { get; set; }

        public string Name { get; set; }
    }
}