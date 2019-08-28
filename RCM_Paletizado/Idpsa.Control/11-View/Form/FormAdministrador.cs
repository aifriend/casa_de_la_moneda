using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Idpsa.Control.User;

namespace IIdpsa.Control.View
{
	public class FormAdministrador : System.Windows.Forms.Form
	{
		#region " Código generado por el Diseñador de Windows Forms "
		public FormAdministrador() : base()
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
		internal System.Windows.Forms.ListView lvUsers;
		internal System.Windows.Forms.ColumnHeader Usuario;
		internal System.Windows.Forms.ColumnHeader Password;
		internal System.Windows.Forms.ColumnHeader Privilegio;
		internal System.Windows.Forms.Button btCerrar;
		internal System.Windows.Forms.GroupBox box;
		internal System.Windows.Forms.TextBox tbUsuario;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.TextBox tbPassword;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.ComboBox cbPrivilegio;
		internal System.Windows.Forms.RadioButton rbNewUser;
		internal System.Windows.Forms.RadioButton rbChangeUser;
		internal System.Windows.Forms.Button btAceptarIntro;
		internal System.Windows.Forms.Button btAceptar;
		internal System.Windows.Forms.Button btEliminar;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAdministrador));
            this.lvUsers = new System.Windows.Forms.ListView();
            this.Usuario = new System.Windows.Forms.ColumnHeader();
            this.Password = new System.Windows.Forms.ColumnHeader();
            this.Privilegio = new System.Windows.Forms.ColumnHeader();
            this.btCerrar = new System.Windows.Forms.Button();
            this.box = new System.Windows.Forms.GroupBox();
            this.btEliminar = new System.Windows.Forms.Button();
            this.btAceptarIntro = new System.Windows.Forms.Button();
            this.cbPrivilegio = new System.Windows.Forms.ComboBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.tbUsuario = new System.Windows.Forms.TextBox();
            this.rbNewUser = new System.Windows.Forms.RadioButton();
            this.rbChangeUser = new System.Windows.Forms.RadioButton();
            this.btAceptar = new System.Windows.Forms.Button();
            this.box.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvUsers
            // 
            this.lvUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Usuario,
            this.Password,
            this.Privilegio});
            this.lvUsers.FullRowSelect = true;
            this.lvUsers.GridLines = true;
            this.lvUsers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvUsers.HideSelection = false;
            this.lvUsers.Location = new System.Drawing.Point(224, 69);
            this.lvUsers.Margin = new System.Windows.Forms.Padding(4);
            this.lvUsers.MultiSelect = false;
            this.lvUsers.Name = "lvUsers";
            this.lvUsers.Size = new System.Drawing.Size(500, 334);
            this.lvUsers.TabIndex = 0;
            this.lvUsers.UseCompatibleStateImageBehavior = false;
            this.lvUsers.View = System.Windows.Forms.View.Details;
            this.lvUsers.SelectedIndexChanged += new System.EventHandler(this.lvUsers_SelectedIndexChanged);
            // 
            // Usuario
            // 
            this.Usuario.Text = "Usuario";
            this.Usuario.Width = 129;
            // 
            // Password
            // 
            this.Password.Text = "Password";
            this.Password.Width = 130;
            // 
            // Privilegio
            // 
            this.Privilegio.Text = "Privilegio";
            this.Privilegio.Width = 113;
            // 
            // btCerrar
            // 
            this.btCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCerrar.Image = global::Idpsa.Controller.Properties.Resources.Fileclose;
            this.btCerrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btCerrar.Location = new System.Drawing.Point(645, 423);
            this.btCerrar.Margin = new System.Windows.Forms.Padding(4);
            this.btCerrar.Name = "btCerrar";
            this.btCerrar.Size = new System.Drawing.Size(79, 28);
            this.btCerrar.TabIndex = 1;
            this.btCerrar.Text = "Cerrar";
            this.btCerrar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btCerrar.Click += new System.EventHandler(this.btCerrar_Click);
            // 
            // box
            // 
            this.box.Controls.Add(this.btEliminar);
            this.box.Controls.Add(this.btAceptarIntro);
            this.box.Controls.Add(this.cbPrivilegio);
            this.box.Controls.Add(this.Label3);
            this.box.Controls.Add(this.Label2);
            this.box.Controls.Add(this.tbPassword);
            this.box.Controls.Add(this.Label1);
            this.box.Controls.Add(this.tbUsuario);
            this.box.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.box.Location = new System.Drawing.Point(21, 59);
            this.box.Margin = new System.Windows.Forms.Padding(4);
            this.box.Name = "box";
            this.box.Padding = new System.Windows.Forms.Padding(4);
            this.box.Size = new System.Drawing.Size(171, 384);
            this.box.TabIndex = 2;
            this.box.TabStop = false;
            this.box.Text = "Selección";
            // 
            // btEliminar
            // 
            this.btEliminar.Image = global::Idpsa.Controller.Properties.Resources.Edit_remove;
            this.btEliminar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btEliminar.Location = new System.Drawing.Point(32, 345);
            this.btEliminar.Margin = new System.Windows.Forms.Padding(4);
            this.btEliminar.Name = "btEliminar";
            this.btEliminar.Size = new System.Drawing.Size(76, 28);
            this.btEliminar.TabIndex = 8;
            this.btEliminar.Text = "Borrar";
            this.btEliminar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btEliminar.Click += new System.EventHandler(this.btEliminar_Click);
            // 
            // btAceptarIntro
            // 
            this.btAceptarIntro.Image = global::Idpsa.Controller.Properties.Resources.Edit_add;
            this.btAceptarIntro.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btAceptarIntro.Location = new System.Drawing.Point(32, 305);
            this.btAceptarIntro.Margin = new System.Windows.Forms.Padding(4);
            this.btAceptarIntro.Name = "btAceptarIntro";
            this.btAceptarIntro.Size = new System.Drawing.Size(76, 28);
            this.btAceptarIntro.TabIndex = 7;
            this.btAceptarIntro.Text = "Añadir";
            this.btAceptarIntro.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btAceptarIntro.Click += new System.EventHandler(this.btAceptarIntro_Click);
            // 
            // cbPrivilegio
            // 
            this.cbPrivilegio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPrivilegio.Items.AddRange(new object[] {
            "Administrador",
            "Mantenimiento",
            "Operario"});
            this.cbPrivilegio.Location = new System.Drawing.Point(11, 256);
            this.cbPrivilegio.Margin = new System.Windows.Forms.Padding(4);
            this.cbPrivilegio.Name = "cbPrivilegio";
            this.cbPrivilegio.Size = new System.Drawing.Size(148, 21);
            this.cbPrivilegio.TabIndex = 6;
            // 
            // Label3
            // 
            this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(11, 226);
            this.Label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(107, 20);
            this.Label3.TabIndex = 5;
            this.Label3.Text = "Privilegio:";
            // 
            // Label2
            // 
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Image = global::Idpsa.Controller.Properties.Resources.Kgpg_identity;
            this.Label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Label2.Location = new System.Drawing.Point(11, 128);
            this.Label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(127, 20);
            this.Label2.TabIndex = 3;
            this.Label2.Text = "Clave de acceso:";
            this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(11, 158);
            this.tbPassword.Margin = new System.Windows.Forms.Padding(4);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(148, 20);
            this.tbPassword.TabIndex = 2;
            // 
            // Label1
            // 
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Image = global::Idpsa.Controller.Properties.Resources.Kontact_contacts;
            this.Label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Label1.Location = new System.Drawing.Point(11, 39);
            this.Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(76, 20);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "Usuario:";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbUsuario
            // 
            this.tbUsuario.Location = new System.Drawing.Point(11, 69);
            this.tbUsuario.Margin = new System.Windows.Forms.Padding(4);
            this.tbUsuario.Name = "tbUsuario";
            this.tbUsuario.Size = new System.Drawing.Size(148, 20);
            this.tbUsuario.TabIndex = 0;
            // 
            // rbNewUser
            // 
            this.rbNewUser.Checked = true;
            this.rbNewUser.Location = new System.Drawing.Point(224, 20);
            this.rbNewUser.Margin = new System.Windows.Forms.Padding(4);
            this.rbNewUser.Name = "rbNewUser";
            this.rbNewUser.Size = new System.Drawing.Size(139, 20);
            this.rbNewUser.TabIndex = 5;
            this.rbNewUser.TabStop = true;
            this.rbNewUser.Text = "Crear Usuario";
            // 
            // rbChangeUser
            // 
            this.rbChangeUser.Location = new System.Drawing.Point(395, 20);
            this.rbChangeUser.Margin = new System.Windows.Forms.Padding(4);
            this.rbChangeUser.Name = "rbChangeUser";
            this.rbChangeUser.Size = new System.Drawing.Size(149, 20);
            this.rbChangeUser.TabIndex = 6;
            this.rbChangeUser.Text = "Modificar Usuario";
            this.rbChangeUser.CheckedChanged += new System.EventHandler(this.rbChangeUser_CheckedChanged);
            // 
            // btAceptar
            // 
            this.btAceptar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btAceptar.Image = global::Idpsa.Controller.Properties.Resources.Button_accept;
            this.btAceptar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btAceptar.Location = new System.Drawing.Point(517, 423);
            this.btAceptar.Margin = new System.Windows.Forms.Padding(4);
            this.btAceptar.Name = "btAceptar";
            this.btAceptar.Size = new System.Drawing.Size(84, 28);
            this.btAceptar.TabIndex = 7;
            this.btAceptar.Text = "Aceptar";
            this.btAceptar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btAceptar.Click += new System.EventHandler(this.btAceptar_Click);
            // 
            // frmAdministrador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 460);
            this.ControlBox = false;
            this.Controls.Add(this.btAceptar);
            this.Controls.Add(this.rbChangeUser);
            this.Controls.Add(this.rbNewUser);
            this.Controls.Add(this.box);
            this.Controls.Add(this.btCerrar);
            this.Controls.Add(this.lvUsers);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAdministrador";
            this.Text = "Administrador de Usuarios";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmAdministrador_Load);
            this.box.ResumeLayout(false);
            this.box.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion
		private bool Actualizate;
		private bool VentanaCargada = false;
        
        

		private void lvUsers_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if ((lvUsers.SelectedItems.Count > 0))
			{
				ListViewItem lvI = lvUsers.SelectedItems[0];
				tbUsuario.Text = lvI.Text;
				this.tbPassword.Text = lvI.SubItems[1].Text;
				string priv = lvI.SubItems[2].Text.Trim().ToUpper();
				for (int i = 0; i <= cbPrivilegio.Items.Count - 1; i++) {
					if ((string)cbPrivilegio.Items[i].ToString().Trim().ToUpper() == priv)
					{
						cbPrivilegio.SelectedIndex = i;
					}
				}
			}
		}
		private void btAceptarIntro_Click(object sender, System.EventArgs e)
		{
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
			if ((tbPassword.Text.Trim().Length < 5))
			{
				tbPassword.BackColor = Color.RosyBrown;
				Interaction.MsgBox("La contraseña debe tener un mínimo de:" + Constants.vbLf + "5 caracteres alfanuméricos", MsgBoxStyle.OkOnly, "");
				tbPassword.Focus();
				return;
			}
			else
			{
				tbPassword.BackColor = Color.White;
			}
			if ((this.rbNewUser.Checked))
			{
				ListViewItem lvI = new ListViewItem();
				
				bool encontrado = false;
                foreach (ListViewItem lv in lvUsers.Items)
                {
					if (lv.Text.Trim() == tbUsuario.Text.Trim())
					{
						encontrado = true;
					}
				}
				if (encontrado == false)
				{
					lvI.Text = tbUsuario.Text.Trim();
					lvI.SubItems.Add(tbPassword.Text);
					lvI.SubItems.Add(cbPrivilegio.Text.ToLower());
					lvUsers.Items.Add(lvI);
				}
				else
				{
					Interaction.MsgBox("No se puede crear el nuevo usuario: " + tbUsuario.Text.Trim() + Constants.vbLf + "el nombre de usuario ya está siendo utilizado", MsgBoxStyle.OkOnly, "");
				}
			}
			else
			{
				if (lvUsers.SelectedItems.Count > 0)
				{
					int index = lvUsers.SelectedItems[0].Index;
					lvUsers.Items[index].SubItems.Clear();
					lvUsers.Items[index].Text = tbUsuario.Text.Trim();
					lvUsers.Items[index].SubItems.Add(tbPassword.Text.Trim());
					lvUsers.Items[index].SubItems.Add(cbPrivilegio.Text.Trim());
				}
			}
		}
		private void rbChangeUser_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rbChangeUser.Checked)
			{
				tbUsuario.Enabled = false;
			}
			else
			{
				tbUsuario.Enabled = true;
			}
		}
		private void btAceptar_Click(object sender, System.EventArgs e)
		{
			AccesoUsuario.Instance.ClearUsers();
			if (Actualizate)
			{
				foreach (ListViewItem lvI in lvUsers.Items) {
					AccesoUsuario.Instance.AddChangeUser(lvI.Text, lvI.SubItems[1].Text, lvI.SubItems[2].Text.Trim().ToUpper());
				}
			}
			AccesoUsuario.Instance.SaveUsers();
			DialogResult = DialogResult.OK;
		}
		private void frmAdministrador_Load(object sender, System.EventArgs e)
		{
			cbPrivilegio.SelectedIndex = 2;
			if (!VentanaCargada)
			{
                System.Collections.Generic.Dictionary<string, AccesoUsuario.PasswordPrivilegio> h = AccesoUsuario.Instance.GetUsers();
				IDictionaryEnumerator en = h.GetEnumerator();
				while (en.MoveNext()) {
					ListViewItem lvI = new ListViewItem();
					lvI.Text = (string)en.Key;
					AccesoUsuario.PasswordPrivilegio pp = (AccesoUsuario.PasswordPrivilegio)en.Value;
					lvI.SubItems.Add(pp.Password);
					lvI.SubItems.Add(pp.Privilegio.ToString());
					lvUsers.Items.Add(lvI);
				}
				Actualizate = true;
				VentanaCargada = true;
			}
		}
		private void btEliminar_Click(object sender, System.EventArgs e)
		{
			if ((lvUsers.SelectedItems.Count > 0))
			{
				lvUsers.SelectedItems[0].Remove();
			}
		}
		private void btCerrar_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
