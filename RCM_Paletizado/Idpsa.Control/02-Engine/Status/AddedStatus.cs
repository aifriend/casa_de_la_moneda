using System;

namespace Idpsa.Control.Engine.Status
{
    public abstract class AddedStatus
    {
        private StatusValue _status;
        public EventHandler<StatusEventArgs> StatusChanged;
        private StatusValue _status2;
        public EventHandler<StatusEventArgs> StatusChanged2;
        private StatusValue _status3;
        public EventHandler<StatusEventArgs> StatusChanged3;

        protected AddedStatus(string name)
        {
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name can't be null or empty"); 
            Name = name;
        }

        public string Name { get; private set; }

        public StatusValue Status
        {
            get { return _status; }
            private set
            {
                if (_status != value)
                {
                    _status = value;
                    OnStatusChanged();
                }
            }
        }

        public StatusValue Status2
        {
            get { return _status2; }
            private set
            {
                if (_status2 != value)
                {
                    _status2 = value;
                    OnStatusChanged2();
                }
            }
        }

        public StatusValue Status3
        {
            get { return _status3; }
            private set
            {
                if (_status3 != value)
                {
                    _status3 = value;
                    OnStatusChanged3();
                }
            }
        }

        public void StatusControl(SystemControl control)
        {
            if (control == null) throw new ArgumentNullException("control");
            Status = StatusControlCore(control);
            Status2 = StatusControlCore2(control);
            Status3 = StatusControlCore3(control);
        }

        public void NotifyStatus()
        {
            OnStatusChanged();
        }
        public void NotifyStatus2()
        {
            OnStatusChanged2();
        }
        public void NotifyStatus3()
        {
            OnStatusChanged3();
        }

        public virtual string Message()
        {
            return String.Format("Status {0} :  {1}", Name, Status);
        }

        public virtual string Message2()
        {
            return String.Format("Status2 {0} :  {1}", Name, Status2);
        }

        public virtual string Message3()
        {
            return String.Format("Status3 {0} :  {1}", Name, Status3);
        }

        protected abstract StatusValue StatusControlCore(SystemControl control);
        protected abstract StatusValue StatusControlCore2(SystemControl control);
        protected abstract StatusValue StatusControlCore3(SystemControl control);

        protected virtual void OnStatusChanged()
        {
            EventHandler<StatusEventArgs> statusChanged = StatusChanged;
            if (statusChanged != null)
            {
                statusChanged(this, StatusEventArgs.Empty);
            }
        }
        protected virtual void OnStatusChanged2()
        {
            EventHandler<StatusEventArgs> statusChanged2 = StatusChanged2;
            if (statusChanged2 != null)
            {
                statusChanged2(this, StatusEventArgs.Empty);
            }
        }
        protected virtual void OnStatusChanged3()
        {
            EventHandler<StatusEventArgs> statusChanged3 = StatusChanged3;
            if (statusChanged3 != null)
            {
                statusChanged3(this, StatusEventArgs.Empty);
            }
        }
    }
}