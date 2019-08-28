using System.Runtime.InteropServices;

namespace Tester
{
    internal class RFIDReader
    {
        public RFIDReader(uint _id)
        {
            Init(_id);
        }

        public bool rfidConnect()
        {
            return ConnectReader();
        }

        public bool rfidDisconnect()
        {
            return DisconnectReader();
        }

        public bool rfidRead(out string code)
        {
            bool found = false;
            code = Read(ref found);
            return found;
        }

        public void rfidReset()
        {
            DisconnectReader();
            ConnectReader();
        }

        public bool rfidIsConnected()
        {
            return IsConnected();
        }

        [DllImport("RFID.dll")]
        public static extern void Init(uint m_iComport);

        [DllImport("RFID.dll")]
        public static extern bool ConnectReader();

        [DllImport("RFID.dll")]
        public static extern bool DisconnectReader();

        [DllImport("RFID.dll")]
        public static extern void ResetReader();

        [DllImport("RFID.dll")]
        public static extern string Read(ref bool found);

        [DllImport("RFID.dll")]
        public static extern bool IsConnected();

        #region LEDS

        public bool IsLedRedON()
        {
            bool ret = IsLedRedOn();
            return ret;
        }

        public bool IsLedGreenON()
        {
            bool ret = IsLedGreenOn();
            return ret;
        }

        public bool IsLedOrangeON()
        {
            bool ret = IsLedOrangeOn();
            return ret;
        }

        [DllImport("RFID.dll")]
        public static extern bool IsLedRedOn();

        [DllImport("RFID.dll")]
        public static extern bool IsLedGreenOn();

        [DllImport("RFID.dll")]
        public static extern bool IsLedOrangeOn();

        public bool SetLedRED()
        {
            bool ret = SetLedRed();
            return ret;
        }

        public bool SetLedGREEN()
        {
            bool ret = SetLedGreen();
            return ret;
        }

        public bool SetLedORANGE()
        {
            bool ret = SetLedOrange();
            return ret;
        }

        [DllImport("RFID.dll")]
        public static extern bool SetLedRed();

        [DllImport("RFID.dll")]
        public static extern bool SetLedGreen();

        [DllImport("RFID.dll")]
        public static extern bool SetLedOrange();

        public bool RetLedRED()
        {
            bool ret = RetLedRed();
            return ret;
        }

        public bool RetLedGREEN()
        {
            bool ret = RetLedGreen();
            return ret;
        }

        public bool RetLedORANGE()
        {
            bool ret = RetLedOrange();
            return ret;
        }

        [DllImport("RFID.dll")]
        public static extern bool RetLedRed();

        [DllImport("RFID.dll")]
        public static extern bool RetLedGreen();

        [DllImport("RFID.dll")]
        public static extern bool RetLedOrange();

        #endregion
    }
}