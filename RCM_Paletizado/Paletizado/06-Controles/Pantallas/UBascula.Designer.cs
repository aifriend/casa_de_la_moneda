namespace Idpsa.Paletizado
{
    partial class UBascula
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
            this.progressBarWeight = new System.Windows.Forms.ProgressBar();
            this.lbNominalWeight = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbWeightMargin = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lbError = new System.Windows.Forms.Label();
            this.lbState = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.lbCalibration = new System.Windows.Forms.Label();
            this.bCalibPassport = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lbActualWeight = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pnResult = new System.Windows.Forms.Panel();
            this.lbPassWeightMax = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbActualDiff = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBarWeight
            // 
            this.progressBarWeight.Location = new System.Drawing.Point(31, 89);
            this.progressBarWeight.Name = "progressBarWeight";
            this.progressBarWeight.Size = new System.Drawing.Size(296, 28);
            this.progressBarWeight.Step = 1;
            this.progressBarWeight.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarWeight.TabIndex = 0;
            // 
            // lbNominalWeight
            // 
            this.lbNominalWeight.AutoSize = true;
            this.lbNominalWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNominalWeight.Location = new System.Drawing.Point(42, 25);
            this.lbNominalWeight.Name = "lbNominalWeight";
            this.lbNominalWeight.Size = new System.Drawing.Size(0, 55);
            this.lbNominalWeight.TabIndex = 2;
            this.lbNominalWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(27, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "0";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbWeightMargin
            // 
            this.lbWeightMargin.AutoSize = true;
            this.lbWeightMargin.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbWeightMargin.Location = new System.Drawing.Point(143, 129);
            this.lbWeightMargin.Name = "lbWeightMargin";
            this.lbWeightMargin.Size = new System.Drawing.Size(0, 24);
            this.lbWeightMargin.TabIndex = 5;
            this.lbWeightMargin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(26, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(623, 551);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "COMPROBACION Y CALIBRACION DE BASCULA";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lbError);
            this.groupBox5.Controls.Add(this.lbState);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.lbStatus);
            this.groupBox5.Controls.Add(this.lbCalibration);
            this.groupBox5.Controls.Add(this.bCalibPassport);
            this.groupBox5.Location = new System.Drawing.Point(51, 376);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(517, 144);
            this.groupBox5.TabIndex = 15;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Calibracion de pasaporte individual";
            // 
            // lbError
            // 
            this.lbError.AutoSize = true;
            this.lbError.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbError.ForeColor = System.Drawing.Color.Red;
            this.lbError.Location = new System.Drawing.Point(146, 117);
            this.lbError.Name = "lbError";
            this.lbError.Size = new System.Drawing.Size(0, 16);
            this.lbError.TabIndex = 29;
            // 
            // lbState
            // 
            this.lbState.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbState.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbState.Location = new System.Drawing.Point(30, 56);
            this.lbState.Name = "lbState";
            this.lbState.Size = new System.Drawing.Size(144, 52);
            this.lbState.TabIndex = 28;
            this.lbState.Text = "En espera";
            this.lbState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(186, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 20);
            this.label4.TabIndex = 27;
            this.label4.Text = "Calibrado";
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.Location = new System.Drawing.Point(30, 32);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(60, 20);
            this.lbStatus.TabIndex = 26;
            this.lbStatus.Text = "Estado";
            // 
            // lbCalibration
            // 
            this.lbCalibration.BackColor = System.Drawing.Color.Black;
            this.lbCalibration.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCalibration.ForeColor = System.Drawing.Color.White;
            this.lbCalibration.Location = new System.Drawing.Point(189, 56);
            this.lbCalibration.Name = "lbCalibration";
            this.lbCalibration.Size = new System.Drawing.Size(137, 52);
            this.lbCalibration.TabIndex = 24;
            this.lbCalibration.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bCalibPassport
            // 
            this.bCalibPassport.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bCalibPassport.Location = new System.Drawing.Point(352, 38);
            this.bCalibPassport.Name = "bCalibPassport";
            this.bCalibPassport.Size = new System.Drawing.Size(135, 70);
            this.bCalibPassport.TabIndex = 0;
            this.bCalibPassport.Text = "Calibrar";
            this.bCalibPassport.UseVisualStyleBackColor = true;
            this.bCalibPassport.Click += new System.EventHandler(this.bCalibPassport_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lbActualWeight);
            this.groupBox4.Location = new System.Drawing.Point(316, 40);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(252, 100);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Peso Actual Grupo";
            // 
            // lbActualWeight
            // 
            this.lbActualWeight.BackColor = System.Drawing.Color.Black;
            this.lbActualWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbActualWeight.ForeColor = System.Drawing.Color.White;
            this.lbActualWeight.Location = new System.Drawing.Point(29, 27);
            this.lbActualWeight.Name = "lbActualWeight";
            this.lbActualWeight.Size = new System.Drawing.Size(193, 57);
            this.lbActualWeight.TabIndex = 25;
            this.lbActualWeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbActualDiff);
            this.groupBox3.Controls.Add(this.pnResult);
            this.groupBox3.Controls.Add(this.lbPassWeightMax);
            this.groupBox3.Controls.Add(this.lbWeightMargin);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.progressBarWeight);
            this.groupBox3.Location = new System.Drawing.Point(50, 165);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(518, 182);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Tolerancias y Valoracion";
            // 
            // pnResult
            // 
            this.pnResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnResult.Location = new System.Drawing.Point(353, 37);
            this.pnResult.Name = "pnResult";
            this.pnResult.Size = new System.Drawing.Size(135, 116);
            this.pnResult.TabIndex = 12;
            // 
            // lbPassWeightMax
            // 
            this.lbPassWeightMax.AutoSize = true;
            this.lbPassWeightMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPassWeightMax.Location = new System.Drawing.Point(270, 129);
            this.lbPassWeightMax.Name = "lbPassWeightMax";
            this.lbPassWeightMax.Size = new System.Drawing.Size(0, 24);
            this.lbPassWeightMax.TabIndex = 10;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbNominalWeight);
            this.groupBox2.Location = new System.Drawing.Point(51, 40);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(237, 100);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Peso Nominal Grupo";
            // 
            // tbActualDiff
            // 
            this.tbActualDiff.BackColor = System.Drawing.Color.SkyBlue;
            this.tbActualDiff.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tbActualDiff.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbActualDiff.Location = new System.Drawing.Point(126, 36);
            this.tbActualDiff.Name = "tbActualDiff";
            this.tbActualDiff.Size = new System.Drawing.Size(107, 42);
            this.tbActualDiff.TabIndex = 30;
            this.tbActualDiff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UBascula
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox1);
            this.Name = "UBascula";
            this.Size = new System.Drawing.Size(678, 606);
            this.groupBox1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarWeight;
        private System.Windows.Forms.Label lbNominalWeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbWeightMargin;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbPassWeightMax;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button bCalibPassport;
        internal System.Windows.Forms.Label lbCalibration;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Label lbState;
        internal System.Windows.Forms.Label lbActualWeight;
        private System.Windows.Forms.Panel pnResult;
        private System.Windows.Forms.Label lbError;
        private System.Windows.Forms.Label tbActualDiff;
    }
}
