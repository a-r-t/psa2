using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2MovesetLogic.src.Models.Fighter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    public class SubroutinesHandler
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public PsaCommandHandler PsaCommandHandler { get; private set; }
        public ActionsHandler ActionsParser { get; private set; }
        public SubActionsHandler SubActionsParser { get; private set; }

        public SubroutinesHandler(PsaFile psaFile, int dataSectionLocation, ActionsHandler actionsParser, SubActionsHandler subActionsParser, PsaCommandHandler psaCommandHandler)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            PsaCommandHandler = psaCommandHandler;
            ActionsParser = actionsParser;
            SubActionsParser = subActionsParser;
        }

        public List<PsaCommand> GetPsaCommandsForSubroutine(int subRoutineLocation)
        {
            return PsaCommandHandler.GetPsaCommands(subRoutineLocation);
        }

        public PsaCommand GetPsaCommand(int subRoutineLocation, int commandIndex)
        {
            return PsaCommandHandler.GetPsaCommand(subRoutineLocation + commandIndex * 2);
        }

        public List<Subroutine> GetAllSubroutines()
        {
            HashSet<int> subRoutines = new HashSet<int>();
            int numberOfSpecialActions = ActionsParser.GetNumberOfSpecialActions();
            int numberOfSubActions = SubActionsParser.GetNumberOfSubActions();

            // find all subroutines in all actions
            for (int i = 0; i < numberOfSpecialActions; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    List<PsaCommand> commands = ActionsParser.GetPsaCommandsInCodeBlock(i, j);
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
                    List<PsaCommand> commands = SubActionsParser.GetPsaCommandsInCodeBlock(i, j);
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
                int subRoutineCommandsPointerLocation = subRoutinesList[i];
                List<PsaCommand> commands = GetPsaCommandsForSubroutine(subRoutineCommandsPointerLocation / 4);
                if (commands.Count == 0)
                {
                    emptySubRoutines.Add(subRoutineCommandsPointerLocation);
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

            HashSet<int> uniqueSubroutines = new HashSet<int>(subRoutinesList);
            uniqueSubroutines.RemoveWhere(s => emptySubRoutines.Contains(s));

            List<Subroutine> subroutines = new List<Subroutine>();
            foreach (int subroutineLocation in uniqueSubroutines)
            {
                try
                {
                    // subroutine are tricky to identify and sometimes not readable
                    // this code is just attemping to get psa commands from the subroutine (which should not throw an exception if the subroutine is "valid" for what is being looked for)
                    // it also ensures each psa command is valid (not null or an instruction less than 0).
                    // as time goes on this will possibly be added to.
                    // if anything throws an exception, the subroutine won't be added to the list of subroutines returned from this method
                    List<PsaCommand> psaCommands = GetPsaCommandsForSubroutine(subroutineLocation / 4);
                    foreach (PsaCommand psaCommand in psaCommands)
                    {
                        if (psaCommand == null)
                        {
                            throw new NullReferenceException("Invalid psa command");
                        }
                        if (psaCommand.Instruction < 0)
                        {
                            throw new ArgumentOutOfRangeException("Invalid psa command instruction");
                        }
                    }
                    subroutines.Add(new Subroutine(subroutineLocation));
                }
                catch (Exception ex)
                {
                }
            }
            return subroutines;
        }
    }
}
