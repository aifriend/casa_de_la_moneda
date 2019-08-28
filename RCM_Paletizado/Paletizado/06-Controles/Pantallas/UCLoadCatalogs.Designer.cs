namespace Idpsa.Paletizado
{
    partial class UCLoadCatalogs
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
            this.tabCont = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this._ucLoadCatalogoLine1 = new Idpsa.UCLoadCatalogo();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this._ucLoadCatalogoLine2 = new Idpsa.UCLoadCatalogo();
            this.tabCont.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabCont
            // 
            this.tabCont.Controls.Add(this.tabPage1);
            this.tabCont.Controls.Add(this.tabPage2);
            this.tabCont.Location = new System.Drawing.Point(0, 0);
            this.tabCont.Name = "tabCont";
            this.tabCont.SelectedIndex = 0;
            this.tabCont.Size = new System.Drawing.Size(558, 566);
            this.tabCont.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this._ucLoadCatalogoLine1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(550, 540);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Linea 1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // _ucLoadCatalogoLine1
            // 
            this._ucLoadCatalogoLine1.BackColor = System.Drawing.SystemColors.Control;
            this._ucLoadCatalogoLine1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._ucLoadCatalogoLine1.Catalogo = null;
            this._ucLoadCatalogoLine1.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ucLoadCatalogoLine1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ucLoadCatalogoLine1.Location = new System.Drawing.Point(3, 3);
            this._ucLoadCatalogoLine1.Margin = new System.Windows.Forms.Padding(4);
            this._ucLoadCatalogoLine1.Name = "_ucLoadCatalogoLine1";
            this._ucLoadCatalogoLine1.Size = new System.Drawing.Size(544, 534);
            this._ucLoadCatalogoLine1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this._ucLoadCatalogoLine2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(550, 540);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Linea 2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // _ucLoadCatalogoLine2
            // 
            this._ucLoadCatalogoLine2.BackColor = System.Drawing.SystemColors.Control;
            this._ucLoadCatalogoLine2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._ucLoadCatalogoLine2.Catalogo = null;
            this._ucLoadCatalogoLine2.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ucLoadCatalogoLine2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ucLoadCatalogoLine2.Location = new System.Drawing.Point(3, 3);
            this._ucLoadCatalogoLine2.Margin = new System.Windows.Forms.Padding(4);
            this._ucLoadCatalogoLine2.Name = "_ucLoadCatalogoLine2";
            this._ucLoadCatalogoLine2.Size = new System.Drawing.Size(544, 534);
            this._ucLoadCatalogoLine2.TabIndex = 1;
            // 
            // UCLoadCatalogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabCont);
            this.Name = "UCLoadCatalogs";
            this.Size = new System.Drawing.Size(558, 566);
            this.tabCont.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabCont;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private UCLoadCatalogo _ucLoadCatalogoLine1;
        private UCLoadCatalogo _ucLoadCatalogoLine2;
    }
}
