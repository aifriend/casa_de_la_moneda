using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Idpsa.Control;
using Idpsa.Paletizado;
using RCMCommonTypes;

namespace Idpsa
{
    public partial class UCLoadCatalogo : UserControl
    {
        #region Delegates

        public delegate void NewCatalogHandler(object sender, DatosCatalogoPaletizado catalog);

        #endregion

        private DatosCatalogoPaletizado _catalog;
        private Dictionary<string, DatosCatalogoPaletizado> _catalogs;

        private IDLine _idLine;
        private FormPaletizadoView _paletizadoView;
        private DatosCatalogoPaletizado _selectedCatalog;
        private IdpsaSystemPaletizado _sys;

        public UCLoadCatalogo()
        {
            InitializeComponent();
        }

        public DatosCatalogoPaletizado Catalogo
        {
            get { return _catalog; }
            set
            {
                _catalog = value;
                LoadDataFromCatalog(_catalog);

                if (_catalog != null)
                    tbCatalogoActual.Text = _catalog.Name;
            }
        }

        public event NewCatalogHandler NewCatalog;

        //public void Initialize(IDLine idLine)
        public void Initialize(IDLine idLine, IdpsaSystemPaletizado sys)
        {
            _idLine = idLine;
            _sys = sys;
            _catalogs = new Dictionary<string, DatosCatalogoPaletizado>();
            LoadCatalogs();
            LoadCatalogsInCbCatalogs();

            _paletizadoView = new FormPaletizadoView();
        }

        private void LoadCatalogs()
        {
            var directory = new DirectoryInfo(ConfigPaletizadoFiles.CatalogsPaletizado);
            FileInfo[] files = directory.GetFiles();
            var bFormatter = new BinaryFormatter();
            _catalogs.Clear();
            try //MDG.2011-03-29.Añadimos tratamiento de excepcion porque a veces falla al crear nuevo catalogo
            {
                foreach (FileInfo file in files)
                {
                    //mirar
                    using (var readFile = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                    {
                        try
                        {
                            var bFormat = new BinaryFormatter();
                            var catalog = (DatosCatalogoPaletizado) bFormat.Deserialize(readFile);
                            if (!_catalogs.ContainsKey(catalog.Name) && catalog.IDLine == _idLine)
                                _catalogs.Add(catalog.Name, catalog);
                        }
                        catch (Exception Ex)
                        {
                            //MDG.2011-03-29. Tratamiento de Excepcion
                            //TO DO
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                //MDG.2011-03-29. Tratamiento de Excepcion
                //TO DO
            }
        }

        private void LoadCatalogsInCbCatalogs()
        {
            cbCatalogs.Items.Clear();
            cbCatalogs.SelectedIndex = -1;

            try //MDG.2011-07-11.Da excepcion al crear catalogo
            {
                foreach (string str in _catalogs.Keys)
                {
                    cbCatalogs.Items.Add(str);
                }
            }
            catch (Exception ex)
            {
                ;
            }
        }

        public void EnableTextBoxPasaportesIniFinal()
        {
            tbPasaporteInicial.BackColor = Color.White;
            tbPasaporteFinal.BackColor = Color.White;
            tbPasaporteInicial.Enabled = true;
            tbPasaporteFinal.Enabled = true;
        }

        public void DisableTextBoxPasaportesIniFinal()
        {
            tbPasaporteInicial.BackColor = Color.LightBlue;
            tbPasaporteFinal.BackColor = Color.LightBlue;
            tbPasaporteInicial.Enabled = false;
            tbPasaporteFinal.Enabled = false;
        }

        public void EnableButtons()
        {
            btAceptar.Enabled = true;
        }

        public void DisableButtons()
        {
            btAceptar.Enabled = false;
        }

        private void LoadDataFromCatalog(DatosCatalogoPaletizado catalog)
        {
            if (catalog == null) return;
            tbPais.Text = catalog.TipoPasaporte.Country.ToString();
            tbCategoria.Text = catalog.TipoPasaporte.Type.ToString();
            tbPaletizado.Text = catalog.PaletizerDefinition.Name;
            tbNDigitos.Text = catalog.TipoPasaporte.NDigits.ToString();
            tbNLetras.Text = catalog.TipoPasaporte.NChars.ToString();
            tbPasaporteInicial.Text = catalog.IdPasaporteIni;
            tbPasaporteFinal.Text = catalog.IdPasaporteEnd;
            tbTieneRfid.Text = catalog.TipoPasaporte.RfIdType.ConvertToString();
            tbPeso.Text = catalog.TipoPasaporte.Weight.ToString();
            tbGrosor.Text = catalog.TipoPasaporte.Thickness.ToString();
            fechaLaminacion.Text = catalog.TipoPasaporte.TieneFechaDeLaminacion ? catalog.FechLaminacion : "No";
        }

        private void OnNewCatalog(DatosCatalogoPaletizado newCatalog)
        {
            if ((NewCatalog != null) && (newCatalog != null))
            {
                NewCatalog(this, newCatalog);
            }
        }

        private void btAceptar_Click(object sender, EventArgs e)
        {
            if (_selectedCatalog != null)
            {
                if (!_sys.Control.InActiveMode(Mode.Automatic))
                {
                    OnNewCatalog(_selectedCatalog);
                    LoadCatalogsInCbCatalogs();
                }
                else
                {
                    MessageBox.Show(@"No se pueden cargar catálogos en modo automático", @"Carga Catálogo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(@"Catálogo no seleccionado", @"Carga Catálogo", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void cbCatalogs_IndexChanged(object sender, EventArgs e)
        {
            _selectedCatalog = _catalogs.ContainsKey(cbCatalogs.Text) ? _catalogs[cbCatalogs.Text] : null;
        }

        public void SetCatalogsChangedHander(object obj, EventArgs e)
        {
            LoadCatalogs();
            LoadCatalogsInCbCatalogs();
        }

        private void btVisualizar_Click(object sender, EventArgs e)
        {
            if (_selectedCatalog == null)
                return;

            if (_selectedCatalog.PaletizerDefinition == null)
                return;

            _paletizadoView.SetData(_selectedCatalog.PaletizerDefinition);
            _paletizadoView.Show();
        }

        private void btnWeightModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbNuevoPeso.Text == "")
                {
                    MessageBox.Show("El valor introducido para peso del pasaporte está en blanco",
                                        "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    double peso = double.Parse(tbNuevoPeso.Text);
                    if (peso < 0)
                    {
                        MessageBox.Show("El valor introducido para peso del pasaporte no puede ser negativo",
                                        "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tbNuevoPeso.Focus();
                    }
                    else
                    {
                        if (_selectedCatalog != null)
                        {
                            _selectedCatalog.TipoPasaporte.NewWeight(peso);
                            if (!_sys.Control.InActiveMode(Mode.Automatic))
                            {
                                OnNewCatalog(_selectedCatalog);
                                LoadCatalogsInCbCatalogs();
                            }
                            else
                            {
                                MessageBox.Show(@"No se pueden cargar catálogos en modo automático", @"Carga Catálogo",
                                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show(@"Catálogo no seleccionado", @"Carga Catálogo", MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                        }
                    }
                }

            }
            catch (Exception)
            {
                MessageBox.Show(@"Operacion incorrecta.", @"Error en asignacion de peso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    }
}