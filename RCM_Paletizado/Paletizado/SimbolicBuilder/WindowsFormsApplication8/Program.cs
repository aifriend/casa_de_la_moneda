using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Idpsa.SimbolicBuilder;

namespace Idpsa.SimbolicBuilder
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicaci�n.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmSimbolicBuilder());
        }
    }
}