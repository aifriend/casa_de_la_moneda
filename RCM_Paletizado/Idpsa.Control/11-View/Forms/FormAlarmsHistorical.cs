using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.Diagnosis;

namespace Idpsa.Control.View
{
	public class FormAlarmsHistorical : Form
	{
		#region " Código generado por el Diseñador de Windows Forms "
		public FormAlarmsHistorical() : base()
		{
			//El Diseñador de Windows Forms requiere esta llamada.
			InitializeComponent();
			rbEmergencias.CheckedChanged += new EventHandler(rbCheckedChanged);
			rbFallos.CheckedChanged += new EventHandler(rbCheckedChanged);
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
		internal Panel Panel2;
		internal ListView lwAlarmas;
		internal ColumnHeader CDescripcion;
		internal ColumnHeader CFrecuencia;
		internal ColumnHeader CFecha;
		internal RadioButton rbEmergencias;
		internal Panel Panel3;
		internal Button Button1;
		internal RadioButton rbFallos;
		[DebuggerStepThrough()]
		private void InitializeComponent()
		{
			Panel1 = new Panel();
			rbFallos = new RadioButton();
			rbEmergencias = new RadioButton();
			Panel2 = new Panel();
			lwAlarmas = new ListView();
			CFecha = new ColumnHeader();
			CDescripcion = new ColumnHeader();
			CFrecuencia = new ColumnHeader();
			Panel3 = new Panel();
			Button1 = new Button();
			lwAlarmas.ColumnClick += new ColumnClickEventHandler( lwAlarmas_ColumnClick );
			Load += new EventHandler( frmHistoricoAlarmas_Load );
			Button1.Click += new EventHandler( Button1_Click );
			lwAlarmas.KeyDown += new KeyEventHandler( lwAlarmas_KeyDown );
			Panel1.SuspendLayout();
			Panel2.SuspendLayout();
			Panel3.SuspendLayout();
			SuspendLayout();
			//
			//Panel1
			//
			Panel1.Controls.Add(rbFallos);
			Panel1.Controls.Add(rbEmergencias);
			Panel1.Dock = DockStyle.Top;
			Panel1.Location = new Point(0, 0);
			Panel1.Name = "Panel1";
			Panel1.Size = new Size(840, 56);
			Panel1.TabIndex = 0;
			//
			//rbFallos
			//
			rbFallos.Location = new Point(520, 20);
			rbFallos.Name = "rbFallos";
			rbFallos.Size = new Size(64, 16);
			rbFallos.TabIndex = 5;
			rbFallos.Text = "Fallos";
			//
			//rbEmergencias
			//
			rbEmergencias.Checked = true;
			rbEmergencias.Location = new Point(296, 16);
			rbEmergencias.Name = "rbEmergencias";
			rbEmergencias.TabIndex = 0;
			rbEmergencias.TabStop = true;
			rbEmergencias.Text = "Emergencias";
			//
			//Panel2
			//
			Panel2.Controls.Add(lwAlarmas);
			Panel2.Dock = DockStyle.Top;
			Panel2.Location = new Point(0, 56);
			Panel2.Name = "Panel2";
			Panel2.Size = new Size(840, 392);
			Panel2.TabIndex = 1;
			//
			//lwAlarmas
			//
			lwAlarmas.Columns.AddRange(new ColumnHeader[] {CFecha, CDescripcion, CFrecuencia});
			lwAlarmas.Dock = DockStyle.Top;
			lwAlarmas.FullRowSelect = true;
			lwAlarmas.GridLines = true;
			lwAlarmas.Location = new Point(0, 0);
			lwAlarmas.Name = "lwAlarmas";
			lwAlarmas.Size = new Size(840, 392);
			lwAlarmas.TabIndex = 0;
			lwAlarmas.View = System.Windows.Forms.View.Details;
			//
			//CFecha
			//
			CFecha.Text = "      Fecha";
			CFecha.Width = 133;
			//
			//CDescripcion
			//
			CDescripcion.Text = "Descripcion";
			CDescripcion.Width = 615;
			//
			//CFrecuencia
			//
			CFrecuencia.Text = "Frecuencia  (%)";
			CFrecuencia.Width = 170;
			//
			//Panel3
			//
			Panel3.Controls.Add(Button1);
			Panel3.Dock = DockStyle.Bottom;
			Panel3.Location = new Point(0, 448);
			Panel3.Name = "Panel3";
			Panel3.Size = new Size(840, 32);
			Panel3.TabIndex = 2;
			//
			//Button1
			//
			Button1.Location = new Point(772, 6);
			Button1.Name = "Button1";
			Button1.Size = new Size(64, 23);
			Button1.TabIndex = 0;
			Button1.Text = "Borrar";
			//
			//frmHistoricoAlarmas
			//
			AutoScaleBaseSize = new Size(5, 13);
			ClientSize = new Size(840, 480);
			Controls.Add(Panel3);
			Controls.Add(Panel2);
			Controls.Add(Panel1);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "frmHistoricoAlarmas";
			Text = "Historico de alarmas";
			TopMost = true;
			Panel1.ResumeLayout(false);
			Panel2.ResumeLayout(false);
			Panel3.ResumeLayout(false);
			ResumeLayout(false);
		}
		#endregion
		private AlarmsHistorical HistoricoAlarmasSeleccionado()
		{
			AlarmsHistorical value=null;
			if (rbFallos.Checked)
			{
				value =  AlarmsHistoricalManager.Instance.AlarmsSys;
			}
            else if (rbEmergencias.Checked) {
                value =  AlarmsHistoricalManager.Instance.EmergenciesSys;
			}
			return value;
		}
		private void lwAlarmas_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if (e.Column == 0)
			{
				HistoricoAlarmasSeleccionado().SortByDate();
			}
            else if (e.Column == 1) {
				HistoricoAlarmasSeleccionado().SortByDescription();
			}
            else if (e.Column == 2) {
				HistoricoAlarmasSeleccionado().SortByFrecuency();
			}
			MostrarAlarmasSelecionadas();
		}
		private void MostrarAlarmasSelecionadas()
		{
			lwAlarmas.Items.Clear();
			AlarmsHistorical Alarmas = HistoricoAlarmasSeleccionado();
            decimal[] Fs = Alarmas.Frecuencies();
            for (int i = 0; i <= Alarmas.Count() - 1; i++)
            {
                lwAlarmas.Items.Add(new ListViewItem(new string[] { Alarmas[i].Date.ToString(), Alarmas[i].Description, (decimal.Round(Fs[i], 1)).ToString() }));
			}
		}
		private void frmHistoricoAlarmas_Load(object sender, EventArgs e)
		{
			MostrarAlarmasSelecionadas();
		}
		private void rbCheckedChanged(object sender, EventArgs e)
		{
			var rb = (RadioButton)sender;
			if (rb.Checked)
			{
				MostrarAlarmasSelecionadas();
			}
		}
		private void Button1_Click(object sender, EventArgs e)
		{
			AlarmsHistorical Alar = HistoricoAlarmasSeleccionado();
			foreach (ListViewItem lw in lwAlarmas.SelectedItems) {
				Alar.Remove(lw.SubItems[1].Text);
			}
			MostrarAlarmasSelecionadas();
		}
		private void lwAlarmas_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				AlarmsHistorical Alar = HistoricoAlarmasSeleccionado();
				foreach (ListViewItem lw in lwAlarmas.SelectedItems) {
					Alar.Remove(lw.SubItems[1].Text);
				}
				MostrarAlarmasSelecionadas();
			}
		}
		private void CbLinea_SelectedIndexChanged(object sender, EventArgs e)
		{
			MostrarAlarmasSelecionadas();
		}
	}
}
