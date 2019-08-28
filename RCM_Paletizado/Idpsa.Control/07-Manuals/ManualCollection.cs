using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Idpsa.Control.Component;
using Idpsa.Control.Tool;

namespace Idpsa.Control.Manuals
{
    public class ManualCollection : IEnumerable<Manual>
    {      
        private readonly Dictionary<string, Manual> _manuales = new Dictionary<string, Manual>();

        public ManualCollection(){}

     
        public Dictionary<string, Manual>.ValueCollection Values
        {
            get { return _manuales.Values; }
        }

        private void Add(string key, Manual manual)
        {
            if (!_manuales.ContainsKey(key))
            {
                _manuales.Add(key, manual);
            }
        }

        public void Add(Manual manual)
        {
            Add(manual.SuperGroup + manual.Group + manual.Description, manual);
        }

        public void AddRange(IEnumerable<Manual> manuals)
        {
            foreach (Manual manual in manuals)
                Add(manual);
        }

        public void Clear()
        {
            _manuales.Clear();
        }

        
        #region Miembros de IEnumerable<Manual>

        public IEnumerator<Manual> GetEnumerator()
        {
            return _manuales.Values.GetEnumerator();
        }

        #endregion

        #region Miembros de IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _manuales.Values.GetEnumerator();
        }

        #endregion
    }
}