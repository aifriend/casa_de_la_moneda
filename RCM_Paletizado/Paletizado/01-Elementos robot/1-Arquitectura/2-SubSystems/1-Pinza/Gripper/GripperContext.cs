using System;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Sequence;
using Idpsa.Paletizado.Definitions;

namespace Idpsa.Paletizado
{
    public sealed class GripperContext : IChainController
    {
        private static readonly GripperContext _instance = new GripperContext();
        private readonly Dictionary<string, Func<object, Subchain>> _callChain;
        private string _idControlledChain;

        private Lines _lines;

        private GripperContext()
        {
            Chain = Chains.Espera;
            State = new Wait_State();
            _callChain = new Dictionary<string, Func<object, Subchain>>();
        }

        //private IDPSASystemPaletizado _sys;//MDG.2011-06-15.Para tener acceso a la posicion actual del robot
        public IGantryState State { get; set; }
        public Chains Chain { get; set; }
        public IContainer CatchContainer { get; set; }
        public IContainer LeaveContainer { get; set; }

        public static GripperContext GetInstance()
        {
            return _instance;
        }

        public void SetLines(Lines lines)
        {
            _lines = lines;
        }

        public void CallChain()
        {
            _callChain[_idControlledChain](Chain).WithParams(State.GetChainParams());
        }

        public bool NextState()
        {
            IGantryState state1 = State;

            if (!State.NextState())
                CheckNextState();

            return !state1.Equals(State);
        }


        private void CheckNextState()
        {
            IEnumerable<ISolicitor> xSolicitors = _lines.GetSolicitors();
            IEnumerable<ISupplier> xSupliers1 = _lines.GetSuppliers();
            //var xSupliers2 = _lines.GetSuppliers(xSolicitors.GetSuppliersLocations());

            var value = (from solicitor in _lines.GetSolicitors()
                         where (solicitor.ElementRequested() != null &&
                                solicitor.ElementRequested().Value != ElementTypes.Paletizer)
                         let suppliers = _lines.GetSuppliers(solicitor.GetSuppliersLocations())
                         from supplier in suppliers
                         where (supplier != null && supplier.ElementSupplied() != null &&
                                supplier.ElementSupplied() == solicitor.ElementRequested())
                         select new
                                    {
                                        Solicitor = solicitor,
                                        Supplier = supplier,
                                    }).FirstOrDefault();
            if (_lines.Line1.BoxNotCatchedDespaletizing)//MDG.2013-04-25
            {
                Chain = Chains.Espera;
                State = new Wait_State();
            }
            else if (value != null)
            {
                CatchContainer = (IContainer) value.Supplier;
                LeaveContainer = (IContainer) value.Solicitor;
                switch (value.Solicitor.ElementRequested())
                {
                    case ElementTypes.Item:
                    case ElementTypes.ItemJaponesa:
                    case ElementTypes.ItemAlemana:
                        Chain = Chains.CatchItem;
                        State = new CathItem_State();
                        break;
                    case ElementTypes.Palet:
                        if (CatchContainer is PaletStore)
                            Chain = Chains.CatchPaletInStore;
                        else
                            Chain = Chains.CatchPalet;
                        State = new CatchPalet_State();
                        break;
                    case ElementTypes.Separator:
                        Chain = Chains.CatchSeparator;
                        State = new CatchSeparator_State();
                        break;
                }
            }
            else
            {
                //Chain = Chains.Espera;
                //State = new Wait_State();

                try //MDG.2011-06-14. Nueva asignacion
                {
                    //if(_sys.Gripper.Position().X
                    //if (_context.CatchContainer.Location.Is(Locations.Entrada)
                    //|| _context.CatchContainer.Location.Is(Locations.Reproceso))//MDG.2010-12-01.Despues de reproceso se vuelve a chequear el peso
                    //{
                    //    Chain = Chains.Espera;
                    //    State = new Wait_State();
                    //}
                    //else
                    //{
                    Chain = Chains.GoToOrigin;
                    State = new GoToOrigin_State();
                    //}
                }
                catch (Exception ex)
                {
                    Chain = Chains.Espera;
                    State = new Wait_State();
                }
            }
        }

        public void ElementLeft()
        {
            State.ElementLeft();
        }

        public void ElementCatched()
        {
            State.ElementCatched();
        }

        #region Miembros de IChainController

        public void SetCaller(string idControlledChain)
        {
            _idControlledChain = idControlledChain;
        }


        public void SetChainCallers(IChainControllable chainControllable)
        {
            if (!_callChain.ContainsKey(chainControllable.IdControlledChain))
            {
                _callChain[chainControllable.IdControlledChain] = chainControllable.GetSubchainCaller();
            }
        }

        public IEnumerable<Subchain> GetSubchains()
        {
            return new Subchain[0];
        }

        #endregion

        #region Nested type: CogerPaletParams

        public struct CogerPaletParams
        {
            public const string position = "position";
        }

        #endregion
    }
}