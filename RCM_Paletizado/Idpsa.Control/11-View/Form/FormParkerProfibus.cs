using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Idpsa.Control.Component;
using Idpsa.Control.Engine;
using Idpsa.Control.Tool;

namespace Idpsa.Control.View
{
	public class FormParkerProfibus : System.Windows.Forms.Form,IViewTaskOwner
	{
		private Label[] STWLab = new Label[16];
		private Label[] CTWLab = new Label[16];
		private Label[] CTWCLab = new Label[16];
		private bool[] CTWCValues = new bool[16];
	    private bool load = false;
	    private CompaxC3I20T11 Parker;
	    private Dictionary<string, CompaxC3I20T11> Parkers;
		private bool QuitErrorClicked;
		private bool StartAbsClicked;
		private bool StartRelClicked;
		private bool StartAddClicked;		
		private TON Timer = new TON();
		private bool btHomingClicked;
		private bool btnStarPkwClicked;
		private CompaxC3I20T11.PkwCommand.PkwAction pkwAccion;
		private CompaxC3I20T11.PkwCommand.PkwStructure.PkwC3Object.PkwObjectType pkwC3Obj;
        private ComboBox cbParker;
        private Label label3;
		private double pkwValor;
		#region " Código generado por el Diseñador de Windows Forms "
		public FormParkerProfibus(IEnumerable<CompaxC3I20T11> parkers) : base()
		{
			//El Diseñador de Windows Forms requiere esta llamada.
			InitializeComponent();
            this.Parkers = new Dictionary<string, CompaxC3I20T11>();
            parkers.ForEach(compax=>Parkers.Add(compax.Name,compax));
            Initialize();           			
		}

        private FormParkerProfibus()
            : base()
        {
            //El Diseñador de Windows Forms requiere esta llamada.
            InitializeComponent();

            Initialize();

        }

        private void Initialize()
        {
            STWLab[0] = SW0;
            STWLab[1] = SW1;
            STWLab[2] = SW2;
            STWLab[3] = SW3;
            STWLab[4] = SW4;
            STWLab[5] = SW5;
            STWLab[6] = SW6;
            STWLab[7] = SW7;
            STWLab[8] = SW8;
            STWLab[9] = SW9;
            STWLab[10] = SW10;
            STWLab[11] = SW11;
            STWLab[12] = SW12;
            STWLab[13] = SW13;
            STWLab[14] = SW14;
            STWLab[15] = SW15;
            CTWLab[0] = CW0;
            CTWLab[1] = CW1;
            CTWLab[2] = CW2;
            CTWLab[3] = CW3;
            CTWLab[4] = CW4;
            CTWLab[5] = CW5;
            CTWLab[6] = CW6;
            CTWLab[7] = CW7;
            CTWLab[8] = CW8;
            CTWLab[9] = CW9;
            CTWLab[10] = CW10;
            CTWLab[11] = CW11;
            CTWLab[12] = CW12;
            CTWLab[13] = CW13;
            CTWLab[14] = CW14;
            CTWLab[15] = CW15;
            //Agregar cualquier inicialización después de la llamada a InitializeComponent()
            //AddHandler CW0.Click, New System.EventHandler(AddressOf SW_Click)
            CTWCLab[0] = CWC0;
            CTWCLab[1] = CWC1;
            CTWCLab[2] = CWC2;
            CTWCLab[3] = CWC3;
            CTWCLab[4] = CWC4;
            CTWCLab[5] = CWC5;
            CTWCLab[6] = CWC6;
            CTWCLab[7] = CWC7;
            CTWCLab[8] = CWC8;
            CTWCLab[9] = CWC9;
            CTWCLab[10] = CWC10;
            CTWCLab[11] = CWC11;
            CTWCLab[12] = CWC12;
            CTWCLab[13] = CWC13;
            CTWCLab[14] = CWC14;
            CTWCLab[15] = CWC15;
            for (int i = 0; i <= 15; i++)
            {
                CTWCLab[i].Click += new System.EventHandler(CWC_Click);
            }
            for (int i = 0; i <= CTWCLab.Length - 1; i++)
            {
                CTWCLab[i].BackColor = Color.Yellow;
            }
            btStop.Enabled = false;



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
		internal System.Windows.Forms.Panel Panel1;
		internal System.Windows.Forms.Panel Panel2;
		internal System.Windows.Forms.Label label1;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.Label Label9;
		internal System.Windows.Forms.Panel Panel3;
		internal System.Windows.Forms.Label CW15;
		internal System.Windows.Forms.Label CW0;
		internal System.Windows.Forms.Label CW1;
		internal System.Windows.Forms.Label CW2;
		internal System.Windows.Forms.Label CW3;
		internal System.Windows.Forms.Label CW4;
		internal System.Windows.Forms.Label CW5;
		internal System.Windows.Forms.Label CW6;
		internal System.Windows.Forms.Label CW7;
		internal System.Windows.Forms.Label CW8;
		internal System.Windows.Forms.Label CW9;
		internal System.Windows.Forms.Label CW10;
		internal System.Windows.Forms.Label CW11;
		internal System.Windows.Forms.Label CW12;
		internal System.Windows.Forms.Label CW13;
		internal System.Windows.Forms.Label CW14;
		internal System.Windows.Forms.Label SW0;
		internal System.Windows.Forms.Label SW1;
		internal System.Windows.Forms.Label SW2;
		internal System.Windows.Forms.Label SW3;
		internal System.Windows.Forms.Label SW4;
		internal System.Windows.Forms.Label SW5;
		internal System.Windows.Forms.Label SW6;
		internal System.Windows.Forms.Label SW7;
		internal System.Windows.Forms.Label SW8;
		internal System.Windows.Forms.Label SW9;
		internal System.Windows.Forms.Label SW10;
		internal System.Windows.Forms.Label SW11;
		internal System.Windows.Forms.Label SW12;
		internal System.Windows.Forms.Label SW13;
		internal System.Windows.Forms.Label SW14;
		internal System.Windows.Forms.Label SW15;
		internal System.Windows.Forms.Label CWC0;
		internal System.Windows.Forms.Label CWC1;
		internal System.Windows.Forms.Label CWC2;
		internal System.Windows.Forms.Label CWC3;
		internal System.Windows.Forms.Label CWC4;
		internal System.Windows.Forms.Label CWC5;
		internal System.Windows.Forms.Label CWC6;
		internal System.Windows.Forms.Label CWC7;
		internal System.Windows.Forms.Label CWC8;
		internal System.Windows.Forms.Label CWC9;
		internal System.Windows.Forms.Label CWC10;
		internal System.Windows.Forms.Label CWC11;
		internal System.Windows.Forms.Label CWC12;
		internal System.Windows.Forms.Label CWC13;
		internal System.Windows.Forms.Label CWC14;
		internal System.Windows.Forms.Label CWC15;
		internal System.Windows.Forms.GroupBox GroupBox3;
		internal System.Windows.Forms.GroupBox GroupBox4;
		internal System.Windows.Forms.GroupBox GroupBox5;
		internal System.Windows.Forms.GroupBox GroupBox6;
		internal System.Windows.Forms.Label lbLectura;
		internal System.Windows.Forms.Label Label15;
		internal System.Windows.Forms.TextBox tbEscribir;
		internal System.Windows.Forms.Label Label14;
		internal System.Windows.Forms.Button btnStarPkw;
		internal System.Windows.Forms.ListBox lbAccion;
        internal System.Windows.Forms.MainMenu MainMenu1;
		internal System.Windows.Forms.ListBox cbPkwObj;
		internal System.Windows.Forms.Label lbError;
		internal System.Windows.Forms.Label Label11;
		internal System.Windows.Forms.Button Button5;
		internal System.Windows.Forms.GroupBox GroupBox1;
		internal System.Windows.Forms.Button btnNeg;
		internal System.Windows.Forms.Button btnPos;
		internal System.Windows.Forms.RadioButton rbJog;
		internal System.Windows.Forms.RadioButton rbAdd;
		internal System.Windows.Forms.RadioButton rbRel;
		internal System.Windows.Forms.RadioButton rbAbs;
		internal System.Windows.Forms.Button btStop;
		internal System.Windows.Forms.Button btStart;
		internal System.Windows.Forms.Button btHoming;
		internal System.Windows.Forms.Button Button4;
		internal System.Windows.Forms.Button Button3;
		internal System.Windows.Forms.Button Button2;
		internal System.Windows.Forms.Button Button1;
		internal System.Windows.Forms.GroupBox GroupBox2;
		internal System.Windows.Forms.Label lbT;
		internal System.Windows.Forms.Label Label12;
		internal System.Windows.Forms.Label LAE;
		internal System.Windows.Forms.Label Label10;
		internal System.Windows.Forms.Label lbAP;
		internal System.Windows.Forms.Label Label8;
		internal System.Windows.Forms.Label lbEstado;
		internal System.Windows.Forms.Label Label13;
		internal System.Windows.Forms.Label lbAV;
		internal System.Windows.Forms.Label Label6;
		internal System.Windows.Forms.GroupBox GroupBox7;
		internal System.Windows.Forms.Label Label5;
		internal System.Windows.Forms.TextBox tbCV;
		internal System.Windows.Forms.Label Label4;
		internal System.Windows.Forms.TextBox tbCA;
		internal System.Windows.Forms.Label lab1;
		internal System.Windows.Forms.TextBox tbCP;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.CW15 = new System.Windows.Forms.Label();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.CW0 = new System.Windows.Forms.Label();
            this.CW1 = new System.Windows.Forms.Label();
            this.CW2 = new System.Windows.Forms.Label();
            this.CW3 = new System.Windows.Forms.Label();
            this.CW4 = new System.Windows.Forms.Label();
            this.CW5 = new System.Windows.Forms.Label();
            this.CW6 = new System.Windows.Forms.Label();
            this.CW7 = new System.Windows.Forms.Label();
            this.CW8 = new System.Windows.Forms.Label();
            this.CW9 = new System.Windows.Forms.Label();
            this.CW10 = new System.Windows.Forms.Label();
            this.CW11 = new System.Windows.Forms.Label();
            this.CW12 = new System.Windows.Forms.Label();
            this.CW13 = new System.Windows.Forms.Label();
            this.CW14 = new System.Windows.Forms.Label();
            this.Panel2 = new System.Windows.Forms.Panel();
            this.SW0 = new System.Windows.Forms.Label();
            this.SW1 = new System.Windows.Forms.Label();
            this.SW2 = new System.Windows.Forms.Label();
            this.SW3 = new System.Windows.Forms.Label();
            this.SW4 = new System.Windows.Forms.Label();
            this.SW5 = new System.Windows.Forms.Label();
            this.SW6 = new System.Windows.Forms.Label();
            this.SW7 = new System.Windows.Forms.Label();
            this.SW8 = new System.Windows.Forms.Label();
            this.SW9 = new System.Windows.Forms.Label();
            this.SW10 = new System.Windows.Forms.Label();
            this.SW11 = new System.Windows.Forms.Label();
            this.SW12 = new System.Windows.Forms.Label();
            this.SW13 = new System.Windows.Forms.Label();
            this.SW14 = new System.Windows.Forms.Label();
            this.SW15 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.Panel3 = new System.Windows.Forms.Panel();
            this.CWC0 = new System.Windows.Forms.Label();
            this.CWC1 = new System.Windows.Forms.Label();
            this.CWC2 = new System.Windows.Forms.Label();
            this.CWC3 = new System.Windows.Forms.Label();
            this.CWC4 = new System.Windows.Forms.Label();
            this.CWC5 = new System.Windows.Forms.Label();
            this.CWC6 = new System.Windows.Forms.Label();
            this.CWC7 = new System.Windows.Forms.Label();
            this.CWC8 = new System.Windows.Forms.Label();
            this.CWC9 = new System.Windows.Forms.Label();
            this.CWC10 = new System.Windows.Forms.Label();
            this.CWC11 = new System.Windows.Forms.Label();
            this.CWC12 = new System.Windows.Forms.Label();
            this.CWC13 = new System.Windows.Forms.Label();
            this.CWC14 = new System.Windows.Forms.Label();
            this.CWC15 = new System.Windows.Forms.Label();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.btnStarPkw = new System.Windows.Forms.Button();
            this.GroupBox6 = new System.Windows.Forms.GroupBox();
            this.lbLectura = new System.Windows.Forms.Label();
            this.Label15 = new System.Windows.Forms.Label();
            this.tbEscribir = new System.Windows.Forms.TextBox();
            this.Label14 = new System.Windows.Forms.Label();
            this.GroupBox5 = new System.Windows.Forms.GroupBox();
            this.cbPkwObj = new System.Windows.Forms.ListBox();
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.lbAccion = new System.Windows.Forms.ListBox();
            this.MainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.lbError = new System.Windows.Forms.Label();
            this.Label11 = new System.Windows.Forms.Label();
            this.Button5 = new System.Windows.Forms.Button();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.btnNeg = new System.Windows.Forms.Button();
            this.btnPos = new System.Windows.Forms.Button();
            this.rbJog = new System.Windows.Forms.RadioButton();
            this.rbAdd = new System.Windows.Forms.RadioButton();
            this.rbRel = new System.Windows.Forms.RadioButton();
            this.rbAbs = new System.Windows.Forms.RadioButton();
            this.btStop = new System.Windows.Forms.Button();
            this.btStart = new System.Windows.Forms.Button();
            this.btHoming = new System.Windows.Forms.Button();
            this.Button4 = new System.Windows.Forms.Button();
            this.Button3 = new System.Windows.Forms.Button();
            this.Button2 = new System.Windows.Forms.Button();
            this.Button1 = new System.Windows.Forms.Button();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.lbT = new System.Windows.Forms.Label();
            this.Label12 = new System.Windows.Forms.Label();
            this.LAE = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.lbAP = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.lbEstado = new System.Windows.Forms.Label();
            this.Label13 = new System.Windows.Forms.Label();
            this.lbAV = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.GroupBox7 = new System.Windows.Forms.GroupBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.tbCV = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.tbCA = new System.Windows.Forms.TextBox();
            this.lab1 = new System.Windows.Forms.Label();
            this.tbCP = new System.Windows.Forms.TextBox();
            this.cbParker = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Panel1.SuspendLayout();
            this.Panel2.SuspendLayout();
            this.Panel3.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            this.GroupBox6.SuspendLayout();
            this.GroupBox5.SuspendLayout();
            this.GroupBox4.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.GroupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // CW15
            // 
            this.CW15.BackColor = System.Drawing.Color.White;
            this.CW15.Dock = System.Windows.Forms.DockStyle.Left;
            this.CW15.Location = new System.Drawing.Point(0, 0);
            this.CW15.Name = "CW15";
            this.CW15.Size = new System.Drawing.Size(56, 40);
            this.CW15.TabIndex = 0;
            this.CW15.Text = "15";
            this.CW15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.CW0);
            this.Panel1.Controls.Add(this.CW1);
            this.Panel1.Controls.Add(this.CW2);
            this.Panel1.Controls.Add(this.CW3);
            this.Panel1.Controls.Add(this.CW4);
            this.Panel1.Controls.Add(this.CW5);
            this.Panel1.Controls.Add(this.CW6);
            this.Panel1.Controls.Add(this.CW7);
            this.Panel1.Controls.Add(this.CW8);
            this.Panel1.Controls.Add(this.CW9);
            this.Panel1.Controls.Add(this.CW10);
            this.Panel1.Controls.Add(this.CW11);
            this.Panel1.Controls.Add(this.CW12);
            this.Panel1.Controls.Add(this.CW13);
            this.Panel1.Controls.Add(this.CW14);
            this.Panel1.Controls.Add(this.CW15);
            this.Panel1.Location = new System.Drawing.Point(16, 112);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(896, 40);
            this.Panel1.TabIndex = 1;
            // 
            // CW0
            // 
            this.CW0.BackColor = System.Drawing.Color.White;
            this.CW0.Dock = System.Windows.Forms.DockStyle.Left;
            this.CW0.Location = new System.Drawing.Point(840, 0);
            this.CW0.Name = "CW0";
            this.CW0.Size = new System.Drawing.Size(56, 40);
            this.CW0.TabIndex = 15;
            this.CW0.Text = "0";
            this.CW0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CW1
            // 
            this.CW1.BackColor = System.Drawing.Color.White;
            this.CW1.Dock = System.Windows.Forms.DockStyle.Left;
            this.CW1.Location = new System.Drawing.Point(784, 0);
            this.CW1.Name = "CW1";
            this.CW1.Size = new System.Drawing.Size(56, 40);
            this.CW1.TabIndex = 14;
            this.CW1.Text = "1";
            this.CW1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CW2
            // 
            this.CW2.BackColor = System.Drawing.Color.White;
            this.CW2.Dock = System.Windows.Forms.DockStyle.Left;
            this.CW2.Location = new System.Drawing.Point(728, 0);
            this.CW2.Name = "CW2";
            this.CW2.Size = new System.Drawing.Size(56, 40);
            this.CW2.TabIndex = 13;
            this.CW2.Text = "2";
            this.CW2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CW3
            // 
            this.CW3.BackColor = System.Drawing.Color.White;
            this.CW3.Dock = System.Windows.Forms.DockStyle.Left;
            this.CW3.Location = new System.Drawing.Point(672, 0);
            this.CW3.Name = "CW3";
            this.CW3.Size = new System.Drawing.Size(56, 40);
            this.CW3.TabIndex = 12;
            this.CW3.Text = "3";
            this.CW3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CW4
            // 
            this.CW4.BackColor = System.Drawing.Color.White;
            this.CW4.Dock = System.Windows.Forms.DockStyle.Left;
            this.CW4.Location = new System.Drawing.Point(616, 0);
            this.CW4.Name = "CW4";
            this.CW4.Size = new System.Drawing.Size(56, 40);
            this.CW4.TabIndex = 11;
            this.CW4.Text = "4";
            this.CW4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CW5
            // 
            this.CW5.BackColor = System.Drawing.Color.White;
            this.CW5.Dock = System.Windows.Forms.DockStyle.Left;
            this.CW5.Location = new System.Drawing.Point(560, 0);
            this.CW5.Name = "CW5";
            this.CW5.Size = new System.Drawing.Size(56, 40);
            this.CW5.TabIndex = 10;
            this.CW5.Text = "5";
            this.CW5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CW6
            // 
            this.CW6.BackColor = System.Drawing.Color.White;
            this.CW6.Dock = System.Windows.Forms.DockStyle.Left;
            this.CW6.Location = new System.Drawing.Point(504, 0);
            this.CW6.Name = "CW6";
            this.CW6.Size = new System.Drawing.Size(56, 40);
            this.CW6.TabIndex = 9;
            this.CW6.Text = "6";
            this.CW6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CW7
            // 
            this.CW7.BackColor = System.Drawing.Color.White;
            this.CW7.Dock = System.Windows.Forms.DockStyle.Left;
            this.CW7.Location = new System.Drawing.Point(448, 0);
            this.CW7.Name = "CW7";
            this.CW7.Size = new System.Drawing.Size(56, 40);
            this.CW7.TabIndex = 8;
            this.CW7.Text = "7";
            this.CW7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CW8
            // 
            this.CW8.BackColor = System.Drawing.Color.White;
            this.CW8.Dock = System.Windows.Forms.DockStyle.Left;
            this.CW8.Location = new System.Drawing.Point(392, 0);
            this.CW8.Name = "CW8";
            this.CW8.Size = new System.Drawing.Size(56, 40);
            this.CW8.TabIndex = 7;
            this.CW8.Text = "8";
            this.CW8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CW9
            // 
            this.CW9.BackColor = System.Drawing.Color.White;
            this.CW9.Dock = System.Windows.Forms.DockStyle.Left;
            this.CW9.Location = new System.Drawing.Point(336, 0);
            this.CW9.Name = "CW9";
            this.CW9.Size = new System.Drawing.Size(56, 40);
            this.CW9.TabIndex = 6;
            this.CW9.Text = "9";
            this.CW9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CW10
            // 
            this.CW10.BackColor = System.Drawing.Color.White;
            this.CW10.Dock = System.Windows.Forms.DockStyle.Left;
            this.CW10.Location = new System.Drawing.Point(280, 0);
            this.CW10.Name = "CW10";
            this.CW10.Size = new System.Drawing.Size(56, 40);
            this.CW10.TabIndex = 5;
            this.CW10.Text = "10";
            this.CW10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CW11
            // 
            this.CW11.BackColor = System.Drawing.Color.White;
            this.CW11.Dock = System.Windows.Forms.DockStyle.Left;
            this.CW11.Location = new System.Drawing.Point(224, 0);
            this.CW11.Name = "CW11";
            this.CW11.Size = new System.Drawing.Size(56, 40);
            this.CW11.TabIndex = 4;
            this.CW11.Text = "11";
            this.CW11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CW12
            // 
            this.CW12.BackColor = System.Drawing.Color.White;
            this.CW12.Dock = System.Windows.Forms.DockStyle.Left;
            this.CW12.Location = new System.Drawing.Point(168, 0);
            this.CW12.Name = "CW12";
            this.CW12.Size = new System.Drawing.Size(56, 40);
            this.CW12.TabIndex = 3;
            this.CW12.Text = "12";
            this.CW12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CW13
            // 
            this.CW13.BackColor = System.Drawing.Color.White;
            this.CW13.Dock = System.Windows.Forms.DockStyle.Left;
            this.CW13.Location = new System.Drawing.Point(112, 0);
            this.CW13.Name = "CW13";
            this.CW13.Size = new System.Drawing.Size(56, 40);
            this.CW13.TabIndex = 2;
            this.CW13.Text = "13";
            this.CW13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CW14
            // 
            this.CW14.BackColor = System.Drawing.Color.White;
            this.CW14.Dock = System.Windows.Forms.DockStyle.Left;
            this.CW14.Location = new System.Drawing.Point(56, 0);
            this.CW14.Name = "CW14";
            this.CW14.Size = new System.Drawing.Size(56, 40);
            this.CW14.TabIndex = 1;
            this.CW14.Text = "14";
            this.CW14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel2
            // 
            this.Panel2.Controls.Add(this.SW0);
            this.Panel2.Controls.Add(this.SW1);
            this.Panel2.Controls.Add(this.SW2);
            this.Panel2.Controls.Add(this.SW3);
            this.Panel2.Controls.Add(this.SW4);
            this.Panel2.Controls.Add(this.SW5);
            this.Panel2.Controls.Add(this.SW6);
            this.Panel2.Controls.Add(this.SW7);
            this.Panel2.Controls.Add(this.SW8);
            this.Panel2.Controls.Add(this.SW9);
            this.Panel2.Controls.Add(this.SW10);
            this.Panel2.Controls.Add(this.SW11);
            this.Panel2.Controls.Add(this.SW12);
            this.Panel2.Controls.Add(this.SW13);
            this.Panel2.Controls.Add(this.SW14);
            this.Panel2.Controls.Add(this.SW15);
            this.Panel2.Location = new System.Drawing.Point(16, 32);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(896, 40);
            this.Panel2.TabIndex = 2;
            // 
            // SW0
            // 
            this.SW0.BackColor = System.Drawing.Color.White;
            this.SW0.Dock = System.Windows.Forms.DockStyle.Left;
            this.SW0.Location = new System.Drawing.Point(840, 0);
            this.SW0.Name = "SW0";
            this.SW0.Size = new System.Drawing.Size(56, 40);
            this.SW0.TabIndex = 15;
            this.SW0.Text = "0";
            this.SW0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SW1
            // 
            this.SW1.BackColor = System.Drawing.Color.White;
            this.SW1.Dock = System.Windows.Forms.DockStyle.Left;
            this.SW1.Location = new System.Drawing.Point(784, 0);
            this.SW1.Name = "SW1";
            this.SW1.Size = new System.Drawing.Size(56, 40);
            this.SW1.TabIndex = 14;
            this.SW1.Text = "1";
            this.SW1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SW2
            // 
            this.SW2.BackColor = System.Drawing.Color.White;
            this.SW2.Dock = System.Windows.Forms.DockStyle.Left;
            this.SW2.Location = new System.Drawing.Point(728, 0);
            this.SW2.Name = "SW2";
            this.SW2.Size = new System.Drawing.Size(56, 40);
            this.SW2.TabIndex = 13;
            this.SW2.Text = "2";
            this.SW2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SW3
            // 
            this.SW3.BackColor = System.Drawing.Color.White;
            this.SW3.Dock = System.Windows.Forms.DockStyle.Left;
            this.SW3.Location = new System.Drawing.Point(672, 0);
            this.SW3.Name = "SW3";
            this.SW3.Size = new System.Drawing.Size(56, 40);
            this.SW3.TabIndex = 12;
            this.SW3.Text = "3";
            this.SW3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SW4
            // 
            this.SW4.BackColor = System.Drawing.Color.White;
            this.SW4.Dock = System.Windows.Forms.DockStyle.Left;
            this.SW4.Location = new System.Drawing.Point(616, 0);
            this.SW4.Name = "SW4";
            this.SW4.Size = new System.Drawing.Size(56, 40);
            this.SW4.TabIndex = 11;
            this.SW4.Text = "4";
            this.SW4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SW5
            // 
            this.SW5.BackColor = System.Drawing.Color.White;
            this.SW5.Dock = System.Windows.Forms.DockStyle.Left;
            this.SW5.Location = new System.Drawing.Point(560, 0);
            this.SW5.Name = "SW5";
            this.SW5.Size = new System.Drawing.Size(56, 40);
            this.SW5.TabIndex = 10;
            this.SW5.Text = "5";
            this.SW5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SW6
            // 
            this.SW6.BackColor = System.Drawing.Color.White;
            this.SW6.Dock = System.Windows.Forms.DockStyle.Left;
            this.SW6.Location = new System.Drawing.Point(504, 0);
            this.SW6.Name = "SW6";
            this.SW6.Size = new System.Drawing.Size(56, 40);
            this.SW6.TabIndex = 9;
            this.SW6.Text = "6";
            this.SW6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SW7
            // 
            this.SW7.BackColor = System.Drawing.Color.White;
            this.SW7.Dock = System.Windows.Forms.DockStyle.Left;
            this.SW7.Location = new System.Drawing.Point(448, 0);
            this.SW7.Name = "SW7";
            this.SW7.Size = new System.Drawing.Size(56, 40);
            this.SW7.TabIndex = 8;
            this.SW7.Text = "7";
            this.SW7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SW8
            // 
            this.SW8.BackColor = System.Drawing.Color.White;
            this.SW8.Dock = System.Windows.Forms.DockStyle.Left;
            this.SW8.Location = new System.Drawing.Point(392, 0);
            this.SW8.Name = "SW8";
            this.SW8.Size = new System.Drawing.Size(56, 40);
            this.SW8.TabIndex = 7;
            this.SW8.Text = "8";
            this.SW8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SW9
            // 
            this.SW9.BackColor = System.Drawing.Color.White;
            this.SW9.Dock = System.Windows.Forms.DockStyle.Left;
            this.SW9.Location = new System.Drawing.Point(336, 0);
            this.SW9.Name = "SW9";
            this.SW9.Size = new System.Drawing.Size(56, 40);
            this.SW9.TabIndex = 6;
            this.SW9.Text = "9";
            this.SW9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SW10
            // 
            this.SW10.BackColor = System.Drawing.Color.White;
            this.SW10.Dock = System.Windows.Forms.DockStyle.Left;
            this.SW10.Location = new System.Drawing.Point(280, 0);
            this.SW10.Name = "SW10";
            this.SW10.Size = new System.Drawing.Size(56, 40);
            this.SW10.TabIndex = 5;
            this.SW10.Text = "10";
            this.SW10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SW11
            // 
            this.SW11.BackColor = System.Drawing.Color.White;
            this.SW11.Dock = System.Windows.Forms.DockStyle.Left;
            this.SW11.Location = new System.Drawing.Point(224, 0);
            this.SW11.Name = "SW11";
            this.SW11.Size = new System.Drawing.Size(56, 40);
            this.SW11.TabIndex = 4;
            this.SW11.Text = "11";
            this.SW11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SW12
            // 
            this.SW12.BackColor = System.Drawing.Color.White;
            this.SW12.Dock = System.Windows.Forms.DockStyle.Left;
            this.SW12.Location = new System.Drawing.Point(168, 0);
            this.SW12.Name = "SW12";
            this.SW12.Size = new System.Drawing.Size(56, 40);
            this.SW12.TabIndex = 3;
            this.SW12.Text = "12";
            this.SW12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SW13
            // 
            this.SW13.BackColor = System.Drawing.Color.White;
            this.SW13.Dock = System.Windows.Forms.DockStyle.Left;
            this.SW13.Location = new System.Drawing.Point(112, 0);
            this.SW13.Name = "SW13";
            this.SW13.Size = new System.Drawing.Size(56, 40);
            this.SW13.TabIndex = 2;
            this.SW13.Text = "13";
            this.SW13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SW14
            // 
            this.SW14.BackColor = System.Drawing.Color.White;
            this.SW14.Dock = System.Windows.Forms.DockStyle.Left;
            this.SW14.Location = new System.Drawing.Point(56, 0);
            this.SW14.Name = "SW14";
            this.SW14.Size = new System.Drawing.Size(56, 40);
            this.SW14.TabIndex = 1;
            this.SW14.Text = "14";
            this.SW14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SW15
            // 
            this.SW15.BackColor = System.Drawing.Color.White;
            this.SW15.Dock = System.Windows.Forms.DockStyle.Left;
            this.SW15.Location = new System.Drawing.Point(0, 0);
            this.SW15.Name = "SW15";
            this.SW15.Size = new System.Drawing.Size(56, 40);
            this.SW15.TabIndex = 0;
            this.SW15.Text = "15";
            this.SW15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(416, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Control Word";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(416, 8);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(136, 16);
            this.Label2.TabIndex = 4;
            this.Label2.Text = "Status Word";
            this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label9
            // 
            this.Label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.Label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label9.Location = new System.Drawing.Point(416, 168);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(136, 16);
            this.Label9.TabIndex = 21;
            this.Label9.Text = "Control Word candidata";
            this.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel3
            // 
            this.Panel3.Controls.Add(this.CWC0);
            this.Panel3.Controls.Add(this.CWC1);
            this.Panel3.Controls.Add(this.CWC2);
            this.Panel3.Controls.Add(this.CWC3);
            this.Panel3.Controls.Add(this.CWC4);
            this.Panel3.Controls.Add(this.CWC5);
            this.Panel3.Controls.Add(this.CWC6);
            this.Panel3.Controls.Add(this.CWC7);
            this.Panel3.Controls.Add(this.CWC8);
            this.Panel3.Controls.Add(this.CWC9);
            this.Panel3.Controls.Add(this.CWC10);
            this.Panel3.Controls.Add(this.CWC11);
            this.Panel3.Controls.Add(this.CWC12);
            this.Panel3.Controls.Add(this.CWC13);
            this.Panel3.Controls.Add(this.CWC14);
            this.Panel3.Controls.Add(this.CWC15);
            this.Panel3.Location = new System.Drawing.Point(16, 192);
            this.Panel3.Name = "Panel3";
            this.Panel3.Size = new System.Drawing.Size(896, 40);
            this.Panel3.TabIndex = 20;
            // 
            // CWC0
            // 
            this.CWC0.BackColor = System.Drawing.Color.White;
            this.CWC0.Dock = System.Windows.Forms.DockStyle.Left;
            this.CWC0.Location = new System.Drawing.Point(840, 0);
            this.CWC0.Name = "CWC0";
            this.CWC0.Size = new System.Drawing.Size(56, 40);
            this.CWC0.TabIndex = 15;
            this.CWC0.Text = "0";
            this.CWC0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CWC1
            // 
            this.CWC1.BackColor = System.Drawing.Color.White;
            this.CWC1.Dock = System.Windows.Forms.DockStyle.Left;
            this.CWC1.Location = new System.Drawing.Point(784, 0);
            this.CWC1.Name = "CWC1";
            this.CWC1.Size = new System.Drawing.Size(56, 40);
            this.CWC1.TabIndex = 14;
            this.CWC1.Text = "1";
            this.CWC1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CWC2
            // 
            this.CWC2.BackColor = System.Drawing.Color.White;
            this.CWC2.Dock = System.Windows.Forms.DockStyle.Left;
            this.CWC2.Location = new System.Drawing.Point(728, 0);
            this.CWC2.Name = "CWC2";
            this.CWC2.Size = new System.Drawing.Size(56, 40);
            this.CWC2.TabIndex = 13;
            this.CWC2.Text = "2";
            this.CWC2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CWC3
            // 
            this.CWC3.BackColor = System.Drawing.Color.White;
            this.CWC3.Dock = System.Windows.Forms.DockStyle.Left;
            this.CWC3.Location = new System.Drawing.Point(672, 0);
            this.CWC3.Name = "CWC3";
            this.CWC3.Size = new System.Drawing.Size(56, 40);
            this.CWC3.TabIndex = 12;
            this.CWC3.Text = "3";
            this.CWC3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CWC4
            // 
            this.CWC4.BackColor = System.Drawing.Color.White;
            this.CWC4.Dock = System.Windows.Forms.DockStyle.Left;
            this.CWC4.Location = new System.Drawing.Point(616, 0);
            this.CWC4.Name = "CWC4";
            this.CWC4.Size = new System.Drawing.Size(56, 40);
            this.CWC4.TabIndex = 11;
            this.CWC4.Text = "4";
            this.CWC4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CWC5
            // 
            this.CWC5.BackColor = System.Drawing.Color.White;
            this.CWC5.Dock = System.Windows.Forms.DockStyle.Left;
            this.CWC5.Location = new System.Drawing.Point(560, 0);
            this.CWC5.Name = "CWC5";
            this.CWC5.Size = new System.Drawing.Size(56, 40);
            this.CWC5.TabIndex = 10;
            this.CWC5.Text = "5";
            this.CWC5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CWC6
            // 
            this.CWC6.BackColor = System.Drawing.Color.White;
            this.CWC6.Dock = System.Windows.Forms.DockStyle.Left;
            this.CWC6.Location = new System.Drawing.Point(504, 0);
            this.CWC6.Name = "CWC6";
            this.CWC6.Size = new System.Drawing.Size(56, 40);
            this.CWC6.TabIndex = 9;
            this.CWC6.Text = "6";
            this.CWC6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CWC7
            // 
            this.CWC7.BackColor = System.Drawing.Color.White;
            this.CWC7.Dock = System.Windows.Forms.DockStyle.Left;
            this.CWC7.Location = new System.Drawing.Point(448, 0);
            this.CWC7.Name = "CWC7";
            this.CWC7.Size = new System.Drawing.Size(56, 40);
            this.CWC7.TabIndex = 8;
            this.CWC7.Text = "7";
            this.CWC7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CWC8
            // 
            this.CWC8.BackColor = System.Drawing.Color.White;
            this.CWC8.Dock = System.Windows.Forms.DockStyle.Left;
            this.CWC8.Location = new System.Drawing.Point(392, 0);
            this.CWC8.Name = "CWC8";
            this.CWC8.Size = new System.Drawing.Size(56, 40);
            this.CWC8.TabIndex = 7;
            this.CWC8.Text = "8";
            this.CWC8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CWC9
            // 
            this.CWC9.BackColor = System.Drawing.Color.White;
            this.CWC9.Dock = System.Windows.Forms.DockStyle.Left;
            this.CWC9.Location = new System.Drawing.Point(336, 0);
            this.CWC9.Name = "CWC9";
            this.CWC9.Size = new System.Drawing.Size(56, 40);
            this.CWC9.TabIndex = 6;
            this.CWC9.Text = "9";
            this.CWC9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CWC10
            // 
            this.CWC10.BackColor = System.Drawing.Color.White;
            this.CWC10.Dock = System.Windows.Forms.DockStyle.Left;
            this.CWC10.Location = new System.Drawing.Point(280, 0);
            this.CWC10.Name = "CWC10";
            this.CWC10.Size = new System.Drawing.Size(56, 40);
            this.CWC10.TabIndex = 5;
            this.CWC10.Text = "10";
            this.CWC10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CWC11
            // 
            this.CWC11.BackColor = System.Drawing.Color.White;
            this.CWC11.Dock = System.Windows.Forms.DockStyle.Left;
            this.CWC11.Location = new System.Drawing.Point(224, 0);
            this.CWC11.Name = "CWC11";
            this.CWC11.Size = new System.Drawing.Size(56, 40);
            this.CWC11.TabIndex = 4;
            this.CWC11.Text = "11";
            this.CWC11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CWC12
            // 
            this.CWC12.BackColor = System.Drawing.Color.White;
            this.CWC12.Dock = System.Windows.Forms.DockStyle.Left;
            this.CWC12.Location = new System.Drawing.Point(168, 0);
            this.CWC12.Name = "CWC12";
            this.CWC12.Size = new System.Drawing.Size(56, 40);
            this.CWC12.TabIndex = 3;
            this.CWC12.Text = "12";
            this.CWC12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CWC13
            // 
            this.CWC13.BackColor = System.Drawing.Color.White;
            this.CWC13.Dock = System.Windows.Forms.DockStyle.Left;
            this.CWC13.Location = new System.Drawing.Point(112, 0);
            this.CWC13.Name = "CWC13";
            this.CWC13.Size = new System.Drawing.Size(56, 40);
            this.CWC13.TabIndex = 2;
            this.CWC13.Text = "13";
            this.CWC13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CWC14
            // 
            this.CWC14.BackColor = System.Drawing.Color.White;
            this.CWC14.Dock = System.Windows.Forms.DockStyle.Left;
            this.CWC14.Location = new System.Drawing.Point(56, 0);
            this.CWC14.Name = "CWC14";
            this.CWC14.Size = new System.Drawing.Size(56, 40);
            this.CWC14.TabIndex = 1;
            this.CWC14.Text = "14";
            this.CWC14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CWC15
            // 
            this.CWC15.BackColor = System.Drawing.Color.White;
            this.CWC15.Dock = System.Windows.Forms.DockStyle.Left;
            this.CWC15.Location = new System.Drawing.Point(0, 0);
            this.CWC15.Name = "CWC15";
            this.CWC15.Size = new System.Drawing.Size(56, 40);
            this.CWC15.TabIndex = 0;
            this.CWC15.Text = "15";
            this.CWC15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GroupBox3
            // 
            this.GroupBox3.Controls.Add(this.btnStarPkw);
            this.GroupBox3.Controls.Add(this.GroupBox6);
            this.GroupBox3.Controls.Add(this.GroupBox5);
            this.GroupBox3.Controls.Add(this.GroupBox4);
            this.GroupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox3.Location = new System.Drawing.Point(8, 535);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(912, 200);
            this.GroupBox3.TabIndex = 26;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "Pkw :";
            // 
            // btnStarPkw
            // 
            this.btnStarPkw.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStarPkw.Location = new System.Drawing.Point(808, 156);
            this.btnStarPkw.Name = "btnStarPkw";
            this.btnStarPkw.Size = new System.Drawing.Size(88, 32);
            this.btnStarPkw.TabIndex = 6;
            this.btnStarPkw.Text = "Start Pkw";
            this.btnStarPkw.Click += new System.EventHandler(this.btnStarPkw_Click);
            // 
            // GroupBox6
            // 
            this.GroupBox6.Controls.Add(this.lbLectura);
            this.GroupBox6.Controls.Add(this.Label15);
            this.GroupBox6.Controls.Add(this.tbEscribir);
            this.GroupBox6.Controls.Add(this.Label14);
            this.GroupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox6.Location = new System.Drawing.Point(594, 32);
            this.GroupBox6.Name = "GroupBox6";
            this.GroupBox6.Size = new System.Drawing.Size(248, 112);
            this.GroupBox6.TabIndex = 5;
            this.GroupBox6.TabStop = false;
            this.GroupBox6.Text = "Valores R/W objeto C3 :";
            // 
            // lbLectura
            // 
            this.lbLectura.BackColor = System.Drawing.Color.Black;
            this.lbLectura.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLectura.ForeColor = System.Drawing.Color.White;
            this.lbLectura.Location = new System.Drawing.Point(88, 32);
            this.lbLectura.Name = "lbLectura";
            this.lbLectura.Size = new System.Drawing.Size(144, 24);
            this.lbLectura.TabIndex = 13;
            this.lbLectura.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label15
            // 
            this.Label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label15.Location = new System.Drawing.Point(16, 72);
            this.Label15.Name = "Label15";
            this.Label15.Size = new System.Drawing.Size(64, 24);
            this.Label15.TabIndex = 12;
            this.Label15.Text = "Escribir :";
            // 
            // tbEscribir
            // 
            this.tbEscribir.Location = new System.Drawing.Point(88, 72);
            this.tbEscribir.Name = "tbEscribir";
            this.tbEscribir.Size = new System.Drawing.Size(144, 20);
            this.tbEscribir.TabIndex = 11;
            // 
            // Label14
            // 
            this.Label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label14.Location = new System.Drawing.Point(16, 32);
            this.Label14.Name = "Label14";
            this.Label14.Size = new System.Drawing.Size(64, 24);
            this.Label14.TabIndex = 10;
            this.Label14.Text = "Lectura :";
            // 
            // GroupBox5
            // 
            this.GroupBox5.Controls.Add(this.cbPkwObj);
            this.GroupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox5.Location = new System.Drawing.Point(312, 14);
            this.GroupBox5.Name = "GroupBox5";
            this.GroupBox5.Size = new System.Drawing.Size(216, 176);
            this.GroupBox5.TabIndex = 4;
            this.GroupBox5.TabStop = false;
            this.GroupBox5.Text = "Seleccion de Objecto C3 :";
            // 
            // cbPkwObj
            // 
            this.cbPkwObj.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbPkwObj.Location = new System.Drawing.Point(3, 16);
            this.cbPkwObj.Name = "cbPkwObj";
            this.cbPkwObj.Size = new System.Drawing.Size(210, 147);
            this.cbPkwObj.TabIndex = 31;
            // 
            // GroupBox4
            // 
            this.GroupBox4.Controls.Add(this.lbAccion);
            this.GroupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox4.Location = new System.Drawing.Point(72, 32);
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.Size = new System.Drawing.Size(168, 136);
            this.GroupBox4.TabIndex = 2;
            this.GroupBox4.TabStop = false;
            this.GroupBox4.Text = "Accion ";
            // 
            // lbAccion
            // 
            this.lbAccion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbAccion.Location = new System.Drawing.Point(3, 16);
            this.lbAccion.Name = "lbAccion";
            this.lbAccion.Size = new System.Drawing.Size(162, 108);
            this.lbAccion.TabIndex = 0;
            // 
            // lbError
            // 
            this.lbError.BackColor = System.Drawing.Color.IndianRed;
            this.lbError.ForeColor = System.Drawing.Color.White;
            this.lbError.Location = new System.Drawing.Point(112, 285);
            this.lbError.Name = "lbError";
            this.lbError.Size = new System.Drawing.Size(456, 16);
            this.lbError.TabIndex = 24;
            this.lbError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label11
            // 
            this.Label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label11.Location = new System.Drawing.Point(32, 285);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(72, 16);
            this.Label11.TabIndex = 23;
            this.Label11.Text = "Ultimo error :";
            // 
            // Button5
            // 
            this.Button5.Location = new System.Drawing.Point(760, 238);
            this.Button5.Name = "Button5";
            this.Button5.Size = new System.Drawing.Size(152, 24);
            this.Button5.TabIndex = 22;
            this.Button5.Text = "Set CW candidata";
            this.Button5.Click += new System.EventHandler(this.Button5_Click);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.btnNeg);
            this.GroupBox1.Controls.Add(this.btnPos);
            this.GroupBox1.Controls.Add(this.rbJog);
            this.GroupBox1.Controls.Add(this.rbAdd);
            this.GroupBox1.Controls.Add(this.rbRel);
            this.GroupBox1.Controls.Add(this.rbAbs);
            this.GroupBox1.Controls.Add(this.btStop);
            this.GroupBox1.Controls.Add(this.btStart);
            this.GroupBox1.Controls.Add(this.btHoming);
            this.GroupBox1.Controls.Add(this.Button4);
            this.GroupBox1.Controls.Add(this.Button3);
            this.GroupBox1.Controls.Add(this.Button2);
            this.GroupBox1.Controls.Add(this.Button1);
            this.GroupBox1.Controls.Add(this.GroupBox2);
            this.GroupBox1.Controls.Add(this.GroupBox7);
            this.GroupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox1.Location = new System.Drawing.Point(8, 317);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(912, 216);
            this.GroupBox1.TabIndex = 27;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Posicionamiento directo :";
            // 
            // btnNeg
            // 
            this.btnNeg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnNeg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNeg.Location = new System.Drawing.Point(718, 62);
            this.btnNeg.Name = "btnNeg";
            this.btnNeg.Size = new System.Drawing.Size(75, 23);
            this.btnNeg.TabIndex = 56;
            this.btnNeg.Text = "<";
            this.btnNeg.UseVisualStyleBackColor = false;
            this.btnNeg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNeg_MouseDown);
            this.btnNeg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnNeg_MouseUp);
            // 
            // btnPos
            // 
            this.btnPos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPos.Location = new System.Drawing.Point(828, 62);
            this.btnPos.Name = "btnPos";
            this.btnPos.Size = new System.Drawing.Size(75, 23);
            this.btnPos.TabIndex = 55;
            this.btnPos.Text = ">";
            this.btnPos.UseVisualStyleBackColor = false;
            this.btnPos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPos_MouseDown);
            this.btnPos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPos_MouseUp);
            // 
            // rbJog
            // 
            this.rbJog.Checked = true;
            this.rbJog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbJog.Location = new System.Drawing.Point(790, 36);
            this.rbJog.Name = "rbJog";
            this.rbJog.Size = new System.Drawing.Size(56, 16);
            this.rbJog.TabIndex = 54;
            this.rbJog.TabStop = true;
            this.rbJog.Text = "Jog";
            this.rbJog.CheckedChanged += new System.EventHandler(this.rbJog_CheckedChanged_1);
            // 
            // rbAdd
            // 
            this.rbAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbAdd.Location = new System.Drawing.Point(848, 120);
            this.rbAdd.Name = "rbAdd";
            this.rbAdd.Size = new System.Drawing.Size(56, 16);
            this.rbAdd.TabIndex = 53;
            this.rbAdd.Text = "Add";
            // 
            // rbRel
            // 
            this.rbRel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbRel.Location = new System.Drawing.Point(784, 120);
            this.rbRel.Name = "rbRel";
            this.rbRel.Size = new System.Drawing.Size(40, 16);
            this.rbRel.TabIndex = 52;
            this.rbRel.Text = "Rel";
            // 
            // rbAbs
            // 
            this.rbAbs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbAbs.Location = new System.Drawing.Point(720, 120);
            this.rbAbs.Name = "rbAbs";
            this.rbAbs.Size = new System.Drawing.Size(44, 16);
            this.rbAbs.TabIndex = 51;
            this.rbAbs.Text = "Abs";
            // 
            // btStop
            // 
            this.btStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btStop.Location = new System.Drawing.Point(762, 178);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(86, 23);
            this.btStop.TabIndex = 50;
            this.btStop.Text = "Stop";
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // btStart
            // 
            this.btStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btStart.Location = new System.Drawing.Point(762, 146);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(86, 24);
            this.btStart.TabIndex = 49;
            this.btStart.Text = "Start";
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // btHoming
            // 
            this.btHoming.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btHoming.Location = new System.Drawing.Point(568, 178);
            this.btHoming.Name = "btHoming";
            this.btHoming.Size = new System.Drawing.Size(112, 24);
            this.btHoming.TabIndex = 48;
            this.btHoming.Text = "Homing";
            this.btHoming.Click += new System.EventHandler(this.btHoming_Click);
            // 
            // Button4
            // 
            this.Button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button4.Location = new System.Drawing.Point(568, 130);
            this.Button4.Name = "Button4";
            this.Button4.Size = new System.Drawing.Size(112, 23);
            this.Button4.TabIndex = 47;
            this.Button4.Text = "Quitar Error";
            this.Button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // Button3
            // 
            this.Button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button3.Location = new System.Drawing.Point(568, 90);
            this.Button3.Name = "Button3";
            this.Button3.Size = new System.Drawing.Size(112, 23);
            this.Button3.TabIndex = 46;
            this.Button3.Text = "Habilitar Operacion";
            this.Button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // Button2
            // 
            this.Button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button2.Location = new System.Drawing.Point(568, 58);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(112, 23);
            this.Button2.TabIndex = 45;
            this.Button2.Text = "Habilitar Driver";
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Button1
            // 
            this.Button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button1.Location = new System.Drawing.Point(568, 26);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(112, 23);
            this.Button1.TabIndex = 44;
            this.Button1.Text = "Energizar";
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.lbT);
            this.GroupBox2.Controls.Add(this.Label12);
            this.GroupBox2.Controls.Add(this.LAE);
            this.GroupBox2.Controls.Add(this.Label10);
            this.GroupBox2.Controls.Add(this.lbAP);
            this.GroupBox2.Controls.Add(this.Label8);
            this.GroupBox2.Controls.Add(this.lbEstado);
            this.GroupBox2.Controls.Add(this.Label13);
            this.GroupBox2.Controls.Add(this.lbAV);
            this.GroupBox2.Controls.Add(this.Label6);
            this.GroupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox2.Location = new System.Drawing.Point(280, 16);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(248, 192);
            this.GroupBox2.TabIndex = 43;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Valores actuales :";
            // 
            // lbT
            // 
            this.lbT.BackColor = System.Drawing.SystemColors.ControlText;
            this.lbT.ForeColor = System.Drawing.Color.White;
            this.lbT.Location = new System.Drawing.Point(104, 168);
            this.lbT.Name = "lbT";
            this.lbT.Size = new System.Drawing.Size(96, 16);
            this.lbT.TabIndex = 16;
            // 
            // Label12
            // 
            this.Label12.Location = new System.Drawing.Point(24, 168);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(72, 16);
            this.Label12.TabIndex = 15;
            this.Label12.Text = "Torque :";
            // 
            // LAE
            // 
            this.LAE.BackColor = System.Drawing.SystemColors.ControlText;
            this.LAE.ForeColor = System.Drawing.Color.White;
            this.LAE.Location = new System.Drawing.Point(104, 136);
            this.LAE.Name = "LAE";
            this.LAE.Size = new System.Drawing.Size(96, 16);
            this.LAE.TabIndex = 14;
            // 
            // Label10
            // 
            this.Label10.Location = new System.Drawing.Point(24, 136);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(72, 16);
            this.Label10.TabIndex = 13;
            this.Label10.Text = "Cod. Error :";
            // 
            // lbAP
            // 
            this.lbAP.BackColor = System.Drawing.Color.Black;
            this.lbAP.ForeColor = System.Drawing.Color.White;
            this.lbAP.Location = new System.Drawing.Point(104, 56);
            this.lbAP.Name = "lbAP";
            this.lbAP.Size = new System.Drawing.Size(96, 16);
            this.lbAP.TabIndex = 12;
            // 
            // Label8
            // 
            this.Label8.Location = new System.Drawing.Point(24, 56);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(72, 16);
            this.Label8.TabIndex = 11;
            this.Label8.Text = "Posicion";
            // 
            // lbEstado
            // 
            this.lbEstado.BackColor = System.Drawing.Color.Navy;
            this.lbEstado.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbEstado.ForeColor = System.Drawing.Color.White;
            this.lbEstado.Location = new System.Drawing.Point(88, 24);
            this.lbEstado.Name = "lbEstado";
            this.lbEstado.Size = new System.Drawing.Size(144, 16);
            this.lbEstado.TabIndex = 10;
            this.lbEstado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label13
            // 
            this.Label13.Location = new System.Drawing.Point(24, 24);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(48, 16);
            this.Label13.TabIndex = 9;
            this.Label13.Text = "Estado :";
            // 
            // lbAV
            // 
            this.lbAV.BackColor = System.Drawing.SystemColors.ControlText;
            this.lbAV.ForeColor = System.Drawing.Color.White;
            this.lbAV.Location = new System.Drawing.Point(104, 96);
            this.lbAV.Name = "lbAV";
            this.lbAV.Size = new System.Drawing.Size(96, 16);
            this.lbAV.TabIndex = 7;
            // 
            // Label6
            // 
            this.Label6.Location = new System.Drawing.Point(24, 96);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(72, 16);
            this.Label6.TabIndex = 5;
            this.Label6.Text = "Velcidad :";
            // 
            // GroupBox7
            // 
            this.GroupBox7.Controls.Add(this.Label5);
            this.GroupBox7.Controls.Add(this.tbCV);
            this.GroupBox7.Controls.Add(this.Label4);
            this.GroupBox7.Controls.Add(this.tbCA);
            this.GroupBox7.Controls.Add(this.lab1);
            this.GroupBox7.Controls.Add(this.tbCP);
            this.GroupBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox7.Location = new System.Drawing.Point(8, 16);
            this.GroupBox7.Name = "GroupBox7";
            this.GroupBox7.Size = new System.Drawing.Size(248, 192);
            this.GroupBox7.TabIndex = 42;
            this.GroupBox7.TabStop = false;
            this.GroupBox7.Text = "Valores consigna :";
            // 
            // Label5
            // 
            this.Label5.Location = new System.Drawing.Point(32, 88);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(72, 16);
            this.Label5.TabIndex = 5;
            this.Label5.Text = "Velocidad";
            // 
            // tbCV
            // 
            this.tbCV.Location = new System.Drawing.Point(112, 88);
            this.tbCV.Name = "tbCV";
            this.tbCV.Size = new System.Drawing.Size(100, 20);
            this.tbCV.TabIndex = 4;
            // 
            // Label4
            // 
            this.Label4.Location = new System.Drawing.Point(32, 144);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(72, 16);
            this.Label4.TabIndex = 3;
            this.Label4.Text = "Aceleracion :";
            // 
            // tbCA
            // 
            this.tbCA.Location = new System.Drawing.Point(112, 144);
            this.tbCA.Name = "tbCA";
            this.tbCA.Size = new System.Drawing.Size(100, 20);
            this.tbCA.TabIndex = 2;
            // 
            // lab1
            // 
            this.lab1.Location = new System.Drawing.Point(32, 32);
            this.lab1.Name = "lab1";
            this.lab1.Size = new System.Drawing.Size(72, 16);
            this.lab1.TabIndex = 1;
            this.lab1.Text = "Posicion :";
            // 
            // tbCP
            // 
            this.tbCP.Location = new System.Drawing.Point(112, 32);
            this.tbCP.Name = "tbCP";
            this.tbCP.Size = new System.Drawing.Size(100, 20);
            this.tbCP.TabIndex = 0;
            // 
            // cbParker
            // 
            this.cbParker.FormattingEnabled = true;
            this.cbParker.Location = new System.Drawing.Point(113, 248);
            this.cbParker.Name = "cbParker";
            this.cbParker.Size = new System.Drawing.Size(158, 21);
            this.cbParker.TabIndex = 28;
            this.cbParker.SelectedIndexChanged += new System.EventHandler(this.cbParker_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(60, 251);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Parker :";
            // 
            // frmParkerProfibus
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(928, 743);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbParker);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.GroupBox3);
            this.Controls.Add(this.lbError);
            this.Controls.Add(this.Label11);
            this.Controls.Add(this.Button5);
            this.Controls.Add(this.Label9);
            this.Controls.Add(this.Panel3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Panel2);
            this.Controls.Add(this.Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Menu = this.MainMenu1;
            this.Name = "frmParkerProfibus";
            this.Text = "Control I20T11 (Profibus)";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ParkerProfibus_Load);
            this.Panel1.ResumeLayout(false);
            this.Panel2.ResumeLayout(false);
            this.Panel3.ResumeLayout(false);
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox6.ResumeLayout(false);
            this.GroupBox6.PerformLayout();
            this.GroupBox5.ResumeLayout(false);
            this.GroupBox4.ResumeLayout(false);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox7.ResumeLayout(false);
            this.GroupBox7.PerformLayout();
            this.Closing += frm_Closing;
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		private void Run()
		{
            if (Visible && !(WindowState== FormWindowState.Minimized)&&load)
		    {
		        RefreshWindow();
		        QuitError();
		        startAbsMovement();
		        startRelMovement();
		        startAddMovement();
		        Pwk();
		        Homing();
		    }
		}
		public void Pwk()
		{
			if (btnStarPkwClicked)
			{
				if (Parker.PkwExecuteCommand(pkwAccion, pkwC3Obj, pkwValor))
				{
					btnStarPkwClicked = false;
					HabilitarControlsPwk(true);
				}
				lbLectura.Text = Parker.PkwLastValeRead().ToString();
			}
		}
		private void startAbsMovement()
		{
			if (StartAbsClicked)
			{
                if (Parker.StartMov(int.Parse(tbCP.Text), int.Parse(tbCV.Text), int.Parse(tbCA.Text)))
				{
					StartAbsClicked = false;
				}
			}
		}
		private void startRelMovement()
		{
			if (StartRelClicked)
			{
                if (Parker.StartRelMov(int.Parse(tbCP.Text), int.Parse(tbCV.Text), int.Parse(tbCA.Text)))
				{
					StartRelClicked = false;
				}
			}
		}
		private void startAddMovement()
		{
			if (StartAddClicked)
			{
                if (Parker.StartRelMov(int.Parse(tbCP.Text), int.Parse(tbCV.Text), int.Parse(tbCA.Text)))
				{
					StartAddClicked = false;
				}
			}
		}

		public void QuitError()
		{           
			if (QuitErrorClicked)			
                if (Parker.QuitError())
                    QuitErrorClicked = false;			
		}

		public void Homing()
		{
			if (this.btHomingClicked)
			{
				if (Parker.Homing())
				{
					btHomingClicked = false;
				}
			}
		}
		public void RefreshWindow()
		{
			bool[] stwV = Parker.ReadStatusWord();
			bool[] ctwV = Parker.ReadControlWord();
			for (int i = 0; i <= stwV.Length - 1; i++) {
				if (stwV[i] == true)
				{
					this.STWLab[i].BackColor = Color.Green;
				}
				else
				{
					this.STWLab[i].BackColor = Color.Yellow;
				}
			}
			for (int i = 0; i <= stwV.Length - 1; i++) {
				if (ctwV[i] == true)
				{
					this.CTWLab[i].BackColor = Color.Green;
				}
				else
				{
					this.CTWLab[i].BackColor = Color.Yellow;
				}
			}
			lbEstado.Text = Parker.CurrentState();
			lbAP.Text = Parker.Posicion.ToString();
			lbAV.Text = Parker.Velocidad.ToString();
			LAE.Text = Parker.LastCodeError.ToString();
			lbT.Text = Parker.Torque.ToString();
			lbError.Text = Parker.LastError();
		}
		
		private void CWC_Click(object sender, System.EventArgs e)
		{
			Label l = (Label)sender;
			int i;
			for (i = 0; i <= 15; i++) {
				if (l.Name == "CWC" + i.ToString())
				{
					break;
				}
			}
			CTWCValues[i] = !CTWCValues[i];
			if (CTWCValues[i])
			{
				CTWCLab[i].BackColor = Color.Green;
			}
			else
			{
				CTWCLab[i].BackColor = Color.Yellow;
			}
		}
		private void Button1_Click(object sender, System.EventArgs e)
		{
			Parker.Energizar();
		}
		private void Button2_Click(object sender, System.EventArgs e)
		{
			Parker.HabilitarDriver();
		}
		private void Button3_Click(object sender, System.EventArgs e)
		{
			Parker.HabilitarOperacion();
		}
		private void Button4_Click(object sender, System.EventArgs e)
		{			
			QuitErrorClicked = true;
		}
		private void btnNeg_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Parker.Jog2 = true;
				Parker.Jog1 = false;
				this.btnNeg.BackColor = Color.FromArgb(128, 255, 128);
			}
		}
		private void btnNeg_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Parker.Jog1 = false;
				Parker.Jog2 = false;
				this.btnNeg.BackColor = Color.FromArgb(255, 255, 128);
			}
		}
		private void btnPos_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Parker.Jog1 = true;
				Parker.Jog2 = false;
				this.btnPos.BackColor = Color.FromArgb(128, 255, 128);
			}
		}
		private void btnPos_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Parker.Jog1 = false;
				Parker.Jog2 = false;
				this.btnPos.BackColor = Color.FromArgb(255, 255, 128);
			}
		}
		private void Button5_Click(object sender, System.EventArgs e)
		{
			Parker.WriteControlWord(CTWCValues);
		}
		private void btHoming_Click(object sender, System.EventArgs e)
		{
			btHomingClicked = true;
		}
		private void ParkerProfibus_Load(object sender, System.EventArgs e)
		{
		   
            foreach (string str in Parkers.Keys)
                cbParker.Items.Add(str);

		    cbParker.SelectedIndex = 0;
		    Parker = Parkers[(string) cbParker.SelectedItem];
            CompaxC3I20T11.PkwCommand.PkwAction[] Acciones = (CompaxC3I20T11.PkwCommand.PkwAction[])System.Enum.GetValues(typeof(CompaxC3I20T11.PkwCommand.PkwAction));
            CompaxC3I20T11.PkwCommand.PkwStructure.PkwC3Object.PkwObjectType[] C3Ob = (CompaxC3I20T11.PkwCommand.PkwStructure.PkwC3Object.PkwObjectType[])System.Enum.GetValues(typeof(CompaxC3I20T11.PkwCommand.PkwStructure.PkwC3Object.PkwObjectType));
			for (int i = 0; i <= Acciones.Length - 1; i++) {
				lbAccion.Items.Add(Acciones[i]);
			}
			for (int i = 0; i <= C3Ob.Length - 1; i++) {
				cbPkwObj.Items.Add(C3Ob[i]);
			}
			lbAccion.SelectedIndex = 0;
			cbPkwObj.SelectedIndex = 0;
		    load = true;
		}
		private void HabilitarControlsPwk(bool Enable)
		{
			this.cbPkwObj.Enabled = Enable;
			this.lbAccion.Enabled = Enable;
			this.btnStarPkw.Enabled = Enable;
		}
		private void btnStarPkw_Click(object sender, System.EventArgs e)
		{
			if (!(this.lbAccion.SelectedItem == null) && !(this.cbPkwObj.SelectedItem == null))
			{
				if (((CompaxC3I20T11.PkwCommand.PkwAction)this.lbAccion.SelectedItem == CompaxC3I20T11.PkwCommand.PkwAction.ChangeValue) | ((CompaxC3I20T11.PkwCommand.PkwAction)this.lbAccion.SelectedItem == CompaxC3I20T11.PkwCommand.PkwAction.ChangeValidateValue))
				{
					if (Information.IsNumeric(this.tbEscribir.Text))
					{
						btnStarPkwClicked = true;
                        this.pkwAccion = (CompaxC3I20T11.PkwCommand.PkwAction)this.lbAccion.SelectedItem;
                        this.pkwC3Obj = (CompaxC3I20T11.PkwCommand.PkwStructure.PkwC3Object.PkwObjectType) this.cbPkwObj.SelectedItem;
						HabilitarControlsPwk(false);
						pkwValor = double.Parse(this.tbEscribir.Text);
					}
				}
				else
				{
					btnStarPkwClicked = true;
                    this.pkwAccion = (CompaxC3I20T11.PkwCommand.PkwAction)this.lbAccion.SelectedItem;
                    this.pkwC3Obj = (CompaxC3I20T11.PkwCommand.PkwStructure.PkwC3Object.PkwObjectType)this.cbPkwObj.SelectedItem;
					HabilitarControlsPwk(false);
					if (Information.IsNumeric(this.tbEscribir.Text))
					{
						pkwValor = double.Parse(this.tbEscribir.Text);
					}
					else
					{
						pkwValor = 1;
					}
				}
			}
		}
	
		private void btStart_Click(object sender, System.EventArgs e)
		{
			if (rbAbs.Checked)
			{
				this.StartAbsClicked = true;
			}
            else if (rbRel.Checked) {
				this.StartRelClicked = true;
			}
            else if (rbAdd.Checked) {
				this.StartAddClicked = true;
			}
		}
		private void rbJog_CheckedChanged_1(object sender, System.EventArgs e)
		{
			btnPos.Enabled = rbJog.Checked;
			btnNeg.Enabled = rbJog.Checked;
			btStart.Enabled = !rbJog.Checked;
			btStop.Enabled = !rbJog.Checked;
		}
		private void btStop_Click(object sender, System.EventArgs e)
		{
            Parker.Parada();
		}

        private void cbParker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Parker = Parkers[(string)cbParker.SelectedItem];
        }

        private void frm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        #region IViewTaskOwner Members

        public System.Collections.Generic.IEnumerable<ViewTask> GetViewTasks()
        {
            return new System.Collections.Generic.List<ViewTask> { Run };
        }

        #endregion

	}
}
