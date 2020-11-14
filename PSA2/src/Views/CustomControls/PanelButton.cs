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
    public partial class PanelButton : Panel
    {
        private bool mouseDown;
        public override string Text { get; set; }
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
                SelectionStatusChangeEvent(this, new EventArgs());
            }
        }

        public Color HoverBackgroundColor { get; set; }
        public Color HoverTextColor { get; set; }

        public event EventHandler SelectedStatusChanged;

        public PanelButton(): base()
        {
            Font = new Font("Times New Roman", 10, FontStyle.Regular);
            TextColor = Color.Black;
            BackgroundColor = Color.White;
            SelectedTextColor = Color.White;
            SelectedBackgroundColor = Color.Black;
            HoverBackgroundColor = Color.Gray;
            HoverTextColor = TextColor;
        }

        private void SelectionStatusChangeEvent(object sender, EventArgs e)
        {
            OnSelectionStatusChanged();
            SelectedStatusChanged?.Invoke(sender, e);
        }

        protected virtual void OnSelectionStatusChanged()
        {

        }

        public virtual void StyleUnselected()
        {
            BackColor = BackgroundColor;
            currentTextColor = TextColor;
        }

        public virtual void StyleSelected()
        {
            BackColor = SelectedBackgroundColor;
            currentTextColor = SelectedTextColor;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Size textSize = Text.Measure(Font);
            e.Graphics.DrawString(Text, Font, new SolidBrush(currentTextColor), new Point(((Width - textSize.Width) / 2), (Height - textSize.Height) / 2));
            base.OnPaint(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (!IsSelected)
            {
                currentTextColor = HoverTextColor;
                BackColor = HoverBackgroundColor;
            }
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (!IsSelected)
            {
                currentTextColor = TextColor;
                BackColor = BackgroundColor;
            }
            base.OnMouseLeave(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = false;
                if (ContainsPoint(e.Location))
                {
                    IsSelected = !IsSelected;
                }
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
            }
            base.OnMouseDown(e);
        }

        public bool ContainsPoint(Point p)
        {
            return p.X > 0 && p.X < Width && p.Y > 0 && p.Y < Height;
        }
    }


}
