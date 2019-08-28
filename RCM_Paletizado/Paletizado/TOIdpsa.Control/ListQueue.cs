using System;
using System.Collections;
using System.Collections.Generic;
using Idpsa.Control;

namespace IdpsaControl.Tool
{
    public class FifoList<T> : IEnumerable<T>
    {
        private List<T> _list;

        public FifoList()
        {
            _list = new List<T>();
        }

        public bool Empty
        {
            get { return _list.Count == 0; }
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public List<T> List
        {
            get { return _list; }
            set { _list = value; }
        }

        public event EventHandler<DataEventArgs<T>> ElementPutted;
        public event EventHandler<DataEventArgs<T>> ElementQuitted;
        public event EventHandler<EventArgs> ElementsCleared;

        public FifoList<T> Put(T element)
        {
            _list.Insert(0, element);
            OnPutted(element);
            return this;
        }

        public T Quit()
        {
            if (_list.Count == 0)
            {
                throw new InvalidOperationException("Element can't bee quited on an empty FifoList<T>");
            }

            T value = _list[_list.Count - 1];
            _list.RemoveAt(_list.Count - 1);
            OnQuitted(value);
            return value;
        }

        public T PeekFirst()
        {
            if (_list.Count == 0)
            {
                throw new InvalidOperationException("An element can't bee peeked on an empty FifoList<T>");
            }
            return _list[0];
        }

        public T PeekLast()
        {
            if (_list.Count == 0)
            {
                throw new InvalidOperationException("An Element can't bee peeked on an empty FifoList<T>");
            }
            return _list[_list.Count - 1];
        }

        public bool TryPeekFirst(out T firstElement)
        {
            firstElement = default(T);
            if (_list.Count == 0)
                return false;
            firstElement = _list[0];
            return true;
        }

        public bool TryPeekLast(out T lastElement)
        {
            lastElement = default(T);
            if (_list.Count == 0)
                return false;
            lastElement = _list[_list.Count - 1];
            return true;
        }

        public void Clear()
        {
            _list.Clear();
            OnCleared();
        }

        protected virtual void OnPutted(T element)
        {
            EventHandler<DataEventArgs<T>> temp = ElementPutted;
            if (temp != null)
                temp(this, new DataEventArgs<T>(element));
        }

        protected virtual void OnQuitted(T element)
        {
            EventHandler<DataEventArgs<T>> temp = ElementQuitted;
            if (temp != null)
                temp(this, new DataEventArgs<T>(element));
        }

        protected virtual void OnCleared()
        {
            EventHandler<EventArgs> temp = ElementsCleared;
            if (temp != null)
                temp(this, EventArgs.Empty);
        }

        #region Miembros de IEnumerable<T>

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        #region Miembros de IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        //MDG.2010-12-02. Nuevo metodo para indroducir y leer la lista interna
    }
}