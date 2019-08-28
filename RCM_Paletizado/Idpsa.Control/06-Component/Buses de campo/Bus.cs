using System;
using System.Collections.Generic;

namespace Idpsa.Control.Component
{
    [Serializable]
    public abstract class Bus
    {
        public IOCollection InCollection { get; protected set; }
        public IOCollection OutCollection { get; protected set; }
        public bool Activated { get; set; }

        protected IBusController Controller { get; set; }
        protected Dictionary<string, Input> Inputs { get; set; }
        protected Dictionary<string, Output> Outputs { get; set; }

        protected Bus(int efectiveInputLeght, int efectiveOutputLength)
        {
            if (efectiveInputLeght < 0)           
                throw new ArgumentException("a Bus efectiveInputLeght can't be negative");

            if (efectiveOutputLength < 0)
                throw new ArgumentException("a Bus efectiveOutputLength can't be negative");
            
            InCollection = new IOCollection();
            Inputs = new Dictionary<string, Input>();
            OutCollection = new IOCollection();
            Outputs = new Dictionary<string, Output>();
            LoadSimbolic(efectiveInputLeght, efectiveOutputLength);
            CreateController();
            if (Controller == null)            
                throw new NullReferenceException("controller");
            
            Activated = true;
        }
        public Input In(string key)
        {
            if (!Inputs.ContainsKey(key))            
                throw new ArgumentException(String.Format("The input with simbolic {0} has not been defined", key));
            
            return Inputs[key];
        }
        public Output Out(string key)
        {
            if (!Outputs.ContainsKey(key))            
                throw new ArgumentException(String.Format("The output with simbolic {0} has not been defined", key));
            
            return Outputs[key];
        }
        public Input TryIn(string key)
        {
            Input value;
            Inputs.TryGetValue(key, out value);
            return value;
        }
        public Output TryOut(string key)
        {
            Output value;
            Outputs.TryGetValue(key, out value);
            return value;
        }
        public void AddInput(Input input)
        {
            if (input == null)
                throw new ArgumentNullException("input"); 
            
            InCollection.Add(input.Symbol, input.IOSignal);
            Inputs.Add(input.Symbol, input);
        }        
        public void AddOutput(Output output)
        {
            if (output == null)
                throw new ArgumentNullException("output");

            OutCollection.Add(output.Symbol, output.IOSignal);
            Outputs.Add(output.Symbol, output);
        }
        public void RemoveInput(string key)
        {
            Inputs.Remove(key);
        }
        public void RemoveOutput(string key)
        {
            Outputs.Remove(key);
        }
        public void Clear()
        {
            Inputs.Clear();
            Outputs.Clear();
            InCollection.Clear();
            OutCollection.Clear();
        }               
        public bool ExistInput(string key)
        {
            return Inputs.ContainsKey(key);
        }
        public bool ExistsOutput(string key)
        {
            return Outputs.ContainsKey(key);
        }
        public virtual bool IsBusOK()
        {
            return Controller.IsBusOK();
        }
        public virtual void Run()
        {
            if (Activated)
            {
                Controller.RunDevice();
            }
            else
            {
                Controller.ResetOutputs();
            }
        }
        public virtual void ResetOutputs()
        {
            Controller.ResetOutputs();
        }
        public virtual void WakeUpDevice()
        {
            Controller.WakeUpDevice();
        }
        public IBusController GetController()
        {
            return Controller;
        }

        protected abstract void LoadSimbolic(int efectiveInputLength, int efectiveOutputLength);
        protected abstract void CreateController();
    }
}