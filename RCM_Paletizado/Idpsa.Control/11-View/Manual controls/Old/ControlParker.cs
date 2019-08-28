using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.Component;
using Idpsa.Control.Mode;
using Idpsa.Control.Mode.Manually;
using Microsoft.VisualBasic;

namespace Idpsa.Control.View
{
    public class ControlParker : UserControl, IRefrescable, IRun
    {
        private readonly Parker Parker;
        private readonly TON TimerReset = new TON();
        //private string Accion = "";
        //private bool AccionStart;
        private double Aceleracion;
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
        //private bool EjecutadoUnaVez;
        //private bool EjecutadoUnaVezJog;
        private bool ForTheFirstTime = true;
        //private bool HabilitarClick;
        //private bool Inicializate;
        //private bool InicioClick;
        //private bool JogClick;
        //private bool ParadaClick;
        //private string ParkerName;
        //private int ParkerSelected;
        private int PasosJog;
        //private int PasosReset;
        //private bool PuntoClick;
        //private bool rdbJogClick;
        //private bool rdbPuntoClick;
        //private bool ResetClick;
        //private bool ResetMotorClick;
        //private double SAceleracion;
        private double SVelocidad;
        //private bool TodosParados;
        private double Velocidad;

        #region " Código generado por el Diseñador de Windows Forms "

        //Requerido por el Diseñador de Windows Forms
        internal Button btnNeg;
        internal Button btnPos;
        private IContainer components;
        //NOTA: el Diseñador de Windows Forms requiere el siguiente procedimiento
        //Puede modificarse utilizando el Diseñador de Windows Forms. 
        //No lo modifique con el editor de código.
        internal Label Label1;
        internal Label Label17;
        internal Label Label2;
        internal Label Label20;
        internal Label Label4;
        internal Label Label5;
        internal Label Label6;
        internal Label lbCom;
        internal Label lblAlive;
        internal Label lblMotorError;
        internal Label lblMotorReady;
        internal Label lbP;
        internal TextBox tbA;
        internal TextBox tbV;

        public ControlParker(Manual manual, Bus bus)
        {
            //El Diseñador de Windows Forms requiere esta llamada.
            InitializeComponent();
            var d = new[] {'|'};
            string[] str = manual.Descripcion.Substring("Parker-".Length).Trim().Split(d);
            var generalManual = (GeneralManual) manual.RepresentedInstance;
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
            Parker =
                new Parker(
                    new Address(int.Parse(generalManual.ActuadoresBas[0].ToString()),
                                int.Parse(generalManual.FinalesCarreraBas[0].ToString())),
                    new Address((int.Parse(generalManual.ActuadoresWrk[0].ToString())),
                                int.Parse(generalManual.FinalesCarreraWrk[0].ToString())), bus.InCollection,
                    bus.OutCollection);
            //New clsParker(New clsParker.AddressStruct(24, 0), New clsParker.AddressStruct(25, 0), _
            //                                 dICollectionMaster, dOCollectionMaster))
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
            Label17 = new Label();
            lblMotorReady = new Label();
            Label1 = new Label();
            lblAlive = new Label();
            lbCom = new Label();
            Label2 = new Label();
            btnNeg = new Button();
            btnPos = new Button();
            lbP = new Label();
            tbV = new TextBox();
            tbA = new TextBox();
            Label4 = new Label();
            Label5 = new Label();
            Label6 = new Label();
            btnNeg.MouseDown += btnNeg_MouseDown;
            btnNeg.MouseUp += btnNeg_MouseUp;
            btnPos.MouseDown += btnPos_MouseDown;
            btnPos.MouseUp += btnPos_MouseUp;
            SuspendLayout();
            //
            //Label20
            //
            Label20.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            Label20.Location = new Point(384, 60);
            Label20.Name = "Label20";
            Label20.Size = new Size(96, 23);
            Label20.TabIndex = 75;
            Label20.Text = "Motor Error:";
            Label20.TextAlign = ContentAlignment.MiddleLeft;
            //
            //lblMotorError
            //
            lblMotorError.BorderStyle = BorderStyle.Fixed3D;
            lblMotorError.ForeColor = Color.White;
            lblMotorError.Location = new Point(480, 60);
            lblMotorError.Name = "lblMotorError";
            lblMotorError.Size = new Size(56, 23);
            lblMotorError.TabIndex = 76;
            lblMotorError.TextAlign = ContentAlignment.MiddleCenter;
            //
            //Label17
            //
            Label17.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            Label17.Location = new Point(200, 60);
            Label17.Name = "Label17";
            Label17.Size = new Size(96, 23);
            Label17.TabIndex = 73;
            Label17.Text = "Motor Ready:";
            Label17.TextAlign = ContentAlignment.MiddleLeft;
            //
            //lblMotorReady
            //
            lblMotorReady.BorderStyle = BorderStyle.Fixed3D;
            lblMotorReady.ForeColor = Color.White;
            lblMotorReady.Location = new Point(296, 60);
            lblMotorReady.Name = "lblMotorReady";
            lblMotorReady.Size = new Size(64, 23);
            lblMotorReady.TabIndex = 74;
            lblMotorReady.TextAlign = ContentAlignment.MiddleCenter;
            //
            //Label1
            //
            Label1.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            Label1.Location = new Point(24, 60);
            Label1.Name = "Label1";
            Label1.Size = new Size(40, 23);
            Label1.TabIndex = 70;
            Label1.Text = "Alive:";
            Label1.TextAlign = ContentAlignment.MiddleLeft;
            //
            //lblAlive
            //
            lblAlive.BorderStyle = BorderStyle.Fixed3D;
            lblAlive.ForeColor = Color.White;
            lblAlive.Location = new Point(80, 60);
            lblAlive.Name = "lblAlive";
            lblAlive.Size = new Size(48, 23);
            lblAlive.TabIndex = 71;
            lblAlive.TextAlign = ContentAlignment.MiddleCenter;
            //
            //lbCom
            //
            lbCom.BackColor = SystemColors.ControlLight;
            lbCom.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbCom.Location = new Point(144, 12);
            lbCom.Name = "lbCom";
            lbCom.Size = new Size(256, 32);
            lbCom.TabIndex = 79;
            lbCom.TextAlign = ContentAlignment.MiddleCenter;
            //
            //Label2
            //
            Label2.BackColor = Color.LightGray;
            Label2.Location = new Point(0, 92);
            Label2.Name = "Label2";
            Label2.Size = new Size(792, 12);
            Label2.TabIndex = 87;
            //
            //btnNeg
            //
            btnNeg.BackColor = Color.FromArgb(255, 255, 150);
            btnNeg.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNeg.Location = new Point(40, 8);
            btnNeg.Name = "btnNeg";
            btnNeg.Size = new Size(88, 40);
            btnNeg.TabIndex = 88;
            btnNeg.Text = "<";
            //
            //btnPos
            //
            btnPos.BackColor = Color.FromArgb(255, 255, 150);
            btnPos.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPos.Location = new Point(424, 8);
            btnPos.Name = "btnPos";
            btnPos.Size = new Size(88, 40);
            btnPos.TabIndex = 89;
            btnPos.Text = ">";
            //
            //lbP
            //
            lbP.BackColor = Color.White;
            lbP.BorderStyle = BorderStyle.Fixed3D;
            lbP.ForeColor = Color.Black;
            lbP.Location = new Point(632, 64);
            lbP.Name = "lbP";
            lbP.Size = new Size(48, 24);
            lbP.TabIndex = 90;
            lbP.TextAlign = ContentAlignment.MiddleCenter;
            //
            //tbV
            //
            tbV.Location = new Point(632, 4);
            tbV.Name = "tbV";
            tbV.Size = new Size(48, 20);
            tbV.TabIndex = 91;
            tbV.Text = "";
            tbV.TextAlign = HorizontalAlignment.Center;
            //
            //tbA
            //
            tbA.Location = new Point(632, 36);
            tbA.Name = "tbA";
            tbA.Size = new Size(48, 20);
            tbA.TabIndex = 92;
            tbA.Text = "";
            tbA.TextAlign = HorizontalAlignment.Center;
            //
            //Label4
            //
            Label4.Location = new Point(560, 4);
            Label4.Name = "Label4";
            Label4.Size = new Size(64, 23);
            Label4.TabIndex = 93;
            Label4.Text = "Velocidad :";
            //
            //Label5
            //
            Label5.Location = new Point(560, 36);
            Label5.Name = "Label5";
            Label5.Size = new Size(64, 20);
            Label5.TabIndex = 94;
            Label5.Text = "Aceleración :";
            //
            //Label6
            //
            Label6.Location = new Point(560, 68);
            Label6.Name = "Label6";
            Label6.Size = new Size(64, 20);
            Label6.TabIndex = 95;
            Label6.Text = "Posición :";
            //
            //controlParker
            //
            Controls.Add(Label6);
            Controls.Add(Label5);
            Controls.Add(Label4);
            Controls.Add(tbA);
            Controls.Add(tbV);
            Controls.Add(lbP);
            Controls.Add(btnPos);
            Controls.Add(btnNeg);
            Controls.Add(Label2);
            Controls.Add(lbCom);
            Controls.Add(Label20);
            Controls.Add(lblMotorError);
            Controls.Add(Label17);
            Controls.Add(lblMotorReady);
            Controls.Add(Label1);
            Controls.Add(lblAlive);
            Name = "controlParker";
            Size = new Size(792, 104);
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
            RefreshParker();
            ModuloJog2();
        }

        #endregion

        public void RefreshParker()
        {
            if (lblAlive.Text != Parker.Alive.ToString())
            {
                lblAlive.Text = Parker.Alive.ToString();
                lblAlive.BackColor = lblAlive.Text == "True" ? Color.Green : Color.Red;
            }
            if (lblMotorReady.Text != Parker.ReadyMotor.ToString())
            {
                lblMotorReady.Text = Parker.ReadyMotor.ToString();
                lblMotorReady.BackColor = lblMotorReady.Text == "True" ? Color.Green : Color.Red;
            }
            if (lblMotorError.Text != Parker.MotorError.ToString())
            {
                lblMotorError.Text = Parker.MotorError.ToString();
                lblMotorError.BackColor = lblMotorError.Text == "True" ? Color.Green : Color.Red;
            }
            lbP.Text = Parker.Posicion.ToString();
        }

        private void ModuloJog2()
        {
            bool press = btnNegPress | btnPosPress;
            if (press)
            {
                ForTheFirstTime = false;
                switch (PasosJog)
                {
                    case 0:
                        //Reset a todas las señales
                        if (btnNegPress)
                        {
                            SVelocidad = -Velocidad;
                        }
                        else if (btnPosPress)
                        {
                            SVelocidad = Velocidad;
                        }

                        PasosJog = 1;
                        break;
                    case 1:
                        //Lanzar comando de parada
                        PasosJog = Parker.Origen_MotorError() ? 2 : 4;

                        break;
                    case 2:
                        //Finalizar comando de parada
                        if (Parker.InicioReset())
                        {
                            PasosJog = 3;
                        }

                        break;
                    case 3:
                        if (Parker.FinalReset())
                        {
                            PasosJog = 4;
                        }
                        else
                        {
                            if ((TimerReset.Timing(500)))
                            {
                                PasosJog = 1;
                            }
                        }

                        break;
                    case 4:
                        PasosJog = Parker.ReadyMotor ? 7 : 5;

                        break;
                    case 5:
                        if (Parker.InicioHabilitar())
                        {
                            PasosJog = 6;
                        }

                        break;
                    case 6:
                        if (Parker.FinalHabilitar())
                        {
                            PasosJog = 7;
                        }
                        else
                        {
                            if (TimerReset.Timing(200))
                            {
                                PasosJog = 1;
                            }
                        }

                        break;
                    case 7:
                        Parker.Velocidad = SVelocidad;
                        Parker.Aceleracion = Aceleracion;
                        Parker.Jog = true;
                        Parker.Punto = false;
                        Parker.Inicio = true;
                        PasosJog = 8;
                        break;
                    case 8:
                        if (Parker.Parada)
                        {
                            Parker.Jog = false;
                            Parker.Inicio = false;
                        }

                        if (Parker.Inicio == false & TimerReset.Timing(200))
                        {
                            PasosJog = 1;
                        }

                        break;
                }
            }
            else
            {
                if (ForTheFirstTime == false)
                {
                    Parker.Parada = true;
                    PasosJog = 0;
                }
            }
        }

        private void btnNeg_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!LeerValores())
                {
                    btnNegPress = false;
                    return;
                }
                btnNegPress = true;
                btnNeg.BackColor = Color.FromArgb(150, 255, 150);
            }
        }

        private void btnNeg_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                btnNegPress = false;
                btnNeg.BackColor = Color.FromArgb(255, 255, 150);
            }
        }

        private void btnPos_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!LeerValores())
                {
                    btnPosPress = false;
                    return;
                }
                btnPosPress = true;
                btnPos.BackColor = Color.FromArgb(150, 255, 150);
            }
        }

        private void btnPos_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                btnPosPress = false;
                btnPos.BackColor = Color.FromArgb(255, 255, 150);
            }
        }

        private bool LeerValores()
        {
            bool well = true;
            if (Information.IsNumeric(tbV.Text) && Information.IsNumeric(tbA.Text))
            {
                Velocidad = int.Parse(tbV.Text);
                Aceleracion = int.Parse(tbA.Text);
            }
            else
            {
                Interaction.MsgBox(
                    "El valor de la velocidad o el de la aceleración" + Constants.vbLf + " introducido no es valido",
                    MsgBoxStyle.OkOnly, "");
                well = false;
            }
            return well;
        }
    }
}