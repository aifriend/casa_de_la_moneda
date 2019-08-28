using System;
using System.Windows.Forms;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    public partial class UCLoadCatalogs : UserControl
    {
        public UCLoadCatalogs()
        {
            InitializeComponent();
        }

        public void SetCatalogsChangedHander(object obj, EventArgs e)
        {
            _ucLoadCatalogoLine1.SetCatalogsChangedHander(obj, e);
            _ucLoadCatalogoLine2.SetCatalogsChangedHander(obj, e);
        }


        public event UCLoadCatalogo.NewCatalogHandler NewCatalog
        {
            add
            {
                _ucLoadCatalogoLine1.NewCatalog += value;
                _ucLoadCatalogoLine2.NewCatalog += value;
            }
            remove
            {
                _ucLoadCatalogoLine1.NewCatalog -= value;
                _ucLoadCatalogoLine2.NewCatalog -= value;
            }
        }

        public void SetCatalog(DatosCatalogoPaletizado catalog)
        {
            if (catalog != null)
            {
                if (ConfigPaletizadoFiles.IntercambioDestinoEntradas == false)
                    //MDG.2010-07-13.Cambio asignacion entradas y salidas. 
                {
                    //Asignacion original: Entrada Japonesa a linea 1 y Entrada Alemana a Linea 2
                    if (catalog.IDLine == IDLine.Japonesa)
                        _ucLoadCatalogoLine1.Catalogo = catalog;
                    else if (catalog.IDLine == IDLine.Alemana)
                        _ucLoadCatalogoLine2.Catalogo = catalog;
                }
                else
                {
                    //MDG.2010-07-13.Cambio asignacion entradas y salidas. Entradas cruzadas
                    //Entrada Japonesa a linea 2 y Entrada Alemana a Linea 1
                    if (catalog.IDLine == IDLine.Alemana)
                        _ucLoadCatalogoLine1.Catalogo = catalog;
                    else if (catalog.IDLine == IDLine.Japonesa)
                        _ucLoadCatalogoLine2.Catalogo = catalog;
                }
            }
        }

        public DatosCatalogoPaletizado GetCatalog(IDLine line)
        {
            if (ConfigPaletizadoFiles.IntercambioDestinoEntradas == false)
                //MDG.2010-07-13.Cambio asignacion entradas y salidas. 
            {
                //Asignacion original: Entrada Japonesa a linea 1 y Entrada Alemana a Linea 2
                if (line == IDLine.Japonesa)
                    return _ucLoadCatalogoLine1.Catalogo;
                else if (line == IDLine.Alemana)
                    return _ucLoadCatalogoLine2.Catalogo;
                else
                    return null;
            }

            //Cambio asignacion entradas y salidas. Entradas cruzadas
            //Entrada Japonesa a linea 2 y Entrada Alemana a Linea 1
            if (line == IDLine.Alemana)
                return _ucLoadCatalogoLine1.Catalogo;
            return line == IDLine.Japonesa ? _ucLoadCatalogoLine2.Catalogo : null;
        }


        //public UCLoadCatalogs Initialize()
        public UCLoadCatalogs Initialize(IdpsaSystemPaletizado sys) //MDG.2011-06-16
        {
            if (ConfigPaletizadoFiles.IntercambioDestinoEntradas == false)
                //MDG.2010-07-13.Cambio asignacion entradas y salidas. 
            {
                //Asignacion original: Entrada Japonesa a linea 1 y Entrada Alemana a Linea 2
                _ucLoadCatalogoLine1.Initialize(IDLine.Japonesa, sys);
                _ucLoadCatalogoLine2.Initialize(IDLine.Alemana, sys);
            }
            else
            {
                //MDG.2010-07-13.Cambio asignacion entradas y salidas. Entradas cruzadas
                //Entrada Japonesa a linea 2 y Entrada Alemana a Linea 1
                _ucLoadCatalogoLine1.Initialize(IDLine.Alemana, sys);
                _ucLoadCatalogoLine2.Initialize(IDLine.Japonesa, sys);
            }
            return this;
        }
    }
}