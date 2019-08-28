using System;
using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Manuals;


namespace Idpsa.Control.Component
{
    [Serializable]
    public abstract class Sensor : IEvaluable, IManualsProvider
    {
        private string _name;
        private string _description;

        protected Sensor(IEvaluable input, string description)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            if (description == null)
                throw new ArgumentNullException("description");
            In = input;
            _description = description;
            _name = input.ToString() ?? String.Empty;
        }

        protected Sensor(Input input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            In = input;
            _description = input.Symbol;
            _name = input.ToString() ?? String.Empty;
        }

        protected Sensor(IEvaluable input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            Name = input.ToString()??String.Empty;
            In = input;
            _name = input.ToString()??String.Empty;
        }

        protected Sensor(string description)
        {
            if (description == null)
                throw new ArgumentNullException("description");
            _description = description;
            In = Input.Simulated;
            _name = String.Empty;
        }

        protected IEvaluable In { get; private set; }

        public string Description
        {
            get { return _description; }
        }        

        public string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        public abstract bool Value();

        public override string ToString()
        {
            return _name;
        }

        #region Miembros de IManualsProvider

        public IEnumerable<Manual> GetManualsRepresentations()
        {
            var manual = new Manual(this);

            if (!String.IsNullOrEmpty(_description))
                manual.Description = _description;

            return new[] {manual};
        }

        #endregion

     
    }
}