using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.Views.CustomControls
{
    public partial class ScintillaListBox : ScintillaExt
    {
        public List<string> Items { get; }

        private int selectedIndex = -1;
        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                int oldIndex = selectedIndex;
                if (oldIndex != value)
                {
                    selectedIndex = value;
                    SelectLine(selectedIndex);
                    SelectIndexChangedEvent(this, new EventArgs());
                    previousSelectedIndex = oldIndex;
                }
            }
        }

        private Color itemForeColor;
        public Color ItemForeColor
        {
            get
            {
                return itemForeColor;
            }
            set
            {
                itemForeColor = value;
                SetupStyles();
                StyleDocument();
            }
        }

        private Color itemBackColor;
        public Color ItemBackColor
        {
            get
            {
                return itemBackColor;
            }
            set
            {
                itemBackColor = value;
                SetupStyles();
                StyleDocument();
            }
        }

        private Color selectedItemForeColor;
        public Color SelectedItemForeColor
        {
            get
            {
                return selectedItemForeColor;
            }
            set
            {
                selectedItemForeColor = value;
                SetupStyles();
                StyleDocument();
            }
        }

        private Color selectedItemBackColor;
        public Color SelectedItemBackColor
        {
            get
            {
                return selectedItemBackColor;
            }
            set
            {
                selectedItemBackColor = value;
                SetupStyles();
                StyleDocument();
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
                SetupStyles();
                StyleDocument();
            }
        }

        private string fontFamily;
        public string FontFamily
        {
            get
            {
                return fontFamily;
            }
            set
            {
                fontFamily = value;
                SetupStyles();
                StyleDocument();
            }
        }

        private float fontSize;
        public float FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                fontSize = value;
                SetupStyles();
                StyleDocument();
            }
        }

        public event EventHandler SelectedIndexChanged;
        private int currentIndexClicked = -1;
        private int previousSelectedIndex = -1;

        public ScintillaListBox() : base()
        {
            FullLineSelect = false;
            MultipleSelection = false;
            Items = new List<string>();
            for (int i = 0; i < Margins.Count; i++)
            {
                Margins[i].Width = 0;
            }

            BackgroundColor = Color.White;
            FontFamily = "Consolas";
            FontSize = 10;

            ItemForeColor = Color.FromArgb(0, 0, 0);
            ItemBackColor = BackgroundColor;
            SelectedItemForeColor = Color.White;
            SelectedItemBackColor = Color.FromArgb(38, 79, 120);

            SetupStyles();

            Lexer = Lexer.Container;

            ReadOnly = true;
            CurrentCursor = Cursors.Arrow;
            CaretStyle = CaretStyle.Invisible;

            StyleDocument();
        }

        private void SetupStyles()
        {
            SetSelectionBackColor(false, Color.Transparent);
            Styles[Style.Default].BackColor = BackgroundColor;
            Styles[Style.Default].Font = FontFamily;
            Styles[Style.Default].SizeF = FontSize;
            StyleClearAll();

            Styles[1].ForeColor = ItemForeColor;
            Styles[1].BackColor = ItemBackColor;

            Styles[2].FillLine = true;
            Styles[2].ForeColor = SelectedItemForeColor;
            Styles[2].BackColor = SelectedItemBackColor;
        }

        private void SelectIndexChangedEvent(object sender, EventArgs e)
        {
            SelectedIndexChanged?.Invoke(sender, e);
        }

        public void AddItem(string item)
        {
            ReadOnly = false;
            if (Items.Count == 0)
            {
                Text = item;
            }
            else
            {
                Text += ($"\n{item}");
            }
            Items.Add(item);
            ReadOnly = true;

            if (SelectedIndex == -1)
            {
                SelectedIndex = 0;
            }

            StyleDocument();
        }

        public void AddItems(List<string> items)
        {
            ReadOnly = false;
            string joinedItems = string.Join("\n", items);
            if (Items.Count == 0)
            {
                Text = joinedItems;
            }
            else
            {
                Text += ($"\n{joinedItems}");
            }
            Items.AddRange(items);
            ReadOnly = true;

            if (SelectedIndex == -1)
            {
                SelectedIndex = 0;
            }

            StyleDocument();
        }

        public void ClearItems()
        {
            Items.Clear();
            ReadOnly = false;
            Text = "";
            ReadOnly = true;
            SelectedIndex = -1;
        }

        public void InsertItem(int index, string item)
        {
            Items.Insert(index, item);
            ReadOnly = false;
            Text = string.Join("\n", Items);
            ReadOnly = true;

            if (SelectedIndex == -1)
            {
                SelectedIndex = 0;
            }

            StyleDocument();
        }

        public void RemoveItem(int index)
        {
            Items.RemoveAt(index);
            ReadOnly = false;
            Text = string.Join("\n", Items);
            ReadOnly = true;

            if (Items.Count == 0)
            {
                SelectedIndex = -1;
            }
        }

        protected override void OnUpdateUI(UpdateUIEventArgs e)
        {
            if ((e.Change & UpdateChange.Selection) == UpdateChange.Selection)
            {
                if (currentIndexClicked >= 0)
                {
                    SelectedIndex = currentIndexClicked;
                }
            }

            if ((e.Change & UpdateChange.Selection) == UpdateChange.Selection
                || (e.Change & UpdateChange.HScroll) == UpdateChange.HScroll
                || (e.Change & UpdateChange.VScroll) == UpdateChange.VScroll)
            {
                StyleDocument();
            }

            base.OnUpdateUI(e);
        }

        private void SelectLine(int lineIndex)
        {
            (int startPosition, int endPosition) = GetLineStartAndEndPositions(lineIndex);
            SetSelection(endPosition, startPosition);
            CurrentPosition = endPosition;
        }

        public void StyleDocument()
        {
            StartStyling(0);
            for (int i = 0; i < Lines.Count; i++)
            {
                StyleLineIndex(i, selectedIndex == i);
            }
        }

        private void StyleLineIndex(int lineIndex, bool isSelected)
        {
            int style = !isSelected ? 1 : 2;

            string text = Lines[lineIndex].Text;
            if (!string.IsNullOrEmpty(text))
            {
                SetStyling(text.Length, style);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int charIndex = CharPositionFromPoint(e.X, e.Y);
            currentIndexClicked = LineFromPosition(charIndex);

            base.OnMouseDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                if (currentIndexClicked < Items.Count - 1)
                {
                    currentIndexClicked++;
                    OnUpdateUI(new UpdateUIEventArgs(UpdateChange.Selection));
                }
            }
            else if (e.KeyData == Keys.Up)
            {
                if (currentIndexClicked > 0)
                {
                    currentIndexClicked--;
                    OnUpdateUI(new UpdateUIEventArgs(UpdateChange.Selection));
                }
            }

            base.OnKeyDown(e);
        }
    }
}
