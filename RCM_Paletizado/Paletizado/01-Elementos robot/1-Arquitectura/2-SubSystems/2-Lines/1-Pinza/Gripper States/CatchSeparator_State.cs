using System;
using System.Collections.Generic;
using Idpsa;
using Idpsa.Control.Engine;
using Idpsa.Paletizado;
using Idpsa.Paletizado.Definitions;

public class CatchSeparator_State : IGantryState
{
    #region Miembros de IGripperState

    private readonly GripperContext _context;
    private readonly IdpsaSystemPaletizado _sys;
    private Lines _lines;

    public CatchSeparator_State()
    {
        _sys = ControlLoop<IdpsaSystemPaletizado>.Instance.Sys;
        _lines = null; //_sys.Lines;
        _context = GripperContext.GetInstance();
    }

    public Dictionary<string, object> GetChainParams()
    {
        var parameters = new Dictionary<string, object>();

        var separatorStore = (SeparatorStore) _context.CatchContainer;
        parameters.Add(CatchBoardParams.initialPosition, separatorStore.InitialPosition);
        parameters.Add(CatchBoardParams.finalPosition, separatorStore.FinalPosition);

        return parameters;
    }

    public bool NextState()
    {
        _context.Chain = Chains.LeaveSeparator;
        _context.State = new LeaveSeparator_State();
        return true;
    }

    public void ElementLeft()
    {
        throw new NotImplementedException();
    }

    public void ElementCatched()
    {
        var paletStore = (SeparatorStore) _context.CatchContainer;
        paletStore.MerorizeHight(_sys.Gripper.Position().Z);
        _sys.Gripper.ElementAdded(paletStore.ElementQuitted());
    }

    #endregion
}