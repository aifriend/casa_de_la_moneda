using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Cilindro2Efecto : Cilindro
    {
        private readonly IActuador _actuadorReposo;
        private readonly IActuador _actuadorTrabajo;
        private readonly ISensor _sensorReposo;
        private readonly ISensor _sensorTrabajo;

        public Cilindro2Efecto(ISensor sensorReposo, ISensor sensorTrabajo, IActuador actuadorReposo,
                               IActuador actuadorTrabajo, string name) : base(name)
        {
            _sensorReposo = sensorReposo;
            _sensorTrabajo = sensorTrabajo;
            _actuadorReposo = actuadorReposo;
            _actuadorTrabajo = actuadorTrabajo;
        }

        public Cilindro2Efecto(ISensor sensorReposo, ISensor sensorTrabajo, IActuador actuadorReposo,
                               IActuador actuadorTrabajo)
            : this(sensorReposo, sensorTrabajo, actuadorReposo, actuadorTrabajo, "")
        {
        }

        public override bool EnReposo
        {
            get { return _sensorReposo.Value() & !_sensorTrabajo.Value(); }
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

        public Posicion GetPosition()
        {
            Posicion pos = Posicion.SinPosicion;
            if (((_sensorReposo.Value()) & (_sensorTrabajo.Value())))
            {
                pos = Posicion.SinPosicion;
            }
            else if (((_sensorReposo.Value()) & (!_sensorTrabajo.Value())))
            {
                pos = Posicion.Reposo;
            }
            else if (((!_sensorReposo.Value()) & (_sensorTrabajo.Value())))
            {
                pos = Posicion.Trabajo;
            }
            return pos;
        }

        protected override IEnumerable<Manual> GetPartialManualRepresentations()
        {
            var generalManual = new GeneralManual();
            generalManual.FinalesCarreraBas[0] = _sensorReposo;
            generalManual.FinalesCarreraWrk[0] = _sensorTrabajo;
            generalManual.ActuadoresBas[0] = _actuadorReposo;
            generalManual.ActuadoresWrk[0] = _actuadorTrabajo;
            return new[] {new Manual(generalManual)};
        }
    }
}