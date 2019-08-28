using System;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Tool;
using Idpsa.Paletizado.Definitions;

namespace Idpsa.Paletizado
{
    [Serializable]
    public class PaletizerSettings
    {
        public PaletizerSettings(Paletizer.Action action, Paletizer.States initialState)
        {
            Action = action;
            InitialState = initialState;
        }

        public Paletizer.Action Action { get; set; }
        public Paletizer.States InitialState { get; set; }
        public bool HasSeparator { get; set; }
        public int FlatSeparatorFrecuency { get; set; }
        public bool DownSeparator { get; set; }
        public int MinFlatsToDo { get; set; }
        public bool NewIDPalet { get; set; }
        public bool PaletMustBeCenteredAfterPutted { get; set; }
    }

    [Serializable]
    public class Paletizer : IPaletizer
    {
        #region Action enum

        [Serializable]
        public enum Action
        {
            Paletize,
            Despaletize
        }

        #endregion

        #region States enum

        [Serializable]
        public enum States
        {
            PutPalet,
            CenterPalet,
            QuitPalet,
            PutPaletizer,
            QuitPaletizer,
            PutItem,
            QuitItem,
            PutSeparator
        }

        #endregion

        private readonly PaletizerDefinition _data;
        public  PaletProduction _production;


        private int _currentFlat;
        private List<Mosaic> _mosaics;
        private Point3D _origen;

        private States _state;

        public Paletizer(string name, Point3D origen,
                         PaletizerDefinition data, PaletizerSettings settings, States storedState,
                         IEnumerable<CajaPasaportes> boxesAlreadyPutted)
        {
            Name = name;
            _currentFlat = 0;
            _data = data;
            _production = new PaletProduction(data, settings.NewIDPalet);
            StartPaletizerCore(origen, settings, boxesAlreadyPutted, storedState);
        }

        public string IDPalet
        {
            get { return _production.IDPalet; }
        }

        public int Count
        {
            get { return _production.Count; }
        }

        #region IPaletizer Members

        public string Name { get; set; }

        public PaletizerSettings Settings { get; set; }

        public States State
        {
            get { return _state; }
            set { _state = value; }
        }

        public IPalet Palet
        {
            get { return _data.Palet; }
        }

        public IPaletizable Item
        {
            get { return _data.Item; }
        }

        public IEnumerable<Mosaic> Mosaics
        {
            get { return _mosaics; }
        }

        public Mosaic CurrentMosaic
        {
            get { return _mosaics[CurrentFlat]; }
        }


        public Point3D CenterOverSurface()
        {
            return _origen.Desplazado(_data.Palet.Base.Lados.X/2, _data.Palet.Base.Lados.Y/2, 0);
        }

        public Point3D CenterOverPalet()
        {
            return _origen.Desplazado(_data.Palet.Base.Lados.X/2, _data.Palet.Base.Lados.Y/2, _data.Palet.Height);
        }


        public PointSpin3D PositionPalet()
        {
            return new PointSpin3D(Spin.S90, CenterOverSurface().Desplazado(0, 0, -_data.Palet.Height));
        }


        public PointSpin3D PositionItem()
        {
            PointSpin2D p2dS = _mosaics[_currentFlat].Position();
            var p3dS = new PointSpin3D(HeightPutItem(), p2dS);
            return p3dS;
        }


        public PointSpin3D PositionPutSeparator()
        {
            return new PointSpin3D(HeightPutSeparator(),
                                   new PointSpin2D(Spin.S0, CenterOverPalet().X, CenterOverPalet().Y));
        }

        public int CurrentFlat
        {
            set { _currentFlat = value; }//MCR.
            get { return _currentFlat; }
        }

        public int CurrentMosaicIndex
        {
            get
            {
                int value = -1;
                if (_mosaics.Count > 0)
                {
                    value = _mosaics[_currentFlat].CurrentIndexItem();
                }
                return value;
            }
        }

        public int RemainingItems()
        {
            int result = 0;
            _mosaics.Where((m, i) => i >= _currentFlat).ForEach(m => result += m.RemainingItems());
            return result;
        }

        public bool EmptyPalet()
        {
            return (RemainingItems() == TotalItems());
        }


        public int TotalItems()
        {
            int result = 0;
            foreach (Mosaic mosaic in _mosaics)
                result += mosaic.TotalItems();

            return result;
        }

        public int TotalToDoItems()
        {
            int result = 0;
            for (int i = 0; i < Settings.MinFlatsToDo; i++)
            {
                result += _mosaics[i].TotalItems();
            }
            return result;
        }

        public void ElementAdded(IElement element)
        {
            switch (element.GeneralType)
            {
                case ElementTypes.Palet:
                    if (Settings.PaletMustBeCenteredAfterPutted)
                    {
                        State = States.CenterPalet;
                    }
                    else
                    {
                        if (Settings.HasSeparator && Settings.DownSeparator)
                            State = States.PutSeparator;
                        else
                            State = States.PutItem;
                    }
                    break;
                case ElementTypes.Item:
                    if (Settings.Action == Action.Despaletize)
                        throw new Exception("Can't add item to paletizer " + Name +
                                            ", because current state is Despaletize");
                    else
                    {
                        AttachElement((IPaletElement) element);
                        NextPaletizeElement();
                    }
                    break;
                case ElementTypes.Separator:
                    if (Settings.Action == Action.Paletize)
                        State = States.PutItem;
                    else
                    {
                        throw new Exception("Can't add a carton to paletizer " + Name + ", because current state is: " +
                                            _state);
                    }
                    break;
            }
        }

        public IElement ElementQuitted()
        {
            IElement element = null;
            switch (State)
            {
                case States.QuitItem:
                    if (Settings.Action != Action.Despaletize)
                        throw new Exception("Quit an item to the paletizer " + Name +
                                            ", is inconsistent with its state: " + _state);
                    element = DettachElement();
                    NextDespaletizeElement();
                    break;
                case States.QuitPalet:
                    element = _data.Palet;
                    State = States.PutPaletizer;
                    break;
                case States.PutPaletizer:
                    //MDG.2011-06-29.No lanzamos excepcion en este caso, que se llama varias veces
                    element = _data.Palet;
                    break;
                default:
                    throw new Exception("It's imposible to remove a element in the current state of the paletizer: " +
                                        Name);
                    break;
            }

            return element;
        }

        public void PaletCentered()
        {
            if (State != States.CenterPalet)
                throw new Exception("state inconsistence");

            if (Settings.HasSeparator && Settings.DownSeparator)
                State = States.PutSeparator;
            else
                State = States.PutItem;
        }

        #endregion

        public void StartPaletizer(Point3D origen, PaletizerSettings settings)
        {
            StartPaletizerCore(origen, settings, new CajaPasaportes[0], settings.InitialState);
        }

        private void StartPaletizerCore(Point3D origen, PaletizerSettings settings,
                                        IEnumerable<CajaPasaportes> boxesAlreadyPutted, States storedState)
        {
            _origen = origen;
            Settings = settings;
            _data.Palet.Base.Origen = new Point2D(origen.X, origen.Y);
            _mosaics = new List<Mosaic>();
            CreatePaletizer(boxesAlreadyPutted, storedState);
        }

        private void CreatePaletizer(IEnumerable<CajaPasaportes> initialBoxes, States storedState)
        {
            MosaicInitialPosition initialPosition = default(MosaicInitialPosition);
            if (Settings.Action == Action.Paletize) //MDG.2011-03-24// ||initialBoxes.Count()>0)
            {
                initialPosition = MosaicInitialPosition.Start;
            }
            else if (Settings.Action == Action.Despaletize)
            {
                initialPosition = MosaicInitialPosition.End;
            }

            foreach (MosaicType mosaicType in _data.MosaicTypes)
                _mosaics.Add(new Mosaic(_data.Item, _data.Palet, mosaicType, initialPosition));


            if (Settings.MinFlatsToDo == 0 || Settings.MinFlatsToDo > _mosaics.Count())
            {
                Settings.MinFlatsToDo = _mosaics.Count();
            }

            if (initialPosition == MosaicInitialPosition.End)
            {
                _currentFlat = Settings.MinFlatsToDo - 1;
            }
            States state = (initialBoxes.Count() > 0) ? storedState : Settings.InitialState;
                //MDG.Duda.Es posible que cargue siempre el stored state
            PutInitialBoxes(state, initialBoxes);
            State = state;
        }

        private double HeightPutItem()
        {
            Mosaic mosaic = _mosaics[_currentFlat];
            PointSpin2D p = mosaic.Position();
            GeometriaConf geo = new GeometriaConf();
            double z = _origen.Z - _data.Palet.Height - _data.Separator.Thickness*_currentFlat -
                       //_data.Item.Dimensions.Height*(_currentFlat + 1);
                       geo.conf.boxMeasures.Z * (_currentFlat + 1)-20;
            return z;
        }

        private double HeightPutSeparator()
        {
            return _origen.Z - _data.Palet.Height -
                   (_data.Separator.Thickness + _data.Item.Dimensions.Height)*_currentFlat - 50;
        }

        private void AssignProduction()
        {
            throw new NotImplementedException();
        }


        private bool AttachElement(IPaletElement element)
        {
            return(_production.Attach(CurrentMosaicIndex, _currentFlat, element));
        }

        private IPaletElement DettachElement()
        {
            return _production.Dettach(CurrentMosaicIndex, _currentFlat);
        }

        private IPaletElement DettachElementForced()
        {
            var v = _production.Dettach(CurrentMosaicIndex, _currentFlat);
            //if (CurrentMosaicIndex == 0 && CurrentFlat == 0)
            //{
            //    //Paletizer.States storedState = default(Paletizer.States);
            //    //var list = new List<CajaPasaportes>();
            //    //StartPaletizerCore(this._origen, Settings, list, storedState);
            //}
            return v;

        }

        public IPaletElement GetLastItem()
        {
            return _production.GetLastItem();
        }

        public IEnumerable<CajaPasaportes> GetBoxes() //MDG.2011-06-17
        {
            return _production.GetBoxes(); //.Select(b => b.Id).ToList();
        }

        private void NextInitialBox()
        {
            if (_mosaics.Count > 0)
            {
                Mosaic mosaic = _mosaics[_currentFlat];
                if ((!mosaic.NextElement()))
                {
                    if (_currentFlat < Settings.MinFlatsToDo - 1)
                    {
                        _currentFlat = _currentFlat + 1;
                    }
                }
            }
        }

        private void NextPaletizeElement()
        {
            if (_mosaics.Count > 0)
            {
                Mosaic mosaic = _mosaics[_currentFlat];
                if ((!mosaic.NextElement()))
                {
                    SetNexElementPaletizeState();
                }
            }
        }
        private void NextPaletizeElementForced(int i)
        {
            //if (i == 2)
            //    NextPaletizeElement();
            //else
            if (_mosaics.Count > 0)
            {
                Mosaic mosaic = _mosaics[_currentFlat];
                if ((!mosaic.NextElement()))
                {
                    SetNexElementPaletizeStateForced();
                }
            }
        }

        private void NextDespaletizeElement()
        {
            Mosaic mosaic = _mosaics[_currentFlat];
            if ((!mosaic.PreviousElement()))
            {
                SetNextElementDepaletizeState();
            }
        }

        private void NextDespaletizeElementForced()
        {
            Mosaic mosaic = _mosaics[_currentFlat];
            if ((!mosaic.PreviousElement()))
            {
                SetNextElementDepaletizeStateForced();
            }
        }

        /// <summary>
        /// Se llama cuando se acaba un piso
        /// antes de incrementar el índice        
        /// </summary>

        protected void SetNexElementPaletizeStateForced()
        {
            if (_currentFlat < Settings.MinFlatsToDo - 1)
            {
                _currentFlat = _currentFlat + 1;
                //_state = States.PutItem;
                Mosaic mosaic = _mosaics[_currentFlat];
                mosaic.ForcedFirstItem();

            }
            else
            {
                _currentFlat = 0;
                _state = States.QuitPaletizer;
            }

        }

        protected void SetNexElementPaletizeState()
        {
            if (_currentFlat < Settings.MinFlatsToDo - 1)
            {
                _currentFlat = _currentFlat + 1;
                _state = States.PutItem;

                if (Settings.HasSeparator)
                {
                    if (Settings.FlatSeparatorFrecuency == 0)
                    {
                        throw new ArgumentException("FlatSeparatorFrecuency cant't be 0 if HasSeparator is set to true");
                    }
                    if (_currentFlat%Settings.FlatSeparatorFrecuency == 0)
                    {
                        _state = States.PutSeparator;
                    }
                }
            }
            else
            {
                _currentFlat = 0;
                _state = States.QuitPaletizer;
            }
        }

        protected void SetNextElementDepaletizeState()
        {
            if (_currentFlat >= 1)
            {
                _currentFlat = _currentFlat - 1;
                _state = States.QuitItem;
                Mosaic mosaic = _mosaics[_currentFlat];
                mosaic.ForcedLastPosition();
            }
            else
            {
                _state = States.QuitPalet;
            }
        }

        protected void SetNextElementDepaletizeStateForced()
        {           
            if (_currentFlat >= 1)
            {
                _currentFlat = _currentFlat - 1;
                //_state = States.QuitItem;
                _mosaics[_currentFlat].ForcedLastPosition();
            }
            else
            {
                _state = States.QuitPalet;
            }
        }

        private void PutInitialBoxes(States state, IEnumerable<CajaPasaportes> boxes)
        {
            CajaPasaportes lastBox = boxes.LastOrDefault();
            int i=0,j=0;
            int FlatToReturn = 0;
            int LastFlat = _mosaics.Count-1;
            if (_mosaics.Count > Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos)
                LastFlat = Geometria.MaxNumberFlatsPaletizadoEnMesaRodillos;
            bool attached = false;
            foreach (CajaPasaportes box in boxes.Where(b => b != null))
            {
                //j = 0;
                //do
                //{
                //    i++;
                    attached = AttachElement(box);
                    if (!(state == States.QuitItem && box == lastBox))
                        NextInitialBox();
                //    if (!(state == States.QuitItem && i == boxes.Count()))
                //    {
                //        if (LastFlat - j > 0)
                //            _currentFlat = LastFlat-j;
                //        _mosaics[LastFlat].ForcedFirstItem();
                //        j++;
                //        i = 0;

                //    }
                //} while (attached == false&&j<_mosaics.Count-1);//MCR2014
                //if (attached == true && FlatToReturn < _currentFlat)
                //    FlatToReturn = _currentFlat;
                //attached = false;
            }
            //_currentFlat = FlatToReturn;
        }

        public StoredDataPaletizado GetDataToStore()
        {
            return new StoredDataPaletizado
                       {
                           Boxes = _production.GetBoxes().Select(b => b.Id).ToList(),
                           State = State
                       };
        }

        //MDG.2011-06-20.Metodo para determinar si un ID ya está paletizado y rechazarlo
        public bool ContainsBoxID(string BoxID)
        {
            List<string> BoxesLocal = _production.GetBoxes().Select(b => b.Id).ToList();
            if (BoxesLocal != null)
            {
                if (BoxesLocal.Contains(BoxID))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public void ForceElementAdded(IElement element, int i)
        {
            if (element!=null)
            {
                AttachElement((IPaletElement)element);
                NextPaletizeElementForced(i);
            } 
        }

        public void ForceElementQuitted(int i)
        {
            DettachElementForced();
            NextDespaletizeElementForced();
        }

        public void ForceElementAddedDepaletizer(IElement element, int i)
        {
            if (element!=null)
            {
                if (Count>0)
                    NextPaletizeElementForced(i);
                AttachElement((IPaletElement)element);
            } 
        }

        public void ForceElementQuittedPaletizer(int i)
        {
            if(Count<TotalToDoItems())
                NextDespaletizeElementForced();
            DettachElementForced();
        }

        public void ClearPaletizer()
        {
            _currentFlat = 0;
            _mosaics[0].ForcedFirstItem();
            _production.ResetElementes();
        }

        public void fillPaletizer(int count)
        {

            SourceGroupSupplier groupSupplier = SourceGroupSupplier.Create(RCMCommonTypes.IDLine.Japonesa);
            CajaPasaportes box=null;
            GrupoPasaportes auxP = null;
            do
            {
                groupSupplier.GetNewIndexCreating(box);
                int ind = groupSupplier.currentIndexCreating;
                auxP = groupSupplier.GetItem(ind + 1);
                if (auxP != null && auxP.TipoPasaporte != null)
                {
                    box = new CajaPasaportes(auxP);
                    for (int c = 0; c < CajaPasaportes.NGrupos; c++)
                    {
                        ind = groupSupplier.currentIndexCreating++;
                        box.Add(groupSupplier.GetItem(ind + 1), CajaPasaportes.NGrupos - c - 1);
                    }

                    box = groupSupplier.GetBox(box.Id);
                    if (this.Count < this.TotalToDoItems())
                    {
                        ForceElementAddedDepaletizer(box, 2);
                    }
                }
            } while (this.Count < count && auxP != null);

        }

        public bool CorretIndex()
        {

            int result = 0;
            for (int i = 0; i < CurrentFlat; i++)
            {
                result += _mosaics[i].TotalItems();
            }
            for (int i = 0; i < CurrentMosaicIndex; i++)
            {
                result++;
            }

            return (result == this.Count);

        }
        #region Miembros de ILocation

        public virtual Locations Location
        {
            get { return Locations.None; }
        }

        #endregion
    }
}