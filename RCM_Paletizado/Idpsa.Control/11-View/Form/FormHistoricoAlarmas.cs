using System.Windows.Forms;
using Idpsa.Control.Diagnosis;

namespace Idpsa.Control.View
{
	public class FrmHistoricoAlarmas : System.Windows.Forms.Form
	{
		#region " Código generado por el Diseñador de Windows Forms "
		public FrmHistoricoAlarmas() : base()
		{
			//El Diseñador de Windows Forms requiere esta llamada.
			InitializeComponent();
			rbEmergencias.CheckedChanged += new System.EventHandler(this.rbCheckedChanged);
			rbFallos.CheckedChanged += new System.EventHandler(this.rbCheckedChanged);
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
		internal System.Windows.Forms.Panel Panel2;
		internal System.Windows.Forms.ListView lwAlarmas;
		internal System.Windows.Forms.ColumnHeader CDescripcion;
		internal System.Windows.Forms.ColumnHeader CFrecuencia;
		internal System.Windows.Forms.ColumnHeader CFecha;
		internal System.Windows.Forms.RadioButton rbEmergencias;
		internal System.Windows.Forms.Panel Panel3;
		internal System.Windows.Forms.Button Button1;
		internal System.Windows.Forms.RadioButton rbFallos;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.Panel1 = new System.Windows.Forms.Panel();
			this.rbFallos = new System.Windows.Forms.RadioButton();
			this.rbEmergencias = new System.Windows.Forms.RadioButton();
			this.Panel2 = new System.Windows.Forms.Panel();
			this.lwAlarmas = new System.Windows.Forms.ListView();
			this.CFecha = new System.Windows.Forms.ColumnHeader();
			this.CDescripcion = new System.Windows.Forms.ColumnHeader();
			this.CFrecuencia = new System.Windows.Forms.ColumnHeader();
			this.Panel3 = new System.Windows.Forms.Panel();
			this.Button1 = new System.Windows.Forms.Button();
			lwAlarmas.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler( lwAlarmas_ColumnClick );
			this.Load += new System.EventHandler( frmHistoricoAlarmas_Load );
			Button1.Click += new System.EventHandler( Button1_Click );
			lwAlarmas.KeyDown += new System.Windows.Forms.KeyEventHandler( lwAlarmas_KeyDown );
			this.Panel1.SuspendLayout();
			this.Panel2.SuspendLayout();
			this.Panel3.SuspendLayout();
			this.SuspendLayout();
			//
			//Panel1
			//
			this.Panel1.Controls.Add(this.rbFallos);
			this.Panel1.Controls.Add(this.rbEmergencias);
			this.Panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.Panel1.Location = new System.Drawing.Point(0, 0);
			this.Panel1.Name = "Panel1";
			this.Panel1.Size = new System.Drawing.Size(840, 56);
			this.Panel1.TabIndex = 0;
			//
			//rbFallos
			//
			this.rbFallos.Location = new System.Drawing.Point(520, 20);
			this.rbFallos.Name = "rbFallos";
			this.rbFallos.Size = new System.Drawing.Size(64, 16);
			this.rbFallos.TabIndex = 5;
			this.rbFallos.Text = "Fallos";
			//
			//rbEmergencias
			//
			this.rbEmergencias.Checked = true;
			this.rbEmergencias.Location = new System.Drawing.Point(296, 16);
			this.rbEmergencias.Name = "rbEmergencias";
			this.rbEmergencias.TabIndex = 0;
			this.rbEmergencias.TabStop = true;
			this.rbEmergencias.Text = "Emergencias";
			//
			//Panel2
			//
			this.Panel2.Controls.Add(this.lwAlarmas);
			this.Panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.Panel2.Location = new System.Drawing.Point(0, 56);
			this.Panel2.Name = "Panel2";
			this.Panel2.Size = new System.Drawing.Size(840, 392);
			this.Panel2.TabIndex = 1;
			//
			//lwAlarmas
			//
			this.lwAlarmas.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {this.CFecha, this.CDescripcion, this.CFrecuencia});
			this.lwAlarmas.Dock = System.Windows.Forms.DockStyle.Top;
			this.lwAlarmas.FullRowSelect = true;
			this.lwAlarmas.GridLines = true;
			this.lwAlarmas.Location = new System.Drawing.Point(0, 0);
			this.lwAlarmas.Name = "lwAlarmas";
			this.lwAlarmas.Size = new System.Drawing.Size(840, 392);
			this.lwAlarmas.TabIndex = 0;
			this.lwAlarmas.View = System.Windows.Forms.View.Details;
			//
			//CFecha
			//
			this.CFecha.Text = "      Fecha";
			this.CFecha.Width = 133;
			//
			//CDescripcion
			//
			this.CDescripcion.Text = "Descripcion";
			this.CDescripcion.Width = 615;
			//
			//CFrecuencia
			//
			this.CFrecuencia.Text = "Frecuencia  (%)";
			this.CFrecuencia.Width = 170;
			//
			//Panel3
			//
			this.Panel3.Controls.Add(this.Button1);
			this.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.Panel3.Location = new System.Drawing.Point(0, 448);
			this.Panel3.Name = "Panel3";
			this.Panel3.Size = new System.Drawing.Size(840, 32);
			this.Panel3.TabIndex = 2;
			//
			//Button1
			//
			this.Button1.Location = new System.Drawing.Point(772, 6);
			this.Button1.Name = "Button1";
			this.Button1.Size = new System.Drawing.Size(64, 23);
			this.Button1.TabIndex = 0;
			this.Button1.Text = "Borrar";
			//
			//frmHistoricoAlarmas
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(840, 480);
			this.Controls.Add(this.Panel3);
			this.Controls.Add(this.Panel2);
			this.Controls.Add(this.Panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmHistoricoAlarmas";
			this.Text = "Historico de alarmas";
			this.TopMost = true;
			this.Panel1.ResumeLayout(false);
			this.Panel2.ResumeLayout(false);
			this.Panel3.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion
		private HistoricoAlarmas HistoricoAlarmasSeleccionado()
		{
			HistoricoAlarmas value=null;
			if (this.rbFallos.Checked)
			{
				value =  HistoricoAlarmasManager.Instance.AlarmasSys;
			}
            else if (this.rbEmergencias.Checked) {
                value =  HistoricoAlarmasManager.Instance.EmergenciasSys;
			}
			return value;
		}
		private void lwAlarmas_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			if (e.Column == 0)
			{
				HistoricoAlarmasSeleccionado().SortPorFecha();
			}
            else if (e.Column == 1) {
				HistoricoAlarmasSeleccionado().SortPorDescripcion();
			}
            else if (e.Column == 2) {
				HistoricoAlarmasSeleccionado().SortPorFrecuencia();
			}
			MostrarAlarmasSelecionadas();
		}
		private void MostrarAlarmasSelecionadas()
		{
			lwAlarmas.Items.Clear();
			HistoricoAlarmas Alarmas = HistoricoAlarmasSeleccionado();
            decimal[] Fs = Alarmas.Frecuencias();
            for (int i = 0; i <= Alarmas.Count() - 1; i++)
            {
                lwAlarmas.Items.Add(new ListViewItem(new string[] { Alarmas[i].Fecha.ToString(), Alarmas[i].Descripcion, (decimal.Round(Fs[i], 1)).ToString() }));
			}
		}
		private void frmHistoricoAlarmas_Load(object sender, System.EventArgs e)
		{
			MostrarAlarmasSelecionadas();
		}
		private void rbCheckedChanged(object sender, System.EventArgs e)
		{
			RadioButton rb = (RadioButton)sender;
			if (rb.Checked)
			{
				MostrarAlarmasSelecionadas();
			}
		}
		private void Button1_Click(object sender, System.EventArgs e)
		{
			HistoricoAlarmas Alar = HistoricoAlarmasSeleccionado();
			foreach (ListViewItem lw in this.lwAlarmas.SelectedItems) {
				Alar.Remove(lw.SubItems[1].Text);
			}
			MostrarAlarmasSelecionadas();
		}
		private void lwAlarmas_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				HistoricoAlarmas Alar = HistoricoAlarmasSeleccionado();
				foreach (ListViewItem lw in this.lwAlarmas.SelectedItems) {
					Alar.Remove(lw.SubItems[1].Text);
				}
				MostrarAlarmasSelecionadas();
			}
		}
		private void CbLinea_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			MostrarAlarmasSelecionadas();
		}
	}
}
