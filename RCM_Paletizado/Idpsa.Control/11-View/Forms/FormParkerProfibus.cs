using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.Component;
using Idpsa.Control.Tool;
using Microsoft.VisualBasic;

namespace Idpsa.Control.View
{
	public class FormParkerProfibus : Form,IViewTasksOwner
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
            Parkers = new Dictionary<string, CompaxC3I20T11>();
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
                CTWCLab[i].Click += new EventHandler(CWC_Click);
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
		private IContainer components;
		//NOTA: el Diseñador de Windows Forms requiere el siguiente procedimiento
		//Puede modificarse utilizando el Diseñador de Windows Forms. 
		//No lo modifique con el editor de código.
		internal Panel Panel1;
		internal Panel Panel2;
		internal Label label1;
		internal Label Label2;
		internal Label Label9;
		internal Panel Panel3;
		internal Label CW15;
		internal Label CW0;
		internal Label CW1;
		internal Label CW2;
		internal Label CW3;
		internal Label CW4;
		internal Label CW5;
		internal Label CW6;
		internal Label CW7;
		internal Label CW8;
		internal Label CW9;
		internal Label CW10;
		internal Label CW11;
		internal Label CW12;
		internal Label CW13;
		internal Label CW14;
		internal Label SW0;
		internal Label SW1;
		internal Label SW2;
		internal Label SW3;
		internal Label SW4;
		internal Label SW5;
		internal Label SW6;
		internal Label SW7;
		internal Label SW8;
		internal Label SW9;
		internal Label SW10;
		internal Label SW11;
		internal Label SW12;
		internal Label SW13;
		internal Label SW14;
		internal Label SW15;
		internal Label CWC0;
		internal Label CWC1;
		internal Label CWC2;
		internal Label CWC3;
		internal Label CWC4;
		internal Label CWC5;
		internal Label CWC6;
		internal Label CWC7;
		internal Label CWC8;
		internal Label CWC9;
		internal Label CWC10;
		internal Label CWC11;
		internal Label CWC12;
		internal Label CWC13;
		internal Label CWC14;
		internal Label CWC15;
		internal GroupBox GroupBox3;
		internal GroupBox GroupBox4;
		internal GroupBox GroupBox5;
		internal GroupBox GroupBox6;
		internal Label lbLectura;
		internal Label Label15;
		internal TextBox tbEscribir;
		internal Label Label14;
		internal Button btnStarPkw;
		internal ListBox lbAccion;
        internal MainMenu MainMenu1;
		internal ListBox cbPkwObj;
		internal Label lbError;
		internal Label Label11;
		internal Button Button5;
		internal GroupBox GroupBox1;
		internal Button btnNeg;
		internal Button btnPos;
		internal RadioButton rbJog;
		internal RadioButton rbAdd;
		internal RadioButton rbRel;
		internal RadioButton rbAbs;
		internal Button btStop;
		internal Button btStart;
		internal Button btHoming;
		internal Button Button4;
		internal Button Button3;
		internal Button Button2;
		internal Button Button1;
		internal GroupBox GroupBox2;
		internal Label lbT;
		internal Label Label12;
		internal Label LAE;
		internal Label Label10;
		internal Label lbAP;
		internal Label Label8;
		internal Label lbEstado;
		internal Label Label13;
		internal Label lbAV;
		internal Label Label6;
		internal GroupBox GroupBox7;
		internal Label Label5;
		internal TextBox tbCV;
		internal Label Label4;
		internal TextBox tbCA;
		internal Label lab1;
		internal TextBox tbCP;
		[DebuggerStepThrough()]
		private void InitializeComponent()
		{
            components = new Container();
            CW15 = new Label();
            Panel1 = new Panel();
            CW0 = new Label();
            CW1 = new Label();
            CW2 = new Label();
            CW3 = new Label();
            CW4 = new Label();
            CW5 = new Label();
            CW6 = new Label();
            CW7 = new Label();
            CW8 = new Label();
            CW9 = new Label();
            CW10 = new Label();
            CW11 = new Label();
            CW12 = new Label();
            CW13 = new Label();
            CW14 = new Label();
            Panel2 = new Panel();
            SW0 = new Label();
            SW1 = new Label();
            SW2 = new Label();
            SW3 = new Label();
            SW4 = new Label();
            SW5 = new Label();
            SW6 = new Label();
            SW7 = new Label();
            SW8 = new Label();
            SW9 = new Label();
            SW10 = new Label();
            SW11 = new Label();
            SW12 = new Label();
            SW13 = new Label();
            SW14 = new Label();
            SW15 = new Label();
            label1 = new Label();
            Label2 = new Label();
            Label9 = new Label();
            Panel3 = new Panel();
            CWC0 = new Label();
            CWC1 = new Label();
            CWC2 = new Label();
            CWC3 = new Label();
            CWC4 = new Label();
            CWC5 = new Label();
            CWC6 = new Label();
            CWC7 = new Label();
            CWC8 = new Label();
            CWC9 = new Label();
            CWC10 = new Label();
            CWC11 = new Label();
            CWC12 = new Label();
            CWC13 = new Label();
            CWC14 = new Label();
            CWC15 = new Label();
            GroupBox3 = new GroupBox();
            btnStarPkw = new Button();
            GroupBox6 = new GroupBox();
            lbLectura = new Label();
            Label15 = new Label();
            tbEscribir = new TextBox();
            Label14 = new Label();
            GroupBox5 = new GroupBox();
            cbPkwObj = new ListBox();
            GroupBox4 = new GroupBox();
            lbAccion = new ListBox();
            MainMenu1 = new MainMenu(components);
            lbError = new Label();
            Label11 = new Label();
            Button5 = new Button();
            GroupBox1 = new GroupBox();
            btnNeg = new Button();
            btnPos = new Button();
            rbJog = new RadioButton();
            rbAdd = new RadioButton();
            rbRel = new RadioButton();
            rbAbs = new RadioButton();
            btStop = new Button();
            btStart = new Button();
            btHoming = new Button();
            Button4 = new Button();
            Button3 = new Button();
            Button2 = new Button();
            Button1 = new Button();
            GroupBox2 = new GroupBox();
            lbT = new Label();
            Label12 = new Label();
            LAE = new Label();
            Label10 = new Label();
            lbAP = new Label();
            Label8 = new Label();
            lbEstado = new Label();
            Label13 = new Label();
            lbAV = new Label();
            Label6 = new Label();
            GroupBox7 = new GroupBox();
            Label5 = new Label();
            tbCV = new TextBox();
            Label4 = new Label();
            tbCA = new TextBox();
            lab1 = new Label();
            tbCP = new TextBox();
            cbParker = new ComboBox();
            label3 = new Label();
            Panel1.SuspendLayout();
            Panel2.SuspendLayout();
            Panel3.SuspendLayout();
            GroupBox3.SuspendLayout();
            GroupBox6.SuspendLayout();
            GroupBox5.SuspendLayout();
            GroupBox4.SuspendLayout();
            GroupBox1.SuspendLayout();
            GroupBox2.SuspendLayout();
            GroupBox7.SuspendLayout();
            SuspendLayout();
            // 
            // CW15
            // 
            CW15.BackColor = Color.White;
            CW15.Dock = DockStyle.Left;
            CW15.Location = new Point(0, 0);
            CW15.Name = "CW15";
            CW15.Size = new Size(56, 40);
            CW15.TabIndex = 0;
            CW15.Text = "15";
            CW15.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Panel1
            // 
            Panel1.Controls.Add(CW0);
            Panel1.Controls.Add(CW1);
            Panel1.Controls.Add(CW2);
            Panel1.Controls.Add(CW3);
            Panel1.Controls.Add(CW4);
            Panel1.Controls.Add(CW5);
            Panel1.Controls.Add(CW6);
            Panel1.Controls.Add(CW7);
            Panel1.Controls.Add(CW8);
            Panel1.Controls.Add(CW9);
            Panel1.Controls.Add(CW10);
            Panel1.Controls.Add(CW11);
            Panel1.Controls.Add(CW12);
            Panel1.Controls.Add(CW13);
            Panel1.Controls.Add(CW14);
            Panel1.Controls.Add(CW15);
            Panel1.Location = new Point(16, 112);
            Panel1.Name = "Panel1";
            Panel1.Size = new Size(896, 40);
            Panel1.TabIndex = 1;
            // 
            // CW0
            // 
            CW0.BackColor = Color.White;
            CW0.Dock = DockStyle.Left;
            CW0.Location = new Point(840, 0);
            CW0.Name = "CW0";
            CW0.Size = new Size(56, 40);
            CW0.TabIndex = 15;
            CW0.Text = "0";
            CW0.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CW1
            // 
            CW1.BackColor = Color.White;
            CW1.Dock = DockStyle.Left;
            CW1.Location = new Point(784, 0);
            CW1.Name = "CW1";
            CW1.Size = new Size(56, 40);
            CW1.TabIndex = 14;
            CW1.Text = "1";
            CW1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CW2
            // 
            CW2.BackColor = Color.White;
            CW2.Dock = DockStyle.Left;
            CW2.Location = new Point(728, 0);
            CW2.Name = "CW2";
            CW2.Size = new Size(56, 40);
            CW2.TabIndex = 13;
            CW2.Text = "2";
            CW2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CW3
            // 
            CW3.BackColor = Color.White;
            CW3.Dock = DockStyle.Left;
            CW3.Location = new Point(672, 0);
            CW3.Name = "CW3";
            CW3.Size = new Size(56, 40);
            CW3.TabIndex = 12;
            CW3.Text = "3";
            CW3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CW4
            // 
            CW4.BackColor = Color.White;
            CW4.Dock = DockStyle.Left;
            CW4.Location = new Point(616, 0);
            CW4.Name = "CW4";
            CW4.Size = new Size(56, 40);
            CW4.TabIndex = 11;
            CW4.Text = "4";
            CW4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CW5
            // 
            CW5.BackColor = Color.White;
            CW5.Dock = DockStyle.Left;
            CW5.Location = new Point(560, 0);
            CW5.Name = "CW5";
            CW5.Size = new Size(56, 40);
            CW5.TabIndex = 10;
            CW5.Text = "5";
            CW5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CW6
            // 
            CW6.BackColor = Color.White;
            CW6.Dock = DockStyle.Left;
            CW6.Location = new Point(504, 0);
            CW6.Name = "CW6";
            CW6.Size = new Size(56, 40);
            CW6.TabIndex = 9;
            CW6.Text = "6";
            CW6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CW7
            // 
            CW7.BackColor = Color.White;
            CW7.Dock = DockStyle.Left;
            CW7.Location = new Point(448, 0);
            CW7.Name = "CW7";
            CW7.Size = new Size(56, 40);
            CW7.TabIndex = 8;
            CW7.Text = "7";
            CW7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CW8
            // 
            CW8.BackColor = Color.White;
            CW8.Dock = DockStyle.Left;
            CW8.Location = new Point(392, 0);
            CW8.Name = "CW8";
            CW8.Size = new Size(56, 40);
            CW8.TabIndex = 7;
            CW8.Text = "8";
            CW8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CW9
            // 
            CW9.BackColor = Color.White;
            CW9.Dock = DockStyle.Left;
            CW9.Location = new Point(336, 0);
            CW9.Name = "CW9";
            CW9.Size = new Size(56, 40);
            CW9.TabIndex = 6;
            CW9.Text = "9";
            CW9.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CW10
            // 
            CW10.BackColor = Color.White;
            CW10.Dock = DockStyle.Left;
            CW10.Location = new Point(280, 0);
            CW10.Name = "CW10";
            CW10.Size = new Size(56, 40);
            CW10.TabIndex = 5;
            CW10.Text = "10";
            CW10.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CW11
            // 
            CW11.BackColor = Color.White;
            CW11.Dock = DockStyle.Left;
            CW11.Location = new Point(224, 0);
            CW11.Name = "CW11";
            CW11.Size = new Size(56, 40);
            CW11.TabIndex = 4;
            CW11.Text = "11";
            CW11.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CW12
            // 
            CW12.BackColor = Color.White;
            CW12.Dock = DockStyle.Left;
            CW12.Location = new Point(168, 0);
            CW12.Name = "CW12";
            CW12.Size = new Size(56, 40);
            CW12.TabIndex = 3;
            CW12.Text = "12";
            CW12.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CW13
            // 
            CW13.BackColor = Color.White;
            CW13.Dock = DockStyle.Left;
            CW13.Location = new Point(112, 0);
            CW13.Name = "CW13";
            CW13.Size = new Size(56, 40);
            CW13.TabIndex = 2;
            CW13.Text = "13";
            CW13.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CW14
            // 
            CW14.BackColor = Color.White;
            CW14.Dock = DockStyle.Left;
            CW14.Location = new Point(56, 0);
            CW14.Name = "CW14";
            CW14.Size = new Size(56, 40);
            CW14.TabIndex = 1;
            CW14.Text = "14";
            CW14.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Panel2
            // 
            Panel2.Controls.Add(SW0);
            Panel2.Controls.Add(SW1);
            Panel2.Controls.Add(SW2);
            Panel2.Controls.Add(SW3);
            Panel2.Controls.Add(SW4);
            Panel2.Controls.Add(SW5);
            Panel2.Controls.Add(SW6);
            Panel2.Controls.Add(SW7);
            Panel2.Controls.Add(SW8);
            Panel2.Controls.Add(SW9);
            Panel2.Controls.Add(SW10);
            Panel2.Controls.Add(SW11);
            Panel2.Controls.Add(SW12);
            Panel2.Controls.Add(SW13);
            Panel2.Controls.Add(SW14);
            Panel2.Controls.Add(SW15);
            Panel2.Location = new Point(16, 32);
            Panel2.Name = "Panel2";
            Panel2.Size = new Size(896, 40);
            Panel2.TabIndex = 2;
            // 
            // SW0
            // 
            SW0.BackColor = Color.White;
            SW0.Dock = DockStyle.Left;
            SW0.Location = new Point(840, 0);
            SW0.Name = "SW0";
            SW0.Size = new Size(56, 40);
            SW0.TabIndex = 15;
            SW0.Text = "0";
            SW0.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SW1
            // 
            SW1.BackColor = Color.White;
            SW1.Dock = DockStyle.Left;
            SW1.Location = new Point(784, 0);
            SW1.Name = "SW1";
            SW1.Size = new Size(56, 40);
            SW1.TabIndex = 14;
            SW1.Text = "1";
            SW1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SW2
            // 
            SW2.BackColor = Color.White;
            SW2.Dock = DockStyle.Left;
            SW2.Location = new Point(728, 0);
            SW2.Name = "SW2";
            SW2.Size = new Size(56, 40);
            SW2.TabIndex = 13;
            SW2.Text = "2";
            SW2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SW3
            // 
            SW3.BackColor = Color.White;
            SW3.Dock = DockStyle.Left;
            SW3.Location = new Point(672, 0);
            SW3.Name = "SW3";
            SW3.Size = new Size(56, 40);
            SW3.TabIndex = 12;
            SW3.Text = "3";
            SW3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SW4
            // 
            SW4.BackColor = Color.White;
            SW4.Dock = DockStyle.Left;
            SW4.Location = new Point(616, 0);
            SW4.Name = "SW4";
            SW4.Size = new Size(56, 40);
            SW4.TabIndex = 11;
            SW4.Text = "4";
            SW4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SW5
            // 
            SW5.BackColor = Color.White;
            SW5.Dock = DockStyle.Left;
            SW5.Location = new Point(560, 0);
            SW5.Name = "SW5";
            SW5.Size = new Size(56, 40);
            SW5.TabIndex = 10;
            SW5.Text = "5";
            SW5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SW6
            // 
            SW6.BackColor = Color.White;
            SW6.Dock = DockStyle.Left;
            SW6.Location = new Point(504, 0);
            SW6.Name = "SW6";
            SW6.Size = new Size(56, 40);
            SW6.TabIndex = 9;
            SW6.Text = "6";
            SW6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SW7
            // 
            SW7.BackColor = Color.White;
            SW7.Dock = DockStyle.Left;
            SW7.Location = new Point(448, 0);
            SW7.Name = "SW7";
            SW7.Size = new Size(56, 40);
            SW7.TabIndex = 8;
            SW7.Text = "7";
            SW7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SW8
            // 
            SW8.BackColor = Color.White;
            SW8.Dock = DockStyle.Left;
            SW8.Location = new Point(392, 0);
            SW8.Name = "SW8";
            SW8.Size = new Size(56, 40);
            SW8.TabIndex = 7;
            SW8.Text = "8";
            SW8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SW9
            // 
            SW9.BackColor = Color.White;
            SW9.Dock = DockStyle.Left;
            SW9.Location = new Point(336, 0);
            SW9.Name = "SW9";
            SW9.Size = new Size(56, 40);
            SW9.TabIndex = 6;
            SW9.Text = "9";
            SW9.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SW10
            // 
            SW10.BackColor = Color.White;
            SW10.Dock = DockStyle.Left;
            SW10.Location = new Point(280, 0);
            SW10.Name = "SW10";
            SW10.Size = new Size(56, 40);
            SW10.TabIndex = 5;
            SW10.Text = "10";
            SW10.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SW11
            // 
            SW11.BackColor = Color.White;
            SW11.Dock = DockStyle.Left;
            SW11.Location = new Point(224, 0);
            SW11.Name = "SW11";
            SW11.Size = new Size(56, 40);
            SW11.TabIndex = 4;
            SW11.Text = "11";
            SW11.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SW12
            // 
            SW12.BackColor = Color.White;
            SW12.Dock = DockStyle.Left;
            SW12.Location = new Point(168, 0);
            SW12.Name = "SW12";
            SW12.Size = new Size(56, 40);
            SW12.TabIndex = 3;
            SW12.Text = "12";
            SW12.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SW13
            // 
            SW13.BackColor = Color.White;
            SW13.Dock = DockStyle.Left;
            SW13.Location = new Point(112, 0);
            SW13.Name = "SW13";
            SW13.Size = new Size(56, 40);
            SW13.TabIndex = 2;
            SW13.Text = "13";
            SW13.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SW14
            // 
            SW14.BackColor = Color.White;
            SW14.Dock = DockStyle.Left;
            SW14.Location = new Point(56, 0);
            SW14.Name = "SW14";
            SW14.Size = new Size(56, 40);
            SW14.TabIndex = 1;
            SW14.Text = "14";
            SW14.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SW15
            // 
            SW15.BackColor = Color.White;
            SW15.Dock = DockStyle.Left;
            SW15.Location = new Point(0, 0);
            SW15.Name = "SW15";
            SW15.Size = new Size(56, 40);
            SW15.TabIndex = 0;
            SW15.Text = "15";
            SW15.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            label1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            label1.Location = new Point(416, 88);
            label1.Name = "label1";
            label1.Size = new Size(136, 16);
            label1.TabIndex = 3;
            label1.Text = "Control Word";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Label2
            // 
            Label2.BackColor = Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            Label2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            Label2.Location = new Point(416, 8);
            Label2.Name = "Label2";
            Label2.Size = new Size(136, 16);
            Label2.TabIndex = 4;
            Label2.Text = "Status Word";
            Label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Label9
            // 
            Label9.BackColor = Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            Label9.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            Label9.Location = new Point(416, 168);
            Label9.Name = "Label9";
            Label9.Size = new Size(136, 16);
            Label9.TabIndex = 21;
            Label9.Text = "Control Word candidata";
            Label9.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Panel3
            // 
            Panel3.Controls.Add(CWC0);
            Panel3.Controls.Add(CWC1);
            Panel3.Controls.Add(CWC2);
            Panel3.Controls.Add(CWC3);
            Panel3.Controls.Add(CWC4);
            Panel3.Controls.Add(CWC5);
            Panel3.Controls.Add(CWC6);
            Panel3.Controls.Add(CWC7);
            Panel3.Controls.Add(CWC8);
            Panel3.Controls.Add(CWC9);
            Panel3.Controls.Add(CWC10);
            Panel3.Controls.Add(CWC11);
            Panel3.Controls.Add(CWC12);
            Panel3.Controls.Add(CWC13);
            Panel3.Controls.Add(CWC14);
            Panel3.Controls.Add(CWC15);
            Panel3.Location = new Point(16, 192);
            Panel3.Name = "Panel3";
            Panel3.Size = new Size(896, 40);
            Panel3.TabIndex = 20;
            // 
            // CWC0
            // 
            CWC0.BackColor = Color.White;
            CWC0.Dock = DockStyle.Left;
            CWC0.Location = new Point(840, 0);
            CWC0.Name = "CWC0";
            CWC0.Size = new Size(56, 40);
            CWC0.TabIndex = 15;
            CWC0.Text = "0";
            CWC0.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CWC1
            // 
            CWC1.BackColor = Color.White;
            CWC1.Dock = DockStyle.Left;
            CWC1.Location = new Point(784, 0);
            CWC1.Name = "CWC1";
            CWC1.Size = new Size(56, 40);
            CWC1.TabIndex = 14;
            CWC1.Text = "1";
            CWC1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CWC2
            // 
            CWC2.BackColor = Color.White;
            CWC2.Dock = DockStyle.Left;
            CWC2.Location = new Point(728, 0);
            CWC2.Name = "CWC2";
            CWC2.Size = new Size(56, 40);
            CWC2.TabIndex = 13;
            CWC2.Text = "2";
            CWC2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CWC3
            // 
            CWC3.BackColor = Color.White;
            CWC3.Dock = DockStyle.Left;
            CWC3.Location = new Point(672, 0);
            CWC3.Name = "CWC3";
            CWC3.Size = new Size(56, 40);
            CWC3.TabIndex = 12;
            CWC3.Text = "3";
            CWC3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CWC4
            // 
            CWC4.BackColor = Color.White;
            CWC4.Dock = DockStyle.Left;
            CWC4.Location = new Point(616, 0);
            CWC4.Name = "CWC4";
            CWC4.Size = new Size(56, 40);
            CWC4.TabIndex = 11;
            CWC4.Text = "4";
            CWC4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CWC5
            // 
            CWC5.BackColor = Color.White;
            CWC5.Dock = DockStyle.Left;
            CWC5.Location = new Point(560, 0);
            CWC5.Name = "CWC5";
            CWC5.Size = new Size(56, 40);
            CWC5.TabIndex = 10;
            CWC5.Text = "5";
            CWC5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CWC6
            // 
            CWC6.BackColor = Color.White;
            CWC6.Dock = DockStyle.Left;
            CWC6.Location = new Point(504, 0);
            CWC6.Name = "CWC6";
            CWC6.Size = new Size(56, 40);
            CWC6.TabIndex = 9;
            CWC6.Text = "6";
            CWC6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CWC7
            // 
            CWC7.BackColor = Color.White;
            CWC7.Dock = DockStyle.Left;
            CWC7.Location = new Point(448, 0);
            CWC7.Name = "CWC7";
            CWC7.Size = new Size(56, 40);
            CWC7.TabIndex = 8;
            CWC7.Text = "7";
            CWC7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CWC8
            // 
            CWC8.BackColor = Color.White;
            CWC8.Dock = DockStyle.Left;
            CWC8.Location = new Point(392, 0);
            CWC8.Name = "CWC8";
            CWC8.Size = new Size(56, 40);
            CWC8.TabIndex = 7;
            CWC8.Text = "8";
            CWC8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CWC9
            // 
            CWC9.BackColor = Color.White;
            CWC9.Dock = DockStyle.Left;
            CWC9.Location = new Point(336, 0);
            CWC9.Name = "CWC9";
            CWC9.Size = new Size(56, 40);
            CWC9.TabIndex = 6;
            CWC9.Text = "9";
            CWC9.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CWC10
            // 
            CWC10.BackColor = Color.White;
            CWC10.Dock = DockStyle.Left;
            CWC10.Location = new Point(280, 0);
            CWC10.Name = "CWC10";
            CWC10.Size = new Size(56, 40);
            CWC10.TabIndex = 5;
            CWC10.Text = "10";
            CWC10.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CWC11
            // 
            CWC11.BackColor = Color.White;
            CWC11.Dock = DockStyle.Left;
            CWC11.Location = new Point(224, 0);
            CWC11.Name = "CWC11";
            CWC11.Size = new Size(56, 40);
            CWC11.TabIndex = 4;
            CWC11.Text = "11";
            CWC11.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CWC12
            // 
            CWC12.BackColor = Color.White;
            CWC12.Dock = DockStyle.Left;
            CWC12.Location = new Point(168, 0);
            CWC12.Name = "CWC12";
            CWC12.Size = new Size(56, 40);
            CWC12.TabIndex = 3;
            CWC12.Text = "12";
            CWC12.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CWC13
            // 
            CWC13.BackColor = Color.White;
            CWC13.Dock = DockStyle.Left;
            CWC13.Location = new Point(112, 0);
            CWC13.Name = "CWC13";
            CWC13.Size = new Size(56, 40);
            CWC13.TabIndex = 2;
            CWC13.Text = "13";
            CWC13.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CWC14
            // 
            CWC14.BackColor = Color.White;
            CWC14.Dock = DockStyle.Left;
            CWC14.Location = new Point(56, 0);
            CWC14.Name = "CWC14";
            CWC14.Size = new Size(56, 40);
            CWC14.TabIndex = 1;
            CWC14.Text = "14";
            CWC14.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CWC15
            // 
            CWC15.BackColor = Color.White;
            CWC15.Dock = DockStyle.Left;
            CWC15.Location = new Point(0, 0);
            CWC15.Name = "CWC15";
            CWC15.Size = new Size(56, 40);
            CWC15.TabIndex = 0;
            CWC15.Text = "15";
            CWC15.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // GroupBox3
            // 
            GroupBox3.Controls.Add(btnStarPkw);
            GroupBox3.Controls.Add(GroupBox6);
            GroupBox3.Controls.Add(GroupBox5);
            GroupBox3.Controls.Add(GroupBox4);
            GroupBox3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            GroupBox3.Location = new Point(8, 535);
            GroupBox3.Name = "GroupBox3";
            GroupBox3.Size = new Size(912, 200);
            GroupBox3.TabIndex = 26;
            GroupBox3.TabStop = false;
            GroupBox3.Text = "Pkw :";
            // 
            // btnStarPkw
            // 
            btnStarPkw.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            btnStarPkw.Location = new Point(808, 156);
            btnStarPkw.Name = "btnStarPkw";
            btnStarPkw.Size = new Size(88, 32);
            btnStarPkw.TabIndex = 6;
            btnStarPkw.Text = "Start Pkw";
            btnStarPkw.Click += new EventHandler(btnStarPkw_Click);
            // 
            // GroupBox6
            // 
            GroupBox6.Controls.Add(lbLectura);
            GroupBox6.Controls.Add(Label15);
            GroupBox6.Controls.Add(tbEscribir);
            GroupBox6.Controls.Add(Label14);
            GroupBox6.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            GroupBox6.Location = new Point(594, 32);
            GroupBox6.Name = "GroupBox6";
            GroupBox6.Size = new Size(248, 112);
            GroupBox6.TabIndex = 5;
            GroupBox6.TabStop = false;
            GroupBox6.Text = "Valores R/W objeto C3 :";
            // 
            // lbLectura
            // 
            lbLectura.BackColor = Color.Black;
            lbLectura.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            lbLectura.ForeColor = Color.White;
            lbLectura.Location = new Point(88, 32);
            lbLectura.Name = "lbLectura";
            lbLectura.Size = new Size(144, 24);
            lbLectura.TabIndex = 13;
            lbLectura.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Label15
            // 
            Label15.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            Label15.Location = new Point(16, 72);
            Label15.Name = "Label15";
            Label15.Size = new Size(64, 24);
            Label15.TabIndex = 12;
            Label15.Text = "Escribir :";
            // 
            // tbEscribir
            // 
            tbEscribir.Location = new Point(88, 72);
            tbEscribir.Name = "tbEscribir";
            tbEscribir.Size = new Size(144, 20);
            tbEscribir.TabIndex = 11;
            // 
            // Label14
            // 
            Label14.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            Label14.Location = new Point(16, 32);
            Label14.Name = "Label14";
            Label14.Size = new Size(64, 24);
            Label14.TabIndex = 10;
            Label14.Text = "Lectura :";
            // 
            // GroupBox5
            // 
            GroupBox5.Controls.Add(cbPkwObj);
            GroupBox5.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            GroupBox5.Location = new Point(312, 14);
            GroupBox5.Name = "GroupBox5";
            GroupBox5.Size = new Size(216, 176);
            GroupBox5.TabIndex = 4;
            GroupBox5.TabStop = false;
            GroupBox5.Text = "Seleccion de Objecto C3 :";
            // 
            // cbPkwObj
            // 
            cbPkwObj.Dock = DockStyle.Fill;
            cbPkwObj.Location = new Point(3, 16);
            cbPkwObj.Name = "cbPkwObj";
            cbPkwObj.Size = new Size(210, 147);
            cbPkwObj.TabIndex = 31;
            // 
            // GroupBox4
            // 
            GroupBox4.Controls.Add(lbAccion);
            GroupBox4.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            GroupBox4.Location = new Point(72, 32);
            GroupBox4.Name = "GroupBox4";
            GroupBox4.Size = new Size(168, 136);
            GroupBox4.TabIndex = 2;
            GroupBox4.TabStop = false;
            GroupBox4.Text = "Accion ";
            // 
            // lbAccion
            // 
            lbAccion.Dock = DockStyle.Fill;
            lbAccion.Location = new Point(3, 16);
            lbAccion.Name = "lbAccion";
            lbAccion.Size = new Size(162, 108);
            lbAccion.TabIndex = 0;
            // 
            // lbError
            // 
            lbError.BackColor = Color.IndianRed;
            lbError.ForeColor = Color.White;
            lbError.Location = new Point(112, 285);
            lbError.Name = "lbError";
            lbError.Size = new Size(456, 16);
            lbError.TabIndex = 24;
            lbError.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Label11
            // 
            Label11.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            Label11.Location = new Point(32, 285);
            Label11.Name = "Label11";
            Label11.Size = new Size(72, 16);
            Label11.TabIndex = 23;
            Label11.Text = "Ultimo error :";
            // 
            // Button5
            // 
            Button5.Location = new Point(760, 238);
            Button5.Name = "Button5";
            Button5.Size = new Size(152, 24);
            Button5.TabIndex = 22;
            Button5.Text = "Set CW candidata";
            Button5.Click += new EventHandler(Button5_Click);
            // 
            // GroupBox1
            // 
            GroupBox1.Controls.Add(btnNeg);
            GroupBox1.Controls.Add(btnPos);
            GroupBox1.Controls.Add(rbJog);
            GroupBox1.Controls.Add(rbAdd);
            GroupBox1.Controls.Add(rbRel);
            GroupBox1.Controls.Add(rbAbs);
            GroupBox1.Controls.Add(btStop);
            GroupBox1.Controls.Add(btStart);
            GroupBox1.Controls.Add(btHoming);
            GroupBox1.Controls.Add(Button4);
            GroupBox1.Controls.Add(Button3);
            GroupBox1.Controls.Add(Button2);
            GroupBox1.Controls.Add(Button1);
            GroupBox1.Controls.Add(GroupBox2);
            GroupBox1.Controls.Add(GroupBox7);
            GroupBox1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            GroupBox1.Location = new Point(8, 317);
            GroupBox1.Name = "GroupBox1";
            GroupBox1.Size = new Size(912, 216);
            GroupBox1.TabIndex = 27;
            GroupBox1.TabStop = false;
            GroupBox1.Text = "Posicionamiento directo :";
            // 
            // btnNeg
            // 
            btnNeg.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            btnNeg.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            btnNeg.Location = new Point(718, 62);
            btnNeg.Name = "btnNeg";
            btnNeg.Size = new Size(75, 23);
            btnNeg.TabIndex = 56;
            btnNeg.Text = "<";
            btnNeg.UseVisualStyleBackColor = false;
            btnNeg.MouseDown += new MouseEventHandler(btnNeg_MouseDown);
            btnNeg.MouseUp += new MouseEventHandler(btnNeg_MouseUp);
            // 
            // btnPos
            // 
            btnPos.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            btnPos.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            btnPos.Location = new Point(828, 62);
            btnPos.Name = "btnPos";
            btnPos.Size = new Size(75, 23);
            btnPos.TabIndex = 55;
            btnPos.Text = ">";
            btnPos.UseVisualStyleBackColor = false;
            btnPos.MouseDown += new MouseEventHandler(btnPos_MouseDown);
            btnPos.MouseUp += new MouseEventHandler(btnPos_MouseUp);
            // 
            // rbJog
            // 
            rbJog.Checked = true;
            rbJog.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            rbJog.Location = new Point(790, 36);
            rbJog.Name = "rbJog";
            rbJog.Size = new Size(56, 16);
            rbJog.TabIndex = 54;
            rbJog.TabStop = true;
            rbJog.Text = "Jog";
            rbJog.CheckedChanged += new EventHandler(rbJog_CheckedChanged_1);
            // 
            // rbAdd
            // 
            rbAdd.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            rbAdd.Location = new Point(848, 120);
            rbAdd.Name = "rbAdd";
            rbAdd.Size = new Size(56, 16);
            rbAdd.TabIndex = 53;
            rbAdd.Text = "Add";
            // 
            // rbRel
            // 
            rbRel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            rbRel.Location = new Point(784, 120);
            rbRel.Name = "rbRel";
            rbRel.Size = new Size(40, 16);
            rbRel.TabIndex = 52;
            rbRel.Text = "Rel";
            // 
            // rbAbs
            // 
            rbAbs.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            rbAbs.Location = new Point(720, 120);
            rbAbs.Name = "rbAbs";
            rbAbs.Size = new Size(44, 16);
            rbAbs.TabIndex = 51;
            rbAbs.Text = "Abs";
            // 
            // btStop
            // 
            btStop.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            btStop.Location = new Point(762, 178);
            btStop.Name = "btStop";
            btStop.Size = new Size(86, 23);
            btStop.TabIndex = 50;
            btStop.Text = "Stop";
            btStop.Click += new EventHandler(btStop_Click);
            // 
            // btStart
            // 
            btStart.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            btStart.Location = new Point(762, 146);
            btStart.Name = "btStart";
            btStart.Size = new Size(86, 24);
            btStart.TabIndex = 49;
            btStart.Text = "Start";
            btStart.Click += new EventHandler(btStart_Click);
            // 
            // btHoming
            // 
            btHoming.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            btHoming.Location = new Point(568, 178);
            btHoming.Name = "btHoming";
            btHoming.Size = new Size(112, 24);
            btHoming.TabIndex = 48;
            btHoming.Text = "Homing";
            btHoming.Click += new EventHandler(btHoming_Click);
            // 
            // Button4
            // 
            Button4.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            Button4.Location = new Point(568, 130);
            Button4.Name = "Button4";
            Button4.Size = new Size(112, 23);
            Button4.TabIndex = 47;
            Button4.Text = "Quitar Error";
            Button4.Click += new EventHandler(Button4_Click);
            // 
            // Button3
            // 
            Button3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            Button3.Location = new Point(568, 90);
            Button3.Name = "Button3";
            Button3.Size = new Size(112, 23);
            Button3.TabIndex = 46;
            Button3.Text = "Habilitar Operacion";
            Button3.Click += new EventHandler(Button3_Click);
            // 
            // Button2
            // 
            Button2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            Button2.Location = new Point(568, 58);
            Button2.Name = "Button2";
            Button2.Size = new Size(112, 23);
            Button2.TabIndex = 45;
            Button2.Text = "Habilitar Driver";
            Button2.Click += new EventHandler(Button2_Click);
            // 
            // Button1
            // 
            Button1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            Button1.Location = new Point(568, 26);
            Button1.Name = "Button1";
            Button1.Size = new Size(112, 23);
            Button1.TabIndex = 44;
            Button1.Text = "Energizar";
            Button1.Click += new EventHandler(Button1_Click);
            // 
            // GroupBox2
            // 
            GroupBox2.Controls.Add(lbT);
            GroupBox2.Controls.Add(Label12);
            GroupBox2.Controls.Add(LAE);
            GroupBox2.Controls.Add(Label10);
            GroupBox2.Controls.Add(lbAP);
            GroupBox2.Controls.Add(Label8);
            GroupBox2.Controls.Add(lbEstado);
            GroupBox2.Controls.Add(Label13);
            GroupBox2.Controls.Add(lbAV);
            GroupBox2.Controls.Add(Label6);
            GroupBox2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            GroupBox2.Location = new Point(280, 16);
            GroupBox2.Name = "GroupBox2";
            GroupBox2.Size = new Size(248, 192);
            GroupBox2.TabIndex = 43;
            GroupBox2.TabStop = false;
            GroupBox2.Text = "Valores actuales :";
            // 
            // lbT
            // 
            lbT.BackColor = SystemColors.ControlText;
            lbT.ForeColor = Color.White;
            lbT.Location = new Point(104, 168);
            lbT.Name = "lbT";
            lbT.Size = new Size(96, 16);
            lbT.TabIndex = 16;
            // 
            // Label12
            // 
            Label12.Location = new Point(24, 168);
            Label12.Name = "Label12";
            Label12.Size = new Size(72, 16);
            Label12.TabIndex = 15;
            Label12.Text = "Torque :";
            // 
            // LAE
            // 
            LAE.BackColor = SystemColors.ControlText;
            LAE.ForeColor = Color.White;
            LAE.Location = new Point(104, 136);
            LAE.Name = "LAE";
            LAE.Size = new Size(96, 16);
            LAE.TabIndex = 14;
            // 
            // Label10
            // 
            Label10.Location = new Point(24, 136);
            Label10.Name = "Label10";
            Label10.Size = new Size(72, 16);
            Label10.TabIndex = 13;
            Label10.Text = "Cod. Error :";
            // 
            // lbAP
            // 
            lbAP.BackColor = Color.Black;
            lbAP.ForeColor = Color.White;
            lbAP.Location = new Point(104, 56);
            lbAP.Name = "lbAP";
            lbAP.Size = new Size(96, 16);
            lbAP.TabIndex = 12;
            // 
            // Label8
            // 
            Label8.Location = new Point(24, 56);
            Label8.Name = "Label8";
            Label8.Size = new Size(72, 16);
            Label8.TabIndex = 11;
            Label8.Text = "Posicion";
            // 
            // lbEstado
            // 
            lbEstado.BackColor = Color.Navy;
            lbEstado.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            lbEstado.ForeColor = Color.White;
            lbEstado.Location = new Point(88, 24);
            lbEstado.Name = "lbEstado";
            lbEstado.Size = new Size(144, 16);
            lbEstado.TabIndex = 10;
            lbEstado.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Label13
            // 
            Label13.Location = new Point(24, 24);
            Label13.Name = "Label13";
            Label13.Size = new Size(48, 16);
            Label13.TabIndex = 9;
            Label13.Text = "Estado :";
            // 
            // lbAV
            // 
            lbAV.BackColor = SystemColors.ControlText;
            lbAV.ForeColor = Color.White;
            lbAV.Location = new Point(104, 96);
            lbAV.Name = "lbAV";
            lbAV.Size = new Size(96, 16);
            lbAV.TabIndex = 7;
            // 
            // Label6
            // 
            Label6.Location = new Point(24, 96);
            Label6.Name = "Label6";
            Label6.Size = new Size(72, 16);
            Label6.TabIndex = 5;
            Label6.Text = "Velcidad :";
            // 
            // GroupBox7
            // 
            GroupBox7.Controls.Add(Label5);
            GroupBox7.Controls.Add(tbCV);
            GroupBox7.Controls.Add(Label4);
            GroupBox7.Controls.Add(tbCA);
            GroupBox7.Controls.Add(lab1);
            GroupBox7.Controls.Add(tbCP);
            GroupBox7.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            GroupBox7.Location = new Point(8, 16);
            GroupBox7.Name = "GroupBox7";
            GroupBox7.Size = new Size(248, 192);
            GroupBox7.TabIndex = 42;
            GroupBox7.TabStop = false;
            GroupBox7.Text = "Valores consigna :";
            // 
            // Label5
            // 
            Label5.Location = new Point(32, 88);
            Label5.Name = "Label5";
            Label5.Size = new Size(72, 16);
            Label5.TabIndex = 5;
            Label5.Text = "Velocidad";
            // 
            // tbCV
            // 
            tbCV.Location = new Point(112, 88);
            tbCV.Name = "tbCV";
            tbCV.Size = new Size(100, 20);
            tbCV.TabIndex = 4;
            // 
            // Label4
            // 
            Label4.Location = new Point(32, 144);
            Label4.Name = "Label4";
            Label4.Size = new Size(72, 16);
            Label4.TabIndex = 3;
            Label4.Text = "Aceleracion :";
            // 
            // tbCA
            // 
            tbCA.Location = new Point(112, 144);
            tbCA.Name = "tbCA";
            tbCA.Size = new Size(100, 20);
            tbCA.TabIndex = 2;
            // 
            // lab1
            // 
            lab1.Location = new Point(32, 32);
            lab1.Name = "lab1";
            lab1.Size = new Size(72, 16);
            lab1.TabIndex = 1;
            lab1.Text = "Posicion :";
            // 
            // tbCP
            // 
            tbCP.Location = new Point(112, 32);
            tbCP.Name = "tbCP";
            tbCP.Size = new Size(100, 20);
            tbCP.TabIndex = 0;
            // 
            // cbParker
            // 
            cbParker.FormattingEnabled = true;
            cbParker.Location = new Point(113, 248);
            cbParker.Name = "cbParker";
            cbParker.Size = new Size(158, 21);
            cbParker.TabIndex = 28;
            cbParker.SelectedIndexChanged += new EventHandler(cbParker_SelectedIndexChanged);
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(60, 251);
            label3.Name = "label3";
            label3.Size = new Size(44, 13);
            label3.TabIndex = 29;
            label3.Text = "Parker :";
            // 
            // frmParkerProfibus
            // 
            AutoScaleBaseSize = new Size(5, 13);
            ClientSize = new Size(928, 743);
            Controls.Add(label3);
            Controls.Add(cbParker);
            Controls.Add(GroupBox1);
            Controls.Add(GroupBox3);
            Controls.Add(lbError);
            Controls.Add(Label11);
            Controls.Add(Button5);
            Controls.Add(Label9);
            Controls.Add(Panel3);
            Controls.Add(Label2);
            Controls.Add(label1);
            Controls.Add(Panel2);
            Controls.Add(Panel1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Menu = MainMenu1;
            Name = "frmParkerProfibus";
            Text = "Control I20T11 (Profibus)";
            TopMost = true;
            Load += new EventHandler(ParkerProfibus_Load);
            Panel1.ResumeLayout(false);
            Panel2.ResumeLayout(false);
            Panel3.ResumeLayout(false);
            GroupBox3.ResumeLayout(false);
            GroupBox6.ResumeLayout(false);
            GroupBox6.PerformLayout();
            GroupBox5.ResumeLayout(false);
            GroupBox4.ResumeLayout(false);
            GroupBox1.ResumeLayout(false);
            GroupBox2.ResumeLayout(false);
            GroupBox7.ResumeLayout(false);
            GroupBox7.PerformLayout();
            Closing += frm_Closing;
            ResumeLayout(false);
            PerformLayout();

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
			if (btHomingClicked)
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
					STWLab[i].BackColor = Color.Green;
				}
				else
				{
					STWLab[i].BackColor = Color.Yellow;
				}
			}
			for (int i = 0; i <= stwV.Length - 1; i++) {
				if (ctwV[i] == true)
				{
					CTWLab[i].BackColor = Color.Green;
				}
				else
				{
					CTWLab[i].BackColor = Color.Yellow;
				}
			}
			lbEstado.Text = Parker.CurrentState();
			lbAP.Text = Parker.Posicion.ToString();
			lbAV.Text = Parker.Velocidad.ToString();
			LAE.Text = Parker.LastCodeError.ToString();
			lbT.Text = Parker.Torque.ToString();
			lbError.Text = Parker.LastError();
		}
		
		private void CWC_Click(object sender, EventArgs e)
		{
			var l = (Label)sender;
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
		private void Button1_Click(object sender, EventArgs e)
		{
			Parker.Energizar();
		}
		private void Button2_Click(object sender, EventArgs e)
		{
			Parker.HabilitarDriver();
		}
		private void Button3_Click(object sender, EventArgs e)
		{
			Parker.HabilitarOperacion();
		}
		private void Button4_Click(object sender, EventArgs e)
		{			
			QuitErrorClicked = true;
		}
		private void btnNeg_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Parker.Jog2 = true;
				Parker.Jog1 = false;
				btnNeg.BackColor = Color.FromArgb(128, 255, 128);
			}
		}
		private void btnNeg_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Parker.Jog1 = false;
				Parker.Jog2 = false;
				btnNeg.BackColor = Color.FromArgb(255, 255, 128);
			}
		}
		private void btnPos_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Parker.Jog1 = true;
				Parker.Jog2 = false;
				btnPos.BackColor = Color.FromArgb(128, 255, 128);
			}
		}
		private void btnPos_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Parker.Jog1 = false;
				Parker.Jog2 = false;
				btnPos.BackColor = Color.FromArgb(255, 255, 128);
			}
		}
		private void Button5_Click(object sender, EventArgs e)
		{
			Parker.WriteControlWord(CTWCValues);
		}
		private void btHoming_Click(object sender, EventArgs e)
		{
			btHomingClicked = true;
		}
		private void ParkerProfibus_Load(object sender, EventArgs e)
		{
		   
            foreach (string str in Parkers.Keys)
                cbParker.Items.Add(str);

		    cbParker.SelectedIndex = 0;
		    Parker = Parkers[(string) cbParker.SelectedItem];
            var Acciones = (CompaxC3I20T11.PkwCommand.PkwAction[])Enum.GetValues(typeof(CompaxC3I20T11.PkwCommand.PkwAction));
            var C3Ob = (CompaxC3I20T11.PkwCommand.PkwStructure.PkwC3Object.PkwObjectType[])Enum.GetValues(typeof(CompaxC3I20T11.PkwCommand.PkwStructure.PkwC3Object.PkwObjectType));
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
			cbPkwObj.Enabled = Enable;
			lbAccion.Enabled = Enable;
			btnStarPkw.Enabled = Enable;
		}
		private void btnStarPkw_Click(object sender, EventArgs e)
		{
			if (!(lbAccion.SelectedItem == null) && !(cbPkwObj.SelectedItem == null))
			{
				if (((CompaxC3I20T11.PkwCommand.PkwAction)lbAccion.SelectedItem == CompaxC3I20T11.PkwCommand.PkwAction.ChangeValue) | ((CompaxC3I20T11.PkwCommand.PkwAction)lbAccion.SelectedItem == CompaxC3I20T11.PkwCommand.PkwAction.ChangeValidateValue))
				{
					if (Information.IsNumeric(tbEscribir.Text))
					{
						btnStarPkwClicked = true;
                        pkwAccion = (CompaxC3I20T11.PkwCommand.PkwAction)lbAccion.SelectedItem;
                        pkwC3Obj = (CompaxC3I20T11.PkwCommand.PkwStructure.PkwC3Object.PkwObjectType) cbPkwObj.SelectedItem;
						HabilitarControlsPwk(false);
						pkwValor = double.Parse(tbEscribir.Text);
					}
				}
				else
				{
					btnStarPkwClicked = true;
                    pkwAccion = (CompaxC3I20T11.PkwCommand.PkwAction)lbAccion.SelectedItem;
                    pkwC3Obj = (CompaxC3I20T11.PkwCommand.PkwStructure.PkwC3Object.PkwObjectType)cbPkwObj.SelectedItem;
					HabilitarControlsPwk(false);
					if (Information.IsNumeric(tbEscribir.Text))
					{
						pkwValor = double.Parse(tbEscribir.Text);
					}
					else
					{
						pkwValor = 1;
					}
				}
			}
		}
	
		private void btStart_Click(object sender, EventArgs e)
		{
			if (rbAbs.Checked)
			{
				StartAbsClicked = true;
			}
            else if (rbRel.Checked) {
				StartRelClicked = true;
			}
            else if (rbAdd.Checked) {
				StartAddClicked = true;
			}
		}
		private void rbJog_CheckedChanged_1(object sender, EventArgs e)
		{
			btnPos.Enabled = rbJog.Checked;
			btnNeg.Enabled = rbJog.Checked;
			btStart.Enabled = !rbJog.Checked;
			btStop.Enabled = !rbJog.Checked;
		}
		private void btStop_Click(object sender, EventArgs e)
		{
            Parker.Parada();
		}

        private void cbParker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Parker = Parkers[(string)cbParker.SelectedItem];
        }

        private void frm_Closing(object sender, CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        #region IViewTaskOwner Members

        public IEnumerable<Action> GetViewTasks()
        {
            return new List<Action> { Run };
        }

        #endregion

	}
}
