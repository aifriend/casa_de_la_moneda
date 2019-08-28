using System.Drawing;
using System.Windows.Forms;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.View
{
    public partial class ControlDataDysplayer<T> : UserControl
    {

        private const int DefaultTitleFontSize = 24;
        private const int DefaultDataFontSize = 52;

        private ControlDataDysplayer()
        {
            InitializeComponent();
        }

        public ControlDataDysplayer(Manual manual)
        {
            InitializeComponent();
            Initialize(manual); 
        }
                     
        private void Initialize(Manual manual)
        {
            var dataNotifierProvider = (IDataNotifierProvider<T>)manual.RepresentedInstance;

            var dataNotifier = dataNotifierProvider.GetDataNotifier();

            lbCom.Text = manual.Description;
            dataNotifier.Subscribe(NewDataHandler);

            var dataFormat = dataNotifier as IManualFormatProvider;

            int titleFontSize = DefaultTitleFontSize;
            int dataFontSize = DefaultDataFontSize;
            if (dataFormat == null)
            {
                if (!dataFormat.HasFormatDirectives)
                {
                    titleFontSize = dataFormat.SenderNameFontSize;
                    dataFontSize = dataFormat.DataFontSize;
                }
            }
                      
            SetTitleFontSize(titleFontSize);
            SetDataFontSize(dataFontSize);             
        }

        private void NewDataHandler(object sender, DataEventArgs<T> e)
        {
             lbStringDysplay.Text = e.Data.ToString();
        }

        private void SetDataFontSize(int fontSize)
        {
            lbStringDysplay.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Bold, GraphicsUnit.Point, ((0)));            
        }

        private void SetTitleFontSize(int fontSize)
        {
            lbCom.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Bold, GraphicsUnit.Point, ((0)));           
        }

    }


   
}
