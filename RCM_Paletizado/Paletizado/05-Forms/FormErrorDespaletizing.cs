using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Idpsa.Paletizado;
using Idpsa.Paletizado.Definitions;

namespace Idpsa//.Paletizado
{
    public partial class FormErrorDespaletizing : Form
    {
        private IdpsaSystemPaletizado _sys;
        public FormErrorDespaletizing(IdpsaSystemPaletizado sys)
        {
            InitializeComponent();
            _sys = sys;
        }

        private void FormErrorDespaletizing_Load(object sender, EventArgs e)
        {

        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            _sys.Lines.Line1.BoxNotCatchedDespaletizing = false;
            this.Hide();

        }
    }
}
