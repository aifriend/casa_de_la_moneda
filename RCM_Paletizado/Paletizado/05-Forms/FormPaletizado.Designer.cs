namespace Idpsa
{
    partial class FormPaletizado
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
            this.tbName = new System.Windows.Forms.TextBox();
            this.lbMosaics = new System.Windows.Forms.ListBox();
            this.btAñadir = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lbMosaicsAdded = new System.Windows.Forms.ListBox();
            this.btEliminar = new System.Windows.Forms.Button();
            this.btAceptar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbPalets = new System.Windows.Forms.ComboBox();
            this.pbMosaic = new System.Windows.Forms.PictureBox();
            this.btCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbMosaic)).BeginInit();
            this.SuspendLayout();
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(131, 59);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(153, 20);
            this.tbName.TabIndex = 0;
            // 
            // lbMosaics
            // 
            this.lbMosaics.FormattingEnabled = true;
            this.lbMosaics.Location = new System.Drawing.Point(10, 193);
            this.lbMosaics.Name = "lbMosaics";
            this.lbMosaics.Size = new System.Drawing.Size(120, 134);
            this.lbMosaics.TabIndex = 1;
            // 
            // btAñadir
            // 
            this.btAñadir.Location = new System.Drawing.Point(13, 344);
            this.btAñadir.Name = "btAñadir";
            this.btAñadir.Size = new System.Drawing.Size(75, 23);
            this.btAñadir.TabIndex = 3;
            this.btAñadir.Text = "Añadir";
            this.btAñadir.UseVisualStyleBackColor = true;
            this.btAñadir.Click += new System.EventHandler(this.btAñadir_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Nombre paletizado :";
            // 
            // lbMosaicsAdded
            // 
            this.lbMosaicsAdded.FormattingEnabled = true;
            this.lbMosaicsAdded.Location = new System.Drawing.Point(164, 193);
            this.lbMosaicsAdded.Name = "lbMosaicsAdded";
            this.lbMosaicsAdded.Size = new System.Drawing.Size(120, 134);
            this.lbMosaicsAdded.TabIndex = 5;
            // 
            // btEliminar
            // 
            this.btEliminar.Location = new System.Drawing.Point(167, 344);
            this.btEliminar.Name = "btEliminar";
            this.btEliminar.Size = new System.Drawing.Size(75, 23);
            this.btEliminar.TabIndex = 6;
            this.btEliminar.Text = "Eliminar";
            this.btEliminar.UseVisualStyleBackColor = true;
            this.btEliminar.Click += new System.EventHandler(this.btEliminar_Click);
            // 
            // btAceptar
            // 
            this.btAceptar.Location = new System.Drawing.Point(611, 392);
            this.btAceptar.Name = "btAceptar";
            this.btAceptar.Size = new System.Drawing.Size(75, 23);
            this.btAceptar.TabIndex = 7;
            this.btAceptar.Text = "Aceptar";
            this.btAceptar.UseVisualStyleBackColor = true;
            this.btAceptar.Click += new System.EventHandler(this.btAceptar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Tipo de palet :";
            // 
            // cbPalets
            // 
            this.cbPalets.FormattingEnabled = true;
            this.cbPalets.Location = new System.Drawing.Point(131, 111);
            this.cbPalets.Name = "cbPalets";
            this.cbPalets.Size = new System.Drawing.Size(153, 21);
            this.cbPalets.TabIndex = 9;
            // 
            // pbMosaic
            // 
            this.pbMosaic.BackColor = System.Drawing.Color.White;
            this.pbMosaic.Location = new System.Drawing.Point(308, 8);
            this.pbMosaic.Name = "pbMosaic";
            this.pbMosaic.Size = new System.Drawing.Size(378, 378);
            this.pbMosaic.TabIndex = 2;
            this.pbMosaic.TabStop = false;
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(509, 392);
            this.btCancelar.Name = "btCancelar";
            this.btCancelar.Size = new System.Drawing.Size(75, 23);
            this.btCancelar.TabIndex = 11;
            this.btCancelar.Text = "Cancelar";
            this.btCancelar.UseVisualStyleBackColor = true;
            this.btCancelar.Click += new System.EventHandler(this.btCancelar_Click);
            // 
            // FormPaletizado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 429);
            this.Controls.Add(this.btCancelar);
            this.Controls.Add(this.cbPalets);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btAceptar);
            this.Controls.Add(this.btEliminar);
            this.Controls.Add(this.lbMosaicsAdded);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btAñadir);
            this.Controls.Add(this.pbMosaic);
            this.Controls.Add(this.lbMosaics);
            this.Controls.Add(this.tbName);
            this.Name = "FormPaletizado";
            this.Text = "Editor paletizado";
            ((System.ComponentModel.ISupportInitialize)(this.pbMosaic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.ListBox lbMosaics;
        private System.Windows.Forms.PictureBox pbMosaic;
        private System.Windows.Forms.Button btAñadir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbMosaicsAdded;
        private System.Windows.Forms.Button btEliminar;
        private System.Windows.Forms.Button btAceptar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbPalets;
        private System.Windows.Forms.Button btCancelar;
    }
}