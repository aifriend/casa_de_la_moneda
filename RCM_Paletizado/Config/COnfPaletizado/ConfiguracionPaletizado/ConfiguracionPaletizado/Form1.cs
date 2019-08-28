using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace ConfiguracionPaletizado
{
    public partial class Form1 : Form
    {
        private Form2 auxContraseña;
        public Form1()
        {
            InitializeComponent();
            InitializeCb();      
        }
        private void InitializeCb()
        {
            var actualFile = "";
            try
            {
                var directory = new DirectoryInfo(ConfigFiles.ConfSaved);
                var files = directory.GetFiles();
                while (comboBox1.Items.Count>0)
                    comboBox1.Items.RemoveAt(0);

                foreach (var file in files)
                {
                    comboBox1.Items.Add(file.Name);
                }
            }
            catch (Exception ex)
            {
            }
        }

        ConfSaved ConfCargada;
        //BOTÓN DE GUARDAR
        private void button4_Click(object sender, EventArgs e)
        {
            string error;
            bool f = false;
            try
            {
                f = FormValuesOK(out error);
            }
            catch (Exception)
            {
                throw;
            }
            if (f)
            {
                bool exist;
                string Name =tbName.Text ;
                int i = 0;
                
                var directory = new DirectoryInfo(ConfigFiles.ConfSaved);
                var files = directory.GetFiles();
                do
                {
                    exist = false;
                    foreach (var file in files)
                    {
                        if (Name == file.Name)
                            exist = true;
                    }
                    if (exist)
                    {
                        Name = tbName.Text + i;
                        i++;
                    }
                } while (exist);

                ConfCargada = new ConfSaved(Name, Double.Parse(tbCajaX.Text), Double.Parse(tbCajaY.Text), Double.Parse(tbCajaZ.Text), Double.Parse(tbAlmacenSepX.Text), Double.Parse(tbAlmacenSepY.Text), Double.Parse((tbAlmacenPal1X).Text), Double.Parse((tbAlmacenPal1Y).Text), Double.Parse(tbAlmacenPal2X.Text), Double.Parse(tbAlmacenPal2y.Text), Double.Parse(tbZonaPal1X.Text), Double.Parse(tbZonaPal1Y.Text), Double.Parse(tbZonaPal2X.Text), Double.Parse(tbZonaPal2Y.Text), Double.Parse(tbMesa1X.Text), Double.Parse(tbMesa1y.Text), Double.Parse(tbMesa2X.Text), Double.Parse(tbMesa2y.Text), Double.Parse(tbBandaEtiX.Text), Double.Parse(tbBandaEtiY.Text), Double.Parse(tbBandaRepX.Text), Double.Parse(tbBandaRepY.Text), Double.Parse(tbMedSepX.Text), Double.Parse(tbMedSeparadorY.Text));
                saveDatosConf(ConfCargada, Path.Combine(ConfigFiles.ConfSaved, ConfCargada.Name));
                InitializeCb();
            }
            else
                MessageBox.Show(error);
        }

        //BOTÓN DE CONFIGURACIÓN POR DEFECTO
        private void button3_Click(object sender, EventArgs e)
        {
            ConfCargada = new ConfSaved("default");
            CargarForm();
            label36.Text = "Configuración por defecto";
        }


        //BOTÓN DE CONFIGURACIÓN ACTUAL
        private void button2_Click(object sender, EventArgs e)
        {
            ConfCargada = LoadDatosConf(ConfigFiles.ConfActual);
            CargarForm();
            label36.Text = "Configuración actual";

        }

        //BOTÓN DE VER CONFIGURACIÓN
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ConfCargada = LoadDatosConf(Path.Combine(ConfigFiles.ConfSaved, comboBox1.Text));
                CargarForm();
                label36.Text = "Configuración guardada";
            }
            catch(Exception exception)
            {}

        }

        //combo box DE VER CONFICURACIONES

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CargarForm()
        {
            tbAlmacenPal1X.Text = ConfCargada.PosPaletStore1.X.ToString();
            tbAlmacenPal1Y.Text = ConfCargada.PosPaletStore1.Y.ToString();
            tbAlmacenPal2X.Text = ConfCargada.PosPaletStore2.X.ToString();
            tbAlmacenPal2y.Text = ConfCargada.PosPaletStore2.Y.ToString();
            tbAlmacenSepX.Text = ConfCargada.PosSeparatorStore.X.ToString();
            tbAlmacenSepY.Text = ConfCargada.PosSeparatorStore.Y.ToString();
            tbBandaEtiX.Text = ConfCargada.PosBandaEntrada.X.ToString();
            tbBandaEtiY.Text = ConfCargada.PosBandaEntrada.Y.ToString();
            tbBandaRepX.Text = ConfCargada.PosCogerBandaReproceso.X.ToString();
            tbBandaRepY.Text = ConfCargada.PosCogerBandaReproceso.Y.ToString();
            tbCajaX.Text = ConfCargada.boxMeasures.X.ToString();
            tbCajaY.Text = ConfCargada.boxMeasures.Y.ToString();
            tbCajaZ.Text = ConfCargada.boxMeasures.Z.ToString();
            tbMedSeparadorY.Text = ConfCargada.SeparatorSize.Y.ToString();
            tbMedSepX.Text = ConfCargada.SeparatorSize.X.ToString();
            tbMesa1X.Text = ConfCargada.PosMesa1.X.ToString();
            tbMesa1y.Text = ConfCargada.PosMesa1.Y.ToString();
            tbMesa2X.Text = ConfCargada.PosMesa2.X.ToString();
            tbMesa2y.Text = ConfCargada.PosMesa2.Y.ToString();
            tbName.Text = ConfCargada.Name;
            tbZonaPal1X.Text = ConfCargada.PosZonaPaletizadoFinal1.X.ToString();
            tbZonaPal1Y.Text = ConfCargada.PosZonaPaletizadoFinal1.Y.ToString();
            tbZonaPal2X.Text = ConfCargada.PosZonaPaletizadoFinal2.X.ToString();
            tbZonaPal2Y.Text = ConfCargada.PosZonaPaletizadoFinal2.Y.ToString();
        }

        private void saveDatosConf(ConfSaved dataConf, string filePath)
        {
            //string filePath = Path.Combine(ConfigFiles.ConfSaved,dataConf.Name);

            using (var writeFile = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                var bFormatter = new BinaryFormatter();
                bFormatter.Serialize(writeFile, dataConf);
            }
        }



        public ConfSaved LoadDatosConf(string filePath)
        {
            //string filePath = Path.Combine(ConfigFiles.ConfSaved, name);
            try
            {
                using (var readFile = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var bFormatter = new BinaryFormatter();
                    var catalog = (ConfSaved) bFormatter.Deserialize(readFile);
                    return catalog;
                }
            }
            catch(Exception exception)
            {
                MessageBox.Show("No se ha podido mostrar la configuración pedida");
                return (new ConfSaved("default"));
            }
        }

        private bool FormValuesOK(out string error)
        {
            bool b = false, vacio=false;
            error = "";
            int tol = 100;
            int minitol = 25;
            var confAux =new ConfSaved("default");


            if ((tbAlmacenPal1X.Text) =="" ||
            tbAlmacenPal1Y.Text =="" ||
            tbAlmacenPal2X.Text=="" ||
            tbAlmacenPal2y.Text =="" ||
            tbAlmacenSepX.Text =="" ||
            tbAlmacenSepY.Text=="" ||
            tbBandaEtiX.Text=="" ||
            tbBandaEtiY.Text=="" ||
            tbBandaRepX.Text =="" ||
            tbBandaRepY.Text =="" ||
            tbCajaX.Text =="" ||
            tbCajaY.Text =="" ||
            tbCajaZ.Text =="" ||
            tbMedSeparadorY.Text =="" ||
            tbMedSepX.Text =="" ||
            tbMesa1X.Text =="" ||
            tbMesa1y.Text =="" ||
            tbMesa2X.Text=="" ||
            tbMesa2y.Text =="" ||
            tbName.Text =="" ||
            tbZonaPal1X.Text =="" ||
            tbZonaPal1Y.Text =="" ||
            tbZonaPal2X.Text =="" ||
            tbZonaPal2Y.Text =="" )
                error = "Cargue valores";
            else if (Int32.Parse(tbAlmacenPal1X.Text) > confAux.PosPaletStore1.X + tol ||
                Int32.Parse(tbAlmacenPal1Y.Text) > confAux.PosPaletStore1.Y + tol ||
                Int32.Parse(tbAlmacenPal1X.Text) < confAux.PosPaletStore1.X - tol ||
                Int32.Parse(tbAlmacenPal1Y.Text) < confAux.PosPaletStore1.Y - tol)
            {
                error = "Los parámetros del almacén de pallets de la línea 1 están fuera de rango";
            }
            else if (Int32.Parse(tbAlmacenPal2X.Text) > confAux.PosPaletStore2.X + tol ||
                Int32.Parse(tbAlmacenPal2y.Text) > confAux.PosPaletStore2.Y+tol||
                Int32.Parse(tbAlmacenPal2X.Text) < confAux.PosPaletStore2.X- tol ||
                Int32.Parse(tbAlmacenPal2y.Text) < confAux.PosPaletStore2.Y- tol)
            {
                error = "Los parámetros del almacén de pallets de la línea 2 están fuera de rango";
            }
            else if (Int32.Parse(tbAlmacenSepX.Text) > confAux.PosSeparatorStore.X + tol ||
                Int32.Parse(tbAlmacenSepY.Text) > confAux.PosSeparatorStore.Y+tol||
                Int32.Parse(tbAlmacenSepX.Text) <confAux.PosSeparatorStore.X-tol||
                Int32.Parse(tbAlmacenSepY.Text) < confAux.PosSeparatorStore.Y-tol)
            {
                error = "Los parámetros del almacén de separadores están fuera de rango";
            }
            else if (Int32.Parse(tbBandaEtiX.Text) > confAux.PosBandaEntrada.X + tol ||
                Int32.Parse(tbBandaEtiY.Text) > confAux.PosBandaEntrada.Y + tol ||
                Int32.Parse(tbBandaEtiX.Text) < confAux.PosBandaEntrada.X - tol ||
                Int32.Parse(tbBandaEtiY.Text) < confAux.PosBandaEntrada.Y - tol)
            {
                error = "Los parámetros dela posición de cogida en la banda de etiquetado están fuera de rango";
            }
            else if (Int32.Parse(tbMesa1X.Text) > confAux.PosMesa1.X + tol ||
                Int32.Parse(tbMesa1y.Text) > confAux.PosMesa1.Y + tol ||
                Int32.Parse(tbMesa1X.Text) < confAux.PosMesa1.X - tol ||
                Int32.Parse(tbMesa1y.Text) < confAux.PosMesa1.Y - tol)
            {
                error = "Los parámetros de la mesa de rodillos 1 están fuera de rango";
            }

            else if (Int32.Parse(tbMesa2X.Text) > confAux.PosMesa2.X + tol ||
                Int32.Parse(tbMesa2y.Text) > confAux.PosMesa2.Y + tol ||
                Int32.Parse(tbMesa2X.Text) < confAux.PosMesa2.X - tol ||
                Int32.Parse(tbMesa2y.Text) < confAux.PosMesa2.Y - tol)
            {
                error = "Los parámetros de la mesa de rodillos 2 están fuera de rango";
            }

            else if (Int32.Parse(tbCajaX.Text) > confAux.boxMeasures.X + minitol ||
                Int32.Parse(tbCajaY.Text) > confAux.boxMeasures.Y + minitol ||
                Int32.Parse(tbCajaZ.Text) > confAux.boxMeasures.Z + minitol ||
                Int32.Parse(tbCajaX.Text) < confAux.boxMeasures.X - minitol ||
                Int32.Parse(tbCajaY.Text) < confAux.boxMeasures.Y - minitol ||
                Int32.Parse(tbCajaZ.Text) < confAux.boxMeasures.Z - minitol)
            {
                error = "Las dimensiones de la caja están fuera de rango";
            }


            else if (Int32.Parse(tbMedSepX.Text) > confAux.SeparatorSize.X + tol ||
                Int32.Parse(tbMedSeparadorY.Text) > confAux.SeparatorSize.Y + tol ||
                Int32.Parse(tbMedSepX.Text) < confAux.SeparatorSize.X - tol ||
                Int32.Parse(tbMedSeparadorY.Text) < confAux.SeparatorSize.Y - tol)
            {
                error = "Las dimensiones del separador están fuera de rango";
            }

            else if (Int32.Parse(tbZonaPal1X.Text) > confAux.PosZonaPaletizadoFinal1.X + tol ||
                 Int32.Parse(tbZonaPal1Y.Text) > confAux.PosZonaPaletizadoFinal1.Y + tol ||
                 Int32.Parse(tbZonaPal1X.Text) < confAux.PosZonaPaletizadoFinal1.X - tol ||
                Int32.Parse(tbZonaPal1Y.Text) < confAux.PosZonaPaletizadoFinal1.Y - tol)
            {
                error = "Las posición de la zona de paletizado 1 está fuera de rango";
            }

            else if (Int32.Parse(tbZonaPal2X.Text) > confAux.PosZonaPaletizadoFinal2.X + tol ||
                 Int32.Parse(tbZonaPal2Y.Text) > confAux.PosZonaPaletizadoFinal2.Y + tol ||
                    Int32.Parse(tbZonaPal2X.Text) < confAux.PosZonaPaletizadoFinal2.X - tol ||
                      Int32.Parse(tbZonaPal2Y.Text) < confAux.PosZonaPaletizadoFinal2.Y - tol)
            {
                error = "Las posición de la zona de paletizado 1 está fuera de rango";
            }

            else if (Int32.Parse(tbBandaRepX.Text) > confAux.PosCogerBandaReproceso.X + tol ||
                Int32.Parse(tbBandaRepY.Text) > confAux.PosCogerBandaReproceso.Y + tol ||
                Int32.Parse(tbBandaRepX.Text) < confAux.PosCogerBandaReproceso.X - tol ||
                 Int32.Parse(tbBandaRepY.Text) < confAux.PosCogerBandaReproceso.Y - tol)
            {
                error = "La posición de la banda de reproceso está fuera de rango";
            }

            if (error == "")
                b = true;

            return b;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            auxContraseña = new Form2();
            auxContraseña.btcancelar.Click += new System.EventHandler(this.CancelarClick);
            auxContraseña.btaceptar.Click += new System.EventHandler(this.AceptarClick);   
            auxContraseña.Show();

        }

        private void CancelarClick(object sender, EventArgs e)
        {
                MessageBox.Show("Cambio de configuración cancelado");
            auxContraseña.Dispose();
        }
        private void AceptarClick(object sender, EventArgs e)
        {
            if (auxContraseña.DialogResult==DialogResult.OK)
            {
                string error = "";
                bool PaletizadoOn = false;
                Process[] myProcesses = Process.GetProcesses();
                foreach (Process myProcess in myProcesses)
                {
                    if (myProcess.ProcessName.Contains("aletizado") && !myProcess.ProcessName.Contains("Conf"))
                    {
                        Console.WriteLine(myProcess.ProcessName + "parada");
                        PaletizadoOn = true;
                    }
                }
                if (PaletizadoOn)
                {
                    MessageBox.Show(
                        "No se puede configurar la máquina con el programa de control corriendo.\n Por favor, ciérrelo antes de modificar la configuración de la máquina.");
                }
                else if (FormValuesOK(out error))
                {
                    saveDatosConf(ConfCargada, ConfigFiles.ConfActual);
                    MessageBox.Show("La nueva configuración ha sido cargada");
                    auxContraseña.Dispose();
                }
                else
                    MessageBox.Show(error);
            }
        }
    }
}
