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
        public int SelectedIndex { get; set; }
        public Color ItemForeColor { get; set; }
        public Color ItemBackColor { get; set; }
        public Color SelectedItemForeColor { get; set; }
        public Color SelectedItemBackColor { get; set; }
        public Color BackgroundColor { get; set; }

        public ScintillaListBox()
        {
            MultipleSelection = false;
            Items = new List<string>();
            for (int i = 0; i < Margins.Count; i++)
            {
                Margins[i].Width = 0;
            }
            CurrentCursor = Cursors.Arrow;

            Styles[Style.Default].BackColor = Color.White;

            Styles[1].ForeColor = Color.FromArgb(0, 0, 0);
            Styles[1].BackColor = Styles[Style.Default].BackColor;

            Styles[2].FillLine = true;
            Styles[2].ForeColor = Color.White;
            Styles[2].BackColor = Color.FromArgb(38, 79, 120);
        }

        public void AddItem(string item)
        {
            Items.Add(item);
            Text += ($"\n{item}");
        }

        public void ClearItems()
        {
            Items.Clear();
            Text = "";
        }

        public void InsertItem(int index, string item)
        {
            Items.Insert(index, item);
            Text = string.Join("\n", Items);
        }

        public void RemoveItem(int index)
        {
            Items.RemoveAt(index);
            Text = string.Join("\n", Items);
        }

        protected override void OnUpdateUI(UpdateUIEventArgs e)
        {
            if ((e.Change & UpdateChange.Selection) == UpdateChange.Selection)
            {
                int currentLine = LineFromPosition(CurrentPosition);
                SelectedIndex = currentLine;
                SelectLine(currentLine);
            }

            base.OnUpdateUI(e);
        }

        private void SelectLine(int lineIndex)
        {
            (int startPosition, int endPosition) = GetLineStartAndEndPositions(lineIndex);
            SetSelection(endPosition, startPosition);
        }

    }
}
