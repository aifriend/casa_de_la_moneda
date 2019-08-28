using System.Collections.Generic;
using Idpsa.Control.Component;

namespace Idpsa.Control.Subsystem
{
    public interface ISpecialDeviceOwner
    {
        IEnumerable<ISpecialDevice> GetSpecialDevices();
    }
}