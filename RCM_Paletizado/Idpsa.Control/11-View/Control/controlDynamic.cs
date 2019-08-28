using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Idpsa.Control.Component;
using Idpsa.Control.Sequence;
using Idpsa.Control.Manuals;


namespace Idpsa.Control.View
{
    public partial class ControlDynamic : UserControl, IRun, IRefrescable 
    {

        private Action _refresh;
        private DynamicRunner _dynamicRuner;
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
            btStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f);
            lbStepComent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8f);
            lbTitle.Text = manual.Descripcion;
            _dynamicManual = (IDynamicManual)manual.RepresentedInstance;
            _dynamicRuner = new DynamicRunner(manual.Descripcion, () => true, (IChainControllersOwner)_dynamicManual);
            _dynamicRuner.Finalized += new Action(() => EnableButtons(true)); 
            TreatCondition(_dynamicManual.LeftCondition, lbCondition1);
            TreatCondition(_dynamicManual.CenterCondition, lbCondition2);
            TreatCondition(_dynamicManual.RightCondition, lbCondition3);
            TreatButton(_dynamicManual.LeftStep, _dynamicManual.LeftStepName ,bt1);
            TreatButton(_dynamicManual.CenterStep, _dynamicManual.CenterStepName, bt2);
            TreatButton(_dynamicManual.RightStep, _dynamicManual.RightStepName, bt3);
            _refresh+=new Action(()=>lbStepComent.Text=_dynamicRuner.CurrentStep.Comentario);
            
            EnableButtons(true);
        }


        private void TreatCondition(IBitEvaluable condition,Label label)
        {
            if (condition != null)
            {
                label.Text = condition.ToString();
                _refresh += new Action(() => { label.BackColor = condition.Value() ? Color.Green : Color.Yellow; });               

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

                button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8f);
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
            DynamicStepBody stepBody;
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

            _dynamicRuner.Start(stepBody);            

        }


        private void btStop_Click(object sender, EventArgs e)
        {            
            _dynamicRuner.Stop();
            EnableButtons(true);
        }


        #region Miembros de IRun
       
        public void Run()
        {            
            _dynamicRuner.Run(); 
        }

        #endregion

        #region Miembros de IRefrescable

        public void RefreshView()
        {
            _refresh();
        }

        #endregion
    }
}
