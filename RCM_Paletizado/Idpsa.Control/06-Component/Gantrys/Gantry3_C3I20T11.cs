using System;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Manuals;
using Idpsa.Control.Subsystem;
using Idpsa.Control.Tool;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Gantry3_C3I20T11 : IRi, ISpecialDevice, ISpecialDeviceOwner, IManualsProvider
    {
        private readonly CompaxC3I20T11 _x;
        private readonly CompaxC3I20T11 _y;
        private readonly CompaxC3I20T11 _z;


        public Gantry3_C3I20T11(CompaxC3I20T11 parX, CompaxC3I20T11 parY, CompaxC3I20T11 parZ)
        {
            _x = parX;
            _y = parY;
            _z = parZ;
        }

        public CompaxC3I20T11 X
        {
            get { return _x; }
        }

        public CompaxC3I20T11 Y
        {
            get { return _y; }
        }

        public CompaxC3I20T11 Z
        {
            get { return _z; }
        }

        #region IRi Members

        public void Ri()
        {
            _x.Parada();
            _y.Parada();
            _z.Parada();
        }

        #endregion

        #region Miembros de ISpecialDevice

        public bool InError()
        {
            return (_x.MotorError() || _y.MotorError() || _z.MotorError());
        }

        public void OnErrorAction()
        {
            _x.OnErrorAction();
            _y.OnErrorAction();
            _z.OnErrorAction();
        }

        #endregion

        #region Miembros de ISpecialDeviceOwner

        public IEnumerable<ISpecialDevice> GetSpecialDevices()
        {
            return new List<ISpecialDevice> {_x, _y, _z};
        }

        #endregion

        #region Miembros de IManualProvider

        public IEnumerable<Manual> GetManualsRepresentations()
        {
            return _x.GetManualsRepresentations().Concat(
                _y.GetManualsRepresentations().Concat(
                    _z.GetManualsRepresentations())
                );
        }

        #endregion

        public Point3D Position()
        {
            return new Point3D(_x.Posicion, _y.Posicion, _z.Posicion);
        }

        public Point3D TargetPosition()
        {
            return new Point3D(_x.TargetPosition, _y.TargetPosition, _z.TargetPosition);
        }

        public Point3D Velocity()
        {
            return new Point3D(_x.Velocidad, _y.Velocidad, _z.Velocidad);
        }

        public Point2D Torke()
        {
            return new Point2D(_x.Torque, _y.Torque);
        }

        public bool InPosition(Point3D posicion, double error)
        {
            return (Position().CoorRespecto(posicion).Modulo() < error);
        }

        public bool InTargetPosition(double error)
        {
            return InPosition(TargetPosition(), error);
        }


        public bool StartMov(Point3D position, int v, int a)
        {
            var initialPosition = new Point3D(_x.Posicion, _y.Posicion, _z.Posicion);
            Point3D rUnitario = position.CoorRespecto(initialPosition).Unitario() ?? new Point3D(0, 0, 0);
            Point3D velocity = rUnitario.Mult(v).CompPositivas();
            Point3D aceleration = rUnitario.Mult(a).CompPositivas();

            //MDG.2011-06-02.Velocidades minimas=1 para que nos se pare con v=0
            if (velocity.X < 5) velocity.X = 5;
            if (velocity.Y < 5) velocity.Y = 5;
            if (velocity.Z < 5) velocity.Z = 5;
            if (aceleration.X < 5) aceleration.X = 5;
            if (aceleration.Y < 5) aceleration.Y = 5;
            if (aceleration.Z < 5) aceleration.Z = 5;

            bool XStarted = _x.StartMov((int)position.X, (int)velocity.X, (int)aceleration.X);
            bool YStarted = _y.StartMov((int)position.Y, (int)velocity.Y, (int)aceleration.Y);
            bool ZStarted = _z.StartMov((int)position.Z, (int)velocity.Z, (int)aceleration.Z);

            bool allStarted = XStarted && YStarted && ZStarted;
            bool anyStarted = XStarted || YStarted || ZStarted;

            if (allStarted != anyStarted)
            {
                _x.Parada();
                _y.Parada();
                _z.Parada();
            }

            return allStarted;
        }

        public bool StartMovIndendiente(Point3D Pf, Point3D v, Point3D a)
        {
            bool xStarted = false;
            bool yStarted = false;
            bool zStarted = false;

            xStarted = _x.StartMov((int)Pf.X, (int)v.X, (int)a.X);
            yStarted = _y.StartMov((int)Pf.Y, (int)v.Y, (int)a.Y);
            zStarted = _z.StartMov((int)Pf.Z, (int)v.Z, (int)a.Z);

            bool allStarted = xStarted && yStarted && zStarted;
            bool anyStarted = xStarted || yStarted || zStarted;

            if (allStarted != anyStarted)
            {
                _x.Parada();
                _y.Parada();
                _z.Parada();
            }
            return allStarted;
        }

        public bool EndMov()
        {
            return (X.EndMov() && Y.EndMov() && Z.EndMov());
        }

        public Dictionary<string, CompaxC3I20T11> GetParkers()
        {
            var parkers = new Dictionary<string, CompaxC3I20T11>();
            parkers.Add(_x.Name, _x);
            parkers.Add(_y.Name, _y);
            parkers.Add(_z.Name, _z);
            return parkers;
        }
    }
}