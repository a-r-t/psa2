using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.ActionsParserHelpers
{
    public class PsaCommandParameter
    {
        public int Type { get; set; }
        public int Value { get; set; }

        public PsaCommandParameter(int type, int value)
        {
            Type = type;
            Value = value;
        }


    }
}
