using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    public class LogicalEvaluable : IEvaluable, IManualsProvider
    {
        private string _description;
        private Func<string> _toString;
        private Func<bool> _value;

        public LogicalEvaluable(Func<bool> value, Func<string> toStringOverload) :
            this(value, toStringOverload, String.Empty)
        {
        }

        public LogicalEvaluable(Func<bool> value, string description) :
            this(value, () => String.Empty, description)
        {
        }

        public LogicalEvaluable(Func<bool> value, Func<string> toStringOverload, string description)
        {
            if (value == null)
                throw new NullReferenceException("value");
            if (toStringOverload == null)
                throw new NullReferenceException("toStringOverload");

            _value = value;
            _toString = toStringOverload;
            _description = description ?? String.Empty;
        }

        public string Description
        {
            get { return _description; }
        }

        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            var manual = new Manual(this);

            if (!String.IsNullOrEmpty(_description))
                manual.Description = _description;

            return new[] {manual};
        }

        #endregion

        #region IEvaluable Members

        public bool Value()
        {
            return _value();
        }

        #endregion

        public override string ToString()
        {
            return _toString();
        }
    }
}