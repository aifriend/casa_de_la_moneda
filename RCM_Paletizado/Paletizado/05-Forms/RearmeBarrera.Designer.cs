namespace Idpsa
{
    partial class RearmeBarrera
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label employeeLabel;
            System.Windows.Forms.Label horaEntradaLabel;
            System.Windows.Forms.Label horaSalidaLabel;
            System.Windows.Forms.Label stateLabel;
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.operariosBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ParadaDBBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.BindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.reportViewer2 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.tryEmpleado = new System.Windows.Forms.Button();
            this.gbRearme = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.horaEntradaTextBox = new System.Windows.Forms.TextBox();
            this.stateTextBox = new System.Windows.Forms.TextBox();
            this.employeeTextBox = new System.Windows.Forms.TextBox();
            this.horaSalidaTextBox = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.reportViewer3 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.cbDia = new System.Windows.Forms.ComboBox();
            this.cbMes = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbAño = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.BindingSource = new System.Windows.Forms.BindingSource(this.components);
            employeeLabel = new System.Windows.Forms.Label();
            horaEntradaLabel = new System.Windows.Forms.Label();
            horaSalidaLabel = new System.Windows.Forms.Label();
            stateLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.operariosBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParadaDBBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource2)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.gbRearme.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // employeeLabel
            // 
            employeeLabel.AutoSize = true;
            employeeLabel.Location = new System.Drawing.Point(33, 32);
            employeeLabel.Name = "employeeLabel";
            employeeLabel.Size = new System.Drawing.Size(56, 13);
            employeeLabel.TabIndex = 1;
            employeeLabel.Text = "empleado:";
            // 
            // horaEntradaLabel
            // 
            horaEntradaLabel.AutoSize = true;
            horaEntradaLabel.Location = new System.Drawing.Point(33, 58);
            horaEntradaLabel.Name = "horaEntradaLabel";
            horaEntradaLabel.Size = new System.Drawing.Size(71, 13);
            horaEntradaLabel.TabIndex = 3;
            horaEntradaLabel.Text = "hora Entrada:";
            // 
            // horaSalidaLabel
            // 
            horaSalidaLabel.AutoSize = true;
            horaSalidaLabel.Location = new System.Drawing.Point(33, 84);
            horaSalidaLabel.Name = "horaSalidaLabel";
            horaSalidaLabel.Size = new System.Drawing.Size(63, 13);
            horaSalidaLabel.TabIndex = 5;
            horaSalidaLabel.Text = "hora Salida:";
            // 
            // stateLabel
            // 
            stateLabel.AutoSize = true;
            stateLabel.Location = new System.Drawing.Point(33, 110);
            stateLabel.Name = "stateLabel";
            stateLabel.Size = new System.Drawing.Size(42, 13);
            stateLabel.TabIndex = 7;
            stateLabel.Text = "estado:";
            // 
            // operariosBindingSource
            // 
            this.operariosBindingSource.DataSource = typeof(Idpsa.ParadaDB);
            // 
            // ParadaDBBindingSource
            // 
            this.ParadaDBBindingSource.DataSource = typeof(Idpsa.ParadaDB);
            // 
            // BindingSource2
            // 
            this.BindingSource2.DataSource = typeof(Idpsa.datosParada);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1006, 571);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(998, 545);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(992, 539);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.textBox1);
            this.panel3.Controls.Add(this.reportViewer2);
            this.panel3.Controls.Add(this.tryEmpleado);
            this.panel3.Controls.Add(this.gbRearme);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(992, 539);
            this.panel3.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.GhostWhite;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(120, 196);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(538, 13);
            this.textBox1.TabIndex = 12;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // reportViewer2
            // 
            reportDataSource1.Name = "RegistroES";
            reportDataSource1.Value = this.operariosBindingSource;
            this.reportViewer2.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer2.LocalReport.ReportEmbeddedResource = "Idpsa.RearmeES2.rdlc";
            this.reportViewer2.Location = new System.Drawing.Point(120, 245);
            this.reportViewer2.Name = "reportViewer2";
            this.reportViewer2.Size = new System.Drawing.Size(538, 250);
            this.reportViewer2.TabIndex = 11;
            // 
            // tryEmpleado
            // 
            this.tryEmpleado.Location = new System.Drawing.Point(708, 294);
            this.tryEmpleado.Name = "tryEmpleado";
            this.tryEmpleado.Size = new System.Drawing.Size(75, 23);
            this.tryEmpleado.TabIndex = 10;
            this.tryEmpleado.Text = "prueba";
            this.tryEmpleado.UseVisualStyleBackColor = true;
            tryEmpleado.Visible = false;
            this.tryEmpleado.Click += new System.EventHandler(this.tryEmpleado_Click);
            // 
            // gbRearme
            // 
            this.gbRearme.Controls.Add(this.button3);
            this.gbRearme.Controls.Add(this.button2);
            this.gbRearme.Controls.Add(this.horaEntradaTextBox);
            this.gbRearme.Controls.Add(employeeLabel);
            this.gbRearme.Controls.Add(this.stateTextBox);
            this.gbRearme.Controls.Add(this.employeeTextBox);
            this.gbRearme.Controls.Add(stateLabel);
            this.gbRearme.Controls.Add(horaEntradaLabel);
            this.gbRearme.Controls.Add(this.horaSalidaTextBox);
            this.gbRearme.Controls.Add(horaSalidaLabel);
            this.gbRearme.Location = new System.Drawing.Point(120, 27);
            this.gbRearme.Name = "gbRearme";
            this.gbRearme.Size = new System.Drawing.Size(538, 144);
            this.gbRearme.TabIndex = 9;
            this.gbRearme.TabStop = false;
            this.gbRearme.Text = "Rearme";
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(319, 84);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(144, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "Rearmar";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(319, 48);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(144, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Activar Rearme";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // horaEntradaTextBox
            // 
            this.horaEntradaTextBox.Enabled = false;
            this.horaEntradaTextBox.Location = new System.Drawing.Point(110, 55);
            this.horaEntradaTextBox.Name = "horaEntradaTextBox";
            this.horaEntradaTextBox.Size = new System.Drawing.Size(100, 20);
            this.horaEntradaTextBox.TabIndex = 4;
            // 
            // stateTextBox
            // 
            this.stateTextBox.Enabled = false;
            this.stateTextBox.Location = new System.Drawing.Point(110, 107);
            this.stateTextBox.Name = "stateTextBox";
            this.stateTextBox.Size = new System.Drawing.Size(100, 20);
            this.stateTextBox.TabIndex = 8;
            // 
            // employeeTextBox
            // 
            this.employeeTextBox.Enabled = false;
            this.employeeTextBox.Location = new System.Drawing.Point(110, 29);
            this.employeeTextBox.Name = "employeeTextBox";
            this.employeeTextBox.Size = new System.Drawing.Size(100, 20);
            this.employeeTextBox.TabIndex = 2;
            // 
            // horaSalidaTextBox
            // 
            this.horaSalidaTextBox.Enabled = false;
            this.horaSalidaTextBox.Location = new System.Drawing.Point(110, 81);
            this.horaSalidaTextBox.Name = "horaSalidaTextBox";
            this.horaSalidaTextBox.Size = new System.Drawing.Size(100, 20);
            this.horaSalidaTextBox.TabIndex = 6;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(998, 545);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.comboBox1);
            this.panel2.Controls.Add(this.reportViewer3);
            this.panel2.Controls.Add(this.reportViewer1);
            this.panel2.Controls.Add(this.cbDia);
            this.panel2.Controls.Add(this.cbMes);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.tbAño);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(992, 539);
            this.panel2.TabIndex = 0;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(648, 118);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(242, 21);
            this.comboBox1.TabIndex = 53;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // reportViewer3
            // 
            reportDataSource2.Name = "RegistroES";
            reportDataSource2.Value = this.ParadaDBBindingSource;
            this.reportViewer3.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer3.LocalReport.ReportEmbeddedResource = "Idpsa.RearmeES3.rdlc";
            this.reportViewer3.Location = new System.Drawing.Point(589, 202);
            this.reportViewer3.Name = "reportViewer3";
            this.reportViewer3.Size = new System.Drawing.Size(400, 250);
            this.reportViewer3.TabIndex = 52;
            // 
            // reportViewer1
            // 
            reportDataSource3.Name = "ParadaDB";
            reportDataSource3.Value = this.BindingSource2;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource3);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Idpsa.RearmeParada.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(93, 202);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(400, 250);
            this.reportViewer1.TabIndex = 51;
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
            this.cbDia.Location = new System.Drawing.Point(157, 25);
            this.cbDia.Name = "cbDia";
            this.cbDia.Size = new System.Drawing.Size(58, 24);
            this.cbDia.TabIndex = 50;
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
            this.cbMes.Location = new System.Drawing.Point(286, 25);
            this.cbMes.Name = "cbMes";
            this.cbMes.Size = new System.Drawing.Size(56, 24);
            this.cbMes.TabIndex = 49;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(369, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 18);
            this.label4.TabIndex = 48;
            this.label4.Text = "Año";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(243, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 18);
            this.label3.TabIndex = 47;
            this.label3.Text = "Mes";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(121, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 18);
            this.label2.TabIndex = 46;
            this.label2.Text = "Día";
            // 
            // tbAño
            // 
            this.tbAño.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbAño.Location = new System.Drawing.Point(409, 25);
            this.tbAño.Name = "tbAño";
            this.tbAño.Size = new System.Drawing.Size(87, 23);
            this.tbAño.TabIndex = 45;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(604, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 36);
            this.button1.TabIndex = 41;
            this.button1.Text = "Registro ES";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BindingSource
            // 
            this.BindingSource.DataSource = typeof(Idpsa.RegistroES);
            // 
            // RearmeBarrera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 572);
            this.Controls.Add(this.tabControl1);
            this.Name = "RearmeBarrera";
            this.Text = "RearmeBarrera";
            this.Load += new System.EventHandler(this.RearmeBarrera_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Closing);
            ((System.ComponentModel.ISupportInitialize)(this.operariosBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParadaDBBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource2)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.gbRearme.ResumeLayout(false);
            this.gbRearme.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.BindingSource ParadaDBBindingSource;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox employeeTextBox;
        private System.Windows.Forms.TextBox horaEntradaTextBox;
        private System.Windows.Forms.TextBox horaSalidaTextBox;
        private System.Windows.Forms.TextBox stateTextBox;
        private System.Windows.Forms.BindingSource operariosBindingSource;
        private System.Windows.Forms.GroupBox gbRearme;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button tryEmpleado;
        private System.Windows.Forms.ComboBox cbDia;
        private System.Windows.Forms.ComboBox cbMes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbAño;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource BindingSource2;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer2;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer3;
        private System.Windows.Forms.BindingSource BindingSource;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;
    }
}