using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Sequence;
using Idpsa.Control.Tool;
using Idpsa.Paletizado.Definitions;

namespace Idpsa.Paletizado
{
    public partial class Gripper
    {
        #region Nested type: CadAutoGripper

        private class CadAutoGripper : StructuredChain
        {
            private readonly IdpsaSystemPaletizado _sys;
            private readonly TON _timer = new TON();
            private readonly TON _timer2 = new TON();
            private readonly Gripper gripper;
            private Gradient A;
            private Gradient V;

            public CadAutoGripper(string name, IdpsaSystemPaletizado sys)
                : base(name)
            {
                _sys = sys;
                gripper = sys.Gripper;
                Context = GripperContext.GetInstance();
                AddChainController(Context);
                AddSteps();
            }

            private GripperContext Context { get; set; }

            protected override void AddSteps()
            {
                Main_Steps();
                Wait_Steps();
                Transfer_Steps();

                CatchPalet_Steps();
                CatchPaletInStore_Steps();
                LeavePalet_Steps();

                CatchItem_Steps();
                CheckItemOK_Steps();
                CheckItemOkDespaletizing_Steps();//MDG.2013-04-25
                LeaveItem_Steps();

                CatchSeparator_Steps();
                LeaveSeparator_Steps();

                GoToOrigin_Steps(); //MDG.2011-06-14
            }

            private void Main_Steps()
            {
                MainChain.Add(new Step("Paso inicial")).Task = () => { NextStep(); };

                MainChain.Add(new Step("Habilitar ejes")).Task = () => { gripper.EnableChain(); };

                MainChain.Add(new Step("Selección subrama ejecutar") {StopChain = true}.WithTag("loop")).Task =
                    () => { Context.CallChain(); };

                MainChain.Add(new Step("Paso final")).Task = () => { GoToStep("loop"); };
            }

            private void Wait_Steps()
            {
                Subchain chain = AddSubchain(Chains.Espera);

                chain.Add(new Step("Pinza en espera") {StopChain = true}).Task = () =>
                                                                                     {
                                                                                         if (Context.NextState())
                                                                                             Return();
                                                                                     };
            }

            private void Transfer_Steps()
            {
                Subchain chain = AddSubchain(Chains.Transfer);

                chain.Add(new Step("Preparar pinza")).Task = () =>
                                                                 {
                                                                     gripper.MoveToCeilingChain
                                                                         (
                                                                             () =>
                                                                             gripper.SetCheckChain(Spin.Any,
                                                                                                   Extensor.Retract,
                                                                                                   Arms.Any, Sucker.Off),
                                                                             300
                                                                         );
                                                                 };

                chain.Add(new Step("Paso final")).Task = () =>
                                                             {
                                                                 Context.ElementLeft();
                                                                 Context.NextState();
                                                                 NextStep();
                                                             };

                chain.Add(new Step("Paso final") {StopChain = true}).Task = () => { Return(); };
            }

            private void CatchPalet_Steps()
            {
                Subchain chain = AddSubchain(Chains.CatchPalet);
                chain.AddParams(CatchPaletParams.position);
                chain.AddParams(CatchPaletParams.positionNextPalet1); //MDG.2011-06-22
                chain.AddParams(CatchPaletParams.positionNextPalet2); //MDG.2011-06-22
                chain.AddParams(CatchPaletParams.positionNextPalet3); //MDG.2011-06-22
                chain.AddParams(CatchPaletParams.positionNextPalet4); //MDG.2011-06-28
                chain.AddParams(CatchPaletParams.positionNextPalet5); //MDG.2011-06-28
                chain.AddParams(CatchPaletParams.spin); //MDG.2011-03.14.Admitimos giro pinza la coger palet Vikex

                chain.Add(new Step("Mover pinza a almacen de palet")).Task = () =>
                                                                                 {
                                                                                     var position =
                                                                                         chain.Param<Point3D>(
                                                                                             CatchPaletParams.position);
                                                                                     var positionNextPalet1 =
                                                                                         chain.Param<Point3D>(
                                                                                             CatchPaletParams.
                                                                                                 positionNextPalet1);
                                                                                     //MDG.2011-06-22
                                                                                     var positionNextPalet2 =
                                                                                         chain.Param<Point3D>(
                                                                                             CatchPaletParams.
                                                                                                 positionNextPalet2);
                                                                                     //MDG.2011-06-22
                                                                                     var positionNextPalet3 =
                                                                                         chain.Param<Point3D>(
                                                                                             CatchPaletParams.
                                                                                                 positionNextPalet3);
                                                                                     //MDG.2011-06-22
                                                                                     var positionNextPalet4 =
                                                                                         chain.Param<Point3D>(
                                                                                             CatchPaletParams.
                                                                                                 positionNextPalet4);
                                                                                     //MDG.2011-06-22
                                                                                     var positionNextPalet5 =
                                                                                         chain.Param<Point3D>(
                                                                                             CatchPaletParams.
                                                                                                 positionNextPalet5);
                                                                                     //MDG.2011-06-22

                                                                                     var spin =
                                                                                         Subchain(Chains.CatchPalet).
                                                                                             Param<Spin>(
                                                                                                 CatchPaletParams.spin);
                                                                                     //MDG.2011-03.14.Admitimos giro pinza para coger palet Vikex

                                                                                     gripper.SecurityMoveChain
                                                                                         (
                                                                                             position,
                                                                                             () =>
                                                                                             gripper.SetCheckChain(spin,
                                                                                                                   Extensor
                                                                                                                       .
                                                                                                                       Retract,
                                                                                                                   Arms.
                                                                                                                       Any,
                                                                                                                   Sucker
                                                                                                                       .
                                                                                                                       Off),
                                                                                             100,
                                                                                             () =>
                                                                                             gripper.SetCheckChain(spin,
                                                                                                                   Extensor
                                                                                                                       .
                                                                                                                       Retract,
                                                                                                                   Arms.
                                                                                                                       Open,
                                                                                                                   Sucker
                                                                                                                       .
                                                                                                                       Off),
                                                                                             1 //MDG.2011-06-15//0
                                                                                         );
                                                                                 };

                chain.Add(new Step("Cerrar brazos pinza")).Task = () =>
                                                                      {
                                                                          gripper.Cerrar();
                                                                          NextStep();
                                                                      };

                chain.Add(new Step("Comprobar brazos pinza cerrados", 5000)).Task = () =>
                                                                                        {
                                                                                            if (_timer.Timing(3000))
                                                                                                //gripper.Cerrada)
                                                                                            {
                                                                                                Context.NextState();
                                                                                                Return();
                                                                                            }
                                                                                        };
            }

            private void CatchPaletInStore_Steps()
            {
                Subchain chain = AddSubchain(Chains.CatchPaletInStore);
                chain.AddParams(CatchPaletParams.position);
                chain.AddParams(CatchPaletParams.positionNextPalet1); //MDG.2011-06-22
                chain.AddParams(CatchPaletParams.positionNextPalet2); //MDG.2011-06-22
                chain.AddParams(CatchPaletParams.positionNextPalet3); //MDG.2011-06-22
                chain.AddParams(CatchPaletParams.positionNextPalet4); //MDG.2011-06-27
                chain.AddParams(CatchPaletParams.positionNextPalet5); //MDG.2011-06-27
                chain.AddParams(CatchPaletParams.spin); //MDG.2011-03.14.Admitimos giro pinza la coger palet Vikex
                int finalPositionZ = 0;
                int positionNextPalet1Z = 0; //MDG.2011-06-22
                int positionNextPalet2Z = 0; //MDG.2011-06-22
                int positionNextPalet3Z = 0; //MDG.2011-06-22
                int positionNextPalet4Z = 0; //MDG.2011-06-27
                int positionNextPalet5Z = 0; //MDG.2011-06-27
                int CountPaletTries = 0;
                //MDG.2011-06-22.Contamos las veces que hemos intentado bajar a por el siguiente palet

                chain.Add(new Step("Mover pinza a almacen de palet")).Task = () =>
                                                                                 {
                                                                                     var maxPosition =
                                                                                         chain.Param<Point3D>(
                                                                                             CatchPaletParams.position);
                                                                                     var maxPositionNextPalet1 =
                                                                                         chain.Param<Point3D>(
                                                                                             CatchPaletParams.
                                                                                                 positionNextPalet1);
                                                                                     //MDG.2011-06-22
                                                                                     var maxPositionNextPalet2 =
                                                                                         chain.Param<Point3D>(
                                                                                             CatchPaletParams.
                                                                                                 positionNextPalet2);
                                                                                     //MDG.2011-06-22
                                                                                     var maxPositionNextPalet3 =
                                                                                         chain.Param<Point3D>(
                                                                                             CatchPaletParams.
                                                                                                 positionNextPalet3);
                                                                                     //MDG.2011-06-22
                                                                                     var maxPositionNextPalet4 =
                                                                                         chain.Param<Point3D>(
                                                                                             CatchPaletParams.
                                                                                                 positionNextPalet4);
                                                                                     //MDG.2011-06-27
                                                                                     var maxPositionNextPalet5 =
                                                                                         chain.Param<Point3D>(
                                                                                             CatchPaletParams.
                                                                                                 positionNextPalet5);
                                                                                     //MDG.2011-06-27
                                                                                     var spin =
                                                                                         Subchain(
                                                                                             Chains.CatchPaletInStore).
                                                                                             Param<Spin>(
                                                                                                 CatchPaletParams.spin);
                                                                                     //MDG.2011-03.14.Admitimos giro pinza la coger palet Vikex
                                                                                     positionNextPalet1Z =
                                                                                         (int) maxPositionNextPalet1.Z;
                                                                                     //MDG.2011-06-22
                                                                                     positionNextPalet2Z =
                                                                                         (int) maxPositionNextPalet2.Z;
                                                                                     //MDG.2011-06-22
                                                                                     positionNextPalet3Z =
                                                                                         (int) maxPositionNextPalet3.Z;
                                                                                     //MDG.2011-06-22
                                                                                     positionNextPalet4Z =
                                                                                         (int) maxPositionNextPalet4.Z;
                                                                                     //MDG.2011-06-27
                                                                                     positionNextPalet5Z =
                                                                                         (int) maxPositionNextPalet5.Z;
                                                                                     //MDG.2011-06-27
                                                                                     CountPaletTries = 0;

                                                                                     gripper.SecurityMoveChain
                                                                                         (
                                                                                             maxPosition,
                                                                                             //() => gripper.SetCheckChain(Spin.S90, Extensor.Retract, Arms.Any, Sucker.Off),
                                                                                             () =>
                                                                                             gripper.SetCheckChain(
                                                                                                                    (spin==Spin.S0)?Spin.S270:spin,//MDG.2013-05-09//spin
                                                                                                                   Extensor
                                                                                                                       .
                                                                                                                       Retract,
                                                                                                                   Arms.
                                                                                                                       Any,
                                                                                                                   Sucker
                                                                                                                       .
                                                                                                                       Off),
                                                                                             //MDG.2011-03.14.Admitimos giro pinza para coger palet Vikex
                                                                                             100,
                                                                                             () =>
                                                                                             gripper.SetCheckChain(spin,
                                                                                                                   Extensor
                                                                                                                       .
                                                                                                                       Dead,
                                                                                                                   Arms.
                                                                                                                       Open,
                                                                                                                   Sucker
                                                                                                                       .
                                                                                                                       Off),
                                                                                             //MDG.2011-03.14.Admitimos giro pinza para coger palet Vikex
                                                                                             150
                                                                                         );
                                                                                 };

                chain.Add(new Step("Comprobar extensor pinza extendido", 1000)).Task = () =>
                                                                                           {
                                                                                               if (gripper.Extendida)
                                                                                               {
                                                                                                   CountPaletTries = 0;
                                                                                                   NextStep();
                                                                                               }
                                                                                           };

                ////////////
                //chain.Add(new Step("Comprobar extensor pinza no extendido", 2000)).Task = () =>
                chain.Add(new Step("Comprobar pinza no extendida", 6000).WithTag("Comprobar pinza no extendida")).Task =
                    () => //MDG.2011-06-22
                        {
                            if (!gripper.Extendida)
                            {
                                if (gripper.Position().Z >= Geometria.ZSuelo)
                                {
                                    finalPositionZ = (int) gripper.Position().Z; //MDG.2011-06-22
                                }
                                else
                                {
                                    finalPositionZ = (int) gripper.Position().Z + 10; // 30;
                                }
                                //NextStep();
                                GoToStep("Bajar a coger palet definitivo");
                            }
                            else if (gripper.Position().Z >= Geometria.ZSuelo)
                            {
                                finalPositionZ = (int) gripper.Position().Z;
                                //NextStep();
                                GoToStep("Bajar a coger palet definitivo");
                            }
                            else if (_timer.Timing(5000)) //MDG.2011-06-22
                            {
                                //No ha detectado palet. Bajamos a siguiente posicion
                                _timer.Reset();
                                //GoToStep("Bajar a siguiente Palet pila");
                                NextStep();
                            }
                        };

                /////////Nuevos pasos para ir a por el siguiente palet 1. MDG.2011-06-22
                chain.Add(new Step("Bajar a siguiente Palet pila", 1000).WithTag("Bajar a siguiente Palet pila")).Task =
                    () =>
                        {
                            if (CountPaletTries < 1)
                            {
                                if (positionNextPalet1Z > 22)
                                {
                                    if (gripper.Z.StartMov(positionNextPalet1Z, gripper.V.Low, gripper.A.Low))
                                    {
                                        CountPaletTries = 1;
                                        NextStep();
                                    }
                                }
                            }
                            else if (CountPaletTries < 2)
                            {
                                if (positionNextPalet2Z > 22)
                                {
                                    if (gripper.Z.StartMov(positionNextPalet2Z, gripper.V.Low, gripper.A.Low))
                                    {
                                        CountPaletTries = 2;
                                        NextStep();
                                    }
                                }
                            }
                            else if (CountPaletTries < 3)
                            {
                                if (positionNextPalet3Z > 22)
                                {
                                    if (gripper.Z.StartMov(positionNextPalet3Z, gripper.V.Low, gripper.A.Low))
                                    {
                                        CountPaletTries = 3;
                                        NextStep();
                                    }
                                }
                            }
                            else if (CountPaletTries < 4)
                            {
                                if (positionNextPalet4Z > 22)
                                {
                                    if (gripper.Z.StartMov(positionNextPalet4Z, gripper.V.Low, gripper.A.Low))
                                    {
                                        CountPaletTries = 4;
                                        NextStep();
                                    }
                                }
                            }
                            else if (CountPaletTries <= 5)
                            {
                                if (positionNextPalet5Z > 22)
                                {
                                    if (gripper.Z.StartMov(positionNextPalet5Z, gripper.V.Low, gripper.A.Low))
                                    {
                                        CountPaletTries = 5;
                                        NextStep();
                                    }
                                }
                            }
                        };

                chain.Add(new Step("Finalizar movimiento eje Z abajo siguiente palet pila", 10000)).Task = () =>
                                                                                                               {
                                                                                                                   if (
                                                                                                                       gripper
                                                                                                                           .
                                                                                                                           Z
                                                                                                                           .
                                                                                                                           EndMov
                                                                                                                           ())
                                                                                                                   {
                                                                                                                       GoToStep
                                                                                                                           ("Comprobar pinza no extendida");
                                                                                                                   }
                                                                                                               };

                /////////////

                chain.Add(new Step("Bajar a coger palet definitivo", 1000).WithTag("Bajar a coger palet definitivo")).
                    Task = () =>
                               {
                                   if (gripper.Z.StartMov(finalPositionZ, gripper.V.Low, gripper.A.Low))
                                   {
                                       NextStep();
                                   }
                               };

                chain.Add(new Step("Finalizar movimiento eje Z abajo", 10000)).Task = () =>
                                                                                          {
                                                                                              if (gripper.Z.EndMov())
                                                                                              {
                                                                                                  gripper.Recoger();
                                                                                                  gripper.Cerrar();
                                                                                                  NextStep();
                                                                                              }
                                                                                          };

                chain.Add(new Step("Comprobar brazos pinza cerrados", 5000)).Task = () =>
                                                                                        {
                                                                                            if (_timer2.Timing(3000))
                                                                                                //gripper.Cerrada)
                                                                                            {
                                                                                                Context.NextState();
                                                                                                Return();
                                                                                            }
                                                                                        };
            }

            private void LeavePalet_Steps()
            {
                Subchain chain = AddSubchain(Chains.LeavePalet);
                chain.AddParams(LeftPaletParams.position,
                                LeftPaletParams.securityPosition1,
                                LeftPaletParams.securityPosition2,
                                LeftPaletParams.spin);

                chain.Add(new Step("Preparar estado pinza")).Task = () =>
                                                                        {
                                                                            var spin =
                                                                                Subchain(Chains.LeavePalet).Param<Spin>(
                                                                                    LeftPaletParams.spin);
                                                                            //MDG.2011-03.14.Admitimos giro pinza la coger palet Vikex

                                                                            //gripper.SetCheckChain(Spin.S90, Extensor.Retract, Arms.Any, Sucker.Off);
                                                                            gripper.SetCheckChain(spin, Extensor.Retract,
                                                                                                  Arms.Any, Sucker.Off);
                                                                            //MDG.2011-03.14.Admitimos giro pinza la coger palet Vikex
                                                                        };

                chain.Add(new Step("Subir pinza 50 mm para evitar mover los otros palets")).Task =
                    //() => { gripper.MoveRelativeChain(new Point3D(0, 0, -20), gripper.V.Middle, gripper.A.Middle, 5); };
                    () => { gripper.MoveRelativeChain(new Point3D(0, 0, -50), gripper.V.Middle, gripper.A.Middle, 5); };//MDG.2013-05-09. Subimos 30 mm más alto

                chain.Add(new Step("Mover pinza lateralmente para evitar interferencias al subir")).Task = () =>
                                                                                                               {
                                                                                                                   //gripper.MoveRelativeChain(new Point3D(20, -20, -10), gripper.V.Hight, gripper.A.Hight, 5);
                                                                                                                   gripper
                                                                                                                       .
                                                                                                                       MoveRelativeChain
                                                                                                                       (new Point3D
                                                                                                                            (20,
                                                                                                                             -20,
                                                                                                                             -10),
                                                                                                                        gripper
                                                                                                                            .
                                                                                                                            V
                                                                                                                            .
                                                                                                                            Middle,
                                                                                                                        gripper
                                                                                                                            .
                                                                                                                            A
                                                                                                                            .
                                                                                                                            Middle,
                                                                                                                        5);
                                                                                                                   //MDG.2011-05-31.Velocidad media, porque velocidad alta ahora es mayor
                                                                                                                   //gripper.MoveRelativeChain(new Point3D(20, -20, -30), gripper.V.Middle, gripper.A.Middle, 5);//MDG.2011-06-22.Subimos 30 en vez de 10 para que vikex no choque
                                                                                                               };

                chain.Add(new Step("Subir pinza hasta el techo")).Task = () => { gripper.MoveToCeilingChain(20); };

                chain.Add(new Step("Ir a segunda posición de seguridad de dejada palet")).Task = () =>
                                                                                                     {
                                                                                                         Context.
                                                                                                             ElementCatched
                                                                                                             ();

                                                                                                         var
                                                                                                             securityPosition1
                                                                                                                 =
                                                                                                                 chain.
                                                                                                                     Param
                                                                                                                     <
                                                                                                                         Point2D
                                                                                                                         >
                                                                                                                     (LeftPaletParams
                                                                                                                          .
                                                                                                                          securityPosition1);
                                                                                                         //gripper.MoveToCeilingChain(securityPosition1, gripper.V.Hight, gripper.A.Hight, 40);
                                                                                                         gripper.
                                                                                                             MoveToCeilingChain
                                                                                                             (securityPosition1,
                                                                                                              gripper.V.
                                                                                                                  Middle,
                                                                                                              gripper.A.
                                                                                                                  Middle,
                                                                                                              40);
                                                                                                         //MDG.2011-05-31.Velocidad media, porque velocidad alta ahora es mayor
                                                                                                     };

                chain.Add(new Step("Ir a primera posición de seguridad de dejada palet")).Task = () =>
                                                                                                     {
                                                                                                         var
                                                                                                             securityposition2
                                                                                                                 =
                                                                                                                 chain.
                                                                                                                     Param
                                                                                                                     <
                                                                                                                         Point3D
                                                                                                                         >
                                                                                                                     (LeftPaletParams
                                                                                                                          .
                                                                                                                          securityPosition2);
                                                                                                         var spin =
                                                                                                             Subchain(
                                                                                                                 Chains.
                                                                                                                     LeavePalet)
                                                                                                                 .Param
                                                                                                                 <Spin>(
                                                                                                                     LeftPaletParams
                                                                                                                         .
                                                                                                                         spin);
                                                                                                         //MDG.2011-03.14.Admitimos giro pinza la coger palet Vikex

                                                                                                         gripper.
                                                                                                             SecurityMoveChain
                                                                                                             (
                                                                                                                 securityposition2,
                                                                                                                 () =>
                                                                                                                 gripper
                                                                                                                     .
                                                                                                                     SetCheckChain
                                                                                                                     (spin,
                                                                                                                      Extensor
                                                                                                                          .
                                                                                                                          Any,
                                                                                                                      Arms
                                                                                                                          .
                                                                                                                          Any,
                                                                                                                      Sucker
                                                                                                                          .
                                                                                                                          Any),
                                                                                                                 100,
                                                                                                                 () =>
                                                                                                                 gripper
                                                                                                                     .
                                                                                                                     SetCheckChain
                                                                                                                     (spin,
                                                                                                                      Extensor
                                                                                                                          .
                                                                                                                          Any,
                                                                                                                      Arms
                                                                                                                          .
                                                                                                                          Any,
                                                                                                                      Sucker
                                                                                                                          .
                                                                                                                          Any),
                                                                                                                 1
                                                                                                             //MDG.2011-06-15//0
                                                                                                             );
                                                                                                     };

                chain.Add(new Step("Ir a posición de dejada palet", 20000)).Task = () =>
                                                                                       {
                                                                                           var position =
                                                                                               chain.Param<Point3D>(
                                                                                                   LeftPaletParams.
                                                                                                       position);
                                                                                           gripper.MoveChain(position,
                                                                                                             gripper.V.
                                                                                                                 Low,
                                                                                                             gripper.A.
                                                                                                                 Low);
                                                                                       };

                chain.Add(new Step("Comprobar brazos pinza abiertos", 5000)).Task = () =>
                                                                                        {
                                                                                            gripper.Abrir();
                                                                                            if (gripper.Abierta)
                                                                                            {
                                                                                                Context.NextState();
                                                                                                Return();
                                                                                            }
                                                                                        };
            }

            private void CatchItem_Steps()
            {
                Subchain chain = AddSubchain(Chains.CatchItem);
                chain.AddParams(CatchBoxParams.spin,
                                CatchBoxParams.extensor,
                                CatchBoxParams.position);

                chain.Add(new Step("Mover pinza a posición de cogida caja")).Task = () =>
                                                                                        {
                                                                                            var position =
                                                                                                chain.Param<Point3D>(
                                                                                                    CatchBoxParams.
                                                                                                        position);
                                                                                            var spin =
                                                                                                Subchain(
                                                                                                    Chains.CatchItem).
                                                                                                    Param<Spin>(
                                                                                                        CatchBoxParams.
                                                                                                            spin);

                                                                                            gripper.SecurityMoveChain
                                                                                                (
                                                                                                    position,
                                                                                                    () =>
                                                                                                    gripper.
                                                                                                        SetCheckChain(
                                                                                                            spin,
                                                                                                            Extensor.
                                                                                                                Retract,
                                                                                                            Arms.Any,
                                                                                                            Sucker.Off),
                                                                                                    10,
                                                                                                    //MDG.2011-06-15//50,
                                                                                                    () =>
                                                                                                    gripper.
                                                                                                        SetCheckChain(
                                                                                                            spin,
                                                                                                            Extensor.
                                                                                                                Retract,
                                                                                                            Arms.Close,
                                                                                                            Sucker.On),
                                                                                                    1
                                                                                                //MDG.2011-06-15//0
                                                                                                );
                                                                                        };

                chain.Add(new Step("Posicionar extensor pinza")).Task = () =>
                                                                            {
                                                                                var extensor =
                                                                                    Subchain(Chains.CatchItem).Param
                                                                                        <Extensor>(
                                                                                            CatchBoxParams.extensor);
                                                                                gripper.SequentialSetCheckChain(
                                                                                    Spin.Any, extensor, Arms.Any,
                                                                                    Sucker.On);
                                                                            };

                chain.Add(new Step("Guardar Estado Catalogos", 5000)).Task = () =>
                                                                                 {
                                                                                     try
                                                                                     {
                                                                                         //MDG.2010-11-24.Guardado catálogos al depositar cada caja
                                                                                         _sys.Production.SaveCatalogs();
                                                                                         //MDG.2011-06-13.Guardado bandas y transportes al depositar cada caja
                                                                                         _sys.Production.
                                                                                             SaveLinesGroupsAndBoxes();
                                                                                         //guardado en fichero
                                                                                         _sys.Production.
                                                                                             SaveTransportGroups();
                                                                                         //guardado en fichero
                                                                                     }
                                                                                     catch
                                                                                     {
                                                                                     }

                                                                                     //_chainController.NextStep();
                                                                                     NextStep();
                                                                                 };

                chain.Add(new Step("Actualizar estado")).Task = () =>
                                                                    {
                                                                        Context.ElementCatched();
                                                                        Context.NextState();

                                                                        Return();
                                                                    };
            }

            private void CheckItemOK_Steps()
            {
                Subchain chain = AddSubchain(Chains.CheckItemOK);
                TON timer = null;

                chain.Add(new Step("Paso inicial")).Task = () =>
                                                               {
                                                                   timer = new TON();
                                                                   gripper.MoveToCeilingChain(0);
                                                               };

                chain.Add(new Step("Iniciar configuración bascula")).Task = () =>
                                                                                {
                                                                                    if (
                                                                                        gripper.BeginWeightConfiguration
                                                                                            ())
                                                                                    {
                                                                                        NextStep();
                                                                                    }
                                                                                };

                chain.Add(new Step("Finalizar configuración bascula")).Task = () =>
                                                                                  {
                                                                                      if (
                                                                                          gripper.EndWeightConfiguration
                                                                                              ())
                                                                                      {
                                                                                          NextStep();
                                                                                      }
                                                                                  };

                chain.Add(new Step("Temporizar antes de pesar")).Task = () =>
                                                                            {
                                                                                if (timer.Timing(2000))
                                                                                {
                                                                                    NextStep();
                                                                                }
                                                                            };

                chain.Add(new Step("Inicio pesar caja")).Task = () =>
                                                                    {
                                                                        if (gripper.BeginWeightItem())
                                                                        {
                                                                            NextStep();
                                                                        }
                                                                    };

                chain.Add(new Step("Final pesar caja")).Task = () =>
                                                                   {
                                                                       if (gripper.EndWeightItem())
                                                                       {
                                                                           Context.NextState();
                                                                           Return();
                                                                       }
                                                                   };
            }

            //MDG.2013-04-25
            private void CheckItemOkDespaletizing_Steps()
            {
                Subchain chain = AddSubchain(Chains.CheckItemOKDespaletizing);
                TON timer = null;

                chain.Add(new Step("Paso inicial")).Task = () =>
                {
                    timer = new TON();
                    gripper.MoveToCeilingChain(0);
                };

                chain.Add(new Step("Iniciar configuración bascula")).Task = () =>
                {
                    if (gripper.BeginWeightConfiguration())
                    {
                        NextStep();
                    }
                };

                chain.Add(new Step("Finalizar configuración bascula")).Task = () =>
                {
                    if (
                        gripper.EndWeightConfiguration
                            ())
                    {
                        NextStep();
                    }
                };

                chain.Add(new Step("Temporizar antes de pesar")).Task = () =>
                {
                    if (timer.Timing(2000))
                    {
                        NextStep();
                    }
                };

                chain.Add(new Step("Inicio pesar caja")).Task = () =>
                {
                    if (gripper.BeginWeightItem())
                    {
                        NextStep();
                    }
                };

                chain.Add(new Step("Final pesar caja")).Task = () =>
                {
                    if (gripper.EndWeightItem())
                    {
                        Context.NextState();
                        Return();
                    }
                };
            }

            private void LeaveItem_Steps()
            {
                Subchain chain = AddSubchain(Chains.LeaveItem);
                chain.AddParams(LeftBoxParams.position,
                                LeftBoxParams.spin,
                                LeftBoxParams.extensor,
                                LeftBoxParams.securityPositions);

                //MDG.2011-03-29.Añado este paso para subir la caja hasta arriba antes de girarla
                chain.Add(new Step("Subir caja hasta arriba")).Task = () => { gripper.MoveToCeilingChain(0); };

                chain.Add(new Step("Trasladar caja a posición de seguridad de dejada")).Task = () =>
                                                                                                   {
                                                                                                       var
                                                                                                           securityPositions
                                                                                                               =
                                                                                                               chain.
                                                                                                                   Param
                                                                                                                   <
                                                                                                                       IEnumerable
                                                                                                                           <
                                                                                                                               Point3D
                                                                                                                               >
                                                                                                                       >
                                                                                                                   (LeftBoxParams
                                                                                                                        .
                                                                                                                        securityPositions);
                                                                                                       var spin =
                                                                                                           chain.Param
                                                                                                               <Spin>(
                                                                                                                   LeftBoxParams
                                                                                                                       .
                                                                                                                       spin);

                                                                                                       //MDG.2011-03-28.Giramos la pinza a posicion de destino desde el principio
                                                                                                       gripper.
                                                                                                           SecurityMoveChain
                                                                                                           (
                                                                                                               securityPositions,
                                                                                                               () =>
                                                                                                               gripper.
                                                                                                                   SetCheckChain
                                                                                                                   (spin,
                                                                                                                    Extensor
                                                                                                                        .
                                                                                                                        Retract,
                                                                                                                    Arms
                                                                                                                        .
                                                                                                                        Close,
                                                                                                                    Sucker
                                                                                                                        .
                                                                                                                        On),
                                                                                                               100,
                                                                                                               () =>
                                                                                                               gripper.
                                                                                                                   SetCheckChain
                                                                                                                   (spin,
                                                                                                                    Extensor
                                                                                                                        .
                                                                                                                        Retract,
                                                                                                                    Arms
                                                                                                                        .
                                                                                                                        Close,
                                                                                                                    Sucker
                                                                                                                        .
                                                                                                                        On),
                                                                                                               5
                                                                                                           );

                                                                                                       //gripper.SecurityMoveChain
                                                                                                       //(
                                                                                                       //    securityPositions,
                                                                                                       //    () => gripper.SetCheckChain(Spin.Any, Extensor.Retract, Arms.Close, Sucker.On),
                                                                                                       //    100,
                                                                                                       //    () => gripper.SetCheckChain(spin, Extensor.Retract, Arms.Close, Sucker.On),
                                                                                                       //    5
                                                                                                       //);
                                                                                                   };

                chain.Add(new Step("Mover pinza a posición de dejada caja")).Task = () =>
                                                                                        {
                                                                                            var position =
                                                                                                chain.Param<Point3D>(
                                                                                                    LeftBoxParams.
                                                                                                        position);
                                                                                            gripper.MoveChain(position,
                                                                                                              gripper.V.
                                                                                                                  Hight,
                                                                                                              gripper.A.
                                                                                                                  Hight);
                                                                                        };

                chain.Add(new Step("Depositar caja", 5000)).Task = () =>
                                                                       {
                                                                           var extensor =
                                                                               chain.Param<Extensor>(
                                                                                   LeftBoxParams.extensor);
                                                                           gripper.SequentialSetCheckChain(Spin.Any,
                                                                                                           extensor,
                                                                                                           Arms.Any,
                                                                                                           Sucker.Off);
                                                                       };

                chain.Add(new Step("Guardar Estado Catalogos", 5000)).Task = () =>
                                                                                 {
                                                                                     try
                                                                                     {
                                                                                         //MDG.2010-07-13.Guardado catálogos al depositar cada caja
                                                                                         _sys.Production.SaveCatalogs();
                                                                                         //guardado en fichero
                                                                                         //MDG.2011-06-13.Guardado bandas y transportes al depositar cada caja
                                                                                         _sys.Production.
                                                                                             SaveLinesGroupsAndBoxes();
                                                                                         //guardado en fichero
                                                                                         _sys.Production.
                                                                                             SaveTransportGroups();
                                                                                         //guardado en fichero
                                                                                     }
                                                                                     catch
                                                                                     {
                                                                                     }
                                                                                     //_chainController.NextStep();
                                                                                     NextStep();
                                                                                 };

                chain.Add(new Step("Establecer siguiente estado", 5000)).Task = () =>
                                                                                    {
                                                                                        Context.NextState();
                                                                                        Return();
                                                                                    };
            }

            private void CatchSeparator_Steps()
            {
                Subchain chain = AddSubchain(Chains.CatchSeparator);
                chain.AddParams(CatchBoardParams.initialPosition, CatchBoardParams.finalPosition);

                chain.Add(new Step("Mover pinza a posición inicial de cogida de carton")).Task = () =>
                                                                                                     {
                                                                                                         var
                                                                                                             initialPosition
                                                                                                                 =
                                                                                                                 chain.
                                                                                                                     Param
                                                                                                                     <
                                                                                                                         Point3D
                                                                                                                         >
                                                                                                                     (CatchBoardParams
                                                                                                                          .
                                                                                                                          initialPosition);

                                                                                                         gripper.
                                                                                                             SecurityMoveChain
                                                                                                             (
                                                                                                                 initialPosition,
                                                                                                                 () =>
                                                                                                                 gripper
                                                                                                                     .
                                                                                                                     SetCheckChain
                                                                                                                     (Spin
                                                                                                                          .
                                                                                                                          Any,
                                                                                                                      Extensor
                                                                                                                          .
                                                                                                                          Retract,
                                                                                                                      Arms
                                                                                                                          .
                                                                                                                          Close,
                                                                                                                      Sucker
                                                                                                                          .
                                                                                                                          Off),
                                                                                                                 50,
                                                                                                                 () =>
                                                                                                                 gripper
                                                                                                                     .
                                                                                                                     SetCheckChain
                                                                                                                     (Spin
                                                                                                                          .
                                                                                                                          S90,
                                                                                                                      Extensor
                                                                                                                          .
                                                                                                                          Dead,
                                                                                                                      Arms
                                                                                                                          .
                                                                                                                          Close,
                                                                                                                      Sucker
                                                                                                                          .
                                                                                                                          Off),
                                                                                                                 1
                                                                                                             //MDG.2011-06-15//0
                                                                                                             );
                                                                                                     };

                chain.Add(new Step("Iniciar movimiento eje Z pinza", 5000)).Task = () =>
                                                                                       {
                                                                                           var finalPosition =
                                                                                               chain.Param<Point3D>(
                                                                                                   CatchBoardParams.
                                                                                                       finalPosition);

                                                                                           if (
                                                                                               gripper.Z.StartMov(
                                                                                                   (int) finalPosition.Z,
                                                                                                   V.Low, A.Hight))
                                                                                           {
                                                                                               gripper.VacioOn();
                                                                                               NextStep();
                                                                                           }
                                                                                       };

                chain.Add(new Step("Finalizar movimiento pinza", 10000)).Task = () =>
                                                                                    {
                                                                                        if (gripper.EnVacioOn)
                                                                                        {
                                                                                            gripper.Z.Parada();
                                                                                            NextStep();
                                                                                        }
                                                                                    };

                chain.Add(new Step("Comprobar movimiento eje z finalizado", 5000)).Task = () =>
                                                                                              {
                                                                                                  if (
                                                                                                      !gripper.Z.
                                                                                                           DriveMoving())
                                                                                                  {
                                                                                                      if (
                                                                                                          gripper.Z.
                                                                                                              DriverResurrection
                                                                                                              ())
                                                                                                      {
                                                                                                          Context.
                                                                                                              ElementCatched
                                                                                                              ();
                                                                                                          Context.
                                                                                                              NextState();
                                                                                                          Return();
                                                                                                      }
                                                                                                  }
                                                                                              };
            }

            private void LeaveSeparator_Steps()
            {
                Subchain chain = AddSubchain(Chains.LeaveSeparator);
                chain.AddParams(LeftBoardParams.position);

                chain.Add(new Step("Mover pinza a posición de dejada de cartón")).Task = () =>
                                                                                             {
                                                                                                 var position =
                                                                                                     chain.Param
                                                                                                         <Point3D>(
                                                                                                             LeftBoardParams
                                                                                                                 .
                                                                                                                 position);

                                                                                                 gripper.
                                                                                                     SecurityMoveChain
                                                                                                     (
                                                                                                         position,
                                                                                                         () =>
                                                                                                         gripper.
                                                                                                             SetCheckChain
                                                                                                             (
                                                                                                                 Spin.
                                                                                                                     Any,
                                                                                                                 Extensor
                                                                                                                     .
                                                                                                                     Retract,
                                                                                                                 Arms.
                                                                                                                     Close,
                                                                                                                 Sucker.
                                                                                                                     On),
                                                                                                         100,
                                                                                                         () =>
                                                                                                         gripper.
                                                                                                             SetCheckChain
                                                                                                             (
                                                                                                                 Spin.
                                                                                                                     S90,
                                                                                                                 Extensor
                                                                                                                     .
                                                                                                                     Dead,
                                                                                                                 Arms.
                                                                                                                     Close,
                                                                                                                 Sucker.
                                                                                                                     On),
                                                                                                         1
                                                                                                     //MDG.2011-06-15//0
                                                                                                     );
                                                                                             };

                chain.Add(new Step("Vacío plano aspirante")).Task = () =>
                                                                        {
                                                                            gripper.VacioOff();
                                                                            NextStep();
                                                                        };

                chain.Add(new Step("Comprobar plano aspirante no en vacío", 5000)).Task = () =>
                                                                                              {
                                                                                                  if (gripper.EnVacioOff)
                                                                                                  {
                                                                                                      Context.NextState();
                                                                                                      Return();
                                                                                                  }
                                                                                              };
            }

            private void GoToOrigin_Steps()
                //MDG.2011-06-14. Nuevo estado para posicionarnos encima de la posicion de recogida de cajas
            {
                Subchain chain = AddSubchain(Chains.GoToOrigin);
                chain.AddParams(GoToOriginParams.spin,
                                GoToOriginParams.extensor,
                                GoToOriginParams.position);

                ////MDG.2011-06-14.Añado este paso para subir hasta arriba antes de girar
                //chain.Add(new Step("Subir hasta arriba movimiento origen")).Task = () =>
                //{
                //    gripper.MoveToCeilingChain(0);
                //};

                chain.Add(new Step("Mover pinza a posición de origen (cogida caja)")).Task = () =>
                                                                                                 {
                                                                                                     var position =
                                                                                                         chain.Param
                                                                                                             <Point3D>(
                                                                                                                 GoToOriginParams
                                                                                                                     .
                                                                                                                     position);
                                                                                                     //var position = new PointSpin3D(Spin.S0, (775+20), 1370, 140);//Para que se mueva a una psoicion fija
                                                                                                     var spin =
                                                                                                         Subchain(
                                                                                                             Chains.
                                                                                                                 GoToOrigin)
                                                                                                             .Param
                                                                                                             <Spin>(
                                                                                                                 GoToOriginParams
                                                                                                                     .
                                                                                                                     spin);

                                                                                                     //position.Z = Gripper.NegativeZLimit;//MDG.2011-06-15
                                                                                                     //var position 

                                                                                                     gripper.
                                                                                                         SecurityMoveChain
                                                                                                         (
                                                                                                             position,
                                                                                                             () =>
                                                                                                             gripper.
                                                                                                                 SetCheckChain
                                                                                                                 (spin,
                                                                                                                  Extensor
                                                                                                                      .
                                                                                                                      Retract,
                                                                                                                  Arms.
                                                                                                                      Any,
                                                                                                                  Sucker
                                                                                                                      .
                                                                                                                      Off),
                                                                                                             10, //50,
                                                                                                             () =>
                                                                                                             gripper.
                                                                                                                 SetCheckChain
                                                                                                                 (spin,
                                                                                                                  Extensor
                                                                                                                      .
                                                                                                                      Retract,
                                                                                                                  Arms.
                                                                                                                      Close,
                                                                                                                  Sucker
                                                                                                                      .
                                                                                                                      Off),
                                                                                                             1
                                                                                                         //MDG.2011-06-15//0
                                                                                                         );
                                                                                                 };

                chain.Add(new Step("Posicionar extensor pinza")).Task = () =>
                                                                            {
                                                                                var extensor =
                                                                                    Subchain(Chains.GoToOrigin).Param
                                                                                        <Extensor>(
                                                                                            GoToOriginParams.extensor);
                                                                                gripper.SequentialSetCheckChain(
                                                                                    Spin.Any, extensor, Arms.Any,
                                                                                    Sucker.Off);
                                                                            };

                //MDG.2011-06-16
                chain.Add(new Step("Guardar Estado Catalogos", 5000)).Task = () =>
                                                                                 {
                                                                                     try
                                                                                     {
                                                                                         //MDG.2010-11-24.Guardado catálogos al depositar cada caja
                                                                                         _sys.Production.SaveCatalogs();
                                                                                         //MDG.2011-06-13.Guardado bandas y transportes al depositar cada caja
                                                                                         _sys.Production.
                                                                                             SaveLinesGroupsAndBoxes();
                                                                                         //guardado en fichero
                                                                                         _sys.Production.
                                                                                             SaveTransportGroups();
                                                                                         //guardado en fichero
                                                                                     }
                                                                                     catch
                                                                                     {
                                                                                     }
                                                                                     NextStep();
                                                                                 };

                chain.Add(new Step("Actualizar estado")).Task = () =>
                                                                    {
                                                                        Context.NextState();

                                                                        Return();
                                                                    };
            }
        }

        #endregion
    }
}