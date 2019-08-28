using System.Windows.Forms;
using Idpsa.Control.Engine;
using Idpsa.Control.Manuals;

namespace Idpsa.Paletizado
{
    public class ManualControlPaletizadoFactory : IManualControlFactory
    {
        private static IdpsaSystemPaletizado _sys = ControlLoop<IdpsaSystemPaletizado>.Instance.Sys;

        #region IManualControlFactory Members

        public UserControl ManualControlFactoryMethod(Manual manual)
        {
            UserControl mControl = null;
            return mControl;
        }

        #endregion
    }
}