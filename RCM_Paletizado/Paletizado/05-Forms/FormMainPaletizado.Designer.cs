using Idpsa;
using System;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Idpsa
{
    partial class FormMainPaletizado
    {

        #region " Windows Form Designer generated code "



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
        //Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        internal System.Windows.Forms.MenuItem menuSimbolico;
        internal System.Windows.Forms.ImageList imgSimbolico;
        internal System.Windows.Forms.MenuItem menuCadenas;
        internal System.Windows.Forms.MenuItem menuParker;
        internal System.Windows.Forms.MenuItem menuVariador;
        internal System.Windows.Forms.OpenFileDialog OpenDialog;
        internal System.Windows.Forms.MenuItem menuAdministrador;
        internal System.Windows.Forms.MenuItem menuPermiso;
        internal System.Windows.Forms.MenuItem menuAlarmas;
        internal System.Windows.Forms.MainMenu mnPasaportes;
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMainPaletizado));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "1",
            "Mensaje Error"}, -1);
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Error Rearme Barrera",
            "Lector RFID no conectado"}, -1);
            this.BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.mnPasaportes = new System.Windows.Forms.MainMenu(this.components);
            this.menuSimbolico = new System.Windows.Forms.MenuItem();
            this.menuCadenas = new System.Windows.Forms.MenuItem();
            this.menuParker = new System.Windows.Forms.MenuItem();
            this.menuVariador = new System.Windows.Forms.MenuItem();
            this.menuAlarmas = new System.Windows.Forms.MenuItem();
            this.menuPermiso = new System.Windows.Forms.MenuItem();
            this.menuAdministrador = new System.Windows.Forms.MenuItem();
            this.menuPruebas = new System.Windows.Forms.MenuItem();
            this.menuNuevo = new System.Windows.Forms.MenuItem();
            this.menuPasaporte = new System.Windows.Forms.MenuItem();
            this.menuCatalogo = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuImpresion = new System.Windows.Forms.MenuItem();
            this.menuGuardar = new System.Windows.Forms.MenuItem();
            this.saveAll = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuGuradarCatalogos = new System.Windows.Forms.MenuItem();
            this.menuGuardarGruposTransporte = new System.Windows.Forms.MenuItem();
            this.menuGuardarCajasBandas = new System.Windows.Forms.MenuItem();
            this.menuItemRefreshForm = new System.Windows.Forms.MenuItem();
            this.menuCargar = new System.Windows.Forms.MenuItem();
            this.menuCargarGruposTransporte = new System.Windows.Forms.MenuItem();
            this.menuCargarCajasBandas = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.imgSimbolico = new System.Windows.Forms.ImageList(this.components);
            this.OpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnManual = new System.Windows.Forms.Button();
            this.lblModoActual = new System.Windows.Forms.Label();
            this.btnVOrig = new System.Windows.Forms.Button();
            this.btnAuto = new System.Windows.Forms.Button();
            this.btnActiveModeStart = new System.Windows.Forms.Button();
            this.btnActiveModeStop = new System.Windows.Forms.Button();
            this.pnlModo = new System.Windows.Forms.Panel();
            this.pnlRearmePasoAPaso = new System.Windows.Forms.Panel();
            this.btEntradaManual0 = new System.Windows.Forms.Button();
            this.checkBoxAutoSemaforo = new System.Windows.Forms.CheckBox();
            this.labelVaciar = new System.Windows.Forms.Label();
            this.btVaciar = new System.Windows.Forms.Button();
            this.btModoAcumulacion = new System.Windows.Forms.Button();
            this.Panel25 = new System.Windows.Forms.Panel();
            this.btnPaso = new System.Windows.Forms.Button();
            this.chkPasoAPaso = new System.Windows.Forms.CheckBox();
            this.btnRearme = new System.Windows.Forms.Button();
            this.lblCycleTime = new System.Windows.Forms.Label();
            this.pnlMaquina = new System.Windows.Forms.Panel();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblConexionMando = new System.Windows.Forms.Label();
            this.lblDiagnosis = new System.Windows.Forms.Label();
            this.lblProteccionesAnuladas = new System.Windows.Forms.Label();
            this.lblProteccionesOk = new System.Windows.Forms.Label();
            this.lblAireOk = new System.Windows.Forms.Label();
            this.lblOrigen = new System.Windows.Forms.Label();
            this.lblSistemaArranque = new System.Windows.Forms.Label();
            this.ColumnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.lstDiagnosis = new System.Windows.Forms.ListView();
            this.CatalogoDBBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.TabSystem = new Idpsa.TabControlEx();
            this.TabPage6 = new System.Windows.Forms.TabPage();
            this.panel9 = new System.Windows.Forms.Panel();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.PnlPaletizadoLine1 = new System.Windows.Forms.Panel();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.PnlPaletizadoLine2 = new System.Windows.Forms.Panel();
            this.TabPage3 = new System.Windows.Forms.TabPage();
            this.PnlTransports = new System.Windows.Forms.Panel();
            this.TabPage4 = new System.Windows.Forms.TabPage();
            this.pnlMovManual = new System.Windows.Forms.Panel();
            this.treeManual = new System.Windows.Forms.TreeView();
            this.TabPage5 = new System.Windows.Forms.TabPage();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.manualBoxReprocessor = new Idpsa.Paletizado.ManualBoxReprocessor();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.pBandaProdec = new System.Windows.Forms.Panel();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.panel99 = new System.Windows.Forms.Panel();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.cbDia = new System.Windows.Forms.ComboBox();
            this.cbMes = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbTurno = new System.Windows.Forms.ComboBox();
            this.tbAño = new System.Windows.Forms.TextBox();
            this.tbcatalogoID = new System.Windows.Forms.TextBox();
            this.datosTurnoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.listAlarmaInfo = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource)).BeginInit();
            this.pnlModo.SuspendLayout();
            this.pnlRearmePasoAPaso.SuspendLayout();
            this.Panel25.SuspendLayout();
            this.pnlMaquina.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CatalogoDBBindingSource)).BeginInit();
            this.TabSystem.SuspendLayout();
            this.TabPage6.SuspendLayout();
            this.TabPage1.SuspendLayout();
            this.TabPage2.SuspendLayout();
            this.TabPage3.SuspendLayout();
            this.TabPage4.SuspendLayout();
            this.TabPage5.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.panel99.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datosTurnoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // BindingSource
            // 
            this.BindingSource.DataSource = typeof(Idpsa.datosTurno);
            // 
            // mnPasaportes
            // 
            this.mnPasaportes.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuSimbolico,
            this.menuCadenas,
            this.menuParker,
            this.menuVariador,
            this.menuAlarmas,
            this.menuPermiso,
            this.menuAdministrador,
            this.menuPruebas,
            this.menuNuevo,
            this.menuImpresion,
            this.menuGuardar,
            this.menuItemRefreshForm,
            this.menuCargar,
            this.menuItem2});
            // 
            // menuSimbolico
            // 
            this.menuSimbolico.Index = 0;
            this.menuSimbolico.Text = "Simbolico";
            this.menuSimbolico.Click += new System.EventHandler(this.menuSimbolico_Click);
            // 
            // menuCadenas
            // 
            this.menuCadenas.Index = 1;
            this.menuCadenas.Text = "Cadenas";
            this.menuCadenas.Click += new System.EventHandler(this.menuCadenas_Click);
            // 
            // menuParker
            // 
            this.menuParker.Index = 2;
            this.menuParker.Text = "Parker";
            this.menuParker.Click += new System.EventHandler(this.menuParker_Click);
            // 
            // menuVariador
            // 
            this.menuVariador.Index = 3;
            this.menuVariador.Text = "Variador";
            this.menuVariador.Click += new System.EventHandler(this.menuVariador_Click);
            // 
            // menuAlarmas
            // 
            this.menuAlarmas.Index = 4;
            this.menuAlarmas.Text = "Alarmas";
            this.menuAlarmas.Click += new System.EventHandler(this.menuAlarmas_Click);
            // 
            // menuPermiso
            // 
            this.menuPermiso.Index = 5;
            this.menuPermiso.Text = "Permiso";
            this.menuPermiso.Click += new System.EventHandler(this.menuPermiso_Click);
            // 
            // menuAdministrador
            // 
            this.menuAdministrador.Index = 6;
            this.menuAdministrador.Text = "Administrador";
            this.menuAdministrador.Click += new System.EventHandler(this.menuAdministrador_Click);
            // 
            // menuPruebas
            // 
            this.menuPruebas.Index = 7;
            this.menuPruebas.Text = "Pruebas";
            // 
            // menuNuevo
            // 
            this.menuNuevo.Index = 8;
            this.menuNuevo.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPasaporte,
            this.menuCatalogo,
            this.menuItem1});
            this.menuNuevo.Text = "Nuevo";
            // 
            // menuPasaporte
            // 
            this.menuPasaporte.Index = 0;
            this.menuPasaporte.Text = "Pasaporte";
            this.menuPasaporte.Click += new System.EventHandler(this.menuPasaporte_Click);
            // 
            // menuCatalogo
            // 
            this.menuCatalogo.Index = 1;
            this.menuCatalogo.Text = "Catálogo";
            this.menuCatalogo.Click += new System.EventHandler(this.menuCatalogo_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 2;
            this.menuItem1.Text = "Paletizado";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuImpresion
            // 
            this.menuImpresion.Index = 9;
            this.menuImpresion.Text = "Impresión";
            this.menuImpresion.Click += new System.EventHandler(this.menuImpresion_Click);
            // 
            // menuGuardar
            // 
            this.menuGuardar.Index = 10;
            this.menuGuardar.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.saveAll,
            this.menuItem3,
            this.menuGuradarCatalogos,
            this.menuGuardarGruposTransporte,
            this.menuGuardarCajasBandas});
            this.menuGuardar.Text = "Guardar";
            // 
            // saveAll
            // 
            this.saveAll.Index = 0;
            this.saveAll.Text = "Guardar Todo";
            this.saveAll.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "-";
            // 
            // menuGuradarCatalogos
            // 
            this.menuGuradarCatalogos.Index = 2;
            this.menuGuradarCatalogos.Text = "Guardar Catálogos";
            this.menuGuradarCatalogos.Click += new System.EventHandler(this.menuGuardarCatalogos_Click);
            // 
            // menuGuardarGruposTransporte
            // 
            this.menuGuardarGruposTransporte.Index = 3;
            this.menuGuardarGruposTransporte.Text = "Guardar Grupos Transporte";
            this.menuGuardarGruposTransporte.Click += new System.EventHandler(this.menuGuardarGruposTransporte_Click);
            // 
            // menuGuardarCajasBandas
            // 
            this.menuGuardarCajasBandas.Index = 4;
            this.menuGuardarCajasBandas.Text = "Guardar Cajas Bandas";
            this.menuGuardarCajasBandas.Click += new System.EventHandler(this.menuGuardarCajasBandas_Click);
            // 
            // menuItemRefreshForm
            // 
            this.menuItemRefreshForm.Index = 11;
            this.menuItemRefreshForm.Text = "Actualizar";
            this.menuItemRefreshForm.Click += new System.EventHandler(this.menuItemRefreshForm_Click);
            // 
            // menuCargar
            // 
            this.menuCargar.Index = 12;
            this.menuCargar.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuCargarGruposTransporte,
            this.menuCargarCajasBandas});
            this.menuCargar.Text = "Cargar";
            // 
            // menuCargarGruposTransporte
            // 
            this.menuCargarGruposTransporte.Index = 0;
            this.menuCargarGruposTransporte.Text = "Cargar Grupos Transporte";
            this.menuCargarGruposTransporte.Click += new System.EventHandler(this.menuCargarGruposTransporte_Click);
            // 
            // menuCargarCajasBandas
            // 
            this.menuCargarCajasBandas.Index = 1;
            this.menuCargarCajasBandas.Text = "Cargar Cajas Bandas";
            this.menuCargarCajasBandas.Click += new System.EventHandler(this.menuCargarCajasBandas_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 13;
            this.menuItem2.Text = "Auto-Semáforo";
            this.menuItem2.Visible = false;
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // imgSimbolico
            // 
            this.imgSimbolico.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imgSimbolico.ImageSize = new System.Drawing.Size(16, 16);
            this.imgSimbolico.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // OpenDialog
            // 
            this.OpenDialog.DefaultExt = "txt";
            this.OpenDialog.Filter = "Catálogo|*.TXT";
            this.OpenDialog.Title = "Cargar Catálogo";
            // 
            // btnManual
            // 
            this.btnManual.BackColor = System.Drawing.Color.LightGray;
            this.btnManual.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnManual.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnManual.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManual.Location = new System.Drawing.Point(0, 776);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(84, 64);
            this.btnManual.TabIndex = 0;
            this.btnManual.Text = "Manual";
            this.btnManual.UseVisualStyleBackColor = false;
            this.btnManual.Click += new System.EventHandler(this.ActionQueryHandler);
            // 
            // lblModoActual
            // 
            this.lblModoActual.BackColor = System.Drawing.Color.Khaki;
            this.lblModoActual.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblModoActual.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModoActual.Location = new System.Drawing.Point(0, 0);
            this.lblModoActual.Name = "lblModoActual";
            this.lblModoActual.Size = new System.Drawing.Size(84, 72);
            this.lblModoActual.TabIndex = 8;
            this.lblModoActual.Text = "Modo Actual";
            this.lblModoActual.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnVOrig
            // 
            this.btnVOrig.BackColor = System.Drawing.Color.LightGray;
            this.btnVOrig.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnVOrig.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnVOrig.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVOrig.Location = new System.Drawing.Point(0, 708);
            this.btnVOrig.Name = "btnVOrig";
            this.btnVOrig.Size = new System.Drawing.Size(84, 68);
            this.btnVOrig.TabIndex = 9;
            this.btnVOrig.Text = "Vuelta Origen";
            this.btnVOrig.UseVisualStyleBackColor = false;
            this.btnVOrig.Click += new System.EventHandler(this.ActionQueryHandler);
            // 
            // btnAuto
            // 
            this.btnAuto.BackColor = System.Drawing.Color.LightGray;
            this.btnAuto.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnAuto.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAuto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAuto.Location = new System.Drawing.Point(0, 633);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(84, 75);
            this.btnAuto.TabIndex = 10;
            this.btnAuto.Text = "Automático";
            this.btnAuto.UseVisualStyleBackColor = false;
            this.btnAuto.Click += new System.EventHandler(this.ActionQueryHandler);
            // 
            // btnActiveModeStart
            // 
            this.btnActiveModeStart.BackColor = System.Drawing.Color.LightGray;
            this.btnActiveModeStart.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnActiveModeStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnActiveModeStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActiveModeStart.Location = new System.Drawing.Point(0, 72);
            this.btnActiveModeStart.Name = "btnActiveModeStart";
            this.btnActiveModeStart.Size = new System.Drawing.Size(84, 72);
            this.btnActiveModeStart.TabIndex = 11;
            this.btnActiveModeStart.Text = "Start";
            this.btnActiveModeStart.UseVisualStyleBackColor = false;
            this.btnActiveModeStart.Click += new System.EventHandler(this.ActionQueryHandler);
            this.btnActiveModeStart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnActiveModeStart_MouseDown);
            this.btnActiveModeStart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnActiveModeStart_MouseUp);
            // 
            // btnActiveModeStop
            // 
            this.btnActiveModeStop.BackColor = System.Drawing.Color.LightGray;
            this.btnActiveModeStop.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnActiveModeStop.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnActiveModeStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActiveModeStop.Location = new System.Drawing.Point(0, 144);
            this.btnActiveModeStop.Name = "btnActiveModeStop";
            this.btnActiveModeStop.Size = new System.Drawing.Size(84, 72);
            this.btnActiveModeStop.TabIndex = 12;
            this.btnActiveModeStop.Text = "Stop";
            this.btnActiveModeStop.UseVisualStyleBackColor = false;
            this.btnActiveModeStop.Click += new System.EventHandler(this.ActionQueryHandler);
            // 
            // pnlModo
            // 
            this.pnlModo.BackColor = System.Drawing.Color.Transparent;
            this.pnlModo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlModo.Controls.Add(this.pnlRearmePasoAPaso);
            this.pnlModo.Controls.Add(this.btnActiveModeStop);
            this.pnlModo.Controls.Add(this.btnActiveModeStart);
            this.pnlModo.Controls.Add(this.btnAuto);
            this.pnlModo.Controls.Add(this.btnVOrig);
            this.pnlModo.Controls.Add(this.lblModoActual);
            this.pnlModo.Controls.Add(this.btnManual);
            this.pnlModo.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlModo.Location = new System.Drawing.Point(940, 0);
            this.pnlModo.Name = "pnlModo";
            this.pnlModo.Size = new System.Drawing.Size(88, 844);
            this.pnlModo.TabIndex = 29;
            // 
            // pnlRearmePasoAPaso
            // 
            this.pnlRearmePasoAPaso.Controls.Add(this.btEntradaManual0);
            this.pnlRearmePasoAPaso.Controls.Add(this.checkBoxAutoSemaforo);
            this.pnlRearmePasoAPaso.Controls.Add(this.labelVaciar);
            this.pnlRearmePasoAPaso.Controls.Add(this.btVaciar);
            this.pnlRearmePasoAPaso.Controls.Add(this.btModoAcumulacion);
            this.pnlRearmePasoAPaso.Controls.Add(this.Panel25);
            this.pnlRearmePasoAPaso.Controls.Add(this.btnRearme);
            this.pnlRearmePasoAPaso.Controls.Add(this.lblCycleTime);
            this.pnlRearmePasoAPaso.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRearmePasoAPaso.Location = new System.Drawing.Point(0, 216);
            this.pnlRearmePasoAPaso.Name = "pnlRearmePasoAPaso";
            this.pnlRearmePasoAPaso.Size = new System.Drawing.Size(84, 417);
            this.pnlRearmePasoAPaso.TabIndex = 13;
            // 
            // btEntradaManual0
            // 
            this.btEntradaManual0.BackColor = System.Drawing.Color.LightGreen;
            this.btEntradaManual0.Location = new System.Drawing.Point(4, 108);
            this.btEntradaManual0.Name = "btEntradaManual0";
            this.btEntradaManual0.Size = new System.Drawing.Size(77, 84);
            this.btEntradaManual0.TabIndex = 50;
            this.btEntradaManual0.Text = "Entrada Manual Prodec";
            this.btEntradaManual0.UseVisualStyleBackColor = false;
            this.btEntradaManual0.Click += new System.EventHandler(this.btEntradaManual_Click);
            // 
            // checkBoxAutoSemaforo
            // 
            this.checkBoxAutoSemaforo.AutoSize = true;
            this.checkBoxAutoSemaforo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAutoSemaforo.Location = new System.Drawing.Point(2, 286);
            this.checkBoxAutoSemaforo.Name = "checkBoxAutoSemaforo";
            this.checkBoxAutoSemaforo.Size = new System.Drawing.Size(85, 17);
            this.checkBoxAutoSemaforo.TabIndex = 48;
            this.checkBoxAutoSemaforo.Text = "EntradaAuto";
            this.checkBoxAutoSemaforo.UseVisualStyleBackColor = true;
            this.checkBoxAutoSemaforo.Visible = false;
            this.checkBoxAutoSemaforo.CheckedChanged += new System.EventHandler(this.checkBoxAutoSemaforo_CheckedChanged);
            // 
            // labelVaciar
            // 
            this.labelVaciar.BackColor = System.Drawing.Color.Khaki;
            this.labelVaciar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelVaciar.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVaciar.ForeColor = System.Drawing.Color.Black;
            this.labelVaciar.Location = new System.Drawing.Point(4, 78);
            this.labelVaciar.Name = "labelVaciar";
            this.labelVaciar.Size = new System.Drawing.Size(76, 26);
            this.labelVaciar.TabIndex = 14;
            this.labelVaciar.Text = "Vaciando";
            this.labelVaciar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelVaciar.Click += new System.EventHandler(this.lblCycleTime_Click);
            // 
            // btVaciar
            // 
            this.btVaciar.BackColor = System.Drawing.SystemColors.Control;
            this.btVaciar.Location = new System.Drawing.Point(4, 93);
            this.btVaciar.Name = "btVaciar";
            this.btVaciar.Size = new System.Drawing.Size(76, 48);
            this.btVaciar.TabIndex = 47;
            this.btVaciar.Text = "Vaciando";
            this.btVaciar.UseVisualStyleBackColor = false;
            this.btVaciar.Visible = false;
            this.btVaciar.Click += new System.EventHandler(this.btVaciar_Click);
            // 
            // btModoAcumulacion
            // 
            this.btModoAcumulacion.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btModoAcumulacion.Location = new System.Drawing.Point(4, 25);
            this.btModoAcumulacion.Name = "btModoAcumulacion";
            this.btModoAcumulacion.Size = new System.Drawing.Size(77, 50);
            this.btModoAcumulacion.TabIndex = 47;
            this.btModoAcumulacion.Text = "Modo Acumulación Inactivo";
            this.btModoAcumulacion.UseVisualStyleBackColor = false;
            this.btModoAcumulacion.Click += new System.EventHandler(this.btModoAcumulacion_Click);
            // 
            // Panel25
            // 
            this.Panel25.BackColor = System.Drawing.Color.Transparent;
            this.Panel25.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Panel25.Controls.Add(this.btnPaso);
            this.Panel25.Controls.Add(this.chkPasoAPaso);
            this.Panel25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Panel25.Location = new System.Drawing.Point(0, 195);
            this.Panel25.Name = "Panel25";
            this.Panel25.Size = new System.Drawing.Size(84, 54);
            this.Panel25.TabIndex = 46;
            // 
            // btnPaso
            // 
            this.btnPaso.BackColor = System.Drawing.Color.LightGray;
            this.btnPaso.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPaso.Location = new System.Drawing.Point(8, 5);
            this.btnPaso.Name = "btnPaso";
            this.btnPaso.Size = new System.Drawing.Size(66, 28);
            this.btnPaso.TabIndex = 46;
            this.btnPaso.Text = "Paso";
            this.btnPaso.UseVisualStyleBackColor = false;
            // 
            // chkPasoAPaso
            // 
            this.chkPasoAPaso.AutoSize = true;
            this.chkPasoAPaso.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPasoAPaso.Location = new System.Drawing.Point(1, 34);
            this.chkPasoAPaso.Name = "chkPasoAPaso";
            this.chkPasoAPaso.Size = new System.Drawing.Size(85, 17);
            this.chkPasoAPaso.TabIndex = 47;
            this.chkPasoAPaso.Text = "Paso a paso";
            // 
            // btnRearme
            // 
            this.btnRearme.BackColor = System.Drawing.Color.LightGray;
            this.btnRearme.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRearme.Location = new System.Drawing.Point(-1, 251);
            this.btnRearme.Name = "btnRearme";
            this.btnRearme.Size = new System.Drawing.Size(85, 35);
            this.btnRearme.TabIndex = 42;
            this.btnRearme.Text = "Rearme";
            this.btnRearme.UseVisualStyleBackColor = false;
            this.btnRearme.Click += new System.EventHandler(this.ActionQueryHandler);
            // 
            // lblCycleTime
            // 
            this.lblCycleTime.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCycleTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCycleTime.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCycleTime.ForeColor = System.Drawing.Color.Black;
            this.lblCycleTime.Location = new System.Drawing.Point(11, 0);
            this.lblCycleTime.Name = "lblCycleTime";
            this.lblCycleTime.Size = new System.Drawing.Size(63, 24);
            this.lblCycleTime.TabIndex = 14;
            this.lblCycleTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCycleTime.Click += new System.EventHandler(this.lblCycleTime_Click);
            // 
            // pnlMaquina
            // 
            this.pnlMaquina.BackColor = System.Drawing.Color.Cornsilk;
            this.pnlMaquina.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlMaquina.Controls.Add(this.PictureBox1);
            this.pnlMaquina.Controls.Add(this.lblConexionMando);
            this.pnlMaquina.Controls.Add(this.lblDiagnosis);
            this.pnlMaquina.Controls.Add(this.lblProteccionesAnuladas);
            this.pnlMaquina.Controls.Add(this.lblProteccionesOk);
            this.pnlMaquina.Controls.Add(this.lblAireOk);
            this.pnlMaquina.Controls.Add(this.lblOrigen);
            this.pnlMaquina.Controls.Add(this.lblSistemaArranque);
            this.pnlMaquina.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMaquina.Location = new System.Drawing.Point(0, 0);
            this.pnlMaquina.Name = "pnlMaquina";
            this.pnlMaquina.Size = new System.Drawing.Size(940, 72);
            this.pnlMaquina.TabIndex = 34;
            // 
            // PictureBox1
            // 
            this.PictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox1.Image")));
            this.PictureBox1.Location = new System.Drawing.Point(0, 0);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(136, 72);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBox1.TabIndex = 15;
            this.PictureBox1.TabStop = false;
            // 
            // lblConexionMando
            // 
            this.lblConexionMando.BackColor = System.Drawing.Color.LightGray;
            this.lblConexionMando.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblConexionMando.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConexionMando.ForeColor = System.Drawing.Color.White;
            this.lblConexionMando.Location = new System.Drawing.Point(356, 9);
            this.lblConexionMando.Name = "lblConexionMando";
            this.lblConexionMando.Size = new System.Drawing.Size(118, 50);
            this.lblConexionMando.TabIndex = 9;
            this.lblConexionMando.Text = "Conexion Mando";
            this.lblConexionMando.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDiagnosis
            // 
            this.lblDiagnosis.BackColor = System.Drawing.Color.LightGray;
            this.lblDiagnosis.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDiagnosis.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiagnosis.ForeColor = System.Drawing.Color.White;
            this.lblDiagnosis.Location = new System.Drawing.Point(839, 9);
            this.lblDiagnosis.Name = "lblDiagnosis";
            this.lblDiagnosis.Size = new System.Drawing.Size(95, 50);
            this.lblDiagnosis.TabIndex = 13;
            this.lblDiagnosis.Text = "Diagnosis";
            this.lblDiagnosis.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProteccionesAnuladas
            // 
            this.lblProteccionesAnuladas.BackColor = System.Drawing.Color.LightGray;
            this.lblProteccionesAnuladas.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblProteccionesAnuladas.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProteccionesAnuladas.ForeColor = System.Drawing.Color.White;
            this.lblProteccionesAnuladas.Location = new System.Drawing.Point(703, 9);
            this.lblProteccionesAnuladas.Name = "lblProteccionesAnuladas";
            this.lblProteccionesAnuladas.Size = new System.Drawing.Size(136, 50);
            this.lblProteccionesAnuladas.TabIndex = 12;
            this.lblProteccionesAnuladas.Text = "Protecciones Anuladas";
            this.lblProteccionesAnuladas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProteccionesOk
            // 
            this.lblProteccionesOk.BackColor = System.Drawing.Color.LightGray;
            this.lblProteccionesOk.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblProteccionesOk.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProteccionesOk.ForeColor = System.Drawing.Color.White;
            this.lblProteccionesOk.Location = new System.Drawing.Point(580, 9);
            this.lblProteccionesOk.Name = "lblProteccionesOk";
            this.lblProteccionesOk.Size = new System.Drawing.Size(123, 50);
            this.lblProteccionesOk.TabIndex = 11;
            this.lblProteccionesOk.Text = "Protecciones Ok";
            this.lblProteccionesOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAireOk
            // 
            this.lblAireOk.BackColor = System.Drawing.Color.LightGray;
            this.lblAireOk.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblAireOk.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAireOk.ForeColor = System.Drawing.Color.White;
            this.lblAireOk.Location = new System.Drawing.Point(474, 9);
            this.lblAireOk.Name = "lblAireOk";
            this.lblAireOk.Size = new System.Drawing.Size(106, 50);
            this.lblAireOk.TabIndex = 10;
            this.lblAireOk.Text = "Aire Ok";
            this.lblAireOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOrigen
            // 
            this.lblOrigen.BackColor = System.Drawing.Color.LightGray;
            this.lblOrigen.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblOrigen.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrigen.ForeColor = System.Drawing.Color.White;
            this.lblOrigen.Location = new System.Drawing.Point(250, 9);
            this.lblOrigen.Name = "lblOrigen";
            this.lblOrigen.Size = new System.Drawing.Size(106, 50);
            this.lblOrigen.TabIndex = 8;
            this.lblOrigen.Text = "Origen";
            this.lblOrigen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSistemaArranque
            // 
            this.lblSistemaArranque.BackColor = System.Drawing.Color.LightGray;
            this.lblSistemaArranque.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSistemaArranque.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSistemaArranque.ForeColor = System.Drawing.Color.White;
            this.lblSistemaArranque.Location = new System.Drawing.Point(138, 9);
            this.lblSistemaArranque.Name = "lblSistemaArranque";
            this.lblSistemaArranque.Size = new System.Drawing.Size(112, 50);
            this.lblSistemaArranque.TabIndex = 7;
            this.lblSistemaArranque.Text = "Sistema Arranque";
            this.lblSistemaArranque.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ColumnHeader3
            // 
            this.ColumnHeader3.Text = "Nro. Error";
            this.ColumnHeader3.Width = 400;
            // 
            // ColumnHeader4
            // 
            this.ColumnHeader4.Text = "Descripción";
            this.ColumnHeader4.Width = 1090;
            // 
            // lstDiagnosis
            // 
            this.lstDiagnosis.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lstDiagnosis.AutoArrange = false;
            this.lstDiagnosis.BackColor = System.Drawing.Color.LightSalmon;
            this.lstDiagnosis.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader3,
            this.ColumnHeader4});
            this.lstDiagnosis.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lstDiagnosis.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstDiagnosis.FullRowSelect = true;
            this.lstDiagnosis.GridLines = true;
            this.lstDiagnosis.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstDiagnosis.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.lstDiagnosis.Location = new System.Drawing.Point(0, 825);
            this.lstDiagnosis.MultiSelect = false;
            this.lstDiagnosis.Name = "lstDiagnosis";
            this.lstDiagnosis.Scrollable = false;
            this.lstDiagnosis.Size = new System.Drawing.Size(940, 19);
            this.lstDiagnosis.TabIndex = 31;
            this.lstDiagnosis.UseCompatibleStateImageBehavior = false;
            this.lstDiagnosis.View = System.Windows.Forms.View.Details;
            // 
            // CatalogoDBBindingSource
            // 
            this.CatalogoDBBindingSource.DataMember = "CatalogoDB";
            // 
            // TabSystem
            // 
            this.TabSystem.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.TabSystem.Controls.Add(this.TabPage6);
            this.TabSystem.Controls.Add(this.TabPage1);
            this.TabSystem.Controls.Add(this.TabPage2);
            this.TabSystem.Controls.Add(this.TabPage3);
            this.TabSystem.Controls.Add(this.TabPage4);
            this.TabSystem.Controls.Add(this.TabPage5);
            this.TabSystem.Controls.Add(this.tabPage7);
            this.TabSystem.Controls.Add(this.tabPage8);
            this.TabSystem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabSystem.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabSystem.ItemSize = new System.Drawing.Size(80, 90);
            this.TabSystem.Location = new System.Drawing.Point(0, 72);
            this.TabSystem.Multiline = true;
            this.TabSystem.Name = "TabSystem";
            this.TabSystem.SelectedIndex = 0;
            this.TabSystem.Size = new System.Drawing.Size(940, 772);
            this.TabSystem.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.TabSystem.TabIndex = 35;
            this.TabSystem.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.TabSystem_DrawItem);
            this.TabSystem.SelectedIndexChanged += new System.EventHandler(this.TabSystem_SelectedIndexChanged);
            // 
            // TabPage6
            // 
            this.TabPage6.Controls.Add(this.panel9);
            this.TabPage6.Location = new System.Drawing.Point(94, 4);
            this.TabPage6.Name = "TabPage6";
            this.TabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage6.Size = new System.Drawing.Size(842, 764);
            this.TabPage6.TabIndex = 5;
            this.TabPage6.Text = "Datos Inicio";
            this.TabPage6.UseVisualStyleBackColor = true;
            // 
            // panel9
            // 
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(3, 3);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(836, 758);
            this.panel9.TabIndex = 2;
            // 
            // TabPage1
            // 
            this.TabPage1.Controls.Add(this.PnlPaletizadoLine1);
            this.TabPage1.Location = new System.Drawing.Point(94, 4);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage1.Size = new System.Drawing.Size(842, 764);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "Linea 1";
            this.TabPage1.UseVisualStyleBackColor = true;
            // 
            // PnlPaletizadoLine1
            // 
            this.PnlPaletizadoLine1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlPaletizadoLine1.Location = new System.Drawing.Point(3, 3);
            this.PnlPaletizadoLine1.Name = "PnlPaletizadoLine1";
            this.PnlPaletizadoLine1.Size = new System.Drawing.Size(836, 758);
            this.PnlPaletizadoLine1.TabIndex = 0;
            // 
            // TabPage2
            // 
            this.TabPage2.AutoScroll = true;
            this.TabPage2.Controls.Add(this.PnlPaletizadoLine2);
            this.TabPage2.Location = new System.Drawing.Point(94, 4);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage2.Size = new System.Drawing.Size(842, 764);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "Linea 2";
            this.TabPage2.UseVisualStyleBackColor = true;
            // 
            // PnlPaletizadoLine2
            // 
            this.PnlPaletizadoLine2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlPaletizadoLine2.Location = new System.Drawing.Point(3, 3);
            this.PnlPaletizadoLine2.Name = "PnlPaletizadoLine2";
            this.PnlPaletizadoLine2.Size = new System.Drawing.Size(836, 758);
            this.PnlPaletizadoLine2.TabIndex = 0;
            // 
            // TabPage3
            // 
            this.TabPage3.Controls.Add(this.PnlTransports);
            this.TabPage3.Location = new System.Drawing.Point(94, 4);
            this.TabPage3.Name = "TabPage3";
            this.TabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage3.Size = new System.Drawing.Size(842, 764);
            this.TabPage3.TabIndex = 2;
            this.TabPage3.Text = "Transporte";
            this.TabPage3.UseVisualStyleBackColor = true;
            // 
            // PnlTransports
            // 
            this.PnlTransports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlTransports.Location = new System.Drawing.Point(3, 3);
            this.PnlTransports.Name = "PnlTransports";
            this.PnlTransports.Size = new System.Drawing.Size(836, 758);
            this.PnlTransports.TabIndex = 0;
            // 
            // TabPage4
            // 
            this.TabPage4.Controls.Add(this.pnlMovManual);
            this.TabPage4.Controls.Add(this.treeManual);
            this.TabPage4.Location = new System.Drawing.Point(94, 4);
            this.TabPage4.Name = "TabPage4";
            this.TabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage4.Size = new System.Drawing.Size(842, 764);
            this.TabPage4.TabIndex = 3;
            this.TabPage4.Text = "Mov. Manual";
            this.TabPage4.UseVisualStyleBackColor = true;
            // 
            // pnlMovManual
            // 
            this.pnlMovManual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMovManual.Location = new System.Drawing.Point(187, 3);
            this.pnlMovManual.Name = "pnlMovManual";
            this.pnlMovManual.Size = new System.Drawing.Size(652, 758);
            this.pnlMovManual.TabIndex = 1;
            // 
            // treeManual
            // 
            this.treeManual.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeManual.Location = new System.Drawing.Point(3, 3);
            this.treeManual.Name = "treeManual";
            this.treeManual.Size = new System.Drawing.Size(184, 758);
            this.treeManual.TabIndex = 0;
            // 
            // TabPage5
            // 
            this.TabPage5.Controls.Add(this.panel7);
            this.TabPage5.Location = new System.Drawing.Point(94, 4);
            this.TabPage5.Name = "TabPage5";
            this.TabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage5.Size = new System.Drawing.Size(842, 764);
            this.TabPage5.TabIndex = 4;
            this.TabPage5.Text = "Validado Manual";
            this.TabPage5.UseVisualStyleBackColor = true;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.panel5);
            this.panel7.Controls.Add(this.panel4);
            this.panel7.Controls.Add(this.panel3);
            this.panel7.Controls.Add(this.panel2);
            this.panel7.Controls.Add(this.panel1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(836, 758);
            this.panel7.TabIndex = 4;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.manualBoxReprocessor);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(37, 10);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(771, 718);
            this.panel5.TabIndex = 4;
            // 
            // manualBoxReprocessor
            // 
            this.manualBoxReprocessor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.manualBoxReprocessor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.manualBoxReprocessor.Location = new System.Drawing.Point(158, 54);
            this.manualBoxReprocessor.Margin = new System.Windows.Forms.Padding(4);
            this.manualBoxReprocessor.Name = "manualBoxReprocessor";
            this.manualBoxReprocessor.Size = new System.Drawing.Size(427, 482);
            this.manualBoxReprocessor.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(808, 10);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(28, 718);
            this.panel4.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 10);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(37, 718);
            this.panel3.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 728);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(836, 30);
            this.panel2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(836, 10);
            this.panel1.TabIndex = 0;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.pBandaProdec);
            this.tabPage7.Location = new System.Drawing.Point(94, 4);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(842, 764);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "Banda Prodec";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // pBandaProdec
            // 
            this.pBandaProdec.BackColor = System.Drawing.Color.White;
            this.pBandaProdec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pBandaProdec.Location = new System.Drawing.Point(0, 0);
            this.pBandaProdec.Name = "pBandaProdec";
            this.pBandaProdec.Size = new System.Drawing.Size(842, 764);
            this.pBandaProdec.TabIndex = 0;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.panel99);
            this.tabPage8.Location = new System.Drawing.Point(94, 4);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Size = new System.Drawing.Size(842, 764);
            this.tabPage8.TabIndex = 7;
            this.tabPage8.Text = "Informe";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // panel99
            // 
            this.panel99.Controls.Add(this.reportViewer1);
            this.panel99.Controls.Add(this.cbDia);
            this.panel99.Controls.Add(this.cbMes);
            this.panel99.Controls.Add(this.button1);
            this.panel99.Controls.Add(this.label5);
            this.panel99.Controls.Add(this.label4);
            this.panel99.Controls.Add(this.label3);
            this.panel99.Controls.Add(this.label2);
            this.panel99.Controls.Add(this.label1);
            this.panel99.Controls.Add(this.cbTurno);
            this.panel99.Controls.Add(this.tbAño);
            this.panel99.Controls.Add(this.tbcatalogoID);
            this.panel99.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel99.Location = new System.Drawing.Point(0, 0);
            this.panel99.Name = "panel99";
            this.panel99.Size = new System.Drawing.Size(842, 764);
            this.panel99.TabIndex = 3;
            // 
            // reportViewer1
            // 
            this.reportViewer1.AutoScroll = true;
            reportDataSource1.Name = "CatalogArray";
            reportDataSource1.Value = this.BindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Idpsa.Report1.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(4, 104);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(800, 469);
            this.reportViewer1.TabIndex = 36;
            // 
            // cbDia
            // 
            this.cbDia.AutoCompleteCustomSource.AddRange(new string[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.cbDia.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDia.FormattingEnabled = true;
            this.cbDia.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.cbDia.Location = new System.Drawing.Point(201, 39);
            this.cbDia.Name = "cbDia";
            this.cbDia.Size = new System.Drawing.Size(58, 24);
            this.cbDia.TabIndex = 35;
            // 
            // cbMes
            // 
            this.cbMes.AutoCompleteCustomSource.AddRange(new string[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.cbMes.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMes.FormattingEnabled = true;
            this.cbMes.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.cbMes.Location = new System.Drawing.Point(263, 39);
            this.cbMes.Name = "cbMes";
            this.cbMes.Size = new System.Drawing.Size(56, 24);
            this.cbMes.TabIndex = 34;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(609, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 36);
            this.button1.TabIndex = 33;
            this.button1.Text = "Informe";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(429, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 18);
            this.label5.TabIndex = 32;
            this.label5.Text = "Turno";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(342, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 18);
            this.label4.TabIndex = 31;
            this.label4.Text = "Año";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(267, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 18);
            this.label3.TabIndex = 30;
            this.label3.Text = "Mes";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(213, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 18);
            this.label2.TabIndex = 29;
            this.label2.Text = "Día";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 18);
            this.label1.TabIndex = 28;
            this.label1.Text = "Catalogo ID";
            // 
            // cbTurno
            // 
            this.cbTurno.AutoCompleteCustomSource.AddRange(new string[] {
            "Mañana",
            "Tarde",
            "Noche"});
            this.cbTurno.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTurno.FormattingEnabled = true;
            this.cbTurno.Items.AddRange(new object[] {
            "Mañana",
            "Tarde",
            "Noche"});
            this.cbTurno.Location = new System.Drawing.Point(433, 39);
            this.cbTurno.Name = "cbTurno";
            this.cbTurno.Size = new System.Drawing.Size(121, 24);
            this.cbTurno.TabIndex = 27;
            // 
            // tbAño
            // 
            this.tbAño.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbAño.Location = new System.Drawing.Point(325, 39);
            this.tbAño.Name = "tbAño";
            this.tbAño.Size = new System.Drawing.Size(87, 23);
            this.tbAño.TabIndex = 26;
            // 
            // tbcatalogoID
            // 
            this.tbcatalogoID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbcatalogoID.Location = new System.Drawing.Point(4, 39);
            this.tbcatalogoID.Name = "tbcatalogoID";
            this.tbcatalogoID.Size = new System.Drawing.Size(171, 23);
            this.tbcatalogoID.TabIndex = 25;
            // 
            // datosTurnoBindingSource
            // 
            this.datosTurnoBindingSource.DataSource = typeof(Idpsa.datosTurno);
            // 
            // listAlarmaInfo
            // 
            this.listAlarmaInfo.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listAlarmaInfo.AutoArrange = false;
            this.listAlarmaInfo.BackColor = System.Drawing.Color.LightSalmon;
            this.listAlarmaInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listAlarmaInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listAlarmaInfo.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listAlarmaInfo.FullRowSelect = true;
            this.listAlarmaInfo.GridLines = true;
            this.listAlarmaInfo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listAlarmaInfo.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem2});
            this.listAlarmaInfo.Location = new System.Drawing.Point(0, 806);
            this.listAlarmaInfo.MultiSelect = false;
            this.listAlarmaInfo.Name = "listAlarmaInfo";
            this.listAlarmaInfo.Scrollable = false;
            this.listAlarmaInfo.Size = new System.Drawing.Size(940, 19);
            this.listAlarmaInfo.TabIndex = 36;
            this.listAlarmaInfo.UseCompatibleStateImageBehavior = false;
            this.listAlarmaInfo.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Nro. Error";
            this.columnHeader1.Width = 400;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Descripción";
            this.columnHeader2.Width = 1090;
            // 
            // FormMainPaletizado
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1028, 844);
            this.Controls.Add(this.listAlarmaInfo);
            this.Controls.Add(this.lstDiagnosis);
            this.Controls.Add(this.TabSystem);
            this.Controls.Add(this.pnlMaquina);
            this.Controls.Add(this.pnlModo);
            this.MaximizeBox = false;
            this.Menu = this.mnPasaportes;
            this.Name = "FormMainPaletizado";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RCM";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmPasaportes_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmEncartonadora_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource)).EndInit();
            this.pnlModo.ResumeLayout(false);
            this.pnlRearmePasoAPaso.ResumeLayout(false);
            this.pnlRearmePasoAPaso.PerformLayout();
            this.Panel25.ResumeLayout(false);
            this.Panel25.PerformLayout();
            this.pnlMaquina.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CatalogoDBBindingSource)).EndInit();
            this.TabSystem.ResumeLayout(false);
            this.TabPage6.ResumeLayout(false);
            this.TabPage1.ResumeLayout(false);
            this.TabPage2.ResumeLayout(false);
            this.TabPage3.ResumeLayout(false);
            this.TabPage4.ResumeLayout(false);
            this.TabPage5.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.panel99.ResumeLayout(false);
            this.panel99.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datosTurnoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Dibujado sobre formulario
        private void TabSystem_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            TabPage CurrentTab = TabSystem.TabPages[e.Index];
            Rectangle ItemRect = TabSystem.GetTabRect(e.Index);
            SolidBrush FillBrush = new SolidBrush(Color.LightGray);
            SolidBrush TextBrush = new SolidBrush(Color.Black);
            StringFormat sf = new StringFormat();
            System.Drawing.Font font = TabSystem.Font;
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            if ((e.State & DrawItemState.Selected) != 0)
            {
                FillBrush.Color = Color.GreenYellow;
                TextBrush.Color = Color.Black;
                font = new Font(font.FontFamily, 17);
            }
            e.Graphics.FillRectangle(FillBrush, ItemRect);
            e.Graphics.DrawString(TabSystem.TabPages[e.Index].Text, font, TextBrush, ItemRect, sf);
        }
        #endregion


        internal System.Windows.Forms.Button btnManual;
        internal System.Windows.Forms.Label lblModoActual;
        internal System.Windows.Forms.Button btnVOrig;
        internal System.Windows.Forms.Button btnAuto;
        internal System.Windows.Forms.Button btnActiveModeStart;
        internal System.Windows.Forms.Button btnActiveModeStop;
        internal System.Windows.Forms.Panel pnlModo;
        internal System.Windows.Forms.Panel pnlMaquina;
        internal System.Windows.Forms.PictureBox PictureBox1;
        internal System.Windows.Forms.Label lblConexionMando;
        internal System.Windows.Forms.Label lblCycleTime;
        internal System.Windows.Forms.Label lblDiagnosis;
        internal System.Windows.Forms.Label lblProteccionesAnuladas;
        internal System.Windows.Forms.Label lblProteccionesOk;
        internal System.Windows.Forms.Label lblAireOk;
        internal System.Windows.Forms.Label lblOrigen;
        internal System.Windows.Forms.Label lblSistemaArranque;
        internal System.Windows.Forms.Button btnRearme;
        internal System.Windows.Forms.Panel Panel25;
        internal System.Windows.Forms.Button btnPaso;
        internal System.Windows.Forms.CheckBox chkPasoAPaso;
        internal TabControlEx TabSystem;
        internal System.Windows.Forms.TabPage TabPage2;
        internal System.Windows.Forms.TabPage TabPage3;
        internal System.Windows.Forms.TabPage TabPage4;
        internal System.Windows.Forms.TabPage TabPage5;
        internal System.Windows.Forms.TabPage TabPage6;
        internal System.Windows.Forms.TreeView treeManual;
        internal System.Windows.Forms.Panel pnlMovManual;
        internal System.Windows.Forms.MenuItem menuPruebas;
        internal System.Windows.Forms.TabPage TabPage1;
        internal System.Windows.Forms.Panel PnlPaletizadoLine1;
        internal System.Windows.Forms.Panel PnlPaletizadoLine2;
        internal System.Windows.Forms.Panel PnlTransports;
        private Panel pnlRearmePasoAPaso;
        private Panel panel7;
        private Panel panel4;
        private Panel panel3;
        private Panel panel2;
        private Panel panel1;
        private Panel panel5;
        //private TabControl PnlTransports;
        private MenuItem menuNuevo;
        private MenuItem menuPasaporte;
        private MenuItem menuCatalogo;
        private Panel panel9;
        private Panel panel99;
        private MenuItem menuItem1;
        private Button btVaciar;
        private MenuItem menuImpresion;
        private Idpsa.Paletizado.ManualBoxReprocessor manualBoxReprocessor;
        private MenuItem menuGuardar;
        private MenuItem menuGuradarCatalogos;
        private MenuItem menuItemRefreshForm;
        private MenuItem menuGuardarGruposTransporte;
        private MenuItem menuCargar;
        private MenuItem menuCargarGruposTransporte;
        private Button btModoAcumulacion;
        private MenuItem menuGuardarCajasBandas;
        private MenuItem menuCargarCajasBandas;
        internal Label labelVaciar;
        private MenuItem menuItem2;
        private CheckBox checkBoxAutoSemaforo;
        internal ColumnHeader ColumnHeader3;
        internal ColumnHeader ColumnHeader4;
        internal ListView lstDiagnosis;
        private Button btEntradaManual0;
        private MenuItem menuItem3;
        private MenuItem saveAll;
        private TabPage tabPage7;
        private Panel pBandaProdec;
        private TabPage tabPage8;
        private ComboBox cbDia;
        private ComboBox cbMes;
        private Button button1;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private ComboBox cbTurno;
        private TextBox tbAño;
        private TextBox tbcatalogoID;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private BindingSource BindingSource;
        private BindingSource CatalogoDBBindingSource;
        private BindingSource datosTurnoBindingSource;
        internal ListView listAlarmaInfo;
        internal ColumnHeader columnHeader1;
        internal ColumnHeader columnHeader2;


    }
}