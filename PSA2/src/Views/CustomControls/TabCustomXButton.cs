using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.Views.CustomControls
{
    public partial class TabCustomXButton : PanelButton
    {
        public int XPadding { get; set; }
        public int XWidth { get; set; }

        public TabCustomXButton(): base()
        {
            XPadding = 0;
            XWidth = 1;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            Pen pen = new Pen(currentTextColor);
            pen.Width = XWidth;

            g.DrawLine(pen, 0 + XPadding, 0 + XPadding, Width - 1 - XPadding, Height - 1 - XPadding);
            g.DrawLine(pen, Width - 1 - XPadding, 0 + XPadding, 0 + XPadding, Height - 1 - XPadding);

            //g.DrawLine(pen, Width - 1 - XPadding, Height - 1 - XPadding, 0 + XPadding, 0 + XPadding);
            //g.DrawLine(pen, 0 + XPadding, Height - 1 - XPadding, Width - 1 - XPadding, 0 + XPadding);

            base.OnPaint(e);
        }
    }
}
