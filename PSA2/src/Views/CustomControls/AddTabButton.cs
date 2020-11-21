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
        public bool IsMouseDown { get; set; }

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

            int endWidth = Width - 1;

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

        public bool IsPointOverButton(Point p)
        {
            return p.X >= 0 && p.X < Width && p.Y >= 0 && p.Y < Height;
        }
    }
}
