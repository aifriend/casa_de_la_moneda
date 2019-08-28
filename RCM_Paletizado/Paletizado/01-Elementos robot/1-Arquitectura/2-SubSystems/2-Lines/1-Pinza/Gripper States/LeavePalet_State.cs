using System;
using System.Collections.Generic;
using Idpsa.Control.Engine;
using Idpsa.Control.Tool;
using Idpsa.Paletizado;
using Idpsa.Paletizado.Definitions;

public class LeavePalet_State : IGantryState
{
    private readonly GripperContext _context;
    private readonly IdpsaSystemPaletizado _sys;
    private readonly Action _transferedElementCatched;
    private Lines _lines;

    public LeavePalet_State(Action transferedElementCatched)
    {
        if (transferedElementCatched == null)
            throw new NullReferenceException("transferedElementCatched");

        _transferedElementCatched = transferedElementCatched;
        _sys = ControlLoop<IdpsaSystemPaletizado>.Instance.Sys;
        _lines = _sys.Lines;
        _context = GripperContext.GetInstance();
    }

    #region IGantryState Members

    public Dictionary<string, object> GetChainParams()
    {
        var parameters = new Dictionary<string, object>();

        Point3D position = ((IPaletizer) _context.LeaveContainer).PositionPalet();
        //PointSpin3D position = ((IPaletizer)_context.LeaveContainer).PositionPalet();//MDG.2011-03-14
        Spin spin = (((IPaletizer)_context.LeaveContainer).Palet.Type == PaletTypes.PaletVikex) ? Spin.S0 : Spin.S90; //Spin.S270;//Spin.S90;
            //MDG.2011-03-14
        position.Z += 40;
        Point2D securityPosition1 = null;
        Point3D securityPosition2 = null;


        if (_context.LeaveContainer.Location == Locations.Paletizado1Japonesa)
        {
            position = position.Desplazado(0, -30, 0);
            position.X = 2048;
        }

        if (_context.LeaveContainer.Location == Locations.Paletizado3Japonesa)
        {
            if (_context.CatchContainer is IPaletizer)
            {
                //MDG.2011-06-29.Coge palet de la mesa 2 al suelo, por lo que va directo
                position.X += 20; //60;
                securityPosition2 = position.Desplazado(40, -40, -40);
                securityPosition1 = new Point2D(securityPosition2.X, securityPosition2.Y);
            }
            else
            {
                securityPosition1 = new Point2D(5064, 280);
                securityPosition2 = position.Desplazado(40, -40, -40);
            }
        }
        else if (_context.LeaveContainer.Location == Locations.PaletizadoAlemana)
        {
            securityPosition2 = position.Desplazado(40, -40, -40);
            securityPosition1 = new Point2D(securityPosition2.X, securityPosition2.Y);
        }
        else
        {
            securityPosition1 = new Point2D(position.X, position.Y);
            securityPosition2 = position;
        }

        parameters.Add(LeftPaletParams.position, position);
        parameters.Add(LeftPaletParams.securityPosition1, securityPosition1);
        parameters.Add(LeftPaletParams.securityPosition2, securityPosition2);
        parameters.Add(LeftPaletParams.spin, spin); //MDG.2011-03-14

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
        IElement palet = _sys.Gripper.ElementQuitted();
        _context.LeaveContainer.ElementAdded(palet);
    }

    public void ElementCatched()
    {
        _transferedElementCatched();
    }

    #endregion
}