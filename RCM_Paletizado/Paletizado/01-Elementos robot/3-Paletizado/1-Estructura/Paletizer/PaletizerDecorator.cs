using System;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Tool;
using Idpsa.Paletizado.Definitions;

namespace Idpsa.Paletizado
{
    [Serializable]
    public class PaletizerEventArgs : EventArgs
    {
        public PaletizerEventArgs()
        {
        }

        //public PaletizerEventArgs(string name, string idPalet,int totalCurentFlatItems, int totalItems,int currentItems,PaletizerDefinition paletizerData, IPaletElement lastElement,int pos, int flat, bool addingElements)
        public PaletizerEventArgs(string name, string idPalet, int totalCurentFlatItems, int totalItems,
                                  int currentItems, PaletizerDefinition paletizerData, IPaletElement lastElement,
                                  int pos, int flat, bool addingElements, StoredDataPaletizado dataToStore,
                                  IEnumerable<CajaPasaportes> boxes) //MDG.2011-06-17
        {
            Name = name;
            IDPalet = idPalet;
            TotalItems = totalItems;
            TotalCurrentFlatItems = totalCurentFlatItems;
            CurrentItems = currentItems;
            PaletizerData = paletizerData;
            LastElement = lastElement;
            Pos = pos;
            Flat = flat;
            DataToStore = dataToStore; //MDG.2011-06-16
            Boxes = boxes; //MDG.2011-06-17
        }

        public string Name { get; set; }
        public string IDPalet { get; set; }
        public int TotalCurrentFlatItems { get; set; }
        public int TotalItems { get; set; }

        public int CurrentItems { get; set; }
        public PaletizerDefinition PaletizerData { get; set; }
        public IPaletElement LastElement { get; set; }
        public int Pos { get; set; }
        public int Flat { get; set; }
        public StoredDataPaletizado DataToStore { get; set; } //MDG.2011-06-16
        public IEnumerable<CajaPasaportes> Boxes { get; set; } //MDG.2011-06-17
    }

    [Serializable]
    public class PaletizerDecorator : IPaletizer
    {
        private readonly Func<string, CajaPasaportes> _boxGetter;

        //Point3D origen, PaletizerDefinition data, PaletizerSettings settings
        private readonly CornerPoint3D _cornerPoint;
        private readonly PaletizerSettings _settings;

        private EventHandler<PaletizerEventArgs> _changed;
        private EventHandler<PaletizerEventArgs> _created;
        private Locations _location;
        private Paletizer _paletizer;
        private PaletizerDefinition _paletizerData;
        private String _stringLocation;

        public PaletizerDecorator(string name, CornerPoint3D cornerPoint, Locations location, PaletizerSettings settings,
                                  Func<string, CajaPasaportes> boxGetter)
        {
            Name = name;
            _cornerPoint = cornerPoint;
            _location = location;
            _stringLocation = _location.ToString();
            _settings = settings;
            _boxGetter = boxGetter;
        }

        protected IPaletizer Paletizer
        {
            get { return _paletizer; }
        }

        public int CurrentMosaicPosition
        {
            get { return _paletizer.CurrentMosaicIndex; }
        }

        public Paletizer UnderLyingPaletizer
        {
            get { return _paletizer; }
            set
            {
                if (value != null)
                {
                    if (!value.Equals(UnderLyingPaletizer))
                    {
                        OnCreated();
                        value.Name = Name;
                    }
                }
                _paletizer = value;
            }
        }

        #region Miembros de ILocation

        public Locations Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public StoredDataPaletizado GetDataToStore()
        {
            if (Paletizer == null)
                return new StoredDataPaletizado();
            else
                return _paletizer.GetDataToStore();
        }

        #endregion

        #region IPaletizer Members

        public string Name { get; set; }

        public int CurrentFlat
        {
            get { return _paletizer.CurrentFlat; }
        }

        public Point3D CenterOverPalet()
        {
            return _paletizer.CenterOverPalet();
        }

        public Point3D CenterOverSurface()
        {
            return _paletizer.CenterOverSurface();
        }

        public int CurrentMosaicIndex
        {
            get { return _paletizer.CurrentMosaicIndex; }
        }

        public void ElementAdded(IElement element)
        {
            _paletizer.ElementAdded(element);
            OnChanged();
        }

        public IElement ElementQuitted()
        {
            IElement value = _paletizer.ElementQuitted();
            OnChanged();
            return value;
        }

        public void PaletCentered()
        {
            _paletizer.PaletCentered();
        }


        public bool EmptyPalet()
        {
            return _paletizer.EmptyPalet();
        }

        public IPaletizable Item
        {
            get { return _paletizer.Item; }
        }

        public IPalet Palet
        {
            get { return _paletizer.Palet; }
        }

        public IEnumerable<Mosaic> Mosaics
        {
            get { return _paletizer.Mosaics; }
        }

        public Mosaic CurrentMosaic
        {
            get { return _paletizer.CurrentMosaic; }
        }

        public PointSpin3D PositionItem()
        {
            return _paletizer.PositionItem();
        }

        public PointSpin3D PositionPalet()
        {
            return _paletizer.PositionPalet();
        }

        public PointSpin3D PositionPutSeparator()
        {
            return _paletizer.PositionPutSeparator();
        }

        public int RemainingItems()
        {
            return _paletizer.RemainingItems();
        }

        public PaletizerSettings Settings
        {
            get { return _paletizer.Settings; }
            set { _paletizer.Settings = value; }
        }

        public Paletizer.States State
        {
            get
            {
                if (UnderLyingPaletizer == null)
                    return Paletizado.Paletizer.States.PutPaletizer;
                else
                    return UnderLyingPaletizer.State;
            }
            set
            {
                if (UnderLyingPaletizer == null) return;
                UnderLyingPaletizer.State = value;
            }
        }

        public int TotalItems()
        {
            return UnderLyingPaletizer.TotalItems();
        }

        public int TotalToDoItems()
        {
            return UnderLyingPaletizer.TotalToDoItems();
        }

        #endregion

        public event EventHandler<PaletizerEventArgs> Changed
        {
            add { _changed += value; }
            remove { _changed -= value; }
        }

        public event EventHandler<PaletizerEventArgs> Created
        {
            add
            {
                _created += value;
                OnCreated();
            }
            remove { _created -= value; }
        }

        private Point3D GetOrigin()
        {
            return new Point3D(_cornerPoint.Z,
                               new Rectangle(_cornerPoint.ToCornerPoint2D(), _paletizerData.Palet.Base.Lados).Origen);
        }

        protected virtual void OnPaletizerEvent(EventHandler<PaletizerEventArgs> notification)
        {
            Paletizer paletizer = _paletizer;

            if (paletizer != null && notification != null)
            {
                IPaletElement box = paletizer.GetLastItem();

                var e = new PaletizerEventArgs
                            {
                                Name = Name,
                                IDPalet = _paletizer.IDPalet,
                                TotalItems = TotalToDoItems(),
                                TotalCurrentFlatItems = CurrentMosaic.TotalItems(),
                                CurrentItems = _paletizer.Count,
                                PaletizerData = _paletizerData,
                                LastElement = _paletizer.GetLastItem(),
                                Pos = CurrentMosaicIndex,
                                Flat = CurrentFlat,
                                DataToStore = _paletizer.GetDataToStore(),
                                //MDG.2011-06-16  
                                Boxes = _paletizer.GetBoxes() //MDG.2011-06-17
                            };
              
                notification(this, e);
            }
        }

        public void StartPaletizer(DatosCatalogoPaletizado datosCatalogo)
        {
            _paletizerData = datosCatalogo.PaletizerDefinition;
            Paletizer.States storedState = default(Paletizer.States);
            IEnumerable<CajaPasaportes> boxes = GetStoredBoxes(datosCatalogo, out storedState);

            if (_settings.Action == Paletizado.Paletizer.Action.Paletize || boxes.Count() > 0)
            {
                _paletizer = new Paletizer(Name, GetOrigin(), _paletizerData, _settings, storedState, boxes);
                if (_settings.Action == Paletizado.Paletizer.Action.Despaletize)
                {
                    List<CajaPasaportes> boxesleft = new List<CajaPasaportes>();
                    List<CajaPasaportes> boxesputted = new List<CajaPasaportes>();
                    bool descolocado = false;
                    int lastPos=0;
                    int lastFlat=0;
                    int index = 0;
                    List<Mosaic> mosaics = _paletizer.Mosaics.ToList();
                    List<CajaPasaportes> cajitas = _paletizer.GetBoxes().ToList();
                    bool noExiste = false;
                    int inicialCount = _paletizer.Count;
                    do
                    {
                        foreach (CajaPasaportes box in cajitas)
                        {
                            if (box.CatalogIndex == index)
                            {
                                index++;
                                _paletizer._production.Modificate(box.Pos, box.Flat, box, lastPos,lastFlat);
                                //box.Pos = lastPos;
                                //box.Flat = lastFlat;
                                if (_paletizer.CurrentFlat == lastFlat && _paletizer.CurrentMosaic.pos < lastPos)
                                    _paletizer.CurrentMosaic.pos = lastPos;
                                if (_paletizer.CurrentFlat < lastFlat)
                                {
                                    _paletizer.CurrentMosaic.pos = lastPos;
                                    _paletizer.CurrentFlat = lastFlat;
                                }
                                if (_paletizer.Mosaics.Count() > 0)
                                {
                                    Mosaic mosaic = mosaics[lastFlat];
                                    bool value = lastPos < mosaic.ItemPositions.Count - 1;
                                    if (value)
                                    {
                                        lastPos++;
                                    }
                                    else
                                    {
                                        if (lastFlat < Settings.MinFlatsToDo - 1)
                                            lastPos = 0;
                                    }
                                }
                                if ((lastPos == 0))
                                {
                                    if (lastFlat < Settings.MinFlatsToDo - 1)
                                    {
                                        lastFlat = lastFlat + 1;
                                    }
                                }
                                boxesputted.Add(box);
                                if (boxesleft.Contains(box))
                                    boxesleft.Remove(box);

                            }
                            else
                            {
                                if (!boxesputted.Contains(box) && !boxesleft.Contains(box))
                                    boxesleft.Add(box);
                            }
                        }
                        noExiste=false;
                        if(boxesleft.Count>0)
                        {
                            noExiste=true;
                            foreach (CajaPasaportes caja in boxesleft)
                                if (caja.CatalogIndex == index)
                                    noExiste = false;
                        }

                    } while (boxesleft.Count != 0 && index<=inicialCount&&!noExiste);
                }
                if (_paletizer != null && boxes.Count() != 0 && (_paletizer.GetBoxes().Count() != boxes.Count() || !_paletizer.CorretIndex()))
                {
                    _paletizer.ClearPaletizer();
                    _paletizer.fillPaletizer(boxes.Count());

                }  
            
                OnCreated();
                OnChanged(); //MDG.Duda de si quitar evento para que cargue bien los datos de las mesas de rodillos


        }
                else if (_settings.Action == Paletizado.Paletizer.Action.Despaletize)
            {
                if (_paletizer != null) //MDG. Duda. Paletizer es null al llegar aqui
                    _paletizer.StartPaletizer(GetOrigin(), _settings);
                else
                {
                    Paletizer aux = new Paletizer(Name, GetOrigin(), _paletizerData, _settings, storedState, boxes);
                    if (aux.Count > 0)
                    {
                        _paletizer = new Paletizer(Name, GetOrigin(), _paletizerData, _settings, storedState, boxes);
                        if (_settings.Action == Paletizado.Paletizer.Action.Despaletize)
                        {
                            List<CajaPasaportes> boxesleft = new List<CajaPasaportes>();
                            bool descolocado = false;
                            int lastPos = 0;
                            int lastFlat = 0;
                            int index = 0;
                            List<Mosaic> mosaics = _paletizer.Mosaics.ToList();
                            do
                            {
                                foreach (CajaPasaportes box in _paletizer.GetBoxes())
                                {
                                    if (box.CatalogIndex == index)
                                    {
                                        index++;
                                        _paletizer._production.Modificate(box.Pos, box.Flat, box, lastFlat, lastPos);
                                        //box.Pos = lastPos;
                                        //box.Flat = lastFlat;
                                        
                                        if (_paletizer.Mosaics.Count() > 0)
                                        {
                                            Mosaic mosaic = mosaics[lastFlat];
                                            bool value = lastPos < mosaic.ItemPositions.Count - 1;
                                            if (value)
                                            {
                                                lastPos++;
                                            }
                                            else
                                            {
                                                if (lastFlat < Settings.MinFlatsToDo - 1)
                                                    lastPos = 0;
                                            }
                                        }
                                        if ((lastPos == 0))
                                        {
                                            if (lastFlat < Settings.MinFlatsToDo - 1)
                                            {
                                                lastFlat = lastFlat + 1;
                                            }
                                        }
                                        if (boxesleft.Contains(box))
                                            boxesleft.Remove(box);
                                    }
                                    else
                                    {
                                        boxesleft.Add(box);
                                    }
                                }
                            } while (boxesleft.Count != 0 && index <= _paletizer.Count);
                        }
                        OnCreated();
                        OnChanged(); //MCR   

                    }
                }
                if (_paletizer != null && boxes.Count() != 0&&(_paletizer.GetBoxes().Count() != boxes.Count() || !_paletizer.CorretIndex()))
                {
                    _paletizer.ClearPaletizer();
                    _paletizer.fillPaletizer(boxes.Count());

                }    
     
            }

        }
        public void StartPaletizerB(DatosCatalogoPaletizado datosCaT)
        {
            PaletizerDefinition auxPaletizerDef = new PaletizerDefinition(datosCaT.PaletizerDefinition);
            int NMosaics = datosCaT.PaletizerDefinition.MosaicTypes.Count;
            if (NMosaics > Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos)
            {
                IPaletizable item = PaletizadoElements.Create(PaletizableTypes.box);
                Mosaic penulM = new Mosaic(item, datosCaT.PaletizerDefinition.Palet, auxPaletizerDef.MosaicTypes[Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos - 1]);
                Mosaic ulM = new Mosaic(item, datosCaT.PaletizerDefinition.Palet, auxPaletizerDef.MosaicTypes[Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos]);
                if (penulM.TotalItems() != ulM.TotalItems())
                    for (int i = 0; i < Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos; i++)
                    {
                        auxPaletizerDef.MosaicTypes[i] = auxPaletizerDef.MosaicTypes[i + 1];
                    }
            }
            DatosCatalogoPaletizado datosCatalogo = new DatosCatalogoPaletizado(datosCaT, auxPaletizerDef);
            StartPaletizer(datosCatalogo);
        }
        public void StartNewPaletizer(DatosCatalogoPaletizado datosCatalogo)
        {
            _paletizerData = datosCatalogo.PaletizerDefinition;
            Paletizer.States storedState = default(Paletizer.States);
            //var boxes = GetStoredBoxes(datosCatalogo, out storedState);
            var boxes = new CajaPasaportes[0]; //reseteamos las cajas. No tiene ninguna
            //storedState = default(States);

            if (_settings.Action == Paletizado.Paletizer.Action.Paletize || boxes.Count() > 0)
            {
                _paletizer = new Paletizer(Name, GetOrigin(), _paletizerData, _settings, storedState, boxes);
                OnCreated();
                OnChanged(); //MDG.Duda de si quitar evento para que cargue bien los datos de las mesas de rodillos
            }
            else if (_settings.Action == Paletizado.Paletizer.Action.Despaletize)
            {
                if (_paletizer != null) //MDG. Duda. Paletizer es null al llegar aqui
                    _paletizer.StartPaletizer(GetOrigin(), _settings);
            }
        }

        public void StartNewPaletizerB(DatosCatalogoPaletizado datosCaT)
        {
            PaletizerDefinition auxPaletizerDef = new PaletizerDefinition(datosCaT.PaletizerDefinition);
            int NMosaics = datosCaT.PaletizerDefinition.MosaicTypes.Count;
            if (NMosaics > Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos)
            {
                IPaletizable item = PaletizadoElements.Create(PaletizableTypes.box);
                Mosaic penulM = new Mosaic(item, datosCaT.PaletizerDefinition.Palet, auxPaletizerDef.MosaicTypes[Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos - 1]);
                Mosaic ulM = new Mosaic(item, datosCaT.PaletizerDefinition.Palet, auxPaletizerDef.MosaicTypes[Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos]);
                if (penulM.TotalItems() != ulM.TotalItems())
                    for (int i = 0; i < Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos; i++)
                    {
                        auxPaletizerDef.MosaicTypes[i] = auxPaletizerDef.MosaicTypes[i + 1];
                    }
            }
            DatosCatalogoPaletizado datosCatalogo = new DatosCatalogoPaletizado(datosCaT, auxPaletizerDef);
            StartNewPaletizer(datosCatalogo);
           
        }

        //MDG.2011-06-17.Metodo para obtener las cajas para mostrarlas en consultas por pantalla
        //public IEnumerable<CajaPasaportes> GetStoredBoxesForScada(DatosCatalogoPaletizado datosCatalogo)
        //{
        //    //_paletizerData = datosCatalogo.PaletizerDefinition;
        //    States storedState = default(States);
        //    var boxes = GetStoredBoxes(datosCatalogo, out storedState);
        //    return boxes;
        //}

        private IEnumerable<CajaPasaportes> GetStoredBoxes(DatosCatalogoPaletizado datosCatalogo,
                                                           out Paletizer.States storedState)
        {
            storedState = default(Paletizer.States);
            if (datosCatalogo.SotoredData != null)
            {
                StoredDataPaletizado storedDataPaletizado = datosCatalogo.SotoredData.GetDataPaletizer(_location);

                if (storedDataPaletizado != null && storedDataPaletizado.Boxes != null)
                {
                    var list = new List<CajaPasaportes>();
                    storedState = storedDataPaletizado.State;
                    foreach (string id in storedDataPaletizado.Boxes)
                    {
                        CajaPasaportes box = _boxGetter(id);
                        list.Add(box);
                    }

                    return storedDataPaletizado.Boxes.Select(id => _boxGetter(id));
                }
            }
            return new CajaPasaportes[0];
        }

        public virtual void OnChanged()
        {
            OnPaletizerEvent(_changed);

            //MDG.2010-11-29.Añadimos Salvado en fichero de variables de paletizado ante evento de cambio de las mismas
            //Idpsa.Paletizado.StoreStateCatalog.Sys.Production.SaveCatalogs();
        }

        protected virtual void OnCreated()
        {
            OnPaletizerEvent(_created);
        }


        //MCR2014
        public void ForceElementAdded(IElement element, int i)
        {
            //IElement element = ForceAddItem(IElement element);
            if (element != null)
            {
                if(i!=2)
                    UnderLyingPaletizer.ForceElementAdded(element,i);
                else
                    UnderLyingPaletizer.ForceElementAddedDepaletizer(element,i);
            }
        }

        public void ForceElementQuitted(int i)
        {
            if (i==2)
                UnderLyingPaletizer.ForceElementQuitted(i);
            else
                UnderLyingPaletizer.ForceElementQuittedPaletizer(i);
        }

        //public CajaPasaportes ForceAddItem()
        //{
        //    if (!_production.IsCatalogReady(_idLine)) return null;
        //    CatalogManager catalogManager = _production.GetCatalogManager(_idLine);
        //    InitCurrentBoxIndex();

        //    if (_idLine == IDLine.Alemana)
        //    {
        //        if (_currentIndex >= 0)
        //            return GetBox(_currentIndex--);
        //    }
        //    else
        //    {
        //        if (_currentIndex < catalogManager.BoxesCount)
        //            return catalogManager.GetBox(_currentIndex++);
        //    }
        //    return null;
        //}


    }
}