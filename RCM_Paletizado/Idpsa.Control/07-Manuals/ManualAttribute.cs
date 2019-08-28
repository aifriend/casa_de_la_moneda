using System;

namespace Idpsa.Control.Manuals
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ManualAttribute : Attribute
    {
        public ManualAttribute(){}
        public string SuperGroup { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
    }
}