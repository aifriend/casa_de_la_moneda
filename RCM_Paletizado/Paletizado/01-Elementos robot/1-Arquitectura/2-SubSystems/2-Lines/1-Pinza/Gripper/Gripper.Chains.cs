using System;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control;
using Idpsa.Control.Sequence;
using Idpsa.Control.Tool;

namespace Idpsa.Paletizado
{
    public partial class Gripper
    {
        #region Arms enum

        public enum Arms
        {
            Any,
            Open,
            Close
        }

        #endregion

        #region Extensor enum

        public enum Extensor
        {
            Any,
            Extend,
            Retract,
            Dead
        }

        #endregion

        #region Sucker enum

        public enum Sucker
        {
            Any,
            On,
            Off
        }

        #endregion

        public void SetChain(Spin spin, Extensor extensor, Arms arms, Sucker sucker)
        {
            GirarChain(spin);
            Extender(extensor);
            Abrir(arms);
            Vacio(sucker);
        }

        public void CheckChain(Spin spin, Extensor extensor, Arms arms, Sucker sucker)
        {
            _chainController.CallChain(ActionChains.CheckElements)
                .WithParam("spin", spin)
                .WithParam("extendida", extensor)
                .WithParam("abierta", arms)
                .WithParam("enVacio", sucker);
        }

        private void InitializeControllerChains()
        {
            Check_Steps();
            SetCheck_Steps();
            SequentialSetCheck_Steps();
            SecurityMove_Steps();
            SecuritySeveralPointsMove_Steps();
            MoveToCeiling_Steps();
        }

        private void Check_Steps()
        {
            Subchain chain = _chainController.AddChain(ActionChains.CheckElements);
            chain.AddParams("spin", "extendida", "abierta", "enVacio");
            TON timer = null;


            chain.Add(new Step(() => "Comprobar pinza en " + chain.TryParam<Spin>("spin").ToStringImage(), 5000)).Task =
                () =>
                    {
                        if (EnGiro(chain.Param<Spin>("spin")))
                        {
                            timer = new TON();
                            _chainController.NextStep();
                        }
                    };

            chain.Add(new Step(
                          () => "Comprobar extensor pinza " + chain.TryParam<Extensor>("extendida").ToStringImage(),
                          5000)).Task = () =>
                                            {
                                                var extensor = chain.Param<Extensor>("extendida");
                                                if (extensor == Extensor.Dead)
                                                {
                                                    if (timer.Timing(1500))
                                                    {
                                                        _chainController.NextStep();
                                                    }
                                                }
                                                else
                                                {
                                                    if (IsExtended(extensor))
                                                        _chainController.NextStep();
                                                }
                                            };

            chain.Add(new Step(() => "Comprobar brazos pinza " + chain.TryParam<Arms>("abierta").ToStringImage(), 5000))
                .Task = () =>
                            {
                                if (Open(chain.Param<Arms>("abierta")))
                                    _chainController.NextStep();
                            };

            chain.Add(
                new Step(() => "Comprobar plano aspirante pinza " + chain.TryParam<Sucker>("enVacio").ToStringImage(),
                         5000)).Task = () =>
                                           {
                                               if (EnVacio(chain.Param<Sucker>("enVacio")))
                                                   _chainController.Return();
                                           };
        }

        public void SequentialSetCheckChain(Spin spin, Extensor extensor, Arms arms, Sucker sucker)
        {
            _chainController.CallChain(ActionChains.SequentialSetCheckElemets)
                .WithParam("spin", spin)
                .WithParam("extendida", extensor)
                .WithParam("abierta", arms)
                .WithParam("enVacio", sucker);
        }

        private void SequentialSetCheck_Steps()
        {
            Subchain chain = _chainController.AddChain(ActionChains.SequentialSetCheckElemets);
            chain.AddParams("spin", "extendida", "abierta", "enVacio");
            TON timer = null;

            chain.Add(new Step("Girar pinza")).Task = () => { GirarChain(chain.Param<Spin>("spin")); };

            chain.Add(new Step(() => "Comprobar pinza en " + chain.TryParam<Spin>("spin").ToStringImage(), 5000)).Task =
                () =>
                    {
                        if (EnGiro(chain.Param<Spin>("spin")))
                        {
                            timer = new TON();
                            _chainController.NextStep();
                        }
                    };

            chain.Add(new Step(
                          () => "Comprobar extensor pinza " + chain.TryParam<Extensor>("extendida").ToStringImage(),
                          5000)).Task = () =>
                                            {
                                                var extensor = chain.Param<Extensor>("extendida");
                                                Extender(extensor);
                                                if (extensor == Extensor.Dead)
                                                {
                                                    if (timer.Timing(1500))
                                                    {
                                                        _chainController.NextStep();
                                                    }
                                                }
                                                else
                                                {
                                                    if (IsExtended(extensor))
                                                        _chainController.NextStep();
                                                }
                                            };

            chain.Add(new Step(() => "Comprobar brazos pinza " + chain.TryParam<Arms>("abierta").ToStringImage(), 5000))
                .Task = () =>
                            {
                                Abrir(chain.Param<Arms>("abierta"));
                                if (Open(chain.Param<Arms>("abierta")))
                                    _chainController.NextStep();
                            };

            chain.Add(
                new Step(() => "Comprobar plano aspirante pinza " + chain.TryParam<Sucker>("enVacio").ToStringImage(),
                         5000)).Task = () =>
                                           {
                                               Vacio(chain.Param<Sucker>("enVacio"));
                                               if (EnVacio(chain.Param<Sucker>("enVacio")))
                                                   _chainController.Return();
                                           };
        }


        public void SetCheckChain(Spin spin, Extensor extensor, Arms arms, Sucker sucker)
        {
            _chainController.CallChain(ActionChains.SetCheckElements)
                .WithParam("spin", spin)
                .WithParam("extendida", extensor)
                .WithParam("abierta", arms)
                .WithParam("enVacio", sucker);
        }

        private void SetCheck_Steps()
        {
            Subchain chain = _chainController.AddChain(ActionChains.SetCheckElements);
            chain.AddParams("spin", "extendida", "abierta", "enVacio");

            Spin spin = default(Spin);
            Extensor extensor = default(Extensor);
            Arms arms = default(Arms);
            Sucker sucker = default(Sucker);

            chain.Add(new Step("Paso inicial")).Task = () =>
                                                           {
                                                               spin = chain.Param<Spin>("spin");
                                                               extensor = chain.Param<Extensor>("extendida");
                                                               arms = chain.Param<Arms>("abierta");
                                                               sucker = chain.Param<Sucker>("enVacio");
                                                               _chainController.NextStep();
                                                           };

            chain.Add(new Step("Preparar pinza")).Task = () => { SetChain(spin, extensor, arms, sucker); };

            chain.Add(new Step("Comprobar pinza preparada")).Task =
                () => { CheckChain(spin, extensor, arms, Sucker.Any); //Cambiar
                };

            chain.Add(new Step("Paso final")).Task = () => { _chainController.Return(); };
        }

        public void MoveToCeilingChain(double posWindow)
        {
            MoveToCeilingChain(_chainController.NextStep, posWindow);
        }

        public void MoveToCeilingChain(Action action, double posWindow)
        {
            _chainController.CallChain(ActionChains.MoveToCeiling)
                .WithParam("action", action)
                .WithParam("posWindow", posWindow);
        }

        private void MoveToCeiling_Steps()
        {
            Subchain chain = _chainController.AddChain(ActionChains.MoveToCeiling);
            chain.AddParams("action", "posWindow");
            double posWindow = 0;
            DynamicStepBody dynamicAction = null;

            chain.Add(new Step("Paso inicial")).Task = () =>
                                                           {
                                                               posWindow = chain.Param<double>("posWindow");

                                                               dynamicAction = new DynamicStepBody(
                                                                   () => "Preparar pinza para inicio movimiento techo",
                                                                   chain.Param<Action>("action")
                                                                   );

                                                               _chainController.NextStep();
                                                           };

            chain.Add(new Step()).SetDynamicBehaviour(() => dynamicAction);

            chain.Add(new Step("Inicio movimiento eje z arriba", 20000)).Task = () =>
                                                                                    {
                                                                                        //if (_gantry.Z.StartMov(NegativeZLimit, V.Low, A.Low))//V.Middle??
                                                                                        if (
                                                                                            _gantry.Z.StartMov(
                                                                                                NegativeZLimit, V.Middle,
                                                                                                A.Low))
                                                                                            //MDG.2011-05-31.Movemos hacia arriba más rapido
                                                                                            _chainController.NextStep();
                                                                                    };

            chain.Add(new Step("Final movimiento eje z arriba", 20000)).Task = () =>
                                                                                   {
                                                                                       if (_gantry.Z.EndMov() ||
                                                                                           _gantry.Z.InPosition(
                                                                                               NegativeZLimit, posWindow))
                                                                                           _chainController.Return();
                                                                                   };
        }

        public void SecurityMoveChain(Point3D pf, Action action1, double posWindow1, Action action2, double posWindow2)
        {
            SecurityMoveChain(pf, action1, posWindow1, action2, posWindow2, () => { });
        }


        public void SecurityMoveChain(Point3D pf, Action action1, double posWindow1, Action action2, double posWindow2,
                                      Action notification)
        {
            _chainController.CallChain(ActionChains.SecurityMove)
                .WithParam("pf", pf)
                .WithParam("action1", action1)
                .WithParam("action2", action2)
                .WithParam("posWindow1", posWindow1)
                .WithParam("posWindow2", posWindow2)
                .WithParam("notification", notification);
        }

        public void SecurityMoveChain(IEnumerable<Point3D> points, Action action1, double posWindow1, Action action2,
                                      double posWindow2)
        {
            SecurityMoveChain(points, action1, posWindow1, action2, posWindow2, () => { });
        }

        public void SecurityMoveChain(IEnumerable<Point3D> points, Action action1, double posWindow1, Action action2,
                                      double posWindow2, Action notification)
        {
            if (points == null)
                throw new ArgumentNullException("points");
            if (points.Count() == 1)
            {
                SecurityMoveChain(points.ElementAt(0), action1, posWindow1, action2, posWindow2, notification);
            }
            else
            {
                _chainController.CallChain(ActionChains.SecuritySeveralPointsMove)
                    .WithParam("points", points)
                    .WithParam("action1", action1)
                    .WithParam("action2", action2)
                    .WithParam("posWindow1", posWindow1)
                    .WithParam("posWindow2", posWindow2)
                    .WithParam("notification", notification);
            }
        }

        private void SecuritySeveralPointsMove_Steps()
        {
            Subchain chain = _chainController.AddChain(ActionChains.SecuritySeveralPointsMove);
            chain.AddParams("points", "action1", "action2", "posWindow1", "posWindow2", "notification");


            IEnumerable<Point3D> points = null;
            IEnumerable<Point3D> pointsHight = null;


            double posWindow1 = 0, posWindow2 = 0, posWindow3 = 0;
            DynamicStepBody dynamicAction1 = null;
            DynamicStepBody dynamicAction2 = null;
            Action notification = null;


            chain.Add(new Step("Paso inicial")).Task = () =>
                                                           {
                                                               points = chain.Param<IEnumerable<Point3D>>("points");
                                                               pointsHight =
                                                                   points.Select(
                                                                       (p) => new Point3D(p) {Z = NegativeZLimit});

                                                               posWindow1 = chain.Param<double>("posWindow1");
                                                               posWindow2 = chain.Param<double>("posWindow2");

                                                               dynamicAction1 = new DynamicStepBody(
                                                                   () => "Preparar pinza para inicio movimiento seguro",
                                                                   chain.Param<Action>("action1")
                                                                   );
                                                               dynamicAction2 = new DynamicStepBody(
                                                                   () =>
                                                                   "Preparar pinza para finalización movimiento seguro",
                                                                   chain.Param<Action>("action2")
                                                                   );

                                                               notification = chain.Param<Action>("notification");

                                                               _chainController.NextStep();
                                                           };

            chain.Add(new Step()).SetDynamicBehaviour(() => dynamicAction1);

            chain.Add(new Step("Inicio movimiento eje z arriba", 20000)).Task = () =>
                                                                                    {
                                                                                        //if(_gantry.Z.StartMov(NegativeZLimit,V.Low,A.Low))
                                                                                        if (
                                                                                            _gantry.Z.StartMov(
                                                                                                NegativeZLimit, V.Middle,
                                                                                                A.Low))
                                                                                            //MDG.2011-06-13.Hacia arriba más rápido
                                                                                            _chainController.NextStep();
                                                                                    };

            chain.Add(new Step("Final movimiento eje z arriba", 20000)).Task = () =>
                                                                                   {
                                                                                       if (_gantry.Z.EndMov() ||
                                                                                           _gantry.Z.InPosition(
                                                                                               NegativeZLimit,
                                                                                               posWindow1))
                                                                                       {
                                                                                           notification();
                                                                                           _chainController.NextStep();
                                                                                       }
                                                                                   };

            chain.Add(new Step("Iniciar movimiento pinza horizontal", 5000)).Task =
                () => { _gantry.TrajectoryChain(pointsHight, V.Middle, A.Middle, 50); };

            chain.Add(new Step("Finalizar movimiento pinza horizontal", 20000)).Task = () =>
                                                                                           {
                                                                                               if (_gantry.EndMov())
                                                                                                   _chainController.
                                                                                                       NextStep();
                                                                                           };

            chain.Add(new Step()).SetDynamicBehaviour(() => dynamicAction2);

            chain.Add(new Step("Inicio movimiento eje z abajo", 20000)).Task = () =>
                                                                                   {
                                                                                       //if (_gantry.Z.StartMov((int)points.Last().Z, V.Low, A.Low))
                                                                                       if (
                                                                                           _gantry.Z.StartMov(
                                                                                               (int) points.Last().Z,
                                                                                               150,
                                                                                               A.Low))
                                                                                           //MDG.2011-06-13.Mov hacia abajo a 200
                                                                                           _chainController.NextStep();
                                                                                   };

            chain.Add(new Step("Final movimiento eje z abajo", 20000)).Task = () =>
                                                                                  {
                                                                                      if (_gantry.Z.EndMov() ||
                                                                                          _gantry.Z.InPosition(
                                                                                              (int) points.Last().Z,
                                                                                              posWindow2))
                                                                                          _chainController.Return();
                                                                                  };
        }

        //Mueve arriba. Despues horizontal. Despues abajo
        private void SecurityMove_Steps()
        {
            Subchain chain = _chainController.AddChain(ActionChains.SecurityMove);
            chain.AddParams("pf", "action1", "action2", "posWindow1", "posWindow2", "notification");

            Point3D ph = null;
            Point3D pf = null;
            double posWindow1 = 0, posWindow2 = 0, posWindow3 = 0;
            DynamicStepBody dynamicAction1 = null;
            DynamicStepBody dynamicAction2 = null;
            Action notification = null;


            chain.Add(new Step("Paso inicial")).Task = () =>
                                                           {
                                                               pf = chain.Param<Point3D>("pf");
                                                               ph = new Point3D(pf.X, pf.Y, NegativeZLimit);

                                                               posWindow1 = chain.Param<double>("posWindow1");
                                                               posWindow2 = chain.Param<double>("posWindow2");

                                                               dynamicAction1 = new DynamicStepBody(
                                                                   () => "Preparar pinza para inicio movimiento seguro",
                                                                   chain.Param<Action>("action1")
                                                                   );
                                                               dynamicAction2 = new DynamicStepBody(
                                                                   () =>
                                                                   "Preparar pinza para finalización movimiento seguro",
                                                                   chain.Param<Action>("action2")
                                                                   );

                                                               notification = chain.Param<Action>("notification");

                                                               _chainController.NextStep();
                                                           };

            chain.Add(new Step()).SetDynamicBehaviour(() => dynamicAction1);

            chain.Add(new Step("Inicio movimiento eje z arriba", 20000)).Task = () =>
                                                                                    {
                                                                                        //if(_gantry.Z.StartMov((int)ph.Z,V.Low,A.Low))
                                                                                        if (_gantry.Z.StartMov(
                                                                                            (int) ph.Z, V.Middle, A.Low))
                                                                                            //MDG.2011-06-13.Movimiento vertical arriba más rapido
                                                                                            _chainController.NextStep();
                                                                                    };

            //chain.Add(new Step("Inicio movimiento eje z arriba", 20000)).Task = () =>
            //{
            //    //estado de espera
            //    _chainController.NextStep();
            //};

            chain.Add(new Step("Final movimiento eje z arriba", 20000)).Task = () =>
                                                                                   {
                                                                                       if (_gantry.Z.EndMov() ||
                                                                                           _gantry.Z.InPosition(
                                                                                               (int) ph.Z, posWindow1))
                                                                                       {
                                                                                           notification();
                                                                                           _chainController.NextStep();
                                                                                       }
                                                                                   };

            chain.Add(new Step("Iniciar movimiento pinza horizontal", 5000)).Task = () =>
                                                                                        {
                                                                                            if (_gantry.StartMov(ph,
                                                                                                                 V.Hight,
                                                                                                                 A.Hight))
                                                                                                _chainController.
                                                                                                    NextStep();
                                                                                        };

            //chain.Add(new Step("Iniciar movimiento pinza horizontal", 20000)).Task = () =>
            //{
            //    //estado de espera
            //    _chainController.NextStep();
            //};     

            chain.Add(new Step("Finalizar movimiento pinza horizontal", 20000)).Task = () =>
                                                                                           {
                                                                                               if (_gantry.EndMov())
                                                                                                   _chainController.
                                                                                                       NextStep();
                                                                                           };

            chain.Add(new Step()).SetDynamicBehaviour(() => dynamicAction2);

            chain.Add(new Step("Inicio movimiento eje z abajo", 20000)).Task = () =>
                                                                                   {
                                                                                       //estado de espera
                                                                                       _chainController.NextStep();
                                                                                   };

            chain.Add(new Step("Inicio movimiento eje z abajo", 20000)).Task = () =>
                                                                                   {
                                                                                       //if (_gantry.Z.StartMov((int)pf.Z, V.Low, A.Low))
                                                                                       if (_gantry.Z.StartMov(
                                                                                           (int) pf.Z, 150, A.Low))
                                                                                           //MDG.2011-06-14.Movimiento vertical hacia abajo el 1,5 veces mas rapido
                                                                                           _chainController.NextStep();
                                                                                   };

            //chain.Add(new Step("Inicio movimiento eje z abajo", 20000)).Task = () =>
            //{
            //    //estado de espera
            //    _chainController.NextStep();
            //};

            chain.Add(new Step("Final movimiento eje z abajo", 20000)).Task = () =>
                                                                                  {
                                                                                      if (_gantry.Z.EndMov() ||
                                                                                          _gantry.Z.InPosition(
                                                                                              (int) pf.Z, posWindow2))
                                                                                          _chainController.Return();
                                                                                      //if (_gantry.Z.EndMov() || _gantry.Z.InPosition((int)pf.Z, posWindow2))
                                                                                      //{
                                                                                      //    if (_gantry.Z.InPosition((int)pf.Z, posWindow2))
                                                                                      //        _chainController.Return();
                                                                                      //    else
                                                                                      //        _chainController.PreviousStep();
                                                                                      //        //_gantry.Z.StartMov((int)pf.Z, 150, A.Low);//MDG.2011-06-14
                                                                                      //}
                                                                                  };
        }

        #region Nested type: ActionChains

        private enum ActionChains
        {
            SetCheckElements,
            SequentialSetCheckElemets,
            CheckElements,
            SecurityMove,
            SecuritySeveralPointsMove,
            MoveToCeiling
        }

        #endregion
    }
}