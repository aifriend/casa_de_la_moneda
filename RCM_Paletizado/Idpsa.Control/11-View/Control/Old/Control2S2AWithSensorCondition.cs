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
    public class Control2S2AWithSensorCondition : UserControl, IRefrescable, IFitWidthToPanel, IRun
    {
        #region Delegates

        public delegate void click_EventHandler();

        #endregion

        #region " Código generado por el Diseñador de Windows Forms "

        private IContainer components;

        public Control2S2AWithSensorCondition(Manual m)
        {
            //El Diseñador de Windows Forms requiere esta llamada.
            Label lbTV = null, lbTIV = null, lbRV = null, lbRIV = null;
            InitializeComponent();
            var d = new[] {'|'};
            string texto = m.Descripcion.Replace("PasoAPaso-", "");
            string[] str = texto.Split(d);
            lbCom.Text = str[0].Trim();
            if (str.Length > 1)
            {
                if (str[1].Trim().Length > 0)
                {
                    btRep.Font = new Font("Microsoft Sans Serif", 8f);
                    btRep.Text = str[1].Trim();
                }
            }
            if (str.Length > 2)
            {
                if (str[2].Trim().Length > 0)
                {
                    btTra.Font = new Font("Microsoft Sans Serif", 8f);
                    btTra.Text = str[2].Trim();
                }
            }

            var generalManual = (GeneralManual) m.RepresentedInstance;
            if ((generalManual.ActuadoresBas[0] != null))
            {
                lbAr.Text = generalManual.ActuadoresBas[0].ToString();
                lbRV = lbAr;
            }
            else
            {
                lbAr.Visible = false;
                lbRIV = lbAr;
            }
            if ((generalManual.ActuadoresWrk[0] != null))
            {
                lbAt.Text = generalManual.ActuadoresWrk[0].ToString();
                lbTV = lbAt;
            }
            else
            {
                lbAt.Visible = false;
                lbTIV = lbAt;
            }
            if ((generalManual.FinalesCarreraBas[0] != null))
            {
                lbSr.Text = generalManual.FinalesCarreraBas[0].ToString();
                lbRV = lbSr;
            }
            else
            {
                lbSr.Visible = false;
                lbRIV = lbSr;
            }
            if ((generalManual.FinalesCarreraWrk[0] != null))
            {
                lbSt.Text = generalManual.FinalesCarreraWrk[0].ToString();
                lbTV = lbSt;
            }
            else
            {
                lbSt.Visible = false;
                lbTIV = lbSt;
            }

            if ((lbRV != null) && (lbRIV != null))
            {
                lbRV.Location = new Point(btRep.Location.X, lbRV.Location.Y);
            }

            if ((lbTV != null) && (lbTIV != null))
            {
                lbTV.Location = new Point(btTra.Location.X, lbTV.Location.Y);
            }
            lbSensor.Text = generalManual.FinalesCarreraBas[1].ToString();
            Sensor = new SensorN(generalManual.FinalesCarreraBas[1]);
            aT = new ActuadorP(generalManual.ActuadoresWrk[0]);
            aR = new ActuadorP(generalManual.ActuadoresBas[0]);
            sR = new SensorP(generalManual.FinalesCarreraBas[0]);
            sT = new SensorP(generalManual.FinalesCarreraWrk[0]);


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
        //NOTA: el Diseñador de Windows Forms requiere el siguiente procedimiento
        //Puede modificarse utilizando el Diseñador de Windows Forms. 
        //No lo modifique con el editor de código.
        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            lbAt = new Label();
            btRep = new Button();
            btTra = new Button();
            lbCom = new Label();
            lbSr = new Label();
            lbAr = new Label();
            lbSt = new Label();
            Label1 = new Label();
            lbSensor = new Label();
            SuspendLayout();
            // 
            // lbAt
            // 
            lbAt.BackColor = Color.White;
            lbAt.BorderStyle = BorderStyle.Fixed3D;
            lbAt.Location = new Point(568, 56);
            lbAt.Name = "lbAt";
            lbAt.Size = new Size(80, 20);
            lbAt.TabIndex = 6;
            lbAt.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btRep
            // 
            btRep.Location = new Point(120, 8);
            btRep.Name = "btRep";
            btRep.Size = new Size(88, 40);
            btRep.TabIndex = 0;
            btRep.Text = "Reposo";
            btRep.Click += btRep_Click;
            // 
            // btTra
            // 
            btTra.Location = new Point(512, 8);
            btTra.Name = "btTra";
            btTra.Size = new Size(88, 40);
            btTra.TabIndex = 1;
            btTra.Text = "Trabajo";
            btTra.Click += btTra_Click;
            // 
            // lbCom
            // 
            lbCom.BackColor = SystemColors.ControlLight;
            lbCom.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, ((0)));
            lbCom.Location = new Point(232, 8);
            lbCom.Name = "lbCom";
            lbCom.Size = new Size(256, 32);
            lbCom.TabIndex = 2;
            lbCom.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbSr
            // 
            lbSr.BackColor = Color.White;
            lbSr.BorderStyle = BorderStyle.Fixed3D;
            lbSr.Location = new Point(64, 56);
            lbSr.Name = "lbSr";
            lbSr.Size = new Size(88, 20);
            lbSr.TabIndex = 3;
            lbSr.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbAr
            // 
            lbAr.BackColor = Color.White;
            lbAr.BorderStyle = BorderStyle.Fixed3D;
            lbAr.Location = new Point(168, 56);
            lbAr.Name = "lbAr";
            lbAr.Size = new Size(88, 20);
            lbAr.TabIndex = 4;
            lbAr.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbSt
            // 
            lbSt.BackColor = Color.White;
            lbSt.BorderStyle = BorderStyle.Fixed3D;
            lbSt.Location = new Point(464, 56);
            lbSt.Name = "lbSt";
            lbSt.Size = new Size(88, 20);
            lbSt.TabIndex = 5;
            lbSt.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Label1
            // 
            Label1.BackColor = Color.LightGray;
            Label1.Location = new Point(0, 88);
            Label1.Name = "Label1";
            Label1.Size = new Size(792, 12);
            Label1.TabIndex = 7;
            // 
            // lbSensor
            // 
            lbSensor.BackColor = Color.White;
            lbSensor.BorderStyle = BorderStyle.Fixed3D;
            lbSensor.Location = new Point(318, 56);
            lbSensor.Name = "lbSensor";
            lbSensor.Size = new Size(88, 20);
            lbSensor.TabIndex = 8;
            lbSensor.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // control2S2AWithSensorCondition
            // 
            Controls.Add(lbSensor);
            Controls.Add(Label1);
            Controls.Add(lbAt);
            Controls.Add(lbSt);
            Controls.Add(lbAr);
            Controls.Add(lbSr);
            Controls.Add(lbCom);
            Controls.Add(btTra);
            Controls.Add(btRep);
            Cursor = Cursors.Hand;
            Name = "control2S2AWithSensorCondition";
            Size = new Size(724, 100);
            ResumeLayout(false);
        }

        #endregion

        private Action ActionAfterStop = () => { };
        protected ActuadorP aR;
        protected ActuadorP aT;
        protected bool btnWorkClicked;
        protected Button btRep;
        protected Button btTra;
        internal Label Label1;
        internal Label lbAr;
        internal Label lbAt;
        internal Label lbCom;
        internal Label lbSensor;
        internal Label lbSr;
        internal Label lbSt;
        protected ISensor Sensor;
        protected ISensor sR;
        protected ISensor sT;

        private int stepsMov;

        #region IFitWidthToPanel Members

        public void FitWidthToPanel(Panel _container)
        {
            //this.Location.X = (_container.Width - this.Size.Width)/2;       
        }

        #endregion

        #region IRefrescable Members

        public virtual void Refresh_()
        {
            //try
            //{
            if (lbSensor.Text.Trim().Length > 0)
            {
                lbSensor.BackColor = Sensor.SecureValue() ? Color.Green : Color.Yellow;
            }


            if (lbSr.Text.Trim().Length > 0)
            {
                lbSr.BackColor = sR.SecureValue() ? Color.Green : Color.Yellow;
            }
            if (lbSt.Text.Trim().Length > 0)
            {
                lbSt.BackColor = sT.SecureValue() ? Color.Green : Color.Yellow;
            }
            if (lbAt.Text.Trim().Length > 0)
            {
                lbAt.BackColor = aT.SecureValue() ? Color.Green : Color.Yellow;
            }

            if (lbAr.Text.Trim().Length > 0)
            {
                lbAr.BackColor = aR.SecureValue() ? Color.Green : Color.Yellow;
            }


            //}
            //catch (Exception e)
            //{
            //}
        }

        #endregion

        #region IRun Members

        public void Run()
        {
            if (btnWorkClicked)
            {
                if (stepsMov == 0)
                {
                    stepsMov = Sensor.Value() ? 1 : 2;
                }
                else if (stepsMov == 1)
                {
                    if (!Sensor.Value())
                        stepsMov = 2;
                }
                else if (stepsMov == 2)
                {
                    if (Sensor.Value())
                    {
                        Stop();
                        ActionAfterStop();
                        btnWorkClicked = false;
                        stepsMov = 0;
                    }
                }
            }
        }

        #endregion

        public event click_EventHandler click_;

        public Control2S2AWithSensorCondition WithActionAfterStop(Action action)
        {
            ActionAfterStop = action;
            return this;
        }

        private void Stop()
        {
            aT.ActivateSecure(false);
            aR.ActivateSecure(true);
        }

        protected virtual void btRep_Click(object sender, EventArgs e)
        {
            Stop();
        }

        protected virtual void btTra_Click(object sender, EventArgs e)
        {
            aT.ActivateSecure(true);
            aR.ActivateSecure(false);
            btnWorkClicked = true;
            stepsMov = 0;
        }
    }
}