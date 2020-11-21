using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.Views.CustomControls
{
    public partial class AddTabButton : PanelButton
    {
        public int PlusSignThickness { get; set; }
        public int PlusSignPadding { get; set; }

        public AddTabButton(): base()
        {
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            SolidBrush brush = new SolidBrush(BackColor);
            g.FillRectangle(brush, new Rectangle(Location.X, Location.Y, Width, Height));

            Pen pen = new Pen(currentTextColor);
            pen.Width = PlusSignThickness;

            int difference = Height - Width;
            int endWidth = Width - 1;
            int endHeight = Height - 1;
            int widthHalf = endWidth / 2;
            int heightHalf = endHeight / 2;

            //int y_padding = ((endHeight - (endWidth / 2)) / 2) / 2;
            //g.DrawLine(pen, endWidth / 2, 0 + PlusSignPadding + y_padding, endWidth / 2, endWidth - PlusSignPadding + y_padding);
            //g.DrawLine(pen, 0 + PlusSignPadding, (endWidth / 2) + y_padding, endWidth - PlusSignPadding, (endWidth / 2) + y_padding);

            /*
            int plus_line_len = endWidth / 2;
            int y_padding = (endHeight - plus_line_len) / 2;
            g.DrawLine(pen, plus_line_len, y_padding, plus_line_len, plus_line_len + y_padding);
            g.DrawLine(pen, PlusSignPadding, endHeight / 2, PlusSignPadding + plus_line_len, endHeight / 2);
            */

            // north to south
            g.DrawLine(pen, endWidth / 2, 0 + PlusSignPadding, endWidth / 2, endWidth - PlusSignPadding);

            // east to west
            g.DrawLine(pen, 0 + PlusSignPadding, endWidth / 2, endWidth - PlusSignPadding, endWidth / 2);
        }

        protected override void OnResize(EventArgs eventargs)
        {
            Height = Width;
            base.OnResize(eventargs);
        }
    }
}
