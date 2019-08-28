using System;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Sequence;
using Idpsa.Control.Tool;

namespace Idpsa.Control.Component
{
    public class Gantry2_C3I20T11_ChainController : Gantry2_C3I20T11, IChainControllersOwner
    {
        #region Algorithm enum

        public enum Algorithm
        {
            ConstantPositionWindow,
            VariablePositonWindow
        }

        #endregion

        private const double maxAngleModule = Math.PI / 2;
        private readonly ChainController _chainController;

        public Gantry2_C3I20T11_ChainController(Gantry2_C3I20T11 gantry)
            : base(gantry.X, gantry.Y)
        {
            _chainController = new ChainController();
            MoveChain_Steps();
            TrayectoryChain_Steps();
            TrayectoryWithVariablePosWindowsChain_Steps();
            EnableChain_Steps();
        }

        public Gantry2_C3I20T11_ChainController(CompaxC3I20T11 x, CompaxC3I20T11 y)
            : base(x, y)
        {
            _chainController = new ChainController();
            MoveChain_Steps();
            TrayectoryChain_Steps();
            TrayectoryWithVariablePosWindowsChain_Steps();
            EnableChain_Steps();
        }

        #region IChainControllersOwner Members

        public IEnumerable<IChainController> GetChainControllers()
        {
            return new List<IChainController> {_chainController};
        }

        #endregion

        public void MoveChain(Point2D p, int v, int a)
        {
            MoveChain(p, v, a, 0.0);
        }

        public void MoveChain(Point2D p, int v, int a, double posWindow)
        {
            _chainController.CallChain(Chains.MoveChain)
                .WithParam("p", p)
                .WithParam("v", v)
                .WithParam("a", a)
                .WithParam("posWindow", posWindow);
        }

        private void MoveChain_Steps()
        {
            Subchain chain = _chainController.AddChain(Chains.MoveChain);
            chain.AddParams("p", "v", "a", "posWindow");

            chain.Add(new Step("Iniciar moviento pinza",10000)).Task = () =>
            {
               if (StartMov(chain.Param<Point2D>("p"),chain.Param<int>("v"),chain.Param<int>("a")))
                   _chainController.NextStep();
            };

            chain.Add(new Step("Finalizar movimiento gantry", 20000)).Task = () =>
            {
                if (EndMov() ||
                    InPosition(chain.Param<Point2D>("p"),chain.Param<double>("posWindow")))
                    _chainController.Return();
            };
        }

        public void TrajectoryChain(IEnumerable<Point2D> points, int v, int a, double posWindow)
        {
            _chainController.CallChain(Chains.TrayectoryChain1)
                .WithParam("points", points)
                .WithParam("v", v)
                .WithParam("a", a)
                .WithParam("posWindow", posWindow);
        }

        public void TrajectoryChain(IEnumerable<Point2D> points, int v, int a, double maxPosWindow, double minPosWindow)
        {
            _chainController.CallChain(Chains.TrayectoryChain2)
                .WithParam("points", points)
                .WithParam("v", v)
                .WithParam("a", a)
                .WithParam("maxPosWindow", maxPosWindow)
                .WithParam("minPosWindow", minPosWindow);
        }

        private void TrayectoryChain_Steps()
        {
            int index = 0;
            Subchain chain = _chainController.AddChain(Chains.TrayectoryChain1);
            chain.AddParams("points", "v", "a", "posWindow");
            var trayectoryPoints = new List<Point2D>();
            int velocity = 0, aceleration = 0;
            double posWindow = 0.0;

            chain.Add(new Step("Cargar parámetros trayectoria")).Task = () =>
            {
                index = 0;
                trayectoryPoints = chain.Param<IEnumerable<Point2D>>("points").ToList();
                velocity = chain.Param<int>("v");
                aceleration = chain.Param<int>("a");
                posWindow = chain.Param<double>("posWindow");
               _chainController.NextStep();
           };

            chain.Add(new Step("Iniciar moviento gantry",10000)).Task = () =>
            {
                if (index < trayectoryPoints.Count)
                {
                    if (StartMov(trayectoryPoints[index],velocity, aceleration))
                        _chainController.NextStep();
                }
                else
                {
                    _chainController.Return();
                }
            };


            chain.Add(new Step("Finalizar movimiento pinza",20000)).Task = () =>
            {
                if (EndMov() ||
                    InPosition(trayectoryPoints[index],posWindow))
                {
                    index++;
                    _chainController.PreviousStep();
                }
            };
        }


        public IEnumerable<double> CalculateVariablePosWindows(Point2D[] points, double maxPosWindow,
                                                               double minPosWindow)
        {
            double minPosWindowFactor = minPosWindow / maxPosWindow;
            var posWindows = new List<double>();
            if (points.Length > 0) posWindows.Add(maxPosWindow);
            if (points.Length > 2)
            {
                for (int i = 1; i < points.Length - 1; i++)
                {
                    Point2D p1 = points[i].CoorRespecto(points[i - 1]);
                    Point2D p2 = points[i + 1].CoorRespecto(points[i]);
                    double variableFactor = CalcualateVariableFactor(Math.Abs(p1.AngleWith(p2)));
                    posWindows.Add(CalculateVariablePosWindow(variableFactor, maxPosWindow, minPosWindowFactor));
                }
            }

            if (points.Length > 1)
            {
                posWindows.Add(maxPosWindow);
            }

            return posWindows;
        }

        private static double CalcualateVariableFactor(double angleModule)
        {
            if (angleModule > maxAngleModule)
                return 0;
            return 1 - (angleModule / maxAngleModule);
        }

        private static double CalculateVariablePosWindow(double variableFactor, double maxPosWindow,
                                                         double minPosWinsowFactor)
        {
            return maxPosWindow * ((1 - minPosWinsowFactor) * variableFactor + minPosWinsowFactor);
        }

        private void TrayectoryWithVariablePosWindowsChain_Steps()
        {
            int index = 0;
            Subchain chain = _chainController.AddChain(Chains.TrayectoryChain2);
            chain.AddParams("points", "v", "a", "maxPosWindow", "minPosWindow");
            var trayectoryPoints = new List<Point2D>();
            int velocity = 0, aceleration = 0;
            IEnumerable<double> posWindows = null;

            chain.Add(new Step("Cargar parámetros trayectoria")).Task = () =>
            {
                index = 0;
                trayectoryPoints = chain.Param<IEnumerable<Point2D>>("points").ToList();

                velocity = chain.Param<int>("v");
                aceleration = chain.Param<int>("a");

                var maxPosWindow = chain.Param<double>("maxPosWindow");
                var minPosWindow = chain.Param<double>("minPosWindow");
                posWindows = CalculateVariablePosWindows(trayectoryPoints.ToArray<Point2D>(),maxPosWindow, minPosWindow);

                _chainController.NextStep();
           };

           chain.Add(new Step("Iniciar moviento pinza",10000)).Task = () =>
           {
               if (index < trayectoryPoints.Count)
               {
                   if (StartMov(trayectoryPoints[index],velocity, aceleration))
                       _chainController.NextStep();
                }
               else
               {
                   _chainController.Return();
               }
            };


            chain.Add(new Step("Finalizar movimiento pinza",20000)).Task = () =>
            {
                if (EndMov() ||
                    InPosition(trayectoryPoints[index],posWindows.ElementAt(index)))
                {
                    index++;
                    _chainController.
                        PreviousStep();
                }
            };
        }

        public void EnableChain()
        {
            _chainController.CallChain(Chains.EnableChain);
        }

        private void EnableChain_Steps()
        {
            Subchain chain = _chainController.AddChain(Chains.EnableChain);

            chain.Add(new Step("Habilitar " + X.Name, 2000)).Task = () =>
            {
                if (X.DriverResurrection())
                    _chainController.NextStep();
            };

            chain.Add(new Step("Habilitar " + Y.Name, 2000)).Task = () =>
            {
                if (Y.DriverResurrection())
                    _chainController.Return();
            };
        }

        #region Nested type: Chains

        private enum Chains
        {
            MoveChain,
            TrayectoryChain1,
            TrayectoryChain2,
            EnableChain
        }

        #endregion
    }
}