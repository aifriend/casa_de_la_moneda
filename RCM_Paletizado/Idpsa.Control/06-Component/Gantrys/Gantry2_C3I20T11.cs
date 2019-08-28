using System;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Manuals;
using Idpsa.Control.Subsystem;
using Idpsa.Control.Tool;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Gantry2_C3I20T11 : IRi, ISpecialDevice, ISpecialDeviceOwner, IManualsProvider
    {
        private readonly CompaxC3I20T11 _x;
        private readonly CompaxC3I20T11 _y;
        private bool _xMovCheck;
        private bool _yMovCheck;

        public Gantry2_C3I20T11(CompaxC3I20T11 x, CompaxC3I20T11 y)
        {
            _x = x;
            _y = y;
        }

        public CompaxC3I20T11 X
        {
            get { return _x; }
        }

        public CompaxC3I20T11 Y
        {
            get { return _y; }
        }

        #region IRi Members

        public void Ri()
        {
            _x.Parada();
            _y.Parada();
        }

        #endregion

        #region Miembros de ISpecialDevice

        public bool InError()
        {
            return (_x.InError() | _y.InError());
        }

        public void OnErrorAction()
        {
            _x.OnErrorAction();
            _y.OnErrorAction();
        }

        #endregion

        #region Miembros de ISpecialDeviceOwner

        public IEnumerable<ISpecialDevice> GetSpecialDevices()
        {
            return new List<ISpecialDevice> {_x, _y};
        }

        #endregion

        #region Miembros de IManualProvider

        public IEnumerable<Manual> GetManualsRepresentations()
        {
            return _x.GetManualsRepresentations().Concat(_y.GetManualsRepresentations()
                );
        }

        #endregion

        private void EndMovCheckSet()
        {
            _xMovCheck = true;
            _yMovCheck = true;
        }

        public Point2D Position()
        {
            return new Point2D(_x.Posicion, _y.Posicion);
        }

        public Point2D TargetPosition()
        {
            return new Point2D(_x.TargetPosition, _y.TargetPosition);
        }

        public Point2D Velocity()
        {
            return new Point2D(_x.Velocidad, _y.Velocidad);
        }

        public Point2D Torke()
        {
            return new Point2D(_x.Torque, _y.Torque);
        }

        public bool InPosition(Point2D position, double error)
        {
            return (Position().CoorRespecto(position).Modulo() < error);
        }

        public bool InTargetPosition(double error)
        {
            return InPosition(TargetPosition(), error);
        }

        public bool StartMov(Point2D position, int v, int a)
        {
            var initialPosition = new Point2D(_x.Posicion, _y.Posicion);
            Point2D rUnitario = position.CoorRespecto(initialPosition).Unitario() ?? new Point2D(0, 0);
            Point2D velocity = rUnitario.Mult(v).CompPositivas();

            if (_x.MaxVelocity != 0 && velocity.X > _x.MaxVelocity)
            {
                velocity.X = _x.MaxVelocity;
            }
            if (_y.MaxVelocity != 0 && velocity.Y > _y.MaxVelocity)
            {
                velocity.Y = _y.MaxVelocity;
            }

            Point2D aceleration = rUnitario.Mult(a).CompPositivas();


            //MDG.2011-06-02.Velocidades minimas=1 para que nos se pare con v=0
            if (velocity.X < 5) velocity.X = 5;
            if (velocity.Y < 5) velocity.Y = 5;
            if (aceleration.X < 5) aceleration.X = 5;
            if (aceleration.Y < 5) aceleration.Y = 5;

            bool xStarted = _x.StartMov((int)position.X, (int)velocity.X, (int)aceleration.X);
            bool yStarted = _y.StartMov((int)position.Y, (int)velocity.Y, (int)aceleration.Y);

            if (xStarted ^ yStarted)
            {
                _x.Parada();
                _y.Parada();
            }

            return (xStarted && yStarted);
        }

        public bool StartMovIndendiente(Point2D position, Point2D v, Point2D a)
        {
            var initialPosition = new Point2D(_x.Posicion, _y.Posicion);
            bool xStarted = false;
            bool yStarted = false;
            EndMovCheckSet();

            _xMovCheck = !(Math.Abs(initialPosition.X - position.X) <= 1);
            _yMovCheck = !(Math.Abs(initialPosition.Y - position.Y) <= 1);

            if (_xMovCheck)
                xStarted = _x.StartMov((int)position.X, (int)v.X, (int)a.X);

            if (_yMovCheck)
                yStarted = _y.StartMov((int)position.Y, (int)v.Y, (int)a.Y);

            if (xStarted ^ yStarted)
            {
                _x.Parada();
                _y.Parada();
            }

            return (xStarted && yStarted);
        }

        public bool EndMov()
        {
            return (X.EndMov() && Y.EndMov());
        }

        public Dictionary<string, CompaxC3I20T11> GetParkers()
        {
            return new Dictionary<string, CompaxC3I20T11> {{_x.Name, _x}, {_y.Name, _y}};            
        }
    }
}