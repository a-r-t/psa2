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
    public partial class ScintillaListSelect : ScintillaExt
    {
        public List<string> Items { get; }

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
        public int CurrentHoveredIndex { get; set; } = -1;
        private int previousHoveredIndex = -1;

        public ScintillaListSelect() : base()
        {
            FullLineSelect = false;
            MultipleSelection = false;
            Items = new List<string>();
            for (int i = 0; i < Margins.Count; i++)
            {
                Margins[i].Width = 0;
            }
            
            SetSelectionBackColor(false, Color.Red);
             
            Styles[Style.Default].BackColor = Color.White;
            Styles[Style.Default].Font = "Consolas";
            Styles[Style.Default].SizeF = 10;
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
  
            StyleDocument();
        }

        private const int WM_MOUSEMOVE = 0x0200;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEMOVE)
            {
                Point mouseCoordinates = PointFromLParam(m.LParam);
                MouseMoved(new MouseEventArgs(MouseButtons.Left, 1, mouseCoordinates.X, mouseCoordinates.Y, 0));
            }
            base.WndProc(ref m);
        }

        public Point PointFromLParam(IntPtr lParam)
        {
            return new Point((int)(lParam) & 0xFFFF, ((int)(lParam) >> 16) & 0xFFFF);
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

            StyleDocument();
        }

        public void ClearItems()
        {
            Items.Clear();
            ReadOnly = false;
            Text = "";
            ReadOnly = true;
            CurrentHoveredIndex = -1;
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

        public void ModifyItem(int index, string newItem)
        {
            Items[index] = newItem;
            ReadOnly = false;
            Text = string.Join("\n", Items);
            ReadOnly = true;
        }

        protected override void OnUpdateUI(UpdateUIEventArgs e)
        {
            if ((e.Change & UpdateChange.HScroll) == UpdateChange.HScroll
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
                StyleLineIndex(i, CurrentHoveredIndex == i);
            }
        }

        private void StyleLineIndex(int lineIndex, bool isHovered)
        {
            int style = !isHovered ? 1 : 2;

            string text = Lines[lineIndex].Text;
            if (!string.IsNullOrEmpty(text))
            {
                SetStyling(text.Length, style);
            }
        }

        protected void MouseMoved(MouseEventArgs e)
        {
            // TODO: manually check mouse position to determine if it is on a line or not
            int charIndex = CharPositionFromPointClose(e.X, e.Y);
            int lineIndex = charIndex != -1 ? LineFromPosition(charIndex) : -1;

            int previousLineIndex = CurrentHoveredIndex;
            CurrentHoveredIndex = lineIndex;
            if (previousLineIndex != CurrentHoveredIndex)
            {
                StyleDocument();
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Point cursorLocation = PointToClient(Cursor.Position);
            if (cursorLocation.X < 0 || cursorLocation.X > Width || cursorLocation.Y < 0 || cursorLocation.Y > Height)
            {
                int oldHoveredIndex = CurrentHoveredIndex;
                CurrentHoveredIndex = -1;
                if (oldHoveredIndex != CurrentHoveredIndex)
                {
                    StyleDocument();
                }
            }
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            ClearSelections();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                if (CurrentHoveredIndex < Items.Count - 1)
                {
                    CurrentHoveredIndex++;
                    StyleDocument();
                }
            }
            else if (e.KeyData == Keys.Up)
            {
                if (CurrentHoveredIndex > 0)
                {
                    CurrentHoveredIndex--;
                    StyleDocument();
                }
            }

            base.OnKeyDown(e);
        }
    }
}
