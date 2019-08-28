using System.Net;

namespace Idpsa.Paletizado
{
    public static class ConfigPaletizadoComunication
    {
        public const int BarcodePort = 3;
        public const int TiltPort = 4;
        public const int BasculaEntradaManualRulamatPort = 1;
        public const int RfidEntradaManualRulamatPort = 15;//electronica buena
        public const int RfidEntradaManualRulamatPort_alternativo = 11;//eletronica normal
        public const int RfidEntradaManualRulamatPort_alternativo2 = 10;//eletronica buena 2//MDG.2013-05-16
        public const int RfidRearme = 9;//electronica buena
        public const int RfidRearme_alt = 17;//eletronica normal
        public const int RfidRearme_alt2 = 18;//eletronica buena 2//MDG.2013-05-16

        //public static readonly IPEndPoint EnlaceJaponesaIpEndPoint = new IPEndPoint(IPAddress.Loopback, 1000);
        public static readonly IPEndPoint EnlaceJaponesaIpEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.13"), 1140);

        //public static readonly IPEndPoint EnlaceRulamatIpEndPoint = new IPEndPoint(IPAddress.Loopback, 1001);
        public static readonly IPEndPoint EnlaceRulamatIpEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.13"), 20001);
    }
}