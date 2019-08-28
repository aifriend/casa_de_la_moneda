using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class IOCollection : NameObjectCollectionBase
    {
        private IOSignal _iOSignal;
       
        public IOCollection(){}
        protected IOCollection(SerializationInfo info, StreamingContext context) : base(info, context){}
      
        public IOCollection(IDictionary d, bool bReadOnly)
        {
            foreach (DictionaryEntry de in d)
            {
                BaseAdd((string)de.Key, de.Value);
            }
            IsReadOnly = bReadOnly;
        }        
        public IOSignal this[int index]
        {
            get
            {
                _iOSignal = (IOSignal)BaseGet(index);                
                return _iOSignal;
            }
        }
        public IOSignal this[String key]
        {
            get { return (IOSignal)BaseGet(key); }
            set { BaseSet(key, value); }
        }
        public string[] AllKeys
        {
            get { return BaseGetAllKeys(); }
        }
        public Array AllValues
        {
            get { return BaseGetAllValues(); }
        } 
        public void Add(string key, IOSignal value)
        {
            BaseAdd(key, value);
        }
        public void Remove(string key)
        {
            BaseRemove(key);
        }
        public void Remove(int index)
        {
            BaseRemoveAt(index);
        }      
        public void Clear()
        {
            BaseClear();
        }
    }
}