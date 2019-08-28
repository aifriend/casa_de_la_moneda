using System.Collections.Generic;
using Idpsa.Control.Manuals;
using Idpsa.Control.Subsystem;
using Idpsa.Paletizado.Definitions;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    public abstract class Line : ISubsystemStateAware
    {
        private readonly IDLine _id;

        private readonly List<ISolicitor> _solicitors;
        private readonly SubsystemStateAware _stateAware;
        private readonly List<ISupplier> _suppliers;

        public Line(IDLine id)
        {
            _id = id;
            _solicitors = new List<ISolicitor>();
            _suppliers = new List<ISupplier>();
            _stateAware = new SubsystemStateAware();
        }

        [Subsystem]
        [Manual(SuperGroup = "Estado", Group = "Socitors/Supliers")]
        public PaletStore PaletStore { get; protected set; }

        [Subsystem]
        public ZonaPaletizadoFinal ZonaPaletizadoFinal { get; protected set; }

        public IDLine Id
        {
            get { return _id; }
        }

        public SubsystemState State
        {
            get { return _stateAware.State; }
        }

        protected void AddSolicitor(ISolicitor solicitor)
        {
            _solicitors.Add(solicitor);
        }

        protected void AddSupplier(ISupplier supplier)
        {
            _suppliers.Add(supplier);
        }

        public IEnumerable<ISolicitor> GetSolicitors()
        {
            return _solicitors;
        }

        public abstract bool SetNewCatalog(DatosCatalogoPaletizado datosCatalogo);

        public IEnumerable<ISupplier> GetSuppliers()
        {
            return _suppliers;
        }

        protected string ToStringIdLine()
        {
            return _id == IDLine.Japonesa ? "linea japonesa" : "linea alemana";
            ;
        }

        public void Activate()
        {
            _stateAware.Activate();
        }

        public void Deactivate()
        {
            _stateAware.Deactivate();
        }

        public abstract StoreStateCatalog GetDataToStore();

        #region Miembros de ISubsystemStateAware

        ISubsystemStateObserver ISubsystemStateAware.SetSubsystemStateController(ISubsystemStateController value)
        {
            return ((ISubsystemStateAware) _stateAware).SetSubsystemStateController(value);
        }

        #endregion
    }
}