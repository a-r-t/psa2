using System;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Forms;

internal class ListBoxHailMary : ListBox
{
    public ListBoxHailMary()
    {
        this.SetStyle(
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint,
            true);
        this.DoubleBuffered = true;
        this.DrawMode = DrawMode.OwnerDrawFixed;
    }

    private const int WM_HSCROLL = 0x114;
    private const int WM_VSCROLL = 0x115;
    private const int WM_MOUSEWHEEL = 0x020A;
    private const int SB_LINELEFT = 0;
    private const int SB_LINERIGHT = 1;
    private const int SB_PAGELEFT = 2;
    private const int SB_PAGERIGHT = 3;
    private const int SB_THUMBPOSITION = 4;
    private const int SB_THUMBTRACK = 5;
    private const int SB_LEFT = 6;
    private const int SB_RIGHT = 7;
    private const int SB_ENDSCROLL = 8;
    private const int WM_KEYDOWN = 0x0100;
    private const int SB_HORZ = 0;
    private const int SB_VERT = 1;
    private const int SIF_TRACKPOS = 0x10;
    private const int SIF_RANGE = 0x1;
    private const int SIF_POS = 0x4;
    private const int SIF_PAGE = 0x2;
    private const int SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS;
    private int mHScroll;
    private bool resizing;

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
                case SB_LINERIGHT:
                case SB_LINELEFT:
                    mHScroll = ((int)msg.WParam >> 2) & 0xffff;
                    break;
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
        else if (msg.Msg == WM_KEYDOWN)
        {
            switch (msg.WParam.ToInt32())
            {
                case (int)Keys.Left:
                    return;
                case (int)Keys.Right:
                    return;
            }
        }
        else if (msg.Msg == 5) // WM_SIZE = 0x05
        {
            this.resizing = true;
            base.WndProc(ref msg);
            this.resizing = false;
            return;
        }
        base.WndProc(ref msg);
        
    }
    
    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        if (e is DrawItemEventArgsExt)
        {
            if (e.Index == -1 || (((DrawItemEventArgsExt)e).FromOnPaint && this.resizing))
            {
                return;
            }
        }
        if (this.Items.Count > 0)
        {
            e.DrawBackground();
            if ((e.State & DrawItemState.Default) == DrawItemState.Default)
            {
                e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), new PointF(e.Bounds.X - mHScroll, e.Bounds.Y));
            }
            else if ((e.State & DrawItemState.Selected) == DrawItemState.Selected && (e.State & DrawItemState.NoAccelerator) == DrawItemState.NoAccelerator)
            {
                e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), new PointF(e.Bounds.X, e.Bounds.Y));
            }
            else if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), new PointF(e.Bounds.X - mHScroll, e.Bounds.Y));
            }
            else
            {
                e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), new PointF(e.Bounds.X - mHScroll, e.Bounds.Y));
            }

        }
        base.OnDrawItem(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        int largestStringSize = 0;
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
                        OnDrawItem(new DrawItemEventArgsExt(e.Graphics, this.Font,
                            irect, i,
                            DrawItemState.Selected, this.ForeColor,
                            this.BackColor, true));
                    }
                    else
                    {
                        OnDrawItem(new DrawItemEventArgsExt(e.Graphics, this.Font,
                            irect, i,
                            DrawItemState.Default, this.ForeColor,
                            this.BackColor, true));
                    }
                    iRegion.Complement(irect);

                    int stringSize = (int)e.Graphics.MeasureString(Items[1].ToString(), this.Font).Width;
                    if (stringSize > largestStringSize)
                    {
                        largestStringSize = stringSize;
                    }
                }
            }
        }
        //HorizontalExtent = largestStringSize;
        HorizontalExtent = 1000;


        base.OnPaint(e);
    }

    protected class DrawItemEventArgsExt : DrawItemEventArgs
    {
        public bool FromOnPaint { get; set; }

        public DrawItemEventArgsExt(Graphics graphics, Font font, Rectangle rect, int index, DrawItemState state, Color foreColor, Color backColor, bool fromOnPaint) 
            : base(graphics, font, rect, index, state, foreColor, backColor)
        {
            FromOnPaint = fromOnPaint;
        }
    }
}