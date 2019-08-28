using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.Component;
using Idpsa.Control.Mode.Manually;

namespace Idpsa.Control.View
{
    public class ControlAltivar31 : ControlActuadorConInv
    {
        private readonly ActuadorP SelVelocidad;

        #region " Código generado por el Diseñador de Windows Forms "

        internal CheckBox cbVelocidad;
        private IContainer components;

        public ControlAltivar31(Manual m) : base(m)
        {
            //El Diseñador de Windows Forms requiere esta llamada.
            InitializeComponent();
            var generalManual = new GeneralManual();
            SelVelocidad = new ActuadorP(generalManual.ActuadoresWrk[1]);
            //Agregar cualquier inicialización después de la llamada a InitializeComponent()
        }

        //UserControl reemplaza a Dispose para limpiar la lista de componentes.
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if ((components != null))
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        //Requerido por el Diseñador de Windows Forms

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            cbVelocidad = new CheckBox();
            cbVelocidad.CheckedChanged += cbVelocidad_CheckedChanged;
            Load += controlAltivar31_Load;
            SuspendLayout();
            //
            //cbVelocidad
            //
            cbVelocidad.Location = new Point(608, 4);
            cbVelocidad.Name = "cbVelocidad";
            cbVelocidad.Size = new Size(88, 38);
            cbVelocidad.TabIndex = 91;
            cbVelocidad.Text = "Velocidad  Alta";
            //
            //controlAltivar31
            //
            Controls.Add(cbVelocidad);
            Name = "controlAltivar31";
            Controls.SetChildIndex(cbVelocidad, 0);
            ResumeLayout(false);
        }

        #endregion

        private void cbVelocidad_CheckedChanged(object sender, EventArgs e)
        {
            if (cbVelocidad.Checked)
            {
                SelVelocidad.Activate(true);
            }
            else
            {
                SelVelocidad.Activate(false);
            }
        }

        private void controlAltivar31_Load(object sender, EventArgs e)
        {
            SelVelocidad.Activate(false);
        }
    }
}