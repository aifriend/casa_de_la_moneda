using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Idpsa;
using Idpsa.Paletizado;
using Idpsa.Control;
using Idpsa.Control.Engine;


namespace Idpsa
{

    public partial class frmCreateCatalog : Form
    {
        private FormPaletizadoView _paletizadoView;
        private Dictionary<string,TipoPasaporte> tiposPasaporte;
        private Dictionary<string, Paletizado.PaletizerDefinition> tiposPaletizado;
        private TipoPasaporte selectedType;
        private Paletizado.PaletizerDefinition selectedPaletizado;
        public event EventHandler SetCatatogsChanged;
        private FileSystemWatcher fileWatcher;

        public frmCreateCatalog()
        {
            InitializeComponent();
            _paletizadoView = new FormPaletizadoView();
            tiposPasaporte = new Dictionary<string, TipoPasaporte>();
            tiposPaletizado = new Dictionary<string, PaletizerDefinition>();
            fileWatcher = new FileSystemWatcher
            {
                Path = ConfigFiles.Catalogs,
                Filter = "*.*",
                NotifyFilter = NotifyFilters.FileName
            };
            fileWatcher.Created += FileWatcherHandler;
            fileWatcher.Deleted += FileWatcherHandler;
            

            fileWatcher.EnableRaisingEvents = true;
            cbPaletizados.SelectedIndexChanged += cbPaletizados_SelectedIndexChanged;

        }

        private void FileWatcherHandler(object obj, FileSystemEventArgs e)
        {           
            OnSetCatalogsChanged();
        }
        
        private void LoadTiposCatalogInCbCatalogs()
        {
            cbTiposPasaporte.Items.Clear();
            
            foreach (string str in tiposPasaporte.Keys)
            {
                cbTiposPasaporte.Items.Add(str);                 
            }
        }

        private void LoadTiposPaletizadoInComboBox()
        {
            cbPaletizados.Items.Clear();
            foreach (string str in tiposPaletizado.Keys)
            {
                cbPaletizados.Items.Add(str);
            }

        }

        private void LoadLinesInComboBox()
        {
            cbLines.Items.Clear();
            var lines = ControlLoop<IDPSASystemPaletizado>.Instance.Sys.Lines;
            cbLines.Items.Add(lines.Line1);
            cbLines.Items.Add(lines.Line2);
            cbLines.SelectedIndex = 0; 
        }

        private void LoadTiposPasaporte()
        {
            DirectoryInfo directory = new DirectoryInfo(ConfigPaletizadoFiles.Passaports);
            FileInfo[] files=directory.GetFiles();           
            BinaryFormatter BFormatter = new BinaryFormatter();
            tiposPasaporte.Clear();

            foreach (FileInfo file in files){
                using (System.IO.FileStream readFile = new System.IO.FileStream(file.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {                
                    BinaryFormatter BFormat = new BinaryFormatter();
                    TipoPasaporte tipo=(TipoPasaporte)BFormat.Deserialize(readFile);
                    tiposPasaporte.Add(tipo.ToString(), tipo);   
                }
            }

        }


        private void LoadTiposPaletizado()
        {
            DirectoryInfo directory = new DirectoryInfo(ConfigPaletizadoFiles.Paletizados);
            FileInfo[] files = directory.GetFiles();
            BinaryFormatter BFormatter = new BinaryFormatter();
            tiposPaletizado.Clear();

            foreach (FileInfo file in files)
            {
                using (System.IO.FileStream readFile = new System.IO.FileStream(file.FullName, System.IO.FileMode.Open , System.IO.FileAccess.Read))
                {
                    BinaryFormatter BFormat = new BinaryFormatter();
                    var paletizado = (Paletizado.PaletizerDefinition)BFormat.Deserialize(readFile);
                    tiposPaletizado.Add(paletizado.Name, paletizado);
                }
            }

        }

        private void LoadDataOfTypeCatalog(TipoPasaporte tipoPasaporte)
        {
            this.tbPais.Text = tipoPasaporte.Country.Name;
            this.tbCategoria.Text = tipoPasaporte.Type.ToString();
            this.tbNDigitos.Text = tipoPasaporte.NDigits.ToString();
            this.tbNLetras.Text = tipoPasaporte.NChars.ToString();
            this.tbTieneRfid.Text = tipoPasaporte.RfIdType.ConvertToString();           
            this.tbPeso.Text = tipoPasaporte.Weight.ToString();
            this.tbGrosor.Text = tipoPasaporte.Thickness.ToString();
        }

        private void LoadDataPaletizado()
        {


        }

        private void LoadCbFechaLam()
        {
            String[] mesSeleccion = new String[12] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };

            Mes.Items.AddRange(mesSeleccion);
            int month = DateTime.Now.Month;
            Mes.SelectedIndex = month - 1;
            int year = DateTime.Now.Year;
            String[] anhoActual = new String[10] { year.ToString(), (year+1).ToString(), (year + 2).ToString(), (year + 3).ToString(), (year + 4).ToString(), (year + 5).ToString(), (year + 6).ToString(), (year + 7).ToString(), (year + 8).ToString(), (year + 9).ToString() };
            Anho.Items.AddRange(anhoActual);
            Anho.SelectedIndex = 1;
            string mes = (string)Mes.SelectedItem;
            string anho = (string)Anho.SelectedItem;
            this.fechaLaminacion.Text = mes + "/" + anho;
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

        private void cbPaletizados_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tiposPaletizado.ContainsKey(cbPaletizados.Text))
            {
                selectedPaletizado = tiposPaletizado[cbPaletizados.Text];
                LoadDataPaletizado();
            }
            else
            {
                selectedPaletizado = null;
            }
        }


        private DatosCatalogoPaletizado Validar()
        {
            string idFirstPasaporte=this.tbPasaporteInicial.Text;
            string idLastPasaporte=this.tbPasaporteFinal.Text;

            string error;

            if (selectedType==null){
                MessageBox.Show("Debe seleccionar un tipo de pasaporte","Especifición errónea", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.cbTiposPasaporte.Focus();
                return null;
            }

            if (selectedPaletizado == null)
            {
                MessageBox.Show("Debe seleccionar un tipo de paletizado", "Especifición errónea", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.cbPaletizados.Focus();
                return null;
            }

            if (!DatosCatalogo.IsIdPasaporteIniCorrect(out error,idFirstPasaporte,selectedType)){
                MessageBox.Show(error,"Especifición errónea", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbPasaporteInicial.Focus();
                return null;
            }

            if (!DatosCatalogo.IsIdPasaporteEndCorrect(out error,idLastPasaporte,idFirstPasaporte,selectedType)){
                MessageBox.Show(error,"Especifición errónea", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbPasaporteFinal.Focus();
                return null;
            }

            if (!DatosCatalogo.IsNPasaportesCorrect(out error,idFirstPasaporte,idLastPasaporte,selectedType)){
                MessageBox.Show(error,"Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbPasaporteFinal.Focus();
                return null;
            }

            var line = cbLines.SelectedItem as Line;
            if (line == null)
            {
                MessageBox.Show("Debe seleccionar la línea de funcionamiento", "Especifición errónea", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.cbLines.Focus();
                return null;
            }

           string mes = (string)Mes.SelectedItem;
           string anho = (string)Anho.SelectedItem;
           this.fechaLaminacion.Text = mes + "/" + anho;
           var fechaLam = fechaLaminacion.Text;
           if (fechaLam == null &&  selectedType.TieneFechaDeLaminacion)
           {
               MessageBox.Show("Debe seleccionar una fecha de laminación", "Especifición errónea", MessageBoxButtons.OK, MessageBoxIcon.Information);
               this.fechaLaminacion.Focus();
               return null;
           }
           if (fechaLam == null && !selectedType.TieneFechaDeLaminacion)
               fechaLam = "no";


           return new DatosCatalogoPaletizado(selectedType,idFirstPasaporte,idLastPasaporte,selectedPaletizado,line.Id, fechaLam);             
        }



        private void btGuardar_Click(object sender, EventArgs e)
        {
            DatosCatalogoPaletizado catalog;

            if ((catalog = Validar()) != null)
            {
                string path= Path.Combine(ConfigFiles.Catalogs,catalog.Name);
                               
                using (System.IO.FileStream writeFile = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {                        
                    BinaryFormatter BFormatter = new BinaryFormatter();
                    BFormatter.Serialize(writeFile,catalog);
                }

                OnSetCatalogsChanged();
                ClearFormulario();  
            }
                   
        }


        private void OnSetCatalogsChanged()
        {
            if (SetCatatogsChanged != null)
                SetCatatogsChanged(new object(),new EventArgs());
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

        public void SetTypePasaportChangedHandler(object obj,EventArgs e)
        {
            ClearFormulario();
            LoadTiposPasaporte();
            LoadTiposCatalogInCbCatalogs();
            LoadLinesInComboBox();            
        }

        public void SetPaletizerDefinitionsChangedHandler(object obj,EventArgs e){

            LoadTiposPaletizado();
            LoadTiposPaletizadoInComboBox();
        }

        private void btVisualizar_Click(object sender, EventArgs e)
        {
            if(selectedPaletizado==null)
                return;

            _paletizadoView.SetData(selectedPaletizado);
        }

        private void Mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string mes = (string)Mes.SelectedItem;
            string anho = (string)Anho.SelectedItem;
            this.fechaLaminacion.Text = mes + "/" + anho;
        }

        private void Anho_SelectedIndexChanged(object sender, EventArgs e)
        {
            string mes = (string)Mes.SelectedItem;
            string anho = (string)Anho.SelectedItem;
            this.fechaLaminacion.Text = mes + "/" + anho;
        }

       
       

    }
}
