using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Views.MovesetEditorViews
{
    public class StyleNeededForSelectedLinesEventArgs : StyleNeededEventArgs
    {
        public int StartPosition { get; }
        public List<int> SelectedLines { get; }

        public StyleNeededForSelectedLinesEventArgs(ScintillaNET.Scintilla scintilla, int startPosition, int endPosition, List<int> selectedLines)
            : base(scintilla, endPosition)
        {
            StartPosition = startPosition;
            SelectedLines = selectedLines;
        }
    }
}
