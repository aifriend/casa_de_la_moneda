using System.Windows.Forms;
using Microsoft.VisualBasic;
using Idpsa.Control.Component;

namespace Idpsa.Control
{
	public class FormVariador : System.Windows.Forms.Form
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
		private System.ComponentModel.IContainer components = null;
		//NOTA: el Diseñador de Windows Forms requiere el siguiente procedimiento
		//Puede modificarse utilizando el Diseñador de Windows Forms. 
		//No lo modifique con el editor de código.
		internal System.Windows.Forms.GroupBox GroupBox1;
		internal System.Windows.Forms.Label lbVelocidad;
		internal System.Windows.Forms.Panel Panel1;
		internal System.Windows.Forms.TextBox tbVelocidad;
		internal System.Windows.Forms.Button btParada;
		internal System.Windows.Forms.Button btInicio;
		internal System.Windows.Forms.Button btReset;
		internal System.Windows.Forms.StatusBar Stbar;
		internal System.Windows.Forms.ComboBox ComboBox;
		internal System.Windows.Forms.GroupBox GroupBox3;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.Label Label4;
		internal System.Windows.Forms.Label lbV;
		internal System.Windows.Forms.Label lbE;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.Label Label5;
		internal System.Windows.Forms.RadioButton rbPos;
		internal System.Windows.Forms.RadioButton rbNeg;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormVariador));
			this.GroupBox1 = new System.Windows.Forms.GroupBox();
			this.rbNeg = new System.Windows.Forms.RadioButton();
			this.rbPos = new System.Windows.Forms.RadioButton();
			this.Label5 = new System.Windows.Forms.Label();
			this.tbVelocidad = new System.Windows.Forms.TextBox();
			this.lbVelocidad = new System.Windows.Forms.Label();
			this.Label1 = new System.Windows.Forms.Label();
			this.Label2 = new System.Windows.Forms.Label();
			this.Panel1 = new System.Windows.Forms.Panel();
			this.btReset = new System.Windows.Forms.Button();
			this.btParada = new System.Windows.Forms.Button();
			this.btInicio = new System.Windows.Forms.Button();
			this.Stbar = new System.Windows.Forms.StatusBar();
			this.ComboBox = new System.Windows.Forms.ComboBox();
			this.GroupBox3 = new System.Windows.Forms.GroupBox();
			this.lbE = new System.Windows.Forms.Label();
			this.lbV = new System.Windows.Forms.Label();
			this.Label3 = new System.Windows.Forms.Label();
			this.Label4 = new System.Windows.Forms.Label();
			this.Closing += new System.ComponentModel.CancelEventHandler( frmVariador_Closing );
			btInicio.Click += new System.EventHandler( btInicio_Click );
			btParada.Click += new System.EventHandler( btParada_Click );
			btReset.Click += new System.EventHandler( btReset_Click );
			ComboBox.SelectedValueChanged += new System.EventHandler( ComboBox_SelectedValueChanged );
			ComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler( ComboBox_KeyPress );
			rbPos.CheckedChanged += new System.EventHandler( rbNeg_CheckedChanged );
			rbNeg.CheckedChanged += new System.EventHandler( rbPos_CheckedChanged );
			tbVelocidad.Leave += new System.EventHandler( tbVelocidad_Leave );
			this.GroupBox1.SuspendLayout();
			this.Panel1.SuspendLayout();
			this.GroupBox3.SuspendLayout();
			this.SuspendLayout();
			//
			//GroupBox1
			//
			this.GroupBox1.Controls.Add(this.rbNeg);
			this.GroupBox1.Controls.Add(this.rbPos);
			this.GroupBox1.Controls.Add(this.Label5);
			this.GroupBox1.Controls.Add(this.tbVelocidad);
			this.GroupBox1.Controls.Add(this.lbVelocidad);
			this.GroupBox1.Controls.Add(this.Label1);
			this.GroupBox1.Controls.Add(this.Label2);
			this.GroupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
			this.GroupBox1.Location = new System.Drawing.Point(8, 48);
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.Size = new System.Drawing.Size(184, 80);
			this.GroupBox1.TabIndex = 0;
			this.GroupBox1.TabStop = false;
			this.GroupBox1.Text = "Parametros Consigna";
			//
			//rbNeg
			//
			this.rbNeg.Location = new System.Drawing.Point(160, 48);
			this.rbNeg.Name = "rbNeg";
			this.rbNeg.Size = new System.Drawing.Size(16, 24);
			this.rbNeg.TabIndex = 10;
			//
			//rbPos
			//
			this.rbPos.Checked = true;
			this.rbPos.Location = new System.Drawing.Point(112, 48);
			this.rbPos.Name = "rbPos";
			this.rbPos.Size = new System.Drawing.Size(16, 24);
			this.rbPos.TabIndex = 9;
			this.rbPos.TabStop = true;
			//
			//Label5
			//
			this.Label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
			this.Label5.Location = new System.Drawing.Point(16, 56);
			this.Label5.Name = "Label5";
			this.Label5.Size = new System.Drawing.Size(64, 16);
			this.Label5.TabIndex = 8;
			this.Label5.Text = "Sentido:";
			//
			//tbVelocidad
			//
			this.tbVelocidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
			this.tbVelocidad.Location = new System.Drawing.Point(96, 16);
			this.tbVelocidad.Name = "tbVelocidad";
			this.tbVelocidad.Size = new System.Drawing.Size(64, 20);
			this.tbVelocidad.TabIndex = 2;
			this.tbVelocidad.Text = "0";
			this.tbVelocidad.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			//
			//lbVelocidad
			//
			this.lbVelocidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
			this.lbVelocidad.Location = new System.Drawing.Point(16, 24);
			this.lbVelocidad.Name = "lbVelocidad";
			this.lbVelocidad.Size = new System.Drawing.Size(64, 16);
			this.lbVelocidad.TabIndex = 0;
			this.lbVelocidad.Text = "Velocidad:";
			//
			//Label1
			//
			this.Label1.BackColor = System.Drawing.Color.Gainsboro;
			this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
			this.Label1.Location = new System.Drawing.Point(136, 48);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(16, 24);
			this.Label1.TabIndex = 6;
			this.Label1.Text = "<";
			//
			//Label2
			//
			this.Label2.BackColor = System.Drawing.Color.Gainsboro;
			this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
			this.Label2.Location = new System.Drawing.Point(88, 48);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(16, 24);
			this.Label2.TabIndex = 7;
			this.Label2.Text = ">";
			//
			//Panel1
			//
			this.Panel1.Controls.Add(this.btReset);
			this.Panel1.Controls.Add(this.btParada);
			this.Panel1.Controls.Add(this.btInicio);
			this.Panel1.Location = new System.Drawing.Point(88, 152);
			this.Panel1.Name = "Panel1";
			this.Panel1.Size = new System.Drawing.Size(280, 32);
			this.Panel1.TabIndex = 1;
			//
			//btReset
			//
			this.btReset.Location = new System.Drawing.Point(208, 8);
			this.btReset.Name = "btReset";
			this.btReset.Size = new System.Drawing.Size(72, 24);
			this.btReset.TabIndex = 2;
			this.btReset.Text = "Reset";
			//
			//btParada
			//
			this.btParada.Location = new System.Drawing.Point(120, 8);
			this.btParada.Name = "btParada";
			this.btParada.Size = new System.Drawing.Size(72, 24);
			this.btParada.TabIndex = 1;
			this.btParada.Text = "Parada";
			//
			//btInicio
			//
			this.btInicio.Location = new System.Drawing.Point(32, 8);
			this.btInicio.Name = "btInicio";
			this.btInicio.Size = new System.Drawing.Size(72, 24);
			this.btInicio.TabIndex = 0;
			this.btInicio.Text = "Inicio";
			//
			//Stbar
			//
			this.Stbar.Location = new System.Drawing.Point(0, 188);
			this.Stbar.Name = "Stbar";
			this.Stbar.Size = new System.Drawing.Size(376, 16);
			this.Stbar.SizingGrip = false;
			this.Stbar.TabIndex = 4;
			this.Stbar.Text = "Código Estado: (Inactivo)";
			//
			//ComboBox
			//
			this.ComboBox.Location = new System.Drawing.Point(8, 16);
			this.ComboBox.Name = "ComboBox";
			this.ComboBox.Size = new System.Drawing.Size(168, 21);
			this.ComboBox.TabIndex = 5;
			//
			//GroupBox3
			//
			this.GroupBox3.Controls.Add(this.lbE);
			this.GroupBox3.Controls.Add(this.lbV);
			this.GroupBox3.Controls.Add(this.Label3);
			this.GroupBox3.Controls.Add(this.Label4);
			this.GroupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
			this.GroupBox3.Location = new System.Drawing.Point(200, 48);
			this.GroupBox3.Name = "GroupBox3";
			this.GroupBox3.Size = new System.Drawing.Size(168, 80);
			this.GroupBox3.TabIndex = 6;
			this.GroupBox3.TabStop = false;
			this.GroupBox3.Text = "Monotorizacion";
			//
			//lbE
			//
			this.lbE.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lbE.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
			this.lbE.Location = new System.Drawing.Point(72, 16);
			this.lbE.Name = "lbE";
			this.lbE.Size = new System.Drawing.Size(80, 23);
			this.lbE.TabIndex = 3;
			this.lbE.Text = "Inactivo";
			this.lbE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//lbV
			//
			this.lbV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lbV.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
			this.lbV.Location = new System.Drawing.Point(88, 48);
			this.lbV.Name = "lbV";
			this.lbV.Size = new System.Drawing.Size(64, 23);
			this.lbV.TabIndex = 2;
			this.lbV.Text = "0";
			this.lbV.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//Label3
			//
			this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
			this.Label3.Location = new System.Drawing.Point(8, 56);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(64, 16);
			this.Label3.TabIndex = 1;
			this.Label3.Text = "Velocidad:";
			//
			//Label4
			//
			this.Label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
			this.Label4.Location = new System.Drawing.Point(8, 24);
			this.Label4.Name = "Label4";
			this.Label4.Size = new System.Drawing.Size(64, 16);
			this.Label4.TabIndex = 0;
			this.Label4.Text = "Estado:";
			//
			//frmVariador
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.ClientSize = new System.Drawing.Size(376, 204);
			this.Controls.Add(this.GroupBox3);
			this.Controls.Add(this.ComboBox);
			this.Controls.Add(this.Stbar);
			this.Controls.Add(this.Panel1);
			this.Controls.Add(this.GroupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			this.Name = "frmVariador";
			this.Text = "Variador";
			this.GroupBox1.ResumeLayout(false);
			this.Panel1.ResumeLayout(false);
			this.GroupBox3.ResumeLayout(false);
			this.ResumeLayout(false);
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
				if (this.Visible == true)
				{
					RefreshVaridador();
					VariadorManual();
				}
				if ((this.Visible != _antesVisible))
				{
					if (_variadorSelected < 0)
					{
						variadores[_variadorSelected - 1].Mando("ActualizarError");
					}
				}
				_antesVisible = this.Visible;
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
		private void frmVariador_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Hide();
			e.Cancel = true;
		}
		private void btInicio_Click(object sender, System.EventArgs e)
		{
			_btInicioClick = true;
		}
		private void btParada_Click(object sender, System.EventArgs e)
		{
			_btParadaClick = true;
		}
		private void btReset_Click(object sender, System.EventArgs e)
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
		private void ComboBox_SelectedValueChanged(object sender, System.EventArgs e)
		{
			_comboBoxChange = true;
		}
		private void ComboBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			e.Handled = true;
		}
		private void rbNeg_CheckedChanged(object sender, System.EventArgs e)
		{
			_rbNegClick = rbNeg.Checked;
			_rbPosClick = !_rbNegClick;
		}
		private void rbPos_CheckedChanged(object sender, System.EventArgs e)
		{
			_rbPosClick = rbPos.Checked;
			_rbNegClick = !_rbPosClick;
		}
		private void tbVelocidad_Leave(object sender, System.EventArgs e)
		{
			if (!Information.IsNumeric(tbVelocidad.Text))
			{
				tbVelocidad.Focus();
                Interaction.MsgBox("El valor de velocidad introducido no es valido", MsgBoxStyle.OkOnly,"");
			}
		}
	}
}
