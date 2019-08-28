using System;
using System.Linq;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    public class SourceBoxSupplier : IItemSuplier<CajaPasaportes>
    {
        private static SystemProductionPaletizado s_production;
        private readonly IDLine _idLine;
        private readonly bool _invertedOrder;
        private readonly SystemProductionPaletizado _production;
        public int _currentIndex; //MDG.2010-12-09.Puesto public para leerlo y escribirlo de fichero

        private SourceBoxSupplier(IDLine idLine, SystemProductionPaletizado production)
        {
            _production = production;
            _idLine = idLine;
            _currentIndex = - 1;
            _invertedOrder = (_idLine == IDLine.Alemana) ? true : false;
        }

        #region IItemSuplier<CajaPasaportes> Members

        public CajaPasaportes QuitItem()
        {
            if (!_production.IsCatalogReady(_idLine)) return null;
            CatalogManager catalogManager = _production.GetCatalogManager(_idLine);
            InitCurrentBoxIndex();

            if (_idLine == IDLine.Alemana)
            {
                if (_currentIndex >= 0)
                    return catalogManager.GetBox(_currentIndex--);
            }
            else
            {
                if (_currentIndex < catalogManager.BoxesCount)
                    return catalogManager.GetBox(_currentIndex++);
            }
            return null;
        }

        #endregion

        public static SourceBoxSupplier Create(IDLine idLine)
        {
            if (s_production == null)
                throw new InvalidOperationException("s_production");

            return new SourceBoxSupplier(idLine, s_production);
        }

        public static void SetSystemProduction(SystemProductionPaletizado production)
        {
            if (production == null)
                throw new ArgumentNullException("production");
            s_production = production;
        }

        public bool IsCatalogReady()
        {
            return _production.IsCatalogReady(_idLine);
        }

        public DatosCatalogoPaletizado GetCatalog()
        {
            if (!IsCatalogReady())
                return null;
            else return _production.GetCatalogManager(_idLine).Catalog;
        }

        private int GetLastBoxIndexStored()
        {
            DatosCatalogoPaletizado catalog = GetCatalog();

            StoreStateCatalog storedData = catalog.SotoredData;
            if (storedData == null)
                return -1;

            CatalogManager manager = _production.GetCatalogManager(_idLine);

            CajaPasaportes lastbox = storedData
                .GetAllBoxes()
                .Select((id) => manager.GetBox(id))
                .OrderByDescending(b => b.CatalogIndex).FirstOrDefault();

            if (lastbox == null)
                return 0;

            return lastbox.CatalogIndex;
        }

        private void InitCurrentBoxIndex()
        {
            if (_currentIndex == -1)
            {
                CatalogManager manager = _production.GetCatalogManager(_idLine);
                if (manager == null)
                    return;
                int lastBoxIndex = GetLastBoxIndexStored();
                _currentIndex = (_invertedOrder) ? manager.BoxesCount - lastBoxIndex - 1 : lastBoxIndex + 1;
            }
        }
    }
}