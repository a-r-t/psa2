using PSA2.src.ExtentionMethods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.Views.CustomControls
{
    public partial class TabCustom: Panel
    {
        public override string Text { get; set; }
        public int Index { get; set; }

        private Color textColor;
        public Color TextColor 
        {
            get
            {
                return textColor;
            }
            set
            {
                textColor = value;
                if (!isSelected)
                {
                    currentTextColor = value;
                }
            }
        }

        private Color selectedTextColor;
        public Color SelectedTextColor 
        {
            get
            {
                return selectedTextColor;
            }
            set
            {
                selectedTextColor = value;
                if (isSelected)
                {
                    currentTextColor = value;
                }
            }
        }

        private Color backgroundColor;
        public Color BackgroundColor 
        { 
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
                if (!isSelected)
                {
                    BackColor = value;
                }
            } 
        }

        private Color selectedBackColor;
        public Color SelectedBackgroundColor 
        { 
            get
            {
                return selectedBackColor;
            }
            set
            {
                selectedBackColor = value;
                if (isSelected)
                {
                    BackColor = value;
                }
            }
        }

        private Color currentTextColor;

        public bool isSelected;
        public bool IsSelected 
        { 
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                if (isSelected)
                {
                    StyleSelected();
                }
                else
                {
                    StyleUnselected();
                }
            } 
        }

        private Size xButtonSize;
        public Size XButtonSize 
        { 
            get
            {
                return xButtonSize;
            }
            set
            {
                xButtonSize = value;
                XButtonLocation = GetXButtonLocation();
            }
        }
        public Point XButtonLocation { get; set; }
        public Color HoverBackgroundColor { get; set; }
        public Color HoverTextColor { get; set; }
        public int TextLeftPadding { get; set; }
        public int XButtonSpacingFromRight { get; set; }
        public int XButtonPadding { get; set; } = 0;
        public int XButtonWidth { get; set; } = 1;
        public Color XButtonXColor { get; set; }
        public Color XButtonXHoveredColor { get; set; }
        public Color XButtonXSelectedColor { get; set; }
        public Color XButtonBackColor { get; set; }
        public Color XButtonHoveredBackColor { get; set; }
        public Color XButtonSelectedBackColor { get; set; }
        public Color TabSelectedXButtonSelectedXColor { get; set; }
        public Color TabSelectedXButtonSelectedBackColor { get; set; }
        private Color currentXButtonColor;
        private Color currentXButtonBackColor;
        private bool xButtonDisabled;
        public bool XButtonDisabled
        { 
            get
            {
                return xButtonDisabled;
            } 
            set
            {
                xButtonDisabled = value;
                if (xButtonDisabled)
                {
                    currentXButtonColor = XButtonXColor;
                    currentXButtonBackColor = XButtonBackColor;
                }
            } 
        }
        public bool XButtonMouseDown { get; set; }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            xButtonDisabled = false;
            base.OnMouseUp(e);
        }

        const int WM_LBUTTONUP = 0x202;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONUP)
            {

            }
            base.WndProc(ref m);
        }

        public TabCustom(string text) : base()
        {
            Font = new Font("Times New Roman", 10, FontStyle.Regular);
            Text = text;
            Width = text.MeasureTextRenderer(Font).Width;
            Height = text.MeasureTextRenderer(Font).Height;

            TextColor = Color.Black;
            BackgroundColor = Color.White;
            SelectedTextColor = Color.White;
            SelectedBackgroundColor = Color.Black;
            HoverBackgroundColor = Color.Gray;
            HoverTextColor = TextColor;
            TextLeftPadding = 4;

            XButtonSize = new Size(14, 14);
            XButtonLocation = new Point(Width - XButtonSize.Width - XButtonSpacingFromRight, (Height / 2) - (XButtonSize.Height / 2));

            XButtonXColor = Color.Black;
            XButtonBackColor = Color.White;
            XButtonXHoveredColor = Color.White;
            XButtonHoveredBackColor = Color.DarkGray;
            XButtonXSelectedColor = Color.White;
            XButtonSelectedBackColor = Color.Black;

            TabSelectedXButtonSelectedXColor = Color.Black;
            TabSelectedXButtonSelectedBackColor = Color.White;


            XButtonPadding = 2;
            XButtonWidth = 2;
            XButtonSpacingFromRight = 4;
            StyleUnselected();
            DoubleBuffered = true;
        }

        public virtual void StyleUnselected()
        {
            BackColor = BackgroundColor;
            currentTextColor = TextColor;
            currentXButtonColor = XButtonXColor;
            currentXButtonBackColor = XButtonBackColor;
        }

        public virtual void StyleSelected()
        {
            BackColor = SelectedBackgroundColor;
            currentTextColor = SelectedTextColor;
            currentXButtonColor = XButtonXSelectedColor;
            currentXButtonBackColor = XButtonSelectedBackColor;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Size textSize = Text.MeasureTextRenderer(Font);
            e.Graphics.DrawString(Text, Font, new SolidBrush(currentTextColor), new Point(TextLeftPadding, (Height - textSize.Height) / 2));
            DrawXButton(e);
        }

        private Point GetXButtonLocation()
        {
            return new Point(Width - XButtonSize.Width - XButtonSpacingFromRight, (Height / 2) - (XButtonSize.Height / 2));
        }

        private void DrawXButton(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            SolidBrush brush = new SolidBrush(currentXButtonBackColor);
            g.FillRectangle(brush, new Rectangle(XButtonLocation.X, XButtonLocation.Y, xButtonSize.Width, xButtonSize.Height));

            Pen pen = new Pen(currentXButtonColor);
            pen.Width = XButtonWidth;

            g.DrawLine(pen, XButtonLocation.X + XButtonPadding, XButtonLocation.Y + XButtonPadding, XButtonLocation.X + xButtonSize.Width - XButtonPadding, XButtonLocation.Y + xButtonSize.Height - XButtonPadding);
            g.DrawLine(pen, XButtonLocation.X + xButtonSize.Width - XButtonPadding, XButtonLocation.Y + XButtonPadding, XButtonLocation.X + XButtonPadding, XButtonLocation.Y + xButtonSize.Height - XButtonPadding);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (!IsSelected)
            {
                currentTextColor = HoverTextColor;
                BackColor = HoverBackgroundColor;
                if (!XButtonDisabled)
                {
                    if (IsMouseOverXButton())
                    {
                        currentXButtonColor = XButtonXHoveredColor;
                        currentXButtonBackColor = XButtonHoveredBackColor;
                    }
                    else
                    {
                        currentXButtonColor = XButtonXHoveredColor;
                        currentXButtonBackColor = HoverBackgroundColor;
                    }
                }
            }
            base.OnMouseEnter(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!XButtonDisabled)
            {
                if (!IsSelected)
                {
                    if (IsMouseOverXButton())
                    {
                        currentXButtonColor = XButtonXHoveredColor;
                        currentXButtonBackColor = XButtonHoveredBackColor;
                    }
                    else
                    {
                        currentXButtonColor = XButtonXHoveredColor;
                        currentXButtonBackColor = HoverBackgroundColor;
                    }
                }
                else
                {
                    if (IsMouseOverXButton())
                    {
                        currentXButtonColor = TabSelectedXButtonSelectedXColor;
                        currentXButtonBackColor = TabSelectedXButtonSelectedBackColor;
                    }
                    else
                    {
                        currentXButtonColor = XButtonXSelectedColor;
                        currentXButtonBackColor = SelectedBackgroundColor;
                    }
                }
                Refresh();
            }
            base.OnMouseMove(e);
        }

        private bool IsPointInXButton(Point p)
        {
            return p.X > XButtonLocation.X && p.X < XButtonLocation.X + XButtonSize.Width && p.Y > XButtonLocation.Y && p.Y < XButtonLocation.Y + XButtonSize.Height;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (!IsSelected)
            {
                currentTextColor = TextColor;
                BackColor = BackgroundColor;
                if (!XButtonDisabled)
                {
                    currentXButtonColor = XButtonXColor;
                    currentXButtonBackColor = XButtonBackColor;
                }
            }
            else
            {
                currentXButtonColor = XButtonXSelectedColor;
                currentXButtonBackColor = XButtonBackColor;
            }
            base.OnMouseLeave(e);
        }

        protected override void OnResize(EventArgs eventargs)
        {
            XButtonLocation = GetXButtonLocation();
            base.OnResize(eventargs);
        }

        public bool IsMouseOverXButton()
        {
            return IsPointInXButton(PointToClient(Cursor.Position));
        }
    }
}
