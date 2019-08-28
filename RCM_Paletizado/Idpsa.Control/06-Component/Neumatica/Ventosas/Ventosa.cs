using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Ventosa : IManualsProvider
    {
        private readonly IActuador _eyectorVacio;
        private readonly ISensor _vacuostato;

        public Ventosa(ISensor vacuostato, IActuador eyectorVacio, string _name)
        {
            _vacuostato = vacuostato;
            _eyectorVacio = eyectorVacio;
            Name = _name;
        }

        public Ventosa(ISensor vacuostato, IActuador eyectorVacio) : this(vacuostato, eyectorVacio, "")
        {
        }

        public string Name { get; private set; }

        public void Vacio(bool Orden)
        {
            _eyectorVacio.Activate(true);
        }

        public void VacioOn()
        {
            _eyectorVacio.Activate(true);
        }

        public void VacioOff()
        {
            _eyectorVacio.Activate(false);
        }

        public bool EnVacio()
        {
            return _vacuostato.Value();
        }

        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            var generalManual = new GeneralManual();
            generalManual.ActuadoresWrk[0] = _eyectorVacio;
            generalManual.FinalesCarreraWrk[0] = _vacuostato;
            generalManual.BasName = "Parar";
            generalManual.WorkName = "Aspirar";

            var manual = new Manual(generalManual);
            if (String.IsNullOrEmpty(manual.Descripcion))
                manual.Descripcion = Name;

            return new[] {manual};
        }

        #endregion
    }
}