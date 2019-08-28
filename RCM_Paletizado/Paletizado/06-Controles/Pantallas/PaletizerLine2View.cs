using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace Idpsa.Paletizado
{
    public partial class PaletizerLine2View : UserControl
    {
        private readonly Timer _timer;
        private Timer _timer2;


        private int a; //MDG.2010-11-24.Contador para pruebas

        //IDPSASystemPaletizado _sys;//MDG.2010-11-24.Para tener acceso al Salvado de catalogos
        public event ForcedAddBox AddBox;
        public event ForcedQuitBox QuitBox;

        public delegate void ForcedAddBox(int paletizer);
        public delegate void ForcedQuitBox(int paletizer);

        public PaletizerLine2View()
        {
            InitializeComponent();
            tabControl1.Selected += SelectedEventHandler;
            _timer = new Timer {AutoReset = false, Interval = 50};
            tabControl1.SelectedIndex = 0;
            TieChilds();
            _timer.Elapsed += delegate
                                  {
                                      _mosaicView.Paint();

                                      //try
                                      //{
                                      //    //MDG.2010-11-24.Guardado catálogos al depositar cada caja
                                      //    _sys.Production.SaveCatalogs();

                                      //}
                                      //catch { }
                                  };
        }

        private void TieChilds()
        {
            _mosaicView.AddBox += AddBoxHandler;
            _mosaicView.QuitBox += QuitBoxHandler;
        }

        private void AddGroup(ListView listView, GrupoPasaportes grupo)
        {
            var item = new ListViewItem(grupo.Id) {Name = grupo.Id};
            listView.Items.Add(item);
        }

        private void QuitGroup(ListView listView, GrupoPasaportes grupo)
        {
            listView.Items.RemoveByKey(grupo.Id);
        }

        public void SubscribeLine2(Line2 line)
        {
            _mosaicView.SubscriveToEnvents(line.Paletizer);
        }


        private void SelectedEventHandler(object sender, TabControlEventArgs e)
        {
            Refresh();
        }

        public void Refresh()
        {
            if (tabControl1.SelectedIndex == 0)
            {
                _timer.Start();
            }
        }
        public void ManualChange(bool b)
        {
            _mosaicView.ShowManualButton(b);            
        }

        private void AddBoxHandler(object sender)
        {
            AddBox(4);
        }

        private void QuitBoxHandler(object sender)
        {
            QuitBox(4);
        }

    }
}