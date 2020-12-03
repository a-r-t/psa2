using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Models.Fighter
{
    public class Subroutine
    {
        public int Location { get; set; }

        public Subroutine(int location)
        {
            Location = location;
        }

        public override string ToString()
        {   
            return $"Subroutine Location: {Location}";
        }
    }
}
