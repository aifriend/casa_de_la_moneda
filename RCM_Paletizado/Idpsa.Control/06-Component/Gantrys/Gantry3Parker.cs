using System;
using Idpsa.Control.Sequence;
using Idpsa.Control.Tool;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Gantry3Parker
    {
        private readonly Parker _x;
        private readonly Parker _y;
        private readonly Parker _z;
        private bool _xEndMov;
        private bool _xEndMovCheck;
        private bool _yEndMov;
        private bool _yEndMovCheck;
        private bool _zEndMov;
        private bool _zEndMovCheck;

        public Gantry3Parker(Parker x, Parker y, Parker z)
        {
            if (x == null)
                throw new ArgumentNullException("x");
            if (y == null)
                throw new ArgumentNullException("y");
            if (z == null)
                throw new ArgumentNullException("z");
            _x = x;
            _y = y;
            _z = z;
        }

        public bool zEndMov
        {
            get { return _zEndMov; }
        }

        public Parker X
        {
            get { return _x; }
        }

        public Parker Y
        {
            get { return _y; }
        }

        public Parker Z
        {
            get { return _z; }
        }

        private void EndMovReset()
        {
            _xEndMov = false;
            _yEndMov = false;
            _zEndMov = false;
        }

        private void EndMovCheckSet()
        {
            _xEndMovCheck = true;
            _yEndMovCheck = true;
            _zEndMovCheck = true;
        }

        public bool StartMovXY(Point3D targetPoint, int v, int a)
        {
            if (targetPoint == null)
                throw new ArgumentNullException("targetPoint");
            var initialPosition = new Point3D(_x.Posicion, _y.Posicion, _z.Posicion);
            Point3D rUnitario = targetPoint.CoorRespecto(initialPosition).Unitario();
            Point3D velocity = rUnitario.Mult(v).CompPositivas();
            Point3D aceleration = rUnitario.Mult(a).CompPositivas();
            EndMovCheckSet();

            if (Math.Abs(initialPosition.X - targetPoint.X) <= 1)            
                _xEndMovCheck = false;
            
            if (Math.Abs(initialPosition.Y - targetPoint.Y) <= 1)            
                _yEndMovCheck = false;
            
            bool noInicio = (!_x.Inicio) & (!_y.Inicio);
            bool securityResult = true;
            if (noInicio)
            {
                if (_xEndMovCheck)
                {
                    securityResult = true & _x.StartMov((int)(targetPoint.X), (int)velocity.X, (int)aceleration.X);
                }
                if (_yEndMovCheck)
                {
                    securityResult = securityResult & _y.StartMov((int)targetPoint.Y, (int)velocity.Y, (int)aceleration.Y);
                }
            }
            if (securityResult == false)
            {
                _x.Parada = true;
                _y.Parada = true;
                _z.Parada = true;
            }
            return noInicio;
        }

        public bool StartMovXYIndendiente(Point3D position, Point2D v, Point2D a)
        {
            if (position == null)
                throw new ArgumentNullException("position");
            var initialPosition = new Point3D(_x.Posicion, _y.Posicion, _z.Posicion);
            EndMovCheckSet();
            if (Math.Abs(initialPosition.X - position.X) <= 1)            
                _xEndMovCheck = false;
            if (Math.Abs(initialPosition.Y - position.Y) <= 1)           
                _yEndMovCheck = false;
            
            bool noInicio = (!_x.Inicio) & (!_y.Inicio);
            bool securityResult = true;
            if (noInicio)
            {
                if (_xEndMovCheck)
                {
                    securityResult = true & _x.StartMov((int)position.X, (int)v.X, (int)a.X);
                }
                if (_yEndMovCheck)
                {
                    securityResult = securityResult & _y.StartMov((int)position.Y, (int)v.Y, (int)a.Y);
                }
            }
            if (securityResult == false)
            {
                _x.Parada = true;
                _y.Parada = true;
                _z.Parada = true;
            }
            return noInicio;
        }

        public bool EndMovXY()
        {
            bool result = false;
            if (X.EndMov() | !_xEndMovCheck)
            {
                _xEndMov = true;
            }
            if (Y.EndMov() | !_yEndMovCheck)
            {
                _yEndMov = true;
            }
            if ((_xEndMov & _yEndMov))
            {
                result = true;
                EndMovReset();
                EndMovCheckSet();
            }
            return result;
        }

        public bool StartMov(Point3D position, int v, int a)
        {
            if (position == null)
                throw new ArgumentNullException("position");
            var initalPosition = new Point3D(_x.Posicion, _y.Posicion, _z.Posicion);
            Point3D rUnitario = position.CoorRespecto(initalPosition).Unitario();
            Point3D velocity = rUnitario.Mult(v).CompPositivas();
            Point3D aceleration = rUnitario.Mult(a).CompPositivas();
            EndMovCheckSet();
            if (Math.Abs(initalPosition.X - position.X) <= 1)            
                _xEndMovCheck = false;
            
            if (Math.Abs(initalPosition.Y - position.Y) <= 1)            
                _yEndMovCheck = false;
            
            if (Math.Abs(initalPosition.Z - position.Z) <= 1)           
                _zEndMovCheck = false;
            

            bool noInicio = (!_x.Inicio) & (!_y.Inicio) & (!_z.Inicio);
            bool securityResult = true;

            if (noInicio)
            {
                if (_xEndMovCheck)
                {
                    securityResult = true & _x.StartMov((int)position.X, (int)velocity.X, (int)aceleration.X);
                }
                if (_yEndMovCheck)
                {
                    securityResult = securityResult & _y.StartMov((int)position.Y, (int)velocity.Y, (int)aceleration.Y);
                }
                if (_zEndMovCheck)
                {
                    securityResult = securityResult & _z.StartMov((int)position.Z, (int)velocity.Z, (int)aceleration.Z);
                }
            }
            if (securityResult == false)
            {
                _x.Parada = true;
                _y.Parada = true;
                _z.Parada = true;
            }
            return noInicio;
        }

        public bool StartMov(Double R, Double Teta, Double Fi, Double v, Double a, Chain cad)
        {
            var Pi = new Point3D(_x.Posicion, _y.Posicion, _z.Posicion);
            Point3D Desp = Point3D.EsfToCart(R, Teta, Fi);
            Point3D Pf = Pi.Desplazado(Desp);
            return StartMov(Pf, (int)v, (int)a);
        }

        public bool EndMov()
        {
            bool result = false;
            if (X.EndMov() | !_xEndMovCheck)
            {
                _xEndMov = true;
            }
            if (Y.EndMov() | !_yEndMovCheck)
            {
                _yEndMov = true;
            }
            if (Z.EndMov() | !_zEndMovCheck)
            {
                _zEndMov = true;
            }
            if ((_x.EndMov() & _y.EndMov() & _z.EndMov()))
            {
                result = true;
                EndMovReset();
                EndMovCheckSet();
            }
            return result;
        }

        public Point3D Posicion()
        {
            return new Point3D(_x.Posicion, _y.Posicion, _z.Posicion);
        }

        public bool EnError()
        {
            return (_x.MotorError | _y.MotorError | _z.MotorError);
        }


    }
}