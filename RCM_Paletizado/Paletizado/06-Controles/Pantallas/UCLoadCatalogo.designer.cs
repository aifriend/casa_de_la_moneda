namespace Idpsa
{
    partial class UCLoadCatalogo
    {
        /// <summary> 
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar 
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCLoadCatalogo));
            this.btAceptar = new System.Windows.Forms.Button();
            this.tbCatalogoActual = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.gbDataCatalog = new System.Windows.Forms.GroupBox();
            this.tbNuevoPeso = new System.Windows.Forms.TextBox();
            this.btnWeightModify = new System.Windows.Forms.Button();
            this.btVisualizar = new System.Windows.Forms.Button();
            this.tbPaletizado = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbCategoria = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbGrosor = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbPais = new System.Windows.Forms.TextBox();
            this.tbPeso = new System.Windows.Forms.TextBox();
            this.tbTieneRfid = new System.Windows.Forms.TextBox();
            this.tbNDigitos = new System.Windows.Forms.TextBox();
            this.tbNLetras = new System.Windows.Forms.TextBox();
            this.tbPasaporteFinal = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPasaporteInicial = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelFechaLaminacion = new System.Windows.Forms.Label();
            this.fechaLaminacion = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbCatalogs = new System.Windows.Forms.ComboBox();
            this.gbDataCatalog.SuspendLayout();
            this.SuspendLayout();
            // 
            // btAceptar
            // 
            this.btAceptar.Image = ((System.Drawing.Image)(resources.GetObject("btAceptar.Image")));
            this.btAceptar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btAceptar.Location = new System.Drawing.Point(439, 20);
            this.btAceptar.Margin = new System.Windows.Forms.Padding(4);
            this.btAceptar.Name = "btAceptar";
            this.btAceptar.Size = new System.Drawing.Size(80, 59);
            this.btAceptar.TabIndex = 0;
            this.btAceptar.Text = "Aceptar";
            this.btAceptar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btAceptar.UseVisualStyleBackColor = true;
            this.btAceptar.Click += new System.EventHandler(this.btAceptar_Click);
            // 
            // tbCatalogoActual
            // 
            this.tbCatalogoActual.BackColor = System.Drawing.Color.White;
            this.tbCatalogoActual.Enabled = false;
            this.tbCatalogoActual.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCatalogoActual.Location = new System.Drawing.Point(162, 20);
            this.tbCatalogoActual.Margin = new System.Windows.Forms.Padding(4);
            this.tbCatalogoActual.Name = "tbCatalogoActual";
            this.tbCatalogoActual.Size = new System.Drawing.Size(260, 22);
            this.tbCatalogoActual.TabIndex = 20;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(45, 26);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 16);
            this.label8.TabIndex = 19;
            this.label8.Text = "Catálogo actual :";
            // 
            // gbDataCatalog
            // 
            this.gbDataCatalog.Controls.Add(this.tbNuevoPeso);
            this.gbDataCatalog.Controls.Add(this.btnWeightModify);
            this.gbDataCatalog.Controls.Add(this.btVisualizar);
            this.gbDataCatalog.Controls.Add(this.tbPaletizado);
            this.gbDataCatalog.Controls.Add(this.label9);
            this.gbDataCatalog.Controls.Add(this.tbCategoria);
            this.gbDataCatalog.Controls.Add(this.label12);
            this.gbDataCatalog.Controls.Add(this.tbGrosor);
            this.gbDataCatalog.Controls.Add(this.label11);
            this.gbDataCatalog.Controls.Add(this.tbPais);
            this.gbDataCatalog.Controls.Add(this.tbPeso);
            this.gbDataCatalog.Controls.Add(this.tbTieneRfid);
            this.gbDataCatalog.Controls.Add(this.tbNDigitos);
            this.gbDataCatalog.Controls.Add(this.tbNLetras);
            this.gbDataCatalog.Controls.Add(this.tbPasaporteFinal);
            this.gbDataCatalog.Controls.Add(this.label5);
            this.gbDataCatalog.Controls.Add(this.label7);
            this.gbDataCatalog.Controls.Add(this.label6);
            this.gbDataCatalog.Controls.Add(this.label4);
            this.gbDataCatalog.Controls.Add(this.label3);
            this.gbDataCatalog.Controls.Add(this.label2);
            this.gbDataCatalog.Controls.Add(this.tbPasaporteInicial);
            this.gbDataCatalog.Controls.Add(this.label1);
            this.gbDataCatalog.Controls.Add(this.labelFechaLaminacion);
            this.gbDataCatalog.Controls.Add(this.fechaLaminacion);
            this.gbDataCatalog.Location = new System.Drawing.Point(26, 95);
            this.gbDataCatalog.Margin = new System.Windows.Forms.Padding(4);
            this.gbDataCatalog.Name = "gbDataCatalog";
            this.gbDataCatalog.Padding = new System.Windows.Forms.Padding(4);
            this.gbDataCatalog.Size = new System.Drawing.Size(493, 388);
            this.gbDataCatalog.TabIndex = 23;
            this.gbDataCatalog.TabStop = false;
            this.gbDataCatalog.Text = "Datos del catálogo actual";
            // 
            // tbNuevoPeso
            // 
            this.tbNuevoPeso.BackColor = System.Drawing.Color.White;
            this.tbNuevoPeso.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbNuevoPeso.Location = new System.Drawing.Point(426, 296);
            this.tbNuevoPeso.Margin = new System.Windows.Forms.Padding(4);
            this.tbNuevoPeso.Name = "tbNuevoPeso";
            this.tbNuevoPeso.Size = new System.Drawing.Size(59, 22);
            this.tbNuevoPeso.TabIndex = 40;
            // 
            // btnWeightModify
            // 
            this.btnWeightModify.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnWeightModify.Location = new System.Drawing.Point(342, 287);
            this.btnWeightModify.Margin = new System.Windows.Forms.Padding(4);
            this.btnWeightModify.Name = "btnWeightModify";
            this.btnWeightModify.Size = new System.Drawing.Size(76, 41);
            this.btnWeightModify.TabIndex = 39;
            this.btnWeightModify.Text = "Modificar Peso";
            this.btnWeightModify.UseVisualStyleBackColor = true;
            this.btnWeightModify.Click += new System.EventHandler(this.btnWeightModify_Click);
            // 
            // btVisualizar
            // 
            this.btVisualizar.Location = new System.Drawing.Point(389, 92);
            this.btVisualizar.Name = "btVisualizar";
            this.btVisualizar.Size = new System.Drawing.Size(76, 23);
            this.btVisualizar.TabIndex = 38;
            this.btVisualizar.Text = "Visualizar";
            this.btVisualizar.UseVisualStyleBackColor = true;
            this.btVisualizar.Click += new System.EventHandler(this.btVisualizar_Click);
            // 
            // tbPaletizado
            // 
            this.tbPaletizado.BackColor = System.Drawing.Color.White;
            this.tbPaletizado.Enabled = false;
            this.tbPaletizado.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPaletizado.Location = new System.Drawing.Point(172, 92);
            this.tbPaletizado.Margin = new System.Windows.Forms.Padding(4);
            this.tbPaletizado.Name = "tbPaletizado";
            this.tbPaletizado.Size = new System.Drawing.Size(210, 22);
            this.tbPaletizado.TabIndex = 37;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 95);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 16);
            this.label9.TabIndex = 36;
            this.label9.Text = "Paletizado :";
            // 
            // tbCategoria
            // 
            this.tbCategoria.BackColor = System.Drawing.Color.White;
            this.tbCategoria.Enabled = false;
            this.tbCategoria.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCategoria.Location = new System.Drawing.Point(173, 56);
            this.tbCategoria.Margin = new System.Windows.Forms.Padding(4);
            this.tbCategoria.Name = "tbCategoria";
            this.tbCategoria.Size = new System.Drawing.Size(292, 22);
            this.tbCategoria.TabIndex = 35;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(20, 62);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(73, 16);
            this.label12.TabIndex = 34;
            this.label12.Text = "Categoría :";
            // 
            // tbGrosor
            // 
            this.tbGrosor.BackColor = System.Drawing.Color.White;
            this.tbGrosor.Enabled = false;
            this.tbGrosor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbGrosor.Location = new System.Drawing.Point(173, 326);
            this.tbGrosor.Margin = new System.Windows.Forms.Padding(4);
            this.tbGrosor.Name = "tbGrosor";
            this.tbGrosor.Size = new System.Drawing.Size(161, 22);
            this.tbGrosor.TabIndex = 33;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(19, 329);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 16);
            this.label11.TabIndex = 32;
            this.label11.Text = "Grosor :";
            // 
            // tbPais
            // 
            this.tbPais.BackColor = System.Drawing.Color.White;
            this.tbPais.Enabled = false;
            this.tbPais.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPais.Location = new System.Drawing.Point(173, 28);
            this.tbPais.Margin = new System.Windows.Forms.Padding(4);
            this.tbPais.Name = "tbPais";
            this.tbPais.Size = new System.Drawing.Size(292, 22);
            this.tbPais.TabIndex = 31;
            // 
            // tbPeso
            // 
            this.tbPeso.BackColor = System.Drawing.Color.White;
            this.tbPeso.Enabled = false;
            this.tbPeso.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPeso.Location = new System.Drawing.Point(173, 296);
            this.tbPeso.Margin = new System.Windows.Forms.Padding(4);
            this.tbPeso.Name = "tbPeso";
            this.tbPeso.Size = new System.Drawing.Size(161, 22);
            this.tbPeso.TabIndex = 30;
            // 
            // tbTieneRfid
            // 
            this.tbTieneRfid.BackColor = System.Drawing.Color.White;
            this.tbTieneRfid.Enabled = false;
            this.tbTieneRfid.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbTieneRfid.Location = new System.Drawing.Point(173, 266);
            this.tbTieneRfid.Margin = new System.Windows.Forms.Padding(4);
            this.tbTieneRfid.Name = "tbTieneRfid";
            this.tbTieneRfid.Size = new System.Drawing.Size(161, 22);
            this.tbTieneRfid.TabIndex = 29;
            // 
            // tbNDigitos
            // 
            this.tbNDigitos.BackColor = System.Drawing.Color.White;
            this.tbNDigitos.Enabled = false;
            this.tbNDigitos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbNDigitos.Location = new System.Drawing.Point(172, 235);
            this.tbNDigitos.Margin = new System.Windows.Forms.Padding(4);
            this.tbNDigitos.Name = "tbNDigitos";
            this.tbNDigitos.Size = new System.Drawing.Size(136, 22);
            this.tbNDigitos.TabIndex = 28;
            // 
            // tbNLetras
            // 
            this.tbNLetras.BackColor = System.Drawing.Color.White;
            this.tbNLetras.Enabled = false;
            this.tbNLetras.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbNLetras.Location = new System.Drawing.Point(173, 205);
            this.tbNLetras.Margin = new System.Windows.Forms.Padding(4);
            this.tbNLetras.Name = "tbNLetras";
            this.tbNLetras.Size = new System.Drawing.Size(135, 22);
            this.tbNLetras.TabIndex = 27;
            // 
            // tbPasaporteFinal
            // 
            this.tbPasaporteFinal.BackColor = System.Drawing.Color.White;
            this.tbPasaporteFinal.Enabled = false;
            this.tbPasaporteFinal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPasaporteFinal.Location = new System.Drawing.Point(173, 169);
            this.tbPasaporteFinal.Margin = new System.Windows.Forms.Padding(4);
            this.tbPasaporteFinal.Name = "tbPasaporteFinal";
            this.tbPasaporteFinal.Size = new System.Drawing.Size(211, 22);
            this.tbPasaporteFinal.TabIndex = 26;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 172);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 16);
            this.label5.TabIndex = 25;
            this.label5.Text = "Pasaporte Final :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 299);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 16);
            this.label7.TabIndex = 24;
            this.label7.Text = "Peso :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 269);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 16);
            this.label6.TabIndex = 23;
            this.label6.Text = "RfID :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 238);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 16);
            this.label4.TabIndex = 22;
            this.label4.Text = "Número dígitos :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 208);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 16);
            this.label3.TabIndex = 21;
            this.label3.Text = "Número letras :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 34);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 16);
            this.label2.TabIndex = 20;
            this.label2.Text = "País :";
            // 
            // tbPasaporteInicial
            // 
            this.tbPasaporteInicial.BackColor = System.Drawing.Color.White;
            this.tbPasaporteInicial.Enabled = false;
            this.tbPasaporteInicial.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPasaporteInicial.Location = new System.Drawing.Point(173, 140);
            this.tbPasaporteInicial.Margin = new System.Windows.Forms.Padding(4);
            this.tbPasaporteInicial.Name = "tbPasaporteInicial";
            this.tbPasaporteInicial.Size = new System.Drawing.Size(211, 22);
            this.tbPasaporteInicial.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 143);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 16);
            this.label1.TabIndex = 18;
            this.label1.Text = "Pasaporte Inicial :";
            // 
            // labelFechaLaminacion
            // 
            this.labelFechaLaminacion.Location = new System.Drawing.Point(20, 356);
            this.labelFechaLaminacion.Name = "labelFechaLaminacion";
            this.labelFechaLaminacion.Size = new System.Drawing.Size(142, 20);
            this.labelFechaLaminacion.TabIndex = 15;
            this.labelFechaLaminacion.Text = "Fecha de Laminación:";
            // 
            // fechaLaminacion
            // 
            this.fechaLaminacion.Enabled = false;
            this.fechaLaminacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fechaLaminacion.Location = new System.Drawing.Point(173, 353);
            this.fechaLaminacion.Name = "fechaLaminacion";
            this.fechaLaminacion.Size = new System.Drawing.Size(163, 22);
            this.fechaLaminacion.TabIndex = 12;
            this.fechaLaminacion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(45, 58);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 16);
            this.label10.TabIndex = 26;
            this.label10.Text = "Catálogos :";
            // 
            // cbCatalogs
            // 
            this.cbCatalogs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCatalogs.FormattingEnabled = true;
            this.cbCatalogs.Location = new System.Drawing.Point(162, 55);
            this.cbCatalogs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbCatalogs.Name = "cbCatalogs";
            this.cbCatalogs.Size = new System.Drawing.Size(260, 24);
            this.cbCatalogs.TabIndex = 27;
            this.cbCatalogs.SelectedIndexChanged += new System.EventHandler(this.cbCatalogs_IndexChanged);
            // 
            // UCLoadCatalogo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.cbCatalogs);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.gbDataCatalog);
            this.Controls.Add(this.tbCatalogoActual);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btAceptar);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UCLoadCatalogo";
            this.Size = new System.Drawing.Size(540, 533);
            this.gbDataCatalog.ResumeLayout(false);
            this.gbDataCatalog.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btAceptar;
        private System.Windows.Forms.TextBox tbCatalogoActual;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox gbDataCatalog;
        private System.Windows.Forms.TextBox tbPais;
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
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbCatalogs;
        private System.Windows.Forms.TextBox tbGrosor;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbCategoria;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbPaletizado;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btVisualizar;
        private System.Windows.Forms.TextBox fechaLaminacion;  //MCR. 2011/03/03.
        private System.Windows.Forms.Label labelFechaLaminacion;
        private System.Windows.Forms.Button btnWeightModify;
        private System.Windows.Forms.TextBox tbNuevoPeso;  //MCR. 2011/03/03.


    }
}
