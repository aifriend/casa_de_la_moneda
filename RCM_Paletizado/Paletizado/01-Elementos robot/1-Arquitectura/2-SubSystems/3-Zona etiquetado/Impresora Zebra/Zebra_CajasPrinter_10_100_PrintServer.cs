using Idpsa.Control.Component;

namespace Idpsa.Paletizado
{
    public class Zebra_CajasPrinter_10_100_PrintServer : Zebra_CajasPrinter
    {
        public Zebra_CajasPrinter_10_100_PrintServer(string name)
            : base(name)
        {
        }

        public Zebra_CajasPrinter_10_100_PrintServer(string name, string printerName)
            : base(name, printerName)
        {
        }


        protected override void PrintString(string s)
        {
            RawPrinterHelper.SendStringToPrinter(PrinterName, s);
        }
    }
}