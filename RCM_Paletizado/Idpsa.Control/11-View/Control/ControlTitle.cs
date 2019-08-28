using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Idpsa.Control.View
{
    public partial class ControlTitle : UserControl
    {
        public ControlTitle(string title)
        {
            InitializeComponent();
            WithFontSize(24);
            lbTitle.Text = title;
        }

        public ControlTitle WithFontSize(int fontSize)
        {
            lbTitle.Font =new System.Drawing.Font("Microsoft Sans Serif", fontSize, FontStyle.Regular);
            return this;
        }
    }
}
