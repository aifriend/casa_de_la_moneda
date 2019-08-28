using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.Component;
using Idpsa.Control.Mode.Manually;

namespace Idpsa.Control.View
{
    public class ControlTelemetroIfmO1D100 : UserControl, IRefrescable
    {
        private readonly IfmO1D100 Telemetro;

        #region " Código generado por el Diseñador de Windows Forms "

        //Requerido por el Diseñador de Windows Forms
        private IContainer components;
        internal Label Label1;
        //NOTA: el Diseñador de Windows Forms requiere el siguiente procedimiento
        //Puede modificarse utilizando el Diseñador de Windows Forms. 
        //No lo modifique con el editor de código.
        internal Label lbCom;
        internal Label lbDist;

        public ControlTelemetroIfmO1D100(Manual manual, Bus bus)
        {
            //El Diseñador de Windows Forms requiere esta llamada.
            InitializeComponent();
            lbCom.Text = manual.Descripcion.Substring("Telemetro-".Length).Trim();
            var generalManual = (GeneralManual) manual.RepresentedInstance;
            Telemetro =
                new IfmO1D100(
                    new Address(int.Parse(generalManual.ActuadoresBas[0].ToString()),
                                int.Parse(generalManual.FinalesCarreraBas[0].ToString())), bus.InCollection);
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

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            lbCom = new Label();
            lbDist = new Label();
            Label1 = new Label();
            SuspendLayout();
            //
            //lbCom
            //
            lbCom.BackColor = SystemColors.ControlLight;
            lbCom.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbCom.Location = new Point(148, 14);
            lbCom.Name = "lbCom";
            lbCom.Size = new Size(256, 32);
            lbCom.TabIndex = 9;
            lbCom.TextAlign = ContentAlignment.MiddleCenter;
            //
            //lbDist
            //
            lbDist.BackColor = Color.Black;
            lbDist.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbDist.ForeColor = Color.White;
            lbDist.Location = new Point(440, 10);
            lbDist.Name = "lbDist";
            lbDist.Size = new Size(184, 40);
            lbDist.TabIndex = 10;
            lbDist.TextAlign = ContentAlignment.MiddleCenter;
            //
            //Label1
            //
            Label1.BackColor = Color.LightGray;
            Label1.Location = new Point(-2, 66);
            Label1.Name = "Label1";
            Label1.Size = new Size(792, 12);
            Label1.TabIndex = 11;
            //
            //ControlTelemetroTudor
            //
            Controls.Add(Label1);
            Controls.Add(lbDist);
            Controls.Add(lbCom);
            Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "ControlTelemetroTudor";
            Size = new Size(792, 78);
            ResumeLayout(false);
        }

        #endregion

        #region IRefrescable Members

        public virtual void Refresh_()
        {
            try
            {
                lbDist.Text = Telemetro.Distancia().ToString();
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}