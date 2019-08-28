using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.Component;
using Idpsa.Control.Mode.Manually;

namespace Idpsa.Control.View
{
    public class ControlSensores : UserControl, IRefrescable
    {
        #region Delegates

        public delegate void click_EventHandler();

        #endregion

        #region " Código generado por el Diseñador de Windows Forms "

        //NOTA: el Diseñador de Windows Forms requiere el siguiente procedimiento
        //Puede modificarse utilizando el Diseñador de Windows Forms. 
        //No lo modifique con el editor de código.
        internal Button btRep;
        internal Button btTra;
        private IContainer components;
        internal Label Label1;
        internal Label lbCom;
        internal Label lbSr1;
        internal Label lbSr2;
        internal Label lbSt1;
        internal Label lbSt2;

        public ControlSensores(Manual manual)
        {
            //El Diseñador de Windows Forms requiere esta llamada.
            InitializeComponent();
            var generalManual = (GeneralManual) manual.RepresentedInstance;
            lsR = new System.Windows.Forms.Control[] {lbSr1, lbSr2};
            lsT = new System.Windows.Forms.Control[] {lbSt1, lbSt2};
            var d = new[] {'|'};
            string[] str = manual.Descripcion.Split(d);
            lbCom.Text = str[0].Trim();
            if (str.Length > 1)
            {
                if (str[1].Trim().Length > 0)
                {
                    btRep.Font = new Font("Microsoft Sans Serif", 8f);
                    btRep.Text = str[1].Trim();
                }
            }
            if (str.Length > 2)
            {
                if (str[2].Trim().Length > 0)
                {
                    btTra.Font = new Font("Microsoft Sans Serif", 8f);
                    btTra.Text = str[2].Trim();
                }
            }
            for (int i = 0; i <= generalManual.FinalesCarreraBas.Length - 1; i++)
            {
                if (i < lsR.Length)
                {
                    if (generalManual.FinalesCarreraBas[i] == null)
                    {
                        lsR[i].Visible = false;
                    }
                    else
                    {
                        lsR[i].Text = generalManual.FinalesCarreraBas[i].ToString();
                        sR.Add(new SensorP(generalManual.FinalesCarreraBas[i]));
                    }
                }
            }
            for (int i = 0; i <= generalManual.FinalesCarreraWrk.Length - 1; i++)
            {
                if (i < lsT.Length)
                {
                    if (generalManual.FinalesCarreraWrk[i] == null)
                    {
                        lsT[i].Visible = false;
                    }
                    else
                    {
                        lsT[i].Text = generalManual.FinalesCarreraWrk[i].ToString();
                        sT.Add(new SensorP(generalManual.FinalesCarreraWrk[i]));
                    }
                }
            }
            for (int i = 0; i <= generalManual.ActuadoresBas.Length - 1; i++)
            {
                if (!(generalManual.ActuadoresBas[i] == null))
                {
                    if (generalManual.ActuadoresBas[i] != null)
                    {
                        aR.Add(new ActuadorP(generalManual.ActuadoresBas[i]));
                    }
                }
            }
            for (int i = 0; i <= generalManual.ActuadoresWrk.Length - 1; i++)
            {
                if (!(generalManual.ActuadoresWrk[i] == null))
                {
                    if (generalManual.ActuadoresWrk[i] != null)
                    {
                        aT.Add(new ActuadorP(generalManual.ActuadoresWrk[i]));
                    }
                }
            }
            //Agregar cualquier inicialización después de la llamada a InitializeComponent()
        }

        //UserControl reemplaza a Dispose para limpiar la lista de componentes.
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if ((components != null))
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            btRep = new Button();
            btTra = new Button();
            lbCom = new Label();
            lbSr1 = new Label();
            lbSt1 = new Label();
            Label1 = new Label();
            lbSr2 = new Label();
            lbSt2 = new Label();
            btRep.Click += btRep_Click;
            btTra.Click += btTra_Click;
            SuspendLayout();
            //
            //btRep
            //
            btRep.Location = new Point(124, 8);
            btRep.Name = "btRep";
            btRep.Size = new Size(88, 40);
            btRep.TabIndex = 0;
            btRep.Text = "Reposo";
            //
            //btTra
            //
            btTra.Location = new Point(516, 8);
            btTra.Name = "btTra";
            btTra.Size = new Size(88, 40);
            btTra.TabIndex = 1;
            btTra.Text = "Trabajo";
            //
            //lbCom
            //
            lbCom.BackColor = SystemColors.ControlLight;
            lbCom.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbCom.Location = new Point(236, 8);
            lbCom.Name = "lbCom";
            lbCom.Size = new Size(256, 32);
            lbCom.TabIndex = 2;
            lbCom.TextAlign = ContentAlignment.MiddleCenter;
            //
            //lbSr1
            //
            lbSr1.BackColor = Color.White;
            lbSr1.BorderStyle = BorderStyle.Fixed3D;
            lbSr1.Location = new Point(80, 56);
            lbSr1.Name = "lbSr1";
            lbSr1.Size = new Size(88, 20);
            lbSr1.TabIndex = 3;
            lbSr1.TextAlign = ContentAlignment.MiddleCenter;
            //
            //lbSt1
            //
            lbSt1.BackColor = Color.White;
            lbSt1.BorderStyle = BorderStyle.Fixed3D;
            lbSt1.Location = new Point(464, 60);
            lbSt1.Name = "lbSt1";
            lbSt1.Size = new Size(88, 20);
            lbSt1.TabIndex = 5;
            lbSt1.TextAlign = ContentAlignment.MiddleCenter;
            //
            //Label1
            //
            Label1.BackColor = Color.LightGray;
            Label1.Location = new Point(-8, 88);
            Label1.Name = "Label1";
            Label1.Size = new Size(792, 16);
            Label1.TabIndex = 7;
            //
            //lbSr2
            //
            lbSr2.BackColor = Color.White;
            lbSr2.BorderStyle = BorderStyle.Fixed3D;
            lbSr2.Location = new Point(184, 56);
            lbSr2.Name = "lbSr2";
            lbSr2.Size = new Size(88, 20);
            lbSr2.TabIndex = 8;
            lbSr2.TextAlign = ContentAlignment.MiddleCenter;
            //
            //lbSt2
            //
            lbSt2.BackColor = Color.White;
            lbSt2.BorderStyle = BorderStyle.Fixed3D;
            lbSt2.Location = new Point(568, 60);
            lbSt2.Name = "lbSt2";
            lbSt2.Size = new Size(88, 20);
            lbSt2.TabIndex = 9;
            lbSt2.TextAlign = ContentAlignment.MiddleCenter;
            //
            //controlSensores
            //
            Controls.Add(lbSt2);
            Controls.Add(lbSr2);
            Controls.Add(Label1);
            Controls.Add(lbSt1);
            Controls.Add(lbSr1);
            Controls.Add(lbCom);
            Controls.Add(btTra);
            Controls.Add(btRep);
            Name = "controlSensores";
            Size = new Size(792, 104);
            ResumeLayout(false);
        }

        #endregion

        private readonly List<ActuadorP> aR = new List<ActuadorP>();
        private readonly List<ActuadorP> aT = new List<ActuadorP>();

        private readonly System.Windows.Forms.Control[] lsR;
        private readonly System.Windows.Forms.Control[] lsT;
        private readonly List<ISensor> sR = new List<ISensor>();
        private readonly List<ISensor> sT = new List<ISensor>();

        #region IRefrescable Members

        public virtual void Refresh_()
        {
            try
            {
                int i;
                for (i = 0; i <= sR.Count - 1; i++)
                {
                    lsR[i].BackColor = sR[i].Value() ? Color.Green : Color.Yellow;
                }
                for (i = 0; i <= sT.Count - 1; i++)
                {
                    lsT[i].BackColor = sT[i].Value() ? Color.Green : Color.Yellow;
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion

        public event click_EventHandler click_;

        protected virtual void btRep_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= aT.Count - 1; i++)
            {
                aT[i].Activate(false);
            }
            for (int i = 0; i <= aR.Count - 1; i++)
            {
                aR[i].Activate(true);
            }
        }

        protected void btTra_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= aT.Count - 1; i++)
            {
                aT[i].Activate(true);
            }
            for (int i = 0; i <= aR.Count - 1; i++)
            {
                aR[i].Activate(false);
            }
        }
    }
}