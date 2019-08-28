using System;
using System.Reflection;
using System.Collections.Generic;
using System.Threading;
using Idpsa.Control.Timing;
using Idpsa.Control.View;

namespace Idpsa.Control.Engine
{   
    public sealed class ControlLoop<TypeSystem>: IDisposable 
        where TypeSystem :IDPSASystem, new()
    {
        #region Thread control

        private readonly object _lookDelegateTasks = new Object();
        private readonly object _lookViewTasks = new Object();

        private readonly List<Action> _delegateTasks;
        private readonly List<Action> _viewTasks;

        private readonly TimerCounter _myCounterClock;
        private readonly Thread _trdAux;
        private readonly Thread _trdIdp;        

        private static readonly ControlLoop<TypeSystem> _instance = new ControlLoop<TypeSystem>();

        public static ControlLoop<TypeSystem> Instance { get { return _instance; } }
        public static bool CLK
        {
            get { return CLOCK.Instance.CLK; }
            private set { CLOCK.Instance.CLK = value; }
        }
        

        private ControlLoop()
        {
            Sys = new TypeSystem();
            _delegateTasks = new List<Action>();
            _viewTasks = new List<Action>();
            _myCounterClock = new TimerCounter();
            _trdIdp = new Thread(ThreadTask) {Priority = ThreadPriority.Highest, IsBackground = true};
            _trdAux = new Thread(ThreadTaskAux) {Priority = ThreadPriority.Lowest, IsBackground = true};
            Work = false;   
        }

        public TypeSystem Sys { get; private set; }
        public bool Work { get; set; }
        
        internal void Start()
        {
            Work = true;
            _trdIdp.Start();
            _trdAux.Start();
        }

        private void ThreadTask()
        {
            Sys.Bus.WakeUpDevice();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            Sys.Control.NotifyInitialState();

            while (Work)
            {
                _myCounterClock.Start();
                MainLoop();
                CLK = !CLK;
                Sys.Control.MaxCycleTime = _myCounterClock.Value;    
                Thread.Sleep(5);                            
            }
        }

        private void ThreadTaskAux()
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;

            while (Work)
            {
                Sys.Signals.SecurityDiagnosisManager();
                ViewTaskManager(); 
                Thread.Sleep(200);
            }
        }

        public void AddDelegateTasks(IDelegateTasksOwner iDelegateTaskOwner)
        {
            if (iDelegateTaskOwner == null)
                throw new ArgumentNullException("iDelegateTaskOwner"); 
            
            lock (_lookDelegateTasks)
            {
                _delegateTasks.AddRange(iDelegateTaskOwner.GetDelegateTasks());
            }
        }

        public void AddViewTasks(IViewTasksOwner iViewTaskOwner)
        {
            if (iViewTaskOwner == null)
                throw new ArgumentNullException("iViewTaskOwner");

            lock (_lookViewTasks)
            {
                _viewTasks.AddRange(iViewTaskOwner.GetViewTasks());  
            } 
        }     

        #endregion

        # region MainLoop

        private void MainLoop()
        {
            Sys.Bus.Run();
            Sys.Signals.SignalsControl();
            Sys.Signals.AddedStatusControl();
            Sys.Control.CheckSetOrigin();
            Sys.Control.OperationModeManager();
            Sys.SpecialDevices.Manager();
            Sys.Subsystems.Manager();
            Sys.Chains.Manager();
            DelegateTaskManager();
            Sys.Ri();
        }

        private void DelegateTaskManager()
        {
            lock (_lookDelegateTasks)
            {
                foreach (Action task in _delegateTasks)
                    task();
            }
        }

        private void ViewTaskManager()
        {
            lock (_lookViewTasks)
            {
                foreach (Action task in _viewTasks)
                    task();
            }
        }
        #endregion

        #region Miembros de IDisposable

        public void Dispose()
        {
            Sys.Dispose();
        }

        #endregion
    }

    internal class CLOCK
    {
        private static CLOCK _instance = new CLOCK();
        public static CLOCK Instance { get { return _instance; } }
        public bool CLK{get;internal set;}
        private CLOCK() { }
    }

}