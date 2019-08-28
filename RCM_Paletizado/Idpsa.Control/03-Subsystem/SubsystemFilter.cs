using System;

namespace Idpsa.Control.Subsystem
{
    [Flags]
    public enum SubsystemFilter
    {
        None = 0,
        BackToOriginRun = 1,
        AutoRun = 2,
        FreeRun = 4,
        Diagnosis = 8,
        Manager = 16,
        Origin = 32,
        Ri = 64,
        SpecialDevice = 128,
        Manuals = 256,
        AutoRun2 = 512,//MDG.2012-07-23
        Ri2 = 1024,//MDG.2012-07-25
        Run = BackToOriginRun | AutoRun | FreeRun | AutoRun2,//MDG.2012-07-23
        All = Run | Diagnosis | Manager | Origin | Ri | SpecialDevice | Manuals
    }
}