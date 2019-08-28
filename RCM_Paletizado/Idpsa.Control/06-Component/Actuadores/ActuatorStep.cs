using System;
using System.Collections.Generic;
using Idpsa.Control.Sequence; 


namespace Idpsa.Control.Component
{
    public class ActuatorStep:IActivable,IChainControllersOwner 
    {
        private Actuator _actuator;        
        private Func<bool> _stopCondition;
        private Action _actionAfterStop;
        private ChainController _chainController;
        private enum Chains { MoveStepChain };

        public ActuatorStep(Actuator actuator, IEvaluable stopCondition)
        {
            if (actuator == null)
                throw new ArgumentNullException("actuator");
            if (stopCondition == null)
                throw new ArgumentNullException("stopCondition");
            _actuator = actuator;            
            _stopCondition = () => stopCondition.Value();
            _chainController = new ChainController();
        }

        public ActuatorStep(Actuator actuator, Func<bool> stopCondition)
        {
            if (actuator == null)
                throw new ArgumentNullException("actuator");
            if (stopCondition == null)
                throw new ArgumentNullException("stopCondition");
            _actuator = actuator;
            _stopCondition = stopCondition;
            _chainController = new ChainController();
        }

        public ActuatorStep WithActionAfterStop(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            _actionAfterStop = action;
            return this;
        }
      
        public bool ConditionValue()
        {
            return _stopCondition();
        }

        public Action GetActionAfterStop()
        {
            return _actionAfterStop; 
        }

        public void MoveStepChain()
        {
            _chainController.CallChain(Chains.MoveStepChain);
        }

        private void MoveStepChain_Steps()
        {
            var chain = _chainController.AddChain(Chains.MoveStepChain);           

            chain.Add(new Step("Inicion cadena movimiento paso")).Task = () =>
            {
                _actuator.Activate(true); 
                if (_stopCondition())                   
                    _chainController.NextStep();                    
                else
                    _chainController.GoToStep("paroMotor");                   
            };

            chain.Add(new Step(()=> "Esperar no cumpliento condición de paro motor "+Name)).Task = () =>
            {
                if (!_stopCondition())                
                    _chainController.NextStep();               
            };

            chain.Add(new Step("Eperar cumpliento condición de paro motor " + Name).WithTag("paroMotor")).Task = () =>
            {
                if (_stopCondition())
                {                    
                    _actuator.Activate(false);
                    if (_actionAfterStop != null) _actionAfterStop();                  
                        _chainController.Return(); 
                }
            };      
        }
               
        public void Activate(bool work)
        {
            _actuator.Activate(work);
        }

        

        public string Name
        {
            get
            {
                return _actuator.Name;
            }           
        }       

       

        public bool Value()
        {
            return _actuator.Value();
        }



        #region Miembros de IChainControllersOwner

        public IEnumerable<IChainController> GetChainControllers()
        {
            return new IChainController[] { _chainController };
        }

        #endregion
    }
}
