using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals.ViewControl;
using Idpsa.Control.Mode;
using Idpsa.Control.Mode.Manually;

namespace Idpsa.Control.View
{
    public class ControlC3I20T11 : UserControl, IRefrescable, IFitWidthToPanel, IRun
    {
        private readonly double Aceleracion;
        private readonly bool invert;
        private readonly CompaxC3I20T11 Parker;
        private readonly double Velocidad;
        //private string Accion = "";
        //private bool AntesVisible;
        //private bool btnNegClick;
        private bool btnNegPress;
        //private bool btnPosClick;
        private bool btnPosPress;
        //private bool EjecutadoUnaVez;
        //private bool EjecutadoUnaVezJog;
        //private bool ForTheFirstTime = true;
        //private bool Inicializate;
        internal Label lbP;
        internal Label lbPosicion;
        internal Label lbV;
        internal Label lbVelocidad;

        #region " Código generado por el Diseñador de Windows Forms "

        internal Button btnNeg;
        internal Button btnPos;
        private IContainer components;
        internal Label Label1;
        internal Label Label14;
        internal Label Label15;
        internal Label Label20;
        internal Label lbCom;
        internal Label lbEstado;
        internal Label lblMotorError;
        internal TextBox tbA;
        internal TextBox tbV;

        public ControlC3I20T11(Manual manual, Bus bus)
        {
            //El Diseñador de Windows Forms requiere esta llamada.
            InitializeComponent();
            var d = new[] {'|'};
            string text = manual.Descripcion.Replace("C3I20T11-", "");
            var generalManual = (GeneralManual) manual.RepresentedInstance;
            string[] str = text.Trim().Split(d);
            lbCom.Text = str[0].Trim();
            if (str.Length > 1)
            {
                if (str[1].Trim().Length > 0)
                {
                    btnNeg.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Regular);
                    btnNeg.Text = str[1].Trim();
                }
            }
            if (str.Length > 2)
            {
                if (str[2].Trim().Length > 0)
                {
                    btnPos.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Regular);
                    btnPos.Text = str[2].Trim();
                }
            }
            if (str.Length > 3)
                if (str[3].Trim().ToUpper() == "INVERT")
                    invert = true;

            Parker =
                new CompaxC3I20T11(
                    new Address(int.Parse(generalManual.ActuadoresBas[0].ToString()),
                                int.Parse(generalManual.FinalesCarreraBas[0].ToString())),
                    new Address((int.Parse(generalManual.ActuadoresWrk[0].ToString())),
                                int.Parse(generalManual.FinalesCarreraWrk[0].ToString())), bus.InCollection,
                    bus.OutCollection);

            Velocidad = 200;
            Aceleracion = 300;
            tbV.Text = Velocidad.ToString();
            tbA.Text = Aceleracion.ToString();
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
            Label20 = new Label();
            lblMotorError = new Label();
            lbEstado = new Label();
            lbCom = new Label();
            Label1 = new Label();
            btnNeg = new Button();
            btnPos = new Button();
            tbV = new TextBox();
            tbA = new TextBox();
            Label14 = new Label();
            Label15 = new Label();
            lbPosicion = new Label();
            lbP = new Label();
            lbVelocidad = new Label();
            lbV = new Label();
            SuspendLayout();
            // 
            // Label20
            // 
            Label20.Font = new Font("Verdana", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((0)));
            Label20.Location = new Point(208, 60);
            Label20.Name = "Label20";
            Label20.Size = new Size(49, 23);
            Label20.TabIndex = 75;
            Label20.Text = "Error:";
            Label20.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMotorError
            // 
            lblMotorError.BorderStyle = BorderStyle.Fixed3D;
            lblMotorError.Font = new Font("Microsoft Sans Serif", 8F);
            lblMotorError.ForeColor = Color.White;
            lblMotorError.Location = new Point(263, 60);
            lblMotorError.Name = "lblMotorError";
            lblMotorError.Size = new Size(357, 23);
            lblMotorError.TabIndex = 76;
            lblMotorError.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbEstado
            // 
            lbEstado.BackColor = Color.FromArgb(((((0)))), ((((0)))), ((((192)))));
            lbEstado.BorderStyle = BorderStyle.Fixed3D;
            lbEstado.Font = new Font("Microsoft Sans Serif", 8F);
            lbEstado.ForeColor = Color.White;
            lbEstado.Location = new Point(36, 60);
            lbEstado.Name = "lbEstado";
            lbEstado.Size = new Size(166, 23);
            lbEstado.TabIndex = 71;
            lbEstado.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbCom
            // 
            lbCom.BackColor = SystemColors.ControlLight;
            lbCom.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, ((0)));
            lbCom.Location = new Point(211, 12);
            lbCom.Name = "lbCom";
            lbCom.Size = new Size(223, 32);
            lbCom.TabIndex = 79;
            lbCom.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Label1
            // 
            Label1.BackColor = Color.LightGray;
            Label1.Font = new Font("Microsoft Sans Serif", 8F);
            Label1.Location = new Point(0, 92);
            Label1.Name = "Label1";
            Label1.Size = new Size(792, 12);
            Label1.TabIndex = 87;
            // 
            // btnNeg
            // 
            btnNeg.BackColor = Color.FromArgb(((((255)))), ((((255)))), ((((150)))));
            btnNeg.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point, ((0)));
            btnNeg.Location = new Point(119, 8);
            btnNeg.Name = "btnNeg";
            btnNeg.Size = new Size(85, 40);
            btnNeg.TabIndex = 88;
            btnNeg.Text = "<";
            btnNeg.UseVisualStyleBackColor = false;
            btnNeg.Click += btnNeg_Click;
            btnNeg.MouseDown += btnNeg_MouseDown;
            btnNeg.MouseUp += btnNeg_MouseUp;
            // 
            // btnPos
            // 
            btnPos.BackColor = Color.FromArgb(((((255)))), ((((255)))), ((((150)))));
            btnPos.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point, ((0)));
            btnPos.Location = new Point(445, 8);
            btnPos.Name = "btnPos";
            btnPos.Size = new Size(85, 40);
            btnPos.TabIndex = 89;
            btnPos.Text = ">";
            btnPos.UseVisualStyleBackColor = false;
            btnPos.MouseDown += btnPos_MouseDown;
            btnPos.MouseUp += btnPos_MouseUp;
            // 
            // tbV
            // 
            tbV.Font = new Font("Microsoft Sans Serif", 8F);
            tbV.Location = new Point(565, 5);
            tbV.Name = "tbV";
            tbV.Size = new Size(55, 20);
            tbV.TabIndex = 91;
            tbV.TextAlign = HorizontalAlignment.Center;
            // 
            // tbA
            // 
            tbA.Font = new Font("Microsoft Sans Serif", 8F);
            tbA.Location = new Point(565, 31);
            tbA.Name = "tbA";
            tbA.Size = new Size(55, 20);
            tbA.TabIndex = 92;
            tbA.TextAlign = HorizontalAlignment.Center;
            // 
            // Label14
            // 
            Label14.AutoSize = true;
            Label14.Font = new Font("Microsoft Sans Serif", 8F);
            Label14.Location = new Point(537, 8);
            Label14.Name = "Label14";
            Label14.Size = new Size(28, 13);
            Label14.TabIndex = 93;
            Label14.Text = "Vel :";
            // 
            // Label15
            // 
            Label15.AutoSize = true;
            Label15.Font = new Font("Microsoft Sans Serif", 8F);
            Label15.Location = new Point(543, 39);
            Label15.Name = "Label15";
            Label15.Size = new Size(20, 13);
            Label15.TabIndex = 94;
            Label15.Text = "A :";
            // 
            // lbPosicion
            // 
            lbPosicion.AutoSize = true;
            lbPosicion.Font = new Font("Microsoft Sans Serif", 8F);
            lbPosicion.Location = new Point(6, 7);
            lbPosicion.Name = "lbPosicion";
            lbPosicion.Size = new Size(31, 13);
            lbPosicion.TabIndex = 97;
            lbPosicion.Text = "Pos :";
            // 
            // lbP
            // 
            lbP.BackColor = Color.Black;
            lbP.BorderStyle = BorderStyle.Fixed3D;
            lbP.Font = new Font("Microsoft Sans Serif", 8F);
            lbP.ForeColor = Color.White;
            lbP.Location = new Point(36, 3);
            lbP.Name = "lbP";
            lbP.Size = new Size(77, 24);
            lbP.TabIndex = 96;
            lbP.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbVelocidad
            // 
            lbVelocidad.AutoSize = true;
            lbVelocidad.Font = new Font("Microsoft Sans Serif", 8F);
            lbVelocidad.Location = new Point(8, 37);
            lbVelocidad.Name = "lbVelocidad";
            lbVelocidad.Size = new Size(28, 13);
            lbVelocidad.TabIndex = 99;
            lbVelocidad.Text = "Vel :";
            // 
            // lbV
            // 
            lbV.BackColor = Color.Black;
            lbV.BorderStyle = BorderStyle.Fixed3D;
            lbV.Font = new Font("Microsoft Sans Serif", 8F);
            lbV.ForeColor = Color.White;
            lbV.Location = new Point(36, 31);
            lbV.Name = "lbV";
            lbV.Size = new Size(77, 24);
            lbV.TabIndex = 98;
            lbV.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // controlC3I20T11
            // 
            Controls.Add(lbVelocidad);
            Controls.Add(lbV);
            Controls.Add(lbPosicion);
            Controls.Add(lbP);
            Controls.Add(Label15);
            Controls.Add(Label14);
            Controls.Add(tbA);
            Controls.Add(tbV);
            Controls.Add(btnPos);
            Controls.Add(btnNeg);
            Controls.Add(Label1);
            Controls.Add(lbCom);
            Controls.Add(Label20);
            Controls.Add(lblMotorError);
            Controls.Add(lbEstado);
            Cursor = Cursors.Hand;
            Name = "controlC3I20T11";
            Size = new Size(638, 104);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        //private int PasosJog;
        //private int PasosReset;
        //private double SAceleracion;
        //private double SVelocidad;
        //private TON TimerReset = new TON();
        //private bool TodosParados;

        #region IFitWidthToPanel Members

        public void FitWidthToPanel(Panel _container)
        {
            //this.Location.X = (_container.Width - this.Size.Width)/2;
        }

        #endregion

        #region IRefrescable Members

        public void Refresh_()
        {
            lbP.Text = Parker.Posicion.ToString();
            lbV.Text = Parker.Velocidad.ToString();
            lbEstado.Text = Parker.CurrentState();
            lblMotorError.BackColor = (Parker.LastError().Trim().ToUpper() == "NONE") ? Color.LightGreen : Color.Salmon;
            lblMotorError.Text = Parker.LastError();
        }

        #endregion

        #region IRun Members

        public void Run()
        {
            JogControl();
        }

        #endregion

        public ControlC3I20T11 WithJojPosEnable(Func<bool> jojPosEnable)
        {
            Parker.WithJojPosEnable(jojPosEnable);
            return this;
        }

        public ControlC3I20T11 WithJojNegEnable(Func<bool> jojNegEnable)
        {
            Parker.WithJojNegEnable(jojNegEnable);
            return this;
        }

        public ControlC3I20T11 WithJojEnable(Func<bool> jojEnable)
        {
            Parker.WithJojEnable(jojEnable);
            return this;
        }


        private void JogControl()
        {
            bool press = btnNegPress | btnPosPress;
            if (press)
            {
                if (!Parker.EnOperationEnable())
                    Parker.DriverResurrection();
                else if (btnPosPress)
                    Parker.JogPos();
                else
                    Parker.JogNeg();
            }
        }


        private void btnNeg_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!invert)
                    btnNegPress = true;
                else
                    btnPosPress = true;

                Parker.DriverResurrectionReset();
                btnNeg.BackColor = Color.FromArgb(128, 255, 128);
            }
        }

        private void btnNeg_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!invert)
                    btnNegPress = false;
                else
                    btnPosPress = false;

                Parker.Jog1 = false;
                Parker.Jog2 = false;
                btnNeg.BackColor = Color.FromArgb(255, 255, 128);
            }
        }


        private void btnPos_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!invert)
                    btnPosPress = true;
                else
                    btnNegPress = true;

                Parker.DriverResurrectionReset();
                btnPos.BackColor = Color.FromArgb(128, 255, 128);
            }
        }

        private void btnPos_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!invert)
                    btnPosPress = false;
                else
                    btnNegPress = false;

                btnPosPress = false;
                Parker.Jog1 = false;
                Parker.Jog2 = false;

                btnPos.BackColor = Color.FromArgb(255, 255, 128);
            }
        }

        private static void btnNeg_Click(object sender, EventArgs e)
        {
        }
    }
}