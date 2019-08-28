using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals;
using Idpsa.Control.Sequence;


namespace Idpsa.Control.View
{
	public class ControlJog : System.Windows.Forms.UserControl,IRefrescable,IRun
	{
		private IJog _device;
        private Action _refreshDelegate;
        private Func<bool> _isActive;
        private Action _activate;
		private bool _btnPosPress;
		private bool _btnNegPress;
		
		
		
		private TON TimerReset = new TON();
				      
        internal Label lbPosicion;
        internal Label lbP;
        internal Label lbVelocidad;
        internal Label lbV;
		
        
		#region " Código generado por el Diseñador de Windows Forms "

        private ControlJog()
            : base()
        {
            //El Diseñador de Windows Forms requiere esta llamada.

            InitializeComponent();            
        }

        public ControlJog(Manual manual)
            : base()
           {
			//El Diseñador de Windows Forms requiere esta llamada.
               
			InitializeComponent();
            Initialize(manual);
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
		internal System.Windows.Forms.Label lbCom;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.Button btnNeg;
        internal System.Windows.Forms.Button btnPos;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
            this.lbCom = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.btnNeg = new System.Windows.Forms.Button();
            this.btnPos = new System.Windows.Forms.Button();
            this.lbPosicion = new System.Windows.Forms.Label();
            this.lbP = new System.Windows.Forms.Label();
            this.lbVelocidad = new System.Windows.Forms.Label();
            this.lbV = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbCom
            // 
            this.lbCom.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lbCom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCom.Location = new System.Drawing.Point(207, 9);
            this.lbCom.Name = "lbCom";
            this.lbCom.Size = new System.Drawing.Size(223, 34);
            this.lbCom.TabIndex = 79;
            this.lbCom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.Gainsboro;
            this.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.Label1.Location = new System.Drawing.Point(0, 76);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(641, 12);
            this.Label1.TabIndex = 87;
            // 
            // btnNeg
            // 
            this.btnNeg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(150)))));
            this.btnNeg.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNeg.Location = new System.Drawing.Point(78, 9);
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
            this.btnPos.Location = new System.Drawing.Point(471, 9);
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
            this.lbPosicion.AutoSize = true;
            this.lbPosicion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lbPosicion.Location = new System.Drawing.Point(167, 54);
            this.lbPosicion.Name = "lbPosicion";
            this.lbPosicion.Size = new System.Drawing.Size(53, 13);
            this.lbPosicion.TabIndex = 97;
            this.lbPosicion.Text = "Posicion :";
            // 
            // lbP
            // 
            this.lbP.BackColor = System.Drawing.Color.Black;
            this.lbP.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lbP.ForeColor = System.Drawing.Color.White;
            this.lbP.Location = new System.Drawing.Point(226, 48);
            this.lbP.Name = "lbP";
            this.lbP.Size = new System.Drawing.Size(77, 24);
            this.lbP.TabIndex = 96;
            this.lbP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbVelocidad
            // 
            this.lbVelocidad.AutoSize = true;
            this.lbVelocidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lbVelocidad.Location = new System.Drawing.Point(320, 54);
            this.lbVelocidad.Name = "lbVelocidad";
            this.lbVelocidad.Size = new System.Drawing.Size(60, 13);
            this.lbVelocidad.TabIndex = 99;
            this.lbVelocidad.Text = "Velocidad :";
            // 
            // lbV
            // 
            this.lbV.BackColor = System.Drawing.Color.Black;
            this.lbV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbV.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lbV.ForeColor = System.Drawing.Color.White;
            this.lbV.Location = new System.Drawing.Point(387, 48);
            this.lbV.Name = "lbV";
            this.lbV.Size = new System.Drawing.Size(77, 24);
            this.lbV.TabIndex = 98;
            this.lbV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // controlJog
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lbVelocidad);
            this.Controls.Add(this.lbV);
            this.Controls.Add(this.lbPosicion);
            this.Controls.Add(this.lbP);
            this.Controls.Add(this.btnPos);
            this.Controls.Add(this.btnNeg);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.lbCom);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "controlJog";
            this.Size = new System.Drawing.Size(636, 87);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        private void Initialize(Manual manual)
        {
            lbCom.Text = manual.Descripcion;
            _device = (IJog)manual.RepresentedInstance;


            if (!String.IsNullOrEmpty(_device.JogNegName))
            {
                this.btnNeg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8f, FontStyle.Regular);
                this.btnNeg.Text = _device.JogNegName;
            }

            if (!String.IsNullOrEmpty(_device.JogPosName))
            {
                this.btnPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8f, FontStyle.Regular);
                this.btnPos.Text = _device.JogPosName;
            }

            _refreshDelegate += () => { };
            var positionMonotorizable = _device as IPositionMonotorizable;
            if (positionMonotorizable != null)
            {
                _refreshDelegate += () => lbP.Text = positionMonotorizable.GetPosition().ToString();
            }
            else
            {
                lbP.Visible = false;
                lbPosicion.Visible = false;
            }

            var velocityMonotorizable = _device as IVelocityMonotorizable;
            if (velocityMonotorizable != null)
            {
                _refreshDelegate += () => lbV.Text = velocityMonotorizable.GetVelocity().ToString();
            }
            else
            {
                lbV.Visible = false;
                lbVelocidad.Visible = false;
            }

            _isActive = () => true;
            _activate = () => { };
            var reactivable = _device as Ireactivable;
            if (reactivable != null)
            {
                _isActive = reactivable.IsActive;
                _activate = reactivable.Reactivate;
            }


            //lbP.Text = Parker.Posicion.ToString();
            // lbV.Text = Parker.Velocidad.ToString();
            // lbEstado.Text = Parker.CurrentState();
            // lblMotorError.BackColor = (Parker.LastError().Trim().ToUpper() == "NONE") ? Color.LightGreen : Color.Salmon;
            // lblMotorError.Text = Parker.LastError();
            //Agregar cualquier inicialización después de la llamada a InitializeComponent()

        }

		public void Run()
		{			
			JogControl();
		}       

        public void RefreshView()
        {
            _refreshDelegate();
        }       

        private void JogControl()
        {
            bool press = this._btnNegPress | this._btnPosPress;
            if (press == true)
            {
                if (!_isActive())
                    _activate();
                else
                    if (_btnPosPress)
                    {
                        if (_device.EnableJogPos())
                            _device.JogPos();
                    }
                    else
                    {
                        if(_device.EnableJogNeg())
                            _device.JogNeg();
                    }
            }
        }

        	
		private void btnNeg_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            if (e.Button == MouseButtons.Left)
            {                   
                _btnNegPress = true;
                _device.StopJog();
                btnNeg.BackColor = Color.FromArgb(128, 255, 128);
            }

		}

		private void btnNeg_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
		    if (e.Button == MouseButtons.Left)
		    {                
                _btnNegPress = false;
                _device.StopJog();
                btnNeg.BackColor = Color.FromArgb(255, 255, 128);
		    }

	    }


		private void btnPos_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
          
            if (e.Button == MouseButtons.Left)
            {                
                _btnPosPress = true;
                _device.StopJog();
                btnPos.BackColor = Color.FromArgb(128, 255, 128);
            }
		}

		private void btnPos_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
		    {                
                _btnPosPress = false;
                _device.StopJog();		        
		        btnPos.BackColor = Color.FromArgb(255, 255, 128);
		    }
		}

		



      

       

       
        
        

        


     
	}
}
