using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Views.MovesetEditorViews
{
    public interface ILocationSelectorListener
    {
        void OnSelect(string sectionText, SectionType sectionType, int sectionIndex, int codeBlockIndex);
    }
}
