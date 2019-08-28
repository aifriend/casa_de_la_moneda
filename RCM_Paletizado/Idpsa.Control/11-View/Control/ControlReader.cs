using System;
using System.Windows.Forms;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.View
{
    public partial class ControlReader : UserControl, IRefrescable
    {
        private IReader _reader;
        
        public ControlReader(Manual manual)
        {
            InitializeComponent();
            Initialize (manual);              
        }

        public void Initialize(Manual manual)
        {
            _reader = (IReader)manual.RepresentedInstance;
            lbComentario.Text = manual.Descripcion;
            var dataNotofier= manual.RepresentedInstance as DataNotifier<string>;
            if (dataNotofier!=null)
                dataNotofier.Subscribe(DataNotifieEventHandler);
        }

        private void DataNotifieEventHandler(object sender, DataEventArgs<string> e)
        {
            lbLectura.Text = e.Data;

        }

        public void RefreshView()
        {
            if (_reader.LastReadedCode != null)
            {
                lbLectura.Text = _reader.LastReadedCode;
            }
            if (_reader.Connected())
            {
                lbState.BackColor = System.Drawing.Color.LightGreen;
                lbState.Text = "Conectado";
            }
            else
            {
                lbState.BackColor = System.Drawing.Color.Salmon;
                lbState.Text = "Desconectado";
            }           
        }


        private void btRead_Click(object sender, EventArgs e)
        {            
                string code;
                if (_reader.ReadCode(out code))
                {
                    lbLectura.Text = code;
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
    }
}
