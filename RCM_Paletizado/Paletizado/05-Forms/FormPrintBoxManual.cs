using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Idpsa.Paletizado
{
    public partial class FormPrintBoxManual : Form
    {
        private readonly Func<string, CajaPasaportes> _boxGetter;

        private FormPrintBoxManual()
        {
            InitializeComponent();
        }

        public FormPrintBoxManual(IEnumerable<Zebra_CajasPrinter> printers, Func<string, CajaPasaportes> boxGetter)
        {
            InitializeComponent();
            _boxGetter = boxGetter;
            LoadCbPrinter(printers);
        }

        private void LoadCbPrinter(IEnumerable<Zebra_CajasPrinter> printers)
        {
            cbPrinter.Items.AddRange(printers.ToArray());
            cbPrinter.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string id = textBox1.Text;
            if (String.IsNullOrEmpty(id))
            {
                MessageBox.Show("Introduzca el número de caja", "Especifición errónea", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                textBox1.Focus();
                return;
            }
            CajaPasaportes box = _boxGetter(id);
            if (box == null)
            {
                MessageBox.Show("El número de caja:  \n  " + id + " \nno se encuentra en catálogo ",
                                "Especifición errónea", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Focus();
                return;
            }
            var printer = (Zebra_CajasPrinter) cbPrinter.SelectedItem;

            printer.Print(box,false);
            //printer.Print(box, true);//prueba etiqueta 99 pasaportes
        }

        private void Closing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string text = textBox1.Text;
        }
    }
}