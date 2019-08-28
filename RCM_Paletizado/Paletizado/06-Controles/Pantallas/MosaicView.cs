using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Idpsa.Control.Tool;

//
//using Idpsa.Paletizado;//

namespace Idpsa.Paletizado
{
    public partial class MosaicView : UserControl
    {
        private PaletizerEventArgs _eventData;
        public event ForcedAddBox AddBox;
        public event ForcedQuitBox QuitBox;

        public delegate void ForcedAddBox(object sender);
        public delegate void ForcedQuitBox(object sender);

        private bool _reverseFirstFlat;


        public MosaicView()
        {
            InitializeComponent();
            var font = new Font("Microsoft Sans Serif", 9.00F, FontStyle.Bold, GraphicsUnit.Point, ((0)));
            SetFont(font);
            _reverseFirstFlat = false;
        }


        public void SubscriveToEnvents(PaletizerDecorator paletizer)
        {
            paletizer.Changed += PaletizerChangedHandler;
            paletizer.Created += NewPaletizerHandler;
        }

        private void PaletizerChangedHandler(object sender, PaletizerEventArgs e)
        {
            if (ValidateEventData(e))
            {
                FillVariablePaletizerData();
                if (e.Name == "Paletizado final linea japonesa" && e.PaletizerData.MosaicTypes.Count() == 5)
                    _reverseFirstFlat = true;
                else
                    _reverseFirstFlat = false;
                DrawMosaic();
                //MDG.2010-11-29.Añadimos Salvado en fichero de variables de paletizado ante evento de cambio de las mismas
                //Sys.Production.SaveCatalogs();
                //MDG.2011-03-24.Quito Salvado de catalogos porque lo hace mientras se estan cargando y no se cargan bien//ControlLoop<IDPSASystemPaletizado>.Instance.Sys.Production.SaveCatalogs();
            }
        }

        private void NewPaletizerHandler(object sender, PaletizerEventArgs e)
        {
            if (ValidateEventData(e))
            {
                FillPermanetPaletizerData();
                FillVariablePaletizerData();
                if (e.Name == "Paletizado final linea japonesa" && e.PaletizerData.MosaicTypes.Count() == 5)
                    _reverseFirstFlat = true;
                else
                    _reverseFirstFlat = false;
                DrawMosaic();
                //MDG.2010-11-29.Añadimos Salvado en fichero de variables de paletizado ante evento de cambio de las mismas
                //Sys.Production.SaveCatalogs();
                ////e.TrySave();
            }
        }

        private void FillPermanetPaletizerData()
        {
            lbTitle.Text = _eventData.Name;
            lbIDPalet.Text = _eventData.IDPalet;

            lbPaletType.Text = _eventData.PaletizerData.Palet.Type.ToString();
        }

        private void FillVariablePaletizerData()
        {
            lbMosaicType.Text = _eventData.PaletizerData.MosaicTypes[_eventData.Flat].ToString();
            lbCajasTotales.Text = _eventData.TotalCurrentFlatItems + " / " + _eventData.TotalItems;
            lbCajasPuestas.Text = _eventData.CurrentItems.ToString();
            lbActualFloor.Text = (_eventData.Flat + 1) + " / " +
                                 _eventData.PaletizerData.MosaicTypes.Count;

            if (_eventData.LastElement != null)
                lbUltimaCaja.Text = ((CajaPasaportes) _eventData.LastElement).Id;
        }

        private void DrawMosaic()
        {
            if (_eventData != null)
            {
                if (_eventData.PaletizerData != null)
                {
                    bool b;
                    if (_eventData.Flat==0)
                        b = _reverseFirstFlat;
                    else
                        b = false;
                    MosaicDraw.DrawMosaicDynamic(picture, _eventData.PaletizerData.Item,
                                                 _eventData.PaletizerData.Palet,
                                                 _eventData.PaletizerData.MosaicTypes[_eventData.Flat], _eventData.Pos,
                                                 Spin.S270,b);
                }
            }
        }

        public void Paint()
        {
            DrawMosaic();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawMosaic();
        }


        private bool ValidateEventData(PaletizerEventArgs e)
        {
            if (e == null) return false;
            ;
            _eventData = e;
            return true;
        }

        private void btListado_Click(object sender, EventArgs e)
        {
            //MDG.2011-06-16
            listViewCajas.Items.Clear();
            if (_eventData != null)
            {
                ////MDG.2011-06-16.Prueba insertar ultimo elemento
                //if (_eventData.LastElement != null)
                //{
                //    var myLstItem = new ListViewItem((((CajaPasaportes)_eventData.LastElement).CatalogIndex + 1).ToString());
                //    myLstItem.Name = ((CajaPasaportes)_eventData.LastElement).Id;
                //    myLstItem.SubItems.Add(((CajaPasaportes)_eventData.LastElement).Id);
                //    myLstItem.SubItems.Add(((CajaPasaportes)_eventData.LastElement).IDPalet.ToString());
                //    myLstItem.SubItems.Add((((CajaPasaportes)_eventData.LastElement).Flat+1).ToString());
                //    myLstItem.SubItems.Add((((CajaPasaportes)_eventData.LastElement).Pos+1).ToString());
                //    listViewCajas.Items.Add(myLstItem);
                //    //MessageBox.Show("Cajas:\n\n" + "1\n" + "2\n", "Listado Cajas Palet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    //MessageBox.Show("Cajas:\n\n" + "1\n" + "2\n", "Listado Cajas Palet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}

                if (_eventData.Boxes != null)
                {
                    //MDG.2011-06-17
                    if (_eventData.Boxes.Count() > 0)
                    {
                        foreach (CajaPasaportes box in _eventData.Boxes)
                        {
                            listViewCajas.Items.Add(new ListViewItem(new[]
                                                                         {
                                                                             ((box).CatalogIndex + 1).
                                                                                 ToString(),
                                                                             (box).Id,
                                                                             (box).IDPalet,
                                                                             ((box).Flat + 1).ToString()
                                                                             ,
                                                                             ((box).Pos + 1).ToString()
                                                                         }));
                        }
                    }

                    ////MDG.2011-06-16
                    //for (int i = 0; i <= _eventData.DataToStore.Boxes.Count() - 1; i++)
                    //{
                    //    listViewCajas.Items.Add(new ListViewItem(new string[] 
                    //        {
                    //            (i+1).ToString(),
                    //            _eventData.DataToStore.Boxes[i].ToString()
                    //        }));

                    //}
                }
            }
            else
            {
                MessageBox.Show("No se puede mostrar el listado de cajas en este momento", "Listado Cajas Palet",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetFont(Font font)
        {
            lbTitle.Font = font;
            lbIDPalet.Font = font;
            lbMosaicType.Font = font;
            lbPaletType.Font = font;
            lbCajasTotales.Font = font;
            lbCajasPuestas.Font = font;
            lbActualFloor.Font = font;
            lbUltimaCaja.Font = font;
        }

        private void OnAddBox()
        {            
            AddBox(this);
        }

        private void OnQuitBox()
        {
            QuitBox(this);
        }

        private void buttonQuitarCaja_Click(object sender, EventArgs e)
        {
            //if (!_sys.Control.InActiveMode(Mode.Automatic))
            {
                if(_eventData!=null)
                    if (_eventData.Boxes.Count() > 0)
                    {
                        OnQuitBox();
                    }
                //MDG.2011-06-14
                //_eventData.PaletizerData
            }
        }

        private void buttonAñadirCaja_Click(object sender, EventArgs e)
        {
            if (_eventData != null)
                if (_eventData.Boxes.Count() < _eventData.TotalItems)
                    OnAddBox();
        }

        public void ShowManualButton(bool b)
        {
            buttonAñadirCaja.Enabled = b;
            buttonQuitarCaja.Enabled = b;
        }



    }
}