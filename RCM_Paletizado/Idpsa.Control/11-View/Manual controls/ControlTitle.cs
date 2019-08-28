using System.Drawing;
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
            lbTitle.Font =new Font("Microsoft Sans Serif", fontSize, FontStyle.Regular);
            return this;
        }
    }
}
