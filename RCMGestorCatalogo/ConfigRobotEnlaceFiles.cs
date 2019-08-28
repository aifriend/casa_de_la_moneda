using System.Windows.Forms;

namespace RECatalogManager
{
    public static class ConfigRobotEnlaceFiles
    {
        //public static readonly string Passport = "\\\\192.168.1.15\\Pasaportes";
        public static readonly string Passport = "\\\\Robot-enlace\\Pasaportes";
        //public static readonly string Passport = Application.StartupPath + "\\PasaportesRobotEnlace";
      //  public static readonly string Catalog = "\\\\192.168.1.15\\Catalogos";
        public static readonly string Catalog = "\\\\Robot-enlace\\Catalogos";
        //public static readonly string Catalog = Application.StartupPath + "\\CatalogosRobotEnlace";
        public static readonly string CatalogLocal = Application.StartupPath + "\\CatalogosRobotEnlace";//MDG.2012-12-11.Para crearlo en local por si no se ha podido crear en el robot de enlace
    }
}