using System;

namespace Idpsa.Control.Subsystem
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SubsystemAttribute : Attribute
    {               
        public SubsystemFilter Filter { get; set; }   
        public SubsystemAttribute()
        {
            Filter = SubsystemFilter.None;           
        }
    }
}