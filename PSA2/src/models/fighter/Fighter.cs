using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter
{
    public class Fighter
    {
        public string FilePath { get; set; }
        public string FileBase { get; set; }
        public List<Action> Actions { get; private set; }
        public List<SubAction> SubActions { get; private set; }
        public List<Subroutine> SubRoutines { get; private set; }
        public List<Override> Overrides { get; private set; }
        public List<Attribute> Attributes { get; private set; }
        public List<Article> Articles { get; private set; }

        public Fighter(string filePath)
        {
            FilePath = filePath;
            FileBase = "Test";
            Actions = new List<Action>();
            SubActions = new List<SubAction>();
            SubRoutines = new List<Subroutine>();
            Overrides = new List<Override>();
            Attributes = new List<Attribute>();
            Articles = new List<Article>();
            LoadMovesetData();
        }

        /// <summary>
        /// Parses moveset data from file and loads it into object
        /// </summary>
        private void LoadMovesetData()
        {

        }
    }
}
