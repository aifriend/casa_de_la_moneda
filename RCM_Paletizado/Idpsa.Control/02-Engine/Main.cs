using System;
using System.Diagnostics;
using Microsoft.Win32;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using Idpsa.Control;
using Idpsa.Control.View;
using Idpsa.Control.User;

namespace Idpsa.Control.Engine
{
    public class Main<TypeSystem,TypeGUI> 
        where TypeSystem: IDPSASystem, new()
        where TypeGUI: FormMain, new()
    {
        private TypeGUI _mainForm;
        private static readonly Main<TypeSystem, TypeGUI> _instance = new Main<TypeSystem,TypeGUI>();

        public static Main<TypeSystem, TypeGUI> Instance { get { return _instance; } }

        private Main(){}

        public void Run()
        {            
            string aplicationToken = typeof(TypeSystem).Assembly.FullName;            
            Mutex MutexIdp = new Mutex(false, aplicationToken);

            if (!MutexIdp.WaitOne(TimeSpan.FromSeconds(5), false))
            {
                return;
            }
            try
            {                
                RunCore();
            }
            finally
            {
                if (_mainForm != null)
                    _mainForm.Dispose();

                if (ControlLoop<TypeSystem>.Instance != null)
                    ControlLoop<TypeSystem>.Instance.Dispose();

                MutexIdp.ReleaseMutex();
            }
        }

        [System.STAThread()]
        private void RunCore()
        {
            StetUp();
                       
            _mainForm = new TypeGUI();

            var delegateTasks = _mainForm as IDelegateTasksOwner;
            if (delegateTasks != null)
                ControlLoop<TypeSystem>.Instance.AddDelegateTasks(delegateTasks);

            var viewTasks = _mainForm as IViewTasksOwner;
            if (viewTasks != null)
                ControlLoop<TypeSystem>.Instance.AddViewTasks(viewTasks);

            _mainForm.AfterLoadEvent += delegate { ControlLoop<TypeSystem>.Instance.Start(); };

            Application.Run(_mainForm);
        }

        private void StetUp()
        {
            Application.EnableVisualStyles();
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;//.High; MCR.2016. mod prioridad.
            UserAccess.Instance.CurrentUser = UserType.Administrador;
            SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvents_SessionEnding);
        }

        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            if (e.Reason == SessionEndReasons.SystemShutdown)
            {
                _mainForm.Close();
            }
        }

    }
}