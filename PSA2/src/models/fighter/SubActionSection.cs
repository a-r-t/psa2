using PSA2.src.FileProcessor.MovesetHandler.Configs;
using PSA2.src.FileProcessor.MovesetHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter
{
    public class SubActionSection
    {
        public string SectionName { get; private set; }
        public List<PsaCommandConfig> Commands { get; private set; }

        public SubActionSection(string sectionName)
        {
            SectionName = sectionName;
            Commands = new List<PsaCommandConfig>();
        }
    }
}
