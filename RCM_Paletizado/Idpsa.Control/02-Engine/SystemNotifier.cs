using System;
using System.Collections.Generic; 
using Idpsa.Control.Engine;

public class SystemNotifier
{
    private Dictionary<SystemControl.IdNotification, Action<SystemControl,object>> _events;

    public SystemNotifier(SystemControl control)
    {
        control.NewNotification += SystemNoficicationsHandler;
    }

     //BootSystem,
     //       Origin,
     //       ConnectionCommand,
     //       GlobalAddedStatusOk,            
     //       Diagnosis,
     //       ProtectionsOK,
     //       ProtectionsCanceled,
     //       OperationMode,
     //       OperationModeSelected,
     //       ModeStatus,
     //       ActiveSubsystems,
     //       MaxCycleTime     
          

    private void SystemNoficicationsHandler(object sender, SystemControl.EventNotificationArgs e)
    {
        if (_events.ContainsKey(e.IdNotification))
        {
            var tempEvent = _events[e.IdNotification];
            if (tempEvent != null)
                tempEvent((SystemControl)sender, e);
        }
    }

}