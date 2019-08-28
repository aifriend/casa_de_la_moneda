using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ConfiguracionPaletizado
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void btaceptar_Click(object sender, EventArgs e)
        {
            if (tbContraseña.Text == "FNMT_PASAPORTES")
                DialogResult = DialogResult.OK;
            else
                MessageBox.Show("La contraseña es incorrecta. No se modificará la configuración."); 

        }

        private void btcancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
