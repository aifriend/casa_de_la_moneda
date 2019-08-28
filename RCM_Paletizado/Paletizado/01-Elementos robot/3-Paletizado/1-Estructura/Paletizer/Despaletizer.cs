using System;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Manuals;
using Idpsa.Control.Tool;
using Idpsa.Paletizado.Definitions;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    [Serializable]
    public class Despaletizer : PaletizerDecorator, ISolicitor, ISupplier, IManualsProvider
    {
        //Point3D origen,
        //    PaletizerDefinition data, PaletizerSettings settings

        private readonly ElementTypes _itemType;
        private readonly Dictionary<ElementTypes, IEnumerable<Locations>> _suppliers;
        private Func<ElementTypes?, bool> _requestElementAllowed;

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
                return _suppliers[element.Value];
            }
            return null;
        }

        public Priority Priority
        {
            //get { return Priority.Hight; }        
            get { return Priority.Low; }
            //MDG.2011-06-01.El despaletizado siempre tiene la menor prioridad para que deposite siempre antes las cajas de la entrada
        }

        private ElementTypes? ElementRequestedCore()
        {
            ElementTypes? element = null;

            switch (State)
            {
                case Paletizado.Paletizer.States.PutPaletizer:
                    element = ElementTypes.Paletizer;
                    break;
            }

            return element;
        }

        #endregion

        #region Miembros de ISupplier

        public ElementTypes? ElementSupplied()
        {
            ElementTypes? element = null;

            switch (State)
            {
                case Paletizado.Paletizer.States.QuitItem:
                    element = _itemType;
                    break;
                case Paletizado.Paletizer.States.QuitPalet:
                    element = ElementTypes.Palet;
                    break;
            }

            return element;
        }

        #endregion

        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            return ((ISolicitor) this).GetManualsRepresentations().Concat(
                ((ISupplier) this).GetManualsRepresentations());
        }

        #endregion

        public Despaletizer(string name, CornerPoint3D cornerPoint, Locations location, PaletizerSettings settings,
                            Func<string, CajaPasaportes> boxGetter,
                            Dictionary<ElementTypes, IEnumerable<Locations>> suppliers, IDLine idLine)
            : base(name, cornerPoint, location, settings, boxGetter)
        {
            _suppliers = suppliers;
            _requestElementAllowed = s => true;
            _itemType = (idLine == IDLine.Japonesa) ? ElementTypes.ItemJaponesa : ElementTypes.ItemAlemana;
        }

        public Despaletizer WithRequestElementAllowed(Func<ElementTypes?, bool> requestElementAllowed)
        {
            _requestElementAllowed = requestElementAllowed;
            return this;
        }
    }
}