using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Cilindro2Efecto1Sensor : Cilindro
    {
        private readonly IActuador _actuadorReposo;
        private readonly IActuador _actuadorTrabajo;
        private readonly ISensor _sensorTrabajo;

        public Cilindro2Efecto1Sensor(ISensor sensorTrabajo, IActuador actuadorReposo, IActuador actuadorTrabajo,
                                      string name)
            : base(name)
        {
            _sensorTrabajo = sensorTrabajo;
            _actuadorReposo = actuadorReposo;
            _actuadorTrabajo = actuadorTrabajo;
        }

        public Cilindro2Efecto1Sensor(ISensor sensorTrabajo, IActuador actuadorReposo, IActuador actuadorTrabajo)
            : this(sensorTrabajo, actuadorReposo, actuadorTrabajo, "")
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
            _actuadorReposo.Activate(true);
            _actuadorTrabajo.Activate(false);
        }

        public override void Trabajo()
        {
            _actuadorReposo.Activate(false);
            _actuadorTrabajo.Activate(true);
        }

        public override void Muerto()
        {
            _actuadorReposo.Activate(false);
            _actuadorTrabajo.Activate(false);
        }

        public void SetPosition(Posicion pos)
        {
            switch (pos)
            {
                case Posicion.SinPosicion:
                    _actuadorReposo.Activate(false);
                    _actuadorTrabajo.Activate(false);
                    break;
                case Posicion.Reposo:
                    _actuadorReposo.Activate(true);
                    _actuadorTrabajo.Activate(false);
                    break;
                case Posicion.Trabajo:
                    _actuadorReposo.Activate(false);
                    _actuadorTrabajo.Activate(true);
                    break;
            }
        }


        protected override IEnumerable<Manual> GetPartialManualRepresentations()
        {
            var generalManual = new GeneralManual();
            generalManual.FinalesCarreraWrk[0] = _sensorTrabajo;
            generalManual.ActuadoresBas[0] = _actuadorReposo;
            generalManual.ActuadoresWrk[0] = _actuadorTrabajo;
            return new[] {new Manual(generalManual)};
        }
    }
}