using System;
using System.Collections.Generic;
using Idpsa;
using Idpsa.Control.Engine;
using Idpsa.Paletizado;
using Idpsa.Paletizado.Definitions;

public class CathItem_State : IGantryState
{
    private readonly GripperContext _context;
    private readonly Lines _lines;
    private readonly IdpsaSystemPaletizado _sys;

    public CathItem_State()
    {
        _sys = ControlLoop<IdpsaSystemPaletizado>.Instance.Sys;
        _lines = _sys.Lines;
        _context = GripperContext.GetInstance();
    }

    #region IGantryState Members

    public Dictionary<string, object> GetChainParams()
    {
        var parameters = new Dictionary<string, object>();
        PointSpin3D position = null;
        Gripper.Extensor extensor = default(Gripper.Extensor);

        if (_context.CatchContainer.Location.Is(Locations.Paletizado))
        {
            position = ((IPaletizer) _context.CatchContainer).PositionItem();
            extensor = Gripper.Extensor.Dead;
        }
        else if (_context.CatchContainer.Location.Is(Locations.Entrada))
        {
            position = _lines.BandaEtiquetado.Position;
            extensor = Gripper.Extensor.Retract;
        }
        else if (_context.CatchContainer.Location.Is(Locations.Reproceso))
        {
            position = _lines.ManualReprocesor.PositionCatch;
            extensor = Gripper.Extensor.Retract;
        }

        parameters.Add(CatchBoxParams.spin, position.Spin);
        parameters.Add(CatchBoxParams.position, position);
        parameters.Add(CatchBoxParams.extensor, extensor);

        return parameters;
    }

    public bool NextState()
    {
        if (_context.CatchContainer.Location.Is(Locations.Entrada)
            || _context.CatchContainer.Location.Is(Locations.Reproceso))
        //MDG.2010-12-01.Despues de reproceso se vuelve a chequear el peso
        {
            _context.Chain = Chains.CheckItemOK;
            _context.State = new ChekItemOk_State();
        }
        else if (_context.CatchContainer.Location.Is(Locations.Paletizado2Japonesa))
        {
            //MDG.2013-04-03. Para que compruebe el peso en despaletizado
            _context.Chain = Chains.CheckItemOKDespaletizing;
            _context.State = new ChekItemOkDespaletizing_State();
        }
        else
        {
            _context.Chain = Chains.LeaveItem;
            _context.State = new LeaveItem_State();
        }
        return true;
    }

    public void ElementLeft()
    {
        throw new NotImplementedException();
    }

    public void ElementCatched()
    {
        IElement box = _context.CatchContainer.ElementQuitted();
        _sys.Gripper.ElementAdded(box);
    }

    #endregion
}