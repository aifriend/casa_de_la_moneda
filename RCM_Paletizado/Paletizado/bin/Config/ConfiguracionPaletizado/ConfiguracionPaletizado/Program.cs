using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace ConfiguracionPaletizado
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool PaletizadoOn = false;
            Process[] myProcesses = Process.GetProcesses();
            foreach (Process myProcess in myProcesses)
            {
                if (myProcess.ProcessName.Contains("aletizado") && !myProcess.ProcessName.Contains("Conf"))
                {
                    Console.WriteLine(myProcess.ProcessName + "parada");
                    PaletizadoOn = true;
                }
            }
            if (PaletizadoOn)
            {
                MessageBox.Show(
                    "No se puede configurar la máquina con el programa de control corriendo.\n Por favor, ciérrelo antes de modificar la configuración de la máquina.");
            }
            else 
            {
                Application.Run(new Form1());
            }
            
        }
    }
}
