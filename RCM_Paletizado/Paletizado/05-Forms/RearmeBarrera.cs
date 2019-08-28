using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Diagnosis;
using Idpsa.Control.Engine;
using Idpsa.Control.Manuals;
using Idpsa.Control.Sequence;
using Idpsa.Control.Subsystem;
using Idpsa.Control.Tool;
using Idpsa.Control.User;
using Idpsa.Control.View;
using Idpsa.Paletizado;
using RCMCommonTypes;
using Microsoft.Reporting.WinForms;

namespace Idpsa
{
    public partial class RearmeBarrera : Form
    {

        private ParadaDB datosProduccion;
        private datosParada datosVisualReport;
        private AuthorizedReset _reset;

        public event ConfirmadoEvent Confirmado;
        public delegate void ConfirmadoEvent(bool b);
        

        //public RearmeBarrera()
        //{
        //    InitializeComponent();
        //}

        public RearmeBarrera(AuthorizedReset res)
        {
            InitializeComponent();
            _reset = res;
            TieToBD();
            Show();
            Hide();
        }

        private void RearmeBarrera_Load(object sender, EventArgs e)
        {
            if (datosVisualReport != null)
            {
                this.reportViewer1.RefreshReport();
                this.reportViewer3.RefreshReport();
            }
            if (datosProduccion!=null)
            this.reportViewer2.RefreshReport();
        }

        private void tryEmpleado_Click(object sender, EventArgs e)
        {
            string emp = _reset._code;
            NewCodeHandler(emp);

        }

        private void NewCodeHandler(string emp)
        {
            bool alguienDentro = true;
            try
            {
                if (datosProduccion == null)
                {
                    var aux = new ParadaDB();
                    aux = new ParadaDB(aux.LoadDatos());
                    TimeSpan difference = DateTime.Now - aux.Rearme._fechaEntrada;
                    if (difference.Seconds>1||aux.estado != EstadoRearme.Rearmado)
                    {
                        datosProduccion = new ParadaDB(new RegistroES(emp));
                        BarreraCaidaEvent(true);
                    }
                }
                else
                {
                    if (datosProduccion.Operarios.Count == 0)
                        BarreraCaidaEvent(true);
                    if (datosProduccion.estado != EstadoRearme.Rearmado)
                    {
                        alguienDentro = datosProduccion.newSignal(new RegistroES(emp));
                        if (!alguienDentro)
                        {
                            reportViewer2.Refresh();
                            reportViewer2.RefreshReport();
                            rearmarCompletado();
                            return;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            try
            {

                operariosBindingSource.ResetBindings(true);
                if (datosProduccion != null)
                {
                    operariosBindingSource.DataSource = datosProduccion.getOperarios();

                    reportViewer2.Refresh();
                    reportViewer2.RefreshReport();
                    datosProduccion.SaveDB();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void NewStateHandler(string txt)
        {
            textBox1.Text = txt;
        }

        private void TieToBD()
        {
            datosProduccion = new ParadaDB();        //MCR
            datosProduccion = new ParadaDB(datosProduccion.LoadDatos());           
            _reset.CodeChange += NewCodeHandler;
            _reset.stateChange += NewStateHandler;
            _reset.rearmeChange += Rearmando;
            _reset.barreraCaidaEvent += BarreraCaidaEvent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            datosVisualReport = new datosParada();
            String fech = "";
            if (cbDia.Text != "" && cbMes.Text != "" && tbAño.Text != "")
                fech = tbAño.Text + cbMes.Text + cbDia.Text;
            datosVisualReport = new datosParada(fech);
            if (datosVisualReport.StopArray.Count == 0)
                MessageBox.Show("No se puede generar un informe con los datos introducidos");
            else
            {
                this.BindingSource2.DataSource = datosVisualReport.getStopArray();
                LoadCb();
            }
            reportViewer1.Refresh();
            reportViewer1.RefreshReport();
          
        }

        private void LoadCb()
        {
            this.comboBox1.Items.Clear();
            foreach (ParadaDB par in datosVisualReport.StopArray)
            {
                this.comboBox1.Items.Add(par.HoraRearme);
            }
            comboBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            employeeTextBox.Enabled = true;
            Rearmando();
            button3.Enabled = true;
            button2.Enabled = false;
        }
        private void Rearmando()
        {
            if (datosProduccion == null)
                datosProduccion = new ParadaDB();
            datosProduccion.estado = _reset.rearmeState;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (employeeTextBox.Text != "")
            {
                if (datosProduccion == null)
                    datosProduccion=new ParadaDB(new RegistroES(employeeTextBox.Text));
                datosProduccion.ForzarRearme(new RegistroES(employeeTextBox.Text));
                reportViewer2.Refresh();
                reportViewer2.RefreshReport();
                rearmarCompletado();
            }

        }
        private void rearmarCompletado()
        {
            try
            {
                if (datosProduccion != null)
                    if(datosProduccion.Rearme.employee!="")
                    {
                        datosProduccion.estado = EstadoRearme.Rearmado;
                        employeeTextBox.Text = datosProduccion.Rearme.employee;
                        horaEntradaTextBox.Text = datosProduccion.Rearme.horaEntrada;
                        horaSalidaTextBox.Text = datosProduccion.Rearme.horaSalida;
                        stateTextBox.Text = datosProduccion.Rearme.state;
                        datosProduccion.SaveDB();                        

                        //_reset.luz.Activate(true);
                        button3.Enabled = false;
                        button2.Enabled = true;
                        this.ConfirmadoHandler();
                        employeeTextBox.Text = "";
                        horaEntradaTextBox.Text = "";
                        horaSalidaTextBox.Text = "";
                        stateTextBox.Text = "";
                        employeeTextBox.Enabled = false;

                        datosProduccion = null;
                        //Hide();
                        //DialogResult = DialogResult.OK;
                    }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void BarreraCaidaEvent(bool b)
        {
            if (b)
                Confirmado(false);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count > 0)
            {
                if (datosVisualReport !=null)
                if (datosVisualReport.StopArray.Count == 0)
                    MessageBox.Show("No se puede generar un informe con los datos introducidos");
                else
                {
                    this.ParadaDBBindingSource.DataSource = datosVisualReport.StopArray[comboBox1.SelectedIndex].getOperarios();
                }

                reportViewer3.Refresh();
                reportViewer3.RefreshReport();
            }
        }

        private void ConfirmadoHandler()
        {
            bool b = false;
            if (datosProduccion != null)
                b = datosProduccion.estado == EstadoRearme.Rearmado;
            Confirmado(b);
        }

        private void Closing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

    }
}
