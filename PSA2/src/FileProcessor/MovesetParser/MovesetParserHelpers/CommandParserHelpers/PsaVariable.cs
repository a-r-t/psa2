using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.CommandParserHelpers
{
    public class PsaVariable
    {
        public int MemoryType { get; set; }
        public int DataType { get; set; }
        public int Id { get; set; }

        public PsaVariable(int memoryType, int dataType, int id)
        {
            MemoryType = memoryType;
            DataType = dataType;
            Id = id;
        }

        public override string ToString()
        {
            return $"{GetMemoryTypeAsString()}-{GetDataTypeAsString()}[{Id}]";
        }

        public string GetMemoryTypeAsString()
        {
            switch (MemoryType)
            {
                case 0: return "IC";
                case 1: return "LA";
                case 2:
                default: return "RA";
            }
        }

        public string GetDataTypeAsString()
        {
            switch (DataType)
            {
                case 0: return "Basic";
                case 1: return "Float";
                case 2:
                default: return "Bit";
            }
        }
    }
}
