using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using RCMCommonTypes;

namespace RECatalogManager
{
    public class RobotEnlaceCatalogManager
    {
        private DatosCatalogo _catalog;

        public void SaveRobotEnlaceCatalogue(string tbPasaporteInicial, string tbPasaporteFinal,
                                             TipoPasaporte selectedType, string mes, string anho)
        {
            if (!Validar(tbPasaporteInicial, tbPasaporteFinal, selectedType, mes, anho))
            {
                MessageBox.Show(@"El catálogo no se puede crear en el Robot de Enlace porque no es válido.",
                                @"Creacion catalogo Robot de Enlace", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (Directory.Exists(ConfigRobotEnlaceFiles.Catalog))
            {
                var pathRobotEnlace = Path.Combine(ConfigRobotEnlaceFiles.Catalog, _catalog.Name);
                if (File.Exists(pathRobotEnlace))
                {
                    if (MessageBox.Show(@"El catálogo ya existe en el Robot de Enlace. ¿Desea sustituirlo?", @"Creación Catálogo Robot de Enlace",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    {
                        using (var writeFile = new FileStream(pathRobotEnlace, FileMode.Create, FileAccess.Write))
                        {
                            var bFormatter = new BinaryFormatter();
                            bFormatter.Serialize(writeFile, _catalog);
                        }
                        MessageBox.Show(@"Catálogo creado en el Robot de Enlace.",
                                @"Creacion catalogo Robot de Enlace", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    using (var writeFile = new FileStream(pathRobotEnlace, FileMode.Create, FileAccess.Write))
                    {
                        var bFormatter = new BinaryFormatter();
                        bFormatter.Serialize(writeFile, _catalog);
                    }
                    MessageBox.Show(@"Catálogo creado en el Robot de Enlace.",
                            @"Creacion catalogo Robot de Enlace", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(@"El Robot de Enlace está apagado. No se creará su catalogo correspondiente. Los pasaportes no se podrán introducir a través del Robot de Enlace, pero se podrán introducir por la entrada manual a la PRODEC.", 
                                @"Creacion catalogo Robot de Enlace", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            //MDG.2012-12-11.Creacion de Catálogos del Robot de Enlace en local tb para poder copiarlos si no se han creado bien
            if (Directory.Exists(ConfigRobotEnlaceFiles.CatalogLocal))
            {
                var pathRobotEnlaceLocal = Path.Combine(ConfigRobotEnlaceFiles.CatalogLocal, _catalog.Name);
                if (File.Exists(pathRobotEnlaceLocal))
                {
                    //if (MessageBox.Show(@"El catálogo ya existe en el Robot de Enlace. ¿Desea sustituirlo?", @"Creación Catálogo Robot de Enlace",
                    //                    MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    //{
                        using (var writeFile = new FileStream(pathRobotEnlaceLocal, FileMode.Create, FileAccess.Write))
                        {
                            var bFormatter = new BinaryFormatter();
                            bFormatter.Serialize(writeFile, _catalog);
                        }
                    //}
                }
                else
                {
                    using (var writeFile = new FileStream(pathRobotEnlaceLocal, FileMode.Create, FileAccess.Write))
                    {
                        var bFormatter = new BinaryFormatter();
                        bFormatter.Serialize(writeFile, _catalog);
                    }
                }
            }
            else
            {
                ;//MessageBox.Show(@"El Robot de Enlace esta apagado. No se creara su catalogo correspondiente",
                //                @"Creacion catalogo Robot de Enlace", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private bool Validar(string tbPasaporteInicial, string tbPasaporteFinal,
                             TipoPasaporte selectedType, string mes, string anho)
        {
            string idFirstPasaporte = tbPasaporteInicial;
            string idLastPasaporte = tbPasaporteFinal;

            string error;

            if (selectedType == null)
            {
                error = @"Debe seleccionar un tipo de pasaporte";
                return false;
            }

            if (!DatosCatalogo.IsIdPasaporteIniCorrect(out error, idFirstPasaporte, selectedType))
            {
                error = @"Especifición errónea";
                return false;
            }

            if (!DatosCatalogo.IsIdPasaporteEndCorrect(out error, idLastPasaporte, idFirstPasaporte, selectedType))
            {
                error = @"Especifición errónea";
                return false;
            }

            if (!DatosCatalogo.IsNPasaportesCorrect(out error, idFirstPasaporte, idLastPasaporte, selectedType))
            {
                error = @"Error de formato";
                return false;
            }

            string fechaLam = mes + @"/" + anho;
            if (fechaLam == null && selectedType.TieneFechaDeLaminacion)
            {
                error = @"Debe seleccionar una fecha de laminación";
                return false;
            }
            if (fechaLam == null && !selectedType.TieneFechaDeLaminacion)
                fechaLam = "no";


            _catalog = new DatosCatalogoRobotEnlace(selectedType, idFirstPasaporte, idLastPasaporte, fechaLam);
            return true;
        }
    }
}