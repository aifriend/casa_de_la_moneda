using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Sequence;
using Idpsa.Control.Tool;

namespace Idpsa.Control.Component
{
    public class Gantry3_C3I20T11_ChainController : Gantry3_C3I20T11, IChainControllersOwner
    {
        private readonly ChainController _chainController;

        public Gantry3_C3I20T11_ChainController(CompaxC3I20T11 x, CompaxC3I20T11 y, CompaxC3I20T11 z)
            : base(x, y, z)
        {
            _chainController = new ChainController();
            MoveChain_Steps();
            TrayectoryChain_Steps();
            EnableChain_Steps();
        }

        #region IChainControllersOwner Members

        public IEnumerable<IChainController> GetChainControllers()
        {
            return new List<IChainController> {_chainController};
        }

        #endregion

        public void MoveChain(Point3D p, int v, int a)
        {
            MoveChain(p, v, a, 0.0);
        }

        public void MoveChain(Point3D p, int v, int a, double posWindow)
        {
            _chainController.CallChain(Chains.MoveChain)
                .WithParam("p", p)
                .WithParam("v", v)
                .WithParam("a", a)
                .WithParam("posWindow", posWindow);
        }

        public void MoveRelativeChain(Point3D shift, int v, int a, double posWindow)
        {
            Point3D p = Position().Desplazado(shift);
            MoveChain(p, v, a, posWindow);
        }

        public void RelativeMoveChain(Point3D shift, int v, int a)
        {
            MoveRelativeChain(shift, v, a, 0);
        }


        private void MoveChain_Steps()
        {
            Subchain chain = _chainController.AddChain(Chains.MoveChain);
            chain.AddParams("p", "v", "a", "posWindow");

            chain.Add(new Step("Iniciar moviento pinza",10000)).Task = () =>
            {
                if (StartMov(chain.Param<Point3D>("p"),chain.Param<int>("v"),chain.Param<int>("a")))
                    _chainController.NextStep();
           };


            chain.Add(new Step("Finalizar movimiento pinza", 20000)).Task = () =>
            {
                if (EndMov() || InPosition(chain.Param<Point3D>("p"),chain.Param<double>("posWindow")))
                    _chainController.Return();
            };
        }

        public void TrajectoryChain(IEnumerable<Point3D> points, int v, int a, double posWindow)
        {
            _chainController.CallChain(Chains.TrayectoryChain)
                .WithParam("points", points)
                .WithParam("v", v)
                .WithParam("a", a)
                .WithParam("posWindow", posWindow);
        }

        private void TrayectoryChain_Steps()
        {
            int index = 0;
            Subchain chain = _chainController.AddChain(Chains.TrayectoryChain);
            chain.AddParams("points", "v", "a", "posWindow");
            var trayectoryPoints = new List<Point3D>();
            int velocity = 0, aceleration = 0;
            double posWindow = 0.0;

            chain.Add(new Step("Cargar parámetros trayectoria")).Task = () =>
            {
                index = 0;
                trayectoryPoints = chain.Param<IEnumerable<Point3D>>("points").ToList();
                velocity = chain.Param<int>("v");
                aceleration = chain.Param<int>("a");
                posWindow = chain.Param<double>("posWindow");
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
                if (EndMov() ||InPosition(trayectoryPoints[index],posWindow))
                {
                    index++;
                    _chainController.PreviousStep();
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

            chain.Add(new Step("Habilitar " + X.Name, 5000)).Task = () =>
            {
                if (X.DriverResurrection())
                    _chainController.NextStep();
            };

            chain.Add(new Step("Habilitar " + Y.Name, 5000)).Task = () =>
            {
                if (Y.DriverResurrection())
                    _chainController.NextStep();
            };

            chain.Add(new Step("Habilitar " + Z.Name, 5000)).Task = () =>
            {
                if (Z.DriverResurrection())
                    _chainController.Return();
            };
        }

        #region Nested type: Chains

        private enum Chains
        {
            MoveChain,
            TrayectoryChain,
            EnableChain
        }

        #endregion
    }
}