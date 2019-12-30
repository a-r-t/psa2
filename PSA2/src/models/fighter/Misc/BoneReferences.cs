using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class BoneReferences
    {
        public int Offset { get; set; }
        public List<int> Bones { get; set; }

        public BoneReferences()
        {
            Bones = new List<int>();
        }
    }
}
