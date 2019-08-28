using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Cilindros2EfectoPuenteados : Cilindro
    {
        private readonly IActuador _actuadorReposo;
        private readonly IActuador _actuadorTrabajo;
        private readonly ISensor _sensorReposo1;
        private readonly ISensor _sensorReposo2;
        private readonly ISensor _sensorTrabajo1;
        private readonly ISensor _sensorTrabajo2;

        public Cilindros2EfectoPuenteados(ISensor sensorReposo1, ISensor sensorTrabajo1, ISensor sensorReposo2,
                                          ISensor sensorTrabajo2, IActuador actuadorReposo, IActuador actuadorTrabajo,
                                          string name, int _timeErrorWatch) : base(name)
        {
            _sensorReposo1 = sensorReposo1;
            _sensorTrabajo1 = sensorTrabajo1;
            _sensorReposo2 = sensorReposo2;
            _sensorTrabajo2 = sensorTrabajo2;
            _actuadorReposo = actuadorReposo;
            _actuadorTrabajo = actuadorTrabajo;
        }

        public Cilindros2EfectoPuenteados(ISensor sensorReposo1, ISensor sensorTrabajo1, ISensor sensorReposo2,
                                          ISensor sensorTrabajo2, IActuador actuadorReposo, IActuador actuadorTrabajo,
                                          string name)
            : this(
                sensorReposo1, sensorTrabajo1, sensorReposo2, sensorTrabajo2, actuadorReposo, actuadorTrabajo, name, 0)
        {
        }

        public Cilindros2EfectoPuenteados(ISensor sensorReposo1, ISensor sensorTrabajo1, ISensor sensorReposo2,
                                          ISensor sensorTrabajo2, IActuador actuadorReposo, IActuador actuadorTrabajo)
            : this(sensorReposo1, sensorTrabajo1, sensorReposo2, sensorTrabajo2, actuadorReposo, actuadorTrabajo, "", 0)
        {
        }

        public override bool EnReposo
        {
            get
            {
                return (_sensorReposo1.Value() & !_sensorTrabajo2.Value()) &&
                       (_sensorReposo2.Value() & !_sensorTrabajo2.Value());
            }
        }

        public override bool EnTrabajo
        {
            get
            {
                return (!_sensorReposo1.Value() & _sensorTrabajo2.Value()) &&
                       (!_sensorReposo2.Value() & _sensorTrabajo2.Value());
            }
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

        protected override IEnumerable<Manual> GetPartialManualRepresentations()
        {
            var generalManual1 = new GeneralManual();
            generalManual1.FinalesCarreraBas[0] = _sensorReposo1;
            generalManual1.FinalesCarreraWrk[0] = _sensorTrabajo1;

            generalManual1.ActuadoresBas[0] = _actuadorReposo;
            generalManual1.ActuadoresWrk[0] = _actuadorTrabajo;

            var generalManual2 = new GeneralManual();
            generalManual2.FinalesCarreraBas[0] = _sensorReposo2;
            generalManual2.FinalesCarreraWrk[0] = _sensorTrabajo2;

            return new[] {new Manual(generalManual1), new Manual(generalManual2)};
        }
    }
}