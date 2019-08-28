using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Idpsa
{
    public partial class TabControlColor : TabControl
    {
        private readonly List<bool> failTabs;

        public TabControlColor()
        {
            InitializeComponent();
            failTabs = new List<bool>();
            for (int i = 0; i < 5; i++)
            {
                failTabs.Add(false);
            }
        }


        public void SetFailTab(int index, bool value)
        {
            failTabs[index] = value;
        }

        public void ClearFailTabs()
        {
            for (int i = 0; i < failTabs.Count; i++)
            {
                failTabs[i] = false;
            }
        }


        public void PaintTabs()
        {
            for (int i = 0; i < failTabs.Count; i++)
            {
                TabPage CurrentTab = TabPages[i];
                Rectangle ItemRect = GetTabRect(i);
                SolidBrush FillBrush;
                var TextBrush = new SolidBrush(Color.Black);
                var sf = new StringFormat();
                Font font = Font;
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                FillBrush = new SolidBrush(failTabs[i] ? Color.Salmon : Color.LightGray);

                TabPages[i].CreateGraphics().FillRectangle(FillBrush, ItemRect);
                TabPages[i].CreateGraphics().DrawString(TabPages[i].Text, font, TextBrush, ItemRect, sf);

                //this.TabPages[i].Invalidate();
                Invalidate();

                //base.OnDrawItem(e);
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            TabPage CurrentTab = TabPages[e.Index];
            Rectangle ItemRect = GetTabRect(e.Index);
            SolidBrush FillBrush;


            var TextBrush = new SolidBrush((e.State == DrawItemState.Selected) ? Color.Blue : Color.Black);
            var sf = new StringFormat();
            Font font = Font;
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            FillBrush = new SolidBrush(failTabs[e.Index] ? Color.Salmon : Color.LightGray);

            if (e.State == DrawItemState.Selected)
            {
                font = new Font(font.FontFamily, 22);
            }

            e.Graphics.FillRectangle(FillBrush, ItemRect);
            e.Graphics.DrawString(TabPages[e.Index].Text, font, TextBrush, ItemRect, sf);

            base.OnDrawItem(e);
        }
    }
}