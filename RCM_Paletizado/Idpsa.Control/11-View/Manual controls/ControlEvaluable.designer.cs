namespace Idpsa.Control.View
{
    partial class ControlEvaluable
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
            this.Label1 = new System.Windows.Forms.Label();
            this.lbSensor = new System.Windows.Forms.Label();
            this.lbCom = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.Gainsboro;
            this.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Label1.Location = new System.Drawing.Point(-2, 31);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(640, 12);
            this.Label1.TabIndex = 9;
            // 
            // lbSensor
            // 
            this.lbSensor.BackColor = System.Drawing.Color.White;
            this.lbSensor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbSensor.Location = new System.Drawing.Point(431, 5);
            this.lbSensor.Name = "lbSensor";
            this.lbSensor.Size = new System.Drawing.Size(132, 20);
            this.lbSensor.TabIndex = 8;
            this.lbSensor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbCom
            // 
            this.lbCom.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lbCom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCom.Location = new System.Drawing.Point(64, 5);
            this.lbCom.Name = "lbCom";
            this.lbCom.Size = new System.Drawing.Size(324, 22);
            this.lbCom.TabIndex = 80;
            this.lbCom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ControlEvaluable
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lbCom);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.lbSensor);
            this.Name = "ControlEvaluable";
            this.Size = new System.Drawing.Size(636, 41);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label lbSensor;
        internal System.Windows.Forms.Label lbCom;
    }
}
