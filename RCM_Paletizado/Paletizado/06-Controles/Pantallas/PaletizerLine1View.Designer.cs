namespace Idpsa.Paletizado
{
    partial class PaletizersLine1View
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tab1 = new System.Windows.Forms.TabPage();
            this._view1 = new Idpsa.Paletizado.MosaicView();
            this.tab2 = new System.Windows.Forms.TabPage();
            this._view2 = new Idpsa.Paletizado.MosaicView();
            this.tab3 = new System.Windows.Forms.TabPage();
            this._view3 = new Idpsa.Paletizado.MosaicView();
            this.tabControl1.SuspendLayout();
            this.tab1.SuspendLayout();
            this.tab2.SuspendLayout();
            this.tab3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(16, 620);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(760, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(18, 620);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(16, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(744, 17);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(16, 597);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(744, 23);
            this.panel4.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tab1);
            this.tabControl1.Controls.Add(this.tab2);
            this.tabControl1.Controls.Add(this.tab3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(16, 17);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(744, 580);
            this.tabControl1.TabIndex = 4;
            // 
            // tab1
            // 
            this.tab1.Controls.Add(this._view1);
            this.tab1.Location = new System.Drawing.Point(4, 22);
            this.tab1.Name = "tab1";
            this.tab1.Padding = new System.Windows.Forms.Padding(3);
            this.tab1.Size = new System.Drawing.Size(736, 554);
            this.tab1.TabIndex = 0;
            this.tab1.Text = "Paletizado inicial";
            this.tab1.UseVisualStyleBackColor = true;
            // 
            // _view1
            // 
            this._view1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._view1.Dock = System.Windows.Forms.DockStyle.Fill;
            this._view1.Location = new System.Drawing.Point(3, 3);
            this._view1.Name = "_view1";
            this._view1.Size = new System.Drawing.Size(730, 548);
            this._view1.TabIndex = 0;
            // 
            // tab2
            // 
            this.tab2.Controls.Add(this._view2);
            this.tab2.Location = new System.Drawing.Point(4, 22);
            this.tab2.Name = "tab2";
            this.tab2.Padding = new System.Windows.Forms.Padding(3);
            this.tab2.Size = new System.Drawing.Size(736, 554);
            this.tab2.TabIndex = 1;
            this.tab2.Text = "Despaletizado";
            this.tab2.UseVisualStyleBackColor = true;
            // 
            // _view2
            // 
            this._view2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._view2.Dock = System.Windows.Forms.DockStyle.Fill;
            this._view2.Location = new System.Drawing.Point(3, 3);
            this._view2.Name = "_view2";
            this._view2.Size = new System.Drawing.Size(730, 548);
            this._view2.TabIndex = 1;
            // 
            // tab3
            // 
            this.tab3.Controls.Add(this._view3);
            this.tab3.Location = new System.Drawing.Point(4, 22);
            this.tab3.Name = "tab3";
            this.tab3.Padding = new System.Windows.Forms.Padding(3);
            this.tab3.Size = new System.Drawing.Size(736, 554);
            this.tab3.TabIndex = 2;
            this.tab3.Text = "Paletizado final";
            this.tab3.UseVisualStyleBackColor = true;
            // 
            // _view3
            // 
            this._view3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._view3.Dock = System.Windows.Forms.DockStyle.Fill;
            this._view3.Location = new System.Drawing.Point(3, 3);
            this._view3.Name = "_view3";
            this._view3.Size = new System.Drawing.Size(730, 548);
            this._view3.TabIndex = 1;
            // 
            // PaletizersLine1View
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "PaletizersLine1View";
            this.Size = new System.Drawing.Size(778, 620);
            this.tabControl1.ResumeLayout(false);
            this.tab1.ResumeLayout(false);
            this.tab2.ResumeLayout(false);
            this.tab3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tab1;
        private System.Windows.Forms.TabPage tab2;
        private System.Windows.Forms.TabPage tab3;
        private Idpsa.Paletizado.MosaicView _view1;
        private Idpsa.Paletizado.MosaicView _view2;
        private Idpsa.Paletizado.MosaicView _view3;
    }
}
