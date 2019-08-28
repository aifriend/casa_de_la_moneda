using System;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control;

namespace Idpsa.Paletizado
{
    public partial class UBascula : UserControl
    {
        private IActivable _botonConexionMandoLuz;
        private RuhlamatManualFeed _manualFeedRuhlamat;
        private IdpsaSystemPaletizado _sys;
        private TON _timer;

        public UBascula()
        {
            InitializeComponent();
        }

        public UBascula Initialize(IdpsaSystemPaletizado sys)
        {
            _sys = sys;
            _timer = new TON();
            _manualFeedRuhlamat = _sys.ManualFeedRuhlamat;
            _manualFeedRuhlamat.Bascula.WeightObtained += WeightObtainedHandler;
            progressBarWeight.Value = 0;
            pnResult.BackColor = Color.Red;
            lbState.BackColor = Color.LightCoral;
            _botonConexionMandoLuz = _sys.Bus.Out("H901");
            return this;
        }

        private void WeightObtainedHandler(double groupWeight)
        {
            //Update limits
            lbNominalWeight.Text = _manualFeedRuhlamat.GroupNominalWeight.ToString();
            progressBarWeight.Maximum = (int) _manualFeedRuhlamat.MaxPassportWeight;
            lbPassWeightMax.Text = decimal.Round((decimal) _manualFeedRuhlamat.MaxPassportWeight, 2).ToString();

            //Show difference and result
            double actualWeightDiff = Math.Abs(groupWeight - _manualFeedRuhlamat.GroupNominalWeight);
            tbActualDiff.Text = decimal.Round((decimal) actualWeightDiff, 2).ToString();
            double weightMargin = (_manualFeedRuhlamat.MaxPassportWeight*RuhlamatManualFeed.MaxErrorAllowed);
            lbWeightMargin.Text = decimal.Round((decimal) weightMargin, 2).ToString();
            if (actualWeightDiff < weightMargin)
            {
                pnResult.Text = @"OK";
                pnResult.BackColor = Color.Green;
            }
            else
            {
                pnResult.Text = @"ERROR";
                pnResult.BackColor = Color.Red;
            }

            //Update progress bar
            progressBarWeight.Value = 0;
            if (actualWeightDiff > progressBarWeight.Maximum)
                progressBarWeight.Value = (int) _manualFeedRuhlamat.MaxPassportWeight;
            else
                progressBarWeight.Value = (int) actualWeightDiff;

            //Update group weight
            lbActualWeight.Text = groupWeight.ToString();
        }

        private void bCalibPassport_Click(object sender, EventArgs e)
        {
            if (_sys.Control.OperationMode != Mode.Manual)
            {
                lbError.Text = @"Se requiere modo manual para calibrar";
            }
            else if (!_botonConexionMandoLuz.Value())
            {
                lbError.Text = @"Se requiere conexion de mando activada";
            }
            else
            {
                string strWeight;
                lbError.Text = string.Empty;
                lbState.BackColor = Color.LightCoral;
                lbState.Text = @"Calibrando...";
                Refresh();
                bCalibPassport.Enabled = false;

                _manualFeedRuhlamat.Bascula.Connect();

                while (!_manualFeedRuhlamat.Bascula.ReadCode(out strWeight))
                    if (_timer.Timing(5000))
                        break;

                if (!string.IsNullOrEmpty(strWeight))
                {
                    var numWeight = (double) decimal.Round(decimal.Parse(strWeight), 2);
                    _manualFeedRuhlamat.GroupNominalWeight = numWeight;
                    numWeight = numWeight/25;
                    _manualFeedRuhlamat.MaxPassportWeight = numWeight;
                    numWeight = (double) decimal.Round((decimal) numWeight, 2);
                    lbCalibration.Text = numWeight.ToString();
                    _manualFeedRuhlamat.Bascula.ReadCode(out strWeight);

                    lbState.Text = @"Terminado";
                    lbState.BackColor = Color.LightGreen;
                }
                else
                {
                    lbState.Text = "Calibracion\nfallida";
                    lbState.BackColor = Color.LightCoral;
                    lbError.Text = @"Pulse calibrar otra vez";
                }

                _manualFeedRuhlamat.Bascula.Disconnect();
                bCalibPassport.Enabled = true;
            }
        }
    }
}