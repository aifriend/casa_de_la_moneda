using System;
using System.Windows.Forms;
using System.Threading;

namespace Idpsa.Control.View
{
    public class MainForm : Form
    {       
        internal event EventHandler<EventArgs> BeforeLoadEvent;
        internal event EventHandler<EventArgs> AfterLoadEvent;

        protected MainForm() { }

        protected virtual void OnBeforeLoadEvent()
        {
            var tempEvent = BeforeLoadEvent;
            if (tempEvent != null)
            {
                tempEvent(this, EventArgs.Empty);
            }
            BeforeLoadEvent = null;
        }

        protected virtual void OnAfterLoadEvent()
        {
            var tempEvent = AfterLoadEvent;

            if (tempEvent != null)
            {
                tempEvent(this, EventArgs.Empty);            
            }
            AfterLoadEvent = null;
        }

        protected override void OnLoad(EventArgs e)
        {            
            OnBeforeLoadEvent();
            FormIdpsaSplash formSplash = StartSplashScreen();
            base.OnLoad(e);                        
            OnAfterLoadEvent();
            EndSplashScreen(formSplash);
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal; 
            Show();            
        }

        private FormIdpsaSplash StartSplashScreen()
        {
            this.SuspendLayout();
            Hide();
            FormIdpsaSplash formSplash = new FormIdpsaSplash();
            formSplash.Splash();
            return formSplash;
        }

        private void EndSplashScreen(FormIdpsaSplash formSplash)
        {
            Thread.Sleep(3000);
            formSplash.Close();
            this.ResumeLayout();
        }


    }
}