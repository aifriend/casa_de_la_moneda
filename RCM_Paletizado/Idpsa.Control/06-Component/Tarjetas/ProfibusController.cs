using System;
using System.Collections;
using System.Runtime.InteropServices;
using Idpsa.Control.Tool;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class ProfibusController : IBusController
    {
        ////Texts
        //private const int BOOTSTART = 4;
        //private const int COLDSTART = 2;
        //private const int CONFIGURATION_DOWNLOAD = 2;

        /////* CONFIG Cannot get the Station configuration         */
        //private const int DEV_CF_CANNOT_GET_STN_CONFIG = -158;

        /////* CONFIG Version of the descript table invalid        */
        //private const int DEV_CF_INVALID_DESCRIPT_VERSION = -150;

        /////* CONFIG Input offset is invalid                      */
        //private const int DEV_CF_INVALID_INPUT_OFFSET = -151;

        /////* CONFIG Invalid output offset                        */
        //private const int DEV_CF_INVALID_OUTPUT_OFFSET = -154;

        /////* CONFIG Data type mismatch                           */
        //private const int DEV_CF_MISMATCH_DATA_TYPE = -163;

        /////* CONFIG Empty slot mismatch                          */
        //private const int DEV_CF_MISMATCH_EMPTY_SLOT = -160;

        /////* CONFIG Input offset mismatch                        */
        //private const int DEV_CF_MISMATCH_INPUT_OFFSET = -161;

        /////* CONFIG Input size does not match configuration      */
        //private const int DEV_CF_MISMATCH_INPUT_SIZE = -153;

        /////* CONFIG Output offset mismatch                       */
        //private const int DEV_CF_MISMATCH_OUTPUT_OFFSET = -162;

        /////* CONFIG Output size does not match configuration     */
        //private const int DEV_CF_MISMATCH_OUTPUT_SIZE = -156;

        /////* CONFIG Module definition is missing                 */
        //private const int DEV_CF_MODULE_DEF_MISSING = -159;

        /////* CONFIG Module definition is missing,(no Slot/Idx)   */
        //private const int DEV_CF_MODULE_DEF_MISSING_NO_SI = -164;

        /////* CONFIG Input size is 0                              */
        //private const int DEV_CF_NO_INPUT_SIZE = -152;

        /////* CONFIG Output size is 0                             */
        //private const int DEV_CF_NO_OUTPUT_SIZE = -155;

        /////* CONFIG Station not configured                       */
        //private const int DEV_CF_STN_NOT_CONFIGURED = -157;

        //private const int DEVICE_MBX_EMPTY = 0;
        //private const int DEVICE_MBX_FULL = 1;

        /////* DRIVER Board not initialized                        */
        //private const int DRV_BOARD_NOT_INITIALIZED = -1;

        /////* USER Configuration file differs from database       */
        //private const int DRV_CF_DIFFERS_FROM_DBM = -128;

        /////* USER Configuration file download error              */
        //private const int DRV_CF_DOWNLOAD_ERROR = -126;

        /////* USER Not enough memory to load configuration file   */
        //private const int DRV_CF_FILE_NO_MEMORY = -122;

        /////* USER No flash segment in the configuration file     */
        //private const int DRV_CF_FILE_NO_SEGMENT = -127;

        /////* USER Configuration file not opend                   */
        //private const int DRV_CF_FILE_OPEN_FAILED = -120;

        /////* USER Configuration file read failed                 */
        //private const int DRV_CF_FILE_READ_FAILED = -123;

        /////* USER Configuration file size zero                   */
        //private const int DRV_CF_FILE_SIZE_ZERO = -121;

        /////* USER Configuration file name not valid              */
        //private const int DRV_CF_FILENAME_INVALID = -125;

        /////* USER Configuration file type invalid                */
        //private const int DRV_CF_INVALID_FILETYPE = -124;

        /////* DMA  Clear database on the device failed            */
        //private const int DRV_CLEAR_DB_FAIL = -57;

        /////* DRIVER Command on this channel is active            */
        //private const int DRV_CMD_ACTIVE = -4;

        /////* USER Database segment unknown                       */
        //private const int DRV_DBM_NO_FLASH_SEGMENT = -136;

        /////* USER Not enough memory to upload database           */
        //private const int DRV_DBM_NO_MEMORY = -132;

        /////* USER Database read failed                           */
        //private const int DRV_DBM_READ_FAILED = -133;

        /////* USER Database size zero                             */
        //private const int DRV_DBM_SIZE_ZERO = -131;

        /////* DEVICE IO data mode unknown                         */
        //private const int DRV_DEV_COM_MODE_UNKNOWN = -24;

        /////* DEVICE Dual port ram not accessable(board not found)*/
        //private const int DRV_DEV_DPM_ACCESS_ERROR = -10;

        /////* DEVICE DPM size differs from configuration          */
        //private const int DRV_DEV_DPMSIZE_MISMATCH = -26;

        /////* DEVICE IO data exchange failed                      */
        //private const int DRV_DEV_EXCHANGE_FAILED = -22;

        /////* DEVICE IO data exchange timeout                     */
        //private const int DRV_DEV_EXCHANGE_TIMEOUT = -23;

        /////* DEVICE Function call failed                         */
        //private const int DRV_DEV_FUNCTION_FAILED = -25;

        /////* DEVICE No message available                         */
        //private const int DRV_DEV_GET_NO_MESSAGE = -19;

        /////* DEVICE GetMessage timeout                           */
        //private const int DRV_DEV_GET_TIMEOUT = -18;

        /////* DEVICE Output port already in use                   */
        //private const int DRV_DEV_HW_PORT_IS_USED = -28;

        /////* DEVICE Send mailbox is full                         */
        //private const int DRV_DEV_MAILBOX_FULL = -16;

        /////* DEVICE COM-flag not set                             */
        //private const int DRV_DEV_NO_COM_FLAG = -21;

        /////* USER Virtual memory not available                   */
        //private const int DRV_DEV_NO_VIRTUAL_MEM = -60;

        /////* DEVICE Not ready (ready flag failed)                */
        //private const int DRV_DEV_NOT_READY = -11;

        /////* DEVICE Not running (running flag failed)            */
        //private const int DRV_DEV_NOT_RUNNING = -12;

        /////* DEVICE Signals wrong OS version                     */
        //private const int DRV_DEV_OS_VERSION_ERROR = -14;

        /////* DEVICE PutMessage timeout                           */
        //private const int DRV_DEV_PUT_TIMEOUT = -17;

        /////* DEVICE RESET command timeout                        */
        //private const int DRV_DEV_RESET_TIMEOUT = -20;

        /////* DEVICE State mode unknown                           */
        //private const int DRV_DEV_STATE_MODE_UNKNOWN = -27;

        /////* DEVICE Error in dual port flags                     */
        //private const int DRV_DEV_SYSERR = -15;

        /////* USER Unmap virtual memory failed                    */
        //private const int DRV_DEV_UNMAP_VIRTUAL_MEM = -61;

        /////* DEVICE Watchdog test failed                         */
        //private const int DRV_DEV_WATCHDOG_FAILED = -13;

        /////* DMA  Database download failed                       */
        //private const int DRV_DMA_DB_DOWN_FAIL = -55;

        /////* DRIVER General DMA error                            */
        //private const int DRV_DMA_ERROR = -71;

        /////* DMA  Firmware download failed                       */
        //private const int DRV_DMA_FW_DOWN_FAIL = -56;

        /////* DMA  Memory allocation error                        */
        //private const int DRV_DMA_INSUFF_MEM = -50;

        /////* DMA  Read I/O timeout                               */
        //private const int DRV_DMA_TIMEOUT_CH4 = -51;

        /////* DMA  Write I/O timeout                              */
        //private const int DRV_DMA_TIMEOUT_CH5 = -52;

        /////* DMA  PCI transfer timeout                           */
        //private const int DRV_DMA_TIMEOUT_CH6 = -53;

        /////* DMA  Download timeout                               */
        //private const int DRV_DMA_TIMEOUT_CH7 = -54;

        /////* USER Firmware file BOOTLOADER active                */
        //private const int DRV_FW_BOOTLOADER_ACTIVE = -118;

        /////* USER Firmware file download error                   */
        //private const int DRV_FW_DOWNLOAD_ERROR = -116;

        /////* USER Not enough memory to load firmware file        */
        //private const int DRV_FW_FILE_NO_MEMORY = -112;

        /////* USER Firmware file not opened                       */
        //private const int DRV_FW_FILE_OPEN_FAILED = -110;

        /////* USER Firmware file read failed                      */
        //private const int DRV_FW_FILE_READ_FAILED = -113;

        /////* USER Firmware file size zero                        */
        //private const int DRV_FW_FILE_SIZE_ZERO = -111;

        /////* USER Firmware file name not valid                   */
        //private const int DRV_FW_FILENAME_INVALID = -115;

        /////* USER Firmware file not found in the internal table  */
        //private const int DRV_FW_FILENAME_NOT_FOUND = -117;

        /////* USER Firmware file type invalid                     */
        //private const int DRV_FW_INVALID_FILETYPE = -114;

        /////* USER Firmware file no file path                     */
        //private const int DRV_FW_NO_FILE_PATH = -119;

        /////* DRIVER General error                                */
        //private const int DRV_GENERAL_ERROR = -70;

        /////* DRIVER Error in internal init state                 */
        //private const int DRV_INIT_STATE_ERROR = -2;

        ///* ------------------------------------------------------------------------------------ */
        ///*  driver errors                                                                       */
        ///* ------------------------------------------------------------------------------------ */
        ///* no error                                            */
        private const int DRV_NO_ERROR = 0;

        /////* DRIVER Unknown parameter in function occured        */
        //private const int DRV_PARAMETER_UNKNOWN = -5;

        /////* DRIVER Could not read PCI dual port memory length   */
        //private const int DRV_PCI_READ_DPM_LENGTH = -8;

        /////* DRIVER Error during PCI set run mode                */
        //private const int DRV_PCI_SET_CONFIG_MODE = -7;

        //private const int DRV_PCI_SET_RUN_MODE = -9;

        /////* RCS error number start                              */
        //private const int DRV_RCS_ERROR_OFFSET = 1000;

        /////* DRIVER Error in internal read state                 */
        //private const int DRV_READ_STATE_ERROR = -3;

        /////* USER Pointer to buffer is a null pointer            */
        //private const int DRV_USR_BUF_PTR_NULL = -45;

        /////* USER IOCTRL function failed                         */
        //private const int DRV_USR_COMM_ERR = -33;

        /////* USER Parameter DeviceNumber  invalid                */
        //private const int DRV_USR_DEV_NUMBER_INVALID = -34;

        /////* USER Device address null pointer                    */
        //private const int DRV_USR_DEV_PTR_NULL = -44;

        /////* USER device function not implemented                */
        //private const int DRV_USR_DEVICE_FUNC_NOTIMPL = -83;

        /////* USER device name invalid                            */
        //private const int DRV_USR_DEVICE_NAME_INVALID = -81;

        /////* USER device name unknown                            */
        //private const int DRV_USR_DEVICE_NAME_UKNOWN = -82;

        /////* USER driver unknown                                 */
        //private const int DRV_USR_DRIVER_UNKNOWN = -80;

        /////* USER Not enough memory to load file                 */
        //private const int DRV_USR_FILE_NO_MEMORY = -102;

        /////* USER File not opened                                */
        //private const int DRV_USR_FILE_OPEN_FAILED = -100;

        /////* USER File read failed                               */
        //private const int DRV_USR_FILE_READ_FAILED = -103;

        /////* USER File size zero                                 */
        //private const int DRV_USR_FILE_SIZE_ZERO = -101;

        /////* USER File name not valid                            */
        //private const int DRV_USR_FILENAME_INVALID = -105;

        /////* USER Parameter InfoArea unknown                     */
        //private const int DRV_USR_INFO_AREA_INVALID = -35;

        /////* USER Can't connect with device                      */
        //private const int DRV_USR_INIT_DRV_ERROR = -31;

        /////* USER File type invalid                              */
        //private const int DRV_USR_INVALID_FILETYPE = -104;

        /////* USER Parameter Mode invalid                         */
        //private const int DRV_USR_MODE_INVALID = -37;

        /////* USER NULL pointer assignment                        */
        //private const int DRV_USR_MSG_BUF_NULL_PTR = -38;

        /////* USER Message buffer too short                       */
        //private const int DRV_USR_MSG_BUF_TOO_SHORT = -39;

        /////* USER Board not initialized (DevInitBoard not called)*/
        //private const int DRV_USR_NOT_INITIALIZED = -32;

        /////* USER Parameter Number invalid                       */
        //private const int DRV_USR_NUMBER_INVALID = -36;

        /////* Error from Interface functions */
        /////* USER Driver not opened (device driver not loaded)   */
        //private const int DRV_USR_OPEN_ERROR = -30;

        /////* USER Pointer to buffer is a null pointer            */
        //private const int DRV_USR_RECVBUF_PTR_NULL = -49;

        /////* USER ReceiveSize parameter too long                 */
        //private const int DRV_USR_RECVSIZE_TOO_LONG = -47;

        /////* USER Pointer to buffer is a null pointer            */
        //private const int DRV_USR_SENDBUF_PTR_NULL = -48;

        /////* USER SendSize parameter too long                    */
        //private const int DRV_USR_SENDSIZE_TOO_LONG = -46;

        /////* USER Parameter Size invalid                         */
        //private const int DRV_USR_SIZE_INVALID = -40;

        /////* USER Parameter Size too long                        */
        //private const int DRV_USR_SIZE_TOO_LONG = -43;

        /////* USER Parameter Size with zero length                */
        //private const int DRV_USR_SIZE_ZERO = -42;

        /////* DRIVER Device WatchDog failed                       */
        //private const int DRV_WDG_DEV_ERROR = -75;

        /////* DRIVER I/O WatchDog failed                          */
        //private const int DRV_WDG_IO_ERROR = -74;

        /////* DRIVER Version is incompatible with DLL             */
        //private const int DRV_WRONG_DRIVER_VERSION = -6;

        //private const int FIRMWARE_DOWNLOAD = 1;
        //private const int FKT_CLOSE = 2;
        //private const int FKT_IO = 5;
        //private const int FKT_OPEN = 1;
        //private const int FKT_READ = 3;
        //private const int FKT_WRITE = 4;
        //private const int GET_CIF_PLC_DRIVER_INFO = 10;
        //private const int GET_DEV_INFO = 6;

        ////    /* ------------------------------------------------------------------------------------ */
        ////    /*  message definition                                                                  */
        ////    /* ------------------------------------------------------------------------------------ */
        ////    /* max. length is 288 Bytes, max message length is 255 + 8 Bytes */
        //////    private struct MSG_STRUC 
        //////    {
        //////      char   rx;
        //////      char   tx;
        //////      char   ln;
        //////      char   nr;
        //////      char   a;
        //////      char   f;
        //////      char   b;
        //////      char   e;
        //////      char[] data;// = new char[255];
        //////      char[] dummy;// = new char[25];      /* for compatibility with older definitions (288 Bytes) */
        //////    }
        ////    /* ------------------------------------------------------------------------------------ */
        ////    /*  INFO structure definitions                                                          */
        ////    /* ------------------------------------------------------------------------------------ */
        ////    /* DEVRESET */
        ////    /* GETINFO InfoArea definitions */
        //private const int GET_DRIVER_INFO = 1;
        //private const int GET_FIRMWARE_INFO = 3;
        //private const int GET_IO_INFO = 7;
        //private const int GET_IO_SEND_DATA = 8;
        //private const int GET_RCS_INFO = 5;
        //private const int GET_TASK_INFO = 4;
        //private const int GET_VERSION_INFO = 2;
        //private const int HOST_MBX_EMPTY = 0;
        //private const int HOST_MBX_FULL = 1;
        ////    /* HOST mode definition */
        //private const int HOST_NOT_READY = 0;
        private const int HOST_READY = 1;
        //private const int HW_PORT_CLEAR_OUTPUT = 2;
        //private const int HW_PORT_READ_INPUT = 3;
        //private const int HW_PORT_SET_OUTPUT = 1;

        /////* *****************************************************************
        ////    DEFINICIONES BASADAS EN : CIFUSER.H
        ////********************************************************************/
        /////* ------------------------------------------------------------------------------------ */
        /////*  global definitions                                                                  */
        /////* ------------------------------------------------------------------------------------ */
        /////* maximum numbers of boards  */
        //private const int MAX_DEV_BOARDS = 4;

        //private const int MEMORY_PTR_CREATE = 1;
        //private const int MEMORY_PTR_RELEASE = 2;

        ////    /* DEVREADWRITERAW / DEVREADWRITEDPMDATA */
        //private const int PARAMETER_READ = 1;
        //private const int PARAMETER_WRITE = 2;
        //private const int SPECIAL_CONTROL_CLEAR = 0;
        //private const int SPECIAL_CONTROL_SET = 1;
        ////    /* STATE definition  */
        //private const int STATE_ERR = 1;
        //private const int STATE_ERR_NON = 0;

        /////* State information in bWriteState and bReadState */
        ////0x01
        //private const int STATE_IN = 1;

        //private const int STATE_IN_IRQ = 4;

        //private const int STATE_MODE_2 = 2;
        //private const int STATE_MODE_3 = 3;
        //private const int STATE_MODE_4 = 4;
        ////    /* DEVSPECIALCONTROL */
        ////0x03
        //private const int STATE_OUT = 3;
        //private const int STATE_WAIT = 2;
        private const int TABLE_LENGTH_MAX = 512;
        private const short tableLength = 512;
        private const int TErrorInExchangeData = 230003;
        private const int TErrorToCloseBoard = 230005;
        private const int TErrorToCloseDriver = 230004;
        private const int TErrorToInitBoard = 230001;
        private const int TErrorToOpenDriver = 230000;
        private const int TMasterOfBusNotDetected = 230002;
        //private const int WARMSTART = 3;
        //private const int WATCHDOG_START = 1;
        //private const int WATCHDOG_STOP = 0;
        private readonly byte[] abReceiveData = new byte[TABLE_LENGTH_MAX + 1];
        private readonly byte[] abSendData = new byte[TABLE_LENGTH_MAX + 1];
        private readonly IOCollection dICollectionLocal = new IOCollection();
        private readonly IOCollection dOCollectionLocal = new IOCollection();
        private long pDevAddress = 0;
        private short tableAdress = 0;
        private long ulTimeout = 0;
        private short usBoardNumber = 0;

        public ProfibusController(int index, IOCollection dICollection, IOCollection dOCollection, int board)
        {
            int i;
            //Add only the items with the correct board
            for (i = 0; i <= dOCollection.Count - 1; i++)
            {
                if ((dOCollection[i].Board == board))
                {
                    dOCollectionLocal.Add(dOCollection[i].Name, dOCollection[i]);
                }
            }
            //Add only the items with the correct board
            for (i = 0; i <= dICollection.Count - 1; i++)
            {
                if ((dICollection[i].Board == board))
                {
                    dICollectionLocal.Add(dICollection[i].Name, dICollection[i]);
                }
            }
        }

        #region IBusController Members

        public void WakeUpDevice()
        {
            //base.WakeUpDevice(); 
            //Open Driver
            CheckIfError(DevOpenDriver(usBoardNumber), TErrorToOpenDriver, 0);
            ////Init Board
            CheckIfError(DevInitBoard(usBoardNumber, pDevAddress), TErrorToInitBoard, 0);
            ////Check if Host is Ready
            CheckIfError(DevSetHostState(usBoardNumber, HOST_READY, ulTimeout), TMasterOfBusNotDetected, 0);
        }

        public void RunDevice()
        {
            //base.RunDevice()
            SetOutputs();
            Refresh();
            ReadInputs();
        }

        public void ResetOutputs()
        {
            //abSendData <- outputs (convert to byte)
            Refresh();
            for (int i = 0; i < dOCollectionLocal.Count - 1; i += 8)
            {
                abSendData[i / 8] = 0;
            }
        }

        public bool IsBusOK()
        {
            return true;
        }

        #endregion

        //0x04
        //// ====================================================================================
        ////  funcion prototypes
        //// ====================================================================================
        [DllImport("CIF32DLL.dll")]
        private static extern short DevOpenDriver(short usDevNumber);

        [DllImport("CIF32DLL.dll")]
        private static extern short DevCloseDriver(short usDevNumber);

        [DllImport("CIF32DLL.dll")]
        private static extern short DevGetBoardInfo(short usDevNumber, short usSize, byte pvData);

        [DllImport("CIF32DLL.dll")]
        private static extern short DevInitBoard(short usDevNumber, long pDevAddress);

        [DllImport("CIF32DLL.dll")]
        private static extern short DevExitBoard(short usDevNumber);

        [DllImport("CIF32DLL.dll")]
        private static extern short DevReset(short usDevNumber, short usMode, long ulTimeout);

        [DllImport("CIF32DLL.dll")]
        private static extern short DevGetInfo(short usDevNumber, short usFunction, short usSize, byte pvData);

        [DllImport("CIF32DLL.dll")]
        private static extern short DevExchangeIO(short usDevNumber, short usSendOffset, short usSendSize,
                                                  ref byte pvSendData, short usReceiveOffset, short usReceiveSize,
                                                  ref byte pvReceiveData, long ulTimeout);

        [DllImport("CIF32DLL.dll")]
        private static extern short DevReadSendData(short usDevNumber, short usOffset, short usSize, byte pvData);

        [DllImport("CIF32DLL.dll")]
        private static extern short DevSetHostState(short usDevNumber, short usMode, long ulTimeout);

        [DllImport("CIF32DLL.dll")]
        private static extern short DevGetBoardInfoEx(short usDevNumber, short usOffset, short usSize, byte pvData);

        [DllImport("CIF32DLL.dll")]
        private static extern short DevExchangeIOEx(short usDevNumber, short usSendOffset, short usSendSize,
                                                    byte pvSendData, short usReceiveOffset, short usReceiveSize,
                                                    byte pvReceiveData, long ulTimeout);

        [DllImport("CIF32DLL.dll")]
        private static extern short DevExchangeIOErr(short usDevNumber, short usSendOffset, short usSendSize,
                                                     byte pvSendData, short usReceiveOffset, short usReceiveSize,
                                                     byte pvReceiveData, ComState ptState, long ulTimeout);

        ////Send and receive process data buffer

        private static void CheckIfError(int errorCode, int errorText, int errorNumber)
        {
            if ((errorCode != DRV_NO_ERROR))
            {
                int k = errorCode;
                //Throw (New DeviceException( _
                //GetString.Get(errorText) + Chr.Space + errorCode.ToString(), _
                //errorNumber, SeverityEnum.CriticalError))
            }
        }

        public void SleepDevice()
        {
            //base.SleepDevice ();
            //Close the Board
            CheckIfError(DevExitBoard(0), TErrorToCloseBoard, 0);
            //Close the Driver
            CheckIfError(DevCloseDriver(0), TErrorToCloseDriver, 0);
        }

        protected void ReadInputs()
        {
            var inputs = new BitArray(abReceiveData);
            for (int i = 0; i <= dICollectionLocal.Count - 1; i++)
            {
                dICollectionLocal[i].Value = inputs.Get(i);
            }
        }

        protected void SetOutputs()
        {
            int i;
            int j;
            //abSendData <- outputs (convert to byte)
            for (i = 0; i < dOCollectionLocal.Count - 1; i += 8)
            {
                int storeByte = 0;
                for (j = 0; j <= 7; j++)
                {
                    if (((j + i) < dOCollectionLocal.Count))
                    {
                        storeByte += AdvancedMath.WeightBit(dOCollectionLocal[j + i].Value, j);
                    }
                }
                abSendData[i / 8] = Convert.ToByte(storeByte);
            }
            //int i;
            //int j;
            //int storeByte = 0;
            ////abSendData <- outputs (convert to byte)
            //for (i = 0; i < dOCollectionLocal.Count - 1; i += 8) {
            //    storeByte = 0;
            //    for (j = 0; j <= 7; j++) {

            //        storeByte += (dOCollectionLocal[j + i].Value)?(1<<j):0;//AdvancedMath.WeightBit(dOCollectionLocal[j + i].Value, j);
            //        }
            //    }
            //    abSendData[i / 8] = Convert.ToByte(storeByte);
        }


        private void Refresh()
        {
            //Exchange Data
            CheckIfError(
                DevExchangeIO(0, tableAdress, (short)(tableAdress + tableLength), ref abSendData[0], tableAdress,
                              (short)(tableAdress + tableLength), ref abReceiveData[0], 0), TErrorInExchangeData, 0);
        }

        #region Nested type: COMSTATE

        private struct ComState
        {
            public ComState(string abState,short usMode, short usStateFlag)
            {
                AbState= abState;
                UsMode = usMode;
                UsStateFlag = usStateFlag;
            }

            public string AbState;

            ///* Actual STATE mode                              */
            public short UsMode;

            ///* State flag                                     */
            public short UsStateFlag;

            ////comment= new char[64];      /* State area                            
        }

        #endregion
    }
}