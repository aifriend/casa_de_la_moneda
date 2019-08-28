using System;
using System.Collections.Generic;
using Idpsa.Control;
using System.Linq;
using Idpsa.Control.Sequence;
using Idpsa.Control.Manuals;
using Idpsa.Control.Tool;
 
namespace Idpsa.Control.Component
{
    [Serializable]
    public class GyreActuatorSommerSFM : IChainControllersOwner,IManualsProvider  
    {
        private ChainController _chainController;
        private ActuatorWithInversorSimple _electroValvula1;
        private ActuatorWithInversorSimple _electroValvula2;
        private TON _timer;
        private const int _periode = 300;

        public string Name{get;private set;}

        IEvaluable _sensor_270;
        IEvaluable _sensor_0;
        IEvaluable _sensor_90;        

        private enum v{a,o,b};

        private void Activate(ActuatorWithInversorSimple electroValvula, v value, bool invert)
        {
            if (invert)
                value = Invert(value);

            switch (value)
            {
                case v.a: electroValvula.Activate1(); break;
                case v.b: electroValvula.Activate2(); break;
                case v.o: electroValvula.Deactivate(); break;
            }

        }

        private v Invert(v value){
            return (value==v.o)? v.o:(value==v.a)? v.b:v.a;
        }

       
        private static PairList<v,v> s270_90=
            new PairList<v, v>() { { v.b, v.o }, { v.b, v.o }, { v.b, v.a }, { v.a, v.a }, { v.a, v.a }, { v.a, v.a }, { v.a, v.a }, { v.a, v.o } };

        private static PairList<v, v> s270_0 = new PairList<v,v>() {{v.b,v.o},{v.a,v.o}};

        private static PairList<v,v> s0_270=
            new PairList<v, v>() { { v.o, v.b }, { v.o, v.b }, { v.o, v.b }, { v.o, v.b }, { v.o, v.b }, { v.b, v.o } };

        private static PairList<v, v> s90_270=new PairList<v,v>(),s90_0=new PairList<v,v>(),s0_90=new PairList<v,v>();       
               

        public GyreActuatorSommerSFM(IEvaluable sensor_270º, IEvaluable sensor_0º, IEvaluable sensor_90º,

            ActuatorWithInversorSimple electroValvula1, ActuatorWithInversorSimple electroValvula2, string parName)
        {
            Name = parName;
            //s0_270.AddRange(s0_270);
            _sensor_270 = new SensorP(sensor_270º);
            _sensor_0 = new SensorP(sensor_0º);
            _sensor_90 = new SensorP(sensor_90º);
            _electroValvula1 = electroValvula1;
            _electroValvula2=electroValvula2;             
            
            foreach (var v in s270_90)
                s90_270.Add(new Pair<v, v>(Invert(v.Value1), Invert(v.Value2)));
            foreach (var v in s270_0)
                s90_0.Add(new Pair<v, v>(Invert(v.Value1), Invert(v.Value2)));
            foreach (var v in s0_270)
                s0_90.Add(new Pair<v, v>(Invert(v.Value1), Invert(v.Value2)));

            _chainController = new ChainController();
            GyrateChain_Steps();
        }

        private PairList<v, v> GetSequence(Spin currentSpin,Spin targetSpin)
        {
            PairList<v,v> squence= new PairList<v,v>();

            switch (targetSpin)
            {
                case  Spin.S270:
                    
                    if(currentSpin== Spin.S0)
                        squence= s0_270;
                    else if(currentSpin== Spin.S90)
                        squence=s90_270; 
                    break;

                case Spin.S0:

                    if (currentSpin == Spin.S270)
                        squence = s270_0;
                    else if (currentSpin == Spin.S90)
                        squence = s90_0;
                    break;
                
                case Spin.S90:

                    if(currentSpin== Spin.S270)
                        squence=s270_90;
                    else if(currentSpin== Spin.S0)
                        squence=s0_90;  
                    break;
            }

            return squence;
            
        }

        public void GyrateChain(Spin spin)
        {
            _chainController.CallChain("giro")
                .WithParam("spin", spin);
        }
      
        private void GyrateChain_Steps()
        {
            var chain = _chainController.AddChain("giro");
            Spin currentSpin=default(Spin);
            Spin targetSpin = default(Spin);
            int index=0;
            chain.AddParam("spin");
            PairList<v,v> squence=null;
            

            chain.Add(new Step( "Comprobar posición actual giro pinza válida", 5000)).Task = () =>
            {
                currentSpin = InGyre();
                if (currentSpin== Spin.Any)
                    currentSpin= Spin.S270;
                if (currentSpin != Spin.Any)
                {
                    _timer = new TON();
                    index=0;                    
                    targetSpin = chain.Param<Spin>("spin");
                    squence = GetSequence(currentSpin, targetSpin); 
                    _chainController.NextStep();
                }
            };

            chain.Add(new Step(() => "Realización paso " + (index + 1) + " giro pinza " + currentSpin.ToStringImage() +" -> " + targetSpin.ToStringImage())).Task = () =>
            {
                if (_timer.Timing(_periode))
                {
                    if (index < squence.Count)
                    {
                        var v = squence[index++];
                        Activate(_electroValvula1, v.Value1,true);
                        Activate(_electroValvula2, v.Value2,true); 
                    }
                    else
                    {
                        _chainController.Return();
                    }
                }
            };       
        }
        
        public bool InGyre(Spin spin)
        {
            bool value = false;
            switch (spin)
            {
                case Spin.S0:
                    value = !_sensor_270.Value() && _sensor_0.Value() && !_sensor_90.Value();
                    break;
                case Spin.S90:
                    value = !_sensor_270.Value() && !_sensor_0.Value() && _sensor_90.Value();
                    break;
                case Spin.S270:
                    value = _sensor_270.Value() && !_sensor_0.Value() && !_sensor_90.Value();
                    break;
                case Spin.Any:
                    value = true;
                    break;
            }
            return value;
        }

        private Spin InGyre(){
            if (InGyre(Spin.S0)) return Spin.S0;
            else if (InGyre(Spin.S90)) return Spin.S90;
            else if (InGyre(Spin.S270)) return Spin.S270;
            else return Spin.Any; 
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
         
                         .WithLeftStep(new DynamicStepBody(() => "Girar -90", ()=>GyrateChain(Spin.S270)))
                         .WithLeftStepName("-90º")
                         .WithLeftCondition(_sensor_270)

                         .WithCenterStep(new DynamicStepBody(() => "Girar 0º",()=> GyrateChain(Spin.S0)))
                         .WithCenterStepName("0º")
                         .WithCenterCondition(_sensor_0)

                         .WhitRightStep(new DynamicStepBody(() => "Girar +90", ()=> GyrateChain(Spin.S90)))
                         .WhitRightStepName("+90º")
                         .WithRightCondition(_sensor_90);              

           return new[] { new Manual(dynamicManual){Description=Name} };
        }

        #endregion
    }
}