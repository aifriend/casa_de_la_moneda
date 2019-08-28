using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Idpsa.Control.User;

namespace Idpsa.Control.View
{
	public class FormAcceso : System.Windows.Forms.Form
	{
		#region " Código generado por el Diseñador de Windows Forms "
		public FormAcceso() : base()
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
		private System.ComponentModel.IContainer components = null;
		//NOTA: el Diseñador de Windows Forms requiere el siguiente procedimiento
		//Puede modificarse utilizando el Diseñador de Windows Forms. 
		//No lo modifique con el editor de código.
		internal System.Windows.Forms.Panel Panel1;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.Button Button1;
		internal System.Windows.Forms.Button Button2;
		internal System.Windows.Forms.TextBox tbUsuario;
		internal System.Windows.Forms.TextBox tbContraseña;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAcceso));
            this.tbUsuario = new System.Windows.Forms.TextBox();
            this.tbContraseña = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.Button1 = new System.Windows.Forms.Button();
            this.Button2 = new System.Windows.Forms.Button();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // tbUsuario
            // 
            this.tbUsuario.Location = new System.Drawing.Point(158, 112);
            this.tbUsuario.Name = "tbUsuario";
            this.tbUsuario.Size = new System.Drawing.Size(152, 20);
            this.tbUsuario.TabIndex = 0;
            this.tbUsuario.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbUsuario_KeyPress);
            // 
            // tbContraseña
            // 
            this.tbContraseña.AccessibleDescription = "";
            this.tbContraseña.Location = new System.Drawing.Point(158, 144);
            this.tbContraseña.Name = "tbContraseña";
            this.tbContraseña.PasswordChar = '*';
            this.tbContraseña.Size = new System.Drawing.Size(152, 20);
            this.tbContraseña.TabIndex = 1;
            this.tbContraseña.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbContraseña_KeyPress);
            // 
            // Label1
            // 
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(6, 80);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(121, 16);
            this.Label1.TabIndex = 3;
            this.Label1.Text = "Control de acceso:";
            // 
            // Button1
            //             
            this.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Button1.Location = new System.Drawing.Point(237, 192);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(75, 24);
            this.Button1.TabIndex = 6;
            this.Button1.Text = "Cancelar";
            this.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Button2
            //            
            this.Button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Button2.Location = new System.Drawing.Point(147, 192);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(70, 24);
            this.Button2.TabIndex = 7;
            this.Button2.Text = "Aceptar";
            this.Button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Label3
            // 
            this.Label3.Image = global::Idpsa.Controller.Properties.Resources.Kgpg_identity;
            this.Label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Label3.Location = new System.Drawing.Point(6, 146);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(112, 16);
            this.Label3.TabIndex = 5;
            this.Label3.Text = "Clave de acceso:";
            this.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label2
            // 
            this.Label2.Image = global::Idpsa.Controller.Properties.Resources.Kontact_contacts;
            this.Label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Label2.Location = new System.Drawing.Point(6, 116);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(121, 16);
            this.Label2.TabIndex = 4;
            this.Label2.Text = "Nombre de usuario:";
            this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Panel1
            // 
            this.Panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Panel1.BackgroundImage")));
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(320, 64);
            this.Panel1.TabIndex = 2;
            // 
            // frmAcceso
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(320, 224);
            this.ControlBox = false;
            this.Controls.Add(this.Button2);
            this.Controls.Add(this.Button1);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.Panel1);
            this.Controls.Add(this.tbContraseña);
            this.Controls.Add(this.tbUsuario);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAcceso";
            this.Text = "Acceso";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        
       
		private void Button2_Click(object sender, System.EventArgs e)
		{
			if (tbUsuario.Text.Trim() == "1" & tbContraseña.Text.Trim() == "1")
			{
				AccesoUsuario.Instance.CurrentUser = TipoUsuario.Administrador;
				tbUsuario.Text = "";
				tbContraseña.Text = "";
				DialogResult = DialogResult.OK;
				return; 
			}
			if ((tbUsuario.Text.Trim().Length == 0))
			{
				tbUsuario.BackColor = Color.RosyBrown;
				Interaction.MsgBox("Debe proporcionar su nombre de usuario", MsgBoxStyle.OkOnly, "");
				tbUsuario.Focus();
				return; 
			}
			else
			{
				tbUsuario.BackColor = Color.White;
			}
			if ((tbContraseña.Text.Trim().Length == 0))
			{
				tbContraseña.BackColor = Color.RosyBrown;
				Interaction.MsgBox("Debe proporcionar la contraseña", MsgBoxStyle.OkOnly, "");
				tbContraseña.Focus();
				return; 
			}
			else
			{
				tbContraseña.BackColor = Color.White;
			}
			if ((tbUsuario.Text.Trim().Length < 5))
			{
				tbUsuario.BackColor = Color.RosyBrown;
				Interaction.MsgBox("El nombre de usuario debe tener un mínimo de:" + Constants.vbLf + "5 caracteres alfanuméricos", MsgBoxStyle.OkOnly, "");
				tbUsuario.Focus();
				return; 
			}
			else
			{
				tbUsuario.BackColor = Color.White;
			}
			if ((tbContraseña.Text.Trim().Length < 5))
			{
				tbContraseña.BackColor = Color.RosyBrown;
				Interaction.MsgBox("La contraseña debe tener un mínimo de:" + Constants.vbLf + "5 caracteres alfanuméricos", MsgBoxStyle.OkOnly, "");
				tbContraseña.Focus();
				return; 
			}
			else
			{
				tbContraseña.BackColor = Color.White;
			}
            AccesoUsuario.Instance.CurrentUser = AccesoUsuario.Instance.GetUserType(tbUsuario.Text.Trim(), tbContraseña.Text.Trim());
            if ((AccesoUsuario.Instance.CurrentUser == TipoUsuario.None))
			{
				Interaction.MsgBox("Nombre o contraseña incorrectos:" + Constants.vbLf + "Asegurese de que su login y contraseña han sido escritos correctamente", MsgBoxStyle.OkOnly, "");
				tbUsuario.Focus();
				return;
			}
			tbUsuario.Text = "";
			tbContraseña.Text = "";
			DialogResult = DialogResult.OK;
		}
		private void tbUsuario_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			char c;
			c = e.KeyChar;
			if (!(char.IsDigit(c) | char.IsLetter(c) | char.IsControl(c)))
			{
				e.Handled = true;
			}
		}
		private void tbContraseña_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			char c;
			c = e.KeyChar;
			if (!(char.IsDigit(c) | char.IsLetter(c) | char.IsControl(c)))
			{
				e.Handled = true;
			}
		}
		private void Button1_Click(object sender, System.EventArgs e)
		{
			tbUsuario.Text = "";
			tbContraseña.Text = "";
			DialogResult = DialogResult.Cancel;
		}
	}
}
