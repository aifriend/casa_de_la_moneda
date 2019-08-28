using System;
using Idpsa.Control.Engine;

namespace Idpsa.Paletizado
{
    public class Principal
    {
        [STAThread]
        public static void Main()
        {
            Main<IdpsaSystemPaletizado, FormMainPaletizado>.Instance.Run();
        }
    }
}