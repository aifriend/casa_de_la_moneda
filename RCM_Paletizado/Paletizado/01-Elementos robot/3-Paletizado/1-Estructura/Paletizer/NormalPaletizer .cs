using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;
using Idpsa.Control.Tool;
using Idpsa.Paletizado.Definitions;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    [Serializable]
    public class NormalPaletizer : PaletizerDecorator, ISolicitor, IManualsProvider
    {
        private readonly ElementTypes _itemType;
        private readonly Dictionary<ElementTypes, IEnumerable<Locations>> suppliers;
        private Func<ElementTypes?, bool> _requestElementAllowed;
        public bool LowPriority;

        public NormalPaletizer(string name, CornerPoint3D cornerPoint, Locations location, PaletizerSettings settings,
                               Func<string, CajaPasaportes> boxGetter,
                               Dictionary<ElementTypes, IEnumerable<Locations>> suppliers, IDLine idLine)
            : base(name, cornerPoint, location, settings, boxGetter)
        {
            this.suppliers = suppliers;
            _requestElementAllowed = s => true;
            _itemType = (idLine == IDLine.Japonesa) ? ElementTypes.ItemJaponesa : ElementTypes.ItemAlemana;
            LowPriority = false;
        }

        public NormalPaletizer WithRequestElementAllowed(Func<ElementTypes?, bool> requestElementAllowed)
        {
            _requestElementAllowed = requestElementAllowed;
            return this;
        }

        #region Miembros de ISolicitor

        public ElementTypes? ElementRequested()
        {
            ElementTypes? element;
            return _requestElementAllowed(element = ElementRequestedCore()) ? element : null;
        }

        public IEnumerable<Locations> GetSuppliersLocations()
        {
            ElementTypes? element;
            if ((element = ElementRequested()).HasValue)
            {
                return suppliers[element.Value];
            }
            return null;
        }

        public Priority Priority
        {
            get
            {
                if (LowPriority)
                {
                    return Priority.Lowest;
                    //mdg.2011-06-01.Poner palet siempre prioridad más alta
                }
                else if (State == Paletizado.Paletizer.States.PutPalet)
                    return Priority.Hight;
                else
                {
                    return Priority.Medium;
                }
            }
        }

        private ElementTypes? ElementRequestedCore()
        {
            ElementTypes? element = null;
            if (Paletizer != null)
            {
                switch (Paletizer.State)
                {
                    case Paletizado.Paletizer.States.PutItem:
                        element = _itemType;
                        break;
                    case Paletizado.Paletizer.States.PutSeparator:
                        element = ElementTypes.Separator;
                        break;
                    case Paletizado.Paletizer.States.PutPalet:
                        element = ElementTypes.Palet;
                        break;
                }
            }
            return element;
        }

        #endregion

        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            return (this).GetManualsRepresentations();
        }

        #endregion
    }
}