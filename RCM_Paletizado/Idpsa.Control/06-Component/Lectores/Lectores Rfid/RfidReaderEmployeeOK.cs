using System;
using System.Collections.Generic;
using System.Threading;
using Idpsa.Control.Manuals;
using Idpsa.Control.Subsystem;
using URMAPI;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class RfidReaderEmployee : RfidReader, IManualsProvider, IOriginDefiner
    {
        public struct PassportType
        {
            public const string None = @"NOREAD";
            public const string A = @"PR-NXP";
            public const string B = @"PR-IFX";
        }

        private readonly URM _urm;

        public RfidReaderEmployee(string name, int usbPort) : base(name)
        {
            try
            {
                _urm = new URM { Ifc = Interface.IfcRs232Dle, Comport = (uint)usbPort };
                _urm.Connect();
                //SetOnlyGreen();
            }
            catch {}
        }

        public RfidReaderEmployee(string name, int usbPort, int usbPortAlternativo)
            : base(name)
        {
            try
            {
                _urm = new URM { Ifc = Interface.IfcRs232Dle, Comport = (uint)usbPort };
                //_urm.Connect()();
                if(!_urm.Connect())
                {
                    _urm = new URM { Ifc = Interface.IfcRs232Dle, Comport = (uint)usbPortAlternativo };
                    if(!_urm.Connect())
                    {
                        _urm = new URM { Ifc = Interface.IfcRs232Dle, Comport = (uint)usbPort };
                        _urm.Connect();
                    }
                }
                //SetOnlyGreen();
            }
            catch { }
        }

        public RfidReaderEmployee(string name, int usbPort, int usbPortAlternativo, int usbPortAlternativo2)
            : base(name)
        {
            try
            {
                _urm = new URM { Ifc = Interface.IfcRs232Dle, Comport = (uint)usbPort };
                if (!_urm.Connect())
                {
                    _urm = new URM { Ifc = Interface.IfcRs232Dle, Comport = (uint)usbPortAlternativo };
                    if (!_urm.Connect())
                    {
                        _urm = new URM { Ifc = Interface.IfcRs232Dle, Comport = (uint)usbPortAlternativo2 };
                        if (!_urm.Connect())
                        {
                            _urm = new URM {Ifc = Interface.IfcRs232Dle, Comport = (uint) usbPort};
                            _urm.Connect();
                        }
                    }
                }
                //SetOnlyGreen();
            }
            catch { }
        }
        public override void Dispose()
        {
            if (Connected())
                Disconnect();
        }

        public override bool Connect()
        {
            var ret = false;
            if (!_urm.Connected)
                if (_urm.Connect())
                {
                    //SetOnlyGreen();
                    ret = true;
                }
            return ret;
        }

        public override bool Disconnect()
        {
            bool ret = false;
            //SetOnlyRed();
            if (_urm.Connected)
                if (_urm.Disconnect())
                    ret = true;
            return ret;
        }

        public override bool ReadCode(out string code)
        {
            var found = false;

            var vatq = new byte[2];
            var vuid = new byte[10];
            byte[] key = CommandChipA.SelectEf();
            byte[] readBytes = new byte[16], rData = new byte[1024];

            byte uidLength = 0;
            byte sak = 0;
            ushort rLen=0;

            code = "";

            try
            {
                byte sectorKey = 0;
                RetCodes ret2 = _urm.Intern.LoadKeyE2(0X60, key, 0);
                while (ret2 == RetCodes.SW_OK && sectorKey < 15)
                {
                    sectorKey++;
                    ret2 = _urm.Intern.LoadKeyE2(0X60, key, sectorKey);
                }

                //ret2 = _urm.Intern.ExchangeCase2(0x90, 0xCA, 0x00, 0x00, 64, rData,ref rLen);


                var ret1 = _urm.Iso14443A.ActivateIdle(vuid, ref uidLength, vatq, out sak);


                //ret1 = _urm.Iso14443A.Request(0x26, vatq);//0x52->request all;

                //ret1 = _urm.Iso14443A.Anticoll(0, 0x93, vuid);

                //ret1 = _urm.Iso14443A.Select(0x93, vuid, out sak);



                var ret3 = _urm.Iso14443A.Authentication(8, 2, 0x60);
                //sectorKey = 0;
                //while (ret3 != RetCodes.SW_OK && sectorKey < 15)
                //{
                //    this.Reset();
                //    sectorKey++;
                //    ret3 = _urm.Iso14443A.Authentication(0, 2, 0x60);

                //}


                //ret1 = _urm.Iso14443A.AuthKey(0, key, 0X60); 

                var ret4 = _urm.Iso14443A.Read(0x30, 8, readBytes, 16);
                //var ret = _urm.Iso14443A.ActivateIdle(vuid, ref uidLength, vatq, out sak);

                LastReadedCode = code;

                if (ret4 == RetCodes.SW_OK)
                {
                    for (var i = 0; i < 4; i++)
                    {
                        string aux = readBytes[i].ToString("X");
                        if (aux.Length > 1)
                            code += aux.Substring(1, 1);
                        ;
                    }

                    if (!String.IsNullOrEmpty(code))
                        found = true;

                    _urm.Iso14443A.PPS(0, 0, 0);
                }
            }
            catch { }

            return found;
        }

        public  bool ReadCode2(out string code)
        {
            var found = false;
            code = string.Empty;
            var chipType = FindChipType();
            switch (chipType)
            {
                case PassportType.A:
                    code = GetSupportIdFromChipA(9);
                    found = true;
                    break;
                //case PassportType.B:
                //    code = GetSupportIdFromChipB(9);
                //    found = true;
                //    break;
            }
            return found;
        }

        public override void Reset()
        {
            if (_urm.Connected) Disconnect();
            Thread.Sleep(2000);
            if (!_urm.Connected) Connect();
        }

        public override bool Connected()
        {
            return _urm.Connected;
        }

        private string FindChipType()
        {
            try
            {
                var atqb = new byte[12];
                var atq = new byte[2];
                var uid = new byte[10];

                byte uidLength = 0;
                byte sak;

                var ret = _urm.Iso14443A.ActivateIdle(uid, ref uidLength, atq, out sak);

                if (ret == RetCodes.SW_OK)
                {
                    return PassportType.A;
                }

                //ret = _urm.Iso14443B.Select(1, 1, atqb);

                //if (ret == RetCodes.SW_OK)
                //{
                //    return PassportType.B;
                //}
            }
            catch {}
            return PassportType.None;
        }

        private string GetSupportIdFromChipA(int length)
        {
            try
            {
               
                var atq = new byte[2];
                var uid = new byte[10];

                //BU_Activate_Click();

                byte uidLength = 0;
                byte sak;

                //var ret = _urm.Intern.RfReset(50);

                //if (ret != RetCodes.SW_OK)
                //{
                //    return string.Empty;
                //}
                byte[] key = CommandChipA.SelectEf();

                 //ret = _urm.Iso14443A.ActivateIdle(uid, ref uidLength, atq, out sak);
                //if (ret == RetCodes.SW_OK)
                var ret = _urm.Iso14443A.AuthKey(0, key, 0X60);//(0, key, 0X60);
                //if (ret == RetCodes.SW_OK)
                //    ret = _urm.Iso14443A.Auth  ( 0,2,0x60);  


                //key = new byte[12] { 0xB, 0x3, 0x4, 0xF, 0x6, 0x6, 0x2, 0x8, 0x1, 0xD, 0x1, 0xA };

                if (ret == RetCodes.SW_OK)
                {
                    var ats = new byte[256];
                    ushort atsLen = 4;

                    ret = _urm.Iso14443A.RATS(0, ats, ref atsLen);


                    if (ret == RetCodes.SW_OK)

                        ret = _urm.Iso14443A.PPS(0, ats[0], ats[1]); 

                    ;

                    if (ret == RetCodes.SW_OK)
                    {
                        var objAts = new ATS(ats, atsLen) {Dri = 0, Dsi = 0};

                        uint responseLen = 0x00;
                        var hlResponse = new byte[1024];

                        var tclOptions = new TclOptions(objAts);
                        _urm.Intern.SetTclOptions(tclOptions);
                        _urm.Tcl.Transmit(0, 0, CommandChipA.SelectDf(), CommandChipA.RequestLenDf, hlResponse,
                                          ref responseLen);
                        if (CommandChipA.CheckDir(hlResponse))
                        {
                            _urm.Tcl.Transmit(0, 0, CommandChipA.SelectEf(), CommandChipA.RequestLenEf, hlResponse,
                                              ref responseLen);
                            if (CommandChipA.CheckFile(hlResponse))
                            {
                                _urm.Tcl.Transmit(0, 0, CommandChipA.SelectId(), CommandChipA.RequestLenId, hlResponse,
                                                  ref responseLen);
                                if (CommandChipA.CheckData(hlResponse))
                                {
                                    return CommandChipA.GetPassportId(hlResponse, length);
                                }
                            }
                        }
                    }
                }
            }
            catch {}
            return PassportType.None;
        }

        //private string GetSupportIdFromChipB(int length)
        //{
        //    try
        //    {
        //        var atqb = new byte[12];

        //        var ret = _urm.Iso14443B.Select(1, 1, atqb);

        //        if (ret == RetCodes.SW_OK)
        //        {
        //            var objAtqb = new ATQB(atqb);

        //            const int requestLen = 12;
        //            var hlRequest = new byte[requestLen];
        //            ushort responseLenAtt = 0x00;
        //            uint responseLen = 0x00;
        //            var hlResponse = new byte[1024];
        //            byte mbli = 0;

        //            objAtqb.Dri = 0;
        //            objAtqb.Dsi = 0;

        //            _urm.Iso14443B.Attrib(0, objAtqb.Pupi, 0, 0, objAtqb.PType, hlRequest, 0, hlResponse,
        //                                  ref responseLenAtt, ref mbli);
        //            var tclOptions = new TclOptions(objAtqb, mbli);
        //            _urm.Intern.SetTclOptions(tclOptions);
        //            _urm.Tcl.Transmit(0, 0, CommandChipA.SelectDf(), CommandChipA.RequestLenDf, hlResponse,
        //                              ref responseLen);
        //            if (CommandChipA.CheckDir(hlResponse))
        //            {
        //                _urm.Tcl.Transmit(0, 0, CommandChipA.SelectEf(), CommandChipA.RequestLenEf, hlResponse,
        //                                  ref responseLen);
        //                if (CommandChipA.CheckFile(hlResponse))
        //                {
        //                    _urm.Tcl.Transmit(0, 0, CommandChipA.SelectId(), CommandChipA.RequestLenId, hlResponse,
        //                                      ref responseLen);
        //                    if (CommandChipA.CheckData(hlResponse))
        //                    {
        //                        return CommandChipA.GetPassportId(hlResponse, length);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch {}
        //    return PassportType.None;
        //}

        #region LEDS

        //private void SetLed(LED_ID ledId, bool ledStatus)
        //{
        //    var led = new bool[8];

        //    var ret = _urm.Intern.GetLed(led);

        //    if (ret == RetCodes.SW_OK)
        //    {
        //        if (ledId == LED_ID.RED)
        //            led[0] = ledStatus;
        //        else if (ledId == LED_ID.ORANGE)
        //            led[1] = ledStatus;
        //        else if (ledId == LED_ID.GREEN)
        //            led[2] = ledStatus;

        //        ret = _urm.Intern.SetLed(led);

        //        if (ret == RetCodes.SW_OK)
        //        {
        //        }
        //    }
        //}

        //private void SetLedRED()
        //{
        //    SetLed(LED_ID.RED, true);
        //}

        //private void SetLedGREEN()
        //{
        //    SetLed(LED_ID.GREEN, true);
        //}

        //private void SetOnlyRed()
        //{
        //    SetLedRED();
        //    RetLedGREEN();
        //}

        //private void SetOnlyGreen()
        //{
        //    RetLedRED();
        //    SetLedGREEN();
        //}

        //private void RetLedRED()
        //{
        //    SetLed(LED_ID.RED, false);
        //}

        //private void RetLedGREEN()
        //{
        //    SetLed(LED_ID.GREEN, false);
        //}

        //private enum LED_ID
        //{
        //    RED,
        //    ORANGE,
        //    GREEN
        //}

        #endregion

        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            var manual = new Manual(this) {Description = Name};
            return new[] {manual};
        }

        #endregion

        #region Miembros de IOriginDefiner

        bool IOriginDefiner.InOrigin()
        {
            return _urm != null && _urm.Connected;
        }

        #endregion

        private void BU_Activate_Click()
        {
            byte[] atq = new byte[2];
            byte[] uid = new byte[10];

            RetCodes ret;
            byte uidLength = 0;
            byte sak = 0;


            bool found = false;
            bool typeA = false;
            bool errorFlag = false;

            String ED_AtsAtqb="",
            ED_Fsci,
            ED_Fwi,
            ED_Sfgi,
            ED_Afi,
            ED_NrApp,
            ED_Adc,
            ED_pType,
            ED_Mbli,
            ED_Crc;

            

            ret = _urm.Intern.RfReset(50);

            if (ret != RetCodes.SW_OK)
            {
                return;
            }

            ret = _urm.Iso14443A.ActivateIdle(uid, ref uidLength, atq, out sak);


            if (ret == RetCodes.SW_OK)
            {
                found = true;
                typeA = true;
            }


            if (typeA == true)
            {
                byte[] ats = new byte[256];
                ushort atsLen = 0;

                ret = _urm.Iso14443A.RATS(0, ats, ref atsLen);

                if (ret != RetCodes.SW_OK)
                {
                    errorFlag = true;
                }

                if (errorFlag == false)
                {
                    ATS objAts = new ATS(ats, atsLen);

                    for (int i = 0; i < atsLen; i++)
                    {
                        ED_AtsAtqb += ats[i].ToString("X");
                    }

                    ED_Fsci = objAts.Fsci.ToString("X");
                    ED_Fwi = objAts.Fwi.ToString("X");
                    ED_Sfgi = objAts.Sfgi.ToString("X");
                    

                    objAts.Dri = 0;
                    objAts.Dsi = 0;

                    TclOptions tclOptions = new TclOptions(objAts);

                    ret = _urm.Iso14443A.PPS(0, 0, 0);

                    if (ret != RetCodes.SW_OK)
                        errorFlag = true;
                }

            }
        }

        #region CommandChipA
        [Serializable]
        public static class CommandChipA
        {
            public const ushort RequestLenDf = 6;
            public const ushort RequestLenEf = 6;
            public const ushort RequestLenId = 6;

            public static byte[] SelectDf()
            {

                //key = new byte[12] { 0xB, 0x3, 0x4, 0xF, 0x6, 0x6, 0x2, 0x8, 0x1, 0xD, 0x1, 0xA };
                var hlRequest = SelectEf();// new byte[RequestLenDf];
                //hlRequest[0] = 0x00;
                //hlRequest[1] = 0xA4;
                //hlRequest[2] = 0x04;
                //hlRequest[3] = 0x00;
                //hlRequest[4] = 0x07;
                //hlRequest[5] = 0xA0;
                //hlRequest[6] = 0x00;
                //hlRequest[7] = 0x00;
                //hlRequest[8] = 0x02;
                //hlRequest[9] = 0x47;
                //hlRequest[10] = 0x10;
                //hlRequest[11] = 0x01;
                return hlRequest;
            }

            public static byte[] SelectEf()
            {
                var hlRequest = new byte[RequestLenEf];
                hlRequest[0] = 0xB3;
                hlRequest[1] = 0x4F;
                hlRequest[2] = 0x66;
                hlRequest[3] = 0x28;
                hlRequest[4] = 0x1D;
                hlRequest[5] = 0x7A;
                //hlRequest[6] = 0x1C;
                return hlRequest;
            }

            public static byte[] SelectId()
            {
                var hlRequest = new byte[RequestLenId];
                hlRequest = SelectEf();
                //hlRequest[0] = 0x00;
                //hlRequest[1] = 0xB0;
                //hlRequest[2] = 0x00;
                //hlRequest[3] = 0x02;
                //hlRequest[4] = 0x09;
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
               // return new string(Encoding.ASCII.GetString((byte[])response).ToCharArray(), 0, length);
                return "";
            }
        }
        #endregion
    }
}