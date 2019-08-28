using Idpsa.Control.Sequence;

namespace Idpsa.Control.Manuals
{
    public interface IDynamicManual : IChainControllersOwner
    {
        IEvaluable LeftCondition { get; }
        IEvaluable RightCondition { get; }
        IEvaluable CenterCondition { get; }
        DynamicStepBody LeftStep { get; }
        DynamicStepBody RightStep { get; }
        DynamicStepBody CenterStep { get; }
        string LeftStepName { get; }
        string RightStepName { get; }
        string CenterStepName { get; }
    }
}