using System.Net;


namespace Idpsa.Paletizado
{
    public static class ConfigCom
    {
        public static readonly IPEndPoint EnlaceJaponesaIpEndPoint = //new IPEndPoint(IPAddress.Loopback, 1000);
                   new IPEndPoint(IPAddress.Parse("192.168.1.13"), 1140);

        public static readonly IPEndPoint Loopback =
            new IPEndPoint(IPAddress.Loopback, 1000);

        public static readonly int TiltPort = 4;
        public static readonly int BarcodePort = 3;//ALVARO_20101125 7;
    }
    
    
}