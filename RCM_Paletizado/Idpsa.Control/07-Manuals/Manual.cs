using System;

namespace Idpsa.Control.Manuals
{
    [Serializable]
    public class Manual
    {
        public Manual(object representedInstance)
        {
            if (representedInstance == null)
                throw new ArgumentNullException("representedInstance"); 
            RepresentedInstance = representedInstance;
            Description = Group = SuperGroup = String.Empty;
        }

        public object RepresentedInstance { get; private set; }
        public string Description { set; get; }
        public string Group { set; get; }
        public string SuperGroup { set; get; }
    }
}