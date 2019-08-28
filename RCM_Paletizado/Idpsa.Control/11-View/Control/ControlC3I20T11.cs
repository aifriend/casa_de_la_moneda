using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Idpsa.Control.Manuals;
using Idpsa.Control.Component;
using Idpsa.Control.Sequence;

namespace Idpsa.Control.View
{
	public class ControlC3I20T11 : System.Windows.Forms.UserControl,IRefrescable,IRun
	{
		private CompaxC3I20T11 Parker;
		private bool btnPosPress;
		private bool btnNegPress;
		
		private TON TimerReset = new TON();
		
       
        internal Label _lbPosicion;
        internal Label _lbP;
        internal Label _lbVelocidad;
        internal Label _lbV;
		
        
		#region " Código generado por el Diseñador de Windows Forms "

        private ControlC3I20T11()
            : base()
        {
            //El Diseñador de Windows Forms requiere esta llamada.

            InitializeComponent();            
        }
        
        public ControlC3I20T11(Manual manual) : base()
           {
			//El Diseñador de Windows Forms requiere esta llamada.
               
			InitializeComponent();
            Initialize(manual);
		}

        private void Initialize(Manual manual)
        {
            lbCom.Text = manual.Descripcion;
            Parker = (CompaxC3I20T11)manual.RepresentedInstance;

            
            if (!String.IsNullOrEmpty(Parker.JogNegName))
            {
                this.btnNeg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8f, FontStyle.Regular);
                this.btnNeg.Text = Parker.JogNegName;
            }

            if (!String.IsNullOrEmpty(Parker.JogPosName))
            {
                this.btnPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8f, FontStyle.Regular);
                this.btnPos.Text = Parker.JogPosName;
            }
                       
            
           
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
		internal System.Windows.Forms.Label Label20;
        internal System.Windows.Forms.Label lblMotorError;
		internal System.Windows.Forms.Label lbEstado;
		internal System.Windows.Forms.Label lbCom;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.Button btnNeg;
        internal System.Windows.Forms.Button btnPos;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
            this.Label20 = new System.Windows.Forms.Label();
            this.lblMotorError = new System.Windows.Forms.Label();
            this.lbEstado = new System.Windows.Forms.Label();
            this.lbCom = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.btnNeg = new System.Windows.Forms.Button();
            this.btnPos = new System.Windows.Forms.Button();
            this._lbPosicion = new System.Windows.Forms.Label();
            this._lbP = new System.Windows.Forms.Label();
            this._lbVelocidad = new System.Windows.Forms.Label();
            this._lbV = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Label20
            // 
            this.Label20.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label20.Location = new System.Drawing.Point(204, 60);
            this.Label20.Name = "Label20";
            this.Label20.Size = new System.Drawing.Size(49, 23);
            this.Label20.TabIndex = 75;
            this.Label20.Text = "Error:";
            this.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMotorError
            // 
            this.lblMotorError.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMotorError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblMotorError.ForeColor = System.Drawing.Color.White;
            this.lblMotorError.Location = new System.Drawing.Point(259, 60);
            this.lblMotorError.Name = "lblMotorError";
            this.lblMotorError.Size = new System.Drawing.Size(357, 23);
            this.lblMotorError.TabIndex = 76;
            this.lblMotorError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbEstado
            // 
            this.lbEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.lbEstado.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbEstado.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lbEstado.ForeColor = System.Drawing.Color.White;
            this.lbEstado.Location = new System.Drawing.Point(26, 60);
            this.lbEstado.Name = "lbEstado";
            this.lbEstado.Size = new System.Drawing.Size(166, 23);
            this.lbEstado.TabIndex = 71;
            this.lbEstado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbCom
            // 
            this.lbCom.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lbCom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCom.Location = new System.Drawing.Point(299, 12);
            this.lbCom.Name = "lbCom";
            this.lbCom.Size = new System.Drawing.Size(223, 32);
            this.lbCom.TabIndex = 79;
            this.lbCom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.Gainsboro;
            this.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.Label1.Location = new System.Drawing.Point(0, 87);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(638, 10);
            this.Label1.TabIndex = 87;
            // 
            // btnNeg
            // 
            this.btnNeg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(150)))));
            this.btnNeg.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNeg.Location = new System.Drawing.Point(205, 8);
            this.btnNeg.Name = "btnNeg";
            this.btnNeg.Size = new System.Drawing.Size(85, 40);
            this.btnNeg.TabIndex = 88;
            this.btnNeg.Text = "<";
            this.btnNeg.UseVisualStyleBackColor = false;
            this.btnNeg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNeg_MouseDown);
            this.btnNeg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnNeg_MouseUp);
            // 
            // btnPos
            // 
            this.btnPos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(150)))));
            this.btnPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPos.Location = new System.Drawing.Point(531, 8);
            this.btnPos.Name = "btnPos";
            this.btnPos.Size = new System.Drawing.Size(85, 40);
            this.btnPos.TabIndex = 89;
            this.btnPos.Text = ">";
            this.btnPos.UseVisualStyleBackColor = false;
            this.btnPos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPos_MouseDown);
            this.btnPos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPos_MouseUp);
            // 
            // lbPosicion
            // 
            this._lbPosicion.AutoSize = true;
            this._lbPosicion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this._lbPosicion.Location = new System.Drawing.Point(26, 7);
            this._lbPosicion.Name = "lbPosicion";
            this._lbPosicion.Size = new System.Drawing.Size(53, 13);
            this._lbPosicion.TabIndex = 97;
            this._lbPosicion.Text = "Posición :";
            // 
            // lbP
            // 
            this._lbP.BackColor = System.Drawing.Color.White;
            this._lbP.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this._lbP.ForeColor = System.Drawing.Color.Black;
            this._lbP.Location = new System.Drawing.Point(89, 4);
            this._lbP.Name = "lbP";
            this._lbP.Size = new System.Drawing.Size(81, 24);
            this._lbP.TabIndex = 96;
            this._lbP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbVelocidad
            // 
            this._lbVelocidad.AutoSize = true;
            this._lbVelocidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this._lbVelocidad.Location = new System.Drawing.Point(26, 37);
            this._lbVelocidad.Name = "lbVelocidad";
            this._lbVelocidad.Size = new System.Drawing.Size(60, 13);
            this._lbVelocidad.TabIndex = 99;
            this._lbVelocidad.Text = "Velocidad :";
            // 
            // lbV
            // 
            this._lbV.BackColor = System.Drawing.Color.White;
            this._lbV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbV.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this._lbV.ForeColor = System.Drawing.Color.Black;
            this._lbV.Location = new System.Drawing.Point(89, 31);
            this._lbV.Name = "lbV";
            this._lbV.Size = new System.Drawing.Size(81, 24);
            this._lbV.TabIndex = 98;
            this._lbV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // controlC3I20T11New
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this._lbVelocidad);
            this.Controls.Add(this._lbV);
            this.Controls.Add(this._lbPosicion);
            this.Controls.Add(this._lbP);
            this.Controls.Add(this.btnPos);
            this.Controls.Add(this.btnNeg);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.lbCom);
            this.Controls.Add(this.Label20);
            this.Controls.Add(this.lblMotorError);
            this.Controls.Add(this.lbEstado);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "controlC3I20T11New";
            this.Size = new System.Drawing.Size(636, 98);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		public void Run()
		{			
			JogControl();
		}       

        public void RefreshView(){
            _lbP.Text = Parker.Posicion.ToString();
            _lbV.Text = Parker.Velocidad.ToString();
            lbEstado.Text = Parker.CurrentState();
            lblMotorError.BackColor = (Parker.LastError().Trim().ToUpper() == "NONE") ? Color.LightGreen : Color.Salmon;
            lblMotorError.Text = Parker.LastError();
        }

        

        private void JogControl()
        {
            bool press = this.btnNegPress | this.btnPosPress;
            if (press == true)
            {
                if (!Parker.EnOperationEnable())
                    Parker.DriverResurrection();
                else
                    if (btnPosPress)
                        Parker.JogPos();
                    else
                        Parker.JogNeg(); 
            }
        }

        	
		private void btnNeg_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            if (e.Button == MouseButtons.Left)
            {                   
                btnNegPress = true;                
                Parker.DriverResurrectionReset();
                btnNeg.BackColor = Color.FromArgb(128, 255, 128);
            }

		}

		private void btnNeg_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
		    if (e.Button == MouseButtons.Left)
		    {                
                btnNegPress = false;                
                Parker.Jog1 = false;
                Parker.Jog2 = false;
                btnNeg.BackColor = Color.FromArgb(255, 255, 128);
		    }

	    }


		private void btnPos_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
          
            if (e.Button == MouseButtons.Left)
            {                
                btnPosPress = true;  
                Parker.DriverResurrectionReset();
                btnPos.BackColor = Color.FromArgb(128, 255, 128);
            }
		}

		private void btnPos_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
		    {                
                btnPosPress = false; 
		        Parker.Jog1 = false;
		        Parker.Jog2 = false;		        
		        btnPos.BackColor = Color.FromArgb(255, 255, 128);
		    }
		}

	}
}
