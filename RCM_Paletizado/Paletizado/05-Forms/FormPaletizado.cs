using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Idpsa.Control.Tool;
using Idpsa.Paletizado;

namespace Idpsa
{
    public partial class FormPaletizado : Form
    {
        private readonly FileSystemWatcher _fileWatcher;
        private readonly string _sDirectory = ConfigPaletizadoFiles.PaletizadosPaletizado;
        public EventHandler<EventArgs> NewPaletizerDefinition;
        private MosaicType? _mosaicToAdd;
        private MosaicType? _mosaicToRemove;
        private PaletTypes? _paletSelected;

        public FormPaletizado()
        {
            InitializeComponent();


            lbMosaics.SelectedIndexChanged += delegate
                                                  {
                                                      if (lbMosaics.SelectedIndex != -1)
                                                          _mosaicToAdd = (MosaicType) lbMosaics.SelectedItem;
                                                      else
                                                          _mosaicToAdd = null;
                                                  };

            lbMosaicsAdded.SelectedIndexChanged += delegate
                                                       {
                                                           if (lbMosaicsAdded.SelectedIndex != -1)
                                                           {
                                                               _mosaicToRemove =
                                                                   (MosaicType) lbMosaicsAdded.SelectedItem;
                                                               OnPaint(null);
                                                           }
                                                           else
                                                               _mosaicToRemove = null;
                                                       };

            cbPalets.SelectedIndexChanged += delegate
                                                 {
                                                     if (cbPalets.SelectedIndex != -1)
                                                     {
                                                         _paletSelected = (PaletTypes) cbPalets.SelectedItem;
                                                     }
                                                     else
                                                         _paletSelected = null;
                                                 };

            FormClosing += delegate(object sender, FormClosingEventArgs e)
                               {
                                   e.Cancel = true;
                                   Hide();
                               };


            _fileWatcher = new FileSystemWatcher
                               {
                                   Path = _sDirectory,
                                   Filter = "*.*",
                                   NotifyFilter = NotifyFilters.FileName
                               };

            _fileWatcher.Created += FileWatcherHandler;
            _fileWatcher.Deleted += FileWatcherHandler;

            _fileWatcher.EnableRaisingEvents = true;

            LoadPalets();
            LoadMosaics();
        }


        private void FileWatcherHandler(object obj, FileSystemEventArgs e)
        {
            OnNewPaletizerDefinition();
        }


        private void LoadMosaics()
        {
            IEnumerable<MosaicType> mosaics = Enum.GetValues(typeof (MosaicType)).Cast<MosaicType>();
            foreach (MosaicType mosaic in mosaics)
                lbMosaics.Items.Add(mosaic);
        }

        private void LoadPalets()
        {
            IEnumerable<PaletTypes> palets = Enum.GetValues(typeof (PaletTypes)).Cast<PaletTypes>();
            foreach (PaletTypes palet in palets)
                cbPalets.Items.Add(palet);

            cbPalets.SelectedIndex = 0;
        }


        private void btAñadir_Click(object sender, EventArgs e)
        {
            if (_mosaicToAdd != null && _mosaicToAdd.HasValue)
            {
                if (lbMosaicsAdded.Items.Count > 5)
                {
                    MessageBox.Show("El número de pisos máximo para un paletizado es 5", "Especificación errónea",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                lbMosaicsAdded.Items.Add(_mosaicToAdd.Value);

                if (lbMosaicsAdded.Items.Count == 5)
                {
                    btAñadir.Enabled = false;
                }

                if (lbMosaicsAdded.Items.Count > 0)
                {
                    btEliminar.Enabled = true;
                }
            }
        }

        private void btEliminar_Click(object sender, EventArgs e)
        {
            if (lbMosaicsAdded.SelectedItem != null)
            {
                lbMosaicsAdded.Items.Remove(lbMosaicsAdded.SelectedItem);
                if (lbMosaicsAdded.Items.Count == 0)
                {
                    btEliminar.Enabled = false;
                }

                if (lbMosaicsAdded.Items.Count < 5)
                {
                    btAñadir.Enabled = true;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            PaintMosaic();
            base.OnPaint(e);
        }

        private void PaintMosaic()
        {
            if (_mosaicToRemove.HasValue && _paletSelected.HasValue)
            {
                IPaletizable item = PaletizadoElements.Create(PaletizableTypes.box);
                IPalet palet = PaletizadoElements.Create(_paletSelected.Value);

                var m = new Mosaic(item, palet, _mosaicToRemove.Value);
                m.DrawMosaic(pbMosaic, Spin.S270);
            }
            else
            {
                MosaicDraw.Erase(pbMosaic);
            }
        }

        protected virtual void OnNewPaletizerDefinition()
        {
            EventHandler<EventArgs> temp = NewPaletizerDefinition;
            if (temp != null)
                temp(this, EventArgs.Empty);
        }

        private PaletizerDefinition Validar()
        {
            if (tbName.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Debe especificarse un nombre para el paletizado", "Especifición errónea",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbName.Focus();
                return null;
            }

            if (lbMosaicsAdded.Items.Count < 1)
            {
                MessageBox.Show("El paletizado debe estar compuesto por algún mosaico", "Especifición errónea",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                lbMosaicsAdded.Focus();
                return null;
            }

            if (cbPalets.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionarse un tipo de palet", "Especifición errónea", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                cbPalets.Focus();
                return null;
            }

            var paletizerDefinition = new PaletizerDefinition(tbName.Text.Trim());
            paletizerDefinition.Palet = PaletizadoElements.Create(_paletSelected.Value);
            paletizerDefinition.Item = PaletizadoElements.Create(PaletizableTypes.box);
            paletizerDefinition.Separator = PaletizadoElements.Create(SeparatorTypes.Carton);
            paletizerDefinition.CoparerType = ComparerMosaicTypes.Tipe1;
            foreach (MosaicType m in lbMosaicsAdded.Items)
            {
                paletizerDefinition.MosaicTypes.Add(m);
            }

            return paletizerDefinition;
        }

        private void btAceptar_Click(object sender, EventArgs e)
        {
            PaletizerDefinition paletizerDefinition = null;
            if ((paletizerDefinition = Validar()) != null)
            {
                string filePath = Path.Combine(_sDirectory, paletizerDefinition.Name);
                if (File.Exists(filePath))
                {
                    MessageBox.Show("El nombre de paletizado seleccionado ya existe", "Especifición errónea",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbName.Focus();
                }

                using (var writeFile = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    //try
                    //{
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(writeFile, paletizerDefinition);
                    //}
                    //catch(Exception ex)
                    //{
                    //    MessageBox.Show(ex.Message, "Especifición errónea", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}
                }
            }
        }

        private void ClearForm()
        {
            lbMosaicsAdded.Items.Clear();
            tbName.Text = String.Empty;
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            ClearForm();
            Close();
        }
    }
}