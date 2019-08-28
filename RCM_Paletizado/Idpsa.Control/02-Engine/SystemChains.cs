using System;
using System.Linq;
using Idpsa.Control.Sequence;
using Idpsa.Control.Subsystem;

namespace Idpsa.Control.Engine
{
    public class SystemChains : IOriginDefiner
    {
        private readonly SystemControl _control;
        //private readonly SystemControl2 _control2;//MDG.2012-07-23

        public ChainCollection AutomaticChains { get; private set; }
        public ChainCollection AutomaticChains2 { get; private set; }
        public ChainCollection BackToOgiginChains { get; private set; }
        public ChainCollection FreeChains { get; private set; }
        public Action Manager { get; private set; }

        public SystemChains(SystemControl control)//, SystemControl2 control2)
        {
            _control = control;
            //_control2 = control2;
            AutomaticChains = new ChainCollection();
            AutomaticChains2 = new ChainCollection();
            BackToOgiginChains = new ChainCollection();
            FreeChains = new ChainCollection();
        }

        internal void SetSubsystems(SubsystemsAnalizer analizer)
        {
            ConstructChains(analizer);
            ConstructManager();
        }
       
        #region IOriginDefiner Members

        public bool InOrigin()
        {
            if (AutomaticChains.AllValues
                .Any(cad => !cad.InOrigin(false, _control.OperationMode, _control.ActiveMode)))
            {
                return false;
            }

            return BackToOgiginChains.AllValues
                .All(cad => cad.InOrigin(true, _control.OperationMode, _control.ActiveMode));
        }

        public bool InOrigin2()
        {
            return AutomaticChains2.AllValues
                .All(cad => cad.InOrigin(true, _control.OperationMode, _control.ActiveMode));
        }

        #endregion

        private void ConstructChains(SubsystemsAnalizer analizer)
        {
            AutomaticChains.Clear();
            AutomaticChains2.Clear();
            BackToOgiginChains.Clear();
            FreeChains.Clear();
            AutomaticChains.AddRange(analizer.GetAutoChains());
            AutomaticChains2.AddRange(analizer.GetAutoChains2());
            BackToOgiginChains.AddRange(analizer.GetBackToOriginChains());
            FreeChains.AddRange(analizer.GetFreeChains());
        }
        private void ConstructManager()
        {
            Manager = null;
            Manager += StopManager;
            Manager += StopManager2;//MDG.2012-07-23

            if (AutomaticChains.Count > 0)
                Manager += AutomaticManager;
            if (AutomaticChains2.Count > 0)
                Manager += AutomaticManager2;//MDG.2012-07-23
            if (BackToOgiginChains.Count > 0)
                Manager += BackToOriginManager;
            if (FreeChains.Count > 0)
                Manager += FreeManager;
        }
        private void AutomaticManager()
        {
            bool init = _control.RiCondition() || _control.OperationMode != Mode.Automatic ||
                        !_control.ActiveMode;
            AutomaticChains.ForEach(chain =>
            {
                chain.Init = init;
                chain.Run();
            });
        }
        private void AutomaticManager2()
        {
            bool init = _control.RiCondition2() ||
                        _control.OperationMode2 != Mode.Automatic
                        || !_control.ActiveMode2;
            AutomaticChains2.ForEach(chain =>
            {
                chain.Init = init;
                chain.Run();
            });
        }
        private void BackToOriginManager()
        {
            bool init = _control.RiCondition() || _control.OperationMode != Mode.BackToOrigin ||
                        !_control.ActiveMode;
            BackToOgiginChains.ForEach(chain =>
                                    {
                                        chain.Init = init;
                                        chain.Run();
                                    });
        }
        private void FreeManager()//MCR 2017
        {
            bool init = _control.ActiveMode;//!_control.SystemOK;
            FreeChains.ForEach(chain =>
                                   {
                                       chain.Init = init;
                                       chain.Run();
                                   });
        }

        private void StopManager()
        {
            bool stoped = false;
            if (_control.OperationMode == Mode.Automatic)
            {
                stoped = AutomaticChains.StopManager(_control.ActiveMode, _control.StopRequest, _control.ModeStatus);
            }
            else if (_control.OperationMode == Mode.BackToOrigin)
            {
                stoped = BackToOgiginChains.StopManager(_control.ActiveMode, _control.StopRequest, _control.ModeStatus);
            }
            if (stoped)
            {
                if (_control.StopRequest == ControlStopRequest.StopAndDeactive)
                {
                    _control.ModeStatus = ControlModeStatus.Deactivated;
                }
                else if (_control.StopRequest == ControlStopRequest.Stop)
                {
                    _control.ModeStatus = ControlModeStatus.Stoped;
                }
            }
        }

        private void StopManager2()
        {
            bool stoped = false;
            if (_control.OperationMode2 == Mode.Automatic)
            {
                stoped = AutomaticChains2.StopManager(_control.ActiveMode2, _control.StopRequest2, _control.ModeStatus2);
            }
            //else if (_control.OperationMode2 == Mode.BackToOrigin)
            //{
            //    stoped = BackToOgiginChains2.StopManager(_control.ActiveMode2, _control.StopRequest2, _control.ModeStatus2);
            //}
            if (stoped)
            {
                if (_control.StopRequest2 == ControlStopRequest.StopAndDeactive)
                {
                    _control.ModeStatus2 = ControlModeStatus.Deactivated;
                }
                else if (_control.StopRequest2 == ControlStopRequest.Stop)
                {
                    _control.ModeStatus2 = ControlModeStatus.Stoped;
                }
            }
        }
    }
}