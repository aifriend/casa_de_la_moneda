using System;
using System.Collections.Generic;
using Idpsa.Control.Sequence;
using Idpsa.Control.Manuals;
using Idpsa.Control.Tool;
 
namespace Idpsa.Control.Component
{
    [Serializable]
    public class GyreActuator : IChainControllersOwner, IManualsProvider  
    {
        private readonly ChainController _chainController;
        private readonly ActuatorWithInversorSimple _electroValvula0;
        private readonly ActuatorWithInversorSimple _electroValvula360;

        public string Name{get;private set;}

        readonly IEvaluable _sensor270;
        readonly IEvaluable _sensor0;
        readonly IEvaluable _sensor90;        

        private enum v
        {
            rest,
            dead,
            work
        };

        private static void Activate(ActuatorWithInversorSimple electroValvula, v value)
        {
            switch (value)
            {
                case v.rest: electroValvula.Activate1(); break;
                case v.work: electroValvula.Activate2(); break;
                case v.dead: electroValvula.Deactivate(); break;
            }
        }

        public GyreActuator(IEvaluable sensor0º, IEvaluable sensor270º, IEvaluable sensor90º,
                            ActuatorWithInversorSimple electroValvula0, 
                            ActuatorWithInversorSimple electroValvula360, 
                            string parName)
        {
            Name = parName;

            _sensor270 = new SensorP(sensor270º);
            _sensor0 = new SensorP(sensor0º);
            _sensor90 = new SensorP(sensor90º);
            _electroValvula0 = electroValvula0;
            _electroValvula360 = electroValvula360;

            _chainController = new ChainController();

            GyrateChainSteps();
        }

        private static PairList<v, v> GetSequence(Spin currentSpin, Spin targetSpin)
        {
            var squence = new PairList<v,v>();

            switch (currentSpin)
            {
                case Spin.S90:
                case Spin.S0:
                case Spin.S270:
                    switch (targetSpin)
                    {
                        case Spin.S0:
                            squence = new PairList<v, v> {{v.work, v.dead}};
                            break;
                        case Spin.S90:
                            squence = new PairList<v, v> {{v.rest, v.rest}};
                            break;
                        case Spin.S270:
                            squence = new PairList<v, v> {{v.rest, v.work}};
                            break;
                    } 
                    break;
            }

            return squence;
            
        }

        public void Gyrate(Spin targetSpin)
        {
            var currentSpin = InGyre();
            if (currentSpin == Spin.Any)
                currentSpin = Spin.S0;
            var squence = GetSequence(currentSpin, targetSpin);
            Activate(_electroValvula0, squence[0].Value1);
            Activate(_electroValvula360, squence[0].Value2);
        }

        public void GyrateChain(Spin spin)
        {
            _chainController.CallChain("giro").WithParam("spin", spin);
        }
      
        private void GyrateChainSteps()
        {
            var chain = _chainController.AddChain("giro");
            Spin currentSpin = default(Spin);
            Spin targetSpin = default(Spin);
            int index = 0;
            chain.AddParam("spin");
            PairList<v,v> squence=null;

            chain.Add(new Step("Comprobar posición actual giro pinza válida", 5000))
                .Task = () =>
                            {
                                currentSpin = InGyre();
                                if (currentSpin == Spin.Any)
                                    currentSpin = Spin.S0;
                                index = 0;
                                targetSpin = chain.Param<Spin>("spin");
                                squence = GetSequence(currentSpin, targetSpin);
                                _chainController.NextStep();
                            };

            chain.Add(
                new Step(
                    () =>
                    "Realización paso " + (index + 1) + " giro pinza " + currentSpin.ToStringImage() + " -> " +
                    targetSpin.ToStringImage()))
                .Task = () =>
                            {
                                if (index < squence.Count)
                                {
                                    var v = squence[index++];
                                    Activate(_electroValvula0, v.Value1);
                                    Activate(_electroValvula360, v.Value2);
                                }
                                else
                                {
                                    _chainController.Return();
                                }
                            };
        }
        
        public bool InGyre(Spin spin)
        {
            var value = false;
            switch (spin)
            {
                case Spin.S0:
                    value = !_sensor270.Value() && _sensor0.Value() && !_sensor90.Value();
                    break;
                case Spin.S90:
                    value = !_sensor270.Value() && !_sensor0.Value() && _sensor90.Value();
                    break;
                case Spin.S270:
                    value = _sensor270.Value() && !_sensor0.Value() && !_sensor90.Value();
                    break;
                case Spin.Any:
                    value = true;
                    break;
            }
            return value;
        }

        private Spin InGyre()
        {
            if (InGyre(Spin.S0)) return Spin.S0;
            if (InGyre(Spin.S90)) return Spin.S90;
            return InGyre(Spin.S270) ? Spin.S270 : Spin.Any;
        }

        #region Miembros de IChainControllersOwner

        public IEnumerable<IChainController> GetChainControllers()
        {
            return new[] { _chainController };
        }

        #endregion

        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            DynamicManual dynamicManual = new DynamicManual(this)
                .WithLeftStep(new DynamicStepBody(() => "Girar -90", () => GyrateChain(Spin.S270)))
                .WithLeftStepName("-90º")
                .WithLeftCondition(_sensor270)
                .WithCenterStep(new DynamicStepBody(() => "Girar 0º", () => GyrateChain(Spin.S0)))
                .WithCenterStepName("0º")
                .WithCenterCondition(_sensor0)
                .WhitRightStep(new DynamicStepBody(() => "Girar +90", () => GyrateChain(Spin.S90)))
                .WhitRightStepName("+90º")
                .WithRightCondition(_sensor90);            

           return new[] { new Manual(dynamicManual){Description=Name} };
        }

        #endregion
    }
}