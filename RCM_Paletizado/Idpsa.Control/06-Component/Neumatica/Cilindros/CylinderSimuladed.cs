using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class CylinderSimuladed : Cylinder
    {
        public CylinderSimuladed(string name) 
            : base(name){}

        public CylinderSimuladed() : this(String.Empty){}

        public override bool InRest
        {
            get { return true; }
        }
        public override bool InWork
        {
            get { return true; }
        }

        public override void Rest(){}
        public override void Work(){}
        public override void Dead(){}

        protected override IEnumerable<Manual> GetPartialManualRepresentations()
        {
            return new[] {new Manual(new GeneralManual())};
        }
    }
}