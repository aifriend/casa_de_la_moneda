using System;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    public class LogicalActivable : LogicalEvaluable,IActivable ,IManualsProvider
    {
        private Action<bool> _activate;

        public LogicalActivable(Action<bool> activate,Func<bool> value, Func<string> toStringOverload):
            this(activate,value,toStringOverload,String.Empty){}

        public LogicalActivable(Action<bool> activate, Func<bool> value, string description) :
            this(activate,value, ()=>String.Empty, description) { }

        public LogicalActivable(Action<bool> activate, Func<bool> value, Func<string> toStringOverload, string description):
            base(value,toStringOverload,description)
        {
            if (activate == null)            
                throw new ArgumentNullException("activate"); 
            
            _activate = activate; 
        }

        #region Miembros de IActivable

        public void Activate(bool work)
        {
            _activate(work);
        }

        #endregion
    }
       
}
