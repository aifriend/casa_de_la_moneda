using System;
using System.Linq;
using Idpsa.Control.Subsystem;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    [Serializable]
    public class SystemProductionPaletizado : IOriginDefiner
    {
        #region IdNotification enum

        public enum IdNotification
        {
            CatalogChanged
        }

        #endregion

        private readonly IdpsaSystemPaletizado _sys;

        public SystemProductionPaletizado(IdpsaSystemPaletizado Sys)
        {
            _sys = Sys;
            SourceBoxSupplier.SetSystemProduction(this);
            SourceGroupSupplier.SetSystemProduction(this);
        }


        public CatalogManager CatalogManagerJaponesa { get; private set; }
        public CatalogManager CatalogManagerAlemana { get; private set; }

        #region IOriginDefiner Members

        public bool InOrigin()
        {
            return IsCatalogReady(IDLine.Japonesa) || IsCatalogReady(IDLine.Alemana);
        }

        #endregion

        [field: NonSerialized]
        public event EventHandler<eventNotificationArgs> NewNotification;

        public DatosCatalogoPaletizado GetCatalog(IDLine value)
        {
            CatalogManager catalogManager = GetCatalogManager(value);
            if (catalogManager == null) return null;
            return catalogManager.Catalog;
        }


        private void GetCatalogManager(IDLine idLine, out CatalogManager value)
        {
            value = null;
            if (idLine == IDLine.Japonesa) value = CatalogManagerJaponesa;
            else if (idLine == IDLine.Alemana) value = CatalogManagerAlemana;
        }

        public CatalogManager GetCatalogManager(IDLine value)
        {
            if (value == IDLine.Japonesa) return CatalogManagerJaponesa;
            if (value == IDLine.Alemana) return CatalogManagerAlemana;

            return null;
        }

        private void SaveCatalog(Line line, CatalogManager catalogManager)
        {
            if (catalogManager != null)
                catalogManager.SaveCatalog(line.GetDataToStore());
        }

        public void SaveCatalogs()
        {
            if (ConfigPaletizadoFiles.IntercambioDestinoEntradas == false)
                //MDG.2010-07-13.Cambio asignacion entradas y salidas. 
            {
                //Asignacion original: Entrada Japonesa a linea 1 y Entrada Alemana a Linea 2
                SaveCatalog(_sys.Lines.Line1, CatalogManagerJaponesa);
                SaveCatalog(_sys.Lines.Line2, CatalogManagerAlemana);
            }
            else
            {
                //MDG.2010-07-13.Cambio asignacion entradas y salidas. Entradas cruzadas
                //Entrada Japonesa a linea 2 y Entrada Alemana a Linea 1
                SaveCatalog(_sys.Lines.Line1, CatalogManagerAlemana);
                SaveCatalog(_sys.Lines.Line2, CatalogManagerJaponesa);
            }
        }

        //MDG.2010-12-02.Salvado de grupos en los transportes.LINEA 2
        public void SaveTransportGroups()
        {
            //MDG.2010-12-02.Salvado empezando por el final de la linea
            if (CatalogManagerAlemana != null)
            {
                SaveElevator2Group(CatalogManagerAlemana);
                SaveTramoTransporte2Groups(CatalogManagerAlemana);
                SaveTramoTransporte1Groups(CatalogManagerAlemana);
                SaveElevator1Group(CatalogManagerAlemana);
            }
            else if (CatalogManagerJaponesa != null) //MDG.2010-12-13. Salvado tambien con linea japonesa
            {
                SaveElevator2Group(CatalogManagerJaponesa);
                SaveTramoTransporte2Groups(CatalogManagerJaponesa);
                SaveTramoTransporte1Groups(CatalogManagerJaponesa);
                SaveElevator1Group(CatalogManagerJaponesa);
            }
        }

        public void SaveElevator1Group(CatalogManager catalogManager)
        {
            SaveElevator1Group(_sys.Lines.Line2.TransporteLinea.Elevador1, catalogManager);
        }

        public void SaveTramoTransporte1Groups(CatalogManager catalogManager)
        {
            SaveTramoTransporteGroup(_sys.Lines.Line2.TransporteLinea.Tramo1, catalogManager, "Tramo 1");
        }

        public void SaveTramoTransporte2Groups(CatalogManager catalogManager)
        {
            SaveTramoTransporteGroup(_sys.Lines.Line2.TransporteLinea.Tramo2, catalogManager, "Tramo 2");
        }

        public void SaveElevator2Group(CatalogManager catalogManager)
        {
            SaveElevator2Group(_sys.Lines.Line2.TransporteLinea.Elevador2, catalogManager);
        }

        private void SaveElevator1Group(Elevador1 elevator, CatalogManager catalogManager)
        {
            if (catalogManager != null)
                catalogManager.SaveElevator1Data(elevator.GetDataToStore());
        }

        private void SaveTramoTransporteGroup(TramoTransporteGruposPasaportes tramoTransporte,
                                              CatalogManager catalogManager, string tramoName)
        {
            if (catalogManager != null)
                catalogManager.SaveTramoTransporteData(tramoTransporte.GetDataToStore());
        }

        private void SaveElevator2Group(Elevador2 elevator, CatalogManager catalogManager)
        {
            if (catalogManager != null)
                catalogManager.SaveElevator2Data(elevator.GetDataToStore());
        }


        //MDG.2010-12-02. Carga de los grupos en los transportes
        public void LoadTransportGroups()
        {
            //MDG.2010-12-02. Carga de los grupos empezando por el final de la linea
            if (CatalogManagerAlemana != null)
            {
                LoadElevator2Group(_sys.Lines.Line2.TransporteLinea.Elevador2, CatalogManagerAlemana);
                LoadTransportGroups(_sys.Lines.Line2.TransporteLinea.Tramo2, CatalogManagerAlemana);
                LoadTransportGroups(_sys.Lines.Line2.TransporteLinea.Tramo1, CatalogManagerAlemana);
                LoadElevator1Group(_sys.Lines.Line2.TransporteLinea.Elevador1, CatalogManagerAlemana);
            }
            else if (CatalogManagerJaponesa != null) //MDG.2010-12-13. Carga tambien con linea japonesa
            {
                LoadElevator2Group(_sys.Lines.Line2.TransporteLinea.Elevador2, CatalogManagerJaponesa);
                LoadTransportGroups(_sys.Lines.Line2.TransporteLinea.Tramo2, CatalogManagerJaponesa);
                LoadTransportGroups(_sys.Lines.Line2.TransporteLinea.Tramo1, CatalogManagerJaponesa);
                LoadElevator1Group(_sys.Lines.Line2.TransporteLinea.Elevador1, CatalogManagerJaponesa);
            }
        }

        private void LoadElevator1Group(Elevador1 elevator, CatalogManager catalogManager)
        {
            if (catalogManager != null)
                elevator.SetDataStored(catalogManager.LoadElevator1Data());
        }

        private void LoadTransportGroups(TramoTransporteGruposPasaportes tramoTransporte, CatalogManager catalogManager)
        {
            if (catalogManager != null)
                tramoTransporte.SetDataStored(catalogManager.LoadTramoTransporteData(tramoTransporte.Name));
        }

        private void LoadElevator2Group(Elevador2 elevator, CatalogManager catalogManager)
        {
            if (catalogManager != null)
                elevator.SetDataStored(catalogManager.LoadElevator2Data());
        }

        //MDG.2010-12-10.Salvado de grupos y caja comunes a las 2 lineas
        public void SaveLinesGroupsAndBoxes()
        {
            //MDG.2010-12-10.Salvado empezando por el final de la linea
            if (CatalogManagerAlemana != null)
            {
                SaveBandaReprocesoBox(CatalogManagerAlemana);
                SaveBandaEtiquetadoBox(CatalogManagerAlemana);
                SaveProdecGroups(CatalogManagerAlemana);
                SaveBandaSalidaEnfajadoraGroups(CatalogManagerAlemana);
            }
            else if (CatalogManagerJaponesa != null) //MDG.2010-12-13. Salvado tambien con linea japonesa
            {
                SaveBandaReprocesoBox(CatalogManagerJaponesa);
                SaveBandaEtiquetadoBox(CatalogManagerJaponesa);
                SaveProdecGroups(CatalogManagerJaponesa);
                SaveBandaSalidaEnfajadoraGroups(CatalogManagerJaponesa);
            }
        }

        public void SaveBandaSalidaEnfajadoraGroups(CatalogManager catalogManager)
        {
            SaveBandaSalidaEnfajadoraGroups(_sys.Lines.BandaSalidaEnfajadora, catalogManager);
        }

        private void SaveBandaSalidaEnfajadoraGroups(BandaSalidaEnfajadora banda, CatalogManager catalogManager)
        {
            if (catalogManager != null)
                catalogManager.SaveBandaSalidaEnfajadoraData(banda.GetDataToStore());
        }

        public void SaveProdecGroups(CatalogManager catalogManager)
        {
            SaveProdecGroups(_sys.Lines.Encajadora, catalogManager);
        }

        private void SaveProdecGroups(Prodec banda, CatalogManager catalogManager)
        {
            if (catalogManager != null)
                catalogManager.SaveProdecData(banda.GetDataToStore());
        }

        public void SaveBandaEtiquetadoBox(CatalogManager catalogManager)
        {
            SaveBandaEtiquetadoBox(_sys.Lines.BandaEtiquetado, catalogManager);
        }

        private void SaveBandaEtiquetadoBox(BandaEtiquetado banda, CatalogManager catalogManager)
        {
            if (catalogManager != null)
                catalogManager.SaveBandaEtiquetadoData(banda.GetDataToStore());
        }

        public void SaveBandaReprocesoBox(CatalogManager catalogManager)
        {
            SaveBandaReprocesoBox(_sys.Lines.ManualReprocesor, catalogManager);
        }

        private void SaveBandaReprocesoBox(ReprocesadorManual banda, CatalogManager catalogManager)
        {
            if (catalogManager != null)
                catalogManager.SaveBandaReprocesoData(banda.GetDataToStore());
        }

        public void LoadLinesGroupsAndBoxes()
        {
            if (CatalogManagerAlemana != null)
            {
                //MDG.2010-12-10. Carga de los grupos empezando por el final de la linea
                LoadBandaReprocesoBox(_sys.Lines.ManualReprocesor, CatalogManagerAlemana);
                LoadBandaEtiquetadoBox(_sys.Lines.BandaEtiquetado, CatalogManagerAlemana);
                LoadProdecGroups(_sys.Lines.Encajadora, CatalogManagerAlemana);
                LoadBandaSalidaEnfajadoraGroups(_sys.Lines.BandaSalidaEnfajadora, CatalogManagerAlemana);
            }
            else if (CatalogManagerJaponesa != null) //MDG.2010-12-13. Carga tambien con linea japonesa
            {
                //MDG.2010-12-10. Carga de los grupos empezando por el final de la linea
                LoadBandaReprocesoBox(_sys.Lines.ManualReprocesor, CatalogManagerJaponesa);
                LoadBandaEtiquetadoBox(_sys.Lines.BandaEtiquetado, CatalogManagerJaponesa);
                LoadProdecGroups(_sys.Lines.Encajadora, CatalogManagerJaponesa);
                LoadBandaSalidaEnfajadoraGroups(_sys.Lines.BandaSalidaEnfajadora, CatalogManagerJaponesa);
            }
        }

        private void LoadBandaSalidaEnfajadoraGroups(BandaSalidaEnfajadora banda, CatalogManager catalogManager)
        {
            if (catalogManager != null)
                banda.SetDataStored(catalogManager.LoadBandaSalidaEnfajadoraData());
        }

        private void LoadProdecGroups(Prodec banda, CatalogManager catalogManager)
        {
            if (catalogManager != null)
                banda.SetDataStored(catalogManager.LoadProdecData());
        }

        private void LoadBandaEtiquetadoBox(BandaEtiquetado banda, CatalogManager catalogManager)
        {
            if (catalogManager != null)
                banda.SetDataStored(catalogManager.LoadBandaEtiquetadoData());
        }

        private void LoadBandaReprocesoBox(ReprocesadorManual banda, CatalogManager catalogManager)
        {
            if (catalogManager != null)
                banda.SetDataStored(catalogManager.LoadBandaReprocesoData());
        }


        ///
        private void SetCatalogManager(DatosCatalogoPaletizado catalog)
        {
            var manager = new CatalogManager(catalog);
            switch (catalog.IDLine)
            {
                case IDLine.Japonesa:
                    CatalogManagerJaponesa = manager;
                    break;
                case IDLine.Alemana:
                    CatalogManagerAlemana = manager;
                    break;
            }
        }

        private bool SetNewCatalogManager(DatosCatalogoPaletizado catalog)
        {
            CatalogManager catalogManager = GetCatalogManager(catalog.IDLine);
            Line line = _sys.Lines.GetLine(catalog.IDLine);
            SaveCatalog(line, catalogManager);

            SetCatalogManager(catalog);
            _sys.Lines.SetNewCatalog(catalog);
            return true;
        }

        public bool TryGetPassport(string id, out Pasaporte p)
        {
            p = null;
            if (CatalogManagerJaponesa != null)
                return CatalogManagerJaponesa.TryGetPassPort(id, out p);
            else if (CatalogManagerAlemana != null)
                return CatalogManagerAlemana.TryGetPassPort(id, out p);
            else
                return false;
        }

        public Pasaporte GetPassport(string id)
        {
            Pasaporte p = null;
            if (TryGetPassport(id, out p))
                return p;
            else
                return null;
        }

        private void LoadCatalogs()
        {
            SetNewCatalogManager(ProveCatalog.Catalog);
        }


        private void OnNewNotification(eventNotificationArgs e)
        {
            if (NewNotification != null)
                NewNotification(this, e);
        }

        private void OnNewNotification(IdNotification idNotification)
        {
            if (NewNotification != null)
                NewNotification(this, new eventNotificationArgs(idNotification, new object()));
        }

        public void ActionRequestedHandler(SystemProductionRequest action)
        {
            switch (action.Id)
            {
                case SystemProductionRequest.IdRequest.Catalog:
                    var catalog = (DatosCatalogoPaletizado) action.Value;
                    if (SetNewCatalogManager(catalog))
                    {
                        OnNewNotification(new eventNotificationArgs(
                                              IdNotification.CatalogChanged, action.Value));
                    }
                    break;
            }
        }

        public bool IsCatalogReady(IDLine value)
        {
            CatalogManager catalogManager = GetCatalogManager(value);
            return (catalogManager != null && catalogManager.Catalog != null &&
                    catalogManager.Estado != CatalogManagerStates.Finalizado);
        }

        public bool IsLastGroupIntroduced(IDLine value) //MCR. 2016
        {
            CatalogManager catalogManager = GetCatalogManager(value);
            return (catalogManager != null &&
                    catalogManager.Estado == CatalogManagerStates.UltimoGrupo);
        }

        public CajaPasaportes GetBox(string idCaja)
        {
            CajaPasaportes caja = null;
            if (CatalogManagerJaponesa != null &&
                (caja = CatalogManagerJaponesa.GetBox(idCaja)) != null)
            {
                return caja;
            }
            else if (CatalogManagerAlemana != null &&
                     (caja = CatalogManagerAlemana.GetBox(idCaja)) != null)
            {
                return caja;
            }
            return null;
        }

        //MDG.2011-06-20
        private bool IsBoxIDInCatalog(Line line, CatalogManager catalogManager, string IDCaja)
        {
            if (catalogManager != null)
            {
                StoreStateCatalog data = line.GetDataToStore();
                if (data != null)
                    if (data.GetAllBoxes().Contains(IDCaja))
                        return true;

                return false;
            }
            return false;
        }

        public bool IsBoxIDInCatalogs(string IDCaja)
        {
            if (ConfigPaletizadoFiles.IntercambioDestinoEntradas == false)
                //MDG.2010-07-13.Cambio asignacion entradas y salidas. 
            {
                //Asignacion original: Entrada Japonesa a linea 1 y Entrada Alemana a Linea 2
                if (IsBoxIDInCatalog(_sys.Lines.Line1, CatalogManagerJaponesa, IDCaja) ||
                    IsBoxIDInCatalog(_sys.Lines.Line2, CatalogManagerAlemana, IDCaja))
                    return true;
                else
                    return false;
            }
            else
            {
                //MDG.2010-07-13.Cambio asignacion entradas y salidas. Entradas cruzadas
                //Entrada Japonesa a linea 2 y Entrada Alemana a Linea 1
                if (IsBoxIDInCatalog(_sys.Lines.Line1, CatalogManagerAlemana, IDCaja) ||
                    IsBoxIDInCatalog(_sys.Lines.Line2, CatalogManagerJaponesa, IDCaja))
                    return true;
                else
                    return false;
            }
        }

        #region Nested type: eventNotificationArgs

        public class eventNotificationArgs : EventArgs
        {
            public eventNotificationArgs(IdNotification valueChanged, object value)
            {
                ValueChanged = valueChanged;
                Value = value;
            }

            public IdNotification ValueChanged { get; private set; }
            public object Value { get; private set; }
        }

        #endregion
    }
}