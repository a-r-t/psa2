﻿using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.Views.CustomControls
{
    public partial class ScintillaExt : ScintillaNET.Scintilla
    {
        public bool FullLineSelect { get; set; }
        private int[] originalLineIndexesSelected = new int[0]; // sometimes you gotta do what you gotta do to get winform controls to behave a certain way :(

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SetCursor(IntPtr hCursor);

        const int WM_SETCURSOR = 0x0020;
        const int WM_MOUSEMOVE = 0x0200;
        const int WM_LBUTTONUP = 0x202;
        bool mouseDown;

        public ScintillaExt() : base()
        {
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            mouseDown = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mouseDown = false;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SETCURSOR || m.Msg == WM_MOUSEMOVE)
            {
                if (!mouseDown)
                {
                    SetCursor(Cursors.Arrow.Handle);
                    return;
                }
            }

            base.WndProc(ref m);
            SetCursor(Cursors.Arrow.Handle);
        }

        public void UpdateCursor()
        {
            SetCursor(Cursors.Arrow.Handle);

        }

        protected override void OnUpdateUI(UpdateUIEventArgs e)
        {
            if (originalLineIndexesSelected.Length < Selections.Count)
            {
                Array.Resize(ref originalLineIndexesSelected, Selections.Count);
            }

            if (FullLineSelect && (e.Change & UpdateChange.Selection) == UpdateChange.Selection)
            {
                for (int i = 0; i < Selections.Count; i++)
                {
                    SelectLine(Selections[i], i);
                }
            }

            base.OnUpdateUI(e);
        }

        private void SelectLine(Selection selection, int selectionIndex)
        {
            if (selection.Anchor == selection.Caret)
            {
                originalLineIndexesSelected[selectionIndex] = LineFromPosition(selection.Anchor);

                while (selection.Anchor != 0 && Text[selection.Anchor - 1] != '\n')
                {
                    selection.Anchor--;
                }

                while (selection.Caret < Text.Length && Text[selection.Caret] != '\n')
                {
                    selection.Caret++;
                }
            }

            else
            {
                int currentLineIndexSelected = LineFromPosition(selection.Caret);
                if (currentLineIndexSelected > originalLineIndexesSelected[selectionIndex])
                {
                    while (selection.Anchor != 0 && Text[selection.Anchor - 1] != '\n')
                    {
                        selection.Anchor--;
                    }

                    while (selection.Caret < Text.Length && Text[selection.Caret] != '\n')
                    {
                        selection.Caret++;
                    }
                }
                else if (currentLineIndexSelected < originalLineIndexesSelected[selectionIndex])
                {
                    while (selection.Caret != 0 && Text[selection.Caret - 1] != '\n')
                    {
                        selection.Caret--;
                    }

                    while (selection.Anchor < Text.Length && Text[selection.Anchor] != '\n')
                    {
                        selection.Anchor++;
                    }
                }
                else
                {
                    while (selection.Anchor != 0 && Text[selection.Anchor - 1] != '\n')
                    {
                        selection.Anchor--;
                    }

                    while (selection.Caret < Text.Length && Text[selection.Caret] != '\n')
                    {
                        selection.Caret++;
                    }
                }
            }
        }

        public List<int> GetSelectedLines()
        {
            HashSet<int> selectedLines = new HashSet<int>();
            selectedLines.Add(LineFromPosition(CurrentPosition));

            foreach (Selection selection in Selections)
            {
                for (int i = selection.Start; i < selection.End; i++)
                {
                    selectedLines.Add(LineFromPosition(i));
                }
            }

            return selectedLines.ToList<int>();
        }

        public void SelectLines(List<int> lines)
        {
            ClearSelections();
            lines.Sort();

            Console.WriteLine("SELECTED LINES: " + string.Join(", ", lines));

            int currentSelectionIndex = 0;
            int currentStart = 0;
            int currentEnd = 0;
            for (int i = 0; i < lines.Count; i++)
            {
                if (i == 0)
                {
                    (currentStart, currentEnd) = GetLineStartAndEndPositions(lines[i]);
                }
                else if (lines[i - 1] == lines[i] - 1)
                {
                    (int lineStart, int lineEnd) = GetLineStartAndEndPositions(lines[i]);
                    currentEnd += ((lineEnd - lineStart) + 1);
                }
                else
                {
                    currentSelectionIndex++;
                    (currentStart, currentEnd) = GetLineStartAndEndPositions(lines[i]);
                    AddSelection(currentEnd, currentStart);
                }

                Selections[currentSelectionIndex].Anchor = currentStart;
                Selections[currentSelectionIndex].Caret = currentEnd;
            }

            this.originalLineIndexesSelected = new int[Selections.Count];
            for (int i = 0; i < Selections.Count; i++)
            {
                this.originalLineIndexesSelected[i] = LineFromPosition(Selections[i].Start);
                Console.WriteLine("SELECTION START AFTER: " + Selections[i].Anchor);
                Console.WriteLine("SELECTION END AFTER: " + Selections[i].Caret);
            }
        }

        public (int, int) GetLineStartAndEndPositions(int lineIndex)
        {
            int startPosition = 0;
            for (int i = 0; i < lineIndex; i++)
            {
                startPosition += Lines[i].Length;
            }

            return (startPosition, startPosition + (Lines[lineIndex].Length - 1));
        }
    }
}