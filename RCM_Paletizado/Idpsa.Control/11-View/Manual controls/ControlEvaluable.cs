using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.View
{
    public partial class ControlEvaluable : UserControl,IRefrescable
    {
        private IEvaluable _evaluable;
        private ControlEvaluable()
        {
            InitializeComponent();
        }

        public ControlEvaluable(Manual manual)
        {
            InitializeComponent();
            Initialize(manual); 
        }

        private void Initialize(Manual manual)
        {
            _evaluable = (IEvaluable)manual.RepresentedInstance;
            lbCom.Text = manual.Description;            
        }



        #region Miembros de IRefrescable

        public void RefreshView()
        {
            lbSensor.Text = _evaluable.ToString();

            if (_evaluable.Value())
            {
                lbSensor.BackColor = Color.Green;
            }
            else
            {
                lbSensor.BackColor = Color.Yellow;
            }
            
        }

        #endregion
    }

   
}
