using System.Windows.Forms;
using Idpsa.Control.Component;

namespace Idpsa.Control.Manuals
{
    public interface IManualControlFactory
    {
        UserControl ManualControlFactoryMethod(Manual manual);
    }
}
