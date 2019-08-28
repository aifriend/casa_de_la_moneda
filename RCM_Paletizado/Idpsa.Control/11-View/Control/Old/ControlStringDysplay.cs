using System;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.Mode.Manually;

namespace Idpsa.Control.View
{
    public partial class ControlStringDysplay : UserControl, IRefrescable
    {
        private readonly Func<string> _stringDataProvider;

        public ControlStringDysplay(Manual manual, Func<string> stringDataProvider)
        {
            InitializeComponent();

            string[] str = manual.Descripcion.Substring("StringDysplay-".Length).Trim().Split(new[] {'|'});
            lbCom.Text = str[0].Trim();
            _stringDataProvider = stringDataProvider;
            WithDataFontSize(52);
            WithTitleFontSize(24);
        }

        public ControlStringDysplay WithDataFontSize(int fontSize)
        {
            lbStringDysplay.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Bold, GraphicsUnit.Point, ((0)));
            return this;
        }

        public ControlStringDysplay WithTitleFontSize(int fontSize)
        {
            lbCom.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Bold, GraphicsUnit.Point, ((0)));
            return this;
        }

        #region Miembros de IRefrescable

        public void Refresh_()
        {
            lbStringDysplay.Text = _stringDataProvider();
        }

        #endregion
    }
}