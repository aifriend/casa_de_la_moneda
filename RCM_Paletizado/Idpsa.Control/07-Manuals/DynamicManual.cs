using System.Collections.Generic;
using Idpsa.Control.Sequence;

namespace Idpsa.Control.Manuals
{
    public class DynamicManual : IDynamicManual
    {
        private readonly IChainControllersOwner _controllersOwner;
        private IEvaluable _centerCondition;
        private DynamicStepBody _centerStep;
        private string _centerStepName;
        private IEvaluable _leftConditon;
        private DynamicStepBody _leftStep;
        private string _leftStepName;
        private IEvaluable _rightCondition;
        private DynamicStepBody _rightStep;
        private string _rightStepName;

        #region Miembros de IDynamicManual

        public DynamicManual(IChainControllersOwner controllersOwner)
        {
            _controllersOwner = controllersOwner;
        }

        IEvaluable IDynamicManual.LeftCondition
        {
            get { return _leftConditon; }
        }

        IEvaluable IDynamicManual.RightCondition
        {
            get { return _rightCondition; }
        }

        IEvaluable IDynamicManual.CenterCondition
        {
            get { return _centerCondition; }
        }

        DynamicStepBody IDynamicManual.LeftStep
        {
            get { return _leftStep; }
        }

        DynamicStepBody IDynamicManual.RightStep
        {
            get { return _rightStep; }
        }

        DynamicStepBody IDynamicManual.CenterStep
        {
            get { return _centerStep; }
        }

        string IDynamicManual.LeftStepName
        {
            get { return _leftStepName; }
        }

        string IDynamicManual.RightStepName
        {
            get { return _rightStepName; }
        }

        string IDynamicManual.CenterStepName
        {
            get { return _centerStepName; }
        }

        IEnumerable<IChainController> IChainControllersOwner.GetChainControllers()
        {
            return _controllersOwner.GetChainControllers();
        }

        #endregion

        public DynamicManual WithLeftCondition(IEvaluable condition)
        {
            _leftConditon = condition;
            return this;
        }

        public DynamicManual WithRightCondition(IEvaluable condition)
        {
            _rightCondition = condition;
            return this;
        }

        public DynamicManual WithCenterCondition(IEvaluable condition)
        {
            _centerCondition = condition;
            return this;
        }

        public DynamicManual WithLeftStep(DynamicStepBody step)
        {
            _leftStep = step;
            return this;
        }

        public DynamicManual WhitRightStep(DynamicStepBody step)
        {
            _rightStep = step;
            return this;
        }

        public DynamicManual WithCenterStep(DynamicStepBody step)
        {
            _centerStep = step;
            return this;
        }

        public DynamicManual WithLeftStepName(string stepName)
        {
            _leftStepName = stepName;
            return this;
        }

        public DynamicManual WhitRightStepName(string stepName)
        {
            _rightStepName = stepName;
            return this;
        }

        public DynamicManual WithCenterStepName(string stepName)
        {
            _centerStepName = stepName;
            return this;
        }
    }
}