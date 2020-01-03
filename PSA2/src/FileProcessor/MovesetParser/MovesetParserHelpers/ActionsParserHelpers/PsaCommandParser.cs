﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.ActionsParserHelpers
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
            if (PsaFile.FileContent[instructionLocation] == -86052851 || PsaFile.FileContent[instructionLocation + 1] < 0 || PsaFile.FileContent[instructionLocation] >= PsaFile.DataSectionSize)
            {
                // FADEF00D
                // Error Data x8 something something
                return null;
            }
            else
            {
                int instruction = 0;
                List<PsaCommandParameter> parameters = new List<PsaCommandParameter>();

                int rawPsaInstruction = PsaFile.FileContent[instructionLocation];
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
                        Console.WriteLine($"Variable: {ConvertParamValueToVariable(paramValue)}");
                    }
                    else if (paramType == (int)ParamType.SCALAR)
                    {
                        Console.WriteLine($"Scalar: {ConvertParamValueToScalar(paramValue)}");
                        // $"{(decimal)paramValue / 60000m:0.0####}";    --- this was what it originally was in PSA-C
                    }
                    else if (paramType == (int)ParamType.VALUE || paramType == (int)ParamType.UNKNOWN1)
                    {
                        Console.WriteLine($"Value: {paramValue.ToString("X")}");
                    }
                    else if (paramType == (int)ParamType.BOOLEAN)
                    {
                        Console.WriteLine($"Boolean: {(paramValue == 1 ? "true" : "false")}");
                    }
                    else if (paramType == (int)ParamType.REQUIREMENT)
                    {
                        Console.WriteLine(ConvertParamValueToRequirement(paramValue));
                    }
                    else if (paramType == (int)ParamType.POINTER)
                    {
                        Console.WriteLine($"Pointer: {paramValue.ToString("X")}");
                    }

                    parameters.Add(new PsaCommandParameter(PsaFile.FileContent[commandParamsLocation + i], PsaFile.FileContent[commandParamsLocation + i + 1]));
                }

                Console.WriteLine(String.Format("Instruction: {0}", instruction));
                parameters.ForEach(t => Console.WriteLine(String.Format("Param Type: {0}, Param Value: {1}", t.Type, t.Value)));
                return new PsaCommand(instruction, parameters);
            }
        }

        public PsaVariable ConvertParamValueToVariable(int paramValue)
        {
            int memoryType = (paramValue >> 28) & 0xF;
            int datatype = (paramValue >> 24) & 0xF;
            int id = paramValue & 0xFFFFFF;
            return new PsaVariable(memoryType, datatype, id);
        }

        public decimal ConvertParamValueToScalar(int paramValue)
        {
            return decimal.Divide(paramValue, 60000);
        }

        public PsaRequirement ConvertParamValueToRequirement(int paramValue)
        {
            int inverseFlag = (paramValue >> 16) & 0xFFFF;
            int requirementId = paramValue & 0xFFFF;

            return new PsaRequirement(requirementId, inverseFlag); 
        }

        public enum ParamType
        {
            VALUE, SCALAR, POINTER, BOOLEAN, UNKNOWN1, VARIABLE, REQUIREMENT
        }



    }
}
