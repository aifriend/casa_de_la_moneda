using System;
using System.Drawing;
using System.Windows.Forms;

namespace Idpsa.Paletizado
{
    public partial class ManualBoxReprocessor : UserControl, IManualReprocessor
    {
        private Func<string, CajaPasaportes> _boxGetter;
        private Zebra_CajasPrinter _printer;
        private IManualReprocessSolicitor _solitor;

        public ManualBoxReprocessor()
        {
            InitializeComponent();
        }

        public Zebra_CajasPrinter GetPrinter()
        {
            return _printer;
        }

        public void SetPrinter(Zebra_CajasPrinter printer)
        {
            _printer = printer;
        }


        public void SetBoxGetter(Func<string, CajaPasaportes> boxGetter)
        {
            _boxGetter = boxGetter;
        }

        public void SetFocusOnBarcode()
        {
            tbBarcode.Focus();
        }

        private void tbBarcode_TextChanged(object sender, EventArgs e)
        {
            //var txt = tbBarcode.Text;
            string txt = tbBarcode.Text.ToUpper(); //MDG.2011-05-18.Para que lo interprete bien siempre

            if (txt.Length == Zebra_CajasPrinter.BarcodeLength)
            {
                Zebra_CajasPrinter.BarcodeData barcodeData = null;
                string errorMessage = null;

                if (_printer.TryExtractDataOfBarcode(txt, out barcodeData, out errorMessage))
                {
                    CajaPasaportes caja = (_solitor.WaitingReprocess)
                                              ? _solitor.GetBoxToReproccess()
                                              : _boxGetter(barcodeData.IdCaja);

                    if (barcodeData.CorrespondsTo(caja))
                    {
                        FillBarcodeData(caja);
                        if (_solitor.WaitingReprocess && !caja.IsBarcodeReadedCorrect())
                        {
                            caja.CodigoBarrasLeido = caja.CodigoBarras;
                        }
                        //MDG.2011-06-16
                        if (_solitor.WaitingReprocess && !caja.IsIdNotRepeated())
                        {
                            caja.EtiquetaDuplicada = false;
                        }
                    }
                }
                else
                {
                    MessageBox.Show(errorMessage, "Error lectura código barras", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
            }
        }

        private void FillBarcodeData(CajaPasaportes caja)
        {
            tbCountry.Text = caja.TipoPasaporte.Country.Name;
            tbIdCaja.Text = tbIdBoxToPrint.Text = caja.Id;
            tbRfid.Text = caja.TipoPasaporte.RfIdType.ConvertToString();
            tbTipoPasaporte.Text = caja.TipoPasaporte.Type.ConvertToString();
            //MDG.2010-12-01.Mostramos en pantalla el fallo que dio la caja
            if (caja.IsCorrectWeight())
                labelErrorPeso.BackColor = Color.Gray;
            else
                labelErrorPeso.BackColor = Color.Red;
            if (caja.IsBarcodeReadedCorrect())
                labelErrorLectura.BackColor = Color.Gray;
            else
                labelErrorLectura.BackColor = Color.Red;
            //MDG.2011-06-16
            if (caja.IsIdNotRepeated())
                labelErrorEtiquetaDuplicada.BackColor = Color.Gray;
            else
                labelErrorEtiquetaDuplicada.BackColor = Color.Red;
        }

        private void ClearBarcodeData()
        {
            tbBarcode.Text = String.Empty;
            tbCountry.Text = String.Empty;
            tbIdCaja.Text = String.Empty;
            tbRfid.Text = String.Empty;
            tbTipoPasaporte.Text = String.Empty;
            //MDG.2010-12-01.Mostramos en pantalla el fallo que dio la caja
            labelErrorPeso.BackColor = Color.Gray;
            labelErrorLectura.BackColor = Color.Gray;
            labelErrorEtiquetaDuplicada.BackColor = Color.Gray;
        }

        private void btValidar_Click(object sender, EventArgs e)
        {
            if (_solitor.WaitingReprocess)
            {
                //if (_solitor.GetBoxToReproccess().IsBarcodeReadedCorrect())
                if ((_solitor.GetBoxToReproccess().IsBarcodeReadedCorrect())
                    && (_solitor.GetBoxToReproccess().IsIdNotRepeated())) //MDG.2011-06-16
                {
                    _solitor.GetBoxToReproccess().ValidacionManual(); //MCR.2015.03.12
                    _solitor.OnReprocess();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string id = tbIdBoxToPrint.Text;
            if (String.IsNullOrEmpty(id))
            {
                MessageBox.Show("Introduzca el número de caja", "Especifición errónea", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                tbIdBoxToPrint.Focus();
                return;
            }
            CajaPasaportes box = _boxGetter(id);
            if (box == null)
            {
                MessageBox.Show("El número de caja:  \n  " + id + " \nno se encuentra en catálogo ",
                                "Especifición errónea", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbIdBoxToPrint.Focus();
                return;
            }
            _printer.Print(box,false);
            //MDG.2011-05-18. No borramos el numero de la caja en el control por si se quiere volver a imprimir//tbIdBoxToPrint.Text = String.Empty;            
            tbBarcode.Focus();
        }

        private void btClearIdCajaToPrint_Click(object sender, EventArgs e)
        {
            tbIdBoxToPrint.Text = String.Empty;
            tbIdBoxToPrint.Focus();
        }

        private void btClearBarcode_Click(object sender, EventArgs e)
        {
            ClearBarcodeData();
            tbBarcode.Focus();
        }

        #region Miembros de IManualReprocessor

        public void AttachToSolicitor(IManualReprocessSolicitor solicitor)
        {
            _solitor = solicitor;
        }

        public void OnNewRequest()
        {
            ClearBarcodeData();
            tbIdBoxToPrint.Text = _solitor.GetBoxToReproccess().Id;
            tbBarcode.Focus();
        }

        #endregion
    }
}