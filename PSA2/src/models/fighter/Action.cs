using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter
{
    public class Action
    {
        public string ActionName { get; private set; }
        public int ActionNumber { get; private set; }
        public ActionSection[] ActionSections { get; private set; }

        public Action(int number)
        {
            ActionName = "Test State";
            ActionNumber = number;
            ActionSections = new ActionSection[]
            {
                new ActionSection("Entry"),
                new ActionSection("Exit")
            };
        }
    }
}
