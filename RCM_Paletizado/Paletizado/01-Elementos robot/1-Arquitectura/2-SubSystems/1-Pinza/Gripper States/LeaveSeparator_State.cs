using System;
using System.Collections.Generic;
using Idpsa.Control.Engine;
using Idpsa.Control.Tool;
using Idpsa.Paletizado;
using Idpsa.Paletizado.Definitions;

public class LeaveSeparator_State : IGantryState
{
    private readonly GripperContext _context;
    private readonly IdpsaSystemPaletizado _sys;
    private Lines _lines;

    public LeaveSeparator_State()
    {
        _sys = ControlLoop<IdpsaSystemPaletizado>.Instance.Sys;
        _lines = _sys.Lines;
        _context = GripperContext.GetInstance();
    }

    #region IGantryState Members

    public Dictionary<string, object> GetChainParams()
    {
        var parameters = new Dictionary<string, object>();

        Point3D position = ((IPaletizer) _context.LeaveContainer).PositionPutSeparator();
        parameters.Add(LeftBoardParams.position, position);

        return parameters;
    }

    public bool NextState()
    {
        _context.Chain = Chains.Transfer;
        _context.State = new TransferState_State(ElementLeft);
        return true;
    }

    public void ElementLeft()
    {
        ((IPaletizer) _context.LeaveContainer).ElementAdded(_sys.Gripper.ElementQuitted());
    }

    public void ElementCatched()
    {
        throw new NotImplementedException();
    }

    #endregion
}