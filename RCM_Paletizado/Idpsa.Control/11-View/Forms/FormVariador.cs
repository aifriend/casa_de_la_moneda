using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using Idpsa.Control.Component;
using Microsoft.VisualBasic;

namespace Idpsa.Control.View
{
	public class FormVariador : Form
	{
		private bool _inicializate;
		private bool _antesVisible;
		private bool _comboBoxChange;
		private int _variadorSelected;		
		private bool _btInicioClick;
		private bool _btParadaClick;
		private bool _btResetClick;
		private bool _rbNegClick;
		private bool _rbPosClick;
		private bool _inicioStart;
        private SiemensMicromaster420Collection variadores;
		#region " Código generado por el Diseñador de Windows Forms "
		private FormVariador() : base()
		{
			//El Diseñador de Windows Forms requiere esta llamada.
			InitializeComponent();
            

			//Agregar cualquier inicialización después de la llamada a InitializeComponent()
		}

        public FormVariador(SiemensMicromaster420Collection variadores)
            : base()
        {
            //El Diseñador de Windows Forms requiere esta llamada.
            InitializeComponent();
            this.variadores = variadores;

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
		internal GroupBox GroupBox1;
		internal Label lbVelocidad;
		internal Panel Panel1;
		internal TextBox tbVelocidad;
		internal Button btParada;
		internal Button btInicio;
		internal Button btReset;
		internal StatusBar Stbar;
		internal ComboBox ComboBox;
		internal GroupBox GroupBox3;
		internal Label Label3;
		internal Label Label4;
		internal Label lbV;
		internal Label lbE;
		internal Label Label2;
		internal Label Label1;
		internal Label Label5;
		internal RadioButton rbPos;
		internal RadioButton rbNeg;
		[DebuggerStepThrough()]
		private void InitializeComponent()
		{
			var resources = new ResourceManager(typeof(FormVariador));
			GroupBox1 = new GroupBox();
			rbNeg = new RadioButton();
			rbPos = new RadioButton();
			Label5 = new Label();
			tbVelocidad = new TextBox();
			lbVelocidad = new Label();
			Label1 = new Label();
			Label2 = new Label();
			Panel1 = new Panel();
			btReset = new Button();
			btParada = new Button();
			btInicio = new Button();
			Stbar = new StatusBar();
			ComboBox = new ComboBox();
			GroupBox3 = new GroupBox();
			lbE = new Label();
			lbV = new Label();
			Label3 = new Label();
			Label4 = new Label();
			Closing += new CancelEventHandler( frmVariador_Closing );
			btInicio.Click += new EventHandler( btInicio_Click );
			btParada.Click += new EventHandler( btParada_Click );
			btReset.Click += new EventHandler( btReset_Click );
			ComboBox.SelectedValueChanged += new EventHandler( ComboBox_SelectedValueChanged );
			ComboBox.KeyPress += new KeyPressEventHandler( ComboBox_KeyPress );
			rbPos.CheckedChanged += new EventHandler( rbNeg_CheckedChanged );
			rbNeg.CheckedChanged += new EventHandler( rbPos_CheckedChanged );
			tbVelocidad.Leave += new EventHandler( tbVelocidad_Leave );
			GroupBox1.SuspendLayout();
			Panel1.SuspendLayout();
			GroupBox3.SuspendLayout();
			SuspendLayout();
			//
			//GroupBox1
			//
			GroupBox1.Controls.Add(rbNeg);
			GroupBox1.Controls.Add(rbPos);
			GroupBox1.Controls.Add(Label5);
			GroupBox1.Controls.Add(tbVelocidad);
			GroupBox1.Controls.Add(lbVelocidad);
			GroupBox1.Controls.Add(Label1);
			GroupBox1.Controls.Add(Label2);
			GroupBox1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			GroupBox1.Location = new Point(8, 48);
			GroupBox1.Name = "GroupBox1";
			GroupBox1.Size = new Size(184, 80);
			GroupBox1.TabIndex = 0;
			GroupBox1.TabStop = false;
			GroupBox1.Text = "Parametros Consigna";
			//
			//rbNeg
			//
			rbNeg.Location = new Point(160, 48);
			rbNeg.Name = "rbNeg";
			rbNeg.Size = new Size(16, 24);
			rbNeg.TabIndex = 10;
			//
			//rbPos
			//
			rbPos.Checked = true;
			rbPos.Location = new Point(112, 48);
			rbPos.Name = "rbPos";
			rbPos.Size = new Size(16, 24);
			rbPos.TabIndex = 9;
			rbPos.TabStop = true;
			//
			//Label5
			//
			Label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			Label5.Location = new Point(16, 56);
			Label5.Name = "Label5";
			Label5.Size = new Size(64, 16);
			Label5.TabIndex = 8;
			Label5.Text = "Sentido:";
			//
			//tbVelocidad
			//
			tbVelocidad.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			tbVelocidad.Location = new Point(96, 16);
			tbVelocidad.Name = "tbVelocidad";
			tbVelocidad.Size = new Size(64, 20);
			tbVelocidad.TabIndex = 2;
			tbVelocidad.Text = "0";
			tbVelocidad.TextAlign = HorizontalAlignment.Right;
			//
			//lbVelocidad
			//
			lbVelocidad.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			lbVelocidad.Location = new Point(16, 24);
			lbVelocidad.Name = "lbVelocidad";
			lbVelocidad.Size = new Size(64, 16);
			lbVelocidad.TabIndex = 0;
			lbVelocidad.Text = "Velocidad:";
			//
			//Label1
			//
			Label1.BackColor = Color.Gainsboro;
			Label1.Font = new Font("Microsoft Sans Serif", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			Label1.Location = new Point(136, 48);
			Label1.Name = "Label1";
			Label1.Size = new Size(16, 24);
			Label1.TabIndex = 6;
			Label1.Text = "<";
			//
			//Label2
			//
			Label2.BackColor = Color.Gainsboro;
			Label2.Font = new Font("Microsoft Sans Serif", 15f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			Label2.Location = new Point(88, 48);
			Label2.Name = "Label2";
			Label2.Size = new Size(16, 24);
			Label2.TabIndex = 7;
			Label2.Text = ">";
			//
			//Panel1
			//
			Panel1.Controls.Add(btReset);
			Panel1.Controls.Add(btParada);
			Panel1.Controls.Add(btInicio);
			Panel1.Location = new Point(88, 152);
			Panel1.Name = "Panel1";
			Panel1.Size = new Size(280, 32);
			Panel1.TabIndex = 1;
			//
			//btReset
			//
			btReset.Location = new Point(208, 8);
			btReset.Name = "btReset";
			btReset.Size = new Size(72, 24);
			btReset.TabIndex = 2;
			btReset.Text = "Reset";
			//
			//btParada
			//
			btParada.Location = new Point(120, 8);
			btParada.Name = "btParada";
			btParada.Size = new Size(72, 24);
			btParada.TabIndex = 1;
			btParada.Text = "Parada";
			//
			//btInicio
			//
			btInicio.Location = new Point(32, 8);
			btInicio.Name = "btInicio";
			btInicio.Size = new Size(72, 24);
			btInicio.TabIndex = 0;
			btInicio.Text = "Inicio";
			//
			//Stbar
			//
			Stbar.Location = new Point(0, 188);
			Stbar.Name = "Stbar";
			Stbar.Size = new Size(376, 16);
			Stbar.SizingGrip = false;
			Stbar.TabIndex = 4;
			Stbar.Text = "Código Estado: (Inactivo)";
			//
			//ComboBox
			//
			ComboBox.Location = new Point(8, 16);
			ComboBox.Name = "ComboBox";
			ComboBox.Size = new Size(168, 21);
			ComboBox.TabIndex = 5;
			//
			//GroupBox3
			//
			GroupBox3.Controls.Add(lbE);
			GroupBox3.Controls.Add(lbV);
			GroupBox3.Controls.Add(Label3);
			GroupBox3.Controls.Add(Label4);
			GroupBox3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			GroupBox3.Location = new Point(200, 48);
			GroupBox3.Name = "GroupBox3";
			GroupBox3.Size = new Size(168, 80);
			GroupBox3.TabIndex = 6;
			GroupBox3.TabStop = false;
			GroupBox3.Text = "Monotorizacion";
			//
			//lbE
			//
			lbE.BorderStyle = BorderStyle.Fixed3D;
			lbE.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			lbE.Location = new Point(72, 16);
			lbE.Name = "lbE";
			lbE.Size = new Size(80, 23);
			lbE.TabIndex = 3;
			lbE.Text = "Inactivo";
			lbE.TextAlign = ContentAlignment.MiddleRight;
			//
			//lbV
			//
			lbV.BorderStyle = BorderStyle.Fixed3D;
			lbV.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			lbV.Location = new Point(88, 48);
			lbV.Name = "lbV";
			lbV.Size = new Size(64, 23);
			lbV.TabIndex = 2;
			lbV.Text = "0";
			lbV.TextAlign = ContentAlignment.MiddleRight;
			//
			//Label3
			//
			Label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			Label3.Location = new Point(8, 56);
			Label3.Name = "Label3";
			Label3.Size = new Size(64, 16);
			Label3.TabIndex = 1;
			Label3.Text = "Velocidad:";
			//
			//Label4
			//
			Label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
			Label4.Location = new Point(8, 24);
			Label4.Name = "Label4";
			Label4.Size = new Size(64, 16);
			Label4.TabIndex = 0;
			Label4.Text = "Estado:";
			//
			//frmVariador
			//
			AutoScaleBaseSize = new Size(5, 13);
			BackColor = Color.Gainsboro;
			ClientSize = new Size(376, 204);
			Controls.Add(GroupBox3);
			Controls.Add(ComboBox);
			Controls.Add(Stbar);
			Controls.Add(Panel1);
			Controls.Add(GroupBox1);
			FormBorderStyle = FormBorderStyle.Fixed3D;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Name = "frmVariador";
			Text = "Variador";
			GroupBox1.ResumeLayout(false);
			Panel1.ResumeLayout(false);
			GroupBox3.ResumeLayout(false);
			ResumeLayout(false);
		}
		#endregion
		public void Run()
		{
			if (_inicializate == false)
			{
				LoadCombo();
				_inicializate = true;
			}
			else
			{
				if (Visible == true)
				{
					RefreshVaridador();
					VariadorManual();
				}
				if ((Visible != _antesVisible))
				{
					if (_variadorSelected < 0)
					{
						variadores[_variadorSelected - 1].Mando("ActualizarError");
					}
				}
				_antesVisible = Visible;
			}
		}
		public void RefreshVaridador()
		{
			if (_comboBoxChange == true)
			{
				_comboBoxChange = false;
				if (ComboBox.Text.Trim().Length > 0)
				{
                    _variadorSelected = int.Parse(ComboBox.Text.Split(new char[] { '/' })[0]) - 1;
                    				
					//CInt(ComboBox.Text.Substring(0, ComboBox.Text.IndexOf("/")))
					ResetMarks();
				}
				else
				{
					_variadorSelected = 0;
				}
			}
			if (_variadorSelected > 0)
			{
				Stbar.Text = "Código Estado: " + variadores[_variadorSelected - 1].LeerEstado();
				lbE.Text = variadores[_variadorSelected - 1].ConocerEstado();
				lbV.Text = variadores[_variadorSelected - 1].LeerVelocidad().ToString();
			}
		}
		private void ResetMarks()
		{
			int i;
			for (i = 0; i <= variadores.Count - 1; i++) {
				variadores[i].Parada();
			}
		}
		private void VariadorManual()
		{
			if (_variadorSelected > 0)
			{
				if (_btInicioClick == true)
				{
					_btInicioClick = false;
					btInicio.Enabled = false;
					_inicioStart = true;
					if ((_rbPosClick == true))
					{
						variadores[_variadorSelected - 1].Mando("ListoAdelante");
					}
					else
					{
						variadores[_variadorSelected - 1].Mando("ListoAtras");
					}
				}
                    else if (_inicioStart == true) {
					_inicioStart = false;
					if ((_rbPosClick == true))
					{
                        variadores[_variadorSelected - 1].Mando("Adelante", int.Parse(tbVelocidad.Text));
					}
					else
					{
						variadores[_variadorSelected - 1].Mando("Atras", int.Parse(tbVelocidad.Text));
					}
					btInicio.Enabled = true;
				}
				if (_btParadaClick == true)
				{
					_btParadaClick = false;
					variadores[_variadorSelected - 1].Parada();
				}
				if (_btResetClick == true)
				{
					_btResetClick = false;
					variadores[_variadorSelected - 1].Mando("ActualizarError");
				}
			}
		}
		private void frmVariador_Closing(object sender, CancelEventArgs e)
		{
			Hide();
			e.Cancel = true;
		}
		private void btInicio_Click(object sender, EventArgs e)
		{
			_btInicioClick = true;
		}
		private void btParada_Click(object sender, EventArgs e)
		{
			_btParadaClick = true;
		}
		private void btReset_Click(object sender, EventArgs e)
		{
			_btResetClick = true;
		}
		private void LoadCombo()
		{
			int i;
			ComboBox.Items.Clear();
			for (i = 0; i <= variadores.Count - 1; i++) {
				ComboBox.Items.Add((i + 1).ToString() + "/" + variadores.AllKeys[i]);
			}
		}
		private void ComboBox_SelectedValueChanged(object sender, EventArgs e)
		{
			_comboBoxChange = true;
		}
		private void ComboBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = true;
		}
		private void rbNeg_CheckedChanged(object sender, EventArgs e)
		{
			_rbNegClick = rbNeg.Checked;
			_rbPosClick = !_rbNegClick;
		}
		private void rbPos_CheckedChanged(object sender, EventArgs e)
		{
			_rbPosClick = rbPos.Checked;
			_rbNegClick = !_rbPosClick;
		}
		private void tbVelocidad_Leave(object sender, EventArgs e)
		{
			if (!Information.IsNumeric(tbVelocidad.Text))
			{
				tbVelocidad.Focus();
                Interaction.MsgBox("El valor de velocidad introducido no es valido", MsgBoxStyle.OkOnly,"");
			}
		}
	}
}
