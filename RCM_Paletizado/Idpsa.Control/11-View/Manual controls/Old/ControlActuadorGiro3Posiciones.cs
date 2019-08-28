using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals.ViewControl;
using Idpsa.Control.Mode.Manually;

namespace Idpsa.Control.View
{
    public class ControlActuadorGiro3Posiciones : UserControl, IRefrescable, IFitWidthToPanel
    {
        #region Delegates

        public delegate void click_EventHandler();

        #endregion

        #region " Código generado por el Diseñador de Windows Forms "

        //NOTA: el Diseñador de Windows Forms requiere el siguiente procedimiento
        //Puede modificarse utilizando el Diseñador de Windows Forms. 
        //No lo modifique con el editor de código.
        internal Button btT1;
        internal Button btT2;
        private IContainer components;
        internal Label Label1;
        internal Label lbAt1;
        internal Label lbAt2;
        internal Label lbCom;
        internal Label lbSt1;
        internal Label lbSt2;

        public ControlActuadorGiro3Posiciones(Manual manual)
        {
            //El Diseñador de Windows Forms requiere esta llamada.
            InitializeComponent();


            var generalManual = (GeneralManual) manual.RepresentedInstance;

            string[] str = manual.Descripcion.Substring("Giro3Posiciones-".Length).Trim().Split(new[] {'|'});
            lbCom.Text = str[0].Trim();
            if (str.Length > 1)
            {
                if (str[1].Trim().Length > 0)
                {
                    btT1.Font = new Font("Microsoft Sans Serif", 8f);
                    btT1.Text = str[1].Trim();
                }
            }
            if (str.Length > 2)
            {
                if (str[2].Trim().Length > 0)
                {
                    btR.Font = new Font("Microsoft Sans Serif", 8f);
                    btR.Text = str[2].Trim();
                }
            }

            if (str.Length > 3)
            {
                if (str[3].Trim().Length > 0)
                {
                    btT2.Font = new Font("Microsoft Sans Serif", 8f);
                    btT2.Text = str[3].Trim();
                }
            }


            if ((generalManual.ActuadoresBas[0] != null))
                lbAr.Text = generalManual.ActuadoresBas[0].ToString();
            else
                lbAr.BackColor = lbAr.Parent.BackColor;

            if ((generalManual.ActuadoresWrk[0] != null))
                lbAt1.Text = generalManual.ActuadoresWrk[0].ToString();
            else
                lbAt1.BackColor = lbAt1.Parent.BackColor;

            if ((generalManual.ActuadoresWrk[1] != null))
                lbAt2.Text = generalManual.ActuadoresWrk[1].ToString();
            else
                lbAt2.BackColor = lbAt2.Parent.BackColor;


            if ((generalManual.FinalesCarreraBas[0] != null))
                lbSr.Text = generalManual.FinalesCarreraBas[0].ToString();
            else
                lbSr.BackColor = lbSr.Parent.BackColor;

            if ((generalManual.FinalesCarreraWrk[0] != null))
                lbSt1.Text = generalManual.FinalesCarreraWrk[0].ToString();
            else
                lbSt1.BackColor = lbSt1.Parent.BackColor;

            if ((generalManual.FinalesCarreraWrk[1] != null))
                lbSt2.Text = generalManual.FinalesCarreraWrk[1].ToString();
            else
                lbSt2.BackColor = lbSt2.Parent.BackColor;


            aR = new ActuadorP(generalManual.ActuadoresBas[0]);
            sR = new SensorP(generalManual.FinalesCarreraBas[0]);
            aT1 = new ActuadorP(generalManual.ActuadoresWrk[0]);
            sT1 = new SensorP(generalManual.FinalesCarreraWrk[0]);
            aT2 = new ActuadorP(generalManual.ActuadoresWrk[1]);
            sT2 = new SensorP(generalManual.FinalesCarreraWrk[1]);
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

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            btT1 = new Button();
            btT2 = new Button();
            lbCom = new Label();
            lbSt1 = new Label();
            lbAt1 = new Label();
            lbAt2 = new Label();
            lbSt2 = new Label();
            Label1 = new Label();
            lbAr = new Label();
            lbSr = new Label();
            btR = new Button();
            SuspendLayout();
            // 
            // btT1
            // 
            btT1.Location = new Point(113, 42);
            btT1.Name = "btT1";
            btT1.Size = new Size(88, 27);
            btT1.TabIndex = 0;
            btT1.Text = "Trabajo 1";
            btT1.Click += btTra1_Click;
            // 
            // btT2
            // 
            btT2.Location = new Point(536, 42);
            btT2.Name = "btT2";
            btT2.Size = new Size(88, 28);
            btT2.TabIndex = 1;
            btT2.Text = "Trabajo 2";
            btT2.Click += btTra2_Click;
            // 
            // lbCom
            // 
            lbCom.BackColor = SystemColors.ControlLight;
            lbCom.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, ((0)));
            lbCom.Location = new Point(113, 4);
            lbCom.Name = "lbCom";
            lbCom.Size = new Size(508, 35);
            lbCom.TabIndex = 2;
            lbCom.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbSt1
            // 
            lbSt1.BackColor = Color.White;
            lbSt1.BorderStyle = BorderStyle.Fixed3D;
            lbSt1.Location = new Point(63, 81);
            lbSt1.Name = "lbSt1";
            lbSt1.Size = new Size(88, 20);
            lbSt1.TabIndex = 3;
            lbSt1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbAt1
            // 
            lbAt1.BackColor = Color.White;
            lbAt1.BorderStyle = BorderStyle.Fixed3D;
            lbAt1.Location = new Point(162, 81);
            lbAt1.Name = "lbAt1";
            lbAt1.Size = new Size(88, 20);
            lbAt1.TabIndex = 4;
            lbAt1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbAt2
            // 
            lbAt2.BackColor = Color.White;
            lbAt2.BorderStyle = BorderStyle.Fixed3D;
            lbAt2.Location = new Point(585, 81);
            lbAt2.Name = "lbAt2";
            lbAt2.Size = new Size(80, 20);
            lbAt2.TabIndex = 6;
            lbAt2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbSt2
            // 
            lbSt2.BackColor = Color.White;
            lbSt2.BorderStyle = BorderStyle.Fixed3D;
            lbSt2.Location = new Point(486, 81);
            lbSt2.Name = "lbSt2";
            lbSt2.Size = new Size(88, 20);
            lbSt2.TabIndex = 5;
            lbSt2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Label1
            // 
            Label1.BackColor = Color.LightGray;
            Label1.Location = new Point(-3, 109);
            Label1.Name = "Label1";
            Label1.Size = new Size(727, 12);
            Label1.TabIndex = 7;
            // 
            // lbAr
            // 
            lbAr.BackColor = Color.White;
            lbAr.BorderStyle = BorderStyle.Fixed3D;
            lbAr.Location = new Point(377, 81);
            lbAr.Name = "lbAr";
            lbAr.Size = new Size(88, 20);
            lbAr.TabIndex = 10;
            lbAr.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbSr
            // 
            lbSr.BackColor = Color.White;
            lbSr.BorderStyle = BorderStyle.Fixed3D;
            lbSr.Location = new Point(278, 81);
            lbSr.Name = "lbSr";
            lbSr.Size = new Size(88, 20);
            lbSr.TabIndex = 9;
            lbSr.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btR
            // 
            btR.Location = new Point(329, 43);
            btR.Name = "btR";
            btR.Size = new Size(88, 27);
            btR.TabIndex = 8;
            btR.Text = "Reposo";
            btR.Click += btRep_Click;
            // 
            // controlActuadorGiro3Posiciones
            // 
            Controls.Add(lbAr);
            Controls.Add(lbSr);
            Controls.Add(btR);
            Controls.Add(Label1);
            Controls.Add(lbAt2);
            Controls.Add(lbSt2);
            Controls.Add(lbAt1);
            Controls.Add(lbSt1);
            Controls.Add(lbCom);
            Controls.Add(btT2);
            Controls.Add(btT1);
            Cursor = Cursors.Hand;
            Name = "controlActuadorGiro3Posiciones";
            Size = new Size(724, 123);
            ResumeLayout(false);
        }

        #endregion

        private readonly ActuadorP aR;
        private readonly ActuadorP aT1;
        private readonly ActuadorP aT2;

        private readonly ISensor sR;
        private readonly ISensor sT1;
        private readonly ISensor sT2;
        internal Button btR;

        internal Label lbAr;
        internal Label lbSr;

        #region IFitWidthToPanel Members

        public void FitWidthToPanel(Panel _container)
        {
            //this.Location.X = (_container.Width - this.Size.Width);
        }

        #endregion

        #region IRefrescable Members

        public virtual void Refresh_()
        {
            lbSr.BackColor = sR.Value() ? Color.Green : Color.Yellow;

            lbSt1.BackColor = sT1.Value() ? Color.Green : Color.Yellow;

            lbSt2.BackColor = sT2.Value() ? Color.Green : Color.Yellow;

            lbAr.BackColor = aR.Value() ? Color.Green : Color.Yellow;

            lbAt1.BackColor = aT1.Value() ? Color.Green : Color.Yellow;

            lbAt2.BackColor = aT2.Value() ? Color.Green : Color.Yellow;
        }

        #endregion

        public event click_EventHandler click_;

        protected virtual void btRep_Click(object sender, EventArgs e)
        {
            aT1.Activate(false);
            aT2.Activate(false);
            aR.Activate(true);
        }

        protected void btTra1_Click(object sender, EventArgs e)
        {
            aT1.Activate(true);
            aT2.Activate(false);
            aR.Activate(false);
        }

        protected void btTra2_Click(object sender, EventArgs e)
        {
            aT1.Activate(false);
            aT2.Activate(true);
            aR.Activate(false);
        }
    }
}