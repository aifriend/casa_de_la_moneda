using System;
using System.Collections.Generic;

namespace Idpsa.Control.Diagnosis
{
    [Serializable]
    public class SecuritySignalCollection
    {
        private readonly Dictionary<string, SecurityDiagnosis> _securidades;
       
        public SecuritySignalCollection()
        {
            _securidades = new Dictionary<string, SecurityDiagnosis>();
        }
        
        public SecuritySignalCollection(IEnumerable<KeyValuePair<string, SecurityDiagnosis>> d, bool _bReadOnly)
        {
            foreach (var de in d)
            {
                _securidades.Add(de.Key, de.Value);
            }
        }
       
        public SecurityDiagnosis this[string key]
        {
            get { return _securidades[key]; }
            set { _securidades[key] = value; }
        }
       
        public Dictionary<string, SecurityDiagnosis>.KeyCollection AllKeys
        {
            get { return _securidades.Keys; }
        }
                
        public Dictionary<string, SecurityDiagnosis>.ValueCollection AllValues
        {
            get { return _securidades.Values; }
        }
               
        public void Add(SecurityDiagnosis value)
        {
            _securidades.Add(value.Name, value);
        }

        public void AddRange(IEnumerable<SecurityDiagnosis> values)
        {
            foreach (SecurityDiagnosis value in values)
            {
                _securidades.Add(value.Name, value);
            }
        }
        
        public void Remove(string key)
        {
            _securidades.Remove(key);
        }

        public void Clear()
        {
            _securidades.Clear();
        }
  
        public List<SecurityDiagnosis> Actuated()
        {
            var actuated = new List<SecurityDiagnosis>();
            foreach (SecurityDiagnosis s in AllValues)
            {
                if (s.Activated())
                {
                    actuated.Add(s);
                }
            }
            return actuated;
        }

        public List<SecurityDiagnosis> NotActuated()
        {
            var notActuated = new List<SecurityDiagnosis>();

            foreach (SecurityDiagnosis s in AllValues)
            {
                if (!s.Activated())
                {
                    notActuated.Add(s);
                }
            }
            return notActuated;
        }
    }
}