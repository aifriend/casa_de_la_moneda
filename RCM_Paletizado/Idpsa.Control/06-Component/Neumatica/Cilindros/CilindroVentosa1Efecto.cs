using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class CilindroVentosa1Efecto : Cilindro
    {
        private readonly IActuador _actuadorTrabajo;
        private readonly ISensor _sensorTrabajo;

        public CilindroVentosa1Efecto(ISensor sensorTrabajo, IActuador actuadorTrabajo, string name) : base(name)
        {
            _sensorTrabajo = sensorTrabajo;
            _actuadorTrabajo = actuadorTrabajo;
        }

        public CilindroVentosa1Efecto(ISensor sensorTrabajo, IActuador actuadorTrabajo)
            : this(sensorTrabajo, actuadorTrabajo, "")
        {
        }

        public override bool EnReposo
        {
            get { return !_sensorTrabajo.Value(); }
        }

        public override bool EnTrabajo
        {
            get { return _sensorTrabajo.Value(); }
        }

        public override void Reposo()
        {
            _actuadorTrabajo.Activate(false);
        }

        public override void Trabajo()
        {
            _actuadorTrabajo.Activate(true);
        }

        public override void Muerto()
        {
            _actuadorTrabajo.Activate(false);
        }

        protected override IEnumerable<Manual> GetPartialManualRepresentations()
        {
            var generalManual = new GeneralManual();
            generalManual.FinalesCarreraWrk[0] = _sensorTrabajo;
            generalManual.ActuadoresWrk[0] = _actuadorTrabajo;

            return new[] {new Manual(new GeneralManual())};
        }
    }
}