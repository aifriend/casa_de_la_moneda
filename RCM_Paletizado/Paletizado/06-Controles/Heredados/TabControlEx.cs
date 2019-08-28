using System;
using System.Drawing;
using System.Windows.Forms;

// CONVERT WARNING :Option Explicit statement is not implemented.

namespace Idpsa
{
    public class TabControlEx : TabControl
    {
        private const int WM_LBUTTONDOWN = 513;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN)
            {
                var pt = new Point(m.LParam.ToInt32());
                int index;
                for (index = 0; index <= TabPages.Count - 1; index++)
                {
                    if (GetTabRect(index).Contains(pt))
                    {
                        if (TabPages[index].Enabled)
                        {
                            base.WndProc(ref m);
                        }
                        return;
                    }
                }
            }
            base.WndProc(ref m);
        }

        protected override void OnKeyDown(KeyEventArgs ke)
        {
            int currentIndex = SelectedIndex;
            int index;
            if (ke.KeyCode == Keys.Left && !(ke.Alt && !ke.Control))
            {
                for (index = currentIndex - 1; index >= 0; index += -1)
                {
                    if (TabPages[index].Enabled)
                    {
                        SelectedIndex = index;
                        break;
                    }
                }
                ke.Handled = true;
            }
            else if (ke.KeyCode == Keys.Right && !(ke.Alt && !ke.Control))
            {
                for (index = currentIndex + 1; index <= TabPages.Count - 1; index++)
                {
                    if (TabPages[index].Enabled)
                    {
                        SelectedIndex = index;
                        break;
                    }
                }
                ke.Handled = true;
            }
            base.OnKeyDown(ke);
        }

        public void DisablePage(ref TabPage pTabPage)
        {
            pTabPage.Enabled = false;
        }

        public void EnablePage(ref TabPage pTabPage)
        {
            pTabPage.Enabled = true;
        }

        private void TabControlEx_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                Int32 intOffsetLeft;
                Int32 intOffsetTop;
                Rectangle rec = e.Bounds;
                var r = new RectangleF(rec.X, rec.Y, rec.Width, rec.Height);
                RectangleF r2;
                var ItemBrush = new SolidBrush(BackColor);
                Brush b;
                if (TabPages[e.Index].Enabled)
                {
                    b = Brushes.Black;
                }
                else
                {
                    b = Brushes.Gray;
                }
                var sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                Bitmap im = null;
                if (TabPages[e.Index].ImageIndex != -1)
                {
                    im = (Bitmap) ImageList.Images[TabPages[e.Index].ImageIndex];
                }
                if (TabPages[e.Index].ImageIndex != -1)
                {
                    r2 = new RectangleF(r.X + (im.Width/2), r.Y, r.Width, r.Height);
                }
                else
                {
                    r2 = new RectangleF(r.X, r.Y, r.Width, r.Height);
                }
                if ((e.State & DrawItemState.Selected) != 0)
                {
                    e.Graphics.FillRectangle(ItemBrush, e.Bounds);
                    e.Graphics.DrawString(TabPages[e.Index].Text, e.Font, b, r2, sf);
                    intOffsetLeft = 5;
                    intOffsetTop = 5;
                }
                else
                {
                    e.Graphics.DrawString(TabPages[e.Index].Text, e.Font, b, r2, sf);
                    intOffsetLeft = 2;
                    intOffsetTop = 2;
                }
                if (TabPages[e.Index].ImageIndex != -1)
                {
                    ImageList.Draw(e.Graphics, Convert.ToInt32(r.Left) + intOffsetLeft,
                                   Convert.ToInt32(r.Top) + intOffsetTop, TabPages[e.Index].ImageIndex);
                }
            }
            catch (Exception ex)
            {
                //'The control is probably being disposed!!!
            }
        }
    }
}