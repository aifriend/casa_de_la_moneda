using System;
using System.Windows.Forms;
using Idpsa.Control.Tool;
using Idpsa.Paletizado;

namespace Idpsa
{
    public partial class FormPaletizadoView : Form
    {
        private PaletizerDefinition _paletizerDefinition;
        private MosaicType? _selectedMosaicType;

        public FormPaletizadoView()
        {
            InitializeComponent();
            FormClosing += delegate(object sender, FormClosingEventArgs e)
                               {
                                   e.Cancel = true;
                                   Hide();
                               };
            lbMosaics.SelectedIndexChanged += delegate
                                                  {
                                                      if (lbMosaics.SelectedIndex != -1)
                                                      {
                                                          _selectedMosaicType = (MosaicType) lbMosaics.SelectedItem;
                                                          PaintMosaic();
                                                      }
                                                      else
                                                      {
                                                          _selectedMosaicType = null;
                                                      }
                                                  };
        }

        public void SetData(PaletizerDefinition paletizerDefinition)
        {
            _paletizerDefinition = paletizerDefinition;
            LoadData();
        }

        private void LoadData()
        {
            if (_paletizerDefinition != null)
            {
                tbName.Text = _paletizerDefinition.Name;
                tbPalet.Text = _paletizerDefinition.Palet.Type.ToString();
            }

            lbMosaics.Items.Clear();

            foreach (MosaicType mosaic in _paletizerDefinition.MosaicTypes)
                lbMosaics.Items.Add(mosaic);

            lbMosaics.SelectedIndex = 0;

            PaintMosaic();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            PaintMosaic();
        }

        private void PaintMosaic()
        {
            if (_paletizerDefinition != null && _selectedMosaicType.HasValue)
            {
                var m = new Mosaic
                    (_paletizerDefinition.Item,
                     _paletizerDefinition.Palet,
                     _selectedMosaicType.Value);


                m.DrawMosaic(pbMosaic, Spin.S270);
            }
            else
            {
                MosaicDraw.Erase(pbMosaic);
            }
        }

        private void btAceptar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}