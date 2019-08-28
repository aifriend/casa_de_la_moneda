using System;
using System.Collections.Generic;
using System.Linq;

namespace Idpsa.Paletizado
{
    [Serializable]
    public class PaletProduction : IDBMappeable
    {
        private static int PaletNumber;
        private readonly Dictionary<ElementIndex, IPaletElement> elements;
        private readonly Dictionary<ElementIndex, positionModified> modifiedPositions;

        public PaletProduction(PaletizerDefinition paletizerDefinition, bool newIdPalet)
        {
            elements = new Dictionary<ElementIndex, IPaletElement>();
            modifiedPositions = new Dictionary<ElementIndex, positionModified>();
            AssingProduction(paletizerDefinition, newIdPalet);
        }

        public string IDPalet { get; private set; }
        public PaletizerDefinition PaletizerDefinition { get; private set; }

        public int Count
        {
            get { return elements.Count; }
        }

        #region IDBMappeable Members

        public DBEntity GetDBMapper()
        {
            throw new NotImplementedException();
        }

        public DBAction DBAction
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion

        public bool Attach(int pos, int flat, IPaletElement element)
        {
            var index = new ElementIndex { Pos = pos, Flat = flat };
            if (elements.ContainsKey(index))
            {
                return false; //MCR2014
            }
            SetPaletElementInformation(flat, pos, element);
            elements.Add(new ElementIndex { Pos = pos, Flat = flat },
                         element);
            return true;
        }

        public IPaletElement Dettach(int pos, int flat)
        {
            IPaletElement element = null;
            var index = new ElementIndex {Pos = pos, Flat = flat};
            if (elements.ContainsKey(index))
            {
                element = elements[index];
                elements.Remove(index);
            }

            return element;
        }

        public void Modificate(int pos, int flat, IPaletElement element, int newpos,int newflat)
        {
            positionModified modif = new positionModified {modified = false};
            var index = new ElementIndex { Pos = pos, Flat = flat };
            var newIndex = new ElementIndex { Pos = newpos, Flat = newflat };
            if (!modifiedPositions.ContainsKey(index))
            {
                if (elements.ContainsKey(index))
                {
                    elements.Remove(index);
                }
            }
            if (elements.ContainsKey(newIndex))
            {
                elements.Remove(newIndex);
            }

            SetPaletElementInformation(newflat, newpos, element);
            modif.modified = true;
            elements.Add(new ElementIndex { Pos = newpos, Flat = newflat },
                         element);
            modifiedPositions.Add(newIndex,modif);
        }

        public void ResetModifiedPositions()
    {
            modifiedPositions.Clear();
    }

        public IPaletElement GetLastItem()
        {
            if (elements.Count > 0)
                return elements.ElementAt(elements.Count - 1).Value;
            else
                return null;
        }

        public IPaletElement PeekElement(int pos, int flat)
        {
            IPaletElement element = null;
            var index = new ElementIndex {Pos = pos, Flat = flat};
            if (elements.ContainsKey(index))
            {
                element = elements[index];
            }
            return element;
        }

        public void AssingProduction(PaletizerDefinition paletizerDefinition, bool newIdPalet)
        {
            if (newIdPalet)
            {
                PaletNumber++;
            }
            IDPalet = PaletNumber.ToString();
            PaletizerDefinition = paletizerDefinition;
            elements.Clear();
        }

        public IPaletElement GetElementAt(int pos, int flat)
        {
            IPaletElement element = null;
            var index = new ElementIndex {Pos = pos, Flat = flat};
            if (elements.ContainsKey(index))
            {
                element = elements[index];
            }
            return element;
        }

        private void SetPaletElementInformation(int flat, int pos, IPaletElement element)
        {
            element.IDPalet = IDPalet;
            element.Flat = flat;
            element.Pos = pos;
        }

        public IEnumerable<CajaPasaportes> GetBoxes()
        {
            return elements.Values.OfType<CajaPasaportes>();
        }
        public void ResetElementes()
        {
            elements.Clear();
        }


        #region Nested type: ElementIndex

        [Serializable]
        private struct ElementIndex
        {
            public int Flat;
            public int Pos;
        }

        private struct positionModified
        {
            public bool modified;
        }

        #endregion
    }
}