using PSA2.src.Views.MovesetEditorViews;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.ExtentionMethods
{
    public static class ScintillaExtensions
    {
        /*
        public static (int, int, List<int>) GetSelectedLinesByPositionRange(this ScintillaNET.Scintilla scintilla)
        {
            HashSet<int> selectedLines = new HashSet<int>();
            int startPosition = scintilla.CurrentPosition;
            selectedLines.Add(scintilla.LineFromPosition(startPosition));
            int endPosition = startPosition;
            
            foreach (Selection selection in scintilla.Selections)
            {
                for (int i = selection.Start; i < selection.End; i++)
                {
                    if (i < startPosition)
                    {
                        startPosition = i;
                    }
                    if (i > endPosition)
                    {
                        endPosition = i;
                    }
                    selectedLines.Add(scintilla.LineFromPosition(i));
                }
            }

            string text = scintilla.Text;

            while (startPosition != 0 && text[startPosition - 1] != '\n')
            {
                startPosition--;
            }

            while (endPosition < text.Length && text[endPosition] != '\n')
            {
                endPosition++;
            }
            return (startPosition, endPosition, selectedLines.ToList<int>());
        }

        public static List<int> GetSelectedLines(this ScintillaNET.Scintilla scintilla)
        {
            HashSet<int> selectedLines = new HashSet<int>();
            selectedLines.Add(scintilla.LineFromPosition(scintilla.CurrentPosition));

            foreach (Selection selection in scintilla.Selections)
            {
                for (int i = selection.Start; i < selection.End; i++)
                {
                    selectedLines.Add(scintilla.LineFromPosition(i));
                }
            }

            return selectedLines.ToList<int>();
        }
        */
    }
}
