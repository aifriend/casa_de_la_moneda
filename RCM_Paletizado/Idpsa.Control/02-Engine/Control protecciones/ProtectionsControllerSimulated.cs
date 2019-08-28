using System;

namespace Idpsa.Control.Engine
{
    [Serializable]
    public class ProtectionsControllerSimulated: IProtectionsController
    {
        public void ProtectionsControl(SystemControl control) { }
        public bool ProtectionsOK { get { return true; } }
        public bool ProtectionsCanceled { get { return false; } }
        public bool ProtectionsOK2 { get { return true; } }
        public bool ProtectionsCanceled2 { get { return false; } }       
    }
}