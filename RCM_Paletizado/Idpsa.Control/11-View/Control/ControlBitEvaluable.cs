using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Idpsa.Control.Manuals;


namespace Idpsa.Control.View
{
    public partial class ControlBitEvaluable : UserControl,IRefrescable
    {
        private IBitEvaluable _bitEvaluable;
        private ControlBitEvaluable()
        {
            InitializeComponent();
        }

        public ControlBitEvaluable(Manual manual)
        {
            InitializeComponent();
            Initialize(manual); 
        }

        private void Initialize(Manual manual)
        {
            _bitEvaluable = (IBitEvaluable)manual.RepresentedInstance;
            lbCom.Text = manual.Descripcion;            
        }



        #region Miembros de IRefrescable

        public void RefreshView()
        {
            lbSensor.Text = _bitEvaluable.ToString();

            if (_bitEvaluable.Value())
            {
                this.lbSensor.BackColor = Color.Green;
            }
            else
            {
                this.lbSensor.BackColor = Color.Yellow;
            }
            
        }

        #endregion
    }

   
}
