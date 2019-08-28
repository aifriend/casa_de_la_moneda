using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace Idpsa.Paletizado
{
    public partial class PaletizersLine1View : UserControl
    {
        private readonly Timer _timer;
        private MosaicView[] _views;

        public event ForcedAddBox AddBox;
        public event ForcedQuitBox QuitBox;

        public delegate void ForcedAddBox(int paletizer);
        public delegate void ForcedQuitBox(int paletizer);

        public PaletizersLine1View()
        {
            InitializeComponent();
            tabControl1.Selected += SelectedEventHandler;
            _timer = new Timer {AutoReset = false, Interval = 50};
            tabControl1.SelectedIndex = 0;
            _timer.Elapsed += delegate { _views[tabControl1.SelectedIndex].Paint(); };
            TieChilds();
        }

        private void TieChilds()
        {
            _view1.AddBox += AddBoxHandler;
            _view2.AddBox += AddBoxHandler;
            _view3.AddBox += AddBoxHandler;
            _view1.QuitBox += QuitBoxHandler;
            _view2.QuitBox += QuitBoxHandler;
            _view3.QuitBox += QuitBoxHandler;
        }

        public void SubscribeLine1(Line1 line)
        {
            _view1.SubscriveToEnvents(line.Mesas.Mesa1.Paletizer);
            _view2.SubscriveToEnvents(line.Mesas.Mesa2.Paletizer);
            _view3.SubscriveToEnvents(line.ZonaPaletizadoFinal.Paletizer);
            _views = new[] {_view1, _view2, _view3};
        }

        public void ManualChange(bool b)
        {
            _view1.ShowManualButton(b);
            _view2.ShowManualButton(b);
            _view3.ShowManualButton(b);
        }

        private void SelectedEventHandler(object sender, TabControlEventArgs e)
        {
            Refresh();
        }

        public void Refresh()
        {
            _timer.Start();
        }

        private void AddBoxHandler(object sender)
        {
            object view = sender;
            if (view.Equals(_view1))
            {
                AddBox(1);
            }
            else if (view.Equals(_view2))
            {
                AddBox(2);
            }
            else if (view.Equals(_view3))
            {
                AddBox(3);
            }
        }

        private void QuitBoxHandler(object sender)
        {
            object view = sender;
            if (view.Equals(_view1))
            {
                QuitBox(1);
            }
            else if (view.Equals(_view2))
            {
                QuitBox(2);
            }
            else if (view.Equals(_view3))
            {
                QuitBox(3);
            }
        }
    }
}