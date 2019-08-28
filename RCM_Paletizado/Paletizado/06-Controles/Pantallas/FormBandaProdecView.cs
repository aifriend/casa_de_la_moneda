using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Idpsa.Control.View;

namespace Idpsa.Paletizado
{
    public partial class FormBandaProdecView : UserControl, IViewTasksOwner
    {
        private readonly IdpsaSystemPaletizado _sys;
        private readonly BandaSalidaEnfajadora _banda;

        public FormBandaProdecView(IdpsaSystemPaletizado sys)
        {
            InitializeComponent();
            _sys = sys;
            _banda = _sys.Lines.BandaSalidaEnfajadora;
        }

        private void Task()
        {
            pSensorEntradaUnomaticElevador2.BackColor = _banda.SensorEntradaAlemanaJaponesa1.Value() ? Color.Red : Color.Black;
            pSensorEntradaManual.BackColor = _banda.SensorEntradaManualAlemana.Value() ? Color.Red: Color.Black;
            pSensorSalidaBanda.BackColor = _banda.SensorPresenciaSalida.Value() ? Color.Red : Color.Black;

            pGrupoSacado.BackColor = _banda.GrupoEnSalida ? Color.Green: Color.Red;

            pSolicitorJaponesa.BackColor = _banda.SolicitorJaponesa.ReadyToPutElement ? Color.Red : Color.Black;
            pSolicitorAlemana.BackColor = _banda.SolicitorAlemana.ReadyToPutElement ? Color.Red : Color.Black;
            pSolicitorAlemanaManual.BackColor = _banda.SolicitorAlemana.ReadyToPutElement ? Color.Red : Color.Black;

            numBanda1.Text = numBanda2.Text = _banda.NumGrupos().ToString(CultureInfo.InvariantCulture);
        }

        public IEnumerable<Action> GetViewTasks()
        {
            return new List<Action> { Task };
        }
    }
}
