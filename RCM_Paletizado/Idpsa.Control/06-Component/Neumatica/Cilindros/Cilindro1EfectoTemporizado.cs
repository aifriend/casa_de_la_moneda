using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Cilindro1EfectoTemporizado : Cilindro
    {
        private readonly IActuador _actuadorTrabajo;
        private readonly int _timeBas;
        private readonly TON _timerBas;
        private readonly TON _timerWork;
        private readonly int _timeWork;

        public Cilindro1EfectoTemporizado(int timeBas, int timeWork, IActuador actuadorTrabajo, string name)
            : base(name)
        {
            _timeBas = timeBas;
            _timeWork = timeWork;
            _actuadorTrabajo = actuadorTrabajo;
            _timerBas = new TON();
            _timerWork = new TON();
        }

        public Cilindro1EfectoTemporizado(int timeBas, int timeWork, IActuador actuadorTrabajo)
            : this(timeBas, timeWork, actuadorTrabajo, "")
        {
        }

        public override bool EnReposo
        {
            get { return _timerBas.Timing(_timeBas); }
        }

        public override bool EnTrabajo
        {
            get { return _timerWork.Timing(_timeWork); }
        }

        public override void Reposo()
        {
            _actuadorTrabajo.Activate(false);
            _timerBas.Reset();
        }

        public override void Trabajo()
        {
            _actuadorTrabajo.Activate(true);
            _timerWork.Reset();
        }

        public override void Muerto()
        {
            throw new NotImplementedException();
        }


        protected override IEnumerable<Manual> GetPartialManualRepresentations()
        {
            //;Uñas|Cerrar|Abrir;;;;;B410B;;;;Y410;;;;B410A;;;;

            var generalManual = new GeneralManual();
            generalManual.ActuadoresWrk[0] = _actuadorTrabajo;

            return new[] {new Manual(generalManual)};
        }
    }
}