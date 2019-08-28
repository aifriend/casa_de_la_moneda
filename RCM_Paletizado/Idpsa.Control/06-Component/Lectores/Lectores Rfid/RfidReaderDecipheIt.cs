using System;
using System.Collections.Generic;
using System.Threading;
using Idpsa.Control.Manuals;
using Idpsa.Control.Subsystem;
using URMAPI;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class RfidReaderDecipheIt : RfidReader, IManualsProvider, IOriginDefiner
    {
        public struct PassportType
        {
            public const string None = @"NOREAD";
            public const string A = @"PR-NXP";
            public const string B = @"PR-IFX";
        }

        private readonly URM _urm;

        public RfidReaderDecipheIt(string name, int usbPort) : base(name)
        {
            try
            {
                _urm = new URM { Ifc = Interface.IfcRs232Dle, Comport = (uint)usbPort };
                _urm.Connect();
                //SetOnlyGreen();
            }
            catch {}
        }

        public RfidReaderDecipheIt(string name, int usbPort, int usbPortAlternativo)
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

        public RfidReaderDecipheIt(string name, int usbPort, int usbPortAlternativo, int usbPortAlternativo2)
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
            code = string.Empty;
            var chipType = FindChipType();
            switch (chipType)
            {
                case PassportType.A:
                    code = GetSupportIdFromChipA(9);
                    found = true;
                    break;
                case PassportType.B:
                    code = GetSupportIdFromChipB(9);
                    found = true;
                    break;
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

                ret = _urm.Iso14443B.Select(1, 1, atqb);

                if (ret == RetCodes.SW_OK)
                {
                    return PassportType.B;
                }
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

                byte uidLength = 0;
                byte sak;

                //var ret = _urm.Intern.RfReset(50);

                //if (ret != RetCodes.SW_OK)
                //{
                //    return string.Empty;
                //}

                var ret = _urm.Iso14443A.ActivateIdle(uid, ref uidLength, atq, out sak);

                if (ret == RetCodes.SW_OK)
                {
                    var ats = new byte[256];
                    ushort atsLen = 0;

                    ret = _urm.Iso14443A.RATS(0, ats, ref atsLen);

                    if (ret == RetCodes.SW_OK)
                    {
                        var objAts = new ATS(ats, atsLen) {Dri = 0, Dsi = 0};

                        uint responseLen = 0x00;
                        var hlResponse = new byte[1024];

                        var tclOptions = new TclOptions(objAts);
                        _urm.Intern.SetTclOptions(tclOptions);
                        _urm.Tcl.Transmit(0, 0, CommandChip.SelectDf(), CommandChip.RequestLenDf, hlResponse,
                                          ref responseLen);
                        if (CommandChip.CheckDir(hlResponse))
                        {
                            _urm.Tcl.Transmit(0, 0, CommandChip.SelectEf(), CommandChip.RequestLenEf, hlResponse,
                                              ref responseLen);
                            if (CommandChip.CheckFile(hlResponse))
                            {
                                _urm.Tcl.Transmit(0, 0, CommandChip.SelectId(), CommandChip.RequestLenId, hlResponse,
                                                  ref responseLen);
                                if (CommandChip.CheckData(hlResponse))
                                {
                                    return CommandChip.GetPassportId(hlResponse, length);
                                }
                            }
                        }
                    }
                }
            }
            catch {}
            return PassportType.None;
        }

        private string GetSupportIdFromChipB(int length)
        {
            try
            {
                var atqb = new byte[12];

                var ret = _urm.Iso14443B.Select(1, 1, atqb);

                if (ret == RetCodes.SW_OK)
                {
                    var objAtqb = new ATQB(atqb);

                    const int requestLen = 12;
                    var hlRequest = new byte[requestLen];
                    ushort responseLenAtt = 0x00;
                    uint responseLen = 0x00;
                    var hlResponse = new byte[1024];
                    byte mbli = 0;

                    objAtqb.Dri = 0;
                    objAtqb.Dsi = 0;

                    _urm.Iso14443B.Attrib(0, objAtqb.Pupi, 0, 0, objAtqb.PType, hlRequest, 0, hlResponse,
                                          ref responseLenAtt, ref mbli);
                    var tclOptions = new TclOptions(objAtqb, mbli);
                    _urm.Intern.SetTclOptions(tclOptions);
                    _urm.Tcl.Transmit(0, 0, CommandChip.SelectDf(), CommandChip.RequestLenDf, hlResponse,
                                      ref responseLen);
                    if (CommandChip.CheckDir(hlResponse))
                    {
                        _urm.Tcl.Transmit(0, 0, CommandChip.SelectEf(), CommandChip.RequestLenEf, hlResponse,
                                          ref responseLen);
                        if (CommandChip.CheckFile(hlResponse))
                        {
                            _urm.Tcl.Transmit(0, 0, CommandChip.SelectId(), CommandChip.RequestLenId, hlResponse,
                                              ref responseLen);
                            if (CommandChip.CheckData(hlResponse))
                            {
                                return CommandChip.GetPassportId(hlResponse, length);
                            }
                        }
                    }
                }
            }
            catch {}
            return PassportType.None;
        }

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
    }
}