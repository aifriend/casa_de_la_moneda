namespace Idpsa.Control.View
{
    partial class ControlDataDysplayer<T>
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
            this.lbCom = new System.Windows.Forms.Label();
            this.lbStringDysplay = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbCom
            // 
            this.lbCom.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lbCom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCom.Location = new System.Drawing.Point(149, 7);
            this.lbCom.Name = "lbCom";
            this.lbCom.Size = new System.Drawing.Size(389, 36);
            this.lbCom.TabIndex = 11;
            this.lbCom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbStringDysplay
            // 
            this.lbStringDysplay.BackColor = System.Drawing.Color.Black;
            this.lbStringDysplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbStringDysplay.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lbStringDysplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStringDysplay.ForeColor = System.Drawing.Color.White;
            this.lbStringDysplay.Location = new System.Drawing.Point(69, 55);
            this.lbStringDysplay.Name = "lbStringDysplay";
            this.lbStringDysplay.Size = new System.Drawing.Size(555, 82);
            this.lbStringDysplay.TabIndex = 12;
            this.lbStringDysplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.Color.Gainsboro;
            this.Label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Label2.Location = new System.Drawing.Point(-3, 143);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(684, 12);
            this.Label2.TabIndex = 89;
            // 
            // controlDataDysplayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.lbStringDysplay);
            this.Controls.Add(this.lbCom);
            this.Name = "controlDataDysplayer";
            this.Size = new System.Drawing.Size(682, 153);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Label lbCom;
        internal System.Windows.Forms.Label lbStringDysplay;
        internal System.Windows.Forms.Label Label2;
    }
}
