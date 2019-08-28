using System;
using System.Collections.Generic;
using Idpsa;
using Idpsa.Control.Engine;
using Idpsa.Control.Tool;
using Idpsa.Paletizado;
using Idpsa.Paletizado.Definitions;

public class LeaveItem_State : IGantryState
{
    private readonly GripperContext _context;
    private readonly Lines _lines;
    private readonly IdpsaSystemPaletizado _sys;


    public LeaveItem_State()
    {
        _sys = ControlLoop<IdpsaSystemPaletizado>.Instance.Sys;
        _lines = _sys.Lines;
        _context = GripperContext.GetInstance();
    }

    #region IGantryState Members

    public Dictionary<string, object> GetChainParams()
    {
        var parameters = new Dictionary<string, object>();
        Gripper.Extensor extensor = default(Gripper.Extensor);
        PointSpin3D position = null;
        Point3D securityPositionFinal = null;
        var securityPositions = new List<Point3D>();


        if (_context.LeaveContainer.Location.Is(Locations.Paletizado))
        {
            var paletizer = (IPaletizer) _context.LeaveContainer;
            position = paletizer.PositionItem();
            position.Z -= 20;
            if (position.Z <= 20 && position.Z >= 0)
                position.Z = 21;    //2014MCR
            securityPositionFinal = CalculateSecurityPosition(position, paletizer);


            if ((_context.CatchContainer.Location == Locations.Entrada ||
                 _context.CatchContainer.Location == Locations.Reproceso))
            {
                if (_context.LeaveContainer.Location == Locations.Paletizado3Japonesa)
                {
                    securityPositions.Add(_sys.Geo.PosPaletStore1); //MCR
                    securityPositions.Add(new Point3D(4300, 350, 0));
                }
                else if (_context.LeaveContainer.Location == Locations.PaletizadoAlemana)
                {
                    securityPositions.Add(_sys.Geo.PosPaletStore1);
                }
            }
            else
            {
                if (paletizer.Settings.MinFlatsToDo>Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos&& _context.LeaveContainer.Location == Locations.Paletizado3Japonesa)
                {
                    var paletizer2 = (IPaletizer)_context.CatchContainer;
                    if (paletizer2.CurrentFlat == 3)
                    {
                        securityPositions.Add(_sys.Geo.PosPaletStore1);
                        securityPositions.Add(new Point3D(4300, 350, 0));
                    }
                }
            }
            securityPositions.Add(securityPositionFinal);
            extensor = Gripper.Extensor.Dead;
        }
        else if (_context.LeaveContainer.Location.Is(Locations.Reproceso))
        {
            extensor = Gripper.Extensor.Retract;
            position = _lines.ManualReprocesor.PositionLeave;
            securityPositions.Add(position);
        }



        parameters.Add(LeftBoxParams.spin, position.Spin);
        parameters.Add(LeftBoxParams.extensor, extensor);
        parameters.Add(LeftBoxParams.securityPositions, securityPositions);
        parameters.Add(LeftBoxParams.position, position);

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
        IPaletElement box = null;
        box = (IPaletElement) _sys.Gripper.ElementQuitted();
        _context.LeaveContainer.ElementAdded(box);
    }


    public void ElementCatched()
    {
        throw new NotImplementedException();
    }

    #endregion

    private PointSpin3D CalculateSecurityPosition(PointSpin3D position, IPaletizer paletizer)
    {
        PointSpin2D previousPosition = paletizer.CurrentMosaic.PreviousPosition();
        PointSpin2D currentPosition = paletizer.CurrentMosaic.Position();
        IPaletizable item = paletizer.CurrentMosaic.Item;

        bool previousElementOnTheLeft = false;
        bool previousElementOnTheFront =false;
        if (previousPosition != null)
        {
            var previousItemRectangle = new Rectangle(item.LxWith(previousPosition.Spin),
                                                      item.LyWith(previousPosition.Spin));
            previousItemRectangle = previousItemRectangle.CentrarEn(previousPosition).Expandido(new Point2D(0, 40));

            var currentItemRectangle = new Rectangle(item.LxWith(currentPosition.Spin),
                                                     item.LyWith(currentPosition.Spin));
            currentItemRectangle = currentItemRectangle.CentrarEn(currentPosition);


            if (currentItemRectangle.IntersectaCon(previousItemRectangle))
            {
                if (currentItemRectangle.Origen.Y - previousItemRectangle.Origen.Y > 0) 
                {
                    previousElementOnTheLeft = true;
                }
            }
            if (paletizer.CurrentMosaic.Positions()[paletizer.CurrentMosaic.Positions().Count - 1].X - paletizer.CurrentMosaic.Positions()[0].X > 0)
            {
                previousElementOnTheFront = true;//MCR2014
            }
        }

        double xShift = 25;
        double yShift = 50;
        double zShift = 0; /*20;*/
        double xSecurity = currentPosition.X;
        double ySecurity = currentPosition.Y;
        if (previousElementOnTheLeft)
        {
            ySecurity += yShift;
        }
        else
        {
            ySecurity -= yShift;
        }
        if (ySecurity >= 1860)
            ySecurity = 1859;

        if (previousElementOnTheFront)
        {
            xSecurity += xShift;
        }
        else
        {
            xSecurity -= xShift;
        }

        return new PointSpin3D(position.Spin, new Point3D(xSecurity, ySecurity, position.Z - zShift));
    }
}