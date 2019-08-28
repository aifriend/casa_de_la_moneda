using System;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals;


namespace Idpsa.Control.View
{
    public class ControlGeneralManual : System.Windows.Forms.UserControl, IRefrescable
	{
		private ISensor sT;
        private ISensor sR;
        private ActuadorP aT;
        private Label lbAt;
        private Button btRep;
        private Button btTra;
        private Func<bool> _workEnable;
        private Func<bool> _basEnable;

        internal Label lbCom;
        internal Label lbSr;
        internal Label lbAr;
        internal Label lbSt;
        internal Label Label1;
        protected ActuadorP aR;
		
		
		#region " Código generado por el Diseñador de Windows Forms "
		public ControlGeneralManual(Manual m) : base()
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
		private System.ComponentModel.IContainer components = null;
		//NOTA: el Diseñador de Windows Forms requiere el siguiente procedimiento
		//Puede modificarse utilizando el Diseñador de Windows Forms. 
        //No lo modifique con el editor de código.
		[System.Diagnostics.DebuggerStepThrough()]
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
            this.lbAt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbAt.Location = new System.Drawing.Point(568, 56);
            this.lbAt.Name = "lbAt";
            this.lbAt.Size = new System.Drawing.Size(80, 20);
            this.lbAt.TabIndex = 6;
            this.lbAt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btRep
            // 
            this.btRep.Location = new System.Drawing.Point(120, 8);
            this.btRep.Name = "btRep";
            this.btRep.Size = new System.Drawing.Size(88, 40);
            this.btRep.TabIndex = 0;
            this.btRep.Text = "Reposo";
            this.btRep.Click += new System.EventHandler(this.btRep_Click);
            // 
            // btTra
            // 
            this.btTra.Location = new System.Drawing.Point(512, 8);
            this.btTra.Name = "btTra";
            this.btTra.Size = new System.Drawing.Size(88, 40);
            this.btTra.TabIndex = 1;
            this.btTra.Text = "Trabajo";
            this.btTra.Click += new System.EventHandler(this.btTra_Click);
            // 
            // lbCom
            // 
            this.lbCom.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lbCom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCom.Location = new System.Drawing.Point(232, 8);
            this.lbCom.Name = "lbCom";
            this.lbCom.Size = new System.Drawing.Size(256, 32);
            this.lbCom.TabIndex = 2;
            this.lbCom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbSr
            // 
            this.lbSr.BackColor = System.Drawing.Color.White;
            this.lbSr.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbSr.Location = new System.Drawing.Point(67, 56);
            this.lbSr.Name = "lbSr";
            this.lbSr.Size = new System.Drawing.Size(85, 20);
            this.lbSr.TabIndex = 3;
            this.lbSr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbAr
            // 
            this.lbAr.BackColor = System.Drawing.Color.White;
            this.lbAr.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbAr.Location = new System.Drawing.Point(168, 56);
            this.lbAr.Name = "lbAr";
            this.lbAr.Size = new System.Drawing.Size(88, 20);
            this.lbAr.TabIndex = 4;
            this.lbAr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbSt
            // 
            this.lbSt.BackColor = System.Drawing.Color.White;
            this.lbSt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbSt.Location = new System.Drawing.Point(467, 56);
            this.lbSt.Name = "lbSt";
            this.lbSt.Size = new System.Drawing.Size(85, 20);
            this.lbSt.TabIndex = 5;
            this.lbSt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.Gainsboro;
            this.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Label1.Location = new System.Drawing.Point(0, 88);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(792, 12);
            this.Label1.TabIndex = 7;
            // 
            // control2S2A
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.lbAt);
            this.Controls.Add(this.lbSt);
            this.Controls.Add(this.lbAr);
            this.Controls.Add(this.lbSr);
            this.Controls.Add(this.lbCom);
            this.Controls.Add(this.btTra);
            this.Controls.Add(this.btRep);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "control2S2A";
            this.Size = new System.Drawing.Size(722, 98);
            this.ResumeLayout(false);

		}
		#endregion



        public void Initialize(Manual manual)
        {

            Label lbTV = null, lbTIV = null, lbRV = null, lbRIV = null;

            string[] str;
            char[] d = new char[] { '|' };
            str = manual.Descripcion.Split(d);
            this.lbCom.Text = str[0].Trim();
            this.btRep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8f);
            this.btTra.Font = new System.Drawing.Font("Microsoft Sans Serif", 8f);

            var generalManual = (GeneralManual)manual.RepresentedInstance;

            if (generalManual.LoadedFromFile)
            {
                if (str.Length > 1)
                {
                    if (str[1].Trim().Length > 0)
                    {
                        this.btRep.Text = str[1].Trim();
                    }
                }
                if (str.Length > 2)
                {
                    if (str[2].Trim().Length > 0)
                    {
                        this.btTra.Text = str[2].Trim();
                    }
                }
            }
            else
            {
                this.btRep.Text = generalManual.BasName;
                this.btTra.Text = generalManual.WorkName;
            }        

            
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

            if ((generalManual.ActuadoresWrk[0] == null) && generalManual.ActuadoresBas[0] == null)
            {
                btRep.Visible = btTra.Visible = false;
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

           


            aT = new ActuadorP(generalManual.ActuadoresWrk[0]);
            aR = new ActuadorP(generalManual.ActuadoresBas[0]);
            sR = new SensorP(generalManual.FinalesCarreraBas[0]);
            sT = new SensorP(generalManual.FinalesCarreraWrk[0]);

            _workEnable = generalManual.WorkEnable;
            _basEnable = generalManual.BasEnable;

        }


		public virtual void RefreshView()
		{
			try {
				if (this.lbSr.Text.Trim().Length > 0)
				{
					if (sR.SecureValue())
					{
						this.lbSr.BackColor = Color.Green;
					}
					else
					{
						this.lbSr.BackColor = Color.Yellow;
					}
				}
				if (this.lbSt.Text.Trim().Length > 0)
				{
					if (sT.SecureValue())
					{
						this.lbSt.BackColor = Color.Green;
					}
					else
					{
						this.lbSt.BackColor = Color.Yellow;
					}
				}

                if (this.lbAt.Text.Trim().Length > 0)
                {
                    if (aT.SecureValue())
                    {
                        this.lbAt.BackColor = Color.Green;
                    }
                    else
                    {
                        this.lbAt.BackColor = Color.Yellow;
                    }
                }

                if (this.lbAr.Text.Trim().Length > 0)
                {
                    if (aR.SecureValue())
                    {
                        this.lbAr.BackColor = Color.Green;
                    }
                    else
                    {
                        this.lbAr.BackColor = Color.Yellow;
                    }
                }            
                                
			}
			catch {}
		}
		protected virtual void btRep_Click(object sender, System.EventArgs e)
		{
            if (_basEnable())
            {
                aT.ActivateSecure(false);
                aR.ActivateSecure(true);
            }
		}
		protected virtual void btTra_Click(object sender, System.EventArgs e)
		{
            if (_workEnable())
            {
                aT.ActivateSecure(true);
                aR.ActivateSecure(false);
            }

		}
      

	}
}
