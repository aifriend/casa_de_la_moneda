using System;
using System.Collections.Generic;

namespace Idpsa.Control.Diagnosis
{
    [Serializable]
    public class DiagnosisCollection
    {
        #region Delegates

        public delegate void CountChangedHandler();

        #endregion

        private readonly Dictionary<string, GeneralDiagnosis> _diagnosis;

        public DiagnosisCollection()
        {
            _diagnosis = new Dictionary<string, GeneralDiagnosis>();
        }

        public DiagnosisCollection(IEnumerable<KeyValuePair<string, GeneralDiagnosis>> d)
        {
            foreach (var de in d)
            {
                _diagnosis.Add(de.Key, de.Value);
            }
        }


        public GeneralDiagnosis this[string key]
        {
            get { return _diagnosis[key]; }
            set { _diagnosis[key] = value; }
        }


        public Dictionary<string, GeneralDiagnosis>.KeyCollection AllKeys
        {
            get { return _diagnosis.Keys; }
        }


        public Dictionary<string, GeneralDiagnosis>.ValueCollection AllValues
        {
            get { return _diagnosis.Values; }
        }

        public int Count
        {
            get { return _diagnosis.Count; }
        }

        public event CountChangedHandler CountChanged;

        private void OnCountChanged()
        {
            if (CountChanged != null)
            {
                CountChanged();
            }
        }

        public bool ContainsKey(string key)
        {
            return _diagnosis.ContainsKey(key);
        }

        public void Add(string key, GeneralDiagnosis value)
        {
            if (!_diagnosis.ContainsKey(key))
            {
                _diagnosis.Add(key, value);
                OnCountChanged();
            }
        }

        public void Remove(string key)
        {
            _diagnosis.Remove(key);
            OnCountChanged();
        }

        public void Clear()
        {
            _diagnosis.Clear();
            OnCountChanged();
        }
    }
}