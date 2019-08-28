using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.View
{
    public class ControlGeneralManual : UserControl, IRefrescable
    {
        private IEvaluable sT;
        private IEvaluable sR;
        private IActivable aT;
        private IActivable aR;

        private Button btRep;
        private Button btTra;
        private Func<bool> _workEnable;
        private Func<bool> _restEnable;

        internal Label lbCom;
        internal Label lbAt;
        internal Label lbSr;
        internal Label lbAr;
        internal Label lbSt;
        internal Label Label1;



        #region " Código generado por el Diseñador de Windows Forms "
        public ControlGeneralManual(Manual m)
            : base()
        {
            //El Diseñador de Windows Forms requiere esta llamada.

            InitializeComponent();

            Initialize(m);
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
        private IContainer components = null;

        //NOTA: el Diseñador de Windows Forms requiere el siguiente procedimiento
        //Puede modificarse utilizando el Diseñador de Windows Forms. 
        //No lo modifique con el editor de código.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.lbAt = new System.Windows.Forms.Label();
            this.btRep = new System.Windows.Forms.Button();
            this.btTra = new System.Windows.Forms.Button();
            this.lbCom = new System.Windows.Forms.Label();
            this.lbSr = new System.Windows.Forms.Label();
            this.lbAr = new System.Windows.Forms.Label();
            this.lbSt = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbAt
            // 
            this.lbAt.BackColor = System.Drawing.Color.White;
            this.lbAt.Location = new System.Drawing.Point(578, 36);
            this.lbAt.Name = "lbAt";
            this.lbAt.Size = new System.Drawing.Size(108, 20);
            this.lbAt.TabIndex = 6;
            this.lbAt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btRep
            // 
            this.btRep.Location = new System.Drawing.Point(114, 6);
            this.btRep.Name = "btRep";
            this.btRep.Size = new System.Drawing.Size(98, 24);
            this.btRep.TabIndex = 0;
            this.btRep.Text = "Reposo";
            this.btRep.Click += new System.EventHandler(this.btRep_Click);
            // 
            // btTra
            // 
            this.btTra.Location = new System.Drawing.Point(520, 6);
            this.btTra.Name = "btTra";
            this.btTra.Size = new System.Drawing.Size(98, 24);
            this.btTra.TabIndex = 1;
            this.btTra.Text = "Trabajo";
            this.btTra.Click += new System.EventHandler(this.btTra_Click);
            // 
            // lbCom
            // 
            this.lbCom.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lbCom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCom.Location = new System.Drawing.Point(232, 6);
            this.lbCom.Name = "lbCom";
            this.lbCom.Size = new System.Drawing.Size(256, 44);
            this.lbCom.TabIndex = 2;
            this.lbCom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbSr
            // 
            this.lbSr.BackColor = System.Drawing.Color.White;
            this.lbSr.Location = new System.Drawing.Point(65, 36);
            this.lbSr.Name = "lbSr";
            this.lbSr.Size = new System.Drawing.Size(112, 20);
            this.lbSr.TabIndex = 3;
            this.lbSr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbAr
            // 
            this.lbAr.BackColor = System.Drawing.Color.White;
            this.lbAr.Location = new System.Drawing.Point(181, 36);
            this.lbAr.Name = "lbAr";
            this.lbAr.Size = new System.Drawing.Size(115, 20);
            this.lbAr.TabIndex = 4;
            this.lbAr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbSt
            // 
            this.lbSt.BackColor = System.Drawing.Color.White;
            this.lbSt.Location = new System.Drawing.Point(461, 36);
            this.lbSt.Name = "lbSt";
            this.lbSt.Size = new System.Drawing.Size(113, 20);
            this.lbSt.TabIndex = 5;
            this.lbSt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.Gainsboro;
            this.Label1.Location = new System.Drawing.Point(0, 65);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(722, 10);
            this.Label1.TabIndex = 7;
            // 
            // ControlGeneralManual
            // 
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.lbAt);
            this.Controls.Add(this.lbSt);
            this.Controls.Add(this.lbAr);
            this.Controls.Add(this.lbSr);
            this.Controls.Add(this.lbCom);
            this.Controls.Add(this.btTra);
            this.Controls.Add(this.btRep);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "ControlGeneralManual";
            this.Size = new System.Drawing.Size(722, 73);
            this.ResumeLayout(false);

        }
        #endregion



        public void Initialize(Manual manual)
        {

            Label lbTV = null, lbTIV = null, lbRV = null, lbRIV = null;

            lbCom.Text = manual.Description;
            btRep.Font = new Font("Microsoft Sans Serif", 8f);
            btTra.Font = new Font("Microsoft Sans Serif", 8f);

            var generalManual = (GeneralManual)manual.RepresentedInstance;

            btRep.Text = generalManual.RestName;
            btTra.Text = generalManual.WorkName;

            if ((generalManual.ActuatorRest != null))
            {
                lbAr.Text = generalManual.ActuatorRest.ToString();
                lbRV = lbAr;
            }
            else
            {
                lbAr.Visible = false;
                lbRIV = lbAr;
            }
            if ((generalManual.ActuatorWrk != null))
            {

                lbAt.Text = generalManual.ActuatorWrk.ToString();
                lbTV = lbAt;
            }
            else
            {
                lbAt.Visible = false;
                lbTIV = lbAt;
            }

            if ((generalManual.ActuatorWrk == null) && generalManual.ActuatorRest == null)
            {
                btRep.Visible = btTra.Visible = false;
            }

            if ((generalManual.LimitSwithRest != null))
            {
                lbSr.Text = generalManual.LimitSwithRest.ToString();
                lbRV = lbSr;
            }
            else
            {
                lbSr.Visible = false;
                lbRIV = lbSr;
            }
            if ((generalManual.LimitSwithWork != null))
            {
                lbSt.Text = generalManual.LimitSwithWork.ToString();
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

            aT = generalManual.ActuatorWrk ?? Output.Simulated;
            aR = generalManual.ActuatorRest ?? Output.Simulated;
            sR = generalManual.LimitSwithRest ?? Input.Simulated;
            sT = generalManual.LimitSwithWork ?? Input.Simulated;

            _workEnable = generalManual.WorkEnable;
            _restEnable = generalManual.RestEnable;
        }

        public virtual void RefreshView()
        {
            lbSr.Text = sR.ToString();
            lbSr.BackColor = sR.Value() ? Color.Green : Color.Yellow;

            lbSt.Text = sT.ToString();
            lbSt.BackColor = sT.Value() ? Color.Green : Color.Yellow;

            lbAt.Text = aT.ToString();
            lbAt.BackColor = aT.Value() ? Color.Green : Color.Yellow;

            lbAr.Text = aR.ToString();
            lbAr.BackColor = aR.Value() ? Color.Green : Color.Yellow;
        }


        protected virtual void btRep_Click(object sender, EventArgs e)
        {
            if (_restEnable())
            {
                aT.Activate(false);
                aR.Activate(true);
            }
        }
        protected virtual void btTra_Click(object sender, EventArgs e)
        {
            if (_workEnable())
            {
                aT.Activate(true);
                aR.Activate(false);
            }

        }




    }
}
