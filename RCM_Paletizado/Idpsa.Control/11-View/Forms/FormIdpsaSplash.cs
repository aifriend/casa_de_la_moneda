using System;
using System.Threading;
using System.Windows.Forms;
using Timer=System.Timers.Timer;

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
        Timer _timerSplash;

        public void Splash()
        {
            return; //TODO: Deshabilitado
            _timerSplashEnabled = true;
            _trdSplash = new Thread(DoSplash) {Priority = ThreadPriority.Lowest, IsBackground = true};
            _trdSplash.Start();
        }

        public new void Close()
        {
            return;
            _timerSplashEnabled = false;
            Thread.Sleep(200);  
            _timerSplash.Stop();
            _trdSplash.Abort();
        }

        private void DoSplash()
        {            
            _timerSplash = new Timer { Interval = 5300 };
            var r = new Random();
            _timerSplash.Elapsed +=
                (s, e) =>
                {
                    if(_timerSplashEnabled) 
                        Invoke(new Action<int>(Progress),
                            new object[] { r.Next(25) });
                };

            _timerSplash.Start();
            ShowDialog();
        }



    }

    
}
