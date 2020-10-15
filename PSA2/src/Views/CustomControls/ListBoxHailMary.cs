using System;
using System.Drawing;
using System.Windows.Forms;

internal class ListBoxHailMary : ListBox
{
    public ListBoxHailMary()
    {
        this.SetStyle(
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint,
            true);
        this.DoubleBuffered = true;
        this.DrawMode = DrawMode.OwnerDrawFixed;
    }

    private const int WM_HSCROLL = 0x114;
    private const int WM_VSCROLL = 0x115;

    private const int SB_LINELEFT = 0;
    private const int SB_LINERIGHT = 1;
    private const int SB_PAGELEFT = 2;
    private const int SB_PAGERIGHT = 3;
    private const int SB_THUMBPOSITION = 4;
    private const int SB_THUMBTRACK = 5;
    private const int SB_LEFT = 6;
    private const int SB_RIGHT = 7;
    private const int SB_ENDSCROLL = 8;

    private const int SIF_TRACKPOS = 0x10;
    private const int SIF_RANGE = 0x1;
    private const int SIF_POS = 0x4;
    private const int SIF_PAGE = 0x2;
    private const int SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS;
    private int mHScroll;
    bool justGotFocus = false;

    protected override void OnGotFocus(EventArgs e)
    {
        base.OnGotFocus(e);
    }

    protected override void OnSelectedIndexChanged(EventArgs e)
    {
        base.OnSelectedIndexChanged(e);
    }

    protected override void WndProc(ref System.Windows.Forms.Message msg)
    {
        if (msg.Msg == WM_HSCROLL)
        {
            switch ((int)msg.WParam & 0xffff)
            {
                case SB_PAGELEFT:
                    mHScroll = Math.Max(0, mHScroll - ClientSize.Width * 2 / 3); //A page is 2/3 the width.
                    break;
                case SB_PAGERIGHT:
                    mHScroll = Math.Min(HorizontalExtent, mHScroll + ClientSize.Width * 2 / 3);
                    break;
                case SB_THUMBPOSITION:
                case SB_THUMBTRACK:
                    mHScroll = ((int)msg.WParam >> 16) & 0xffff;
                    break;
            }
        }
        base.WndProc(ref msg);
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        if (this.Items.Count > 0)
        {
            e.DrawBackground();
            if (e.State == DrawItemState.Default)
            {
                e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), new PointF(e.Bounds.X - mHScroll, e.Bounds.Y));
            }
            else
            {
                e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), new PointF(e.Bounds.X, e.Bounds.Y));
            }
        }
        base.OnDrawItem(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Region iRegion = new Region(e.ClipRectangle);
        e.Graphics.FillRegion(new SolidBrush(this.BackColor), iRegion);
        if (this.Items.Count > 0)
        {
            for (int i = 0; i < this.Items.Count; ++i)
            {
                System.Drawing.Rectangle irect = this.GetItemRectangle(i);
                if (e.ClipRectangle.IntersectsWith(irect))
                {
                    if ((this.SelectionMode == SelectionMode.One && this.SelectedIndex == i)
                    || (this.SelectionMode == SelectionMode.MultiSimple && this.SelectedIndices.Contains(i))
                    || (this.SelectionMode == SelectionMode.MultiExtended && this.SelectedIndices.Contains(i)))
                    {
                        OnDrawItem(new DrawItemEventArgs(e.Graphics, this.Font,
                            irect, i,
                            DrawItemState.Selected, this.ForeColor,
                            this.BackColor));
                    }
                    else
                    {
                        OnDrawItem(new DrawItemEventArgs(e.Graphics, this.Font,
                            irect, i,
                            DrawItemState.Default, this.ForeColor,
                            this.BackColor));
                    }
                    iRegion.Complement(irect);
                }
            }
        }
        justGotFocus = false;
        //HorizontalExtent = (int)e.Graphics.MeasureString(Items[1].ToString(), this.Font).Width;

        base.OnPaint(e);
    }
}