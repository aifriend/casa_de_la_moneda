using System;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.Manuals;
using Idpsa.Control.Sequence;

namespace Idpsa.Control.View
{
    public partial class ControlDynamic : UserControl, IRun, IRefrescable 
    {
        private Action _refreshView;
        private DynamicChain _dynamicChain;
        private IDynamicManual _dynamicManual;


        private ControlDynamic()
        {
            InitializeComponent();
        }

        public ControlDynamic(Manual manual)
        {
            InitializeComponent();
            Initialize(manual); 
        }

        public void Initialize(Manual manual)
        {
            btStop.Font = new Font("Microsoft Sans Serif", 10f);
            lbStepComment.Font = new Font("Microsoft Sans Serif", 8f);
            lbTitle.Text = manual.Description;
            _dynamicManual = (IDynamicManual)manual.RepresentedInstance;
            _dynamicChain = new DynamicChain(manual.Description, _dynamicManual);
            _dynamicChain.Finalized += (() => EnableButtons(true)); 
            TreatCondition(_dynamicManual.LeftCondition, lbCondition1);
            TreatCondition(_dynamicManual.CenterCondition, lbCondition2);
            TreatCondition(_dynamicManual.RightCondition, lbCondition3);
            TreatButton(_dynamicManual.LeftStep, _dynamicManual.LeftStepName ,bt1);
            TreatButton(_dynamicManual.CenterStep, _dynamicManual.CenterStepName, bt2);
            TreatButton(_dynamicManual.RightStep, _dynamicManual.RightStepName, bt3);
            _refreshView+=(()=>lbStepComment.Text=_dynamicChain.CurrentStep.Comment);
            
            EnableButtons(true);
        }

        private void TreatCondition(IEvaluable condition,Label label)
        {
            if (condition != null)
            {
                label.Text = condition.ToString();
                _refreshView += (() => { label.BackColor = condition.Value() ? Color.Green : Color.Yellow; }); 
            }
            else
            {
                label.Visible = false;
            }
        }

        private void TreatButton(DynamicStepBody dynamicStep,string dynamicStepName,Button button)
        {
            if (dynamicStep != null)
            {
                if (!String.IsNullOrEmpty(dynamicStepName))
                    button.Text = dynamicStepName;

                button.Font = new Font("Microsoft Sans Serif", 8f);
                button.Click += buttonAction_Click;
            }
            else
            {
                button.Visible = false;   
            }
        }

        private void EnableButtons(bool enable)
        {
            bt1.Enabled = bt2.Enabled = bt3.Enabled = enable;
            btStop.Enabled = !enable;
        }

        private void buttonAction_Click(object sender, EventArgs e)
        {
            EnableButtons(false);
            DynamicStepBody stepBody = null;
            if (sender.Equals(bt1))
            {
                stepBody = _dynamicManual.LeftStep; 
            }
            else if (sender.Equals(bt2))
            {
                stepBody = _dynamicManual.CenterStep; 
            }
            else
            {
                stepBody = _dynamicManual.RightStep;
            }

            _dynamicChain.WithStepBody(stepBody)
                         .Start();           

        }

        private void btStop_Click(object sender, EventArgs e)
        {            
            _dynamicChain.Stop();
            EnableButtons(true);
        }


        #region Miembros de IRun
       
        public void Run()
        {            
            _dynamicChain.Run(); 
        }

        #endregion

        #region Miembros de IRefrescable

        public void RefreshView()
        {
            _refreshView();
        }

        #endregion
    }
}
