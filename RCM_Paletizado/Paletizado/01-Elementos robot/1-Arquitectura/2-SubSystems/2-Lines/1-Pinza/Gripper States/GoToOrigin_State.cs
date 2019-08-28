using System;
using System.Collections.Generic;
using Idpsa;
using Idpsa.Control.Engine;
using Idpsa.Paletizado;
using Idpsa.Paletizado.Definitions;

//MDG.2011-06-15

public class GoToOrigin_State : IGantryState
{
    private readonly IdpsaSystemPaletizado _sys;
    private GripperContext _context;
    private Lines _lines;

    public GoToOrigin_State()
    {
        _sys = ControlLoop<IdpsaSystemPaletizado>.Instance.Sys;
        _lines = _sys.Lines;
        _context = GripperContext.GetInstance();
    }

    #region IGantryState Members

    public Dictionary<string, object> GetChainParams()
    {
        var parameters = new Dictionary<string, object>();
        PointSpin3D positionA = null;
        Gripper.Extensor extensor = default(Gripper.Extensor);

        //Va a banda de etiquetado
        positionA = Geometria.PosOrigenBandaEntrada; //MDG.2011-06-15.Hora19:35
        //positionA = new PointSpin3D(Spin.S0, (775 + 20), 1370, Gripper.NegativeZLimit);//Para que se mueva a una posicion fija       
        //positionA = Geometria.PosBandaEntrada;// _lines.BandaEtiquetado.Position;
        //positionA.X = Geometria.PosBandaEntrada.X + 20;//MDG.2011-06-15.Aumento margen para que no coincida del todo la posicion
        ////positionA.Y = _lines.BandaEtiquetado.Position.Y;//MDG.2011-06-15.Aumento margen para que no coincida del todo la posicion
        //positionA.Z = Gripper.NegativeZLimit;//22//La posicion de destino es el limite arriba del todo
        //positionA.Spin = _lines.BandaEtiquetado.Position.Spin;
        extensor = Gripper.Extensor.Retract;

        parameters.Add(GoToOriginParams.spin, positionA.Spin);
        parameters.Add(GoToOriginParams.position, positionA);
        parameters.Add(GoToOriginParams.extensor, extensor);

        return parameters;
    }

    public bool NextState()
    {
        //_context.Chain = Chains.Espera;
        //_context.State = new Wait_State();
        return false;
    }

    public void ElementLeft()
    {
        throw new NotImplementedException();
    }

    public void ElementCatched()
    {
        throw new NotImplementedException();
    }

    #endregion
}