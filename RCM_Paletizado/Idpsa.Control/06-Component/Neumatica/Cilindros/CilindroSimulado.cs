using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class CilindroSimulado : Cilindro
    {
        public CilindroSimulado(string name) : base(name)
        {
        }

        public CilindroSimulado() : this("")
        {
        }

        public override bool EnReposo
        {
            get { return true; }
        }

        public override bool EnTrabajo
        {
            get { return true; }
        }

        public override void Reposo()
        {
        }

        public override void Trabajo()
        {
        }

        public override void Muerto()
        {
        }

        protected override IEnumerable<Manual> GetPartialManualRepresentations()
        {
            return new[] {new Manual(new GeneralManual())};
        }
    }
}