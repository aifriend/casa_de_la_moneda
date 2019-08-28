using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.User;
using Idpsa.Controller.Properties;
using Microsoft.VisualBasic;

namespace Idpsa
{
	public class FormAccessParker : Form
	{
		#region " Código generado por el Diseñador de Windows Forms "
		public FormAccessParker() : base()
		{
			//El Diseñador de Windows Forms requiere esta llamada.
			InitializeComponent();
			//Agregar cualquier inicialización después de la llamada a InitializeComponent()
		}

       
		//Form reemplaza a Dispose para limpiar la lista de componentes.
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
		internal Panel Panel1;
		internal Label Label1;
		internal Label Label2;
		internal Label Label3;
		internal Button Button1;
		internal Button Button2;
		internal TextBox tbUsuario;
		internal TextBox tbContraseña;
		[DebuggerStepThrough()]
		private void InitializeComponent()
		{
            var resources = new ComponentResourceManager(typeof(FormAccessParker));
            tbUsuario = new TextBox();
            tbContraseña = new TextBox();
            Label1 = new Label();
            Button1 = new Button();
            Button2 = new Button();
            Label3 = new Label();
            Label2 = new Label();
            Panel1 = new Panel();
            SuspendLayout();
            // 
            // tbUsuario
            // 
            tbUsuario.Location = new Point(158, 112);
            tbUsuario.Name = "tbUsuario";
            tbUsuario.Size = new Size(152, 20);
            tbUsuario.TabIndex = 0;
            tbUsuario.KeyPress += new KeyPressEventHandler(tbUsuario_KeyPress);
            // 
            // tbContraseña
            // 
            tbContraseña.AccessibleDescription = "";
            tbContraseña.Location = new Point(158, 144);
            tbContraseña.Name = "tbContraseña";
            tbContraseña.PasswordChar = '*';
            tbContraseña.Size = new Size(152, 20);
            tbContraseña.TabIndex = 1;
            tbContraseña.KeyPress += new KeyPressEventHandler(tbContraseña_KeyPress);
            // 
            // Label1
            // 
            Label1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            Label1.Location = new Point(6, 80);
            Label1.Name = "Label1";
            Label1.Size = new Size(121, 16);
            Label1.TabIndex = 3;
            Label1.Text = "Control de acceso:";
            // 
            // Button1
            //             
            Button1.ImageAlign = ContentAlignment.MiddleLeft;
            Button1.Location = new Point(237, 192);
            Button1.Name = "Button1";
            Button1.Size = new Size(75, 24);
            Button1.TabIndex = 6;
            Button1.Text = "Cancelar";
            Button1.TextAlign = ContentAlignment.MiddleRight;
            Button1.Click += new EventHandler(Button1_Click);
            // 
            // Button2
            //            
            Button2.ImageAlign = ContentAlignment.MiddleLeft;
            Button2.Location = new Point(147, 192);
            Button2.Name = "Button2";
            Button2.Size = new Size(70, 24);
            Button2.TabIndex = 7;
            Button2.Text = "Aceptar";
            Button2.TextAlign = ContentAlignment.MiddleRight;
            Button2.Click += new EventHandler(Button2_Click);
            // 
            // Label3
            // 
//            Label3.Image = Resources.Kgpg_identity;
            Label3.ImageAlign = ContentAlignment.MiddleLeft;
            Label3.Location = new Point(6, 146);
            Label3.Name = "Label3";
            Label3.Size = new Size(112, 16);
            Label3.TabIndex = 5;
            Label3.Text = "Clave de acceso:";
            Label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Label2
            // 
//            Label2.Image = Resources.Kontact_contacts;
            Label2.ImageAlign = ContentAlignment.MiddleLeft;
            Label2.Location = new Point(6, 116);
            Label2.Name = "Label2";
            Label2.Size = new Size(121, 16);
            Label2.TabIndex = 4;
            Label2.Text = "Nombre de usuario:";
            Label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Panel1
            // 
            Panel1.BackgroundImage = ((Image)(resources.GetObject("Panel1.BackgroundImage")));
            Panel1.Location = new Point(0, 0);
            Panel1.Name = "Panel1";
            Panel1.Size = new Size(320, 64);
            Panel1.TabIndex = 2;
            // 
            // frmAcceso
            // 
            AutoScaleBaseSize = new Size(5, 13);
            ClientSize = new Size(320, 224);
            ControlBox = false;
            Controls.Add(Button2);
            Controls.Add(Button1);
            Controls.Add(Label3);
            Controls.Add(Label2);
            Controls.Add(Label1);
            Controls.Add(Panel1);
            Controls.Add(tbContraseña);
            Controls.Add(tbUsuario);
//            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = ((Icon)(resources.GetObject("$this.Icon")));
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAcceso";
            Text = "Acceso";
            ResumeLayout(false);
            PerformLayout();

		}
		#endregion

        
       
		private void Button2_Click(object sender, EventArgs e)
		{
            if (tbUsuario.Text == "Admin")
                if (tbContraseña.Text == "RCM_2017")
                    DialogResult = DialogResult.OK;
                else
                    MessageBox.Show("La contraseña es incorrecta."); 
            else
                MessageBox.Show("El usuario es incorrecto."); 
		}
		private void tbUsuario_KeyPress(object sender, KeyPressEventArgs e)
		{
			char c;
			c = e.KeyChar;
			if (!(char.IsDigit(c) | char.IsLetter(c) | char.IsControl(c)))
			{
				e.Handled = true;
			}
		}
        private void tbContraseña_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c;
            c = e.KeyChar;
            if (!(char.IsDigit(c) | char.IsLetter(c) | char.IsControl(c)))
            {
                e.Handled = true;
            }
        }

		private void Button1_Click(object sender, EventArgs e)
		{
			tbUsuario.Text = "";
			tbContraseña.Text = "";
			DialogResult = DialogResult.Cancel;
            Dispose();
		}
	}
}
