using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class CompaxC3I20T11 : ISpecialDevice, IManualsProvider
    {
        #region DataFormat enum

        [Flags]
        public enum DataFormat
        {
            None = 0,
            Word = 1,
            DoubleWord = 2,
            ArrayOfWord = 4,
            ArrayOfDoubleWord = 8,
            U16 = 16 | Word,
            U32 = 32 | DoubleWord,
            I16 = 64 | Word,
            I32 = 128 | DoubleWord,
            C4_3 = 256 | DoubleWord
        }

        #endregion

        private static DataFormat FormatLength(DataFormat D)
        {
            DataFormat Value = DataFormat.None;
            if ((D & DataFormat.Word) == DataFormat.Word)
            {
                Value = DataFormat.Word;
            }
            else if ((D & DataFormat.DoubleWord) == DataFormat.DoubleWord)
            {
                Value = DataFormat.DoubleWord;
            }
            else if ((D & DataFormat.ArrayOfWord) == DataFormat.ArrayOfWord)
            {
                Value = DataFormat.ArrayOfWord;
            }
            else if ((D & DataFormat.ArrayOfDoubleWord) == DataFormat.ArrayOfDoubleWord)
            {
                Value = DataFormat.ArrayOfDoubleWord;
            }
            return Value;
        }

        #region "Almacenamiento de errores"

        private static readonly Dictionary<int, string> _errorList = LoadErrorList();

        private static Dictionary<int, string> LoadErrorList()
        {
            var EList = new Dictionary<int, string>();

            {
                EList.Add(0, "");
                EList.Add(1, "None");
                EList.Add(8978, "Effective drive current monitor");
                EList.Add(8992, "Overcurrent (power stage)");
                EList.Add(12592, "Phase is missing");
                EList.Add(12816, "DC link voltage exceeds limit");
                EList.Add(12834, "DC link voltage too low");
                EList.Add(16912, "Power output stage / device temperature > 85 C");
                EList.Add(17168, "Motor Temperature");
                EList.Add(20753, "Auxiliary Voltage 15V faulty");
                EList.Add(20754, "Overvoltage on 24V Control (X4/1 pin)");
                EList.Add(20757, "Auxiliary Voltage -15V faulty");
                EList.Add(20758, "Undervoltage 24V");
                EList.Add(20759, "Undervoltage - Additional I/O option");
                EList.Add(21376, "Short Circuit on Digital Output");
                EList.Add(21536, "Braking Resistor overloaded (Peak Current)");
                EList.Add(21537, "Braking Resistor overloaded (Continuous Current)");
                EList.Add(21632, "Short Circuit - Motor Brake");
                EList.Add(21633, "Open Circuit - Motor Brake");
                EList.Add(21649, "Power Stage Off / Low voltage on ENA_in (X4/3)");
                EList.Add(21664, "Limit switch I5 (X12/12) activated");
                EList.Add(21665, "Limit switch I6 (X12/13) activated");
                EList.Add(24392, "Effective motor current monitor");
                EList.Add(24593, "System overload 31.25us");
                EList.Add(24594, "System overload 500us");
                EList.Add(25216, "IEC61131-3 Division by zero");
                EList.Add(25217, "IEC61131-3 Cycle time exceeded");
                EList.Add(25218, "IEC61131-3 program stack overflow");
                EList.Add(25219, "IEC61131-3 FB overflow");
                EList.Add(25220, "IEC61131-3 illegal instruction");
                EList.Add(28961, "Motor Stall occurred");
                EList.Add(29056, "Motor Peak Current monitor");
                EList.Add(29456, "Speed too high");
                EList.Add(29472, "Position Tracking error");
                EList.Add(29475, "Target position beyond positive end limit");
                EList.Add(29476, "Target position beyond negative end limit");
                EList.Add(29479, "Change of direction during movement");
                EList.Add(29569, "Resolver output level too high");
                EList.Add(29570, "Resolver output level too low");
                EList.Add(29573, "Positiondifference between load mounted and motor feedback too high");
                EList.Add(29585, "Feedback level exceeds limit");
                EList.Add(29586, "Feedback level too low");
                EList.Add(29588, "Feedback eeprom data invalid");
                EList.Add(29589, "Error while storing data in feedback eeprom");
                EList.Add(29600, "Hall commutation: invalid combination of hall signals");
                EList.Add(29601, "Hall commutation: invalid correction value fine angle");
                EList.Add(29605, "Automatic commutation: no standstill of drive on start");
                EList.Add(29606, "Automatic commutation: Movement of more than 60 electrical degrees");
                EList.Add(29607, "Automatic commutation: Movement of more than 5 electrical degrees during phase 2");
                EList.Add(29608, "Automatic commutation: No standstill during phase 3");
                EList.Add(29609, "Automatic commutation: Timeout during phase 3");
                EList.Add(29610, "Automatic commutation: Too many trials during phase 3");
                EList.Add(29611, "Automatic commutation: Timeout");
                EList.Add(29612, "Automatic commutation: No motor connected");
                EList.Add(29616, "Distance coding: invalid reference mark position");
                EList.Add(29824, "Cam generator: invalid segment in linking table");
                EList.Add(29825, "Cam generator: invalid master segment distance");
                EList.Add(29826, "Cam generator: cam point not found");
                EList.Add(29827, "Cam generator: cam data error");
                EList.Add(29828, "Cam generator: cam point of coupling segment not found");
                EList.Add(29829, "Cam generator: cam data coupling segment error");
                EList.Add(29830, "Cam generator: multiple segment change");
                EList.Add(29831, "Cam generator: maximum allowable master or slave speed value exceeded");
                EList.Add(29832, "Cam generator: maximum allowable internal speed value exceeded");
                EList.Add(30096, "HEDA synchronization error");
                EList.Add(30097, "HEDA communication error");
                EList.Add(30098, "HEDA Acyclic Receiver Overrun");
                EList.Add(30099, "HEDA PLL failure");
                EList.Add(30100, "HEDA Cyclic Receiver Overrun");
                EList.Add(33040, "Receive buffer overflow");
                EList.Add(33056, "CRC error or passive mode (CAN)");
                EList.Add(33072, "FBI Timeout");
                EList.Add(33153, "Invalid velocity");
                EList.Add(33154, "CAM command error");
                EList.Add(33155, "Watchdog test movement");
                EList.Add(33168, "CamCommand: unknown command");
                EList.Add(33169, "CamCommand: SetC must be executed before SetM");
                EList.Add(33170, "CamCommand: command not permitted at present");
                EList.Add(33171, "CamCommand: invalid table row entry for the selected cam");
                EList.Add(33172, "CamCommand: invalid coupling segment");
                EList.Add(33173, "CamCommand: invalid segment");
                EList.Add(33174, "CamCommand: invalid master segment distance");
                EList.Add(33175, "CamCommand: invalid segment figure in cam memory");
                EList.Add(33176, "CamCommand: invalid delta value for quadratic coupling");
                EList.Add(33177, "CamCommand: internal value range overflow");
                EList.Add(33178, "CamCommand: invalid slope of quadratic coupling");
                EList.Add(33179, "CamCommand: invalid parameters for changing of master reset distance");
                EList.Add(33180, "CamCommand: invalid master source selected");
                EList.Add(33264, "Bus off (CAN)");
                EList.Add(33265, "Fieldbus synchronization error");
                EList.Add(34322, "Reference Limit");
                EList.Add(65281, "No object with this index available");
                EList.Add(65282, "No object with this subindex available");
                EList.Add(65283, "Object is \"read only\"");
                EList.Add(65284, "Object cannot be read");
                EList.Add(65285, "Version conflict; object data not valid in flash");
                EList.Add(65286, "No object for process data; object cannot be mapped");
                EList.Add(65287, "Data not valid");
                EList.Add(65288, "No convert function");
                EList.Add(65289, "Process data overflow");
                EList.Add(65296, "Command syntax error");
                EList.Add(65297, "Value not valid");
                EList.Add(65298, "Checksum error");
                EList.Add(65299, "Timeout error");
                EList.Add(65300, "Overflow error");
                EList.Add(65301, "Parity error");
                EList.Add(65302, "Frame error");
                EList.Add(65303, "Gateway Timeout");
                EList.Add(65312, "Flash sector delete failed");
                EList.Add(65313, "Program flash cell failed");
                EList.Add(65314, "Checksum error of prog. Flash area");
                EList.Add(65315, "DOWN/UPLOAD activated");
                EList.Add(65316, "DOWN/UPLOAD not activated");
                EList.Add(65328, "EEPROM Delay Count Error");
                EList.Add(65329, "Eeprom Timeout error");
                EList.Add(65330, "Eeprom ACK error");
                EList.Add(65344, "Memory for PLC could not be reserved");
                EList.Add(65345, "Stored data invalid");
                EList.Add(65346, "No objects available");
                EList.Add(65347, "No IEC61131 program");
                EList.Add(65349, "No FBI");
                EList.Add(65350, "Motor powered");
                EList.Add(65351, "Devicetype dif");
                EList.Add(65352, "LEI/CTP ID data");
                EList.Add(65353, "Irregular device state");
                EList.Add(65354, "SCI_FBI Timeout error");
                EList.Add(65355, "Nicht genügend Speicher für IEC Programm");
                EList.Add(65356, "IEC: object not found");
                EList.Add(65357, "IEC: Version conflict");
                EList.Add(65358, "Not enough memory for configured Scope settings available");
                EList.Add(65361, "HEDA timeout error");
                EList.Add(65362, "No HEDA Master");
                EList.Add(65376, "CANopen library");
                EList.Add(65377, "CANopen confirmation");
                EList.Add(65378, "No CANopen master");
                EList.Add(65392, "EnDat: Internal alarm \"illumination\"");
                EList.Add(65393, "EnDat: Internal alarm \"signal amplitude\"");
                EList.Add(65394, "EnDat: Internal alarm \"position error\"");
                EList.Add(65408, "EnDat: Internal warning \"frequency transgression\"");
                EList.Add(65409, "EnDat: Internal warning \"temperature transgression\"");
                EList.Add(65410, "EnDat: Internal warning \"control reserve illumination\"");
                EList.Add(65424, "Feedback System isn't compatible to the Compax3 Feedback Option");
                EList.Add(65425, "Invalid combination of hall signals rough commutation");
                EList.Add(65426, "Invalid commutation");
                EList.Add(65429, "EnDat: Timeout");
                EList.Add(65430, "EnDat: Error type A");
                EList.Add(65431, "EnDat: CRC error");
                EList.Add(65432, "Acknowledgement error");
                EList.Add(65433, "EnDat: Error type B");
                EList.Add(65434, "EnDat: invalid EEPROM parameter");
                EList.Add(65435, "EnDat: Speed too high");
                EList.Add(65436, "EnDat: No EnDat feedback");
                EList.Add(65441, "SinCos Analog signals outside specification");
                EList.Add(65442, "SinCos Internal angle offset fault");
                EList.Add(65443, "SinCos Table destroyed via data field partition");
                EList.Add(65444, "SinCos Analog limits not available");
                EList.Add(65445, "SinCos Internal I²C-Bus not functioning I²T?");
                EList.Add(65446, "SinCos Internal checksum error");
                EList.Add(65447, "SinCos Feedback reset via program supervision");
                EList.Add(65448, "SinCos Counter overflow");
                EList.Add(65449, "SinCos Parity error");
                EList.Add(65450, "SinCos Checksum of transmitted data is faulty");
                EList.Add(65451, "SinCos Unknown command code");
                EList.Add(65452, "SinCos Number of transmitted data is faulty");
                EList.Add(65453, "SinCos Improper command argument transmitted");
                EList.Add(65454, "SinCos the selected data field is not to be exceeded");
                EList.Add(65455, "SinCos Invalid access code");
                EList.Add(65456, "SinCos Size of the stated data field is not variable");
                EList.Add(65457, "SinCos Stated word address outside data field");
                EList.Add(65458, "SinCos access to non-existing data field. data field");
                EList.Add(65468, "SinCos Absolute value control of the analog signals");
                EList.Add(65469, "SinCos Transmitter current approaching limit");
                EList.Add(65470, "SinCos Feedback temperature approaching limit");
                EList.Add(65471, "SinCos Speed exceeds normal, no position generation permitted");
                EList.Add(65472, "SinCos Position Singleturn unreliable");
                EList.Add(65473, "SinCos Position error Multiturn");
                EList.Add(65474, "SinCos Position error Multiturn");
                EList.Add(65475, "SinCos Position error Multiturn");
                EList.Add(65476, "SinCos internal error");
                EList.Add(65488, "SinCos CRC");
                EList.Add(65489, "SinCos RX Timeout");
                EList.Add(65490, "SinCos RX Overrrun");
                EList.Add(65491, "SinCos RX Parity");
                EList.Add(65492, "SinCos RX Frame");
                EList.Add(65493, "Unknown SinCos encoder type");
                EList.Add(65494, "SinCos speed exceeds normal when writing encoder position");
                EList.Add(65495, "Wrong CTP");
                EList.Add(65496, "Transmitter system variant is not supported");
                EList.Add(65504, "MC Home is only permitted in the \"Standstill\" state (with energized drive)");
                EList.Add(65505, "CamOut not possible during coupling");
                EList.Add(65506, "Invalid parameter transfer when calling up a function block");
                EList.Add(65507, "Coupling/decoupling only possible with C3_CamIn or C3_CamOut Mode 0");
                EList.Add(65508, "C3 powerPLmC: invalid protocol");
                EList.Add(65514, "FBK system error 1");
                EList.Add(65515, "FBK system error 2");
                EList.Add(65516, "FBK system error 3");
                EList.Add(65517, "FBK system error 4");
            }
            return EList;
        }

        #endregion

        #region "Definición de la clase PkwCommand"

        #region Nested type: PkwCommand

        [Serializable]
        public class PkwCommand
        {
            #region PkwAction enum

            [Flags]
            public enum PkwAction
            {
                RecuestValue = 1,
                ChangeValue = 2,
                ValidateValue = 4,
                ChangeValidateValue = ChangeValue | ValidateValue
            }

            #endregion

            private PkwAction _action;

            public PkwCommand(PkwAction parAction, PkwStructure.PkwC3Object.PkwObjectType parC3Obj, double parC3ObjValue)
            {
                _action = parAction;
                Message = new PkwStructure(parC3Obj, parC3ObjValue);
                State = PkwState.NotStarted;
            }

            #region "Definición de la clase PlwStructure"

            [Serializable]
            public class PkwStructure
            {
                public const int Length = 64;
                private PkwAk _ak;

                public PkwStructure(PkwC3Object.PkwObjectType parC3Obj, double Value)
                {
                    _ak = new PkwAk(0);
                    C3Obj = PkwC3Object.Crear(parC3Obj, Value);
                }

                public PkwC3Object C3Obj { get; private set; }

                public PkwAk Ak
                {
                    get { return _ak; }
                    set { _ak = value; }
                }

                public bool Equivale(int parAk, string parPNU_subIndex, double PWEValue)
                {
                    bool valor = false;
                    if (_ak.Equivale(parAk) && C3Obj.Equivale(parPNU_subIndex, PWEValue))
                    {
                        valor = true;
                    }
                    return valor;
                }

                public override bool Equals(object obj)
                {
                    if (ReferenceEquals(null, obj)) return false;
                    if (ReferenceEquals(this, obj)) return true;
                    if (obj.GetType() != typeof (PkwStructure)) return false;
                    return Equals((PkwStructure)obj);
                }

                public bool Equals(PkwStructure obj)
                {
                    if (ReferenceEquals(null, obj)) return false;
                    if (ReferenceEquals(this, obj)) return true;
                    return Equals(obj._ak, _ak) && Equals(obj.C3Obj, C3Obj);
                }

                public override int GetHashCode()
                {
                    unchecked
                    {
                        return ((_ak != null ? _ak.GetHashCode() : 0) * 397) ^ (C3Obj != null ? C3Obj.GetHashCode() : 0);
                    }
                }

                #region "Definición de la clase PlwAk"

                [Serializable]
                public class PkwAk
                {
                    public const int AkLength = 4;
                    public const int Shift = 0;
                    private readonly int _Ak;
                    public readonly bool[] AkBits;

                    public PkwAk(int Ak)
                    {
                        _Ak = Ak;
                        var bits = new bool[AkLength];
                        for (int i = 0; i < AkLength; i++)
                        {
                            if ((_Ak & (1 << i)) != 0)
                            {
                                bits[i] = true;
                            }
                        }
                        AkBits = bits;
                        Array.Reverse(AkBits);
                    }

                    public bool Equivale(int v)
                    {
                        bool value = false;
                        if (_Ak == v)
                        {
                            value = true;
                        }
                        return value;
                    }

                    public new bool Equals(object obj)
                    {
                        bool value;
                        if (obj == null)
                        {
                            value = false;
                        }
                        else
                        {
                            if (ReferenceEquals(obj.GetType(), typeof (PkwAk)))
                            {
                                var V_Ak = (PkwAk)obj;
                                value = V_Ak._Ak == _Ak;
                            }
                            else
                            {
                                value = false;
                            }
                        }
                        return value;
                    }
                }

                #endregion

                #region "Definición de la clase PlwCompaxObject"

                [Serializable]
                public class PkwC3Object
                {
                    #region PkwObjectType enum

                    public enum PkwObjectType
                    {
                        STOP_decel,
                        STOP_jerk,
                        FSTOP1_decel,
                        FSTOP1_jerk,
                        FSTOP3_decel,
                        FSTOP3_jerk,
                        HOMING_home_offset,
                        Limit_Position_Positive,
                        Limit_Position_Negative,
                        POSITION_Jerk_accel,
                        POSITION_Jerk_decel,
                        JOG_speed,
                        JOG_accel,
                        JOG_jerk,
                        ValidParameter_Global,
                        ObjectDir_Objekts
                    }

                    #endregion

                    public readonly PNU_SubIndex Id;
                    public readonly PkwObjectType Name;
                    public readonly ObjectValue PWE;

                    private PkwC3Object(PkwObjectType name, PNU_SubIndex id, ObjectValue parPWE)
                    {
                        Name = name;
                        Id = id;
                        PWE = parPWE;
                    }

                    public static PkwC3Object Crear(PkwObjectType name, double value)
                    {
                        PkwC3Object Valor = null;
                        switch (name)
                        {
                            case PkwObjectType.STOP_decel:
                                Valor = new PkwC3Object(PkwObjectType.STOP_decel, new PNU_SubIndex("305.1"),
                                                        new ObjectValue((int)value, DataFormat.U32));
                                break;
                            case PkwObjectType.STOP_jerk:
                                Valor = new PkwC3Object(PkwObjectType.STOP_jerk, new PNU_SubIndex("306.1"),
                                                        new ObjectValue((int)value, DataFormat.U32));
                                break;
                            case PkwObjectType.FSTOP1_decel:
                                Valor = new PkwC3Object(PkwObjectType.FSTOP1_decel, new PNU_SubIndex("307.1"),
                                                        new ObjectValue((int)value, DataFormat.U32));
                                break;
                            case PkwObjectType.FSTOP1_jerk:
                                Valor = new PkwC3Object(PkwObjectType.FSTOP1_jerk, new PNU_SubIndex("308.1"),
                                                        new ObjectValue((int)value, DataFormat.U32));
                                break;
                            case PkwObjectType.FSTOP3_decel:
                                Valor = new PkwC3Object(PkwObjectType.FSTOP3_decel, new PNU_SubIndex("309.1"),
                                                        new ObjectValue((int)value, DataFormat.U32));
                                break;
                            case PkwObjectType.FSTOP3_jerk:
                                Valor = new PkwC3Object(PkwObjectType.FSTOP3_jerk, new PNU_SubIndex("310.1"),
                                                        new ObjectValue((int)value, DataFormat.U32));
                                break;
                            case PkwObjectType.Limit_Position_Positive:
                                Valor = new PkwC3Object(PkwObjectType.Limit_Position_Positive, new PNU_SubIndex("321.1"),
                                                        new ObjectValue((int)value, DataFormat.C4_3));
                                break;
                            case PkwObjectType.Limit_Position_Negative:
                                Valor = new PkwC3Object(PkwObjectType.Limit_Position_Negative, new PNU_SubIndex("322.1"),
                                                        new ObjectValue((int)value, DataFormat.C4_3));
                                break;
                            case PkwObjectType.HOMING_home_offset:
                                Valor = new PkwC3Object(PkwObjectType.HOMING_home_offset, new PNU_SubIndex("303.1"),
                                                        new ObjectValue((int)value, DataFormat.C4_3));
                                break;
                            case PkwObjectType.POSITION_Jerk_accel:
                                Valor = new PkwC3Object(PkwObjectType.POSITION_Jerk_accel, new PNU_SubIndex("313.1"),
                                                        new ObjectValue((int)value, DataFormat.U32));
                                break;
                            case PkwObjectType.POSITION_Jerk_decel:
                                Valor = new PkwC3Object(PkwObjectType.POSITION_Jerk_decel, new PNU_SubIndex("314.1"),
                                                        new ObjectValue((int)value, DataFormat.U32));
                                break;
                            case PkwObjectType.JOG_accel:
                                Valor = new PkwC3Object(PkwObjectType.JOG_accel, new PNU_SubIndex("315.1"),
                                                        new ObjectValue((int)value, DataFormat.U32));
                                break;
                            case PkwObjectType.JOG_speed:
                                Valor = new PkwC3Object(PkwObjectType.JOG_speed, new PNU_SubIndex("316.1"),
                                                        new ObjectValue((int)value, DataFormat.C4_3));
                                break;

                            case PkwObjectType.ValidParameter_Global:
                                Valor = new PkwC3Object(PkwObjectType.ValidParameter_Global, new PNU_SubIndex("338.11"),
                                                        new ObjectValue((int)value, DataFormat.U16));
                                break;
                            case PkwObjectType.ObjectDir_Objekts:
                                Valor = new PkwC3Object(PkwObjectType.ObjectDir_Objekts, new PNU_SubIndex("339.1"),
                                                        new ObjectValue((int)value, DataFormat.I16));
                                break;
                        }
                        return Valor;
                    }

                    public bool Equivale(string pnu_subIndex, double PWEValue)
                    {
                        bool valor = false;
                        if (Id.Equivale(pnu_subIndex) && PWE.Equivale(PWEValue))
                        {
                            valor = true;
                        }
                        return valor;
                    }

                    public override bool Equals(object obj)
                    {
                        if (ReferenceEquals(null, obj)) return false;
                        if (ReferenceEquals(this, obj)) return true;
                        if (obj.GetType() != typeof (PkwC3Object)) return false;
                        return Equals((PkwC3Object)obj);
                    }

                    public bool Equals(PkwC3Object obj)
                    {
                        if (ReferenceEquals(null, obj)) return false;
                        if (ReferenceEquals(this, obj)) return true;
                        return Equals(obj.Id, Id) && Equals(obj.Name, Name) && Equals(obj.PWE, PWE);
                    }

                    public override int GetHashCode()
                    {
                        unchecked
                        {
                            int result = (Id != null ? Id.GetHashCode() : 0);
                            result = (result * 397) ^ Name.GetHashCode();
                            result = (result * 397) ^ (PWE != null ? PWE.GetHashCode() : 0);
                            return result;
                        }
                    }

                    #region "Definición de la clase PNU_SubIndex"

                    [Serializable]
                    public class PNU_SubIndex
                    {
                        public const int IndiceLength = 11;
                        public const int Length = IndiceLength + SubIndiceLength;
                        public const int Shift = 5;
                        public const int SubIndiceLength = 8;
                        public bool[] Index;
                        public int PNU;
                        public int SubIndex;

                        public PNU_SubIndex(string Ind)
                        {
                            PNU = int.Parse(Ind.Split(new[] {'.'})[0]);
                            SubIndex = int.Parse(Ind.Split(new[] {'.'})[1]);
                            var BitsIndice = new bool[IndiceLength];
                            for (int i = 0; i <= IndiceLength - 1; i++)
                            {
                                if ((PNU & (1 << i)) != 0)
                                {
                                    BitsIndice[i] = true;
                                }
                            }
                            Array.Reverse(BitsIndice);
                            var bits = new bool[SubIndiceLength];
                            for (int i = 0; i <= SubIndiceLength - 1; i++)
                            {
                                if ((SubIndex & (1 << i)) != 0)
                                {
                                    bits[i] = true;
                                }
                            }
                            Array.Reverse(bits);
                            var data = new bool[Length];
                            BitsIndice.CopyTo(data, 0);
                            bits.CopyTo(data, IndiceLength);
                            Index = data;
                        }

                        public override string ToString()
                        {
                            return PNU + "." + SubIndex;
                        }

                        public bool Equivale(string v)
                        {
                            bool value = false;
                            int V_PNU = int.Parse(v.Split(new[] {'.'})[0]);
                            int V_SubIndex = int.Parse(v.Split(new[] {'.'})[1]);
                            if (V_PNU == PNU && V_SubIndex == SubIndex)
                            {
                                value = true;
                            }
                            return value;
                        }

                        ///<summary>
                        ///
                        ///                    Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
                        ///                
                        ///</summary>
                        ///
                        ///<returns>
                        ///true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
                        ///                
                        ///</returns>
                        ///
                        ///<param name="obj">
                        ///                    The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />. 
                        ///                </param>
                        ///<exception cref="T:System.NullReferenceException">
                        ///                    The <paramref name="obj" /> parameter is null.
                        ///                </exception><filterpriority>2</filterpriority>
                        public override bool Equals(object obj)
                        {
                            if (ReferenceEquals(null, obj)) return false;
                            if (ReferenceEquals(this, obj)) return true;
                            if (obj.GetType() != typeof (PNU_SubIndex)) return false;
                            return Equals((PNU_SubIndex)obj);
                        }

                        public bool Equals(PNU_SubIndex obj)
                        {
                            if (ReferenceEquals(null, obj)) return false;
                            if (ReferenceEquals(this, obj)) return true;
                            return Equals(obj.Index, Index) && obj.PNU == PNU && obj.SubIndex == SubIndex;
                        }

                        public override int GetHashCode()
                        {
                            unchecked
                            {
                                int result = (Index != null ? Index.GetHashCode() : 0);
                                result = (result * 397) ^ PNU;
                                result = (result * 397) ^ SubIndex;
                                return result;
                            }
                        }
                    }

                    #endregion

                    #region "Definición de la clase ObjectValue"

                    [Serializable]
                    public class ObjectValue
                    {
                        public const int Shift = 32;
                        public readonly DataFormat Format;
                        public readonly double Value;

                        public ObjectValue(int value, DataFormat format)
                        {
                            Value = value;
                            Format = format;
                        }

                        public bool Equivale(double v)
                        {
                            bool Valor = false;
                            if (Value == v)
                            {
                                Valor = true;
                            }
                            return Valor;
                        }

                        ///<summary>
                        ///
                        ///                    Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
                        ///                
                        ///</summary>
                        ///
                        ///<returns>
                        ///true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
                        ///                
                        ///</returns>
                        ///
                        ///<param name="obj">
                        ///                    The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />. 
                        ///                </param>
                        ///<exception cref="T:System.NullReferenceException">
                        ///                    The <paramref name="obj" /> parameter is null.
                        ///                </exception><filterpriority>2</filterpriority>
                        public override bool Equals(object obj)
                        {
                            if (ReferenceEquals(null, obj)) return false;
                            if (ReferenceEquals(this, obj)) return true;
                            if (obj.GetType() != typeof (ObjectValue)) return false;
                            return Equals((ObjectValue)obj);
                        }

                        public bool Equals(ObjectValue obj)
                        {
                            if (ReferenceEquals(null, obj)) return false;
                            if (ReferenceEquals(this, obj)) return true;
                            return Equals(obj.Format, Format) && obj.Value == Value;
                        }

                        public override int GetHashCode()
                        {
                            unchecked
                            {
                                return (Format.GetHashCode() * 397) ^ Value.GetHashCode();
                            }
                        }
                    }

                    #endregion
                }

                #endregion
            }

            #endregion

            public PkwStructure Message { get; private set; }
            public double ReadValue { get; set; }

            internal PkwState State { get; set; }

            public PkwAction Action
            {
                get { return _action; }
                set { _action = value; }
            }

            public int NewAk()
            {
                DataFormat Wide = FormatLength(Message.C3Obj.PWE.Format);
                int NAk = 0;
                switch (State)
                {
                    case PkwState.NotStarted:
                    case PkwState.NotValid:
                        if (_action == PkwAction.RecuestValue)
                        {
                            switch (Wide)
                            {
                                case DataFormat.Word:
                                    NAk = 1;
                                    break;
                                case DataFormat.DoubleWord:
                                    NAk = 1;
                                    break;
                                case DataFormat.ArrayOfWord:
                                case DataFormat.ArrayOfDoubleWord:
                                    NAk = 6;
                                    break;
                            }
                        }
                        else
                        {
                            switch (Wide)
                            {
                                case DataFormat.Word:
                                    NAk = 2;
                                    break;
                                case DataFormat.DoubleWord:
                                    NAk = 3;
                                    break;
                                case DataFormat.ArrayOfWord:
                                    NAk = 7;
                                    break;
                                case DataFormat.ArrayOfDoubleWord:
                                    NAk = 8;
                                    break;
                            }
                        }

                        break;
                    case PkwState.Started:
                    case PkwState.AskValid:
                        if (_action == PkwAction.RecuestValue)
                        {
                            switch (Wide)
                            {
                                case DataFormat.Word:
                                    NAk = 1;
                                    break;
                                case DataFormat.DoubleWord:
                                    NAk = 2;
                                    break;
                                case DataFormat.ArrayOfWord:
                                    NAk = 8;
                                    break;
                                case DataFormat.ArrayOfDoubleWord:
                                    NAk = 8;
                                    break;
                            }
                        }
                        else
                        {
                            switch (Wide)
                            {
                                case DataFormat.Word:
                                    NAk = 1;
                                    break;
                                case DataFormat.DoubleWord:
                                    NAk = 2;
                                    break;
                                case DataFormat.ArrayOfWord:
                                    NAk = 4;
                                    break;
                                case DataFormat.ArrayOfDoubleWord:
                                    NAk = 5;
                                    break;
                            }
                        }

                        break;
                }
                return NAk;
            }

            public void UpdateAk()
            {
                Message.Ak = new PkwStructure.PkwAk(NewAk());
            }
        }

        #endregion

        #region Nested type: PkwState

        internal enum PkwState
        {
            None,
            NotStarted,
            NotValid,
            AskValid,
            Started,
            Valid,
            Finish,
            Finished
        }

        #endregion

        #endregion

        #region "Definición de atributos"

        private readonly int _bitDPInputs;
        private readonly int _bitDPOutputs;
        private readonly int _bitInputs;
        private readonly int _bitOutputs;
        private readonly IOCollection _dICollection = new IOCollection();
        private readonly IOCollection _dOCollection = new IOCollection();
        private readonly Address _inputs;
        private readonly string _name;
        private readonly Address _outputs;
        private Func<bool> _jojPosEnable;
        private Func<bool> _jojNegEnable;
        private double _posicion;
        private int _stepsHoming;

        //48 W / 32
        public bool InvertJogSense { get; private set; }
        public string JogPosName { get; private set; }
        public string JogNegName { get; private set; }
        //80 W  /32
        public double MaxVelocity { get; private set; }

        public CompaxC3I20T11 WithMaxVelocity(double maxVelocity)
        {
            MaxVelocity = maxVelocity;
            return this;
        }

        private delegate void DelTypeMov();

        #endregion

        #region "Metodos y actributos poco usados"

        private bool Jog1Pos
        {
            set
            {
                if (!value || _jojPosEnable())
                    WriteControlBit(8, value);
            }
        }

        private bool Jog2Neg
        {
            set
            {
                if (!value || _jojNegEnable())
                    WriteControlBit(9, value);
            }
        }

        public bool Jog1
        {
            set
            {
                if (!InvertJogSense)
                    Jog1Pos = value;
                else
                    Jog2Neg = value;
            }
        }

        public bool Jog2
        {
            set
            {
                if (!InvertJogSense)
                    Jog2Neg = value;
                else
                    Jog1Pos = value;
            }
        }

        //----------------------Status word------------------------------------

        private void StartResetError()
        {
            WriteControlBit(7, false);
        }

        private void EndResetError()
        {
            WriteControlBit(7, true);
        }

        public bool Homing()
        {
            bool value = false;
            if (_stepsHoming == 0)
            {
                if (ReadControlBit(11))
                {
                    WriteControlBit(11, false);
                    _stepsHoming = 1;
                }
                else
                {
                    WriteControlBit(11, true);
                    _stepsHoming = 2;
                }
            }
            else if (_stepsHoming == 1)
            {
                WriteControlBit(11, true);
                _stepsHoming = 2;
            }
            else if (_stepsHoming == 2)
            {
                WriteControlBit(11, false);
                _stepsHoming = 0;
                value = true;
            }
            return value;
        }

        public void JogPos()
        {
            WriteControlBit(0, true);
            WriteControlBit(1, true);
            WriteControlBit(2, true);
            WriteControlBit(3, true);
            WriteControlBit(4, false);
            WriteControlBit(5, false);
            Jog1 = true; //WriteControlBit(8, true);
            Jog2 = false;
            WriteControlBit(10, true);
        }

        public void JogNeg()
        {
            WriteControlBit(0, true);
            WriteControlBit(1, true);
            WriteControlBit(2, true);
            WriteControlBit(3, true);
            WriteControlBit(4, false);
            WriteControlBit(5, false);
            //WriteControlBit(8, false);
            //WriteControlBit(9, true);
            Jog1 = false;
            Jog2 = true;
            WriteControlBit(10, true);
        }

        public bool TargetReached()
        {
            return ReadStatusBit(10);
        }

        public bool DriveMoving()
        {
            return !ReadStatusBit(13);
        }

        public void Parada()
        {
            WriteControlBit(3, false);
        }

        public bool EndMov()
        {            
            if (!DriveMoving() && (Math.Abs(Posicion - _targetPosAbs) <= 0.5))//MDG.2011-06-14.Porque trabajamos con enteros//0.5))           
                return true;
            
            return false;
        }

        #endregion

        #region "Determinación de estados"

        public string CurrentState()
        {
            string state;
            if (EnVoltageShichedOff())
            {
                state = "Voltage switched-off";
            }
            else if (EnSwitchOnInhibit())
            {
                state = "Switch-on inhibit";
            }
            else if (EnReadyToSwichOn())
            {
                state = "Ready to Switch-on";
            }
            else if (EnReady())
            {
                state = "Ready";
            }
            else if (EnOperationEnable())
            {
                state = "Operation Enable";
            }
            else if (EnFaultErrorReaction1())
            {
                state = "Error Reaction 1";
            }
            else if (EnFaultErrorReaction2y5())
            {
                state = "Error Reaction 2AND5";
            }
            else
            {
                state = "Desconocido";
            }
            return state;
        }

        private bool EnVoltageShichedOff()
        {
            bool value = true;
            for (int i = 0; i <= 15; i++)
            {
                value = value & !ReadStatusBit(i);
            }
            return value;
        }

        private bool EnSwitchOnInhibit()
        {
            return (!ReadStatusBit(0) && !ReadStatusBit(1) && !ReadStatusBit(2) && !ReadStatusBit(3) && ReadStatusBit(4) &&
                    ReadStatusBit(5) && ReadStatusBit(6) && ReadStatusBit(13));
        }

        private bool EnReadyToSwichOn()
        {
            return ReadStatusBit(0) && !ReadStatusBit(1) && !ReadStatusBit(2) && !ReadStatusBit(3) && ReadStatusBit(4) &&
                   ReadStatusBit(5) && !ReadStatusBit(6) && ReadStatusBit(13);
        }

        private bool EnReady()
        {
            return ReadStatusBit(0) && ReadStatusBit(1) && !ReadStatusBit(2) && !ReadStatusBit(3) && ReadStatusBit(4) &&
                   ReadStatusBit(5) && !ReadStatusBit(6) && ReadStatusBit(13);
        }

        public bool EnOperationEnable()
        {
            return ReadStatusBit(0) && ReadStatusBit(1) && ReadStatusBit(2) && !ReadStatusBit(3) && ReadStatusBit(4) &&
                   ReadStatusBit(5) && !ReadStatusBit(6);
        }

        private bool EnFaultErrorReaction2y5()
        {
            return !ReadStatusBit(0) && !ReadStatusBit(1) && !ReadStatusBit(2) && ReadStatusBit(3) && ReadStatusBit(4) &&
                   ReadStatusBit(5) && !ReadStatusBit(6) && ReadStatusBit(13);
        }

        private bool EnFaultErrorReaction1()
        {
            return ReadStatusBit(0) && ReadStatusBit(1) && ReadStatusBit(2) && ReadStatusBit(3) && ReadStatusBit(4) &&
                   ReadStatusBit(5) && !ReadStatusBit(6) && !ReadStatusBit(10) && ReadStatusBit(13);
        }

        #endregion

        #region "Control de transición de estados"

        private readonly TON _timerEnable = new TON();
        private int _quitErrorSteps;
        private int enableOperationSteps;

        public void Energizar()
        {
            TA2();
        }

        public void HabilitarDriver()
        {
            TA3();
        }

        public void HabilitarOperacion()
        {
            TA4();
        }


        public void QuitErrorReset()
        {
            _quitErrorSteps = 0;
        }
        
        public bool QuitError()
        {
            bool value = false;
            HabilitarOperacion();
            if (_quitErrorSteps == 0)
            {
                StartResetError();
                _quitErrorSteps = 1;
            }
            else if (_quitErrorSteps == 1)
            {
                _quitErrorSteps = 0;
                EndResetError();
                value = true;
            }
            return value;
        }

        public void EnableOperationReset()
        {
            enableOperationSteps = 0;
        }


        public bool Enable_Operation()
        {
            bool value = false;

            if (_timerEnable.Timing(200))
            {
                if (enableOperationSteps == 0)
                {
                    Energizar();
                    enableOperationSteps = 1;
                }
                else if (enableOperationSteps == 1)
                {
                    HabilitarDriver();
                    enableOperationSteps = 2;
                }
                else if (enableOperationSteps == 2)
                {
                    enableOperationSteps = 0;
                    HabilitarOperacion();
                    value = true;
                }
            }
            return value;
        }

        public void DriverResurrectionReset()
        {
            _quitErrorSteps = 0;
            enableOperationSteps = 0;
        }


        public bool DriverResurrection()
        {
            bool resurrection = false;

            if (LastCodeError == 1 && !EnOperationEnable())
                Enable_Operation();
            else if (!EnOperationEnable())
                QuitError();
            else if (LastCodeError == 1 && EnOperationEnable())
            {
                resurrection = true;
                DriverResurrectionReset();
            }
            return resurrection;
        }


        private void TA2()
        {
            WriteControlBit(0, false);
            WriteControlBit(1, true);
            WriteControlBit(2, true);
            WriteControlBit(10, true);
        }

        private void TA3()
        {
            WriteControlBit(0, true);
            WriteControlBit(1, true);
            WriteControlBit(2, true);
            WriteControlBit(10, true);
        }

        private void TA4()
        {
            WriteControlBit(0, true);
            WriteControlBit(1, true);
            WriteControlBit(2, true);
            WriteControlBit(3, true);
            WriteControlBit(10, true);
        }

        private void TC1(bool startEdge)
        {
            WriteControlBit(0, true);
            WriteControlBit(1, true);
            WriteControlBit(2, true);
            WriteControlBit(3, true);
            WriteControlBit(4, true);
            WriteControlBit(5, true);
            WriteControlBit(6, startEdge);
            WriteControlBit(10, true);
        }

        #endregion

        #region "Constructores"
               

        public CompaxC3I20T11(Address inputs, Address outputs, Bus bus, string name)
        {
            if (bus == null)
                throw new ArgumentNullException("bus");
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("name");
            _name = name;
            _inputs = inputs;
            _outputs = outputs;
            _dICollection = bus.InCollection;
            _dOCollection = bus.OutCollection;
            _bitInputs = (_inputs.Byte * 8) + _inputs.Bit;
            _bitOutputs = (_outputs.Byte * 8) + _outputs.Bit;
            _bitDPInputs = _bitInputs + PkwCommand.PkwStructure.Length;
            _bitDPOutputs = _bitOutputs + PkwCommand.PkwStructure.Length;
            _jojPosEnable = _jojNegEnable = () => true;
        }
          
        public CompaxC3I20T11 WithInvertedJogSense(bool invert)
        {
            InvertJogSense = invert;
            return this;
        }
        public CompaxC3I20T11 WithJojPosName(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("name");
            JogPosName = name;
            return this;
        }
        public CompaxC3I20T11 WithJojNegName(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("name");
            JogNegName = name;
            return this;
        }
        public CompaxC3I20T11 WithJojPosEnable(Func<bool> jojPosEnable)
        {
            if (jojPosEnable == null)
                throw new ArgumentNullException("jojPosEnable");
            _jojPosEnable = jojPosEnable;
            return this;
        }
        public CompaxC3I20T11 WithJojNegEnable(Func<bool> jojNegEnable)
        {
            if (jojNegEnable == null)
                throw new ArgumentNullException("jojNegEnable");
            _jojNegEnable = jojNegEnable;
            return this;
        }
        public CompaxC3I20T11 WithJojEnable(Func<bool> jojEnable)
        {
            if (jojEnable == null)
                throw new ArgumentNullException("jojEnable");
            _jojPosEnable = _jojNegEnable = jojEnable;
            return this;
        }

        #endregion

        #region "Posicionamiento directo"

        public string Name
        {
            get { return _name; }
        }

        public double Posicion
        {
            get
            {
                _posicion = ReadC4_3(_bitDPInputs + 16);
                return _posicion;
            }
            set
            {
                if (value >= -2147483647 & value <= 2147483647)
                {
                    _posicion = value;
                }
                else
                {
                    _posicion = 0;
                }
                WriteC4_3(_bitDPOutputs + 16, _posicion);
            }
        }


        public double Velocidad
        {
            get { return ReadC4_3(_bitDPInputs + 48); }
            set
            {
                if (!(value >= -32767 & value <= 32767))
                {
                    value = 0;
                }
                WriteC4_3(_bitDPOutputs + 48, value);
            }
        }


        public int LastCodeError
        {
            get { return ReadU16(_bitDPInputs + 80); }
        }

        public double Aceleracion
        {
            set
            {
                if (!(value >= -32767 & value <= 32767))
                {
                    value = 0;
                }
                WriteC4_3(_bitDPOutputs + 80, value);
            }
        }

        public double Torque
        {
            get { return ReadC4_3(_bitDPInputs + 96); }
        }

        public bool InPosition(double position, double error)
        {
            return (Math.Abs(Posicion - position) < error);
        }

        public string LastError()
        {
            string str;
            if (_errorList.ContainsKey(LastCodeError))
            {
                str = _errorList[LastCodeError];
            }
            else
            {
                str = "Error not found";
            }
            return str;
        }

        private void InicioStartAbsMovement(double P, double V, double A)
        {
            //If EnOperationEnable() AndAlso LastMotionOrderAcepted() Then
            TC1(false);
            Posicion = P;
            Velocidad = V;
            Aceleracion = A;
            //End If
            return;
        }

        #endregion

        #region "Lectura StatusWord"

        public bool ReadStatusBit(int pos)
        {
            return ReadWordBit(pos, _bitDPInputs);
        }

        public bool[] ReadStatusWord()
        {
            return ReadWord(_bitDPInputs);
        }

        #endregion

        #region "R/W ControlWord"

        public bool ReadControlBit(int pos)
        {
            var i = (int)Math.Floor((double)pos / 8);
            int j = pos % 8;
            return _dOCollection[_bitDPOutputs + (1 - i) * 8 + j].Value;
        }

        public bool[] ReadControlWord()
        {
            var word = new bool[16];
            for (int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    word[i * 8 + j] = _dOCollection[_bitDPOutputs + (1 - i) * 8 + j].Value;
                }
            }
            return word;
        }

        public void WriteControlBit(int pos, bool value)
        {
            WriteWordBit(pos, _bitDPOutputs, value);
        }

        public void WriteControlWord(bool[] word)
        {
            WriteWord(_bitDPOutputs, word);
        }

        #endregion

        #region "Lectura del profibus"

        public bool ReadWordBit(int pos, int offset)
        {
            var i = (int)Math.Floor((double)pos / 8);
            int j = pos % 8;
            return _dICollection[offset + (1 - i) * 8 + j].Value;
        }

        public bool[] ReadByte(int offset)
        {
            var Octeto = new bool[9];
            for (int i = 0; i <= 7; i++)
            {
                Octeto[i] = _dICollection[offset + i].Value;
            }
            return Octeto;
        }

        protected bool[] ReadWord(int offset)
        {
            var Word = new bool[16];
            for (int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    Word[i * 8 + j] = _dICollection[offset + (1 - i) * 8 + j].Value;
                }
            }
            return Word;
        }

        protected bool[] Read2Word(int offset)
        {
            var DWord = new bool[32];
            for (int i = 0; i <= 3; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    DWord[i * 8 + j] = _dICollection[offset + (3 - i) * 8 + j].Value;
                }
            }
            return DWord;
        }

        #endregion

        #region "Escritura del profibus"

        protected void WriteWordBit(int pos, int parOffset, bool value)
        {
            var i = (int)Math.Floor((double)pos / 8);
            int j = pos % 8;
            _dOCollection[parOffset + (1 - i) * 8 + j].Value = value;
        }

        protected void WriteByte(int parOffset, bool[] Octeto)
        {
            for (int i = 0; i <= 7; i++)
            {
                _dOCollection[parOffset + i].Value = Octeto[i];
            }
        }

        protected void WriteWord(int parOffset, bool[] word)
        {
            for (int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    _dOCollection[parOffset + (1 - i) * 8 + j].Value = word[i * 8 + j];
                }
            }
        }

        protected void Write2Word(int parOffset, bool[] word)
        {
            for (int i = 0; i <= 3; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    _dOCollection[parOffset + (3 - i) * 8 + j].Value = word[i * 8 + j];
                }
            }
        }

        protected void WriteSWord(int parOffset, bool[] word)
        {
            for (int i = 0; i <= 3; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    _dOCollection[i * 8 + j].Value = word[i * 8 + j];
                }
            }
        }

        protected void WriteMetaDataWord(int parOffset, bool[] word)
        {
            for (int i = 0; i <= 3; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    _dOCollection[i * 8 + j].Value = word[parOffset + (3 - i) * 8 + j];
                }
            }
        }

        #endregion

        #region "R/W profibus con formato"

        #region "Lecturas con formato"

        private int ReadI16(int parOffset)
        {
            int value = 0;
            bool[] word = ReadWord(parOffset);
            for (int i = 0; i <= word.Length - 1; i++)
            {
                if (word[i])
                {
                    value += (1 << i);
                }
            }
            return value;
        }

        private int ReadI32(int parOffset)
        {
            int value = 0;
            bool[] word = Read2Word(parOffset);
            for (int i = 0; i <= word.Length - 1; i++)
            {
                if (word[i])
                {
                    value += (1 << i);
                }
            }
            return value;
        }

        private int ReadU16(int parOffset)
        {
            int value = 0;
            bool[] word = ReadWord(parOffset);
            for (int i = 0; i <= word.Length - 1; i++)
            {
                if (word[i])
                {
                    value += (1 << i);
                }
            }
            return value;
        }

        private int ReadU32(int parOffset)
        {
            long value = 0;
            bool[] word = Read2Word(parOffset);
            for (int i = 0; i <= word.Length - 1; i++)
            {
                if (word[i])
                {
                    value += (1 << i);
                }
            }
            return (int)value;
        }

        private double ReadC4_3(int parOffset)
        {
            double value = 0;
            bool[] DWord = Read2Word(parOffset);
            for (int i = 0; i <= DWord.Length - 1; i++)
            {
                if (DWord[i])
                {
                    value += (1 << i);
                }
            }
            return (value / 1000);
        }

        private double ReadFormatValue(int parOffset, DataFormat Formato)
        {
            double value = 0;
            switch (Formato)
            {
                case DataFormat.U16:
                    value = ReadU16(parOffset + 16);
                    break;
                case DataFormat.U32:
                    value = ReadU32(parOffset);
                    break;
                case DataFormat.I16:
                    value = ReadI16(parOffset + 16);
                    break;
                case DataFormat.I32:
                    value = ReadI32(parOffset);
                    break;
                case DataFormat.C4_3:
                    value = ReadC4_3(parOffset);
                    break;
            }
            return value;
        }

        #endregion

        #region "Escrituras con formato"

        private void WriteI16(int parOffset, int value)
        {
            var Word = new bool[16];
            for (int i = 0; i <= Word.Length - 1; i++)
            {
                if ((value & (1 << i)) != 0)
                {
                    Word[i] = true;
                }
            }
            WriteWord(parOffset, Word);
        }

        private void WriteI32(int parOffset, int value)
        {
            var Dword = new bool[32];
            for (int i = 0; i <= Dword.Length - 1; i++)
            {
                if ((value & (1 << i)) != 0)
                {
                    Dword[i] = true;
                }
            }
            Write2Word(parOffset, Dword);
        }

        private void WriteU16(int parOffset, int value)
        {
            if (value < 0) value = -value;
            var Word = new bool[16];
            for (int i = 0; i <= Word.Length - 1; i++)
            {
                if ((value & (1 << i)) != 0)
                {
                    Word[i] = true;
                }
            }
            WriteWord(parOffset, Word);
        }

        private void WriteU32(int parOffset, int value)
        {
            if (value < 0) value = -value;
            var Dword = new bool[32];
            for (int i = 0; i <= Dword.Length - 1; i++)
            {
                if ((value & (1 << i)) != 0)
                {
                    Dword[i] = true;
                }
            }
            Write2Word(parOffset, Dword);
        }

        private void WriteC4_3(int parOffset, double value)
        {
            //C4_3
            var v = (int)Math.Round(1000 * value);
            var Dword = new bool[32];
            for (int i = 0; i <= Dword.Length - 1; i++)
            {
                if ((v & (1 << i)) != 0)
                {
                    Dword[i] = true;
                }
            }
            Write2Word(parOffset, Dword);
        }

        private void WriteFormatValue(int parOffset, DataFormat Formato, double value)
        {
            switch (Formato)
            {
                case DataFormat.U16:
                    WriteU16(parOffset + 16, (int)value);
                    break;
                case DataFormat.U32:
                    WriteU32(parOffset, (int)value);
                    break;
                case DataFormat.I16:
                    WriteI16(parOffset + 16, (int)value);
                    break;
                case DataFormat.I32:
                    WriteI32(parOffset + 16, (int)value);
                    break;
                case DataFormat.C4_3:
                    WriteC4_3(parOffset, value);
                    break;
            }
        }

        #endregion

        #endregion

        #region "PKW tratamiento específico"

        private PkwCommand _pkw;

        #region "R/W de Pkw"

        private bool[] PkwReadMetaData()
        {
            return Read2Word(_bitInputs);
        }

        private int PkwReadAk()
        {
            int V = 0;
            bool[] Data = PkwReadMetaData();
            for (int i = 0; i <= PkwCommand.PkwStructure.PkwAk.AkLength - 1; i++)
            {
                if (Data[PkwCommand.PkwStructure.PkwAk.Shift + 31 - i])
                {
                    V += (1 << (3 - i));
                }
            }
            return V;
        }

        private string PkwRead_PMUSubIndex()
        {
            int index = 0;
            int subIndex = 0;
            bool[] Data = PkwReadMetaData();
            for (int i = 0; i <= PkwCommand.PkwStructure.PkwC3Object.PNU_SubIndex.IndiceLength - 1; i++)
            {
                if (Data[31 - PkwCommand.PkwStructure.PkwC3Object.PNU_SubIndex.Shift - i])
                {
                    index += (1 << (PkwCommand.PkwStructure.PkwC3Object.PNU_SubIndex.IndiceLength - 1 - i));
                }
            }
            for (int i = 0; i <= PkwCommand.PkwStructure.PkwC3Object.PNU_SubIndex.SubIndiceLength - 1; i++)
            {
                if (
                    Data[
                        31 - PkwCommand.PkwStructure.PkwC3Object.PNU_SubIndex.Shift -
                        PkwCommand.PkwStructure.PkwC3Object.PNU_SubIndex.IndiceLength - i])
                {
                    subIndex += (1 << (PkwCommand.PkwStructure.PkwC3Object.PNU_SubIndex.SubIndiceLength - 1 - i));
                }
            }
            return (index + "." + subIndex);
        }

        private void PkwReadValue()
        {
            _pkw.ReadValue =
                (ReadFormatValue(_bitInputs + PkwCommand.PkwStructure.PkwC3Object.ObjectValue.Shift,
                                 _pkw.Message.C3Obj.PWE.Format));
        }

        public double PkwLastValeRead()
        {
            return _pkw.ReadValue;
        }

        private void PkwWriteValue()
        {
            WriteFormatValue(_bitOutputs + PkwCommand.PkwStructure.PkwC3Object.ObjectValue.Shift,
                             _pkw.Message.C3Obj.PWE.Format, _pkw.Message.C3Obj.PWE.Value);
        }

        private void PkwWriteMetaData()
        {
            var Data = new bool[32];
            _pkw.Message.Ak.AkBits.CopyTo(Data, PkwCommand.PkwStructure.PkwAk.Shift);
            _pkw.Message.C3Obj.Id.Index.CopyTo(Data, PkwCommand.PkwStructure.PkwC3Object.PNU_SubIndex.Shift);
            Array.Reverse(Data);
            Write2Word(_bitOutputs, Data);
        }

        private void PkwWriteMessage()
        {
            PkwWriteMetaData();
            PkwWriteValue();
        }

        #endregion

        #region "Control de Pkw"

        private int _stepsMove;
        private int _targetPosAbs;

        public int TargetPosition
        {
            get { return _targetPosAbs; }
        }

        public bool Inicio
        {
            get { return true; }
        }

        public void PkwNewCommand(PkwCommand.PkwAction Accion, PkwCommand.PkwStructure.PkwC3Object.PkwObjectType C3Obj,
                                  double Value)
        {
            _pkw = new PkwCommand(Accion, C3Obj, Value);
        }

        private bool PkwSendCommand()
        {
            _pkw.UpdateAk();
            PkwWriteMessage();
            return true;
        }

        private bool PkwReceiveCommad()
        {
            bool value = false;
            if (PkwReadAk() == _pkw.NewAk() & _pkw.Message.C3Obj.Id.Equivale(PkwRead_PMUSubIndex()))
            {
                PkwReadValue();
                value = true;
            }
            return value;
        }

        private bool PkwStartValidation()
        {
            PkwNewCommand(PkwCommand.PkwAction.ChangeValue,
                          PkwCommand.PkwStructure.PkwC3Object.PkwObjectType.ValidParameter_Global, 1);
            _pkw.UpdateAk();
            PkwWriteMessage();
            return true;
        }

        private bool PkwFinishValidation()
        {
            bool value = false;
            if (PkwReadAk() == _pkw.NewAk() & _pkw.Message.C3Obj.Id.Equivale(PkwRead_PMUSubIndex()))
            {
                PkwReadValue();
                value = true;
            }
            return value;
        }

        public bool PkwExecuteCommand(PkwCommand.PkwAction Accion,
                                      PkwCommand.PkwStructure.PkwC3Object.PkwObjectType C3Obj, double Value)
        {
            bool Valor = false;
            if ((_pkw == null))
            {
                PkwNewCommand(Accion, C3Obj, Value);
            }
            else
            {
                if (_pkw.State == PkwState.Finished)
                {
                    PkwNewCommand(Accion, C3Obj, Value);
                }
            }
            if (_pkw != null)
                if (_pkw != null)
                    switch (_pkw.State)
                    {
                        case PkwState.NotStarted:
                            if (PkwSendCommand())
                            {
                                _pkw.State = PkwState.Started;
                            }

                            break;
                        case PkwState.Started:
                            if (PkwReceiveCommad())
                            {
                                switch (_pkw.Action)
                                {
                                    case PkwCommand.PkwAction.RecuestValue:
                                    case PkwCommand.PkwAction.ChangeValue:
                                        _pkw.State = PkwState.Finish;
                                        break;
                                    case PkwCommand.PkwAction.ChangeValidateValue:
                                        _pkw.State = PkwState.NotValid;
                                        break;
                                }
                            }

                            break;
                        case PkwState.NotValid:
                            if (PkwStartValidation())
                            {
                                _pkw.State = PkwState.AskValid;
                            }

                            break;
                        case PkwState.AskValid:
                            if (PkwFinishValidation())
                            {
                                _pkw.State = PkwState.Finish;
                            }

                            break;
                        case PkwState.Finish:
                            Valor = true;
                            _pkw.State = PkwState.Finished;
                            break;
                    }
            return Valor;
        }

        public bool SetPkwAtribute(PkwCommand.PkwStructure.PkwC3Object.PkwObjectType C3Obj, double Value)
        {
            bool Valor = false;
            PkwCommand.PkwAction Accion = PkwCommand.PkwAction.ChangeValue;
            if ((_pkw == null))
            {
                PkwNewCommand(Accion, C3Obj, Value);
            }
            else
            {
                if (_pkw.State == PkwState.Finished)
                {
                    PkwNewCommand(Accion, C3Obj, Value);
                }
            }
            if (_pkw != null)
                if (_pkw != null)
                    switch (_pkw.State)
                    {
                        case PkwState.NotStarted:
                            if (PkwSendCommand())
                            {
                                _pkw.State = PkwState.Started;
                            }

                            break;
                        case PkwState.Started:
                            if (PkwReceiveCommad())
                            {
                                _pkw.State = PkwState.Finished;
                                Valor = true;
                            }

                            break;
                    }
            return Valor;
        }

        public bool GetPkwAtribute(PkwCommand.PkwStructure.PkwC3Object.PkwObjectType C3Obj)
        {
            bool Valor = false;
            PkwCommand.PkwAction Accion = PkwCommand.PkwAction.RecuestValue;
            if ((_pkw == null))
            {
                PkwNewCommand(Accion, C3Obj, 0);
            }
            else
            {
                if (_pkw.State == PkwState.Finished)
                {
                    PkwNewCommand(Accion, C3Obj, 0);
                }
            }
            if (_pkw != null)
                if (_pkw != null)
                    switch (_pkw.State)
                    {
                        case PkwState.NotStarted:
                            if (PkwSendCommand())
                            {
                                _pkw.State = PkwState.Started;
                            }

                            break;
                        case PkwState.Started:
                            if (PkwReceiveCommad())
                            {
                                _pkw.State = PkwState.Finished;
                                Valor = true;
                            }

                            break;
                    }
            return Valor;
        }

        public bool SetPosJerkAcel(double Value)
        {
            return SetPkwAtribute(PkwCommand.PkwStructure.PkwC3Object.PkwObjectType.POSITION_Jerk_accel, Value);
        }

        public bool GetPosJerkAcel()
        {
            return GetPkwAtribute(PkwCommand.PkwStructure.PkwC3Object.PkwObjectType.POSITION_Jerk_accel);
        }

        public bool SetPosJerkDecel(double Value)
        {
            return SetPkwAtribute(PkwCommand.PkwStructure.PkwC3Object.PkwObjectType.POSITION_Jerk_decel, Value);
        }

        public bool GetPosJerkDecel()
        {
            return GetPkwAtribute(PkwCommand.PkwStructure.PkwC3Object.PkwObjectType.POSITION_Jerk_decel);
        }

        public bool SetJogSpeed(double Value)
        {
            return SetPkwAtribute(PkwCommand.PkwStructure.PkwC3Object.PkwObjectType.JOG_speed, Value);
        }

        public bool GetJogSpeed()
        {
            return GetPkwAtribute(PkwCommand.PkwStructure.PkwC3Object.PkwObjectType.JOG_speed);
        }

        public bool SetJogJerk(double Value)
        {
            return SetPkwAtribute(PkwCommand.PkwStructure.PkwC3Object.PkwObjectType.JOG_jerk, Value);
        }

        public bool GetJogJerk()
        {
            return GetPkwAtribute(PkwCommand.PkwStructure.PkwC3Object.PkwObjectType.JOG_jerk);
        }

        public bool SetPosLimit(double Value)
        {
            return SetPkwAtribute(PkwCommand.PkwStructure.PkwC3Object.PkwObjectType.Limit_Position_Positive, Value);
        }

        public bool GetPosLimit()
        {
            return GetPkwAtribute(PkwCommand.PkwStructure.PkwC3Object.PkwObjectType.Limit_Position_Positive);
        }

        public bool SetNegLimit(double Value)
        {
            return SetPkwAtribute(PkwCommand.PkwStructure.PkwC3Object.PkwObjectType.Limit_Position_Negative, Value);
        }

        public bool GetNegLimit()
        {
            return GetPkwAtribute(PkwCommand.PkwStructure.PkwC3Object.PkwObjectType.Limit_Position_Negative);
        }

        public bool ValitateValues()
        {
            return SetPkwAtribute(PkwCommand.PkwStructure.PkwC3Object.PkwObjectType.ValidParameter_Global, 1);
        }

        public bool SaveValues()
        {
            return SetPkwAtribute(PkwCommand.PkwStructure.PkwC3Object.PkwObjectType.ObjectDir_Objekts, 1);
        }

        private bool StartTypeMov(int pos, int v, int a, DelTypeMov SetMov)
        {
            bool Valor = false;
            if ((_stepsMove == 0))
            {
                SetMov();
                InicioStartAbsMovement(pos, v, a);
                _stepsMove = 1;
            }
            else if (_stepsMove == 1)
            {
                TC1(true);
                _stepsMove = 2;
            }
            else if (_stepsMove == 2)
            {
                TC1(false);
                _stepsMove = 0;
                Valor = true;
            }
            return Valor;
        }

        private void SetMovAbs()
        {
            WriteControlBit(15, false);
            WriteControlBit(13, false);
            WriteControlBit(12, false);
        }

        private void SetMovRel()
        {
            WriteControlBit(15, true);
            WriteControlBit(13, false);
            WriteControlBit(12, false);
        }

        private void SetMovAdd()
        {
            WriteControlBit(15, false);
            WriteControlBit(13, true);
            WriteControlBit(12, false);
        }

        public bool StartMov(int pos, int v, int a)
        {
            _targetPosAbs = pos;
            return StartTypeMov(pos, v, a, SetMovAbs);
        }

        public bool StartRelMov(int pos, int v, int a)
        {
            return StartTypeMov(pos, v, a, SetMovRel);
        }

        public bool StartTypeAddMov(int pos, int v, int a)
        {
            return StartTypeMov(pos, v, a, SetMovAdd);
        }

        public bool MotorError()
        {
            return ReadStatusBit(3);
        }

        public bool EnOrigen(double posOrigen)
        {
            bool _EnOrigen;
            if ((MotorError() == false) & InPosition(posOrigen, 1.0))
            {
                _EnOrigen = true;
            }
            else
            {
                _EnOrigen = false;
            }
            return _EnOrigen;
        }

        #endregion

        #endregion

        #region Miembros de ISpecialDevice

        public bool InError()
        {
            return MotorError();
        }

        public void OnErrorAction()
        {
            Parada();
        }

        #endregion

        #region Miembros de IManualProvider

        public IEnumerable<Manual> GetManualsRepresentations()
        {
            var manual = new Manual(this);
            manual.Description = Name;
            return new[] {manual};
        }

        #endregion
    }
}