using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Idpsa.Control;
using Idpsa.Control.Engine;
using Timer = System.Timers.Timer;

namespace Idpsa.Paletizado
{
    public partial class TransportsView : UserControl
    {
        private readonly Timer _timer2;
        private int Tramo1ViewIndex;
        private int Tramo2ViewIndex;
        private BandaEtiquetado _bandaEtiquetado; //MDG.2010-12-03.Lo visualizamos tambien
        private ReprocesadorManual _bandaReprocesadoManual; //MDG.2010-12-03.Lo visualizamos tambien
        private BandaSalidaEnfajadora _bandaSalidaEnfajadora; //MDG.2010-12-03.Lo visualizamos tambien

        private Elevador1 _elevador1; //MDG.2010-12-02.Lo visualizamos tambien
        private Elevador2 _elevador2; //MDG.2010-12-02.Lo visualizamos tambien

        private SourceGroupSupplier _line2GroupSupplier; //MDG.2010-12-09.Lo visualizamos tambien

        private Prodec _prodec; //MDG.2010-12-03.Lo visualizamos tambien
        private Timer _timer;
        private TramoTransporteGruposPasaportes _transporte1;
        private TramoTransporteGruposPasaportes _transporte2;
        private int a; //MDG.2010-11-24.Contador para pruebas
        
        //IDPSASystemPaletizado _sys;//MDG.2010-11-24.Para tener acceso al Salvado de catalogos
        
        private SystemControllerPaletizado _controllerSystem;//MDG.2012-07-23
        private SystemControl _control;//MDG.2012-07-23
        

        public TransportsView()
        {
            InitializeComponent();
            tabControl1.Selected += SelectedEventHandler;
            //_timer = new System.Timers.Timer() { AutoReset = false, Interval = 50 };
            //MDG.2010-11-24.Nuevo temporizador con autorearme(se vuelve a ejecutar continuamente cada 100 ms)
            _timer2 = new Timer {AutoReset = true, Interval = 100};
            tabControl1.SelectedIndex = 0;
            //_timer.Elapsed += delegate { 
            //    _mosaicView.Paint();

            //    //try
            //    //{
            //    //    //MDG.2010-11-24.Guardado catálogos al depositar cada caja
            //    //    _sys.Production.SaveCatalogs();

            //    //}
            //    //catch { }
            //};
            _timer2.Elapsed += delegate { RefreshListasTransportes(); };
        }


        private void ElementAddedHandler(object sender, DataEventArgs<GrupoPasaportes> grupo)
        {
            //var listView = GetListView(sender);
            //AddGroup(listView, grupo.Data);
            //listView.Refresh();
            //listView.Invalidate();
            RefreshListasTransportes();
        }

        private void ElementQuittedHandler(object sender, DataEventArgs<GrupoPasaportes> grupo)
        {
            //var listView = GetListView(sender);
            //QuitGroup(listView, grupo.Data);
            RefreshListasTransportes();
        }

        private void ElementsClearedHandler(object sender, EventArgs e)
        {
            //var listView = GetListView(sender);
            //listView.Items.Clear();
            RefreshListasTransportes();
        }

        private void AddGroup(ListView listView, GrupoPasaportes grupo)
        {
            var item = new ListViewItem(grupo.Id) {Name = grupo.Id};
            listView.Items.Add(item);
        }

        private void QuitGroup(ListView listView, GrupoPasaportes grupo)
        {
            listView.Items.RemoveByKey(grupo.Id);
        }

        public void SubscribeLine2(Line2 line)
        {
            //_mosaicView.SubscriveToEnvents(line.Paletizer);
            _elevador1 = line.TransporteLinea.Elevador1;
            _transporte1 = line.TransporteLinea.Tramo1;
            _transporte2 = line.TransporteLinea.Tramo2;
            _elevador2 = line.TransporteLinea.Elevador2;

            _transporte1.GroupPutted += ElementAddedHandler;
            _transporte1.GroupQuitted += ElementQuittedHandler;
            _transporte1.GroupsCleared += ElementsClearedHandler;

            _transporte2.GroupPutted += ElementAddedHandler;
            _transporte2.GroupQuitted += ElementQuittedHandler;
            _transporte2.GroupsCleared += ElementsClearedHandler;


            _line2GroupSupplier = line.Supplier; //MDG.2010-12-09.Lo visualizamos tambien
        }

        public void SubscribeLines(Lines lines) //MDG.2010-12-03
        {
            _bandaSalidaEnfajadora = lines.BandaSalidaEnfajadora;
            _prodec = lines.Encajadora;
            _bandaEtiquetado = lines.BandaEtiquetado;
            _bandaReprocesadoManual = lines.ManualReprocesor;
        }

        public void SubscribeControllerSystem(SystemControllerPaletizado controllerSystem) //MDG.2012-07-23
        {
            _controllerSystem = controllerSystem;
        }
        public void SubscribeControl(SystemControl control) //MDG.2012-07-23
        {
            _control = control;
        }

        public void TieToSysControl()
        {
            _control.NewNotification += NewSystemControlNotificationHandler;
        }


        private void SelectedEventHandler(object sender, TabControlEventArgs e)
        {
            Refresh();
        }

        public void Refresh()
        {
            //if (tabControl1.SelectedIndex == 1)
            //{
            //    _timer.Start();
            //}
            //MDG.2010-11-24.Obligo a actualizar las listas
            if (tabControl1.SelectedIndex == 0)
            {
                _timer2.Start();
            }
        }

        private void buttonRefreshListasTransportes_Click(object sender, EventArgs e)
        {
            RefreshListasTransportes();
        }

        private void RefreshListasTransportes()
        {
            try
            {
                //MDG.2010-11-24.Actualizamos la pantalla de visualizacion de los grupos de pasaportes
                RefreshIndiceGrupoEntradaLine2();
                RefreshElevador1();
                RefreshTramo1();
                RefreshTramo2();
                RefreshElevador2();
                RefreshBandaSalidaFajado();
                RefreshProdec();
                RefreshBandaEtiquetado();
                RefreshBandaReprocesoManual();
            }
            catch //si da error ignoramos porque es visualizacion
            {
            }
        }

        private void RefreshIndiceGrupoEntradaLine2()
        {
            try
            {
                GrupoPasaportes ItemActual = _line2GroupSupplier.GetItem();
                labelIndiceGrupoEntrada.Text = (_line2GroupSupplier.CurrentIndex + 1).ToString();

                //MDG.2011-06-13.Mostramos el siguiente grupo a mostrar en el nuevo item
                
                if (_line2GroupSupplier.CurrentIndex == -1) //MDG.2011-06-15
                    lblSiguienteGrupoAscensor.Text = "-"; //MDG.2011-06-15
                else
                    lblSiguienteGrupoAscensor.Text = ItemActual.IdPlus1;
            }
            catch (Exception ex)
            {
                lblSiguienteGrupoAscensor.Text = "-"; //MDG.2011-06-15
            }
        }

        private void RefreshElevador1()
        {
            //Refresco pantalla CANTIDAD DE GRUPOS
            if (_elevador1.PassportGroup == null)
            {
                labelCantidadGruposAscensor1.Text = "0 / 1";
            }
            else
            {
                labelCantidadGruposAscensor1.Text = "1 / 1";
            }

            //MDG.2010.11.24. Actualizamos el grupo existente en el elevador 1
            if (_elevador1.PassportGroup != null) //(_listView1.Items.Count > 0)
            {
                lblAscensor1Grupo01.Text = _elevador1.PassportGroup.IdPlus1; // _listView1.Items[0].Text;
                lblAscensor1Grupo01.BackColor = Color.Green;
            }
            else
            {
                lblAscensor1Grupo01.Text = "";
                lblAscensor1Grupo01.BackColor = Color.Red;
            }
        }

        private void RefreshTramo1()
        {
            labelIndiceVistaTramo1.Text = Tramo1ViewIndex.ToString(); //MDG.2011-05-18
            //a++;//prueba para comprobar cuando refresca
            labelCantidadGruposTramo1.Text = _transporte1.Grupos.Count() + " / " + _transporte1.Capacity;
            ; //_listView1.Items.Count.ToString();
            //labelCantidadGruposTramo1.Text = a.ToString();//prueba para comprobar cuando refresca

            //MDG.2011-06-13.Intento de cuadrar los indices de grupo con los indices de caja
            //No es posible porque el id es ya un string
            //int DesfaseGrupos = 25; //Todos los pasaportes de la linea 2 son finos, vienen de 25 en 25

            //Refresco pantalla Texto de los grupos del transporte aereo 1
            if (_transporte1.Grupos.Count() > 0) //(_listView1.Items.Count > 0)
            {
                //lblTramo1Grupo01.Text = _transporte1._grupos.List[0].Id;// _listView1.Items[0].Text;
                try
                {
                    lblTramo1Grupo01.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 1 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo01.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo01.Text = "";
                lblTramo1Grupo01.BackColor = Color.Red;
            }
            if (_transporte1.Grupos.Count() > 1) //(_listView1.Items.Count > 1)
            {
                //lblTramo1Grupo02.Text = _transporte1._grupos.List[1].Id;//_listView1.Items[1].Text;
                try
                {
                    lblTramo1Grupo02.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 2 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo02.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo02.Text = "";
                lblTramo1Grupo02.BackColor = Color.Red;
            }
            if (_transporte1.Grupos.Count() > 2) //(_listView1.Items.Count > 2)
            {
                //lblTramo1Grupo03.Text = _transporte1._grupos.List[2].Id;//_listView1.Items[2].Text;
                try
                {
                    lblTramo1Grupo03.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 3 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo03.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo03.Text = "";
                lblTramo1Grupo03.BackColor = Color.Red;
            }
            if (_transporte1.Grupos.Count() > 3) //(_listView1.Items.Count > 3)
            {
                //lblTramo1Grupo04.Text = _transporte1._grupos.List[3].Id;//_listView1.Items[3].Text;
                try
                {
                    lblTramo1Grupo04.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 4 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo04.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo04.Text = "";
                lblTramo1Grupo04.BackColor = Color.Red;
            }
            if (_transporte1.Grupos.Count() > 4) //(_listView1.Items.Count > 4)
            {
                //lblTramo1Grupo05.Text = _transporte1._grupos.List[4].Id;//_listView1.Items[4].Text;
                try
                {
                    lblTramo1Grupo05.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 5 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo05.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo05.Text = "";
                lblTramo1Grupo05.BackColor = Color.Red;
            }
            if (_transporte1.Grupos.Count() > 5) //(_listView1.Items.Count > 5)
            {
                //lblTramo1Grupo06.Text = _transporte1._grupos.List[5].Id;//_listView1.Items[5].Text;
                try
                {
                    lblTramo1Grupo06.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 6 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo06.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo06.Text = "";
                lblTramo1Grupo06.BackColor = Color.Red;
            }
            if (_transporte1.Grupos.Count() > 6) //(_listView1.Items.Count > 6)
            {
                //lblTramo1Grupo07.Text = _transporte1._grupos.List[6].Id;//_listView1.Items[6].Text;
                try
                {
                    lblTramo1Grupo07.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 7 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo07.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo07.Text = "";
                lblTramo1Grupo07.BackColor = Color.Red;
            }
            if (_transporte1.Grupos.Count() > 7) //(_listView1.Items.Count > 7)
            {
                //lblTramo1Grupo08.Text = _transporte1._grupos.List[7].Id;//_listView1.Items[7].Text;
                try
                {
                    lblTramo1Grupo08.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 8 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo08.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo08.Text = "";
                lblTramo1Grupo08.BackColor = Color.Red;
            }
            if (_transporte1.Grupos.Count() > 8) //(_listView1.Items.Count > 8)
            {
                //lblTramo1Grupo09.Text = _transporte1._grupos.List[8].Id;//_listView1.Items[8].Text;
                try
                {
                    lblTramo1Grupo09.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 9 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo09.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo09.Text = "";
                lblTramo1Grupo09.BackColor = Color.Red;
            }
            if (_transporte1.Grupos.Count() > 9) //(_listView1.Items.Count > 9)
            {
                //lblTramo1Grupo10.Text = _transporte1._grupos.List[9].Id;//_listView1.Items[9].Text;
                try
                {
                    lblTramo1Grupo10.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 10 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo10.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo10.Text = "";
                lblTramo1Grupo10.BackColor = Color.Red;
            }
            if (_transporte1.Grupos.Count() > 10) //(_listView1.Items.Count > 10)
            {
                //lblTramo1Grupo11.Text = _transporte1._grupos.List[10].Id;//_listView1.Items[10].Text;
                try
                {
                    lblTramo1Grupo11.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 11 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo11.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo11.Text = "";
                lblTramo1Grupo11.BackColor = Color.Red;
            }
            if (_transporte1.Grupos.Count() > 11) //(_listView1.Items.Count > 10)
            {
                //lblTramo1Grupo12.Text = _transporte1._grupos.List[11].Id;//_listView1.Items[10].Text;
                try
                {
                    lblTramo1Grupo12.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 12 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo12.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo12.Text = "";
                lblTramo1Grupo12.BackColor = Color.Red;
            }
            if (_transporte1.Grupos.Count() > 12) //(_listView1.Items.Count > 10)
            {
                //lblTramo1Grupo13.Text = _transporte1._grupos.List[12].Id;//_listView1.Items[10].Text;//
                try
                {
                    lblTramo1Grupo13.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 13 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo13.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo13.Text = "";
                lblTramo1Grupo13.BackColor = Color.Red;
            }
            if (_transporte1.Grupos.Count() > 13) //(_listView1.Items.Count > 10)
            {
                //lblTramo1Grupo14.Text = _transporte1._grupos.List[13].Id;//_listView1.Items[10].Text;
                try
                {
                    lblTramo1Grupo14.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 14 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo14.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo14.Text = "";
                lblTramo1Grupo14.BackColor = Color.Red;
            }
            if (_transporte1.Grupos.Count() > 14) //(_listView1.Items.Count > 10)
            {
                //lblTramo1Grupo15.Text = _transporte1._grupos.List[14].Id;//_listView1.Items[10].Text;
                try
                {
                    lblTramo1Grupo15.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 15 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo15.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo15.Text = "";
                lblTramo1Grupo15.BackColor = Color.Red;
            }
            if (_transporte1.Grupos.Count() > 15) //(_listView1.Items.Count > 10)
            {
                //lblTramo1Grupo16.Text = _transporte1._grupos.List[15].Id;//_listView1.Items[10].Text;
                try
                {
                    lblTramo1Grupo16.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 16 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo16.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo16.Text = "";
                lblTramo1Grupo16.BackColor = Color.Red;
            }
            if (_transporte1.Grupos.Count() > 16) //(_listView1.Items.Count > 10)
            {
                //lblTramo1Grupo17.Text = _transporte1._grupos.List[16].Id;//_listView1.Items[10].Text;
                try
                {
                    lblTramo1Grupo17.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 17 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo17.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo17.Text = "";
                lblTramo1Grupo17.BackColor = Color.Red;
            }
            if (_transporte1.Grupos.Count() > 17) //(_listView1.Items.Count > 10)
            {
                //lblTramo1Grupo18.Text = _transporte1._grupos.List[17].Id;//_listView1.Items[10].Text;
                try
                {
                    lblTramo1Grupo18.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 18 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo18.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo18.Text = "";
                lblTramo1Grupo18.BackColor = Color.Red;
            }
            if (_transporte1.Grupos.Count() > 18) //(_listView1.Items.Count > 10)
            {
                //lblTramo1Grupo19.Text = _transporte1._grupos.List[18].Id;//_listView1.Items[10].Text;
                try
                {
                    lblTramo1Grupo19.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 19 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo19.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo19.Text = "";
                lblTramo1Grupo19.BackColor = Color.Red;
            }
            if (_transporte1.Grupos.Count() > 19) //(_listView1.Items.Count > 10)
            {
                //lblTramo1Grupo20.Text = _transporte1._grupos.List[19].Id;//_listView1.Items[10].Text;
                try
                {
                    lblTramo1Grupo20.Text =
                        _transporte1.Grupos.List[(_transporte1.Grupos.Count() - 20 - Tramo1ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo1Grupo20.BackColor = Color.Green;
            }
            else
            {
                lblTramo1Grupo20.Text = "";
                lblTramo1Grupo20.BackColor = Color.Red;
            }
        }

        private void RefreshTramo2()
        {
            labelIndiceVistaTramo2.Text = Tramo2ViewIndex.ToString(); //MDG.2011-05-18

            labelCantidadGruposTramo2.Text = _transporte2.Grupos.Count() + " / " + _transporte2.Capacity;
            //_listView2.Items.Count.ToString();

            //Refresco pantalla Texto de los grupos del transporte aereo 1
            if (_transporte2.Grupos.Count() > 0) //(_listView2.Items.Count > 0)
            {
                //lblTramo2Grupo01.Text = _transporte2._grupos.List[0].Id;//_listView2.Items[0].Text;
                try
                {
                    lblTramo2Grupo01.Text =
                        _transporte2.Grupos.List[(_transporte2.Grupos.Count() - 1 - Tramo2ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo2Grupo01.BackColor = Color.Green;
            }
            else
            {
                lblTramo2Grupo01.Text = "";
                lblTramo2Grupo01.BackColor = Color.Red;
            }
            if (_transporte2.Grupos.Count() > 1) //(_listView2.Items.Count > 1)
            {
                //lblTramo2Grupo02.Text = _transporte2._grupos.List[1].Id;//_listView2.Items[1].Text;
                try
                {
                    lblTramo2Grupo02.Text =
                        _transporte2.Grupos.List[(_transporte2.Grupos.Count() - 2 - Tramo2ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo2Grupo02.BackColor = Color.Green;
            }
            else
            {
                lblTramo2Grupo02.Text = "";
                lblTramo2Grupo02.BackColor = Color.Red;
            }
            if (_transporte2.Grupos.Count() > 2) //(_listView2.Items.Count > 2)
            {
                //lblTramo2Grupo03.Text = _transporte2._grupos.List[2].Id;//_listView2.Items[2].Text;
                try
                {
                    lblTramo2Grupo03.Text =
                        _transporte2.Grupos.List[(_transporte2.Grupos.Count() - 3 - Tramo2ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo2Grupo03.BackColor = Color.Green;
            }
            else
            {
                lblTramo2Grupo03.Text = "";
                lblTramo2Grupo03.BackColor = Color.Red;
            }
            if (_transporte2.Grupos.Count() > 3) //(_listView2.Items.Count > 3)
            {
                //lblTramo2Grupo04.Text = _transporte2._grupos.List[3].Id;//_listView2.Items[3].Text;
                try
                {
                    lblTramo2Grupo04.Text =
                        _transporte2.Grupos.List[(_transporte2.Grupos.Count() - 4 - Tramo2ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo2Grupo04.BackColor = Color.Green;
            }
            else
            {
                lblTramo2Grupo04.Text = "";
                lblTramo2Grupo04.BackColor = Color.Red;
            }
            if (_transporte2.Grupos.Count() > 4) //(_listView2.Items.Count > 4)
            {
                //lblTramo2Grupo05.Text = _transporte2._grupos.List[4].Id;//_listView2.Items[4].Text;
                try
                {
                    lblTramo2Grupo05.Text =
                        _transporte2.Grupos.List[(_transporte2.Grupos.Count() - 5 - Tramo2ViewIndex)].IdPlus1;
                }
                catch
                {
                }
                lblTramo2Grupo05.BackColor = Color.Green;
            }
            else
            {
                lblTramo2Grupo05.Text = "";
                lblTramo2Grupo05.BackColor = Color.Red;
            }
        }

        private void RefreshElevador2()
        {
            if (_elevador2.GrupoPasaportes == null)
            {
                labelCantidadGruposAscensor2.Text = "0 / 1";
            }
            else
            {
                labelCantidadGruposAscensor2.Text = "1 / 1";
            }
            //MDG.2010.11.24. Actualizamos el grupo existente en el elevador 2
            if (_elevador2.GrupoPasaportes != null)
            {
                lblAscensor2Grupo01.Text = _elevador2.GrupoPasaportes.IdPlus1; // _listView1.Items[0].Text;
                lblAscensor2Grupo01.BackColor = Color.Green;
            }
            else
            {
                lblAscensor2Grupo01.Text = "";
                lblAscensor2Grupo01.BackColor = Color.Red;
            }
        }

        private void RefreshBandaSalidaFajado()
        {
            //MDG.2010-12-03
            labelCantidadGruposTramoEntradaProdec.Text = _bandaSalidaEnfajadora.NumGrupos() + " / 1";
            // +_bandaSalidaEnfajadora._gruposPasaportes.Capacity.ToString();//_listView2.Items.Count.ToString();
            //MDG.2010.12-03. Actualizamos los grupos existentes en la banda de entrada a la Prodec
            if (_bandaSalidaEnfajadora.PassportGroup.Count() > 0)
            {
                lblTramoEntradaProdecGrupo01.Text = _bandaSalidaEnfajadora.PassportGroup[0].IdPlus1;
                lblTramoEntradaProdecGrupo01.BackColor = Color.Green;
            }
            else
            {
                lblTramoEntradaProdecGrupo01.Text = "";
                lblTramoEntradaProdecGrupo01.BackColor = Color.Red;
            }
            if (_bandaSalidaEnfajadora.PassportGroup.Count() > 1)
            {
                lblTramoEntradaProdecGrupo02.Text = _bandaSalidaEnfajadora.PassportGroup[1].IdPlus1;
                lblTramoEntradaProdecGrupo02.BackColor = Color.Green;
            }
            else
            {
                lblTramoEntradaProdecGrupo02.Text = "";
                lblTramoEntradaProdecGrupo02.BackColor = Color.Red;
            }
            if (_bandaSalidaEnfajadora.PassportGroup.Count() > 2)
            {
                lblTramoEntradaProdecGrupo03.Text = _bandaSalidaEnfajadora.PassportGroup[2].IdPlus1;
                lblTramoEntradaProdecGrupo03.BackColor = Color.Green;
            }
            else
            {
                lblTramoEntradaProdecGrupo03.Text = "";
                lblTramoEntradaProdecGrupo03.BackColor = Color.Red;
            }
            if (_bandaSalidaEnfajadora.PassportGroup.Count() > 3)
            {
                lblTramoEntradaProdecGrupo04.Text = _bandaSalidaEnfajadora.PassportGroup[3].IdPlus1;
                lblTramoEntradaProdecGrupo04.BackColor = Color.Green;
            }
            else
            {
                lblTramoEntradaProdecGrupo04.Text = "";
                lblTramoEntradaProdecGrupo04.BackColor = Color.Red;
            }
        }

        private void RefreshProdec()
        {
            //labelCantidadGruposProdec.Text = "0 / 4";
            labelCantidadGruposProdec.Text = _prodec.GruposPasaportes.Count() + " / " +
                                             _prodec.GruposPasaportes.Capacity; //_listView2.Items.Count.ToString();

            //MDG.2010.11.24. Actualizamos los grupos existentes en la encajadora Prodec
            if (_prodec.GruposPasaportes.Count() > 0)
            {
                lblProdecGrupo01.Text = _prodec.GruposPasaportes[0].IdPlus1;
                lblProdecGrupo01.BackColor = Color.Green;
            }
            else
            {
                lblProdecGrupo01.Text = "";
                lblProdecGrupo01.BackColor = Color.Red;
            }
            if (_prodec.GruposPasaportes.Count() > 1)
            {
                lblProdecGrupo02.Text = _prodec.GruposPasaportes[1].IdPlus1;
                lblProdecGrupo02.BackColor = Color.Green;
            }
            else
            {
                lblProdecGrupo02.Text = "";
                lblProdecGrupo02.BackColor = Color.Red;
            }
            if (_prodec.GruposPasaportes.Count() > 2)
            {
                lblProdecGrupo03.Text = _prodec.GruposPasaportes[2].IdPlus1;
                lblProdecGrupo03.BackColor = Color.Green;
            }
            else
            {
                lblProdecGrupo03.Text = "";
                lblProdecGrupo03.BackColor = Color.Red;
            }
            if (_prodec.GruposPasaportes.Count() > 3)
            {
                lblProdecGrupo04.Text = _prodec.GruposPasaportes[3].IdPlus1;
                lblProdecGrupo04.BackColor = Color.Green;
            }
            else
            {
                lblProdecGrupo04.Text = "";
                lblProdecGrupo04.BackColor = Color.Red;
            }
        }

        private void RefreshBandaEtiquetado()
        {
            if (_bandaEtiquetado.Caja == null)
                labelCantidadCajasBandaEtiquetado.Text = "0 / 1";
            else
                labelCantidadCajasBandaEtiquetado.Text = "1 / 1";
            //MDG.2010.11.24. Actualizamos los grupos existentes en la encajadora Prodec
            if (_bandaEtiquetado.Caja != null)
            {
                lblBandaEtiquetadoCaja01.Text = _bandaEtiquetado.Caja.Id;
                lblBandaEtiquetadoCaja01.BackColor = Color.Green;
            }
            else
            {
                lblBandaEtiquetadoCaja01.Text = "";
                lblBandaEtiquetadoCaja01.BackColor = Color.Red;
            }
        }

        private void RefreshBandaReprocesoManual()
        {
            if (_bandaReprocesadoManual.Caja == null)
                labelCantidadCajasBandaReprocesadoManual.Text = "0 / 1";
            else
                labelCantidadCajasBandaReprocesadoManual.Text = "1 / 1";
            //MDG.2010.12-03. Actualizamos las cajas existentes en la banda de reprocesado manual
            if (_bandaReprocesadoManual.Caja != null)
            {
                lblBandaReprocesadoManualCaja01.Text = _bandaReprocesadoManual.Caja.Id;
                lblBandaReprocesadoManualCaja01.BackColor = Color.Green;
            }
            else
            {
                lblBandaReprocesadoManualCaja01.Text = "";
                lblBandaReprocesadoManualCaja01.BackColor = Color.Red;
            }
        }

        private void buttonClearTransportGroups_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show(
                    "¿Desea borrar todos los grupos existentes en los ascensores y los transportadores aéreos de la línea 2?",
                    "Banda Reproceso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //MDG.2011-04-26.Confirmacion de seguridad
            {
                _elevador1.QuitItem();
                try
                {
                    _elevador1.State = Elevador1.States.Bajar;
                } //MDG.2011-05-18
                catch
                {
                }
                _transporte1.Grupos.Clear();
                _transporte2.Grupos.Clear();
                _elevador2.QuitItem();
                try
                {
                    _elevador2._state = Elevador2.States.Subir;
                } //MDG.2011-05-18
                catch
                {
                }
                _line2GroupSupplier.CurrentIndex = -1;
                //MDG.2010-12-09.Reseteo tambien el indice actual para que vuelva a consultarlo desde la ultima caja paletizada

                //Sys.Production.SaveTransportGroups();//MDG.2011-05-30.Nos aseguramos de que se salva borrado
            }
            //Reseteo Cajas comunes
        }

        private void buttonClearBoxBandaEtiquetado_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("¿Desea borrar la caja existente en la banda de Etiquetado?", "Banda Etiquetado",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //MDG.2011-04-26.Confirmacion de seguridad
            {
                _bandaEtiquetado.SetDataStored(new StoredDataBandaEtiquetadoBox()); //
                //{
                //    Caja=null,
                //    State = 
                //});
                //MDG.2011-04-26.Valores por defecto
                //_bandaEtiquetado.Caja = null;
                _bandaEtiquetado.State = BandaEtiquetado.States.EsperarCajaEnProdec;
            }
        }

        private void buttonClearBandaSalidaFajadoraGroups_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("¿Desea todos los grupos existentes en la Banda de Entrada de la Encajadora Prodec?",
                                "Banda Entrada Encajadora Prodec", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes) //MDG.2011-04-26.Confirmacion de seguridad
            {
                _bandaSalidaEnfajadora.PassportGroup.Clear();
            }
        }

        private void buttonClearProdecGroups_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("¿Desea borrar todos los grupos existentes en la Encajadora Prodec?",
                                "Encajadora Prodec", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes) //MDG.2011-04-26.Confirmacion de seguridad
            {
                _prodec.GruposPasaportes.Clear();
            }
        }

        private void buttonClearBandaReprocesadoBox_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("¿Desea borrar la caja existente en la banda de Reprocesado?", "Banda Reproceso",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //MDG.2011-04-26.Confirmacion de seguridad
            {
                _bandaReprocesadoManual.SetDataStored(new StoredDataBandaReprocesoBox());
                //_bandaReprocesadoManual.Caja == null;
            }
        }

        //MDG.2011-04-26
        private void buttonConfirmarCajaEtiquetado_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show(
                    "¿Desea confirmar la salida de una caja de la Encajadora Prodec, para que entre en la Banda de Etiquetado?\n\nNOTA: Para que este comando funcione debe : 1- No haber ya una caja en la Banda de Etiquetado.\nHaber al menos 4 grupos en el buffer de la Encajadora Prodec.\n\nDebe ser utilizado sólo si se ha quedado detenida una caja a la salida de la encajadora en modo Automático.",
                    "Banda Etiquetado", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //MDG.2011-04-26.Confirmacion de seguridad//MDG.2011-06-14.Amplío texto descripción
            {
                if (_bandaEtiquetado.Caja != null)
                {
                    MessageBox.Show(
                        "No se puede traspasar la información de la siguiente caja porque ya la Banda de Etiquetado ya tiene información de otra caja. Este comando no se puede utilizar en este caso.",
                        "Banda Etiquetado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (_prodec.GruposPasaportes.Count() < 4) //MDG.2011-06-15
                {
                    MessageBox.Show(
                        "No se puede traspasar la información de la siguiente caja porque no hay al menos 4 grupos en el buffer de la encajadora. Este comando no se puede utilizar en este caso.",
                        "Banda Etiquetado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    _bandaEtiquetado.State = BandaEtiquetado.States.EsperarCajaEnProdec;
                    _bandaEtiquetado.PuentePermisoEntradaCaja = true;
                    MessageBox.Show("Información traspasada correctamente", "Banda Etiquetado", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
            }
        }

        private void buttonModificarIndiceEntradaLinea2_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("¿Desea incrementar en 1 el indice de los grupos de entrada?", "Ascensor Entrada",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //MDG.2011-04-26.Confirmacion de seguridad
                _line2GroupSupplier.CurrentIndex += 1;
        }

        private void buttonDecrementarIndiceEntradaLinea2_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("¿Desea decrementar en 1 el indice de los grupos de entrada?", "Ascensor",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //MDG.2011-04-26.Confirmacion de seguridad
                if (_line2GroupSupplier.CurrentIndex > 1) //MDG.2011-06-15//2)
                    _line2GroupSupplier.CurrentIndex -= 1;
                else
                    _line2GroupSupplier.CurrentIndex = -1;
        }

        private void buttonAñadirGrupoEncajadora_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("¿Desea añadir un grupo de Pasaportes a la Encajadora Prodec?", "Encajadora Prodec", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)//MDG.2011-04-26.Confirmacion de seguridad
            //{
            //GrupoPasaportes ItemToAdd = _line2GroupSupplier.GetItem(360);
            GrupoPasaportes ItemToAdd = _line2GroupSupplier.GetItem((int) numericUpDownGrupoEncajadora.Value);
            if (ItemToAdd != null)
            {
                _prodec.GruposPasaportes.Add(ItemToAdd);
                if (numericUpDownGrupoEncajadora.Value > 0) //MDG.2011-06-13. Evitamos excepción en el control
                    numericUpDownGrupoEncajadora.Value = numericUpDownGrupoEncajadora.Value - 1;
                else
                    numericUpDownGrupoEncajadora.Value = 0;
                MessageBox.Show("Grupo añadido correctamente.");
            }
            else
            {
                MessageBox.Show(
                    "El grupo no se puede añadir. El índice no es válido en el catálogo cargado en la Línea 2.");
            }
            //}
        }

        private void buttonIncrementarIndiceVistaTramo1_Click(object sender, EventArgs e)
        {
            //MDG.2011-05-18
            if (Tramo1ViewIndex < (_transporte1.Grupos.Count() - 20))
            {
                Tramo1ViewIndex++;
            }
            else if (_transporte1.Grupos.Count() <= 20)
            {
                Tramo1ViewIndex = 0;
            }
            else
            {
                Tramo1ViewIndex = _transporte1.Grupos.Count() - 20;
            }
            RefreshTramo1();
        }

        private void buttonDecrementarIndiceVistaTramo1_Click(object sender, EventArgs e)
        {
            //MDG.2011-05-18
            if (Tramo1ViewIndex <= 1)
            {
                Tramo1ViewIndex = 0;
            }
            else if (Tramo1ViewIndex <= (_transporte1.Grupos.Count() - 20))
            {
                Tramo1ViewIndex--;
            }
            else if (_transporte1.Grupos.Count() <= 20)
            {
                Tramo1ViewIndex = 0;
            }
            else
            {
                Tramo1ViewIndex = _transporte1.Grupos.Count() - 20;
            }
            RefreshTramo1();
        }

        private void buttonIncrementarIndiceVistaTramo2_Click(object sender, EventArgs e)
        {
            //MDG.2011-05-18
            if (Tramo2ViewIndex < (_transporte2.Grupos.Count() - 5))
            {
                Tramo2ViewIndex++;
            }
            else if (_transporte2.Grupos.Count() <= 5)
            {
                Tramo2ViewIndex = 0;
            }
            else
            {
                Tramo2ViewIndex = _transporte2.Grupos.Count() - 5;
            }
            RefreshTramo2();
        }

        private void buttonDecrementarIndiceVistaTramo2_Click(object sender, EventArgs e)
        {
            //MDG.2011-05-18
            if (Tramo2ViewIndex <= 1)
            {
                Tramo2ViewIndex = 0;
            }
            else if (Tramo2ViewIndex <= _transporte2.Grupos.Count() - 5)
            {
                Tramo2ViewIndex--;
            }
            else if (_transporte2.Grupos.Count() <= 5)
            {
                Tramo2ViewIndex = 0;
            }
            else
            {
                Tramo2ViewIndex = _transporte2.Grupos.Count() - 5;
            }
            RefreshTramo2();
        }


        #region Notificaciones al modelo
        
        private void ActionQueryHandler(object sender, EventArgs e)
        {
            Object b = sender;
            if (b.Equals(btnActiveModeStart))
            {
                //if (chkPasoAPaso.Checked)
                //{
                //    _controllerSystem.StartModeWithStepByStep();
                //}
                //else
                //{
                    _controllerSystem.StartMode2();
                //}
            }
            else if (b.Equals(btnActiveModeStop))
            {
                //_controllerSystem.StopModeAndDeativate2();
                //MDG.2013-02-06.En vez de ordenar stop pone en manual porque no hay configuradas paradas controladas
                _controllerSystem.ManualMode2();
            }
            else if (b.Equals(btnAuto))
            {
                _controllerSystem.AutomaticMode2();
            }
            //else if (b.Equals(btnVOrig))
            //{
            //    _controllerSystem.BackToOriginMode2();
            //}
            else if (b.Equals(btnManual))
            {
                _controllerSystem.ManualMode2();
            }
            else if (b.Equals(btnRearme))
            {
                _controllerSystem.Rearmament2();
                //lstDiagnosis.Items.Clear();
            }
        }
        
        #endregion

        #region Notificaciones del modelo

        private void NewSystemControlNotificationHandler(object obj, SystemControl.EventNotificationArgs e)
        {
            switch (e.IdNotification)
            {
                //case SystemControl.IdNotification.GlobalAddedStatusOk:
                //    lblAireOk.BackColor = ((bool)e.Value) ? Color.Green : Color.Red;
                //    break;
                case SystemControl.IdNotification.ConnectionCommand2:
                    lblConexionMando.BackColor = ((bool)e.Value) ? Color.Green : Color.Red;
                    break;
                //case SystemControl.IdNotification.Diagnosis:
                //    lblDiagnosis.BackColor = ((bool)e.Value) ? Color.Red : Color.Green;
                //    break;
                //case SystemControl.IdNotification.Origin:
                //    lblOrigen.BackColor = ((bool)e.Value) ? Color.Green : Color.Red;
                //    break;
                //case SystemControl.IdNotification.ProtectionsCanceled:
                //    lblProteccionesAnuladas.BackColor = ((bool)e.Value) ? Color.Red : Color.Green;
                //    break;
                case SystemControl.IdNotification.ProtectionsOK2:
                    lblProteccionesOk.BackColor = ((bool)e.Value) ? Color.Green : Color.Red;
                    break;
                //case SystemControl.IdNotification.BootSystem:
                //    lblSistemaArranque.BackColor = ((bool)e.Value) ? Color.Green : Color.Red;
                //    break;
                case SystemControl.IdNotification.OperationMode2:
                    lblModoActual.Text = e.Value.ToString();
                    //_manualManager.EnableManualPanel((Mode)(e.Value) == Mode.Manual);
                    break;

                case SystemControl.IdNotification.ModeStatus2:
                    var modeStatus = (ControlModeStatus)e.Value;
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
        #endregion

    }
}