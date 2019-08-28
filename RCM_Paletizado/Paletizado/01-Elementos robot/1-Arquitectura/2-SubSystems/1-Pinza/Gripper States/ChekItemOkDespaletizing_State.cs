using System;
using System.Collections.Generic;
using Idpsa;
using Idpsa.Control.Engine;
using Idpsa.Control.Tool;
using Idpsa.Paletizado;
using Idpsa.Paletizado.Definitions;

public class ChekItemOkDespaletizing_State : IGantryState
{
    #region Miembros de IGripperState

    private readonly GripperContext _context;
    private readonly Lines _lines;
    private readonly IdpsaSystemPaletizado _sys;

    public ChekItemOkDespaletizing_State()
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
        if (!box.IsMinimunWeight())
        {
            //_context.LeaveContainer = _lines.ManualReprocesor;
            //primer intento
            //_context.Chain = Chains.CatchItem;
            //_context.State = new CathItem_State();
            //segundo intento
            _context.Chain = Chains.LeaveItem;
            _context.State = new LeaveItem_State();
            //MessageBox.Show("La caja no se ha recogido correctamente. Reintentando captura.");
            //Lanzamos evento para mostrar mensaje
            _sys.Lines.Line1.BoxNotCatchedDespaletizing = true;
        }
        else
        {
            _context.Chain = Chains.LeaveItem;
            _context.State = new LeaveItem_State();
            _sys.Lines.Line1.BoxNotCatchedDespaletizing = false;
        }

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