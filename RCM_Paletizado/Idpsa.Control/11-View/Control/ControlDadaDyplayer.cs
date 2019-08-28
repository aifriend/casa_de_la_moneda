using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
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

          //string[] str = manual.Descripcion.Substring("StringDysplay-".Length).Trim().Split(new char[] { '|' });
          //  lbCom.Text = str[0].Trim();
          //  _stringDataProvider = stringDataProvider;
          //  WithDataFontSize(72);
          //  WithTitleFontSize(24); 
        
        
        private void Initialize(Manual manual)
        {
            var dataNotifierProvider = (IDataNotifierProvider<T>)manual.RepresentedInstance;

            var dataNotifier = dataNotifierProvider.GetDataNotifier();

            lbCom.Text = manual.Descripcion;
            dataNotifier.Subscribe(NewDataHandler);

            int titleFontSize = dataNotifier.HasFormatDirectives ? dataNotifier.SenderNameFontSize : DefaultTitleFontSize;
            int dataFontSize = dataNotifier.HasFormatDirectives ? dataNotifier.DataFontSize : DefaultDataFontSize;

            SetTitleFontSize(titleFontSize);
            SetDataFontSize(dataFontSize); 
            
        }

        private void NewDataHandler(object sender, DataEventArgs<T> e)
        {
             lbStringDysplay.Text = e.Data.ToString();
        }

        private void SetDataFontSize(int fontSize)
        {
            lbStringDysplay.Font = new System.Drawing.Font("Microsoft Sans Serif", fontSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            
        }

        private void SetTitleFontSize(int fontSize)
        {
            lbCom.Font = new System.Drawing.Font("Microsoft Sans Serif", fontSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           
        }

    }


   
}
