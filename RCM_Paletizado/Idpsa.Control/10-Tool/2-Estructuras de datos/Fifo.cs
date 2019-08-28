using System;

namespace Idpsa.Control.Tool
{
    [Serializable]
    public class Fifo
    {
        private readonly int[] _query;
        private int _indexQ;

        public Fifo(int NoElemetos)
        {
            _query = new int[NoElemetos];
            for (int i = 0; i <= _query.GetUpperBound(0); i++)
            {
                _query[i] = -1;
            }
            _indexQ = 0;
        }

        public void Query(int id)
        {
            int ind = _indexQ;
            if (InQuery(id) == false)
            {
                for (int i = 0; i <= _query.GetUpperBound(0); i++)
                {
                    if (_query[ind] == -1)
                    {
                        _query[ind] = id;
                        break;
                    }
                    ind = (_indexQ + 1) % _query.Length;
                }
            }
        }

        public bool InQuery(int id)
        {
            bool encontrado = false;
            for (int i = 0; i <= _query.GetUpperBound(0); i++)
            {
                if (_query[i] == id)
                {
                    encontrado = true;
                    break;
                }
            }
            return encontrado;
        }

        private void UpdatePriorIndexQuery()
        {
            for (int i = 0; i <= _query.GetUpperBound(0); i++)
            {
                if ((_query[_indexQ] == -1))
                {
                    _indexQ = (_indexQ + 1) % _query.Length;
                    break;
                }
            }
        }

        public int PriorQueryId()
        {
            UpdatePriorIndexQuery();
            return _query[_indexQ];
        }

        public void DelPriorQuery()
        {
            UpdatePriorIndexQuery();
            _query[_indexQ] = -1;
            UpdatePriorIndexQuery();
        }

        public void DelQuery(int id)
        {
            //For i As Integer = 0 To _query.GetUpperBound(0)
            //If (_query(i) = id) Then
            //_query(i) = -1
            //End If
            //Next
            //UpdatePriorIndexQuery()
        }

        public void DelQueryEsp(int id)
        {
            for (int i = 0; i <= _query.GetUpperBound(0); i++)
            {
                if ((_query[i] == id))
                {
                    _query[i] = -1;
                }
            }
            UpdatePriorIndexQuery();
        }
    }
}