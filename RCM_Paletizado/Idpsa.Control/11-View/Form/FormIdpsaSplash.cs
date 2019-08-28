using System;
using System.Threading;
using System.Windows.Forms;

namespace Idpsa.Control
{
    public partial class FormIdpsaSplash : Form
    {
        public FormIdpsaSplash()
        {
            InitializeComponent();
            pbLoad.Style = ProgressBarStyle.Continuous;
            pbLoad.Minimum = 0;
            pbLoad.Maximum = 270;            
        }

       
        const int NDOTS=5;
        private volatile bool _timerSplashEnabled;

        private void Progress(int increment)
        {
            pbLoad.Value += (increment + pbLoad.Value > pbLoad.Maximum) ? pbLoad.Maximum - pbLoad.Value : increment;
            pictureBox1.Invalidate();
        }

        Thread _trdSplash;
        System.Timers.Timer _timerSplash;

        public void Splash()
        {
            _timerSplashEnabled = true;
            _trdSplash = new Thread(DoSplash);
            _trdSplash.Priority = ThreadPriority.Lowest;
            _trdSplash.IsBackground = true;
            _trdSplash.Start();
        }

        public new void Close()
        {            
            _timerSplashEnabled = false;
            Thread.Sleep(200);  
            _timerSplash.Stop();
            _trdSplash.Abort();
        }

        private void DoSplash()
        {            
            _timerSplash = new System.Timers.Timer { Interval = 500 };
            Random r = new Random();
            _timerSplash.Elapsed +=
                (s, e) =>
                {
                    if(_timerSplashEnabled) 
                        this.Invoke(new System.Action<int>(Progress),
                            new object[] { r.Next(25) });
                };

            _timerSplash.Start();
            ShowDialog();
        }



    }

    
}
