using System;
using System.Collections.Generic;
using Idpsa;
using Idpsa.Control.Engine;
using Idpsa.Control.Tool;
using Idpsa.Paletizado;
using Idpsa.Paletizado.Definitions;

public class ChekItemOk_State : IGantryState
{
    #region Miembros de IGripperState

    private readonly GripperContext _context;
    private readonly Lines _lines;
    private readonly IdpsaSystemPaletizado _sys;

    public ChekItemOk_State()
    {
        _sys = ControlLoop<IdpsaSystemPaletizado>.Instance.Sys;
        _lines = _sys.Lines;
        _context = GripperContext.GetInstance();
    }

    public Dictionary<string, object> GetChainParams()
    {
        var parameters = new Dictionary<string, object>();
        Point3D position = null;
        return parameters;
    }

    public bool NextState()
    {
        var box = (CajaPasaportes) _sys.Gripper.PeekElement();
        //if (!box.IsCorrectWeight()||!box.IsBarcodeReadedCorrect()) //ALVARO_20101125 (!box.IsBarcodeReadedCorrect())
        if (!box.IsCorrectWeight() || !box.IsBarcodeReadedCorrect() || !box.IsIdNotRepeated())
            //MDG.2011-06-16.Comprobamos tambien identificador no repetido en paletizado y banda de reproceso
        {
            _context.LeaveContainer = _lines.ManualReprocesor;
        }

        _context.Chain = Chains.LeaveItem;
        _context.State = new LeaveItem_State();

        return true;
    }

    public void ElementLeft()
    {
        throw new NotImplementedException();
    }

    public void ElementCatched()
    {
        _sys.Gripper.ElementAdded(_context.CatchContainer.ElementQuitted());
    }

    #endregion
}