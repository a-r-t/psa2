using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.fighter
{
    public class ActionSection
    {
        public string SectionName { get; private set; }
        public List<Command> Commands { get; private set; }

        public ActionSection(string sectionName)
        {
            SectionName = sectionName;
            Commands = new List<Command>();
        }
    }
}
