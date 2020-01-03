using PSA2.src.FileProcessor.MovesetParser;
using PSA2.src.FileProcessor.MovesetParser.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter
{
    public class SubActionSection
    {
        public string SectionName { get; private set; }
        public List<PsaCommand> Commands { get; private set; }

        public SubActionSection(string sectionName)
        {
            SectionName = sectionName;
            Commands = new List<PsaCommand>();
        }
    }
}
