using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter
{
    public class SubAction
    {
        public string SubActionName { get; private set; }
        public int SubActionNumber { get; private set; }
        public SubActionSection[] SubActionSections { get; private set; }
        public Animation Animation { get; set; }

        public SubAction(int number)
        {
            SubActionName = "Test State";
            SubActionNumber = number;
            SubActionSections = new SubActionSection[]
            {
                new SubActionSection("Main"),
                new SubActionSection("GFX"),
                new SubActionSection("SFX"),
                new SubActionSection("Other")
            };
            Animation = new Animation("<null>");
        }
    }
}
