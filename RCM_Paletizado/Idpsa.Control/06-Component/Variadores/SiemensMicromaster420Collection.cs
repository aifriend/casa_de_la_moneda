using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class SiemensMicromaster420Collection : NameObjectCollectionBase
    {
        // Creates an empty collection.
        public SiemensMicromaster420Collection()
        {
        }

        protected SiemensMicromaster420Collection(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        //New
        // Adds elements from an IDictionary into the new collection.
        public SiemensMicromaster420Collection(IDictionary d, bool bReadOnly)
        {
            foreach (DictionaryEntry de in d)
            {
                BaseAdd((string)de.Key, de.Value);
            }
            IsReadOnly = bReadOnly;
        }

        //New
        // Gets a key-and-value pair (DictionaryEntry) using an index.
        public SiemensMicromaster420 this[int index]
        {
            //DictionaryEntry
            get { return (SiemensMicromaster420)(BaseGet(index)); }
        }

        // Gets or sets the value associated with the specified key.
        public SiemensMicromaster420 this[string key]
        {
            get
            {
                try
                {
                    return (SiemensMicromaster420)(BaseGet(key));
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {
                try
                {
                    BaseSet(key, value);
                }
                catch (Exception)
                {
                }
            }
        }

        // Gets a String array that contains all the keys in the collection.
        public string[] AllKeys
        {
            get { return BaseGetAllKeys(); }
        }

        // Gets an Object array that contains all the values in the collection.
        public Array AllValues
        {
            get { return BaseGetAllValues(); }
        }

        // Gets a String array that contains all the values in the collection.
        public string[] AllStringValues
        {
            get { return (string[])BaseGetAllValues(Type.GetType("System.String")); }
        }

        // Gets a value indicating if the collection contains keys that are not null.
        public bool HasKeys
        {
            get { return BaseHasKeys(); }
        }

        // Adds an entry to the collection.
        //Public Sub Add(ByVal key As [String], ByVal value As [Object])
        public void Add(string key, SiemensMicromaster420 value)
        {
            BaseAdd(key, value);
        }

        //Add
        // Removes an entry with the specified key from the collection.
        public void Remove(string key)
        {
            BaseRemove(key);
        }

        //Remove
        // Removes an entry in the specified index from the collection.
        public void Remove(int index)
        {
            BaseRemoveAt(index);
        }

        //Remove
        // Clears all the elements in the collection.
        public void Clear()
        {
            BaseClear();
        }

        //Clear
    }
}