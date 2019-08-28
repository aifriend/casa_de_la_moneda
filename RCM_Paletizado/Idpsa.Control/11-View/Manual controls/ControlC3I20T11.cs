using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals;
using Idpsa.Control.Sequence;

namespace Idpsa.Control.View
{
	public class ControlC3I20T11 : UserControl,IRefrescable,IRun
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
            lbCom.Text = manual.Description;
            Parker = (CompaxC3I20T11)manual.RepresentedInstance;

            
            if (!String.IsNullOrEmpty(Parker.JogNegName))
            {
                btnNeg.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Regular);
                btnNeg.Text = Parker.JogNegName;
            }

            if (!String.IsNullOrEmpty(Parker.JogPosName))
            {
                btnPos.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Regular);
                btnPos.Text = Parker.JogPosName;
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
		private IContainer components = null;
		//NOTA: el Diseñador de Windows Forms requiere el siguiente procedimiento
		//Puede modificarse utilizando el Diseñador de Windows Forms. 
		//No lo modifique con el editor de código.
		internal Label Label20;
        internal Label lblMotorError;
		internal Label lbEstado;
		internal Label lbCom;
		internal Label Label1;
		internal Button btnNeg;
        internal Button btnPos;
		[DebuggerStepThrough()]
		private void InitializeComponent()
		{
            Label20 = new Label();
            lblMotorError = new Label();
            lbEstado = new Label();
            lbCom = new Label();
            Label1 = new Label();
            btnNeg = new Button();
            btnPos = new Button();
            _lbPosicion = new Label();
            _lbP = new Label();
            _lbVelocidad = new Label();
            _lbV = new Label();
            SuspendLayout();
            // 
            // Label20
            // 
            Label20.Font = new Font("Verdana", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            Label20.Location = new Point(204, 60);
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
            lblMotorError.Location = new Point(259, 60);
            lblMotorError.Name = "lblMotorError";
            lblMotorError.Size = new Size(357, 23);
            lblMotorError.TabIndex = 76;
            lblMotorError.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbEstado
            // 
            lbEstado.BackColor = Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            lbEstado.BorderStyle = BorderStyle.Fixed3D;
            lbEstado.Font = new Font("Microsoft Sans Serif", 8F);
            lbEstado.ForeColor = Color.White;
            lbEstado.Location = new Point(26, 60);
            lbEstado.Name = "lbEstado";
            lbEstado.Size = new Size(166, 23);
            lbEstado.TabIndex = 71;
            lbEstado.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbCom
            // 
            lbCom.BackColor = SystemColors.ControlLight;
            lbCom.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            lbCom.Location = new Point(299, 12);
            lbCom.Name = "lbCom";
            lbCom.Size = new Size(223, 32);
            lbCom.TabIndex = 79;
            lbCom.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Label1
            // 
            Label1.BackColor = Color.Gainsboro;
            Label1.BorderStyle = BorderStyle.Fixed3D;
            Label1.Font = new Font("Microsoft Sans Serif", 8F);
            Label1.Location = new Point(0, 87);
            Label1.Name = "Label1";
            Label1.Size = new Size(638, 10);
            Label1.TabIndex = 87;
            // 
            // btnNeg
            // 
            btnNeg.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(150)))));
            btnNeg.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            btnNeg.Location = new Point(205, 8);
            btnNeg.Name = "btnNeg";
            btnNeg.Size = new Size(85, 40);
            btnNeg.TabIndex = 88;
            btnNeg.Text = "<";
            btnNeg.UseVisualStyleBackColor = false;
            btnNeg.MouseDown += new MouseEventHandler(btnNeg_MouseDown);
            btnNeg.MouseUp += new MouseEventHandler(btnNeg_MouseUp);
            // 
            // btnPos
            // 
            btnPos.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(150)))));
            btnPos.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            btnPos.Location = new Point(531, 8);
            btnPos.Name = "btnPos";
            btnPos.Size = new Size(85, 40);
            btnPos.TabIndex = 89;
            btnPos.Text = ">";
            btnPos.UseVisualStyleBackColor = false;
            btnPos.MouseDown += new MouseEventHandler(btnPos_MouseDown);
            btnPos.MouseUp += new MouseEventHandler(btnPos_MouseUp);
            // 
            // lbPosicion
            // 
            _lbPosicion.AutoSize = true;
            _lbPosicion.Font = new Font("Microsoft Sans Serif", 8F);
            _lbPosicion.Location = new Point(26, 7);
            _lbPosicion.Name = "lbPosicion";
            _lbPosicion.Size = new Size(53, 13);
            _lbPosicion.TabIndex = 97;
            _lbPosicion.Text = "Posición :";
            // 
            // lbP
            // 
            _lbP.BackColor = Color.White;
            _lbP.BorderStyle = BorderStyle.Fixed3D;
            _lbP.Font = new Font("Microsoft Sans Serif", 8F);
            _lbP.ForeColor = Color.Black;
            _lbP.Location = new Point(89, 4);
            _lbP.Name = "lbP";
            _lbP.Size = new Size(81, 24);
            _lbP.TabIndex = 96;
            _lbP.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbVelocidad
            // 
            _lbVelocidad.AutoSize = true;
            _lbVelocidad.Font = new Font("Microsoft Sans Serif", 8F);
            _lbVelocidad.Location = new Point(26, 37);
            _lbVelocidad.Name = "lbVelocidad";
            _lbVelocidad.Size = new Size(60, 13);
            _lbVelocidad.TabIndex = 99;
            _lbVelocidad.Text = "Velocidad :";
            // 
            // lbV
            // 
            _lbV.BackColor = Color.White;
            _lbV.BorderStyle = BorderStyle.Fixed3D;
            _lbV.Font = new Font("Microsoft Sans Serif", 8F);
            _lbV.ForeColor = Color.Black;
            _lbV.Location = new Point(89, 31);
            _lbV.Name = "lbV";
            _lbV.Size = new Size(81, 24);
            _lbV.TabIndex = 98;
            _lbV.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // controlC3I20T11New
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(_lbVelocidad);
            Controls.Add(_lbV);
            Controls.Add(_lbPosicion);
            Controls.Add(_lbP);
            Controls.Add(btnPos);
            Controls.Add(btnNeg);
            Controls.Add(Label1);
            Controls.Add(lbCom);
            Controls.Add(Label20);
            Controls.Add(lblMotorError);
            Controls.Add(lbEstado);
            Cursor = Cursors.Hand;
            Name = "controlC3I20T11New";
            Size = new Size(636, 98);
            ResumeLayout(false);
            PerformLayout();

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
            bool press = btnNegPress | btnPosPress;
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

        	
		private void btnNeg_MouseDown(object sender, MouseEventArgs e)
		{
            if (e.Button == MouseButtons.Left)
            {                   
                btnNegPress = true;                
                Parker.DriverResurrectionReset();
                btnNeg.BackColor = Color.FromArgb(128, 255, 128);
            }

		}

		private void btnNeg_MouseUp(object sender, MouseEventArgs e)
		{
		    if (e.Button == MouseButtons.Left)
		    {                
                btnNegPress = false;                
                Parker.Jog1 = false;
                Parker.Jog2 = false;
                btnNeg.BackColor = Color.FromArgb(255, 255, 128);
		    }

	    }


		private void btnPos_MouseDown(object sender, MouseEventArgs e)
		{
          
            if (e.Button == MouseButtons.Left)
            {                
                btnPosPress = true;  
                Parker.DriverResurrectionReset();
                btnPos.BackColor = Color.FromArgb(128, 255, 128);
            }
		}

		private void btnPos_MouseUp(object sender, MouseEventArgs e)
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
