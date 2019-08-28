using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.Component;
using Idpsa.Control.Mode;
using Idpsa.Control.Mode.Manually;

namespace Idpsa.Control.View
{
    public class ControlActuadorConInv : UserControl, IRefrescable, IRun
    {
        #region Delegates

        public delegate bool DelSeguridad();

        #endregion

        private readonly ActuadorConInv elemento;
        private readonly SensorP termico;
        //private string Accion = "";
        //private bool AccionStart;
        //private bool AliveClick;
        //private bool AntesVisible;
        //private bool btnAceleracionClick;
        //private bool btnDesPotenciarClick;
        //private bool btnFinClick;
        //private bool btnInicioClick;
        //private bool btnNegClick;
        private bool btnNegPress;
        //private bool btnPosClick;
        //private bool btnPosicionClick;
        private bool btnPosPress;
        //private bool btnPotenciarClick;
        //private bool btnResetClick;
        //private bool btnResetMotorClick;
        //private bool btnVelocidadClick;
        //private bool cmbParkerChange;
        //private bool DeshabilitarClick;
        //private bool HabilitarClick;
        //private bool Inicializate;
        //private bool InicioClick;
        //private bool JogClick;
        //private bool ParadaClick;
        //private string ParkerName;
        //private int ParkerSelected;
        //private bool PuntoClick;
        //private bool rdbJogClick;
        //private bool rdbPuntoClick;
        //private bool ResetClick;
        //private bool ResetMotorClick;
        public DelSeguridad Seguridad;
        public DelSeguridad SeguridadNeg;
        public DelSeguridad SeguridadPos;

        #region " Código generado por el Diseñador de Windows Forms "

        internal Button btnNeg;
        internal Button btnPos;
        private IContainer components;
        internal Label Label1;
        internal Label Label2;
        internal Label lbCom;
        internal Label lbTermico;

        public ControlActuadorConInv()
        {
            InitializeComponent();
        }

        public ControlActuadorConInv(Manual manual)
        {
            //El Diseñador de Windows Forms requiere esta llamada.
            InitializeComponent();
            var d = new[] {'|'};
            string texto = "";
            if (manual.Descripcion.StartsWith("Inversor-"))
            {
                texto = "Inversor-";
            }
            else if (manual.Descripcion.StartsWith("Altivar31-"))
            {
                texto = "Altivar31-";
            }
            string[] str = manual.Descripcion.Substring(texto.Length).Trim().Split(d);
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
            var generalManual = (GeneralManual) manual.RepresentedInstance;
            elemento = new ActuadorConInv(new ActuadorP(generalManual.ActuadoresBas[0]),
                                          new ActuadorP(generalManual.ActuadoresWrk[0]));
            termico = new SensorP(generalManual.FinalesCarreraWrk[0]);
            lbTermico.ForeColor = Color.Black;
            lbTermico.Text = generalManual.FinalesCarreraWrk[0].ToString();
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
            lbTermico = new Label();
            lbCom = new Label();
            Label2 = new Label();
            btnNeg = new Button();
            btnPos = new Button();
            Label1 = new Label();
            btnNeg.MouseDown += btnNeg_MouseDown;
            btnNeg.MouseUp += btnNeg_MouseUp;
            btnPos.MouseDown += btnPos_MouseDown;
            btnPos.MouseUp += btnPos_MouseUp;
            SuspendLayout();
            //
            //lbTermico
            //
            lbTermico.BorderStyle = BorderStyle.Fixed3D;
            lbTermico.ForeColor = Color.White;
            lbTermico.Location = new Point(356, 56);
            lbTermico.Name = "lbTermico";
            lbTermico.Size = new Size(56, 20);
            lbTermico.TabIndex = 74;
            lbTermico.TextAlign = ContentAlignment.MiddleCenter;
            //
            //lbCom
            //
            lbCom.BackColor = SystemColors.ControlLight;
            lbCom.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbCom.Location = new Point(224, 8);
            lbCom.Name = "lbCom";
            lbCom.Size = new Size(256, 32);
            lbCom.TabIndex = 79;
            lbCom.TextAlign = ContentAlignment.MiddleCenter;
            //
            //Label2
            //
            Label2.BackColor = Color.LightGray;
            Label2.Location = new Point(0, 84);
            Label2.Name = "Label2";
            Label2.Size = new Size(792, 12);
            Label2.TabIndex = 87;
            //
            //btnNeg
            //
            btnNeg.BackColor = Color.FromArgb(255, 255, 150);
            btnNeg.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNeg.Location = new Point(120, 4);
            btnNeg.Name = "btnNeg";
            btnNeg.Size = new Size(88, 40);
            btnNeg.TabIndex = 88;
            btnNeg.Text = "<";
            //
            //btnPos
            //
            btnPos.BackColor = Color.FromArgb(255, 255, 150);
            btnPos.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPos.Location = new Point(504, 4);
            btnPos.Name = "btnPos";
            btnPos.Size = new Size(88, 40);
            btnPos.TabIndex = 89;
            btnPos.Text = ">";
            //
            //Label1
            //
            Label1.Location = new Point(292, 60);
            Label1.Name = "Label1";
            Label1.Size = new Size(60, 16);
            Label1.TabIndex = 90;
            Label1.Text = "Térmico :";
            Label1.TextAlign = ContentAlignment.MiddleCenter;
            //
            //controlActuadorConInv
            //
            Controls.Add(Label1);
            Controls.Add(btnPos);
            Controls.Add(btnNeg);
            Controls.Add(Label2);
            Controls.Add(lbCom);
            Controls.Add(lbTermico);
            Name = "controlActuadorConInv";
            Size = new Size(792, 96);
            ResumeLayout(false);
        }

        #endregion

        //private string WorkMode = "P";

        #region IRefrescable Members

        public void Refresh_()
        {
        }

        #endregion

        #region IRun Members

        public void Run()
        {
            RefreshInv();
            ModuloJog2();
        }

        #endregion

        public string Comentario()
        {
            return lbCom.Text;
        }

        public void RefreshInv()
        {
            lbTermico.BackColor = termico.Value() ? Color.Green : Color.Yellow;
        }

        private void ModuloJog2()
        {
            bool press = (btnNegPress & !SegNeg()) | (btnPosPress & !SegPos());
            if (Enabled)
            {
                if (press)
                {
                    if (!(Seguridad == null))
                    {
                        if (Seguridad())
                        {
                            elemento.Deactivate();
                            return;
                        }
                    }
                    if (btnNegPress)
                    {
                        elemento.Activate2();
                    }
                    else if (btnPosPress)
                    {
                        elemento.Activate1();
                    }
                }
                else
                {
                    elemento.Deactivate();
                }
            }
        }

        protected void btnNeg_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                btnNegPress = true;
                btnNeg.BackColor = Color.FromArgb(150, 255, 150);
            }
        }

        protected void btnNeg_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                btnNegPress = false;
                btnNeg.BackColor = Color.FromArgb(255, 255, 150);
            }
        }

        protected void btnPos_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                btnPosPress = true;
                btnPos.BackColor = Color.FromArgb(150, 255, 150);
            }
        }

        protected void btnPos_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                btnPosPress = false;
                btnPos.BackColor = Color.FromArgb(255, 255, 150);
            }
        }

        private bool SegPos()
        {
            bool value = !(SeguridadPos == null) && SeguridadPos();
            return value;
        }

        private bool SegNeg()
        {
            bool value;
            value = !(SeguridadPos == null) && SeguridadNeg();
            return value;
        }
    }
}