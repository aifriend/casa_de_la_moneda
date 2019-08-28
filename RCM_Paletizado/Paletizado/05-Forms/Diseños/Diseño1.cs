
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace Idpsa
{
	public class Diseño1 : System.Windows.Forms.Form
	{
		#region " Código generado por el Diseñador de Windows Forms "
		public Diseño1() : base()
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
		private System.ComponentModel.IContainer components;
		//NOTA: el Diseñador de Windows Forms requiere el siguiente procedimiento
		//Puede modificarse utilizando el Diseñador de Windows Forms. 
		//No lo modifique con el editor de código.
		internal System.Windows.Forms.GroupBox GroupBox1;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.GroupBox GroupBox2;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.GroupBox GroupBox3;
		internal System.Windows.Forms.Label Label3;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.GroupBox1 = new System.Windows.Forms.GroupBox();
			this.Label1 = new System.Windows.Forms.Label();
			this.GroupBox2 = new System.Windows.Forms.GroupBox();
			this.Label2 = new System.Windows.Forms.Label();
			this.GroupBox3 = new System.Windows.Forms.GroupBox();
			this.Label3 = new System.Windows.Forms.Label();
			GroupBox1.Enter += new System.EventHandler( GroupBox1_Enter );
			this.GroupBox1.SuspendLayout();
			this.GroupBox2.SuspendLayout();
			this.GroupBox3.SuspendLayout();
			this.SuspendLayout();
			//
			//GroupBox1
			//
			this.GroupBox1.BackColor = System.Drawing.Color.Lime;
			this.GroupBox1.Controls.Add(this.Label1);
			this.GroupBox1.Location = new System.Drawing.Point(16, 16);
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.Size = new System.Drawing.Size(80, 88);
			this.GroupBox1.TabIndex = 0;
			this.GroupBox1.TabStop = false;
			this.GroupBox1.Text = "GroupBox1";
			//
			//Label1
			//
			this.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Label1.Location = new System.Drawing.Point(8, 56);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(64, 24);
			this.Label1.TabIndex = 0;
			this.Label1.Text = "0000";
			//
			//GroupBox2
			//
			this.GroupBox2.BackColor = System.Drawing.Color.Lime;
			this.GroupBox2.Controls.Add(this.Label2);
			this.GroupBox2.Location = new System.Drawing.Point(104, 16);
			this.GroupBox2.Name = "GroupBox2";
			this.GroupBox2.Size = new System.Drawing.Size(80, 88);
			this.GroupBox2.TabIndex = 1;
			this.GroupBox2.TabStop = false;
			this.GroupBox2.Text = "GroupBox2";
			//
			//Label2
			//
			this.Label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Label2.Location = new System.Drawing.Point(8, 56);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(64, 24);
			this.Label2.TabIndex = 0;
			this.Label2.Text = "0000";
			//
			//GroupBox3
			//
			this.GroupBox3.BackColor = System.Drawing.Color.Lime;
			this.GroupBox3.Controls.Add(this.Label3);
			this.GroupBox3.Location = new System.Drawing.Point(192, 16);
			this.GroupBox3.Name = "GroupBox3";
			this.GroupBox3.Size = new System.Drawing.Size(80, 88);
			this.GroupBox3.TabIndex = 2;
			this.GroupBox3.TabStop = false;
			this.GroupBox3.Text = "GroupBox3";
			//
			//Label3
			//
			this.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Label3.Location = new System.Drawing.Point(8, 56);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(64, 24);
			this.Label3.TabIndex = 0;
			this.Label3.Text = "0000";
			//
			//Diseño1
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(864, 582);
			this.Controls.Add(this.GroupBox3);
			this.Controls.Add(this.GroupBox2);
			this.Controls.Add(this.GroupBox1);
			this.Name = "Diseño1";
			this.Text = "Diseño1";
			this.GroupBox1.ResumeLayout(false);
			this.GroupBox2.ResumeLayout(false);
			this.GroupBox3.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion
		private void GroupBox1_Enter(object sender, System.EventArgs e)
		{
		}
	}
}
