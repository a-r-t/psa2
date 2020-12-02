using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Views.MovesetEditorViews.SectionSelectors
{
    public class SubActionOption
    {
        public string Name { get; set; }
        public int Index { get; set; }

        public SubActionOption(string name, int index)
        {
            Name = name;
            Index = index;
        }

        public override string ToString()
        {
            string displayName = Index.ToString("X");
            if (Name != "")
            {
                displayName += $" - {Name}";
            }
            return displayName;
        }
    }
}
