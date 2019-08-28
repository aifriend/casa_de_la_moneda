namespace Idpsa.Paletizado
{
    partial class ManualBoxReprocessor
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btClearBarcode = new System.Windows.Forms.Button();
            this.tbBarcode = new System.Windows.Forms.TextBox();
            this.labelErrorLectura = new System.Windows.Forms.Label();
            this.labelErrorPeso = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbRfid = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbTipoPasaporte = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbCountry = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbIdCaja = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btClearIdCajaToPrint = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbIdBoxToPrint = new System.Windows.Forms.TextBox();
            this.btValidar = new System.Windows.Forms.Button();
            this.labelErrorEtiquetaDuplicada = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btClearBarcode);
            this.groupBox1.Controls.Add(this.tbBarcode);
            this.groupBox1.Controls.Add(this.labelErrorLectura);
            this.groupBox1.Controls.Add(this.labelErrorEtiquetaDuplicada);
            this.groupBox1.Controls.Add(this.labelErrorPeso);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tbRfid);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbTipoPasaporte);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbCountry);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbIdCaja);
            this.groupBox1.Location = new System.Drawing.Point(17, 136);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(389, 294);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Lectura código de barras :";
            // 
            // btClearBarcode
            // 
            this.btClearBarcode.Location = new System.Drawing.Point(314, 43);
            this.btClearBarcode.Name = "btClearBarcode";
            this.btClearBarcode.Size = new System.Drawing.Size(52, 23);
            this.btClearBarcode.TabIndex = 11;
            this.btClearBarcode.Text = "Clear";
            this.btClearBarcode.UseVisualStyleBackColor = true;
            this.btClearBarcode.Click += new System.EventHandler(this.btClearBarcode_Click);
            // 
            // tbBarcode
            // 
            this.tbBarcode.Location = new System.Drawing.Point(19, 44);
            this.tbBarcode.Margin = new System.Windows.Forms.Padding(4);
            this.tbBarcode.Name = "tbBarcode";
            this.tbBarcode.Size = new System.Drawing.Size(288, 22);
            this.tbBarcode.TabIndex = 10;
            this.tbBarcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbBarcode.TextChanged += new System.EventHandler(this.tbBarcode_TextChanged);
            // 
            // labelErrorLectura
            // 
            this.labelErrorLectura.BackColor = System.Drawing.Color.LightSlateGray;
            this.labelErrorLectura.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelErrorLectura.Location = new System.Drawing.Point(203, 236);
            this.labelErrorLectura.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelErrorLectura.Name = "labelErrorLectura";
            this.labelErrorLectura.Size = new System.Drawing.Size(163, 21);
            this.labelErrorLectura.TabIndex = 9;
            this.labelErrorLectura.Text = "Error Lectura";
            this.labelErrorLectura.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelErrorPeso
            // 
            this.labelErrorPeso.BackColor = System.Drawing.Color.LightSlateGray;
            this.labelErrorPeso.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelErrorPeso.Location = new System.Drawing.Point(19, 236);
            this.labelErrorPeso.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelErrorPeso.Name = "labelErrorPeso";
            this.labelErrorPeso.Size = new System.Drawing.Size(163, 21);
            this.labelErrorPeso.TabIndex = 9;
            this.labelErrorPeso.Text = "Error Peso";
            this.labelErrorPeso.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Blue;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(19, 21);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(347, 21);
            this.label9.TabIndex = 9;
            this.label9.Text = "Código de barras:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 207);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 16);
            this.label7.TabIndex = 7;
            this.label7.Text = "RFID :";
            // 
            // tbRfid
            // 
            this.tbRfid.BackColor = System.Drawing.Color.White;
            this.tbRfid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbRfid.Location = new System.Drawing.Point(171, 201);
            this.tbRfid.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tbRfid.Name = "tbRfid";
            this.tbRfid.Size = new System.Drawing.Size(194, 22);
            this.tbRfid.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 172);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 16);
            this.label5.TabIndex = 5;
            this.label5.Text = "Categoría :";
            // 
            // tbTipoPasaporte
            // 
            this.tbTipoPasaporte.BackColor = System.Drawing.Color.White;
            this.tbTipoPasaporte.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbTipoPasaporte.Location = new System.Drawing.Point(171, 165);
            this.tbTipoPasaporte.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tbTipoPasaporte.Name = "tbTipoPasaporte";
            this.tbTipoPasaporte.Size = new System.Drawing.Size(194, 22);
            this.tbTipoPasaporte.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 136);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "País :";
            // 
            // tbCountry
            // 
            this.tbCountry.BackColor = System.Drawing.Color.White;
            this.tbCountry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCountry.Location = new System.Drawing.Point(171, 130);
            this.tbCountry.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tbCountry.Name = "tbCountry";
            this.tbCountry.Size = new System.Drawing.Size(194, 22);
            this.tbCountry.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 98);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Número de caja :";
            // 
            // tbIdCaja
            // 
            this.tbIdCaja.BackColor = System.Drawing.Color.White;
            this.tbIdCaja.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbIdCaja.Location = new System.Drawing.Point(141, 92);
            this.tbIdCaja.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tbIdCaja.Name = "tbIdCaja";
            this.tbIdCaja.Size = new System.Drawing.Size(225, 22);
            this.tbIdCaja.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btClearIdCajaToPrint);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tbIdBoxToPrint);
            this.groupBox2.Location = new System.Drawing.Point(17, 25);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(389, 103);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Impresión etiqueta :";
            // 
            // btClearIdCajaToPrint
            // 
            this.btClearIdCajaToPrint.Location = new System.Drawing.Point(322, 36);
            this.btClearIdCajaToPrint.Name = "btClearIdCajaToPrint";
            this.btClearIdCajaToPrint.Size = new System.Drawing.Size(52, 23);
            this.btClearIdCajaToPrint.TabIndex = 14;
            this.btClearIdCajaToPrint.Text = "Clear";
            this.btClearIdCajaToPrint.UseVisualStyleBackColor = true;
            this.btClearIdCajaToPrint.Click += new System.EventHandler(this.btClearIdCajaToPrint_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(273, 66);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 29);
            this.button1.TabIndex = 13;
            this.button1.Text = "Imprimir";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 39);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "Número de caja :";
            // 
            // tbIdBoxToPrint
            // 
            this.tbIdBoxToPrint.Location = new System.Drawing.Point(141, 36);
            this.tbIdBoxToPrint.Margin = new System.Windows.Forms.Padding(4);
            this.tbIdBoxToPrint.Name = "tbIdBoxToPrint";
            this.tbIdBoxToPrint.Size = new System.Drawing.Size(174, 22);
            this.tbIdBoxToPrint.TabIndex = 11;
            this.tbIdBoxToPrint.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btValidar
            // 
            this.btValidar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btValidar.Location = new System.Drawing.Point(284, 430);
            this.btValidar.Margin = new System.Windows.Forms.Padding(4);
            this.btValidar.Name = "btValidar";
            this.btValidar.Size = new System.Drawing.Size(123, 36);
            this.btValidar.TabIndex = 2;
            this.btValidar.Text = "Validar";
            this.btValidar.UseVisualStyleBackColor = true;
            this.btValidar.Click += new System.EventHandler(this.btValidar_Click);
            // 
            // labelErrorEtiquetaDuplicada
            // 
            this.labelErrorEtiquetaDuplicada.BackColor = System.Drawing.Color.LightSlateGray;
            this.labelErrorEtiquetaDuplicada.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelErrorEtiquetaDuplicada.Location = new System.Drawing.Point(19, 269);
            this.labelErrorEtiquetaDuplicada.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelErrorEtiquetaDuplicada.Name = "labelErrorEtiquetaDuplicada";
            this.labelErrorEtiquetaDuplicada.Size = new System.Drawing.Size(163, 21);
            this.labelErrorEtiquetaDuplicada.TabIndex = 9;
            this.labelErrorEtiquetaDuplicada.Text = "Etiqueta Duplicada";
            this.labelErrorEtiquetaDuplicada.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ManualBoxReprocessor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.btValidar);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ManualBoxReprocessor";
            this.Size = new System.Drawing.Size(427, 482);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label tbIdCaja;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label tbRfid;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label tbTipoPasaporte;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label tbCountry;
        private System.Windows.Forms.TextBox tbBarcode;
        private System.Windows.Forms.Button btValidar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbIdBoxToPrint;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btClearBarcode;
        private System.Windows.Forms.Button btClearIdCajaToPrint;
        private System.Windows.Forms.Label labelErrorPeso;
        private System.Windows.Forms.Label labelErrorLectura;
        private System.Windows.Forms.Label labelErrorEtiquetaDuplicada;
    }
}
