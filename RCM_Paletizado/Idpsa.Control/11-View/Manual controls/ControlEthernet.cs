using System;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.View
{
    public partial class ControlEthernet : UserControl, IRefrescable
    {
        private ISocket _reader;

        public ControlEthernet(Manual manual)
        {
            InitializeComponent();
            Initialize (manual);              
        }

        private void Initialize(Manual manual)
        {
            _reader = (ISocket)manual.RepresentedInstance;
            lbComment.Text = manual.Description;
            var dataNotofier= manual.RepresentedInstance as DataNotifier<string>;
            if (dataNotofier!=null)
                dataNotofier.Subscribe(DataNotifieEventHandler);
        }

        private void DataNotifieEventHandler(object sender, DataEventArgs<string> e)
        {
            lbRead.Text = e.Data;

        }

        public void RefreshView()
        {
            if (_reader.LastMsgRead != null)
            {
                lbRead.Text = _reader.LastMsgRead;
            }
            if (_reader.Connected())
            {
                lbState.BackColor = Color.LightGreen;
                lbState.Text = @"Conectado";
            }
            else
            {
                lbState.BackColor = Color.Salmon;
                lbState.Text = @"Desconectado";
            }           
        }


        private void btAction_Click(object sender, EventArgs e)
        {            
                string code;
                if (_reader.ReadMsg(out code))
                {
                    lbRead.Text = code;
                }            
        }
               

        private void button1_Click(object sender, EventArgs e)
        {
            _reader.Reset();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _reader.Connect();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _reader.Disconnect();
        }

        private void btReceive_Click(object sender, EventArgs e)
        {
            string code;
            if (_reader.ReadMsg(out code))
            {
                lbRead.Text = code;
            }
        }
    }
}
