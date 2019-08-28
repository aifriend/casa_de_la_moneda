namespace Idpsa.Control.View
{
    partial class ControlLinealActuator
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
            this.lbP = new System.Windows.Forms.Label();
            this.btnPos = new System.Windows.Forms.Button();
            this.btnNeg = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lbReferenced = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btRefenciar = new System.Windows.Forms.Button();
            this.lbCom = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbCounter = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbP
            // 
            this.lbP.BackColor = System.Drawing.Color.Black;
            this.lbP.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lbP.ForeColor = System.Drawing.Color.White;
            this.lbP.Location = new System.Drawing.Point(336, 76);
            this.lbP.Name = "lbP";
            this.lbP.Size = new System.Drawing.Size(135, 24);
            this.lbP.TabIndex = 99;
            this.lbP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPos
            // 
            this.btnPos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(150)))));
            this.btnPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnPos.Location = new System.Drawing.Point(495, 12);
            this.btnPos.Name = "btnPos";
            this.btnPos.Size = new System.Drawing.Size(85, 40);
            this.btnPos.TabIndex = 98;
            this.btnPos.Text = ">";
            this.btnPos.UseVisualStyleBackColor = false;
            this.btnPos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPos_MouseDown);
            this.btnPos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPos_MouseUp);
            // 
            // btnNeg
            // 
            this.btnNeg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(150)))));
            this.btnNeg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnNeg.Location = new System.Drawing.Point(79, 12);
            this.btnNeg.Name = "btnNeg";
            this.btnNeg.Size = new System.Drawing.Size(85, 40);
            this.btnNeg.TabIndex = 97;
            this.btnNeg.Text = "<";
            this.btnNeg.UseVisualStyleBackColor = false;
            this.btnNeg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNeg_MouseDown);
            this.btnNeg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnNeg_MouseUp);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label1.Location = new System.Drawing.Point(336, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 16);
            this.label1.TabIndex = 100;
            this.label1.Text = "Posicion :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbReferenced
            // 
            this.lbReferenced.BackColor = System.Drawing.Color.Yellow;
            this.lbReferenced.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbReferenced.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lbReferenced.ForeColor = System.Drawing.Color.White;
            this.lbReferenced.Location = new System.Drawing.Point(191, 76);
            this.lbReferenced.Name = "lbReferenced";
            this.lbReferenced.Size = new System.Drawing.Size(108, 14);
            this.lbReferenced.TabIndex = 105;
            this.lbReferenced.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label2.Location = new System.Drawing.Point(191, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 16);
            this.label2.TabIndex = 104;
            this.label2.Text = "Referenciado :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btRefenciar
            // 
            this.btRefenciar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btRefenciar.Location = new System.Drawing.Point(89, 60);
            this.btRefenciar.Name = "btRefenciar";
            this.btRefenciar.Size = new System.Drawing.Size(75, 30);
            this.btRefenciar.TabIndex = 109;
            this.btRefenciar.Text = "Referenciar";
            this.btRefenciar.UseVisualStyleBackColor = true;
            this.btRefenciar.Click += new System.EventHandler(this.btRefenciar_Click);
            // 
            // lbCom
            // 
            this.lbCom.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lbCom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCom.Location = new System.Drawing.Point(219, 12);
            this.lbCom.Name = "lbCom";
            this.lbCom.Size = new System.Drawing.Size(223, 32);
            this.lbCom.TabIndex = 110;
            this.lbCom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.LightGray;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label3.Location = new System.Drawing.Point(0, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(638, 12);
            this.label3.TabIndex = 111;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label4.Location = new System.Drawing.Point(492, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 16);
            this.label4.TabIndex = 113;
            this.label4.Text = "Contador :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbCounter
            // 
            this.lbCounter.BackColor = System.Drawing.Color.Black;
            this.lbCounter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lbCounter.ForeColor = System.Drawing.Color.White;
            this.lbCounter.Location = new System.Drawing.Point(492, 76);
            this.lbCounter.Name = "lbCounter";
            this.lbCounter.Size = new System.Drawing.Size(135, 24);
            this.lbCounter.TabIndex = 112;
            this.lbCounter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // controlLinealActuator
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbCounter);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbCom);
            this.Controls.Add(this.btRefenciar);
            this.Controls.Add(this.lbReferenced);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbP);
            this.Controls.Add(this.btnPos);
            this.Controls.Add(this.btnNeg);
            this.Name = "controlLinealActuator";
            this.Size = new System.Drawing.Size(638, 122);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Label lbP;
        internal System.Windows.Forms.Button btnPos;
        internal System.Windows.Forms.Button btnNeg;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label lbReferenced;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btRefenciar;
        internal System.Windows.Forms.Label lbCom;
        internal System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        internal System.Windows.Forms.Label lbCounter;
    }
}
