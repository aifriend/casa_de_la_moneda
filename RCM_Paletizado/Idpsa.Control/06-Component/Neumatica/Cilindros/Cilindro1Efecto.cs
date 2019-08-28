using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Cilindro1Efecto : Cilindro
    {
        private readonly IActuador _actuadorTrabajo;
        private readonly ISensor _sensorReposo;
        private readonly ISensor _sensorTrabajo;

        public Cilindro1Efecto(ISensor sensorReposo, ISensor sensorTrabajo, IActuador actuadorTrabajo, string name)
            : base(name)
        {
            _sensorReposo = sensorReposo;
            _sensorTrabajo = sensorTrabajo;
            _actuadorTrabajo = actuadorTrabajo;
        }

        public Cilindro1Efecto(ISensor sensorReposo, ISensor sensorTrabajo, IActuador actuadorTrabajo)
            : this(sensorReposo, sensorTrabajo, actuadorTrabajo, "")
        {
        }

        public override bool EnReposo
        {
            get { return !_sensorTrabajo.Value() & _sensorReposo.Value(); }
        }

        public override bool EnTrabajo
        {
            get { return _sensorTrabajo.Value() & !_sensorReposo.Value(); }
        }

        public ISensor SensorReposo
        {
            get { return _sensorReposo; }
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
            //;Uñas|Cerrar|Abrir;;;;;B410B;;;;Y410;;;;B410A;;;;
            var generalManual = new GeneralManual();

            generalManual.FinalesCarreraBas[0] = _sensorReposo;
            generalManual.FinalesCarreraWrk[0] = _sensorTrabajo;
            generalManual.ActuadoresWrk[0] = _actuadorTrabajo;

            return new[] {new Manual(generalManual)};
        }
    }
}