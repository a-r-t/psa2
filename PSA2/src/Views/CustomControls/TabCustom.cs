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

        public Color HoverBackgroundColor { get; set; }

        /*
        private Color hoverTextColor;
        public Color HoverTextColor
        {
            get
            {
                return hoverTextColor;
            }
            set
            {
                hoverTextColor = value;
            }
        }
        */
        public Color HoverTextColor { get; set; }

        public TabCustom(string text) : base()
        {
            Font = new Font("Times New Roman", 10, FontStyle.Regular);
            Text = text;
            Width = text.Measure(Font).Width + 20;
            Height = 30;
            TextColor = Color.Black;
            BackgroundColor = Color.White;
            SelectedTextColor = Color.White;
            SelectedBackgroundColor = Color.Black;
            HoverBackgroundColor = Color.Gray;
            HoverTextColor = TextColor;
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
            e.Graphics.DrawString(Text, Font, new SolidBrush(currentTextColor), new Point(((Width - textSize.Width) / 2) + 3, (Height - textSize.Height) / 2));
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
    }
}
