using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Component;
using Idpsa.Control.Subsystem;
using Idpsa.Control.Tool;

namespace Idpsa.Control.Engine
{
    public class SpecialDevicesManager : IEnumerable<ISpecialDevice>
    {
        private readonly SystemControl _control;
        private readonly List<ISpecialDevice> _specialDevices;
        private readonly List<ISpecialDevice> _allSpecialDevices;

        public SpecialDevicesManager(SystemControl control, IEnumerable<ISpecialDevice> allSpecialDevices)
        {
            _control = control;
            _allSpecialDevices = allSpecialDevices.ToList();
            _specialDevices = new List<ISpecialDevice>();
        }

        internal void SetSpecialDevices(ISpecialDeviceOwner specialDeviceOwner)
        {           
            _specialDevices.Clear();
            _specialDevices.AddRange(specialDeviceOwner.GetSpecialDevices());           
        }

        #region IEnumerable<ISpecialDevice> Members
        
        public IEnumerator<ISpecialDevice> GetEnumerator()
        {
            return _allSpecialDevices.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public void Manager()
        {
            if (_control.InActiveMode(Mode.Automatic))               
            {
                foreach (ISpecialDevice specialDevice in _specialDevices)
                {
                    if (specialDevice.InError())
                    {
                        specialDevice.OnErrorAction();
                    }
                }
            }
        }

        public IEnumerable<T> GetSpecialDevices<T>() where T : ISpecialDevice
        {
            List<T> values = _allSpecialDevices.OfType<T>().ToList();
            List<ISpecialDeviceOwner> owners = _allSpecialDevices.OfType<ISpecialDeviceOwner>()
                .Except(values.OfType<ISpecialDeviceOwner>()).ToList();

            var auxValues = new List<ISpecialDeviceOwner>();
            while (owners.Count > 0)
            {
                owners.ForEach(owner => 
                       owner.GetSpecialDevices()
                            .ForEach(d =>
                                         {
                                             if (d is T)
                                                 values.Add((T)d);
                                             else if (d is ISpecialDeviceOwner)
                                                 auxValues.Add((ISpecialDeviceOwner)d);
                                         }
                            )
                      );

                owners = auxValues;
                auxValues.Clear();
            }
            return values;
        }
    }
}