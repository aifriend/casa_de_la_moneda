using System.Windows.Forms;

namespace Idpsa.Control
{
    public static class ConfigFiles
    {
        public static readonly string Access = Application.StartupPath + "\\Accesos\\Accesos.bin";
        public static readonly string Alarms = Application.StartupPath + "\\Alarmas\\Alarmas.bin";
        public static readonly string CurrentSystem = Application.StartupPath + "\\System\\" + "System.bin";
        public static readonly string Emergencies = Application.StartupPath + "\\Emergencias\\Alarmas.bin";
        public static readonly string Simbolic = Application.StartupPath + "\\Simbolico\\" + "simbolico.dat";
        public static readonly string Groups = Application.StartupPath + "\\DatosTransportes";
    }
}