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
    public class EspecialPaletizer : PaletizerDecorator, ISolicitor, IManualsProvider
    {
        private readonly Despaletizer _despaletizer;
        private readonly ElementTypes _itemType;
        private readonly IEnumerable<Locations> _locationSupplierItemFirstFlat;
        private readonly IEnumerable<Locations> _locationSupplierItemRemainingFlats;
        private readonly bool _paletizeDirectly;
        private readonly Dictionary<ElementTypes, IEnumerable<Locations>> _suppliers;
        private Func<ElementTypes?, bool> _requestElementAllowed;

        public EspecialPaletizer(string name, CornerPoint3D cornerPoint, Locations location, PaletizerSettings settings,
                                 Dictionary<ElementTypes, IEnumerable<Locations>> suppliers,
                                 Func<string, CajaPasaportes> boxGetter, Despaletizer despaletizer, IDLine idLine,
                                 bool paletizeDirectly)
            : base(name, cornerPoint, location, settings, boxGetter)
        {
            _locationSupplierItemFirstFlat = new[] {Locations.Reproceso, Locations.Entrada};
            _locationSupplierItemRemainingFlats = new[] {Locations.Paletizado2Japonesa};
            _suppliers = suppliers;
            _despaletizer = despaletizer;
            _itemType = (idLine == IDLine.Japonesa) ? ElementTypes.ItemJaponesa : ElementTypes.ItemAlemana;
            _paletizeDirectly = paletizeDirectly;
            _requestElementAllowed = s => true;
        }

        public EspecialPaletizer WithRequestElementAllowed(Func<ElementTypes?, bool> requestElementAllowed)
        {
            _requestElementAllowed = requestElementAllowed;
            return this;
        }

        protected override void OnCreated()
        {
            if (UnderLyingPaletizer != null) //&& _despaletizer != null && _despaletizer.UnderLyingPaletizer != null)//MCR2014
            {
                //if (_despaletizer.Mosaics.Count() + 1 > Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos)
                if (Mosaics.Count() + 1 > Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos)
                {
                    Mosaics.ElementAt(0).ItemPositions.Reverse();
                }
            }
            base.OnCreated();
        }

        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            return (this).GetManualsRepresentations();
        }

        #endregion

        #region Miembros de ISolicitor

        public ElementTypes? ElementRequested()
        {
            ElementTypes? element;
            return _requestElementAllowed(element = ElementRequestedCore()) ? element : null;
        }

        public IEnumerable<Locations> GetSuppliersLocations()
        {
            ElementTypes? element;

            if ((element = ElementRequested()) != null)
            {
                if (element == _itemType)
                {
                    if (State == Paletizado.Paletizer.States.PutItem)
                    {
                        if (_paletizeDirectly)
                        {
                            return _locationSupplierItemFirstFlat;
                        }
                        else if (_despaletizer.State == Paletizado.Paletizer.States.QuitItem)
                        {
                            if (CurrentFlat == 0 &&
                                _despaletizer.Mosaics.Count() + 1 > Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos)
                            {
                                return _locationSupplierItemFirstFlat;
                            }
                            else
                            {
                                return _locationSupplierItemRemainingFlats;
                            }
                        }
                    }
                }
                else
                {
                    return _suppliers[element.Value];
                }
            }
            return null;
        }


        public Priority Priority
        {
            get
            {
                //return (State == States.PutItem) ? Priority.Hiest : Priority.Low;
                return Priority.Low;
                //MDG.2011-06-01.Posicion de paletizado final tiene siempre la menor prioridad, para que deposite antes las cajas que vengan por la entrada
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
    }
}