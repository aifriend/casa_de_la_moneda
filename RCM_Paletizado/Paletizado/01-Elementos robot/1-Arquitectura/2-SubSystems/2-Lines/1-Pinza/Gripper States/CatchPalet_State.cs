using System;
using System.Collections.Generic;
using Idpsa;
using Idpsa.Control.Engine;
using Idpsa.Control.Tool;
using Idpsa.Paletizado;
using Idpsa.Paletizado.Definitions;

public class CatchPalet_State : IGantryState
{
    #region Miembros de IGripperState

    private readonly GripperContext _context;
    private readonly IdpsaSystemPaletizado _sys;
    private Lines _lines;

    public CatchPalet_State()
    {
        _sys = ControlLoop<IdpsaSystemPaletizado>.Instance.Sys;
        _lines = _sys.Lines;
        _context = GripperContext.GetInstance();
    }

    public Dictionary<string, object> GetChainParams()
    {
        var parameters = new Dictionary<string, object>();
        Point3D position = null;
        Point3D positionNextPalet1 = null;
        Point3D positionNextPalet2 = null;
        Point3D positionNextPalet3 = null;
        Point3D positionNextPalet4 = null;
        Point3D positionNextPalet5 = null;
        //PointSpin3D position = null;//MDG.2011-03-14.Para admitir recogida del palet vikex
        Spin spin = Spin.S90; //Spin.S270;//Spin.S90; //MDG.2011-03-14.Para admitir recogida del palet vikex

        if (_context.CatchContainer is PaletStore)
        {
            position = ((PaletStore) _context.CatchContainer).Position();
            positionNextPalet1 = ((PaletStore) _context.CatchContainer).PositionNextPalet1();
                //MDG.2011-06-22.Segunda Posicion
            positionNextPalet2 = ((PaletStore) _context.CatchContainer).PositionNextPalet2();
                //MDG.2011-06-22.Tercera Posicion
            positionNextPalet3 = ((PaletStore) _context.CatchContainer).PositionNextPalet3();
                //MDG.2011-06-22.Cuarta Posicion
            positionNextPalet4 = ((PaletStore) _context.CatchContainer).PositionNextPalet4();
                //MDG.2011-06-27.Quinta Posicion
            positionNextPalet5 = ((PaletStore) _context.CatchContainer).PositionNextPalet5();
                //MDG.2011-06-27.Sexta Posicion
            spin = (((PaletStore)_context.CatchContainer).Palet.Type == PaletTypes.PaletVikex) ? Spin.S0 : Spin.S90; //Spin.S270;//Spin.S90;
                //MDG.2011-03-14
        }
        else if (_context.CatchContainer is IPaletizer)
        {
            if (((IPaletizer) _context.CatchContainer).Palet.Type == PaletTypes.PaletVikex)
                //MDG.2011-06-29.Cambio posicion de recogida de palet vikex para que lo coja mas centrado en X
                position = ((IPaletizer) _context.CatchContainer).PositionPalet().Desplazado(0, 0, 40);
            else
                position = ((IPaletizer) _context.CatchContainer).PositionPalet().Desplazado(0, 0, 40);

            positionNextPalet1 = position; //MDG.2011-06-22.Posicion repetida porque no es pila de palets
            positionNextPalet2 = position; //MDG.2011-06-22.Posicion repetida porque no es pila de palets
            positionNextPalet3 = position; //MDG.2011-06-22.Posicion repetida porque no es pila de palets
            positionNextPalet4 = position; //MDG.2011-06-22.Posicion repetida porque no es pila de palets
            positionNextPalet5 = position; //MDG.2011-06-22.Posicion repetida porque no es pila de palets

            spin = (((IPaletizer)_context.CatchContainer).Palet.Type == PaletTypes.PaletVikex) ? Spin.S0 : Spin.S90; //Spin.S270;//Spin.S90;
                //MDG.2011-03-14
        }

        parameters.Add(CatchPaletParams.position, position);
        parameters.Add(CatchPaletParams.positionNextPalet1, positionNextPalet1); //MDG.2011-06-22
        parameters.Add(CatchPaletParams.positionNextPalet2, positionNextPalet2); //MDG.2011-06-22
        parameters.Add(CatchPaletParams.positionNextPalet3, positionNextPalet3); //MDG.2011-06-22
        parameters.Add(CatchPaletParams.positionNextPalet4, positionNextPalet4); //MDG.2011-06-27
        parameters.Add(CatchPaletParams.positionNextPalet5, positionNextPalet5); //MDG.2011-06-27
        parameters.Add(CatchPaletParams.spin, spin); //MDG.2011-03-14

        return parameters;
    }

    public bool NextState()
    {
        _context.Chain = Chains.LeavePalet;
        _context.State = new LeavePalet_State(ElementCatched);
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