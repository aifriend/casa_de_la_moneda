using System.Collections.Generic;
using System.Linq;

namespace Idpsa.Paletizado.Definitions
{
    public class Mediator
    {
        private readonly IEnumerable<ISolicitor> _commonSolicitors;
        private readonly IEnumerable<ISupplier> _commonSuppliers;
        private readonly List<ISolicitor> _solicitors;
        private readonly List<ISupplier> _suppliers;

        public Mediator(IEnumerable<ISolicitor> commonSolicitors, IEnumerable<ISupplier> commonSuppliers)
        {
            _commonSolicitors = commonSolicitors;
            _commonSuppliers = commonSuppliers;
            _solicitors = new List<ISolicitor>(_commonSolicitors);
            _suppliers = new List<ISupplier>(_commonSuppliers);
        }

        public void AddSuppliersAndSolicitors(Line line)
        {
            AddSolicitors(line);
            AddSuppliers(line);
        }

        public void RemoveSuppliersAndSolicitors(Line line)
        {
            RemoveSolicitors(line);
            RemoveSuppliers(line);
        }

        private void AddSolicitors(Line line)
        {
            foreach (ISolicitor solicitor in line.GetSolicitors())
                _solicitors.Add(solicitor);
        }

        private void AddSuppliers(Line line)
        {
            foreach (ISupplier supplier in line.GetSuppliers())
                _suppliers.Add(supplier);
        }

        private void RemoveSolicitors(Line line)
        {
            foreach (ISolicitor solicitor in line.GetSolicitors())
                _solicitors.Remove(solicitor);
        }

        private void RemoveSuppliers(Line line)
        {
            foreach (ISupplier supplier in line.GetSuppliers())
                _suppliers.Remove(supplier);
        }

        public IEnumerable<ISolicitor> GetSolicitors()
        {
            return _solicitors.OrderBy(s => s.Priority);
        }

        public IEnumerable<ISupplier> GetSuppliers()
        {
            return _suppliers;
        }
    }
}