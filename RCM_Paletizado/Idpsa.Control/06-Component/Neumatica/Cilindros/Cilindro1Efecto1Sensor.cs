using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Cilindro1Efecto1Sensor : Cilindro
    {
        private readonly IActuador _actuadorTrabajo;
        private readonly ISensor _sensorTrabajo;

        public Cilindro1Efecto1Sensor(ISensor sensorTrabajo, IActuador actuadorTrabajo, string name)
            : base(name)
        {
            _sensorTrabajo = sensorTrabajo;
            _actuadorTrabajo = actuadorTrabajo;
        }

        public Cilindro1Efecto1Sensor(ISensor sensorTrabajo, IActuador actuadorTrabajo)
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

        public ISensor SensorTrabajo
        {
            get { return _sensorTrabajo; }
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
            return new[] {new Manual(generalManual)};
        }
    }
}