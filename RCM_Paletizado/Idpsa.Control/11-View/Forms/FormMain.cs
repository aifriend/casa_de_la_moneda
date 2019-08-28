using System;
using System.Threading;
using System.Windows.Forms;

namespace Idpsa.Control.View
{
    public class FormMain : Form
    {       
        internal event EventHandler<EventArgs> BeforeLoadEvent;
        internal event EventHandler<EventArgs> AfterLoadEvent;

        protected FormMain() { }

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
            SuspendLayout();
            Hide();
            var formSplash = new FormIdpsaSplash();
            formSplash.Splash();
            return formSplash;
        }

        private void EndSplashScreen(FormIdpsaSplash formSplash)
        {
            Thread.Sleep(2000);
            formSplash.Close();
            ResumeLayout();
        }


    }
}