using PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.CommandParserHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers
{
    public class SubRoutinesParser
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public PsaCommandHandler PsaCommandHandler { get; private set; }
        public ActionsParser ActionsParser { get; private set; }
        public SubActionsParser SubActionsParser { get; private set; }

        public SubRoutinesParser(PsaFile psaFile, int dataSectionLocation, ActionsParser actionsParser, SubActionsParser subActionsParser, PsaCommandHandler psaCommandHandler)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            PsaCommandHandler = psaCommandHandler;
            ActionsParser = actionsParser;
            SubActionsParser = subActionsParser;
        }

        public List<PsaCommand> GetPsaCommandsForSubRoutine(int subRoutineLocation)
        {
            return PsaCommandHandler.GetPsaCommands(subRoutineLocation);
        }

        public List<int> GetAllSubRoutines()
        {
            HashSet<int> subRoutines = new HashSet<int>();
            int numberOfSpecialActions = ActionsParser.GetNumberOfSpecialActions();
            int numberOfSubActions = SubActionsParser.GetNumberOfSubActions();

            // find all subroutines in all actions
            for (int i = 0; i < numberOfSpecialActions; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    List<PsaCommand> commands = ActionsParser.GetPsaCommandsForActionCodeBlock(i, j);
                    foreach (PsaCommand command in commands)
                    {
                        if (command != null)
                        {
                            if (command.Instruction.ToString("X8") == "00070100" && command.Parameters[0].Value < PsaFile.DataSectionSize)
                            {
                                subRoutines.Add(command.Parameters[0].Value);
                            }
                            else if (command.Instruction.ToString("X8") == "0D000200" && command.Parameters[1].Value < PsaFile.DataSectionSize)
                            {
                                subRoutines.Add(command.Parameters[1].Value);
                            }
                        }
                    }
                }
            }

            // find all subroutines in all sub actions
            for (int i = 0; i < numberOfSubActions; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    List<PsaCommand> commands = SubActionsParser.GetPsaCommandsForSubAction(i, j);
                    foreach (PsaCommand command in commands)
                    {
                        if (command != null)
                        {
                            if (command.Instruction.ToString("X8") == "00070100" && command.Parameters[0].Value < PsaFile.DataSectionSize)
                            {
                                subRoutines.Add(command.Parameters[0].Value);
                            }
                            else if (command.Instruction.ToString("X8") == "0D000200" && command.Parameters[1].Value < PsaFile.DataSectionSize)
                            {
                                subRoutines.Add(command.Parameters[1].Value);
                            }
                        }
                    }
                }
            }

            // "recursively" without using recursion find all subroutines from other subroutines
            HashSet<int> emptySubRoutines = new HashSet<int>();
            List<int> subRoutinesList = subRoutines.ToList();
            for (int i = 0; i < subRoutinesList.Count; i++)
            {
                int subRoutineLocation = subRoutinesList[i];

                List<PsaCommand> commands = GetPsaCommandsForSubRoutine(subRoutineLocation);
                if (commands.Count == 0)
                {
                    emptySubRoutines.Add(subRoutineLocation);
                }
                else
                {
                    foreach (PsaCommand command in commands)
                    {
                        if (command != null)
                        {
                            if (command.Instruction.ToString("X8") == "00070100" && command.Parameters[0].Value < PsaFile.DataSectionSize)
                            {
                                subRoutines.Add(command.Parameters[0].Value);
                                subRoutinesList.Add(command.Parameters[0].Value);
                            }
                            else if (command.Instruction.ToString("X8") == "0D000200" && command.Parameters[0].Value < PsaFile.DataSectionSize)
                            {
                                subRoutines.Add(command.Parameters[1].Value);
                                subRoutinesList.Add(command.Parameters[1].Value);
                            }
                        }
                    }
                }
            }

            HashSet<int> subRoutinesNested = subRoutinesList.ToHashSet<int>();
            subRoutinesNested.RemoveWhere(emptySubRoutines.Contains);
            subRoutinesNested.ToList().OrderBy(x => x.ToString("X8")).ToList().ForEach(s => Console.WriteLine(s.ToString("X8")));
            return subRoutinesNested.ToList().OrderBy(x => x.ToString("X8")).ToList();
        }
    }
}
