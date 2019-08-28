using System.Collections.Generic;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    public static class ProveCatalog
    {
        public const int NInicialPalets = 6; //MDG.2011-06-27.//4;//MDG.2011-06-22.Pasamos a 4 palets en la pila //2;

        static ProveCatalog()
        {
            var country = new Country("españa", 4);
            TipoPasaporte.Types subTipoPasaporte = TipoPasaporte.Types.Normal;
            TipoPasaporte tipoPasaporte = TipoPasaporte.Español;
            var paletizerDefinition = new PaletizerDefinition("ProveCatalog")
                                          {
                                              CoparerType = ComparerMosaicTypes.Tipe1,
                                              Item = PaletizadoElements.Create(PaletizableTypes.box),
                                              MosaicTypes =
                                                  new List<MosaicType>
                                                      {MosaicType.MosaicoPrueba, MosaicType.MosaicoPrueba},
                                              Palet = PaletizadoElements.Create(PaletTypes.EuroPalet),
                                              Separator = PaletizadoElements.Create(SeparatorTypes.Carton)
                                          };

            Catalog = new DatosCatalogoPaletizado(tipoPasaporte,
                                                  "AB000001", "AB060000",
                                                  paletizerDefinition, IDLine.Japonesa, "");
        }

        public static DatosCatalogoPaletizado Catalog { get; private set; }
    }
}