using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Idpsa.Control.Engine;
using Idpsa.Paletizado;
using RCMCommonTypes;
using RECatalogManager;

namespace Idpsa
{
    public partial class FrmCreatePaletizerCatalog : Form
    {
        private readonly FileSystemWatcher _fileWatcher;
        private readonly FormPaletizadoView _paletizadoView;
        private readonly Dictionary<string, PaletizerDefinition> _tiposPaletizado;
        private readonly Dictionary<string, TipoPasaporte> _tiposPasaporte;
        private PaletizerDefinition _selectedPaletizado;
        private TipoPasaporte _selectedType;

        public FrmCreatePaletizerCatalog()
        {
            InitializeComponent();
            _paletizadoView = new FormPaletizadoView();
            _tiposPasaporte = new Dictionary<string, TipoPasaporte>();
            _tiposPaletizado = new Dictionary<string, PaletizerDefinition>();
            _fileWatcher = new FileSystemWatcher
                               {
                                   Path = ConfigPaletizadoFiles.CatalogsPaletizado,
                                   Filter = "*.*",
                                   NotifyFilter = NotifyFilters.FileName
                               };
            _fileWatcher.Created += FileWatcherHandler;
            _fileWatcher.Deleted += FileWatcherHandler;


            _fileWatcher.EnableRaisingEvents = true;
            cbPaletizados.SelectedIndexChanged += cbPaletizados_SelectedIndexChanged;
        }

        public event EventHandler SetCatatogsChanged;

        private void FileWatcherHandler(object obj, FileSystemEventArgs e)
        {
            OnSetCatalogsChanged();
        }

        private void LoadTiposCatalogInCbCatalogs()
        {
            cbTiposPasaporte.Items.Clear();

            foreach (string str in _tiposPasaporte.Keys)
            {
                cbTiposPasaporte.Items.Add(str);
            }
        }

        private void LoadTiposPaletizadoInComboBox()
        {
            cbPaletizados.Items.Clear();
            foreach (string str in _tiposPaletizado.Keys)
            {
                cbPaletizados.Items.Add(str);
            }
        }

        private void LoadLinesInComboBox()
        {
            cbLines.Items.Clear();
            Lines lines = ControlLoop<IdpsaSystemPaletizado>.Instance.Sys.Lines;
            cbLines.Items.Add(lines.Line1);
            cbLines.Items.Add(lines.Line2);
            cbLines.SelectedIndex = 0;
        }

        private void LoadTiposPasaporte()
        {
            var directory = new DirectoryInfo(ConfigPaletizadoFiles.PassaportPaletizado);
            FileInfo[] files = directory.GetFiles();
            var bFormatter = new BinaryFormatter();
            _tiposPasaporte.Clear();
            try
            {
                foreach (FileInfo file in files)
                {
                    using (var readFile = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                    {
                        var bFormat = new BinaryFormatter();
                        var tipo = (TipoPasaporte) bFormat.Deserialize(readFile);
                        _tiposPasaporte.Add(tipo.ToString(), tipo);
                    }
                }
            }
            catch
            {
                ;
            }
        }

        private void LoadTiposPaletizado()
        {
            var directory = new DirectoryInfo(ConfigPaletizadoFiles.PaletizadosPaletizado);
            FileInfo[] files = directory.GetFiles();
            var BFormatter = new BinaryFormatter();
            _tiposPaletizado.Clear();

            foreach (FileInfo file in files)
            {
                using (var readFile = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                {
                    var BFormat = new BinaryFormatter();
                    var paletizado = (PaletizerDefinition) BFormat.Deserialize(readFile);
                    _tiposPaletizado.Add(paletizado.Name, paletizado);
                }
            }
        }

        private void LoadDataOfTypeCatalog(TipoPasaporte tipoPasaporte)
        {
            tbPais.Text = tipoPasaporte.Country.Name;
            tbCategoria.Text = tipoPasaporte.Type.ToString();
            tbNDigitos.Text = tipoPasaporte.NDigits.ToString();
            tbNLetras.Text = tipoPasaporte.NChars.ToString();
            tbTieneRfid.Text = tipoPasaporte.RfIdType.ConvertToString();
            tbPeso.Text = tipoPasaporte.Weight.ToString(CultureInfo.InvariantCulture.NumberFormat);
            tbGrosor.Text = tipoPasaporte.Thickness.ToString();
        }

        private void LoadDataPaletizado()
        {
        }

        private void LoadCbFechaLam()
        {
            var mesSeleccion = new String[12] {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"};

            Mes.Items.AddRange(mesSeleccion);
            int month = DateTime.Now.Month;
            Mes.SelectedIndex = month - 1;
            int year = DateTime.Now.Year;
            var anhoActual = new String[10]
                                 {
                                     year.ToString(), (year + 1).ToString(), (year + 2).ToString(),
                                     (year + 3).ToString(),
                                     (year + 4).ToString(), (year + 5).ToString(), (year + 6).ToString(),
                                     (year + 7).ToString(), (year + 8).ToString(), (year + 9).ToString()
                                 };
            Anho.Items.AddRange(anhoActual);
            Anho.SelectedIndex = 1;
            var mes = (string) Mes.SelectedItem;
            var anho = (string) Anho.SelectedItem;
            fechaLaminacion.Text = mes + "/" + anho;
        }

        private void cbTiposPasaporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_tiposPasaporte.ContainsKey(cbTiposPasaporte.Text))
            {
                _selectedType = _tiposPasaporte[cbTiposPasaporte.Text];
                LoadDataOfTypeCatalog(_selectedType);
            }
            else
            {
                _selectedType = null;
            }
            if (_selectedType == null || _selectedType.TieneFechaDeLaminacion)
            {
                cbFechaLam.Checked = true;
                Mes.Show();
                Anho.Show();
                fechaLaminacion.Show();
                labelMes.Show();
                labelAnho.Show();
            }
            else
            {
                Mes.Hide();
                Anho.Hide();
                fechaLaminacion.Hide();
                labelMes.Hide();
                labelAnho.Hide();
                cbFechaLam.Checked = false;
            }
        }

        private void cbPaletizados_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_tiposPaletizado.ContainsKey(cbPaletizados.Text))
            {
                _selectedPaletizado = _tiposPaletizado[cbPaletizados.Text];
                LoadDataPaletizado();
            }
            else
            {
                _selectedPaletizado = null;
            }
        }


        private DatosCatalogoPaletizado Validar()
        {
            string idFirstPasaporte = tbPasaporteInicial.Text;
            string idLastPasaporte = tbPasaporteFinal.Text;

            string error;

            if (_selectedType == null)
            {
                MessageBox.Show(@"Debe seleccionar un tipo de pasaporte", @"Especifición errónea", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                cbTiposPasaporte.Focus();
                return null;
            }

            if (_selectedPaletizado == null)
            {
                MessageBox.Show(@"Debe seleccionar un tipo de paletizado", @"Especifición errónea", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                cbPaletizados.Focus();
                return null;
            }

            if (!DatosCatalogo.IsIdPasaporteIniCorrect(out error, idFirstPasaporte, _selectedType))
            {
                MessageBox.Show(error, @"Especifición errónea", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbPasaporteInicial.Focus();
                return null;
            }

            if (!DatosCatalogo.IsIdPasaporteEndCorrect(out error, idLastPasaporte, idFirstPasaporte, _selectedType))
            {
                MessageBox.Show(error, @"Especifición errónea", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbPasaporteFinal.Focus();
                return null;
            }

            if (!DatosCatalogo.IsNPasaportesCorrect(out error, idFirstPasaporte, idLastPasaporte, _selectedType))
            {
                MessageBox.Show(error, @"Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbPasaporteFinal.Focus();
                return null;
            }

            if (_selectedType.Country.Name.Equals(RCMCommonTypes.Country.España.Name))
                if (!DatosCatalogo.ValidarTipoyN(idFirstPasaporte, _selectedType))
                {
                    DialogResult dialogRes = MessageBox.Show(@"El tipo de pasaporte seleccionado no se corresponde con las letras de la numeración. ¿Desea cambiar el tipo?", "Descuadre numeracion y tipo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogRes == DialogResult.Yes)
                        return null;
                }//MCR.2016. Mod Impresion.

            var line = cbLines.SelectedItem as Line;
            if (line == null)
            {
                MessageBox.Show(@"Debe seleccionar la línea de funcionamiento", @"Especifición errónea",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbLines.Focus();
                return null;
            }

            var mes = (string) Mes.SelectedItem;
            var anho = (string) Anho.SelectedItem;
            fechaLaminacion.Text = mes + @"/" + anho;
            string fechaLam = fechaLaminacion.Text;
            if (fechaLam == null && _selectedType.TieneFechaDeLaminacion)
            {
                MessageBox.Show(@"Debe seleccionar una fecha de laminación", @"Especifición errónea",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                fechaLaminacion.Focus();
                return null;
            }
            if (fechaLam == null && !_selectedType.TieneFechaDeLaminacion)
                fechaLam = "no";


            return new DatosCatalogoPaletizado(_selectedType, idFirstPasaporte, idLastPasaporte, _selectedPaletizado,
                                               line.Id, fechaLam);
        }


        private void BtGuardarClick(object sender, EventArgs e)
        {
            var line = cbLines.SelectedItem as Line;
            if (line == null) return;
            if (line.Id == IDLine.Alemana)
            {
                var catalogManager = new RobotEnlaceCatalogManager();
                catalogManager.SaveRobotEnlaceCatalogue(tbPasaporteInicial.Text,
                                                        tbPasaporteFinal.Text,
                                                        _selectedType,
                                                        (string) Mes.SelectedItem,
                                                        (string) Anho.SelectedItem);
            }
            DatosCatalogoPaletizado paletizadoCatalog;
            if ((paletizadoCatalog = Validar()) == null) return;
            SavePaletizadoCatalogue(paletizadoCatalog);
            OnSetCatalogsChanged();
            ClearFormulario();
        }

        private static void SavePaletizadoCatalogue(DatosCatalogo catalog)
        {
            string pathPaletizado = Path.Combine(ConfigPaletizadoFiles.CatalogsPaletizado, catalog.Name);
            if (File.Exists(pathPaletizado))
            {
                if (MessageBox.Show(@"El catálogo ya existe. ¿Desea sustituirlo?", @"Creación catálogo Paletizado",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    using (var writeFile = new FileStream(pathPaletizado, FileMode.Create, FileAccess.Write))
                    {
                        var bFormatter = new BinaryFormatter();
                        bFormatter.Serialize(writeFile, catalog);
                    }
                    MessageBox.Show(@"Catálogo creado en Paletizado.",
                            @"Creacion catálogo Paletizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                using (var writeFile = new FileStream(pathPaletizado, FileMode.Create, FileAccess.Write))
                {
                    var bFormatter = new BinaryFormatter();
                    bFormatter.Serialize(writeFile, catalog);
                }
                MessageBox.Show(@"Catálogo creado en Paletizado.",
                        @"Creacion catálogo Paletizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnSetCatalogsChanged()
        {
            if (SetCatatogsChanged != null)
                SetCatatogsChanged(new object(), new EventArgs());
        }

        private void ClearFormulario()
        {
            cbTiposPasaporte.SelectedIndex = -1;
            tbNLetras.Text = "";
            tbNDigitos.Text = "";
            tbPasaporteFinal.Text = "";
            tbPasaporteInicial.Text = "";
            tbPais.Text = "";
            tbCategoria.Text = "";
            tbTieneRfid.Text = "";
            tbGrosor.Text = "";
            tbPeso.Text = "";
            fechaLaminacion.Text = "";
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            ClearFormulario();
        }

        private void frmCreateCatalog_Load(object sender, EventArgs e)
        {
            LoadTiposPasaporte();
            LoadTiposCatalogInCbCatalogs();
            LoadTiposPaletizado();
            LoadLinesInComboBox();
            LoadTiposPaletizadoInComboBox();
            LoadCbFechaLam(); //MCR. 2011/03/03. Introduccion Fecha Laminacion.
        }

        private void btCerrar_Click(object sender, EventArgs e)
        {
            ClearFormulario();
            Close();
        }

        private void Closing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public void SetTypePasaportChangedHandler(object obj, EventArgs e)
        {
            ClearFormulario();
            LoadTiposPasaporte();
            LoadTiposCatalogInCbCatalogs();
            LoadLinesInComboBox();
        }

        public void SetPaletizerDefinitionsChangedHandler(object obj, EventArgs e)
        {
            LoadTiposPaletizado();
            LoadTiposPaletizadoInComboBox();
        }

        private void btVisualizar_Click(object sender, EventArgs e)
        {
            if (_selectedPaletizado == null)
                return;

            _paletizadoView.SetData(_selectedPaletizado);
        }

        private void Mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mes = (string) Mes.SelectedItem;
            var anho = (string) Anho.SelectedItem;
            fechaLaminacion.Text = mes + "/" + anho;
        }

        private void Anho_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mes = (string) Mes.SelectedItem;
            var anho = (string) Anho.SelectedItem;
            fechaLaminacion.Text = mes + "/" + anho;
        }

        private void cbFechaLam_CheckedChanged(object sender, EventArgs e)
        {
            if (_selectedType != null)
            {
                _selectedType.TieneFechaDeLaminacion = cbFechaLam.Checked;
                if (_selectedType.TieneFechaDeLaminacion)
                {
                    Mes.Show();
                    Anho.Show();
                    fechaLaminacion.Show();
                    labelMes.Show();
                    labelAnho.Show();
                }
                else
                {
                    Mes.Hide();
                    Anho.Hide();
                    fechaLaminacion.Hide();
                    labelMes.Hide();
                    labelAnho.Hide();
                }
            }
        }
    }
}