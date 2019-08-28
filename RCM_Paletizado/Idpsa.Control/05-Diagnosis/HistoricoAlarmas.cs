using System;
using System.Collections.Generic;

namespace Idpsa.Control.Diagnosis
{
    [Serializable]
    public class HistoricoAlarmas
    {
        private const int _MaxSize = 25;
        private bool _ordenadoAscPorDescripcion;
        private bool _ordenadoAscPorFecha;
        private bool _ordenadoAscPorFrecuencia;

        public HistoricoAlarmas()
        {
            Alarmas = new List<ElementoHistoricoAlarmas>();
        }

        public HistoricoAlarmas(IEnumerable<ElementoHistoricoAlarmas> parElementos)
        {
            Alarmas = new List<ElementoHistoricoAlarmas>(parElementos);
        }

        public HistoricoAlarmas(IEnumerable<string> parDescripciones)
        {
            Alarmas = new List<ElementoHistoricoAlarmas>();
            foreach (string text in parDescripciones)
            {
                Add(text);
            }
        }

        public List<ElementoHistoricoAlarmas> Alarmas { get; private set; }

        public object MaxSize
        {
            get { return _MaxSize; }
        }

        public ElementoHistoricoAlarmas this[int index]
        {
            get { return Alarmas[index]; }
        }

        public bool Existe(ElementoHistoricoAlarmas alarma)
        {
            bool value = false;
            string text = alarma.Descripcion;
            foreach (ElementoHistoricoAlarmas alarm in Alarmas)
            {
                if (alarm.Descripcion == text)
                {
                    value = true;
                }
            }
            return value;
        }

        public int Index(ElementoHistoricoAlarmas alarma)
        {
            int value = -1;
            string text = alarma.Descripcion;
            for (int i = 0; i <= Alarmas.Count - 1; i++)
            {
                if (Alarmas[i].Descripcion == text)
                {
                    value = i;
                }
            }
            return value;
        }

        public void Add(ElementoHistoricoAlarmas alarma)
        {
            int indice = Index(alarma);
            if (indice != -1)
            {
                Alarmas[indice].Fecha = alarma.Fecha;
                Alarmas[indice].Repeticiones += 1;
            }
            else
            {
                if (Alarmas.Count + 1 >= _MaxSize)
                {
                    if (!_ordenadoAscPorFecha)
                    {
                        AscendentePorFecha();
                        Alarmas.RemoveAt(0);
                    }
                }
                Alarmas.Add(alarma);
            }
        }

        public void Add(string descripcion)
        {
            var elemento = new ElementoHistoricoAlarmas(descripcion);
            Add(elemento);
        }

        public decimal[] Frecuencias()
        {
            var fs = new decimal[Alarmas.Count];
            int total = 0;
            foreach (ElementoHistoricoAlarmas elemento in Alarmas)
            {
                total += elemento.Repeticiones;
            }


            for (int i = 0; i <= Alarmas.Count - 1; i++)
            {
                fs[i] = (Alarmas[i].Repeticiones * 100) / total;
            }
            return fs;
        }

        public void Remove(int index)
        {
            Alarmas.RemoveAt(index);
        }

        public void Remove(string descripcion)
        {
            foreach (ElementoHistoricoAlarmas a in Alarmas)
            {
                if (a.Descripcion == descripcion)
                {
                    Alarmas.Remove(a);
                    break;
                }
            }
        }

        public int Count()
        {
            return Alarmas.Count;
        }

        public void AscendentePorFrecuencia()
        {
            Alarmas.Sort(new SortAscendentePorFrecuencia());
            _ordenadoAscPorFrecuencia = true;
        }

        public void DescendentePorFrecuencia()
        {
            Alarmas.Sort(new SortDescendentePorFrecuencia());
            _ordenadoAscPorFrecuencia = false;
        }

        public void AscendentePorDescripcion()
        {
            Alarmas.Sort(new SortAscendentePorDescripcion());
            _ordenadoAscPorDescripcion = true;
        }

        public void DescendentePorDescripcion()
        {
            Alarmas.Sort(new SortDescendentePorDescripcion());
            _ordenadoAscPorDescripcion = false;
        }

        public void AscendentePorFecha()
        {
            Alarmas.Sort(new SortAscendentePorFecha());
            _ordenadoAscPorFecha = true;
        }

        public void DescendentePorFecha()
        {
            Alarmas.Sort(new SortDescendentePorFecha());
            _ordenadoAscPorFecha = false;
        }

        public void SortPorFrecuencia()
        {
            if (_ordenadoAscPorFrecuencia)
            {
                DescendentePorFrecuencia();
            }
            else
            {
                AscendentePorFrecuencia();
            }
        }

        public void SortPorFecha()
        {
            if (_ordenadoAscPorFecha)
            {
                DescendentePorFecha();
            }
            else
            {
                AscendentePorFecha();
            }
        }

        public void SortPorDescripcion()
        {
            if (_ordenadoAscPorDescripcion)
            {
                DescendentePorDescripcion();
            }
            else
            {
                AscendentePorDescripcion();
            }
        }

        #region Nested type: ElementoHistoricoAlarmas

        [Serializable]
        public class ElementoHistoricoAlarmas
        {
            public ElementoHistoricoAlarmas(ElementoHistoricoAlarmas elemento)
            {
                Fecha = elemento.Fecha;
                Descripcion = elemento.Descripcion;
                Repeticiones = elemento.Repeticiones;
            }

            public ElementoHistoricoAlarmas(DateTime fecha, string descripcion, int repeticiones)
            {
                Fecha = fecha;
                Descripcion = descripcion;
                Repeticiones = repeticiones;
            }

            public ElementoHistoricoAlarmas(string descripcion, int repeticiones)
                : this(DateTime.Now, descripcion, repeticiones)
            {
            }

            public ElementoHistoricoAlarmas(string descripcion) : this(DateTime.Now, descripcion, 1)
            {
            }

            public DateTime Fecha { get; set; }
            public string Descripcion { get; set; }
            public int Repeticiones { get; set; }
        }

        #endregion

        #region Nested type: SortAscendentePorDescripcion

        private class SortAscendentePorDescripcion : IComparer<ElementoHistoricoAlarmas>
        {
            #region Miembros de IComparer<ElementoHistoricoAlarmas>

            public int Compare(ElementoHistoricoAlarmas e1, ElementoHistoricoAlarmas e2)
            {
                int value;
                if (e1.Descripcion.CompareTo(e2.Descripcion) > 0)
                {
                    value = 1;
                }
                else if (e1.Descripcion.CompareTo(e2.Descripcion) < 0)
                {
                    value = -1;
                }
                else
                {
                    value = 0;
                }
                return value;
            }

            #endregion
        }

        #endregion

        #region Nested type: SortAscendentePorFecha

        private class SortAscendentePorFecha : IComparer<ElementoHistoricoAlarmas>
        {
            #region Miembros de IComparer<ElementoHistoricoAlarmas>

            int IComparer<ElementoHistoricoAlarmas>.Compare(ElementoHistoricoAlarmas e1, ElementoHistoricoAlarmas e2)
            {
                int value;
                if (e1.Fecha > e2.Fecha)
                {
                    value = 1;
                }
                else if (e1.Fecha < e2.Fecha)
                {
                    value = -1;
                }
                else
                {
                    value = new SortAscendentePorDescripcion().Compare(e1, e2);
                }
                return value;
            }

            #endregion
        }

        #endregion

        #region Nested type: SortAscendentePorFrecuencia

        private class SortAscendentePorFrecuencia : IComparer<ElementoHistoricoAlarmas>
        {
            #region Miembros de IComparer<ElementoHistoricoAlarmas>

            int IComparer<ElementoHistoricoAlarmas>.Compare(ElementoHistoricoAlarmas e1, ElementoHistoricoAlarmas e2)
            {
                int value;
                if (e1.Repeticiones > e2.Repeticiones)
                {
                    value = 1;
                }
                else if (e1.Repeticiones < e2.Repeticiones)
                {
                    value = -1;
                }
                else
                {
                    value = new SortAscendentePorDescripcion().Compare(e1, e2);
                }
                return value;
            }

            #endregion
        }

        #endregion

        #region Nested type: SortDescendentePorDescripcion

        private class SortDescendentePorDescripcion : IComparer<ElementoHistoricoAlarmas>
        {
            #region Miembros de IComparer<ElementoHistoricoAlarmas>

            int IComparer<ElementoHistoricoAlarmas>.Compare(ElementoHistoricoAlarmas e1, ElementoHistoricoAlarmas e2)
            {
                int value;
                if (e1.Descripcion.CompareTo(e2.Descripcion) > 0)
                {
                    value = -1;
                }
                else value = e1.Descripcion.CompareTo(e2.Descripcion) < 0 ? 1 : 0;
                return value;
            }

            #endregion
        }

        #endregion

        #region Nested type: SortDescendentePorFecha

        private class SortDescendentePorFecha : IComparer<ElementoHistoricoAlarmas>
        {
            #region Miembros de IComparer<ElementoHistoricoAlarmas>

            int IComparer<ElementoHistoricoAlarmas>.Compare(ElementoHistoricoAlarmas e1, ElementoHistoricoAlarmas e2)
            {
                int value;
                if (e1.Fecha > e2.Fecha)
                {
                    value = -1;
                }
                else value = e1.Fecha < e2.Fecha ? 1 : new SortAscendentePorDescripcion().Compare(e1, e2);
                return value;
            }

            #endregion
        }

        #endregion

        #region Nested type: SortDescendentePorFrecuencia

        private class SortDescendentePorFrecuencia : IComparer<ElementoHistoricoAlarmas>
        {
            #region Miembros de IComparer<ElementoHistoricoAlarmas>

            int IComparer<ElementoHistoricoAlarmas>.Compare(ElementoHistoricoAlarmas e1, ElementoHistoricoAlarmas e2)
            {
                int value;
                if (e1.Repeticiones > e2.Repeticiones)
                {
                    value = -1;
                }
                else value = e1.Repeticiones < e2.Repeticiones ? 1 : new SortAscendentePorDescripcion().Compare(e1, e2);
                return value;
            }

            #endregion
        }

        #endregion
    }
}