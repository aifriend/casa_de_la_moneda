using System;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.Component;
using Idpsa.Control.Mode;
using Idpsa.Control.Mode.Manually;

namespace Idpsa.Control.View
{
    public partial class ControlLinealActuator : UserControl, IRefrescable, IRun
    {
        private readonly LinealActuatorWithIncrementalEncoder linealActuator;
        private bool doReference;
        private bool negDirection;
        private bool posDirection;

        public ControlLinealActuator(Manual manual, LinealActuatorWithIncrementalEncoder linealActuator)
        {
            InitializeComponent();

            var d = new[] {'|'};

            string[] str = manual.Descripcion.Substring("LinealActuator-".Length).Trim().Split(d);
            lbCom.Text = str[1].Trim();
            if (str.Length > 2)
            {
                if (str[2].Trim().Length > 0)
                {
                    btnNeg.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Regular);
                    btnNeg.Text = str[2].Trim();
                }
            }
            if (str.Length > 3)
            {
                if (str[3].Trim().Length > 0)
                {
                    btnPos.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Regular);
                    btnPos.Text = str[3].Trim();
                }
            }

            this.linealActuator = linealActuator;
        }

        private void btnNeg_MouseDown(object sender, MouseEventArgs e)
        {
            negDirection = true;
            doReference = false;
        }

        private void btnNeg_MouseUp(object sender, MouseEventArgs e)
        {
            negDirection = false;
        }

        private void btnPos_MouseDown(object sender, MouseEventArgs e)
        {
            posDirection = true;
            doReference = false;
        }

        private void btnPos_MouseUp(object sender, MouseEventArgs e)
        {
            posDirection = false;
        }

        private void btRefenciar_Click(object sender, EventArgs e)
        {
            doReference = true;
            posDirection = false;
            negDirection = false;
        }

        #region Miembros de IRefrescable

        public void Refresh_()
        {
            lbReferenced.BackColor = linealActuator.Referenced ? Color.Green : Color.Red;
            lbP.Text = linealActuator.Position().ToString();
            lbCounter.Text = linealActuator.Encoder.PulseCounter.GetValue().ToString();
        }

        #endregion

        #region Miembros de IRun

        public void Run()
        {
            if (doReference)
                if (linealActuator.Reference(0, 0.1))
                    doReference = false;

            if (posDirection)
                linealActuator.Activate1();
            else if (negDirection)
                linealActuator.Activate2();
            else
                linealActuator.Deactivate();
        }

        #endregion
    }
}