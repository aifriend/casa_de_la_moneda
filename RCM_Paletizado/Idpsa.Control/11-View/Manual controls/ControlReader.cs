using System;
using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.View
{
    public partial class ControlReader: UserControl, IRefrescable
    {
        private IReader _reader;

        public ControlReader(Manual manual)
        {
            InitializeComponent();
            Initialize (manual);              
        }

        private void Initialize(Manual manual)
        {
            _reader = (IReader)manual.RepresentedInstance;
            lbComment.Text = manual.Description;
            var dataNotofier= manual.RepresentedInstance as DataNotifier<string>;
            if (dataNotofier!=null)
                dataNotofier.Subscribe(DataNotifieEventHandler);
            checkBox1.Checked = !_reader.Habilitado;//MCR. 2015.03.13
        }

        private void DataNotifieEventHandler(object sender, DataEventArgs<string> e)
        {
            lbRead.Text = e.Data;

        }

        public void RefreshView()
        {
            if (_reader.LastReadedCode != null)
            {
                lbRead.Text = _reader.LastReadedCode;
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


        private void btRead_Click(object sender, EventArgs e)
        {            
                string code;
                if (_reader.ReadCode(out code))
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _reader.Habilitado = !checkBox1.Checked; //MCR. 2015.03.13
        }
    }
}
