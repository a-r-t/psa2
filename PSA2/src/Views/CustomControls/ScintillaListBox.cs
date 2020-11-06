using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.Views.CustomControls
{
    public partial class ScintillaListBox : ScintillaExt
    {
        public List<string> Items { get; }

        private int selectedIndex;
        public int SelectedIndex 
        { 
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;
                SelectLine(SelectedIndex);
            }
        }

        public Color ItemForeColor 
        { 
            get
            {
                return Styles[1].ForeColor;
            }
            set
            {
                Styles[1].ForeColor = value;

            }
        }

        public Color ItemBackColor 
        { 
            get
            {
                return Styles[1].BackColor;
            } 
            set
            {
                Styles[1].BackColor = value;
            }
        }

        public Color SelectedItemForeColor 
        { 
            get
            {
                return Styles[2].ForeColor;
            }
            set
            {
                Styles[2].ForeColor = value;
            }
        }

        public Color SelectedItemBackColor 
        {
            get
            {
                return Styles[2].BackColor;
            }
            set
            {
                Styles[2].BackColor = value;
            }
        }

        public Color BackgroundColor
        { 
            get
            {
                return Styles[Style.Default].BackColor;
            }
            set
            {
                Styles[Style.Default].BackColor = value;
            }
        }

        public event EventHandler SelectedIndexChanged;

        public ScintillaListBox() : base()
        {
            FullLineSelect = false;
            MultipleSelection = false;
            Items = new List<string>();
            for (int i = 0; i < Margins.Count; i++)
            {
                Margins[i].Width = 0;
            }
            SetSelectionBackColor(true, Color.FromArgb(38, 79, 120));

            Styles[Style.Default].BackColor = Color.White;
            //Styles[Style.Default].Font = "Consolas";
            //Styles[Style.Default].SizeF = 12;
            StyleClearAll();

            Styles[1].ForeColor = Color.FromArgb(0, 0, 0);
            Styles[1].BackColor = Styles[Style.Default].BackColor;

            Styles[2].FillLine = true;
            Styles[2].ForeColor = Color.White;
            Styles[2].BackColor = Color.FromArgb(38, 79, 120);
            Lexer = Lexer.Container;

            ReadOnly = true;
            CurrentCursor = Cursors.Arrow;
            CaretStyle = CaretStyle.Invisible;

            UpdateUI += new EventHandler<UpdateUIEventArgs> (SelectIndexChangedEvent);
            StyleDocument();
        }

        private void SelectIndexChangedEvent(object sender, UpdateUIEventArgs e)
        {
            SelectedIndexChanged?.Invoke(sender, e);
        }

        public void AddItem(string item)
        {
            Items.Add(item);
            ReadOnly = false;
            if (Items.Count == 1)
            {
                Text = item;
            }
            else
            {
                Text += ($"\n{item}");
            }
            ReadOnly = true;
            StyleDocument();
        }

        public void AddItems(List<string> items)
        {
            ReadOnly = false;
            string joinedItems = string.Join("\n", items);
            if (Items.Count == 1)
            {
                Text = joinedItems;
            }
            else
            {
                Text += ($"\n{joinedItems}");
            }
            ReadOnly = true;
            StyleDocument();
        }

        public void ClearItems()
        {
            Items.Clear();
            ReadOnly = false;
            Text = "";
            ReadOnly = true;
        }

        public void InsertItem(int index, string item)
        {
            Items.Insert(index, item);
            ReadOnly = false;
            Text = string.Join("\n", Items);
            ReadOnly = true;
            StyleDocument();
        }

        public void RemoveItem(int index)
        {
            Items.RemoveAt(index);
            ReadOnly = false;
            Text = string.Join("\n", Items);
            ReadOnly = true;
        }

        protected override void OnUpdateUI(UpdateUIEventArgs e)
        {
            if ((e.Change & UpdateChange.Selection) == UpdateChange.Selection)
            {
                int currentLine = LineFromPosition(CurrentPosition);
                SelectedIndex = currentLine;
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
            SetSelection(endPosition + 1, startPosition);
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

    }
}
