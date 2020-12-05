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

        /// <summary>
        /// Gets all subroutines in moveset (currently only actions and subactions but this will be updated later)
        /// </summary>
        /// <returns>list of subroutine locations</returns>
        public List<int> GetAllSubroutineLocations()
        {
            HashSet<int> subroutines = new HashSet<int>();
            int numberOfSpecialActions = ActionsParser.GetNumberOfSpecialActions();
            int numberOfSubActions = SubActionsParser.GetNumberOfSubActions();

            List<int> actionsSubroutines = GetAllSubroutinesFromActions();
            foreach (int subroutine in actionsSubroutines)
            {
                subroutines.Add(subroutine);
            }

            List<int> subActionsSubroutines = GetAllSubroutinesFromSubActions();
            foreach (int subroutine in subActionsSubroutines)
            {
                subroutines.Add(subroutine);
            }

            // "recursively" without using recursion find all subroutines from other subroutines
            HashSet<int> emptySubroutines = new HashSet<int>();
            List<int> subroutinesList = subroutines.ToList();
            for (int i = 0; i < subroutinesList.Count; i++)
            {
                int subRoutineCommandsPointerLocation = subroutinesList[i];
                List<PsaCommand> commands = GetPsaCommandsForSubroutine(subRoutineCommandsPointerLocation / 4);
                if (commands.Count == 0)
                {
                    emptySubroutines.Add(subRoutineCommandsPointerLocation);
                }
                else
                {
                    foreach (PsaCommand command in commands)
                    {
                        if (command != null)
                        {

                            if (command.Instruction.ToString("X8") == "00070100" && command.Parameters[0].Value < PsaFile.DataSectionSize && !subroutines.Contains(command.Parameters[0].Value))
                            {
                                subroutines.Add(command.Parameters[0].Value);
                                subroutinesList.Add(command.Parameters[0].Value);
                            }
                            else if (command.Instruction.ToString("X8") == "0D000200" && command.Parameters[1].Value < PsaFile.DataSectionSize && !subroutines.Contains(command.Parameters[1].Value))
                            {
                                subroutines.Add(command.Parameters[1].Value);
                                subroutinesList.Add(command.Parameters[1].Value);
                            }
                        }
                    }
                }
            }

            HashSet<int> uniqueSubroutines = new HashSet<int>(subroutinesList);
            uniqueSubroutines.RemoveWhere(s => emptySubroutines.Contains(s) || !IsSubroutineValid(s));
            
            return uniqueSubroutines.ToList();
        }

        private bool IsSubroutineValid(int location)
        {
            try
            {
                // subroutine are tricky to identify and sometimes not readable
                // this code is just attemping to get psa commands from the subroutine (which should not throw an exception if the subroutine is "valid" for what is being looked for)
                // it also ensures each psa command is valid (not null or an instruction less than 0).
                // as time goes on this will possibly be added to.
                // if anything throws an exception, the subroutine won't be added to the list of subroutines returned from this method
                List<PsaCommand> psaCommands = GetPsaCommandsForSubroutine(location / 4);
                foreach (PsaCommand psaCommand in psaCommands)
                {
                    if (psaCommand == null)
                    {
                        return false;
                    }
                    if (psaCommand.Instruction < 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private List<int> GetAllSubroutinesFromActions()
        {
            List<int> subroutines = new List<int>();

            int numberOfSpecialActions = ActionsParser.GetNumberOfSpecialActions();

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
                                subroutines.Add(command.Parameters[0].Value);
                            }
                            else if (command.Instruction.ToString("X8") == "0D000200" && command.Parameters[1].Value < PsaFile.DataSectionSize)
                            {
                                subroutines.Add(command.Parameters[1].Value);
                            }
                        }
                    }
                }
            }

            return subroutines;
        }

        private List<int> GetAllSubroutinesFromSubActions()
        {
            List<int> subroutines = new List<int>();

            int numberOfSubActions = SubActionsParser.GetNumberOfSubActions();

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
                                subroutines.Add(command.Parameters[0].Value);
                            }
                            else if (command.Instruction.ToString("X8") == "0D000200" && command.Parameters[1].Value < PsaFile.DataSectionSize)
                            {
                                subroutines.Add(command.Parameters[1].Value);
                            }
                        }
                    }
                }
            }

            return subroutines;
        }
    }
}
