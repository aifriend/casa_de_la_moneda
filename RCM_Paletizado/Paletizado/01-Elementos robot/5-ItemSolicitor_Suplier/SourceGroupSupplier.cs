using System;
using System.Linq;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    public class SourceGroupSupplier : IItemSuplier<GrupoPasaportes>
    {
        private static SystemProductionPaletizado s_production;
        private readonly IDLine _idLine;
        private readonly bool _invertedOrder;
        private readonly SystemProductionPaletizado _production;
        private int _currentIndex;

        private SourceGroupSupplier(IDLine idLine, SystemProductionPaletizado production)
        {
            _production = production;
            _idLine = idLine;
            _currentIndex = -1; //En la inicializacion se pone a -1 para extraer el índice del catálogo

            //_invertedOrder = false;//OJO. MDG.2010-11-30. Puenteo para probar numeracion incremental
            _invertedOrder = (_idLine == IDLine.Alemana) ? true : false; //Numeracion decremental solo linea alemana
        }

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set { _currentIndex = value; }
        }

        #region IItemSuplier<GrupoPasaportes> Members

        public GrupoPasaportes QuitItem()
        {
            if (!_production.IsCatalogReady(_idLine)) return null;
            CatalogManager catalogManager = _production.GetCatalogManager(_idLine);
            InitCurrentGroupIndex();

            if (_idLine == IDLine.Alemana)
            {
                if (_invertedOrder)
                {
                    if (_currentIndex >= 0)
                        return catalogManager.GetGroup(_currentIndex--);
                }
                else //MDG.2010-11-30.Consideramos orden incremental para la linea 2 tambien
                {
                    //if (_currentIndex < catalogManager.BoxesCount)
                    if (_currentIndex < (catalogManager.BoxesCount*CajaPasaportes.NGrupos))
                        return catalogManager.GetGroup(_currentIndex++);
                }
            }
            else
            {
                if (_currentIndex < catalogManager.BoxesCount)
                    return catalogManager.GetGroup(_currentIndex++);
            }
            return null;
        }

        #endregion

        public static SourceGroupSupplier Create(IDLine idLine)
        {
            if (s_production == null)
                throw new InvalidOperationException("s_production");
            return new SourceGroupSupplier(idLine, s_production);
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

        public CajaPasaportes GetBox(string id)
        {
            CatalogManager manager = _production.GetCatalogManager(_idLine);
            if (manager == null) return null;
            return manager.GetBox(id);
        }

        public GrupoPasaportes GetItem()
        {
            InitCurrentGroupIndex();
            return GetItem(CurrentIndex);
        }

        public GrupoPasaportes GetItem(int itemIndex)
        {
            if (!_production.IsCatalogReady(_idLine)) return null;
            CatalogManager catalogManager = _production.GetCatalogManager(_idLine);

            try
            {
                if (_idLine == IDLine.Alemana)
                {
                    if (_invertedOrder)
                    {
                        if (itemIndex == 0)
                            catalogManager.Estado = CatalogManagerStates.UltimoGrupo; //2016. MCR
                        if (itemIndex >= 0)
                            return catalogManager.GetGroup(itemIndex);
                    }
                    else //MDG.2010-11-30.Consideramos orden incremental para la linea 2 tambien
                    {
                        if (itemIndex < (catalogManager.BoxesCount*CajaPasaportes.NGrupos))
                            return catalogManager.GetGroup(itemIndex);
                    }
                }
                else
                {
                    if (itemIndex < catalogManager.BoxesCount * CajaPasaportes.NGrupos)
                        return catalogManager.GetGroup(itemIndex);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        //MDG.2011-07-05
        public GrupoPasaportes GetItemFromId(string ItemId)
        {
            if (!_production.IsCatalogReady(_idLine)) return null;
            CatalogManager catalogManager = _production.GetCatalogManager(_idLine);

            try
            {
                return catalogManager.GetGroupFromId(ItemId);
            }
            catch
            {
                return null;
            }
        }

        //MDG.2011-07-05
        public bool ContainsGroupId(string ItemId)
        {
            if (!_production.IsCatalogReady(_idLine)) return false;
            CatalogManager catalogManager = _production.GetCatalogManager(_idLine);

            try
            {
                GrupoPasaportes group = catalogManager.GetGroupFromId(ItemId);
                return (group != null);
            }
            catch
            {
                return false;
            }
        }

        private int GetLastGroupIndexStored()
        {
            DatosCatalogoPaletizado catalog = GetCatalog();

            StoreStateCatalog storedData = catalog.SotoredData;
            if (storedData == null)
                return 0;

            CatalogManager manager = _production.GetCatalogManager(_idLine);

            CajaPasaportes lastbox = null;

            if (_idLine == IDLine.Alemana) //MDG.2010-11-30.Numeracion invertida sólo habilitada en linea 2 
            {
                if (_invertedOrder)
                    //MDG.2010-11-30.Buscamos en orden inverso si la linea 2 esta con numeracion invertida
                {
                    lastbox = storedData
                        .GetAllBoxes()
                        .Select((id) => manager.GetBox(id))
                        .OrderBy(b => b.CatalogIndex).FirstOrDefault(); //se coge la caja con el catalog index mas bajo
                }
                else
                {
                    lastbox = storedData
                        .GetAllBoxes()
                        .Select((id) => manager.GetBox(id))
                        .OrderByDescending(b => b.CatalogIndex).FirstOrDefault();
                    //se coge la caja con el catalog index mas alto
                }
            }
            else
            {
                lastbox = storedData
                    .GetAllBoxes()
                    .Select((id) => manager.GetBox(id))
                    .OrderByDescending(b => b.CatalogIndex).FirstOrDefault();
            }

            if (lastbox == null)
                return 0;

            if (_idLine == IDLine.Alemana) //MDG.2010-11-30.Numeracion invertida sólo habilitada en linea 2 
            {
                if (_invertedOrder)
                    return lastbox.CatalogIndex*CajaPasaportes.NGrupos; //old
                    //return (lastbox.CatalogIndex - 1) * CajaPasaportes.NGrupos;//MDG.2011-05-10
                else
                    return (lastbox.CatalogIndex + 1)*CajaPasaportes.NGrupos; //MDG.2010.11.30.Numeracion directa
            }
            else //japonesa
            {
                return (lastbox.CatalogIndex+1)*CajaPasaportes.NGrupos; //old, japonesa
            }
        }

        public void GetNewIndexForced()
        {
            InitCurrentGroupIndexForced();
        }

        private void InitCurrentGroupIndex()
        {
            if (!_production.IsCatalogReady(_idLine)) return; //2012-04-24.Comprobacion
            if(_production.IsLastGroupIntroduced(_idLine)) return; //2016 MCR
            if (_currentIndex != -1) return; //solo se ejecuta si el indice no esta inicializado(si vale -1)
            CatalogManager manager = _production.GetCatalogManager(_idLine);
            if (manager == null) return;
            int lastGroupIndex = GetLastGroupIndexStored();
            //OLd 2010-12-09. Cogemos el indice de laultima caja almacenada

            if ((lastGroupIndex <= 0) && _invertedOrder) //caso inicial, sin cajas depositadas
            {
                _currentIndex = manager.GroupsCount - lastGroupIndex - 1;
            }
            else //siguientes cajas
            {
                //_currentIndex = (_invertedOrder) ? manager.GroupsCount - lastGroupIndex - 1 : lastGroupIndex;
                _currentIndex = (_invertedOrder) ? lastGroupIndex - 1 : lastGroupIndex;
            }
        }

        private void InitCurrentGroupIndexForced()
        {
            if (!_production.IsCatalogReady(_idLine)) return; //2012-04-24.Comprobacion
            //if (_currentIndex != -1) return; //solo se ejecuta si el indice no esta inicializado(si vale -1)
            CatalogManager manager = _production.GetCatalogManager(_idLine);
            if (manager == null) return;
            int lastGroupIndex = GetLastGroupIndexStored();
            //OLd 2010-12-09. Cogemos el indice de laultima caja almacenada

            if ((lastGroupIndex <= 0) && _invertedOrder) //caso inicial, sin cajas depositadas
            {
                _currentIndex = manager.GroupsCount - lastGroupIndex - 1;
            }
            else //siguientes cajas
            {
                //_currentIndex = (_invertedOrder) ? manager.GroupsCount - lastGroupIndex - 1 : lastGroupIndex;
                _currentIndex = (_invertedOrder) ? lastGroupIndex - 1 : lastGroupIndex;
            }
        }


        public int currentIndexCreating;

        public void GetNewIndexCreating(CajaPasaportes box)
        {
            if (!_production.IsCatalogReady(_idLine)) return; //2012-04-24.Comprobacion
            //if (_currentIndex != -1) return; //solo se ejecuta si el indice no esta inicializado(si vale -1)
            CatalogManager manager = _production.GetCatalogManager(_idLine);
            if (manager == null) return;
            int lastGroupIndexCreating = InitCurrentGroupIndexCreating(box);
            //OLd 2010-12-09. Cogemos el indice de laultima caja almacenada

            if ((lastGroupIndexCreating <= 0) && _invertedOrder) //caso inicial, sin cajas depositadas
            {
                currentIndexCreating = manager.GroupsCount - lastGroupIndexCreating - 1;
            }
            else //siguientes cajas
            {
                //_currentIndex = (_invertedOrder) ? manager.GroupsCount - lastGroupIndex - 1 : lastGroupIndex;
                currentIndexCreating = (_invertedOrder) ? lastGroupIndexCreating - 1 : lastGroupIndexCreating;
            }
        }

        //public void forceCurrentIndex(int i)
        //{
        //    _currentIndexCreating = i;
        //}

        private int InitCurrentGroupIndexCreating(CajaPasaportes lastbox)
        {
            //DatosCatalogoPaletizado catalog = GetCatalog();

            //StoreStateCatalog storedData = catalog.SotoredData;
            //if (storedData == null)
            //    return 0;

            //CatalogManager manager = _production.GetCatalogManager(_idLine);

            if (lastbox == null)
                return 0;

            if (_idLine == IDLine.Alemana) //MDG.2010-11-30.Numeracion invertida sólo habilitada en linea 2 
            {
                if (_invertedOrder)
                    return lastbox.CatalogIndex * CajaPasaportes.NGrupos; //old
                //return (lastbox.CatalogIndex - 1) * CajaPasaportes.NGrupos;//MDG.2011-05-10
                else
                    return (lastbox.CatalogIndex + 1) * CajaPasaportes.NGrupos; //MDG.2010.11.30.Numeracion directa
            }
            else //japonesa
            {
                return (lastbox.CatalogIndex + 1) * CajaPasaportes.NGrupos; //old, japonesa
            }

        }
    }
}