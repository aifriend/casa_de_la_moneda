using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.User;
using Idpsa.Controller.Properties;
using Microsoft.VisualBasic;

namespace Idpsa.Control.View
{
	public class FormUsersAdministrator : Form
	{
		#region " Código generado por el Diseñador de Windows Forms "
		public FormUsersAdministrator() : base()
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
		internal ListView lvUsers;
		internal ColumnHeader Usuario;
		internal ColumnHeader Password;
		internal ColumnHeader Privilegio;
		internal Button btCerrar;
		internal GroupBox box;
		internal TextBox tbUsuario;
		internal Label Label1;
		internal Label Label2;
		internal TextBox tbPassword;
		internal Label Label3;
		internal ComboBox cbPrivilegio;
		internal RadioButton rbNewUser;
		internal RadioButton rbChangeUser;
		internal Button btAceptarIntro;
		internal Button btAceptar;
		internal Button btEliminar;
		[DebuggerStepThrough()]
		private void InitializeComponent()
		{
            var resources = new ComponentResourceManager(typeof(FormUsersAdministrator));
            lvUsers = new ListView();
            Usuario = new ColumnHeader();
            Password = new ColumnHeader();
            Privilegio = new ColumnHeader();
            btCerrar = new Button();
            box = new GroupBox();
            btEliminar = new Button();
            btAceptarIntro = new Button();
            cbPrivilegio = new ComboBox();
            Label3 = new Label();
            Label2 = new Label();
            tbPassword = new TextBox();
            Label1 = new Label();
            tbUsuario = new TextBox();
            rbNewUser = new RadioButton();
            rbChangeUser = new RadioButton();
            btAceptar = new Button();
            box.SuspendLayout();
            SuspendLayout();
            // 
            // lvUsers
            // 
            lvUsers.Columns.AddRange(new ColumnHeader[] {
            Usuario,
            Password,
            Privilegio});
            lvUsers.FullRowSelect = true;
            lvUsers.GridLines = true;
            lvUsers.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvUsers.HideSelection = false;
            lvUsers.Location = new Point(224, 69);
            lvUsers.Margin = new Padding(4);
            lvUsers.MultiSelect = false;
            lvUsers.Name = "lvUsers";
            lvUsers.Size = new Size(500, 334);
            lvUsers.TabIndex = 0;
            lvUsers.UseCompatibleStateImageBehavior = false;
            lvUsers.View = System.Windows.Forms.View.Details;
            lvUsers.SelectedIndexChanged += new EventHandler(lvUsers_SelectedIndexChanged);
            // 
            // Usuario
            // 
            Usuario.Text = "Usuario";
            Usuario.Width = 129;
            // 
            // Password
            // 
            Password.Text = "Password";
            Password.Width = 130;
            // 
            // Privilegio
            // 
            Privilegio.Text = "Privilegio";
            Privilegio.Width = 113;
            // 
            // btCerrar
            // 
            btCerrar.DialogResult = DialogResult.Cancel;
            btCerrar.Image = Resources.Fileclose;
            btCerrar.ImageAlign = ContentAlignment.MiddleLeft;
            btCerrar.Location = new Point(645, 423);
            btCerrar.Margin = new Padding(4);
            btCerrar.Name = "btCerrar";
            btCerrar.Size = new Size(79, 28);
            btCerrar.TabIndex = 1;
            btCerrar.Text = "Cerrar";
            btCerrar.TextAlign = ContentAlignment.MiddleRight;
            btCerrar.Click += new EventHandler(btCerrar_Click);
            // 
            // box
            // 
            box.Controls.Add(btEliminar);
            box.Controls.Add(btAceptarIntro);
            box.Controls.Add(cbPrivilegio);
            box.Controls.Add(Label3);
            box.Controls.Add(Label2);
            box.Controls.Add(tbPassword);
            box.Controls.Add(Label1);
            box.Controls.Add(tbUsuario);
            box.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            box.Location = new Point(21, 59);
            box.Margin = new Padding(4);
            box.Name = "box";
            box.Padding = new Padding(4);
            box.Size = new Size(171, 384);
            box.TabIndex = 2;
            box.TabStop = false;
            box.Text = "Selección";
            // 
            // btEliminar
            // 
            btEliminar.Image = Resources.Edit_remove;
            btEliminar.ImageAlign = ContentAlignment.MiddleLeft;
            btEliminar.Location = new Point(32, 345);
            btEliminar.Margin = new Padding(4);
            btEliminar.Name = "btEliminar";
            btEliminar.Size = new Size(76, 28);
            btEliminar.TabIndex = 8;
            btEliminar.Text = "Borrar";
            btEliminar.TextAlign = ContentAlignment.MiddleRight;
            btEliminar.Click += new EventHandler(btEliminar_Click);
            // 
            // btAceptarIntro
            // 
            btAceptarIntro.Image = Resources.Edit_add;
            btAceptarIntro.ImageAlign = ContentAlignment.MiddleLeft;
            btAceptarIntro.Location = new Point(32, 305);
            btAceptarIntro.Margin = new Padding(4);
            btAceptarIntro.Name = "btAceptarIntro";
            btAceptarIntro.Size = new Size(76, 28);
            btAceptarIntro.TabIndex = 7;
            btAceptarIntro.Text = "Añadir";
            btAceptarIntro.TextAlign = ContentAlignment.MiddleRight;
            btAceptarIntro.Click += new EventHandler(btAceptarIntro_Click);
            // 
            // cbPrivilegio
            // 
            cbPrivilegio.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPrivilegio.Items.AddRange(new object[] {
            "Administrador",
            "Mantenimiento",
            "Operario"});
            cbPrivilegio.Location = new Point(11, 256);
            cbPrivilegio.Margin = new Padding(4);
            cbPrivilegio.Name = "cbPrivilegio";
            cbPrivilegio.Size = new Size(148, 21);
            cbPrivilegio.TabIndex = 6;
            // 
            // Label3
            // 
            Label3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            Label3.Location = new Point(11, 226);
            Label3.Margin = new Padding(4, 0, 4, 0);
            Label3.Name = "Label3";
            Label3.Size = new Size(107, 20);
            Label3.TabIndex = 5;
            Label3.Text = "Privilegio:";
            // 
            // Label2
            // 
            Label2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            Label2.Image = Resources.Kgpg_identity;
            Label2.ImageAlign = ContentAlignment.MiddleLeft;
            Label2.Location = new Point(11, 128);
            Label2.Margin = new Padding(4, 0, 4, 0);
            Label2.Name = "Label2";
            Label2.Size = new Size(127, 20);
            Label2.TabIndex = 3;
            Label2.Text = "Clave de acceso:";
            Label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tbPassword
            // 
            tbPassword.Location = new Point(11, 158);
            tbPassword.Margin = new Padding(4);
            tbPassword.Name = "tbPassword";
            tbPassword.Size = new Size(148, 20);
            tbPassword.TabIndex = 2;
            // 
            // Label1
            // 
            Label1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            Label1.Image = Resources.Kontact_contacts;
            Label1.ImageAlign = ContentAlignment.MiddleLeft;
            Label1.Location = new Point(11, 39);
            Label1.Margin = new Padding(4, 0, 4, 0);
            Label1.Name = "Label1";
            Label1.Size = new Size(76, 20);
            Label1.TabIndex = 1;
            Label1.Text = "Usuario:";
            Label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tbUsuario
            // 
            tbUsuario.Location = new Point(11, 69);
            tbUsuario.Margin = new Padding(4);
            tbUsuario.Name = "tbUsuario";
            tbUsuario.Size = new Size(148, 20);
            tbUsuario.TabIndex = 0;
            // 
            // rbNewUser
            // 
            rbNewUser.Checked = true;
            rbNewUser.Location = new Point(224, 20);
            rbNewUser.Margin = new Padding(4);
            rbNewUser.Name = "rbNewUser";
            rbNewUser.Size = new Size(139, 20);
            rbNewUser.TabIndex = 5;
            rbNewUser.TabStop = true;
            rbNewUser.Text = "Crear Usuario";
            // 
            // rbChangeUser
            // 
            rbChangeUser.Location = new Point(395, 20);
            rbChangeUser.Margin = new Padding(4);
            rbChangeUser.Name = "rbChangeUser";
            rbChangeUser.Size = new Size(149, 20);
            rbChangeUser.TabIndex = 6;
            rbChangeUser.Text = "Modificar Usuario";
            rbChangeUser.CheckedChanged += new EventHandler(rbChangeUser_CheckedChanged);
            // 
            // btAceptar
            // 
            btAceptar.DialogResult = DialogResult.Cancel;
            btAceptar.Image = Resources.Button_accept;
            btAceptar.ImageAlign = ContentAlignment.MiddleLeft;
            btAceptar.Location = new Point(517, 423);
            btAceptar.Margin = new Padding(4);
            btAceptar.Name = "btAceptar";
            btAceptar.Size = new Size(84, 28);
            btAceptar.TabIndex = 7;
            btAceptar.Text = "Aceptar";
            btAceptar.TextAlign = ContentAlignment.MiddleRight;
            btAceptar.Click += new EventHandler(btAceptar_Click);
            // 
            // frmAdministrador
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(747, 460);
            ControlBox = false;
            Controls.Add(btAceptar);
            Controls.Add(rbChangeUser);
            Controls.Add(rbNewUser);
            Controls.Add(box);
            Controls.Add(btCerrar);
            Controls.Add(lvUsers);
            Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = ((Icon)(resources.GetObject("$this.Icon")));
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAdministrador";
            Text = "Administrador de Usuarios";
            TopMost = true;
            Load += new EventHandler(frmAdministrador_Load);
            box.ResumeLayout(false);
            box.PerformLayout();
            ResumeLayout(false);

		}
		#endregion
		private bool Actualizate;
		private bool VentanaCargada = false;
        
        

		private void lvUsers_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((lvUsers.SelectedItems.Count > 0))
			{
				ListViewItem lvI = lvUsers.SelectedItems[0];
				tbUsuario.Text = lvI.Text;
				tbPassword.Text = lvI.SubItems[1].Text;
				string priv = lvI.SubItems[2].Text.Trim().ToUpper();
				for (int i = 0; i <= cbPrivilegio.Items.Count - 1; i++) {
					if ((string)cbPrivilegio.Items[i].ToString().Trim().ToUpper() == priv)
					{
						cbPrivilegio.SelectedIndex = i;
					}
				}
			}
		}
		private void btAceptarIntro_Click(object sender, EventArgs e)
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
			if ((rbNewUser.Checked))
			{
				var lvI = new ListViewItem();
				
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
		private void rbChangeUser_CheckedChanged(object sender, EventArgs e)
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
		private void btAceptar_Click(object sender, EventArgs e)
		{
			UserAccess.Instance.ClearUsers();
			if (Actualizate)
			{
				foreach (ListViewItem lvI in lvUsers.Items) {
					UserAccess.Instance.AddChangeUser(lvI.Text, lvI.SubItems[1].Text, lvI.SubItems[2].Text.Trim().ToUpper());
				}
			}
			UserAccess.Instance.SaveUsers();
			DialogResult = DialogResult.OK;
		}
		private void frmAdministrador_Load(object sender, EventArgs e)
		{
			cbPrivilegio.SelectedIndex = 2;
			if (!VentanaCargada)
			{
                Dictionary<string, UserAccess.PasswordPrivilegio> h = UserAccess.Instance.GetUsers();
				IDictionaryEnumerator en = h.GetEnumerator();
				while (en.MoveNext()) {
					var lvI = new ListViewItem();
					lvI.Text = (string)en.Key;
					var pp = (UserAccess.PasswordPrivilegio)en.Value;
					lvI.SubItems.Add(pp.Password);
					lvI.SubItems.Add(pp.Privilegio.ToString());
					lvUsers.Items.Add(lvI);
				}
				Actualizate = true;
				VentanaCargada = true;
			}
		}
		private void btEliminar_Click(object sender, EventArgs e)
		{
			if ((lvUsers.SelectedItems.Count > 0))
			{
				lvUsers.SelectedItems[0].Remove();
			}
		}
		private void btCerrar_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
