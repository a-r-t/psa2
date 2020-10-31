using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Models.Fighter.Misc
{
    public class Crawl
    {
        public int Offset { get; set; }
        public int Forward { get; set; }
        public int Backward { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString()}, {nameof(Forward)}={Forward.ToString()}, {nameof(Backward)}={Backward.ToString()}}}";
        }
    }
}
