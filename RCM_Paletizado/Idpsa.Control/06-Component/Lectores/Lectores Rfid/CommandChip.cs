using System;
using System.Collections.Generic;
using System.Text;

namespace Idpsa.Control.Component
{
    [Serializable]
    public static class CommandChip
    {
        public const ushort RequestLenDf = 12;
        public const ushort RequestLenEf = 7;
        public const ushort RequestLenId = 5;

        public static byte[] SelectDf()
        {
            var hlRequest = new byte[RequestLenDf];
            hlRequest[0] = 0x00;
            hlRequest[1] = 0xA4;
            hlRequest[2] = 0x04;
            hlRequest[3] = 0x00;
            hlRequest[4] = 0x07;
            hlRequest[5] = 0xA0;
            hlRequest[6] = 0x00;
            hlRequest[7] = 0x00;
            hlRequest[8] = 0x02;
            hlRequest[9] = 0x47;
            hlRequest[10] = 0x10;
            hlRequest[11] = 0x01;
            return hlRequest;
        }

        public static byte[] SelectEf()
        {
            var hlRequest = new byte[RequestLenEf];
            hlRequest[0] = 0x00;
            hlRequest[1] = 0xA4;
            hlRequest[2] = 0x02;
            hlRequest[3] = 0x00;
            hlRequest[4] = 0x02;
            hlRequest[5] = 0x2F;
            hlRequest[6] = 0x1C;
            return hlRequest;
        }

        public static byte[] SelectId()
        {
            var hlRequest = new byte[RequestLenId];
            hlRequest[0] = 0x00;
            hlRequest[1] = 0xB0;
            hlRequest[2] = 0x00;
            hlRequest[3] = 0x02;
            hlRequest[4] = 0x09;
            return hlRequest;
        }

        public static bool CheckDir(IList<byte> response)
        {
            var ok90 = response[23];
            var ok00 = response[24];
            return (ok90 == 0x90) && (ok00 == 0x00);
        }

        public static bool CheckFile(IList<byte> response)
        {
            var ok90 = response[14];
            var ok00 = response[15];
            return (ok90 == 0x90) && (ok00 == 0x00);
        }

        public static bool CheckData(IList<byte> response)
        {
            var ok90 = response[9];
            var ok00 = response[10];
            return (ok90 == 0x90) && (ok00 == 0x00);
        }

        public static string GetPassportId(IEnumerable<byte> response, int length)
        {
            return new string(Encoding.ASCII.GetString((byte[])response).ToCharArray(), 0, length);
        }
    }
}