using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using RCMCommonTypes;
using RECatalogManager;

namespace Idpsa
{
    public partial class FrmCreatePasaporte : Form
    {
        private readonly FileSystemWatcher _fileWatcherPaletizado;
        private List<Country> _paises;

        public FrmCreatePasaporte()
        {
            InitializeComponent();
            _paises = new List<Country>();

            _fileWatcherPaletizado = new FileSystemWatcher
                                         {
                                             Path = ConfigPaletizadoFiles.PassaportPaletizado,
                                             Filter = "*.*",
                                             NotifyFilter = NotifyFilters.FileName
                                         };
            _fileWatcherPaletizado.Created += FileWatcherPaletizadoHandler;
            _fileWatcherPaletizado.Deleted += FileWatcherPaletizadoHandler;
            _fileWatcherPaletizado.Changed += FileWatcherPaletizadoHandler;

            _fileWatcherPaletizado.EnableRaisingEvents = true;

            ClearForm();
        }

        public event EventHandler SetTypePasaportChanged;

        private void FileWatcherPaletizadoHandler(object obj, FileSystemEventArgs e)
        {
            OnSetTypePasaportChanged();
        }

        private void LoadCountriesInCombo()
        {
            cbPaises.Items.Clear();
            foreach (Country country in _paises)
            {
                cbPaises.Items.Add(country);
            }

            if (cbPaises.Items.Count > 0)
                cbPaises.SelectedIndex = 0;
        }

        private void LoadCategoriesInCombo()
        {
            cbCategorias.Items.Clear();
            foreach (TipoPasaporte.Types v in Enum.GetValues(typeof (TipoPasaporte.Types)))
            {
                if (v != TipoPasaporte.Types.NotDefined)
                {
                    cbCategorias.Items.Add(v);
                }
            }

            if (cbCategorias.Items.Count > 0)
                cbCategorias.SelectedIndex = 0;
        }

        public void LoadRfIdInCombo()
        {
            cbTieneRfid.Items.Clear();
            foreach (TipoPasaporte.TypeRfid v in Enum.GetValues(typeof (TipoPasaporte.TypeRfid)))
            {
                if (v != TipoPasaporte.TypeRfid.NoDefined)
                {
                    cbTieneRfid.Items.Add(v);
                }
            }

            if (cbTieneRfid.Items.Count > 0)
                cbTieneRfid.SelectedIndex = 0;
        }

        public void LoadGrosorInCombo()
        {
            cbGrosor.Items.Clear();
            foreach (TipoPasaporte.Thicknesses v in Enum.GetValues(typeof (TipoPasaporte.Thicknesses)))
            {
                cbGrosor.Items.Add(v);
            }

            if (cbGrosor.Items.Count > 0)
                cbGrosor.SelectedIndex = 0;
        }


        private void LoadCountries()
        {
            var directory = new DirectoryInfo(ConfigPaletizadoFiles.Countries);
            FileInfo[] files = directory.GetFiles();
            var bFormatter = new BinaryFormatter();
            _paises.Clear();

            _paises = new List<Country> {Country.España, Country.RepDominicana, Country.Panama};

            foreach (FileInfo file in files)
            {
                using (var readFile = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                {
                    var bFormat = new BinaryFormatter();
                    var country = (Country) bFormat.Deserialize(readFile);
                    _paises.Add(country);
                }
            }
        }


        private TipoPasaporte Validar()
        {
            TipoPasaporte tipo = null;
            if (cbPaises.SelectedIndex == -1)
            {
                MessageBox.Show("No hay países disponibles, debe utilizar el editor de países", "Error de definición",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            int numberLetras;
            if (tbNLetras.Text == "")
            {
                MessageBox.Show("Debe introducir el número de letras", "Error de formato", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                tbNLetras.Focus();
                return null;
            }

            if (!int.TryParse(tbNLetras.Text, out numberLetras))
            {
                MessageBox.Show("El valor introducido para el número de letras no es numérico", "Error de formato",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbNLetras.Focus();
                return null;
            }
            else
            {
                if (numberLetras < 0)
                {
                    MessageBox.Show("El valor introducido para el número de letras no puede ser negativo",
                                    "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbNLetras.Focus();
                    return null;
                }
            }

            int numberDigitos;
            if (tbNDigitos.Text == "")
            {
                MessageBox.Show("Debe introducir el número de dígitos", "Error de formato", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                tbNDigitos.Focus();
                return null;
            }

            if (!int.TryParse(tbNDigitos.Text, out numberDigitos))
            {
                MessageBox.Show("El valor introducido para el número de dígitos no es numérico", "Error de formato",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbNDigitos.Focus();
                return null;
            }
            else
            {
                if (numberDigitos < 0)
                {
                    MessageBox.Show("El valor introducido para el número de dígitos no puede ser negativo",
                                    "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbNDigitos.Focus();
                    return null;
                }
            }

            if (cbTieneRfid.SelectedIndex < 0)
            {
                MessageBox.Show("Debe indicar si el pasaporte posee o no indentificación RfID", "Error de formato",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbTieneRfid.Focus();
                return null;
            }


            if (tbPeso.Text == "")
            {
                MessageBox.Show("Debe introducir peso del pasaporte", "Error de formato", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                tbPeso.Focus();
                return null;
            }

            double peso = double.Parse(tbPeso.Text);
            if (peso < 0)
            {
                MessageBox.Show("El valor introducido para peso del pasaporte no puede ser negativo",
                                "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbPeso.Focus();
                return null;
            }

            if (tbResponsable.Text == "")
            {
                MessageBox.Show("Debe indicar responsable", "Error de formato", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                tbResponsable.Focus();
                return null;
            }

            if (tbDestinatario.Text == "")
            {
                MessageBox.Show("Debe indicar destinatario", "Error de formato", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                tbDestinatario.Focus();
                return null;
            }

            tipo = new TipoPasaporte((Country) cbPaises.SelectedItem, (TipoPasaporte.Types) cbCategorias.SelectedItem,
                                     numberLetras, numberDigitos, (TipoPasaporte.TypeRfid) cbTieneRfid.SelectedItem,
                                     (TipoPasaporte.Thicknesses) cbGrosor.SelectedItem,
                                     peso, tbDestinatario.Text, tbResponsable.Text, cbConFechaDeLaminacion.Checked,
                                     tbNombrePasaporte.Text);
            return tipo;
        }

        private void btGuardar_Click(object sender, EventArgs e)
        {
            TipoPasaporte tipo = Validar();
            if (tipo == null) return;

            //Save passport type for Paletizado
            string filePathPaletizado = Path.Combine(ConfigPaletizadoFiles.PassaportPaletizado,
                                                     tipo.Country.Name + "_" + tipo.Type + "_Chip_" +
                                                     tipo.RfIdType);
            if (File.Exists(filePathPaletizado))
            {
                if (MessageBox.Show(@"El tipo de pasaporte ya existe. ¿Desea sustituirlo?", @"Carga tipo pasaporte",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    using (var writeFile = new FileStream(filePathPaletizado, FileMode.Create, FileAccess.Write))
                    {
                        var bFormatter = new BinaryFormatter();
                        bFormatter.Serialize(writeFile, tipo);
                    }
                }
            }
            else
            {
                using (var writeFile = new FileStream(filePathPaletizado, FileMode.Create, FileAccess.Write))
                {
                    var bFormatter = new BinaryFormatter();
                    bFormatter.Serialize(writeFile, tipo);
                }
            }

            //Save passport type for Robot Enlace
            string filePathRobotEnlace = Path.Combine(ConfigRobotEnlaceFiles.Passport,
                                                      tipo.Country.Name + "_" + tipo.Type + "_Chip_" +
                                                      tipo.RfIdType);
            if (File.Exists(filePathRobotEnlace))
            {
                if (MessageBox.Show(@"El tipo de pasaporte ya existe. ¿Desea sustituirlo?", @"Carga tipo pasaporte",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    using (var writeFile = new FileStream(filePathRobotEnlace, FileMode.Create, FileAccess.Write))
                    {
                        var bFormatter = new BinaryFormatter();
                        bFormatter.Serialize(writeFile, tipo);
                    }
                }
            }
            else
            {
                using (var writeFile = new FileStream(filePathRobotEnlace, FileMode.Create, FileAccess.Write))
                {
                    var bFormatter = new BinaryFormatter();
                    bFormatter.Serialize(writeFile, tipo);
                }
            }

            ClearForm();
        }

        private void ClearForm()
        {
            tbNDigitos.Text = "";
            tbNLetras.Text = "";
            tbPeso.Text = "";
            tbDestinatario.Text = "";
            tbResponsable.Text = "";
            cbConFechaDeLaminacion.Checked = false;
            tbNombrePasaporte.Text = "";
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void Closing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void btCerrar_Click(object sender, EventArgs e)
        {
            ClearForm();
            Close();
        }

        private void OnSetTypePasaportChanged()
        {
            if (SetTypePasaportChanged != null)
                SetTypePasaportChanged(new object(), new EventArgs());
        }

        private void frmCreatePasaporte_Load(object sender, EventArgs e)
        {
            LoadCountries();
            LoadCountriesInCombo();
            LoadCategoriesInCombo();

            LoadRfIdInCombo();
            LoadGrosorInCombo();
        }
    }
}