using System;

namespace RCMCommonTypes
{
    [Serializable]
    public class GroupResume
    {
        public TipoPasaporte TipoPasaporte { get; private set; }        
        public string Id { get; private set; }  
        public int PassportsNumber { get; private set; }
        public double Weight { get; private set; }
        public bool LastOfBox { get; set; }
        
        public GroupResume() { }

        public GroupResume(TipoPasaporte tipo,string id
            , int passportsNumber, double weight, bool lastOfBox)
        {
            TipoPasaporte = tipo;
            Id = id;                               
            PassportsNumber = passportsNumber;
            Weight = weight;
            LastOfBox = lastOfBox;
        }
        
    }
}
