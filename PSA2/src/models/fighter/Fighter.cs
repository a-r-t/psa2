using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter
{
    public class Fighter
    {
        public string FileBase { get; set; }
        public List<Action> Actions { get;  set; }
        public List<SubAction> SubActions { get;  set; }
        public List<Subroutine> SubRoutines { get;  set; }
        public List<Override> Overrides { get;  set; }
        public List<Attribute> Attributes { get;  set; }
        public List<Article> Articles { get;  set; }
        public ModelVisibility ModelVisibility { get; set; }

        public Fighter()
        {
            FileBase = "Test";
            Actions = new List<Action>();
            SubActions = new List<SubAction>();
            SubRoutines = new List<Subroutine>();
            Overrides = new List<Override>();
            Attributes = new List<Attribute>();
            Articles = new List<Article>();
            ModelVisibility = new ModelVisibility();
        }
    }
}
