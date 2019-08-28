using System;
using System.Collections.Generic;
using Idpsa.Control.Subsystem;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Beacon : IManagerRunnable
    {    
        private readonly Dictionary<IActivable, Element> _lights;
			
        public Beacon()
        {
            _lights = new Dictionary<IActivable, Element>();
        }

        public Beacon WihtLightOnWhen(IActivable light, IEvaluable condition)
        {           
            if (condition == null)
                throw new ArgumentNullException("condition");

            return WihtLightOnWhenCore(light, condition.Value);
        }
        public Beacon WihtLightOnWhen(IActivable light, Func<bool> condition)
        {
            return WihtLightOnWhenCore(light,condition);
        }
        public Beacon WithBlinkWhen(IActivable light, IEvaluable condition, int semiperidode)
        {            
            if (condition == null)
                throw new ArgumentNullException("condition");
          
            return WithBlinkWhenCore(light, condition.Value, semiperidode);             
        }
        public Beacon WithBlinkWhen(IActivable light, Func<bool> condition, int semiperidode)
        {            
            return WithBlinkWhenCore(light,condition,semiperidode); 
        }   
        public void TurnOff()
		{
            foreach (var light in _lights.Keys)
                light.Activate(false);
		}

        #region Miembros de IManagerRunable
        public IEnumerable<System.Action> GetManagers()
        {
            return new []{ new Action(()=>
                                        {
                                            foreach (var light in _lights)
                                                LightManager(light.Key, light.Value);
                                        }
                                      )
                         }; 
        }        
        #endregion
        
        private Beacon WihtLightOnWhenCore(IActivable light, Func<bool> condition)
        {
            if (light == null)
                throw new ArgumentNullException("light");
            if (condition == null)
                throw new ArgumentNullException("condition");

            if (!_lights.ContainsKey(light))
            {
                _lights.Add(light, new Element() { LightContition = condition });
            }
            else
            {
                _lights[light].LightContition = condition;

            }
            return this;
        }
        private Beacon WithBlinkWhenCore(IActivable light, Func<bool> condition, int semiperidode)
        {
            if (light == null)
                throw new ArgumentNullException("light");
            if (condition == null)
                throw new ArgumentNullException("condition");
            if (semiperidode < 0)
                throw new ArgumentOutOfRangeException("semiperiode can't be negative");

            if (!_lights.ContainsKey(light))
            {
                _lights.Add(light, new Element() { LightBlinkCondition = condition, Semiperiode = semiperidode, Timer = new TON() });
            }
            else
            {
                _lights[light].LightBlinkCondition = condition;
                _lights[light].Semiperiode = semiperidode;
                _lights[light].Timer = new TON();
            }

            return this;
        }
        private void LightManager(IActivable light, Element element)
        {
            if (element.LightContition())
            {
                light.Activate(true);
                return;
            }

            if (!element.LightBlinkCondition())
            {
                light.Activate(false);
                return;
            }

            if (element.Timer.TimingWithReset(element.Semiperiode, element.LightBlinkCondition()))
            {
                light.Activate(!light.Value());
                return;
            }
        }

        private class Element
        {
            public Element()
            {
                LightContition = () => false;
                LightBlinkCondition = () => false;
            }
            public Func<bool> LightContition { get; set; }
            public Func<bool> LightBlinkCondition { get; set; }
            public int Semiperiode { get; set; }
            public TON Timer { get; set; }
        }
    }
}