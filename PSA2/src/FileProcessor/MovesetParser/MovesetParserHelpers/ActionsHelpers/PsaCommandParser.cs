using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.ActionsHelpers
{
    public class PsaCommandParser
    {
        public PsaFile PsaFile { get; private set; }
        
        public PsaCommandParser(PsaFile psaFile)
        {
            PsaFile = psaFile;
        }

        public PsaCommand GetPsaInstruction(int instructionLocation) // instructionLocation = j
        {
            Console.WriteLine("Get PSA Instruction");
            int instruction = 0;
            List<(int Type, int Value)> parameters = new List<(int Type, int Value)>();

            if (PsaFile.FileContent[instructionLocation] == -86052851)
            {
                // FADEF00D
            }
            else if (PsaFile.FileContent[instructionLocation + 1] < 0 || PsaFile.FileContent[instruction] >= PsaFile.DataSectionSize)
            {
                // Error Data x8 something something
            }

            int rawPsaInstruction = PsaFile.FileContent[instructionLocation];
            Console.WriteLine(rawPsaInstruction);
            Console.WriteLine(rawPsaInstruction.ToString("X8"));

            // this ain't even needed, whyyyy
            int firstHalfPsaInstruction = ((rawPsaInstruction >> 16) & 0xFFFF); // this gets you the first half of it? so like if a command is 120B0100, it'll give back 120B

            /*
            PsaCommandConfig psaCommand;
            if (!PsaCommands.ContainsKey(rawPsaInstruction.ToString("X8")))
            {
                // psa command not found...
                //Console.WriteLine("NOT FOUND");
            }
            else
            {
                //Console.WriteLine("FOUND!");
                psaCommand = PsaCommands[rawPsaInstruction.ToString("X8")];
            }
            */
            instruction = rawPsaInstruction;

            // gets number of params in instruction based on 3rd byte in word for instruction's location
            // e.g. 120B0100's 3rd byte is 01, which means it only has 1 param
            int numberOfParams = ((rawPsaInstruction >> 8) & 0xFF);

            // guessing
            int commandParamsLocation = PsaFile.FileContent[instructionLocation + 1] / 4;
            for (int i = 0; i < numberOfParams * 2; i += 2)
            {
                int paramType = PsaFile.FileContent[commandParamsLocation + i];
                int paramValue = PsaFile.FileContent[commandParamsLocation + i + 1];
                Console.WriteLine(String.Format("Param Type: {0}", paramType));
                Console.WriteLine(String.Format("Param Value: {0}", paramValue));

                if (paramType == (int)ParamType.VARIABLE)
                {
                    Console.WriteLine(ConvertParamValueToVariable(paramValue));
                }
                else if (paramType == (int)ParamType.SCALAR)
                {
                    Console.WriteLine(ConvertParamValueToScalar(paramValue));
                    // Console.WriteLine($"{paramValue / 60000m}");
                    // Console.WriteLine($"{(decimal)paramValue / 60000m:0.0####}");    --- this was what it originally was in PSA-C
                }
                else if (paramType == (int)ParamType.VALUE || paramType == (int)ParamType.UNKNOWN1)
                {
                    Console.WriteLine(paramValue.ToString("X"));
                }
                else if (paramType == (int)ParamType.BOOLEAN)
                {
                    Console.WriteLine(paramValue == 0 ? "true" : "false");
                    /*
                        Original Code:

                        if (alm[n + 1] == 0)
                        {
                            rd1 += "false";
                        }
                        else if (alm[n + 1] == 1)
                        {
                            rd1 += "true";
                        }
                        else
                        {
                            rd1 = rd1 + "3x" + alm[n + 1].ToString("X");
                        }
                    */
                }
                else if (paramType == (int)ParamType.REQUIREMENT)
                {

                }

                parameters.Add((Type: PsaFile.FileContent[commandParamsLocation + i], Value: PsaFile.FileContent[commandParamsLocation + i + 1]));
            }

            Console.WriteLine(String.Format("Instruction: {0}", instruction));
            parameters.ForEach(t => Console.WriteLine(String.Format("Param Type: {0}, Param Value: {1}", t.Type, t.Value)));
            return new PsaCommand(instruction, parameters);
        }

        public PsaVariable ConvertParamValueToVariable(int paramValue)
        {
            int memoryType = (paramValue >> 28) & 0xF;
            int datatype = (paramValue >> 24) & 0xF;
            int id = paramValue & 0xFFFFFF;

            // 301989950
/*            Console.WriteLine(paramValue >> 28);
            Console.WriteLine(paramValue >> 24);
            Console.WriteLine((paramValue >> 28) & 0xF);
            Console.WriteLine((paramValue >> 24) & 0xF);*/
                    return new PsaVariable(memoryType, datatype, id);
        }

        public decimal ConvertParamValueToScalar(int paramValue)
        {
            return decimal.Divide(paramValue, 60000);
        }

        public enum ParamType
        {
            VALUE, SCALAR, POINTER, BOOLEAN, UNKNOWN1, VARIABLE, REQUIREMENT
        }



    }
}
