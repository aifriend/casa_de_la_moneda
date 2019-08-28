using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Idpsa.Control.Engine;
using Idpsa.Control.Sequence;

namespace Idpsa.Control.View
{
	public class FormChains : Form,IViewTasksOwner
	{		
		private TON _timer1 = new TON();
        private TON _timer2 = new TON();
        private TON _timer3 = new TON();
        private TON _timer4 = new TON();
        internal Panel panel1;
        internal ListView listView1;
        internal ColumnHeader columnHeader6;
        internal ColumnHeader columnHeader7;
        internal ColumnHeader columnHeader8;
        internal Panel panel4;
        internal ListView listView2;
        internal ColumnHeader columnHeader9;
        internal ColumnHeader columnHeader10;
        internal ColumnHeader columnHeader11;
        internal ListView listView3;
        internal ColumnHeader columnHeader12;
        internal ColumnHeader columnHeader13;
        internal ColumnHeader columnHeader14;
        internal Panel panel5;
        internal ListView listView4;
        internal ColumnHeader columnHeader15;
        internal ColumnHeader columnHeader16;
        internal ColumnHeader columnHeader17;
        internal Panel panel6;
        internal Panel panel8;
        internal ListView listViewLibres;
        internal ColumnHeader columnHeader21;
        internal ColumnHeader columnHeader22;
        internal ColumnHeader columnHeader23;
        internal Panel panel7;
        internal ListView listView5;
        internal ColumnHeader columnHeader18;
        internal ColumnHeader columnHeader19;
        internal ColumnHeader columnHeader20;
        internal TabPage tabPage7;
        internal TabPage tabPage4;
        internal Panel panel9;
        internal ListView listView6;
        internal ColumnHeader columnHeader24;
        internal ColumnHeader columnHeader25;
        internal ColumnHeader columnHeader26;
        internal TabPage tabPage5;
        internal Panel panel10;
        internal ListView listView7;
        internal ColumnHeader columnHeader27;
        internal ColumnHeader columnHeader28;
        internal ColumnHeader columnHeader29;
        internal TabPage tabPage6;
        internal Panel panel11;
        internal ListView listView8;
        internal ColumnHeader columnHeader30;
        internal ColumnHeader columnHeader31;
        internal ColumnHeader columnHeader32;
        internal ListView listViewAuto2;
        internal ColumnHeader columnHeader33;
        internal ColumnHeader columnHeader34;
        internal ColumnHeader columnHeader35;
        private IDPSASystem _sys;
     
        
		#region " Código generado por el Diseñador de Windows Forms "
		private FormChains() : base()
		{
			//El Diseñador de Windows Forms requiere esta llamada.
			InitializeComponent();            
			//Agregar cualquier inicialización después de la llamada a InitializeComponent()
		}

        public FormChains(IDPSASystem sys)
            : base()
        {
            //El Diseñador de Windows Forms requiere esta llamada.
            InitializeComponent();
            //Agregar cualquier inicialización después de la llamada a InitializeComponent()
            _sys = sys;
            _sys.Control.NewNotification += delegate(object sender, SystemControl.EventNotificationArgs ev)
            {
                if (ev.IdNotification == SystemControl.IdNotification.SubsystemsAndStates)
                {
                    LoadViews();
                }
            };
  
            LoadViews();
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
		internal ListView listViewAuto;
		internal ColumnHeader Input;
		internal ColumnHeader ColumnHeader1;
		internal ColumnHeader ColumnHeader2;
		internal TabControl tbCadenas;
		internal TabPage TabPage1;
		internal TabPage TabPage2;
		internal ListView listViewVOrigen;
		internal ColumnHeader ColumnHeader3;
		internal ColumnHeader ColumnHeader4;
		internal ColumnHeader ColumnHeader5;
		internal Panel Panel2;
		internal Panel Panel3;
		internal Button BtCongelar;
        internal TabPage TabPage3;
		[DebuggerStepThrough()]
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChains));
            this.listViewAuto = new System.Windows.Forms.ListView();
            this.Input = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.tbCadenas = new System.Windows.Forms.TabControl();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.Panel3 = new System.Windows.Forms.Panel();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.Panel2 = new System.Windows.Forms.Panel();
            this.listViewVOrigen = new System.Windows.Forms.ListView();
            this.ColumnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.TabPage3 = new System.Windows.Forms.TabPage();
            this.panel8 = new System.Windows.Forms.Panel();
            this.listViewLibres = new System.Windows.Forms.ListView();
            this.columnHeader21 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader22 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader23 = new System.Windows.Forms.ColumnHeader();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.listViewAuto2 = new System.Windows.Forms.ListView();
            this.columnHeader33 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader34 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader35 = new System.Windows.Forms.ColumnHeader();
            this.BtCongelar = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.panel4 = new System.Windows.Forms.Panel();
            this.listView2 = new System.Windows.Forms.ListView();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
            this.listView3 = new System.Windows.Forms.ListView();
            this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader13 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader14 = new System.Windows.Forms.ColumnHeader();
            this.panel5 = new System.Windows.Forms.Panel();
            this.listView4 = new System.Windows.Forms.ListView();
            this.columnHeader15 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader16 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader17 = new System.Windows.Forms.ColumnHeader();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.listView5 = new System.Windows.Forms.ListView();
            this.columnHeader18 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader19 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader20 = new System.Windows.Forms.ColumnHeader();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.panel9 = new System.Windows.Forms.Panel();
            this.listView6 = new System.Windows.Forms.ListView();
            this.columnHeader24 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader25 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader26 = new System.Windows.Forms.ColumnHeader();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.panel10 = new System.Windows.Forms.Panel();
            this.listView7 = new System.Windows.Forms.ListView();
            this.columnHeader27 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader28 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader29 = new System.Windows.Forms.ColumnHeader();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.panel11 = new System.Windows.Forms.Panel();
            this.listView8 = new System.Windows.Forms.ListView();
            this.columnHeader30 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader31 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader32 = new System.Windows.Forms.ColumnHeader();
            this.tbCadenas.SuspendLayout();
            this.TabPage1.SuspendLayout();
            this.TabPage2.SuspendLayout();
            this.TabPage3.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewAuto
            // 
            this.listViewAuto.BackColor = System.Drawing.Color.Azure;
            this.listViewAuto.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Input,
            this.ColumnHeader1,
            this.ColumnHeader2});
            this.listViewAuto.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewAuto.FullRowSelect = true;
            this.listViewAuto.GridLines = true;
            this.listViewAuto.Location = new System.Drawing.Point(0, 0);
            this.listViewAuto.Margin = new System.Windows.Forms.Padding(4);
            this.listViewAuto.Name = "listViewAuto";
            this.listViewAuto.Size = new System.Drawing.Size(937, 403);
            this.listViewAuto.TabIndex = 18;
            this.listViewAuto.UseCompatibleStateImageBehavior = false;
            this.listViewAuto.View = System.Windows.Forms.View.Details;
            // 
            // Input
            // 
            this.Input.Text = "Cadena";
            this.Input.Width = 379;
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "Paso Actual";
            this.ColumnHeader1.Width = 109;
            // 
            // ColumnHeader2
            // 
            this.ColumnHeader2.Text = "Descripción";
            this.ColumnHeader2.Width = 600;
            // 
            // tbCadenas
            // 
            this.tbCadenas.Controls.Add(this.TabPage1);
            this.tbCadenas.Controls.Add(this.tabPage7);
            this.tbCadenas.Controls.Add(this.TabPage2);
            this.tbCadenas.Controls.Add(this.TabPage3);
            this.tbCadenas.Location = new System.Drawing.Point(0, 0);
            this.tbCadenas.Margin = new System.Windows.Forms.Padding(4);
            this.tbCadenas.Name = "tbCadenas";
            this.tbCadenas.SelectedIndex = 0;
            this.tbCadenas.Size = new System.Drawing.Size(949, 455);
            this.tbCadenas.TabIndex = 19;
            // 
            // TabPage1
            // 
            this.TabPage1.Controls.Add(this.Panel3);
            this.TabPage1.Controls.Add(this.listViewAuto);
            this.TabPage1.Location = new System.Drawing.Point(4, 25);
            this.TabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Size = new System.Drawing.Size(941, 426);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "Automático";
            this.TabPage1.UseVisualStyleBackColor = true;
            // 
            // Panel3
            // 
            this.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Panel3.Location = new System.Drawing.Point(0, 416);
            this.Panel3.Margin = new System.Windows.Forms.Padding(4);
            this.Panel3.Name = "Panel3";
            this.Panel3.Size = new System.Drawing.Size(941, 10);
            this.Panel3.TabIndex = 21;
            // 
            // TabPage2
            // 
            this.TabPage2.Controls.Add(this.Panel2);
            this.TabPage2.Controls.Add(this.listViewVOrigen);
            this.TabPage2.Location = new System.Drawing.Point(4, 25);
            this.TabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Size = new System.Drawing.Size(941, 426);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "Vuelta Origen";
            this.TabPage2.UseVisualStyleBackColor = true;
            // 
            // Panel2
            // 
            this.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Panel2.Location = new System.Drawing.Point(0, 416);
            this.Panel2.Margin = new System.Windows.Forms.Padding(4);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(941, 10);
            this.Panel2.TabIndex = 21;
            // 
            // listViewVOrigen
            // 
            this.listViewVOrigen.BackColor = System.Drawing.Color.Azure;
            this.listViewVOrigen.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader3,
            this.ColumnHeader4,
            this.ColumnHeader5});
            this.listViewVOrigen.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewVOrigen.FullRowSelect = true;
            this.listViewVOrigen.GridLines = true;
            this.listViewVOrigen.Location = new System.Drawing.Point(0, 0);
            this.listViewVOrigen.Margin = new System.Windows.Forms.Padding(4);
            this.listViewVOrigen.Name = "listViewVOrigen";
            this.listViewVOrigen.Size = new System.Drawing.Size(937, 403);
            this.listViewVOrigen.TabIndex = 19;
            this.listViewVOrigen.UseCompatibleStateImageBehavior = false;
            this.listViewVOrigen.View = System.Windows.Forms.View.Details;
            // 
            // ColumnHeader3
            // 
            this.ColumnHeader3.Text = "Cadena";
            this.ColumnHeader3.Width = 378;
            // 
            // ColumnHeader4
            // 
            this.ColumnHeader4.Text = "Paso Actual";
            this.ColumnHeader4.Width = 110;
            // 
            // ColumnHeader5
            // 
            this.ColumnHeader5.Text = "Descripción";
            this.ColumnHeader5.Width = 600;
            // 
            // TabPage3
            // 
            this.TabPage3.Controls.Add(this.panel8);
            this.TabPage3.Controls.Add(this.listViewLibres);
            this.TabPage3.Location = new System.Drawing.Point(4, 25);
            this.TabPage3.Margin = new System.Windows.Forms.Padding(4);
            this.TabPage3.Name = "TabPage3";
            this.TabPage3.Size = new System.Drawing.Size(941, 426);
            this.TabPage3.TabIndex = 2;
            this.TabPage3.Text = "Libres";
            this.TabPage3.UseVisualStyleBackColor = true;
            // 
            // panel8
            // 
            this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel8.Location = new System.Drawing.Point(0, 416);
            this.panel8.Margin = new System.Windows.Forms.Padding(4);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(941, 10);
            this.panel8.TabIndex = 23;
            // 
            // listViewLibres
            // 
            this.listViewLibres.BackColor = System.Drawing.Color.Azure;
            this.listViewLibres.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader21,
            this.columnHeader22,
            this.columnHeader23});
            this.listViewLibres.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewLibres.FullRowSelect = true;
            this.listViewLibres.GridLines = true;
            this.listViewLibres.Location = new System.Drawing.Point(0, 0);
            this.listViewLibres.Margin = new System.Windows.Forms.Padding(4);
            this.listViewLibres.Name = "listViewLibres";
            this.listViewLibres.Size = new System.Drawing.Size(937, 403);
            this.listViewLibres.TabIndex = 22;
            this.listViewLibres.UseCompatibleStateImageBehavior = false;
            this.listViewLibres.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader21
            // 
            this.columnHeader21.Text = "Cadena";
            this.columnHeader21.Width = 379;
            // 
            // columnHeader22
            // 
            this.columnHeader22.Text = "Paso Actual";
            this.columnHeader22.Width = 109;
            // 
            // columnHeader23
            // 
            this.columnHeader23.Text = "Descripción";
            this.columnHeader23.Width = 600;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.listViewAuto2);
            this.tabPage7.Location = new System.Drawing.Point(4, 25);
            this.tabPage7.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(941, 426);
            this.tabPage7.TabIndex = 3;
            this.tabPage7.Text = "AutoTransporte";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // listViewAuto2
            // 
            this.listViewAuto2.BackColor = System.Drawing.Color.Azure;
            this.listViewAuto2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader33,
            this.columnHeader34,
            this.columnHeader35});
            this.listViewAuto2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewAuto2.FullRowSelect = true;
            this.listViewAuto2.GridLines = true;
            this.listViewAuto2.Location = new System.Drawing.Point(2, 12);
            this.listViewAuto2.Margin = new System.Windows.Forms.Padding(4);
            this.listViewAuto2.Name = "listViewAuto2";
            this.listViewAuto2.Size = new System.Drawing.Size(937, 403);
            this.listViewAuto2.TabIndex = 19;
            this.listViewAuto2.UseCompatibleStateImageBehavior = false;
            this.listViewAuto2.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader33
            // 
            this.columnHeader33.Text = "Cadena";
            this.columnHeader33.Width = 379;
            // 
            // columnHeader34
            // 
            this.columnHeader34.Text = "Paso Actual";
            this.columnHeader34.Width = 109;
            // 
            // columnHeader35
            // 
            this.columnHeader35.Text = "Descripción";
            this.columnHeader35.Width = 600;
            // 
            // BtCongelar
            // 
            this.BtCongelar.Location = new System.Drawing.Point(837, 463);
            this.BtCongelar.Margin = new System.Windows.Forms.Padding(4);
            this.BtCongelar.Name = "BtCongelar";
            this.BtCongelar.Size = new System.Drawing.Size(107, 28);
            this.BtCongelar.TabIndex = 20;
            this.BtCongelar.Text = "Congelar";
            this.BtCongelar.Click += new System.EventHandler(this.BtCongelar_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 416);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(941, 10);
            this.panel1.TabIndex = 21;
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.Color.Azure;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.listView1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(937, 403);
            this.listView1.TabIndex = 18;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Cadena";
            this.columnHeader6.Width = 379;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Paso Actual";
            this.columnHeader7.Width = 109;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Descripción";
            this.columnHeader8.Width = 600;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 416);
            this.panel4.Margin = new System.Windows.Forms.Padding(4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(941, 10);
            this.panel4.TabIndex = 21;
            // 
            // listView2
            // 
            this.listView2.BackColor = System.Drawing.Color.Azure;
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11});
            this.listView2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView2.FullRowSelect = true;
            this.listView2.GridLines = true;
            this.listView2.Location = new System.Drawing.Point(0, 0);
            this.listView2.Margin = new System.Windows.Forms.Padding(4);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(937, 403);
            this.listView2.TabIndex = 18;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Cadena";
            this.columnHeader9.Width = 379;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Paso Actual";
            this.columnHeader10.Width = 109;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Descripción";
            this.columnHeader11.Width = 600;
            // 
            // listView3
            // 
            this.listView3.BackColor = System.Drawing.Color.Azure;
            this.listView3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14});
            this.listView3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView3.FullRowSelect = true;
            this.listView3.GridLines = true;
            this.listView3.Location = new System.Drawing.Point(0, 0);
            this.listView3.Margin = new System.Windows.Forms.Padding(4);
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(937, 403);
            this.listView3.TabIndex = 18;
            this.listView3.UseCompatibleStateImageBehavior = false;
            this.listView3.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Cadena";
            this.columnHeader12.Width = 379;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Paso Actual";
            this.columnHeader13.Width = 109;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "Descripción";
            this.columnHeader14.Width = 600;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 416);
            this.panel5.Margin = new System.Windows.Forms.Padding(4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(941, 10);
            this.panel5.TabIndex = 21;
            // 
            // listView4
            // 
            this.listView4.BackColor = System.Drawing.Color.Azure;
            this.listView4.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader15,
            this.columnHeader16,
            this.columnHeader17});
            this.listView4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView4.FullRowSelect = true;
            this.listView4.GridLines = true;
            this.listView4.Location = new System.Drawing.Point(0, 0);
            this.listView4.Margin = new System.Windows.Forms.Padding(4);
            this.listView4.Name = "listView4";
            this.listView4.Size = new System.Drawing.Size(937, 403);
            this.listView4.TabIndex = 18;
            this.listView4.UseCompatibleStateImageBehavior = false;
            this.listView4.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "Cadena";
            this.columnHeader15.Width = 379;
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "Paso Actual";
            this.columnHeader16.Width = 109;
            // 
            // columnHeader17
            // 
            this.columnHeader17.Text = "Descripción";
            this.columnHeader17.Width = 600;
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 416);
            this.panel6.Margin = new System.Windows.Forms.Padding(4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(941, 10);
            this.panel6.TabIndex = 21;
            // 
            // panel7
            // 
            this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel7.Location = new System.Drawing.Point(0, 416);
            this.panel7.Margin = new System.Windows.Forms.Padding(4);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(941, 10);
            this.panel7.TabIndex = 21;
            // 
            // listView5
            // 
            this.listView5.BackColor = System.Drawing.Color.Azure;
            this.listView5.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader18,
            this.columnHeader19,
            this.columnHeader20});
            this.listView5.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView5.FullRowSelect = true;
            this.listView5.GridLines = true;
            this.listView5.Location = new System.Drawing.Point(0, 0);
            this.listView5.Margin = new System.Windows.Forms.Padding(4);
            this.listView5.Name = "listView5";
            this.listView5.Size = new System.Drawing.Size(937, 403);
            this.listView5.TabIndex = 18;
            this.listView5.UseCompatibleStateImageBehavior = false;
            this.listView5.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader18
            // 
            this.columnHeader18.Text = "Cadena";
            this.columnHeader18.Width = 379;
            // 
            // columnHeader19
            // 
            this.columnHeader19.Text = "Paso Actual";
            this.columnHeader19.Width = 109;
            // 
            // columnHeader20
            // 
            this.columnHeader20.Text = "Descripción";
            this.columnHeader20.Width = 600;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.panel9);
            this.tabPage4.Controls.Add(this.listView6);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(941, 426);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Automático";
            // 
            // panel9
            // 
            this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel9.Location = new System.Drawing.Point(0, 416);
            this.panel9.Margin = new System.Windows.Forms.Padding(4);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(941, 10);
            this.panel9.TabIndex = 21;
            // 
            // listView6
            // 
            this.listView6.BackColor = System.Drawing.Color.Azure;
            this.listView6.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader24,
            this.columnHeader25,
            this.columnHeader26});
            this.listView6.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView6.FullRowSelect = true;
            this.listView6.GridLines = true;
            this.listView6.Location = new System.Drawing.Point(0, 0);
            this.listView6.Margin = new System.Windows.Forms.Padding(4);
            this.listView6.Name = "listView6";
            this.listView6.Size = new System.Drawing.Size(937, 403);
            this.listView6.TabIndex = 18;
            this.listView6.UseCompatibleStateImageBehavior = false;
            this.listView6.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader24
            // 
            this.columnHeader24.Text = "Cadena";
            this.columnHeader24.Width = 379;
            // 
            // columnHeader25
            // 
            this.columnHeader25.Text = "Paso Actual";
            this.columnHeader25.Width = 109;
            // 
            // columnHeader26
            // 
            this.columnHeader26.Text = "Descripción";
            this.columnHeader26.Width = 600;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.panel10);
            this.tabPage5.Controls.Add(this.listView7);
            this.tabPage5.Location = new System.Drawing.Point(4, 25);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(941, 426);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Vuelta Origen";
            // 
            // panel10
            // 
            this.panel10.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel10.Location = new System.Drawing.Point(0, 416);
            this.panel10.Margin = new System.Windows.Forms.Padding(4);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(941, 10);
            this.panel10.TabIndex = 21;
            // 
            // listView7
            // 
            this.listView7.BackColor = System.Drawing.Color.Azure;
            this.listView7.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader27,
            this.columnHeader28,
            this.columnHeader29});
            this.listView7.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView7.FullRowSelect = true;
            this.listView7.GridLines = true;
            this.listView7.Location = new System.Drawing.Point(0, 0);
            this.listView7.Margin = new System.Windows.Forms.Padding(4);
            this.listView7.Name = "listView7";
            this.listView7.Size = new System.Drawing.Size(937, 403);
            this.listView7.TabIndex = 19;
            this.listView7.UseCompatibleStateImageBehavior = false;
            this.listView7.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader27
            // 
            this.columnHeader27.Text = "Cadena";
            this.columnHeader27.Width = 378;
            // 
            // columnHeader28
            // 
            this.columnHeader28.Text = "Paso Actual";
            this.columnHeader28.Width = 110;
            // 
            // columnHeader29
            // 
            this.columnHeader29.Text = "Descripción";
            this.columnHeader29.Width = 600;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.panel11);
            this.tabPage6.Controls.Add(this.listView8);
            this.tabPage6.Location = new System.Drawing.Point(4, 25);
            this.tabPage6.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(941, 426);
            this.tabPage6.TabIndex = 2;
            this.tabPage6.Text = "Libres";
            // 
            // panel11
            // 
            this.panel11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel11.Location = new System.Drawing.Point(0, 416);
            this.panel11.Margin = new System.Windows.Forms.Padding(4);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(941, 10);
            this.panel11.TabIndex = 23;
            // 
            // listView8
            // 
            this.listView8.BackColor = System.Drawing.Color.Azure;
            this.listView8.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader30,
            this.columnHeader31,
            this.columnHeader32});
            this.listView8.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView8.FullRowSelect = true;
            this.listView8.GridLines = true;
            this.listView8.Location = new System.Drawing.Point(0, 0);
            this.listView8.Margin = new System.Windows.Forms.Padding(4);
            this.listView8.Name = "listView8";
            this.listView8.Size = new System.Drawing.Size(937, 403);
            this.listView8.TabIndex = 22;
            this.listView8.UseCompatibleStateImageBehavior = false;
            this.listView8.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader30
            // 
            this.columnHeader30.Text = "Cadena";
            this.columnHeader30.Width = 379;
            // 
            // columnHeader31
            // 
            this.columnHeader31.Text = "Paso Actual";
            this.columnHeader31.Width = 109;
            // 
            // columnHeader32
            // 
            this.columnHeader32.Text = "Descripción";
            this.columnHeader32.Width = 600;
            // 
            // FormChains
            // 
            this.ClientSize = new System.Drawing.Size(952, 500);
            this.Controls.Add(this.BtCongelar);
            this.Controls.Add(this.tbCadenas);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormChains";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Control Cadenas";
            this.TopMost = true;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmCadenas_Closing);
            this.tbCadenas.ResumeLayout(false);
            this.TabPage1.ResumeLayout(false);
            this.TabPage2.ResumeLayout(false);
            this.TabPage3.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion
		
        private void LoadViews()
        {
            LoadView(listViewAuto, _sys.Chains.AutomaticChains);
            LoadView(listViewVOrigen, _sys.Chains.BackToOgiginChains);
            LoadView(listViewLibres, _sys.Chains.FreeChains);
            LoadView(listViewAuto2, _sys.Chains.AutomaticChains2);
        }

		private void Run()
		{
		    if (BtCongelar.Text != "Congelar" || !Visible) return;

            RefreshAuto();
            RefreshAuto2();
		    RefreshVOrig();
		    RefreshFree();
		}

        private void LoadView(ListView listView, ChainCollection cadenas)
        {
            listView.Items.Clear();

            foreach (Chain myCadena in cadenas.AllValues)
            {
                var myLstItem = new ListViewItem(myCadena.CurrentExecutionSubchainName);
                myLstItem.SubItems.Add(myCadena.CurrentStepIndex.ToString());
                myLstItem.SubItems.Add(myCadena.CurrentStep.Comment);
                listView.Items.AddRange(new ListViewItem[] { myLstItem });
            }
        }

        private void RefreshView(ListView listView, ChainCollection cadenas)
        {
            for (int i = 0; i < cadenas.AllValues.Count;i++)
            {                
                var chain = cadenas.AllValues.ElementAt(i);
                string stepName = chain.CurrentExecutionSubchainName;
                string stepIndex = chain.CurrentStepIndex.ToString();
                string stepComment = chain.CurrentStep.Comment;

                if (listView.Items[i].SubItems[0].Text != stepName)
                    listView.Items[i].SubItems[0].Text = stepName;

                if (listView.Items[i].SubItems[1].Text != stepIndex)
                    listView.Items[i].SubItems[1].Text = stepIndex;

                if (listView.Items[i].SubItems[2].Text != stepComment)
                    listView.Items[i].SubItems[2].Text = stepComment;                                          
            }
        }		

				
		private void RefreshAuto()
		{
			if (((Width > 500) & TabPage1.Visible & _timer1.Timing(500)))
			{
                RefreshView(listViewAuto, _sys.Chains.AutomaticChains);
			}
		}
		private void RefreshVOrig()
		{
			if (((Width > 500) & TabPage2.Visible & _timer2.Timing(500)))
			{
                RefreshView(listViewVOrigen, _sys.Chains.BackToOgiginChains);
			}
		}
		private void RefreshFree()
		{
			if (((Width > 500) & TabPage3.Visible & _timer3.Timing(500)))
			{
                RefreshView(listViewLibres, _sys.Chains.FreeChains);
			}
		}

        private void RefreshAuto2()
        {
            if (((Width > 500) & tabPage7.Visible & _timer4.Timing(500)))
            {
                RefreshView(listViewAuto2, _sys.Chains.AutomaticChains2);
            }
        }

		private void frmCadenas_Closing(object sender, CancelEventArgs e)
		{
			Hide();
			e.Cancel = true;
		}
		private void BtCongelar_Click(object sender, EventArgs e)
		{
			if (BtCongelar.Text == "Congelar")
			{
				BtCongelar.Text = "Descongelar";
			}
			else
			{
				BtCongelar.Text = "Congelar";
			}
		}

        #region IViewTaskOwner Members

        public IEnumerable<Action> GetViewTasks()
        {
            return new List<Action> { Run };
        }

        #endregion

      

       
	}
}
