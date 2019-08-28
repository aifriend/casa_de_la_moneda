using System;
using System.Collections.Generic;

namespace Idpsa.Control.Diagnosis
{
    [Serializable]
    public class AlarmsHistorical
    {
        private const int _MaxSize = 25;
        private bool _sortedAscByDescription;
        private bool _sortedAscByDate;
        private bool _sortedAscByFrecuency;

        public List<Element> Alarms { get; private set; }

        public AlarmsHistorical()
        {
            Alarms = new List<Element>();
        }
             
        public object MaxSize
        {
            get { return _MaxSize; }
        }

        public Element this[int index]
        {
            get { return Alarms[index]; }
        }

        public bool Exist(Element alarm)
        {
            bool exist = false;
            string text = alarm.Description;
            foreach (Element value in Alarms)
            {
                if (value.Description == text)
                {
                    exist = true;
                }
            }
            return exist;
        }

        public int Index(Element alarm)
        {
            int value = -1;
            string text = alarm.Description;
            for (int i = 0; i <= Alarms.Count - 1; i++)
            {
                if (Alarms[i].Description == text)
                {
                    value = i;
                }
            }
            return value;
        }

        public void Add(Element alarm)
        {
            int index = Index(alarm);
            if (index != -1)
            {
                Alarms[index].Date = alarm.Date;
                Alarms[index].Repetitions += 1;
            }
            else
            {
                if (Alarms.Count + 1 >= _MaxSize)
                {
                    if (!_sortedAscByDate)
                    {
                        AscendingByDate();
                        Alarms.RemoveAt(0);
                    }
                }
                Alarms.Add(alarm);
            }
        }

        public void Add(string descripcion)
        {
            var elemento = new Element(descripcion);
            Add(elemento);
        }

        public decimal[] Frecuencies()
        {
            var fs = new decimal[Alarms.Count];
            int total = 0;
            foreach (Element element in Alarms)
            {
                total += element.Repetitions;
            }
            for (int i = 0; i <= Alarms.Count - 1; i++)
            {
                fs[i] = (Alarms[i].Repetitions * 100) / total;
            }
            return fs;
        }

        public void Remove(int index)
        {
            Alarms.RemoveAt(index);
        }

        public void Remove(string descripcion)
        {
            foreach (Element a in Alarms)
            {
                if (a.Description == descripcion)
                {
                    Alarms.Remove(a);
                    break;
                }
            }
        }

        public int Count()
        {
            return Alarms.Count;
        }

        public void AscendingByFrequency()
        {
            Alarms.Sort(new SortAscendingByFrecuency());
            _sortedAscByFrecuency = true;
        }

        public void DescendingByFrequency()
        {
            Alarms.Sort(new SortDescendingByFrecuency());
            _sortedAscByFrecuency = false;
        }

        public void AscendingByDescription()
        {
            Alarms.Sort(new SortAscendingByDescription());
            _sortedAscByDescription = true;
        }

        public void DescendingByDescription()
        {
            Alarms.Sort(new SortDescendingByDescription());
            _sortedAscByDescription = false;
        }

        public void AscendingByDate()
        {
            Alarms.Sort(new SortAscendingByDate());
            _sortedAscByDate = true;
        }

        public void DescendingByDate()
        {
            Alarms.Sort(new SortDescendingByDate());
            _sortedAscByDate = false;
        }

        public void SortByFrecuency()
        {
            if (_sortedAscByFrecuency)
            {
                DescendingByFrequency();
            }
            else
            {
                AscendingByFrequency();
            }
        }

        public void SortByDate()
        {
            if (_sortedAscByDate)
            {
                DescendingByDate();
            }
            else
            {
                AscendingByDate();
            }
        }

        public void SortByDescription()
        {
            if (_sortedAscByDescription)
            {
                DescendingByDescription();
            }
            else
            {
                AscendingByDescription();
            }
        }

        #region Nested type: AlarmsHistoricalElement

        [Serializable]
        public class Element
        {
            public Element(Element element)
            {
                Date = element.Date;
                Description = element.Description;
                Repetitions = element.Repetitions;
            }

            public Element(DateTime date, string description, int repetitions)
            {
                Date = date;
                Description = description;
                Repetitions = repetitions;
            }

            public Element(string description, int repetitions)
                : this(DateTime.Now, description, repetitions)
            {
            }

            public Element(string description) : this(DateTime.Now, description, 1)
            {
            }

            public DateTime Date { get; set; }
            public string Description { get; set; }
            public int Repetitions { get; set; }
        }

        #endregion

        #region Nested type: SortAscendingByDescripcion

        private class SortAscendingByDescription : IComparer<Element>
        {
            #region Miembros de IComparer<ElementoHistoricoAlarmas>

            public int Compare(Element e1, Element e2)
            {
                int value;
                if (e1.Description.CompareTo(e2.Description) > 0)
                {
                    value = 1;
                }
                else if (e1.Description.CompareTo(e2.Description) < 0)
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

        #region Nested type: SortAscendingByDate

        private class SortAscendingByDate : IComparer<Element>
        {
            #region Miembros de IComparer<ElementoHistoricoAlarmas>

            int IComparer<Element>.Compare(Element e1, Element e2)
            {
                int value;
                if (e1.Date > e2.Date)
                {
                    value = 1;
                }
                else if (e1.Date < e2.Date)
                {
                    value = -1;
                }
                else
                {
                    value = new SortAscendingByDescription().Compare(e1, e2);
                }
                return value;
            }

            #endregion
        }

        #endregion

        #region Nested type: SortAscendingByFrecuency

        private class SortAscendingByFrecuency : IComparer<Element>
        {
            #region Miembros de IComparer<ElementoHistoricoAlarmas>

            int IComparer<Element>.Compare(Element e1, Element e2)
            {
                int value;
                if (e1.Repetitions > e2.Repetitions)
                {
                    value = 1;
                }
                else if (e1.Repetitions < e2.Repetitions)
                {
                    value = -1;
                }
                else
                {
                    value = new SortAscendingByDescription().Compare(e1, e2);
                }
                return value;
            }

            #endregion
        }

        #endregion

        #region Nested type: SortDescendingByDescription

        private class SortDescendingByDescription : IComparer<Element>
        {
            #region Miembros de IComparer<ElementoHistoricoAlarmas>

            int IComparer<Element>.Compare(Element e1, Element e2)
            {
                int value;
                if (e1.Description.CompareTo(e2.Description) > 0)
                {
                    value = -1;
                }
                else value = e1.Description.CompareTo(e2.Description) < 0 ? 1 : 0;
                return value;
            }

            #endregion
        }

        #endregion

        #region Nested type: SortDescendingByDate

        private class SortDescendingByDate : IComparer<Element>
        {
            #region Miembros de IComparer<ElementoHistoricoAlarmas>

            int IComparer<Element>.Compare(Element e1, Element e2)
            {
                int value;
                if (e1.Date > e2.Date)
                {
                    value = -1;
                }
                else value = e1.Date < e2.Date ? 1 : new SortAscendingByDescription().Compare(e1, e2);
                return value;
            }

            #endregion
        }

        #endregion

        #region Nested type: SortDescendingByFrecuency

        private class SortDescendingByFrecuency : IComparer<Element>
        {
            #region Miembros de IComparer<ElementoHistoricoAlarmas>

            int IComparer<Element>.Compare(Element e1, Element e2)
            {
                int value;
                if (e1.Repetitions > e2.Repetitions)
                {
                    value = -1;
                }
                else value = e1.Repetitions < e2.Repetitions ? 1 : new SortAscendingByDescription().Compare(e1, e2);
                return value;
            }

            #endregion
        }

        #endregion
    }
}