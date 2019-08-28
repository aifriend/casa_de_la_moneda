using System.Windows.Forms;

namespace CatalogFactory
{
    partial class frmCreateRobotEnlaceCatalog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCreateRobotEnlaceCatalog));
            this.gbDataCatalog = new System.Windows.Forms.GroupBox();
            this.tbCategoria = new System.Windows.Forms.TextBox();
            this.tbGrosor = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbPais = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbPeso = new System.Windows.Forms.TextBox();
            this.tbTieneRfid = new System.Windows.Forms.TextBox();
            this.tbNDigitos = new System.Windows.Forms.TextBox();
            this.tbNLetras = new System.Windows.Forms.TextBox();
            this.tbPasaporteFinal = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.fechaLaminacion = new System.Windows.Forms.TextBox();
            this.Mes = new System.Windows.Forms.ComboBox();
            this.labelMes = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Anho = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPasaporteInicial = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelAnho = new System.Windows.Forms.Label();
            this.labelFechaLaminacion = new System.Windows.Forms.Label();
            this.cbTiposPasaporte = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btCancelar = new System.Windows.Forms.Button();
            this.btGuardar = new System.Windows.Forms.Button();
            this.btCerrar = new System.Windows.Forms.Button();
            this.gbDataCatalog.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDataCatalog
            // 
            this.gbDataCatalog.Controls.Add(this.tbCategoria);
            this.gbDataCatalog.Controls.Add(this.tbGrosor);
            this.gbDataCatalog.Controls.Add(this.label11);
            this.gbDataCatalog.Controls.Add(this.tbPais);
            this.gbDataCatalog.Controls.Add(this.label10);
            this.gbDataCatalog.Controls.Add(this.tbPeso);
            this.gbDataCatalog.Controls.Add(this.tbTieneRfid);
            this.gbDataCatalog.Controls.Add(this.tbNDigitos);
            this.gbDataCatalog.Controls.Add(this.tbNLetras);
            this.gbDataCatalog.Controls.Add(this.tbPasaporteFinal);
            this.gbDataCatalog.Controls.Add(this.label5);
            this.gbDataCatalog.Controls.Add(this.fechaLaminacion);
            this.gbDataCatalog.Controls.Add(this.Mes);
            this.gbDataCatalog.Controls.Add(this.labelMes);
            this.gbDataCatalog.Controls.Add(this.label7);
            this.gbDataCatalog.Controls.Add(this.label6);
            this.gbDataCatalog.Controls.Add(this.label4);
            this.gbDataCatalog.Controls.Add(this.Anho);
            this.gbDataCatalog.Controls.Add(this.label3);
            this.gbDataCatalog.Controls.Add(this.label2);
            this.gbDataCatalog.Controls.Add(this.tbPasaporteInicial);
            this.gbDataCatalog.Controls.Add(this.label1);
            this.gbDataCatalog.Controls.Add(this.labelAnho);
            this.gbDataCatalog.Controls.Add(this.labelFechaLaminacion);
            this.gbDataCatalog.Location = new System.Drawing.Point(17, 80);
            this.gbDataCatalog.Margin = new System.Windows.Forms.Padding(5);
            this.gbDataCatalog.Name = "gbDataCatalog";
            this.gbDataCatalog.Padding = new System.Windows.Forms.Padding(5);
            this.gbDataCatalog.Size = new System.Drawing.Size(670, 437);
            this.gbDataCatalog.TabIndex = 26;
            this.gbDataCatalog.TabStop = false;
            this.gbDataCatalog.Text = "Datos del catálogo actual";
            // 
            // tbCategoria
            // 
            this.tbCategoria.BackColor = System.Drawing.Color.GhostWhite;
            this.tbCategoria.Enabled = false;
            this.tbCategoria.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCategoria.Location = new System.Drawing.Point(244, 58);
            this.tbCategoria.Margin = new System.Windows.Forms.Padding(5);
            this.tbCategoria.Name = "tbCategoria";
            this.tbCategoria.Size = new System.Drawing.Size(376, 22);
            this.tbCategoria.TabIndex = 38;
            // 
            // tbGrosor
            // 
            this.tbGrosor.BackColor = System.Drawing.Color.GhostWhite;
            this.tbGrosor.Enabled = false;
            this.tbGrosor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbGrosor.Location = new System.Drawing.Point(244, 339);
            this.tbGrosor.Margin = new System.Windows.Forms.Padding(5);
            this.tbGrosor.Name = "tbGrosor";
            this.tbGrosor.Size = new System.Drawing.Size(163, 22);
            this.tbGrosor.TabIndex = 37;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(36, 342);
            this.label11.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 16);
            this.label11.TabIndex = 36;
            this.label11.Text = "Grosor :";
            // 
            // tbPais
            // 
            this.tbPais.BackColor = System.Drawing.Color.GhostWhite;
            this.tbPais.Enabled = false;
            this.tbPais.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPais.Location = new System.Drawing.Point(244, 26);
            this.tbPais.Margin = new System.Windows.Forms.Padding(5);
            this.tbPais.Name = "tbPais";
            this.tbPais.Size = new System.Drawing.Size(376, 22);
            this.tbPais.TabIndex = 33;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(36, 31);
            this.label10.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 16);
            this.label10.TabIndex = 32;
            this.label10.Text = "País :";
            // 
            // tbPeso
            // 
            this.tbPeso.BackColor = System.Drawing.Color.GhostWhite;
            this.tbPeso.Enabled = false;
            this.tbPeso.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPeso.Location = new System.Drawing.Point(244, 307);
            this.tbPeso.Margin = new System.Windows.Forms.Padding(5);
            this.tbPeso.Name = "tbPeso";
            this.tbPeso.Size = new System.Drawing.Size(213, 22);
            this.tbPeso.TabIndex = 30;
            // 
            // tbTieneRfid
            // 
            this.tbTieneRfid.BackColor = System.Drawing.Color.GhostWhite;
            this.tbTieneRfid.Enabled = false;
            this.tbTieneRfid.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbTieneRfid.Location = new System.Drawing.Point(244, 275);
            this.tbTieneRfid.Margin = new System.Windows.Forms.Padding(5);
            this.tbTieneRfid.Name = "tbTieneRfid";
            this.tbTieneRfid.Size = new System.Drawing.Size(163, 22);
            this.tbTieneRfid.TabIndex = 29;
            // 
            // tbNDigitos
            // 
            this.tbNDigitos.BackColor = System.Drawing.Color.GhostWhite;
            this.tbNDigitos.Enabled = false;
            this.tbNDigitos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbNDigitos.Location = new System.Drawing.Point(243, 221);
            this.tbNDigitos.Margin = new System.Windows.Forms.Padding(5);
            this.tbNDigitos.Name = "tbNDigitos";
            this.tbNDigitos.Size = new System.Drawing.Size(212, 22);
            this.tbNDigitos.TabIndex = 28;
            // 
            // tbNLetras
            // 
            this.tbNLetras.BackColor = System.Drawing.Color.GhostWhite;
            this.tbNLetras.Enabled = false;
            this.tbNLetras.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbNLetras.Location = new System.Drawing.Point(244, 189);
            this.tbNLetras.Margin = new System.Windows.Forms.Padding(5);
            this.tbNLetras.Name = "tbNLetras";
            this.tbNLetras.Size = new System.Drawing.Size(211, 22);
            this.tbNLetras.TabIndex = 27;
            // 
            // tbPasaporteFinal
            // 
            this.tbPasaporteFinal.BackColor = System.Drawing.Color.White;
            this.tbPasaporteFinal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPasaporteFinal.Location = new System.Drawing.Point(244, 139);
            this.tbPasaporteFinal.Margin = new System.Windows.Forms.Padding(5);
            this.tbPasaporteFinal.Name = "tbPasaporteFinal";
            this.tbPasaporteFinal.Size = new System.Drawing.Size(280, 22);
            this.tbPasaporteFinal.TabIndex = 26;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(36, 140);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 16);
            this.label5.TabIndex = 25;
            this.label5.Text = "Pasaporte Final :";
            // 
            // fechaLaminacion
            // 
            this.fechaLaminacion.BackColor = System.Drawing.Color.White;
            this.fechaLaminacion.Enabled = false;
            this.fechaLaminacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fechaLaminacion.Location = new System.Drawing.Point(244, 385);
            this.fechaLaminacion.Name = "fechaLaminacion";
            this.fechaLaminacion.Size = new System.Drawing.Size(163, 22);
            this.fechaLaminacion.TabIndex = 12;
            this.fechaLaminacion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Mes
            // 
            this.Mes.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Mes.FormattingEnabled = true;
            this.Mes.ItemHeight = 24;
            this.Mes.Location = new System.Drawing.Point(467, 383);
            this.Mes.Name = "Mes";
            this.Mes.Size = new System.Drawing.Size(53, 32);
            this.Mes.TabIndex = 13;
            this.Mes.SelectedIndexChanged += new System.EventHandler(this.Mes_SelectedIndexChanged);
            // 
            // labelMes
            // 
            this.labelMes.Location = new System.Drawing.Point(474, 367);
            this.labelMes.Name = "labelMes";
            this.labelMes.Size = new System.Drawing.Size(40, 30);
            this.labelMes.TabIndex = 15;
            this.labelMes.Text = "Mes";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(36, 308);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 16);
            this.label7.TabIndex = 24;
            this.label7.Text = "Peso (gramos):";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(36, 278);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 16);
            this.label6.TabIndex = 23;
            this.label6.Text = "RfID :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 222);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 16);
            this.label4.TabIndex = 22;
            this.label4.Text = "Número dígitos :";
            // 
            // Anho
            // 
            this.Anho.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Anho.FormattingEnabled = true;
            this.Anho.ItemHeight = 24;
            this.Anho.Location = new System.Drawing.Point(563, 383);
            this.Anho.Name = "Anho";
            this.Anho.Size = new System.Drawing.Size(80, 32);
            this.Anho.TabIndex = 13;
            this.Anho.SelectedIndexChanged += new System.EventHandler(this.Anho_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 193);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 16);
            this.label3.TabIndex = 21;
            this.label3.Text = "Número letras :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 62);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 16);
            this.label2.TabIndex = 20;
            this.label2.Text = "Categoría :";
            // 
            // tbPasaporteInicial
            // 
            this.tbPasaporteInicial.BackColor = System.Drawing.Color.White;
            this.tbPasaporteInicial.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPasaporteInicial.Location = new System.Drawing.Point(244, 107);
            this.tbPasaporteInicial.Margin = new System.Windows.Forms.Padding(5);
            this.tbPasaporteInicial.Name = "tbPasaporteInicial";
            this.tbPasaporteInicial.Size = new System.Drawing.Size(280, 22);
            this.tbPasaporteInicial.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 111);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 16);
            this.label1.TabIndex = 18;
            this.label1.Text = "Pasaporte Inicial :";
            // 
            // labelAnho
            // 
            this.labelAnho.Location = new System.Drawing.Point(585, 367);
            this.labelAnho.Name = "labelAnho";
            this.labelAnho.Size = new System.Drawing.Size(40, 38);
            this.labelAnho.TabIndex = 15;
            this.labelAnho.Text = "Año";
            // 
            // labelFechaLaminacion
            // 
            this.labelFechaLaminacion.Location = new System.Drawing.Point(36, 388);
            this.labelFechaLaminacion.Name = "labelFechaLaminacion";
            this.labelFechaLaminacion.Size = new System.Drawing.Size(142, 20);
            this.labelFechaLaminacion.TabIndex = 15;
            this.labelFechaLaminacion.Text = "Fecha de Laminación:";
            // 
            // cbTiposPasaporte
            // 
            this.cbTiposPasaporte.BackColor = System.Drawing.Color.GhostWhite;
            this.cbTiposPasaporte.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTiposPasaporte.FormattingEnabled = true;
            this.cbTiposPasaporte.Location = new System.Drawing.Point(261, 27);
            this.cbTiposPasaporte.Margin = new System.Windows.Forms.Padding(4);
            this.cbTiposPasaporte.Name = "cbTiposPasaporte";
            this.cbTiposPasaporte.Size = new System.Drawing.Size(426, 24);
            this.cbTiposPasaporte.TabIndex = 27;
            this.cbTiposPasaporte.SelectedIndexChanged += new System.EventHandler(this.cbTiposPasaporte_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(51, 33);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(137, 16);
            this.label9.TabIndex = 28;
            this.label9.Text = "Tipos de  Pasaporte :";
            // 
            // btCancelar
            // 
            this.btCancelar.Image = ((System.Drawing.Image)(resources.GetObject("btCancelar.Image")));
            this.btCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btCancelar.Location = new System.Drawing.Point(477, 535);
            this.btCancelar.Margin = new System.Windows.Forms.Padding(5);
            this.btCancelar.Name = "btCancelar";
            this.btCancelar.Size = new System.Drawing.Size(98, 34);
            this.btCancelar.TabIndex = 30;
            this.btCancelar.Text = "Cancelar";
            this.btCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btCancelar.UseVisualStyleBackColor = true;
            this.btCancelar.Click += new System.EventHandler(this.btCancelar_Click);
            // 
            // btGuardar
            // 
            this.btGuardar.Image = ((System.Drawing.Image)(resources.GetObject("btGuardar.Image")));
            this.btGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btGuardar.Location = new System.Drawing.Point(594, 535);
            this.btGuardar.Margin = new System.Windows.Forms.Padding(5);
            this.btGuardar.Name = "btGuardar";
            this.btGuardar.Size = new System.Drawing.Size(93, 34);
            this.btGuardar.TabIndex = 29;
            this.btGuardar.Text = "Guardar";
            this.btGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btGuardar.UseVisualStyleBackColor = true;
            this.btGuardar.Click += new System.EventHandler(this.btGuardar_Click);
            // 
            // btCerrar
            // 
            this.btCerrar.Image = ((System.Drawing.Image)(resources.GetObject("btCerrar.Image")));
            this.btCerrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btCerrar.Location = new System.Drawing.Point(325, 535);
            this.btCerrar.Margin = new System.Windows.Forms.Padding(5);
            this.btCerrar.Name = "btCerrar";
            this.btCerrar.Size = new System.Drawing.Size(86, 34);
            this.btCerrar.TabIndex = 31;
            this.btCerrar.Text = "Cerrar";
            this.btCerrar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btCerrar.UseVisualStyleBackColor = true;
            this.btCerrar.Click += new System.EventHandler(this.btCerrar_Click);
            // 
            // frmCreateRobotEnlaceCatalog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 584);
            this.Controls.Add(this.btCerrar);
            this.Controls.Add(this.btCancelar);
            this.Controls.Add(this.btGuardar);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cbTiposPasaporte);
            this.Controls.Add(this.gbDataCatalog);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmCreateRobotEnlaceCatalog";
            this.Text = "Editor de catálogos";
            this.Load += new System.EventHandler(this.frmCreateCatalog_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Closing);
            this.gbDataCatalog.ResumeLayout(false);
            this.gbDataCatalog.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDataCatalog;
        private System.Windows.Forms.TextBox tbPeso;
        private System.Windows.Forms.TextBox tbTieneRfid;
        private System.Windows.Forms.TextBox tbNDigitos;
        private System.Windows.Forms.TextBox tbNLetras;
        private System.Windows.Forms.TextBox tbPasaporteFinal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPasaporteInicial;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbTiposPasaporte;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.Button btGuardar;
        private System.Windows.Forms.Button btCerrar;
        private System.Windows.Forms.TextBox tbPais;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbGrosor;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbCategoria;
        private System.Windows.Forms.ComboBox Mes;
        private System.Windows.Forms.ComboBox Anho;
        private System.Windows.Forms.Label labelMes;
        private System.Windows.Forms.Label labelAnho;
        private System.Windows.Forms.Label labelFechaLaminacion;//MCR. 2011/03/03. Introduccion Fecha Laminacion.
        private System.Windows.Forms.TextBox fechaLaminacion; //MCR. 2011/03/03. Introduccion Fecha Laminacion.

    }
}