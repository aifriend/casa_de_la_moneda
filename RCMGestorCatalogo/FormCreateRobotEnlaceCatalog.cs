using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using CatalogFactory.Catalogo;
using Idpsa;

namespace CatalogFactory
{
    public partial class frmCreateRobotEnlaceCatalog : Form
    {
        private readonly Dictionary<string,TipoPasaporte> tiposPasaporte;
        private TipoPasaporte selectedType;
        private readonly FileSystemWatcher fileWatcher;
        public event EventHandler SetCatatogsChanged;

        public frmCreateRobotEnlaceCatalog()
        {
            InitializeComponent();

            tiposPasaporte = new Dictionary<string, TipoPasaporte>();
            fileWatcher = new FileSystemWatcher
                              {
                                  Path = ConfigFiles.CatalogsRobotEnlace,
                                  Filter = "*.*",
                                  NotifyFilter = NotifyFilters.FileName
                              };
            fileWatcher.Created += FileWatcherHandler;
            fileWatcher.Deleted += FileWatcherHandler;
            
            fileWatcher.EnableRaisingEvents = true;
        }

        private void frmCreateCatalog_Load(object sender, EventArgs e)
        {
            LoadTiposPasaporte();
            LoadTiposCatalogInCbCatalogs();
            LoadCbFechaLam();
        }

        private void FileWatcherHandler(object obj, FileSystemEventArgs e)
        {           
            OnSetCatalogsChanged();
        }

        private void OnSetCatalogsChanged()
        {
            if (SetCatatogsChanged != null)
                SetCatatogsChanged(new object(), new EventArgs());
        }

        private void LoadTiposCatalogInCbCatalogs()
        {
            cbTiposPasaporte.Items.Clear();
            
            foreach (string str in tiposPasaporte.Keys)
            {
                cbTiposPasaporte.Items.Add(str);                 
            }
        }

        private void LoadTiposPasaporte()
        {
            var directory = new DirectoryInfo(ConfigFiles.Passaports);
            FileInfo[] files=directory.GetFiles();           
            var bFormatter = new BinaryFormatter();
            tiposPasaporte.Clear();

            foreach (FileInfo file in files){
                using (var readFile = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                {                
                    var bFormat = new BinaryFormatter();
                    var tipo=(TipoPasaporte)bFormat.Deserialize(readFile);
                    tiposPasaporte.Add(tipo.ToString(), tipo);   
                }
            }
        }

        private void LoadDataOfTypeCatalog(TipoPasaporte tipoPasaporte)
        {
            tbPais.Text = tipoPasaporte.Country.Name;
            tbCategoria.Text = tipoPasaporte.Type.ToString();
            tbNDigitos.Text = tipoPasaporte.NDigits.ToString();
            tbNLetras.Text = tipoPasaporte.NChars.ToString();
            tbTieneRfid.Text = tipoPasaporte.RfIdType.ToString();
            tbPeso.Text = tipoPasaporte.Weight.ToString();
            tbGrosor.Text = tipoPasaporte.Thickness.ToString();
        }

        private void LoadCbFechaLam()
        {
            var mesSeleccion = new String[12] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };

            Mes.Items.AddRange(mesSeleccion);
            int month = DateTime.Now.Month;
            Mes.SelectedIndex = month - 1;
            int year = DateTime.Now.Year;
            var anhoActual = new String[10] { year.ToString(), (year+1).ToString(), (year + 2).ToString(), (year + 3).ToString(), (year + 4).ToString(), (year + 5).ToString(), (year + 6).ToString(), (year + 7).ToString(), (year + 8).ToString(), (year + 9).ToString() };
            Anho.Items.AddRange(anhoActual);
            Anho.SelectedIndex = 1;
            var mes = (string)Mes.SelectedItem;
            var anho = (string)Anho.SelectedItem;
            fechaLaminacion.Text = mes + "/" + anho;
        }

        private void cbTiposPasaporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tiposPasaporte.ContainsKey(cbTiposPasaporte.Text))
            {                
                selectedType = tiposPasaporte[cbTiposPasaporte.Text];
                LoadDataOfTypeCatalog(selectedType);
            }
            else
            {
                selectedType = null;
            }
            if (selectedType==null || selectedType.TieneFechaDeLaminacion)
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

        private DatosCatalogoRobotEnlace Validar()
        {
            string idFirstPasaporte=tbPasaporteInicial.Text;
            string idLastPasaporte=tbPasaporteFinal.Text;

            string error;

            if (selectedType==null){
                MessageBox.Show(@"Debe seleccionar un tipo de pasaporte", @"Especifición errónea", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbTiposPasaporte.Focus();
                return null;
            }

            if (!DatosCatalogo.IsIdPasaporteIniCorrect(out error,idFirstPasaporte,selectedType)){
                MessageBox.Show(error, @"Especifición errónea", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbPasaporteInicial.Focus();
                return null;
            }

            if (!DatosCatalogo.IsIdPasaporteEndCorrect(out error,idLastPasaporte,idFirstPasaporte,selectedType)){
                MessageBox.Show(error,@"Especifición errónea", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbPasaporteFinal.Focus();
                return null;
            }

            if (!DatosCatalogo.IsNPasaportesCorrect(out error,idFirstPasaporte,idLastPasaporte,selectedType)){
                MessageBox.Show(error, @"Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbPasaporteFinal.Focus();
                return null;
            }

            var mes = (string)Mes.SelectedItem;
            var anho = (string)Anho.SelectedItem;
            fechaLaminacion.Text = mes + "/" + anho;
            var fechaLam = fechaLaminacion.Text;
            if (fechaLam == null &&  selectedType.TieneFechaDeLaminacion)
            {
                MessageBox.Show(@"Debe seleccionar una fecha de laminación", @"Especifición errónea", MessageBoxButtons.OK, MessageBoxIcon.Information);
                fechaLaminacion.Focus();
                return null;
            }
            if (fechaLam == null && !selectedType.TieneFechaDeLaminacion)
                fechaLam = "no";


            return new DatosCatalogoRobotEnlace(selectedType, idFirstPasaporte, idLastPasaporte, fechaLam);             
        }

        private void btGuardar_Click(object sender, EventArgs e)
        {
            DatosCatalogoRobotEnlace catalog;

            if ((catalog = Validar()) == null) return;

            var path = Path.Combine(ConfigFiles.CatalogsRobotEnlace, catalog.Name);
                               
            using (var writeFile = new FileStream(path, FileMode.Create, FileAccess.Write))
            {                        
                var bFormatter = new BinaryFormatter();
                bFormatter.Serialize(writeFile, catalog);
            }

            ClearFormulario();
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

        public void SetTypePasaportChangedHandler(object obj,EventArgs e)
        {
            ClearFormulario();
            LoadTiposPasaporte();
            LoadTiposCatalogInCbCatalogs();
        }

        private void Mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mes = (string)Mes.SelectedItem;
            var anho = (string)Anho.SelectedItem;
            fechaLaminacion.Text = mes + @"/" + anho;
        }

        private void Anho_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mes = (string)Mes.SelectedItem;
            var anho = (string)Anho.SelectedItem;
            fechaLaminacion.Text = mes + @"/" + anho;
        }
    }
}