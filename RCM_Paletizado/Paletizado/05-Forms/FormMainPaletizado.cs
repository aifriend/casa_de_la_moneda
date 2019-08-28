using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Diagnosis;
using Idpsa.Control.Engine;
using Idpsa.Control.Manuals;
using Idpsa.Control.Sequence;
using Idpsa.Control.Subsystem;
using Idpsa.Control.Tool;
using Idpsa.Control.User;
using Idpsa.Control.View;
using Idpsa.Paletizado;
using RCMCommonTypes;

namespace Idpsa
{
    public partial class FormMainPaletizado : FormMain, IDelegateTasksOwner, IViewTasksOwner
    {
        #region Propiedades

        private readonly FrmCreatePaletizerCatalog _editorCatalogoPaletizado = new FrmCreatePaletizerCatalog();
        private readonly FormPaletizado _editorPaletizado = new FormPaletizado();
        private readonly FrmCreatePasaporte _editorTipoPasaporte = new FrmCreatePasaporte();
        private readonly FormAlarmsHistorical _sfrmHistoricoAlarmas = new FormAlarmsHistorical();
        private SystemControllerPaletizado _controllerSystem;
        //private FormAccess _formAcceso;
        private RearmeBarrera _formAcceso;
        private FormUsersAdministrator _formAdministrador;
        private FormChains _formCadenas;
        private frmDIOSignals _formDioSignal;
        private FormParkerProfibus _formParker;
        private FormPrintBoxManual _formPrintBox;
        private FormVariador _formVariador;
        private FormAccessParker auxContraseña; //MCR
        private FormErrorDespaletizing _formErrorDespaletizado;//MDG.2013-04-30
        private bool cadenaRota; //MCR

        private datosTurno datosProduccion;
        private datosTurno datosVisualReport;
        private bool loading;
        private int CatDBL1;
        private int CatDBL2;

        private ManualManager _manualManager;

        private OcrPassPorts _ocrPassPorts;
        private PaletizerLine2View _paletizerLine2View;
        private PaletizersLine1View _paletizersLine1View;


        private string _refreshGrupoAnteriorManual;
        private string _refreshSuperGrupoAnteriorManual;
        private IdpsaSystemPaletizado _sys;

        private TON _timer = new TON();
        private TransportsView _transportsView;
        private FormBandaProdecView _formBandaProdecView;
        private Thread _treadViewTasks;
        private UBascula _uBascula;
        private UCLoadCatalogs _ucLoadCatalogs;

        private List<Action> _viewTasks;

        #endregion

        #region Contructores

        public FormMainPaletizado()
        {
            InitializeComponent();
            Initializate();
        }

        private void Initializate()
        {
            loading = true;
            _sys = ControlLoop<IdpsaSystemPaletizado>.Instance.Sys;
            _sys.Lines.DeactivateLines();

            _manualManager = ManualManager.Create(TreeViewLevels.Two, pnlMovManual, treeManual, _sys,
                                                  new ManualControlPaletizadoFactory())
                .WithTreeViewItemHeight(70).WithTreeViewSizeFont(12);
            _controllerSystem = new SystemControllerPaletizado(_sys);

            var printer = new Zebra_CajasPrinter_10_100_PrintServer("Etiquetadora manual", "ZebraTLP2844");
            manualBoxReprocessor.SetBoxGetter((id) => _sys.Production.GetBox(id));
            manualBoxReprocessor.SetPrinter(printer);

            TieManualReprocessorAndSolicitor();

            IniChildForms();
            _sys.barreraRearme.luz.Activate(true);
        }

        private void TieManualReprocessorAndSolicitor()
        {
            _sys.Lines.ManualReprocesor.AttachToReprocessor(manualBoxReprocessor);
            manualBoxReprocessor.AttachToSolicitor(_sys.Lines.ManualReprocesor);
        }

        private void InitializateLoaderCatalog()
        {
            _ucLoadCatalogs = new UCLoadCatalogs().Initialize(_sys); //MDG.2011-06-16
            panel9.Controls.Add(_ucLoadCatalogs);
            _ucLoadCatalogs.Location = new Point((panel9.Width - _ucLoadCatalogs.Width)/2,
                                                 (panel9.Height - _ucLoadCatalogs.Height)/2);
        }

        private void InitializeBandaProdecViews()
        {
            Panel container1 = pBandaProdec;
            container1.Controls.Add(_formBandaProdecView);
            _formBandaProdecView.Location =
                new Point(container1.Location.X + (container1.Size.Width - _formBandaProdecView.Width) / 2,
                          container1.Location.Y + (container1.Size.Height - _formBandaProdecView.Height) / 2 - 15);
        }

        private void InitializePaletizersViews()
        {
            _paletizersLine1View = new PaletizersLine1View();
            _paletizersLine1View.SubscribeLine1(_sys.Lines.Line1);
            _paletizerLine2View = new PaletizerLine2View();
            _paletizerLine2View.SubscribeLine2(_sys.Lines.Line2);
            //_paletizerLine2View.SubscribeLines(Sys.Lines);
            _transportsView = new TransportsView(); //MDG.2011-06-30
            _transportsView.SubscribeLine2(_sys.Lines.Line2);
            _transportsView.SubscribeLines(_sys.Lines);
            _transportsView.SubscribeControllerSystem(_controllerSystem);//MDG.2012-07-23
            _transportsView.SubscribeControl(_sys.Control);//MDG.2012-07-23
            _transportsView.TieToSysControl();//MDG.2012-07-23

            Panel container1 = PnlPaletizadoLine1;
            container1.Controls.Add(_paletizersLine1View);
            _paletizersLine1View.Location =
                new Point(container1.Location.X + (container1.Size.Width - _paletizersLine1View.Width)/2,
                          container1.Location.Y + (container1.Size.Height - _paletizersLine1View.Height)/2 - 15);
            //MDG.2011-06-16.Para centrar mejor los controles//- 80);

            Panel container2 = PnlPaletizadoLine2;
            container2.Controls.Add(_paletizerLine2View);
            _paletizerLine2View.Location =
                new Point(container2.Location.X + (container2.Size.Width - _paletizerLine2View.Width)/2,
                          container2.Location.Y + (container2.Size.Height - _paletizerLine2View.Height)/2 - 15);
            //MDG.2010-11-24.Para centrar mejor los controles//- 80);
            //MDG.2011-06-30
            Panel container3 = PnlTransports;
            container3.Controls.Add(_transportsView);
            _transportsView.Location =
                new Point(container2.Location.X + (container2.Size.Width - _transportsView.Width)/2,
                          container2.Location.Y + (container2.Size.Height - _transportsView.Height)/2 - 15);
            //MDG.2010-11-24.Para centrar mejor los controles//- 80);
        }

        private void IniChildForms()
        {
            _formAcceso = new RearmeBarrera(_sys.barreraRearme);
            _formAdministrador = new FormUsersAdministrator();
            _formParker = new FormParkerProfibus(_sys.SpecialDevices.GetSpecialDevices<CompaxC3I20T11>());
            _formDioSignal = new frmDIOSignals(_sys.Bus);
            _formCadenas = new FormChains(_sys);
            _formBandaProdecView = new FormBandaProdecView(_sys);
            _formPrintBox =
                new FormPrintBoxManual(
                    new[] {_sys.Lines.BandaEtiquetado.Etiquetadora.ZebraPrinter, manualBoxReprocessor.GetPrinter()},
                    (id) => _sys.Production.GetBox(id));
            _formVariador = new FormVariador(new SiemensMicromaster420Collection());
            _formErrorDespaletizado=new FormErrorDespaletizing(_sys);//MDG.2013-04-30
        }

        #endregion

        #region Subscripciones a eventos

        private void TieChilds()
        {
            _editorTipoPasaporte.SetTypePasaportChanged += _editorCatalogoPaletizado.SetTypePasaportChangedHandler;
            _editorPaletizado.NewPaletizerDefinition += _editorCatalogoPaletizado.SetPaletizerDefinitionsChangedHandler;
            _editorCatalogoPaletizado.SetCatatogsChanged += _ucLoadCatalogs.SetCatalogsChangedHander;
            _ucLoadCatalogs.NewCatalog += NewCatalogHandler;
            _paletizersLine1View.AddBox += NewForcedBoxAddedHandler;
            _paletizerLine2View.AddBox += NewForcedBoxAddedHandler;
            _paletizersLine1View.QuitBox += NewForcedBoxQuittedHandler;
            _paletizerLine2View.QuitBox += NewForcedBoxQuittedHandler;
            _formAcceso.Confirmado += BarreraRearmada;//MCR

            _sys.Lines.Line2.TransporteLinea.Tramo1.GroupPutted += NewGroupAereoEventHandler;
            _sys.Lines.Line2.TransporteLinea.Tramo2.GroupQuitted += QuittedGroupAereoEventHandler;
            _sys.Lines.Encajadora.GroupAdded += GroupAddedToProdec;
            _sys.Lines.Encajadora.BoxCreated += BoxCreatedHandler;
            _sys.Lines.Line1.Mesas.Mesa1.Paletizer.Changed += PaletizadoMesa1EventHandler;
            _sys.Lines.Line1.Mesas.Mesa2.Paletizer.Changed += PaletizadoMesa2EventHandler;
            _sys.Lines.Line1.ZonaPaletizadoFinal.Paletizer.Changed += PaletizadoFinalLinea1EventHandler;
            _sys.Lines.Line2.Paletizer.Changed += PaletizadoFinalLinea2EventHandler;
            _sys.barreraRearme.alarmaRFIDEvent += AlarmaRfidEventHandler;
        }

        private void TieToSys()
        {
            _sys.Control.NewNotification += NewSystemControlNotificationHandler;
            _sys.Production.NewNotification += NewProductionControlNotificationHandler;
        }

        private void TieToBD()
        {
            datosProduccion = new datosTurno();        //MCR
            datosProduccion.LoadDatos();
            _sys.Db.DataBaseChanged += DataBaseChangedHandler;
        }

        private void TieToDiagnosis()
        {
            DiagnosisManager.Instance.Items.CountChanged += DiagnosisActivaCountChangedHandler;
        }

        #endregion

        #region Carga de ventana

        private void frmPasaportes_Load(object sender, EventArgs e)
        {
            InitializateLoaderCatalog();
            InitializePaletizersViews();
            InitializeBandaProdecViews();
            InitializeRobotEnlaceManualMode();
            TieChilds();
            TieToSys();
            TieToBD();
            TieToDiagnosis();

            TabSystem.Alignment = TabAlignment.Left;
            TabSystem.DrawMode = TabDrawMode.OwnerDrawFixed;
            Size s = Size;
            pnlMovManual.Size = new Size(pnlMovManual.Size.Width, pnlMovManual.Parent.Size.Height);
            menuAdministrador.Visible = UserAccess.Instance.CurrentUser <= UserType.Administrador;
            _manualManager.LoadTreeView();
            ActualizarPermiso();
            lstDiagnosis.Items.Clear();
            lstDiagnosis.Height = 0;

            ResumeLayout();
            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }

        private void ActualizarPermiso()
        {
            if (!UserAccess.Instance.CurrentUserEqualOrSuperior(UserType.Mantenimiento))
            {
                menuSimbolico.Visible = false;
                menuPermiso.Visible = true;
                menuCadenas.Visible = false;
                menuAlarmas.Visible = false;
                menuParker.Visible = false;
                menuVariador.Visible = false;
                menuNuevo.Visible = false;
            }
            else
            {
                menuSimbolico.Visible = true;
                menuPermiso.Visible = true;
                menuCadenas.Visible = true;
                menuAlarmas.Visible = true;
                menuParker.Visible = true;
                menuVariador.Visible = true;
                menuNuevo.Visible = true;
            }
            if (UserAccess.Instance.CurrentUserEqualOrSuperior(UserType.Administrador))
            {
                menuAdministrador.Visible = true;
            }
            else
            {
                menuAdministrador.Visible = false;
            }
        }

        #endregion

        #region Procesado cíclico

        private void RefreshEntradaProdecView()
        {
            switch (_sys.ManualFeedRuhlamat.State)
            {
                case SubsystemState.Activated:
                    btEntradaManual0.Text = "Entrada Prodec ACTIVA\n---\nRobotEnlace INACTIVO";
                    btEntradaManual0.BackColor = Color.LightGreen;
                    break;
                case SubsystemState.Deactivated:
                    btEntradaManual0.Text = "Entrada Prodec INACTIVA\n---\nRobotEnlace ACTIVO";
                    btEntradaManual0.BackColor = Color.DarkOrange;
                    break;
            }
        }

        private void RefreshManual()
        {
            //manualManager.RefreshManual(TabSystem.SelectedIndex == 3);          
            _manualManager.RefreshManual(TabSystem.SelectedIndex == 4);
            //MDG.2011-06-20.Como cambio el orden de las pestañas no funcionaba.Ahora si
        }

        private void RefreshVisuLinea2()
        {
            _paletizerLine2View.Refresh();
        }

        private void RefreshVisuTransports()
        {
            _transportsView.Refresh();
        }

        #endregion

        #region Notificaciones del modelo

        private void NewSystemControlNotificationHandler(object obj, SystemControl.EventNotificationArgs e)
        {
            switch (e.IdNotification)
            {
                case SystemControl.IdNotification.GlobalAddedStatusOk:
                    lblAireOk.BackColor = ((bool) e.Value) ? Color.Green : Color.Red;
                    break;
                case SystemControl.IdNotification.ConnectionCommand:
                    lblConexionMando.BackColor = ((bool) e.Value) ? Color.Green : Color.Red;
                    if ((bool)e.Value)
                        _sys.barreraRearme.maquinaIniciada = true;
                    break;
                case SystemControl.IdNotification.Diagnosis:
                    lblDiagnosis.BackColor = ((bool) e.Value) ? Color.Red : Color.Green;
                    break;
                case SystemControl.IdNotification.Origin:
                    lblOrigen.BackColor = ((bool) e.Value) ? Color.Green : Color.Red;
                    break;
                case SystemControl.IdNotification.ProtectionsCanceled:
                    lblProteccionesAnuladas.BackColor = ((bool) e.Value) ? Color.Red : Color.Green;
                    break;
                case SystemControl.IdNotification.ProtectionsOK:
                    lblProteccionesOk.BackColor = ((bool) e.Value) ? Color.Green : Color.Red;
                    break;
                case SystemControl.IdNotification.BootSystem:
                    lblSistemaArranque.BackColor = ((bool) e.Value) ? Color.Green : Color.Red;
                    break;
                case SystemControl.IdNotification.OperationMode:
                    lblModoActual.Text = e.Value.ToString();
                    _manualManager.EnableManualPanel((Mode) (e.Value) == Mode.Manual);
                    break;

                case SystemControl.IdNotification.ModeStatus:
                    var modeStatus = (ControlModeStatus) e.Value;
                    if (modeStatus == ControlModeStatus.Activated)
                    {
                        lblModoActual.BackColor = Color.FromArgb(150, 255, 150);
                    }
                    else if (modeStatus == ControlModeStatus.Deactivated)
                    {
                        lblModoActual.BackColor = Color.Khaki;
                    }
                    else if (modeStatus == ControlModeStatus.Stoped)
                    {
                        lblModoActual.BackColor = Color.Orange;
                    }
                    break;
            }
        }

        private void NewProductionControlNotificationHandler(object obj,
                                                             SystemProductionPaletizado.eventNotificationArgs e)
        {
            switch (e.ValueChanged)
            {
                case SystemProductionPaletizado.IdNotification.CatalogChanged:
                    var catalog = (DatosCatalogoPaletizado) e.Value;
                    _ucLoadCatalogs.SetCatalog(catalog);
                    Text = String.Format("RCM catálogos: línea 1 {0}, línea 2 {1}",
                                         (_ucLoadCatalogs.GetCatalog(IDLine.Japonesa) != null)
                                             ? _ucLoadCatalogs.GetCatalog(IDLine.Japonesa).Name
                                             : "No cargado",
                                         (_ucLoadCatalogs.GetCatalog(IDLine.Alemana) != null)
                                             ? _ucLoadCatalogs.GetCatalog(IDLine.Alemana).Name
                                             : "No cargado"
                        );
                    break;
            }
        }

        


        private void DiagnosisActivaCountChangedHandler()
        {
            if (DiagnosisManager.Instance.Items.Count > 0)
            {
                _sys.Control.Diagnosis = true;
                try
                {
                    foreach (GeneralDiagnosis diagnosis in DiagnosisManager.Instance.Items.AllValues)
                    {
                        if (!lstDiagnosis.Items.ContainsKey(diagnosis.Id))
                        {
                            var myLstItem = new ListViewItem(diagnosis.Name) {Name = diagnosis.Id};
                            myLstItem.SubItems.Add(diagnosis.ErrorMessage);
                            lstDiagnosis.Items.Add(myLstItem);
                        }
                    }

                    foreach (SecurityDiagnosis diagnosis in _sys.Signals.NotActuatedSecurities())
                    {
                        if (lstDiagnosis.Items.ContainsKey(diagnosis.Id))
                        {
                            lstDiagnosis.Items.RemoveByKey(diagnosis.Id);
                        }
                    }
                }
                catch
                {
                }
            }
            else
            {
                try
                {
                    _sys.Control.Diagnosis = false;
                    if (lstDiagnosis.Items.Count > 0) 
                        lstDiagnosis.Items.Clear();
                }
                catch
                {
                }
            }

            lstDiagnosis.Height = 20*DiagnosisManager.Instance.Items.Count;
        }


        private void DataBaseChangedHandler()
        {
            new Thread(RefreshGroups) {IsBackground = true}.Start();
        }

        private void NewForcedBoxAddedHandler(int i)
        {
            if (_sys.Lines.BandaSalidaEnfajadora.PassportGroup.Count > 0)
                MessageBox.Show(
                    "No se pueden hacer cambios mientras haya grupos en la banda de entrada a la encajadora Prodec",
                    "Añadir caja");
            else if (_sys.Lines.Encajadora.GruposPasaportes.Count > 0)
                MessageBox.Show(
                    "No se pueden hacer cambios mientras haya grupos dentro de la encajadora Prodec",
                    "Añadir caja");
            else
            {
                if (i==4)
                    _sys.Lines.Line2.ForceAddBox(i);
                else
                    _sys.Lines.Line1.ForceAddBox(i);
            }
        }

        private void NewForcedBoxQuittedHandler(int i)
        {
            if (_sys.Lines.BandaSalidaEnfajadora.PassportGroup.Count > 0)
                MessageBox.Show(
                    "No se pueden hacer cambios mientras haya grupos en la banda de entrada a la encajadora Prodec",
                    "Quitar caja");
            else if (_sys.Lines.Encajadora.GruposPasaportes.Count > 0)
                MessageBox.Show(
                    "No se pueden hacer cambios mientras haya grupos dentro de la encajadora Prodec",
                    "Quitar caja");
            else
            {
                if (i == 4)
                    _sys.Lines.Line2.ForceQuitBox(i);
                else
                    _sys.Lines.Line1.ForceQuitBox(i);
            }
        }

        private void NewGroupAereoEventHandler(object sender, DataEventArgs<GrupoPasaportes> gPas)
        {
            if (loading) return;
            GrupoPasaportes grupo = gPas.Data;
            if (CatDBL2 != -1 && datosProduccion.CatalogArray[CatDBL2] != null)
                if (datosProduccion.CatalogArray[CatDBL2].getEntrada() == Entrada.Ruhlamat && grupo != null && datosProduccion.CatalogArray[CatDBL2]._aereos!=null)
                    datosProduccion.CatalogArray[CatDBL2]._aereos.Add(grupo);            
        }
        private void QuittedGroupAereoEventHandler(object sender, DataEventArgs<GrupoPasaportes> gPas) 
        {
            if (loading) return;
            GrupoPasaportes grupo = gPas.Data;
            if (CatDBL2 != -1 && datosProduccion.CatalogArray[CatDBL2] != null)
                if (datosProduccion.CatalogArray[CatDBL2].getEntrada() == Entrada.Ruhlamat && grupo != null)
                    if (datosProduccion.CatalogArray[CatDBL2]._aereos != null && datosProduccion.CatalogArray[CatDBL2]._aereos.Count>0)
                    datosProduccion.CatalogArray[CatDBL2]._aereos.Remove(grupo); 
        }
        private void GroupAddedToProdec(GrupoPasaportes grupo) 
        {
            if (loading) return;
            int i=getActualCatalog();
            if (i != -1 && datosProduccion.CatalogArray[i] != null)
                if (grupo != null)
                    datosProduccion.CatalogArray[i].changeLastPassportProduced(grupo.IdsPasaportes(GrupoPasaportes.NMaximo).Last());
        }
        private void BoxCreatedHandler(CajaPasaportes caja) 
        {
            if (loading) return;
            int i = getActualCatalog();
            if (i != -1 && datosProduccion.CatalogArray[i] != null)
                if (caja != null)
                    datosProduccion.CatalogArray[i].changeLastBoxProduced(caja.Id);
        }
        private void PaletizadoMesa1EventHandler(object sender, PaletizerEventArgs e) 
        {
            if (loading) return;
            int i=CatDBL1;
            if (e.Boxes.Count() == 0) return;
            CajaPasaportes caja = (CajaPasaportes)e.Boxes.Last();

            if (i != -1 && datosProduccion.CatalogArray[i] != null)
                if (caja != null)
                    datosProduccion.CatalogArray[i].changeLastBoxPaletized(caja.Id,1);
        }
        private void PaletizadoMesa2EventHandler(object sender, PaletizerEventArgs e) 
        {
            if (loading) return;
            if (e.Boxes.Count() == 0) return;
            int i = CatDBL1;
            CajaPasaportes caja = (CajaPasaportes)e.Boxes.Last();

            if (i != -1 && datosProduccion.CatalogArray[i] != null)
                if (caja != null)
                    datosProduccion.CatalogArray[i].changeLastBoxPaletized(caja.Id, 2);
        }
        private void PaletizadoFinalLinea1EventHandler(object sender, PaletizerEventArgs e) 
        {
            if (loading) return;
            if (e.Boxes.Count() == 0) return;
            int i = CatDBL1;
            CajaPasaportes caja = (CajaPasaportes)e.Boxes.Last();

            if (i != -1 && datosProduccion.CatalogArray[i] != null)
                if (caja != null)
                    datosProduccion.CatalogArray[i].changeLastBoxPaletized(caja.Id, 3);
        }
        private void PaletizadoFinalLinea2EventHandler(object sender, PaletizerEventArgs e) {
            if (loading) return;
            if (e.Boxes.Count() == 0) return;
            int i = CatDBL2;
            CajaPasaportes caja = (CajaPasaportes)e.Boxes.Last();

            if (i != -1 && datosProduccion.CatalogArray[i] != null)
                if (caja != null)
                    datosProduccion.CatalogArray[i].changeLastBoxPaletized(caja.Id, 4);
        }   
   
        private void AlarmaRfidEventHandler(bool alarmOn)
        {
            if (alarmOn)
            {
                listAlarmaInfo.Show();
                var myLstItem = new ListViewItem("Fallo Rearme Barrera");
                myLstItem.SubItems.Add("Lector RFID no conectado");
                listAlarmaInfo.Items.Add(myLstItem);
            }
            else
            {
                listAlarmaInfo.Clear();
                listAlarmaInfo.Hide();
            }


        }

        private void RefreshGroups()
        {
            CheckForIllegalCrossThreadCalls = false;
            try
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<BDGrupoPasaportes>(g => g.Pasaportes);
                DataContext dataContext = _sys.Db.GetDataContext();
                dataContext.LoadOptions = loadOptions;

                Table<BDGrupoPasaportes> groupTable = dataContext.GetTable<BDGrupoPasaportes>();

                var query = from g in groupTable
                            select new
                                       {
                                           Grupo = new
                                                       {
                                                           g.ID,
                                                           Fajado = g.Fajado.ToString(),
                                                           FechaInicial = g.FechaInicial.ToString(),
                                                           FechaFinal =
                                g.FechaFinal.HasValue ? g.FechaFinal.Value.ToString() : ""
                                                       },
                                           Pasaportes = from p in g.Pasaportes
                                                        select new
                                                                   {
                                                                       p.ID,
                                                                       IDGRUPO = p.IDGrupo,
                                                                       RFID = p.RfID
                                                                   }
                                       };


                var list1 = new ArrayList();
                var list2 = new ArrayList();

                foreach (var item in query)
                {
                    list1.Add(item.Grupo);
                    list2.AddRange(item.Pasaportes.ToList());
                }

                //this.dataGridView1.DataSource = list1;
                //this.dataGridView2.DataSource = list2;               
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        #endregion

        #region Peticiones al modelo

        private void ActionQueryHandler(object sender, EventArgs e)
        {
            Object b = sender;
            if (b.Equals(btnActiveModeStart))
            {
                ////if ((_sys.Control.OperationMode == Mode.BackToOrigin))//MDG.2013-03-26
                ////{
                //    if (chkPasoAPaso.Checked)
                //    {
                //        _controllerSystem.StartModeWithStepByStep();
                //    }
                //    else
                //    {
                //        _controllerSystem.StartMode();
                //    }
                ////}
                if (!cadenaRota)
                {
                    _paletizersLine1View.ManualChange(false);
                    _paletizerLine2View.ManualChange(false);
                    _sys.Control.StartingAuto = true;
                }
            }
            else if (b.Equals(btnActiveModeStop))
            {
                _controllerSystem.StopModeAndDeativate();
                _paletizersLine1View.ManualChange(false);
                _paletizerLine2View.ManualChange(false);
                _sys.Control.StartingAuto = false;                
            }
            else if (b.Equals(btnAuto))
            {
                _controllerSystem.AutomaticMode();
                _paletizersLine1View.ManualChange(false);
                _paletizerLine2View.ManualChange(false);
                _sys.Control.StartingAuto = false;
            }
            else if (b.Equals(btnVOrig))
            {
                _controllerSystem.BackToOriginMode();
                _paletizersLine1View.ManualChange(false);
                _paletizerLine2View.ManualChange(false);
                _sys.Control.StartingAuto = false;
            }
            else if (b.Equals(btnManual))
            {
                _controllerSystem.ManualMode();
                _paletizersLine1View.ManualChange(true);
                _paletizerLine2View.ManualChange(true);
                _sys.Control.StartingAuto = false;
            }
            else if (b.Equals(btnRearme))
            {
                _controllerSystem.Rearmament();
                _paletizersLine1View.ManualChange(false);
                _paletizerLine2View.ManualChange(false);
                lstDiagnosis.Items.Clear();
                _sys.Control.StartingAuto = false;
            }
        }

        //private void NewCatalogHandler(DatosCatalogo catalogo)
        //{
        //    _controllerSystem.RequestSystemProduction(SystemProductionRequest.IdRequest.Catalog, catalogo);
        //}

        private void btnPaso_Click(object sender, EventArgs e)
        {
            Chain.ActivateStepInStepByStep(true);
        }

        private void NewCatalogHandler(object sender, DatosCatalogoPaletizado catalogo)
        {
            loading = true;
            //MDG.2011-05-30.Salvamos todos los grupos y cajas previo a la carga de los mismos de fichero
            //(Solo se salvan si ya hay algun catálogo cargado)
            _sys.Production.SaveLinesGroupsAndBoxes();
            _sys.Production.SaveTransportGroups();

            _controllerSystem.RequestSystemProduction(SystemProductionRequest.IdRequest.Catalog, catalogo);

            //MDG.2011-05-30.Salvamos todos los grupos y cajas previo a la carga de los mismos de fichero
            //(Solo se salvan si ya hay algun catálogo cargado)
            //Sys.Production.SaveLinesGroupsAndBoxes();
            //Sys.Production.SaveTransportGroups();

            if (catalogo != null)
            {
                int i;
                CatalogoDB newCat = new CatalogoDB(catalogo, getEntry(catalogo.IDLine));
                if (datosProduccion.CatalogArray.FindIndex(delegate(CatalogoDB cat) { return cat.catalogoID == newCat.catalogoID; }) != -1)
                {
                    i = datosProduccion.CatalogArray.FindIndex(delegate(CatalogoDB cat) { return cat.catalogoID == newCat.catalogoID; });
                }
                else
                {
                    datosProduccion.newCatalog(newCat);//, getEntry(catalogo.IDLine));
                    i = datosProduccion.CatalogArray.FindIndex(delegate(CatalogoDB cat) { return cat.catalogoID == newCat.catalogoID; });
                }

                if (catalogo.IDLine==IDLine.Japonesa)//MCR.
                    CatDBL1 = i; 
                else
                    CatDBL2 = i;

                bool b = datosProduccion.CatalogArray[i].ActualizarDBCat();
                if (!b)
                {
                    CatalogoDB[] AuxArray = new CatalogoDB[datosProduccion.CatalogArray.Count()];
                    datosProduccion.CatalogArray.CopyTo(AuxArray);
                    datosProduccion = new datosTurno();
                    if (AuxArray[CatDBL1] != null)
                    {
                        datosProduccion.newCatalog(new CatalogoDB(AuxArray[CatDBL1]));
                        CatDBL1 = datosProduccion.CatalogArray.Count - 1;
                    }
                    if (AuxArray[CatDBL2] != null)
                    {
                        datosProduccion.newCatalog(new CatalogoDB(AuxArray[CatDBL2]));
                        CatDBL2 = datosProduccion.CatalogArray.Count - 1;
                    }
                }
            }

            loading = false;


            //MDG.2010-12-10.Cargamos los grupos y cajas comunes a las 2 lineas
            _sys.Production.LoadLinesGroupsAndBoxes();

            //MDG.2010-12-02.Cargamos los grupos de los transportes cuando se carga la linea 2
            _sys.Production.LoadTransportGroups();
            loading = false;
        }

        #endregion

        #region Cierre aplicacion

        private int closeSteps;

        #region IDelegateTasksOwner Members

        public IEnumerable<Action> GetDelegateTasks()
        {
            return new Action[] { RefreshManual, GestorClose, RefreshEntradaProdecView, GestorArranqueSonoro, GestorMensajesLinea1};
        }

        #endregion

        #region IViewTasksOwner Members

        public IEnumerable<Action> GetViewTasks()
        {
            var list = new List<Action>();
            new IViewTasksOwner[] {_formDioSignal, _formCadenas, _formParker, _formBandaProdecView}
                .ForEach(t => list.AddRange(t.GetViewTasks()));
            return list;
        }

        #endregion

        private void GestorClose()
        {
            switch (closeSteps)
            {
                case 1:
                    closeSteps = 2;
                    Close();
                    break;
                case 3:
                    closeSteps = 4;
                    Close();
                    break;
                case 5:
                    Close();
                    break;
            }
        }
        private TON _timerStartAuto = new TON();//MDG.2013-03-26
        //private bool PermisionStartAuto
        
        private void GestorArranqueSonoro()
        {
            //MDG.2013-03-26.Para que suene durante 3 segundos
            if (_timerStartAuto.Timing(3000, _sys.Control.StartingAuto))
            {
                if (_sys.Control.OperationMode == Mode.Automatic)
                {
                    if (chkPasoAPaso.Checked)
                    {
                        _controllerSystem.StartModeWithStepByStep();
                    }
                    else
                    {
                        _controllerSystem.StartMode();
                    }
                }
                _sys.Control.StartingAuto = false;
                _timerStartAuto.Reset();
            }
            else if (_sys.Control.StartingAuto==false)
            {
                _timerStartAuto.Reset();
            }
        }

        private void GestorMensajesLinea1()//MDG.2013-04-25
        {
            try
            {
                if(_sys.Lines.Line1.BoxNotCatchedDespaletizing 
                    && _formErrorDespaletizado.Visible==false
                    )
                {
                    //DialogResult messageResult=MessageBox.Show("No se ha podido recoger la caja en el despaletizado de la línea 1." +
                    //    "\nPor favor Recójala y colóquela en su posición en el palet final." +
                    //    "\n ¿Ha colocado la caja en su sitio?"
                    //    ,"Caja no recogida en Despaletizado línea 1."
                    //    ,MessageBoxButtons.YesNo
                    //    ,MessageBoxIcon.Stop);
                    //DialogResult messageResult = _formErrorDespaletizado.ShowDialog();
                    //if(messageResult==DialogResult.Yes)
                    //    _sys.Lines.Line1.BoxNotCatchedDespaletizing = false;
                    _formErrorDespaletizado.Show();
                    _formErrorDespaletizado.Focus();
                }

            }
            catch (Exception)
            {
                ;

                //throw;
            }
        }

        private void frmEncartonadora_Closing(object sender, CancelEventArgs e)
        {
            switch (closeSteps)
            {
                case 0:
                    //MDG.2010-07-13.Esto estaba comentado. Creo que sirve para guardar el estado completo de la maquina.
                    //Sys.TrySave();                   

                    _sys.Production.SaveCatalogs();
                    _sys.Production.SaveLinesGroupsAndBoxes();
                    //MDG.2010-12-10.Salvamos todos los grupos y cajasde los transportes comunes a las 2 lineas
                    _sys.Production.SaveTransportGroups();
                    //MDG.2010-12-02.Salvamos todos los grupos de los transportes de la linea 2
                    AlarmsHistoricalManager.Instance.SaveAlarmsHistorical();
                    _sys.Bus.Activated = false;
                    closeSteps = 1;
                    e.Cancel = true;
                    break;
                case 2:
                    closeSteps = 3;
                    e.Cancel = true;
                    break;
                case 4:
                    closeSteps = 5;
                    ControlLoop<IdpsaSystemPaletizado>.Instance.Work = false;
                    break;
            }
        }

        #endregion

        #region Eventos menú

        private void menuSimbolico_Click(object sender, EventArgs e)
        {
            _formDioSignal.Show();
        }

        private void menuCadenas_Click(object sender, EventArgs e)
        {
            _formCadenas.Show();
        }

        private void menuParker_Click(object sender, EventArgs e) //MCR
        {
            auxContraseña = new FormAccessParker();
            auxContraseña.Button2.Click += new System.EventHandler(this.AceptarClick);
            auxContraseña.Show();

        }

        private void AceptarClick(object sender, EventArgs e)
        {   if (auxContraseña.DialogResult==DialogResult.OK)
                _formParker.Show();
        }

        private void menuVariador_Click(object sender, EventArgs e)
        {
            _formVariador.Show();
        }

        private void menuAdministrador_Click(object sender, EventArgs e)
        {
            _formAdministrador.ShowDialog();
        }

        private void menuPermiso_Click(object sender, EventArgs e)
        {
            if (!_sys.Control.ActiveMode)
                 _formAcceso.Show();//MCR.
            //if ((_formAcceso.ShowDialog() == DialogResult.Cancel))
            //{
            //    return;
            //}
            //ActualizarPermiso();
        }

        private void menuAlarmas_Click(object sender, EventArgs e)
        {
            _sfrmHistoricoAlarmas.ShowDialog();
        }

        private void menuPasaporte_Click(object sender, EventArgs e)
        {
            _editorTipoPasaporte.Show();
        }

        #endregion

        private void menuItem1_Click(object sender, EventArgs e)
        {
            _editorPaletizado.Show();
        }

        private void btVaciar_Click(object sender, EventArgs e)
        {
            if (_sys.Lines.Line1.TransporteLinea.Tramo1.Vaciar)
            {
                _sys.Lines.Line1.TransporteLinea.Tramo1.Vaciar = false;
                btVaciar.Text = "No Vaciando";
            }
            else
            {
                _sys.Lines.Line2.TransporteLinea.Tramo1.Vaciar = true;
                _sys.Lines.Line2.TransporteLinea.Tramo2.Vaciar = true;
                btVaciar.Text = "Vaciando";
            }

            //MDG.2010-07-13.Comprobacion orden de vaciado aún activa
            if (_sys.Lines.Line2.TransporteLinea.Tramo1.Vaciar ||
                _sys.Lines.Line2.TransporteLinea.Tramo2.Vaciar)
            {
                //MDG.2010-07-13.Anulamos vaciado
                _sys.Lines.Line2.TransporteLinea.Tramo1.Vaciar = false;
                _sys.Lines.Line2.TransporteLinea.Tramo2.Vaciar = false;
                btVaciar.Text = "No Vaciando";
            }
            else
            {
                //MDG.2010-07-13.Ordenamos vaciado
                _sys.Lines.Line2.TransporteLinea.Tramo1.Vaciar = true;
                _sys.Lines.Line2.TransporteLinea.Tramo2.Vaciar = true;
                btVaciar.Text = "Vaciando";
            }
        }

        private void menuImpresion_Click(object sender, EventArgs e)
        {
            _formPrintBox.Show();
        }

        private void TabSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = TabSystem.SelectedIndex;
            if (index == 1) //0)
            {
                _paletizersLine1View.Refresh();
            }
            else if (index == 2) //1)
            {
                _paletizerLine2View.Refresh();
            }
            else if (index == 3)
            {
                _transportsView.Refresh();
            }
            else if (TabSystem.SelectedIndex == 5) //4)
            {
                manualBoxReprocessor.SetFocusOnBarcode();
            }
        }

        private void menuItemRefreshForm_Click(object sender, EventArgs e)
        {
            RefreshManual();
        }

        private void menuCargarGruposTransporte_Click(object sender, EventArgs e)
        {
            _sys.Production.LoadTransportGroups();
        }

        private void btModoAcumulacion_L1_Click(object sender, EventArgs e)
        {
            if (_sys.Lines.BandaSalidaEnfajadora.PassportGroup.Count > 0)
                MessageBox.Show(
                    "No se puede cambiar de modo mientras haya grupos en la banda de entrada a la encajadora Prodec",
                    "Confirmacion Cambio Modo Acumulacion");
            else if (_sys.Lines.Encajadora.GruposPasaportes.Count % 4 > 0)
                MessageBox.Show(
                    "No se puede cambiar de modo mientras haya grupos de una caja sin completar dentro de la encajadora Prodec",
                    "Confirmacion Cambio Modo Acumulacion");
            else
            {
                //MDG.2010-12-03
                _sys.Lines.Line1.ModoAcumulacion = !_sys.Lines.Line1.ModoAcumulacion;
                if (_sys.Lines.Line1.ModoAcumulacion)
                {
                    btModoAcumulacion_L1.Text = "Modo Acumulacion L1 activo";
                    btModoAcumulacion_L1.BackColor = Color.LightGreen;
                    _sys.Lines.Line1.TransporteLinea.Tramo1.Capacity = 100;
                    _sys.Lines.Line1.TransporteLinea.Tramo1.ModoAcumulacion = true;
                    _sys.Lines.Semaphore.ModoAcumulacion_L2_T1 = true;
                    btVaciar.Text = "No vaciando";
                }
            }
        }

        private void btModoAcumulacion_L2_T1_Click(object sender, EventArgs e)
        {
            if (_sys.Lines.BandaSalidaEnfajadora.PassportGroup.Count > 0)
                MessageBox.Show(
                    "No se puede cambiar de modo mientras haya grupos en la banda de entrada a la encajadora Prodec",
                    "Confirmacion Cambio Modo Acumulacion");
            else if (_sys.Lines.Encajadora.GruposPasaportes.Count % 4 > 0)
                MessageBox.Show(
                    "No se puede cambiar de modo mientras haya grupos de una caja sin completar dentro de la encajadora Prodec",
                    "Confirmacion Cambio Modo Acumulacion");
            else
            {
                //MDG.2010-12-03
                _sys.Lines.Line2.ModoAcumulacion_T1 = !_sys.Lines.Line2.ModoAcumulacion_T1;
                if (_sys.Lines.Line2.ModoAcumulacion_T1)
                {
                    btModoAcumulacion_L2_T1.Text = "Modo Acumulacion L2-T1 activo";
                    btModoAcumulacion_L2_T1.BackColor = Color.LightGreen;
                    _sys.Lines.Line2.TransporteLinea.Tramo1.Capacity = 100;
                    _sys.Lines.Line2.TransporteLinea.Tramo2.Capacity = 20;
                    _sys.Lines.Line2.TransporteLinea.Tramo1.ModoAcumulacion = true;
                    _sys.Lines.Line2.TransporteLinea.Tramo2.ModoAcumulacion = true;
                    _sys.Lines.Semaphore.ModoAcumulacion_L2_T1 = true; //MDG.2011-05-30.Esta variable regula el semáforo
                    //Sys.Lines.Line2.TransporteLinea.Elevador2.ModoAcumulacion = true;//MDG.2011-05-30
                    btVaciar.Text = "No vaciando"; //MDG.2011-06-30
                }
            }
        }


        private void btModoAcumulacion_L2_T2_Click(object sender, EventArgs e)
        {
            if (_sys.Lines.BandaSalidaEnfajadora.PassportGroup.Count > 0)
                MessageBox.Show(
                    "No se puede cambiar de modo mientras haya grupos en la banda de entrada a la encajadora Prodec",
                    "Confirmacion Cambio Modo Acumulacion");
            else if (_sys.Lines.Encajadora.GruposPasaportes.Count % 4 > 0)
                MessageBox.Show(
                    "No se puede cambiar de modo mientras haya grupos de una caja sin completar dentro de la encajadora Prodec",
                    "Confirmacion Cambio Modo Acumulacion");
            else
            {
                _sys.Lines.Line2.ModoAcumulacion_T2 = !_sys.Lines.Line2.ModoAcumulacion_T2;
                if (_sys.Lines.Line2.ModoAcumulacion_T2)
                {
                    btModoAcumulacion_L2_T2.Text = "Modo Acumulacion L2-T2 Inactivo";
                    btModoAcumulacion_L2_T2.BackColor = Color.LightBlue;
                    _sys.Lines.Line2.TransporteLinea.Tramo1.Capacity = 80; // 10;
                    _sys.Lines.Line2.TransporteLinea.Tramo2.Capacity = 5;
                    _sys.Lines.Line2.TransporteLinea.Tramo1.ModoAcumulacion = false;
                    _sys.Lines.Line2.TransporteLinea.Tramo2.ModoAcumulacion = false;
                    _sys.Lines.Semaphore.ModoAcumulacion_L2_T2 = false; //MDG.2011-05-30.Esta variable regula el semáforo
                    //Sys.Lines.Line2.TransporteLinea.Elevador2.ModoAcumulacion = false;//MDG.2011-05-30
                    btVaciar.Text = "Vaciando"; //MDG.2011-06-30
                }
            }
        }

        private void menuCargarCajasBandas_Click(object sender, EventArgs e)
        {
            _sys.Production.LoadLinesGroupsAndBoxes();
        }

        private void InitializeRobotEnlaceManualMode()
        {
            //_sys.ManualFeedRuhlamat.Activate();
            //_sys.Rulhamat.DeactivateRobotEnlace();
            //btEntradaManual0.Text = "Entrada Prodec ACTIVA\n---\nRobotEnlace INACTIVO";
            //btEntradaManual0.BackColor = Color.LightGreen;
            _sys.ManualFeedRuhlamat.Deactivate();
            _sys.Rulhamat.ActivateRobotEnlace();
            btEntradaManual0.Text = @"Entrada Prodec INACTIVA\n---\nRobotEnlace ACTIVO";
            btEntradaManual0.BackColor = Color.DarkOrange;
        }

        private void btEntradaManual_Click(object sender, EventArgs e)
        {
            if (_sys.Control.OperationMode != Mode.Automatic)
            {
                if (_sys.ManualFeedRuhlamat.State == SubsystemState.Deactivated)
                {
                    if (_sys.Lines.Line2.TransporteLinea.IsEmpty()) // && _sys.ManualFeedRuhlamat.IsEmpty())
                    {
                        _sys.ManualFeedRuhlamat.Activate();
                        _sys.Rulhamat.DeactivateRobotEnlace();
                        btEntradaManual0.Text = @"Entrada Prodec ACTIVA\n---\nRobotEnlace INACTIVO";
                        btEntradaManual0.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        MessageBox.Show(
                            @"Habilitacion de entrada manual a PRODEC no permitida. Vaciar Linea 2 y entrada manual a PRODEC",
                            @"Entrada Manual a Prodec", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (_sys.ManualFeedRuhlamat.State == SubsystemState.Activated)
                {
                    _sys.ManualFeedRuhlamat.Deactivate();
                    _sys.Rulhamat.ActivateRobotEnlace();
                    btEntradaManual0.Text = @"Entrada Prodec INACTIVA\n---\nRobotEnlace ACTIVO";
                    btEntradaManual0.BackColor = Color.DarkOrange;
                }
            }
            else
            {
                MessageBox.Show(@"Cambio de modo no permitido en modo automatico. Cambie a modo manual",
                                @"Entrada Manual a Prodec", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void menuCatalogo_Click(object sender, EventArgs e)
        {
            _editorCatalogoPaletizado.Show();
        }

        #region Save

        private void menuItem4_Click(object sender, EventArgs e)
        {
            _sys.Production.SaveCatalogs();
            _sys.Production.SaveLinesGroupsAndBoxes();
            _sys.Production.SaveTransportGroups();
        }

        private void menuGuardarCatalogos_Click(object sender, EventArgs e)
        {
            //MDG.2010-07-13.Guardado catálogos desde menu, sin cerrar la aplicacion
            _sys.Production.SaveCatalogs();
        }

        private void menuGuardarCajasBandas_Click(object sender, EventArgs e)
        {
            _sys.Production.SaveLinesGroupsAndBoxes();
        }

        private void menuGuardarGruposTransporte_Click(object sender, EventArgs e)
        {
            _sys.Production.SaveTransportGroups();
        }

        #endregion

        private TON _timerStart = new TON();//MDG.2013-03-26
        private void btnActiveModeStart_MouseDown(object sender, MouseEventArgs e)
        {
            ////MDG.2013-03-26
            //if ((_sys.Control.OperationMode == Mode.Automatic))//MDG.2013-03-26
            //{
            //    if (_timerStart.Timing(3000, true))
            //    {

            //        if (chkPasoAPaso.Checked)
            //        {
            //            _controllerSystem.StartModeWithStepByStep();
            //        }
            //        else
            //        {
            //            _controllerSystem.StartMode();
            //        }
            //        _sys.Control.StartingAuto = false;
            //    }
            //    else
            //    {
            //        _sys.Control.StartingAuto = true;
            //    }
            //}
        }

        private void btnActiveModeStart_MouseUp(object sender, MouseEventArgs e)
        {
            //_sys.Control.StartingAuto = false;
            //_timerStart.Reset();
        }

        private Entrada getEntry(IDLine idLine)
        {
            if (idLine == IDLine.Japonesa)
                return Entrada.Unomatic;
            else if (_sys.ManualFeedRuhlamat.State == SubsystemState.Deactivated)
                return Entrada.ManualProdec;
            else
                return Entrada.Ruhlamat;
        }
        private int getActualCatalog()
        {
            if (_sys.Lines.Line2.ModoAcumulacion_T2)
                return (CatDBL1);
            else
                return (CatDBL2);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            datosVisualReport = new datosTurno();
            String turn = "";
            String fech = "";
            String idcat = tbcatalogoID.Text;
            if (cbTurno.Text != "")
                turn = cbTurno.Text.Substring(0, 1);
            if (cbDia.Text != "" && cbMes.Text != "" && tbAño.Text != "")
                fech = tbAño.Text + cbMes.Text + cbDia.Text;
            bool b = datosVisualReport.SearchDB(turn, fech, idcat);
            if (!b)
                MessageBox.Show("No se puede generar un informe con los datos introducidos");
            else
                this.BindingSource.DataSource = datosVisualReport.getCatalogs();
            reportViewer1.Refresh();
            reportViewer1.RefreshReport();
        }

        private void BarreraRearmada(bool b)  //MCR
        {
            if (_sys.barreraRearme.maquinaIniciada)
                this.cadenaRota = !b;
            if (cadenaRota)
            {
                _sys.barreraRearme.luz.Activate(false);
                //try { _formAcceso.ShowDialog(); }
                //catch { Exception exc; }
            }
            else

                _sys.barreraRearme.luz.Activate(true);
        }
    }
}