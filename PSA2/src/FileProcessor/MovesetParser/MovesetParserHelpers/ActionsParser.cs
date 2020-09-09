using PSA2.src.FileProcessor.MovesetParser.Configs;
using PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.CommandParserHelpers;
using PSA2.src.utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers
{
    public class ActionsParser
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public PsaCommandParser PsaCommandParser { get; private set; }
        public const int NOP = 131072; // 131072 is the nop command in psa (20000)
        public const int FADEF00D = -86052851; // -86052851 is FFFF FFFF FADE F00D
        public const int FADE0D8A = -86110838; // -86110838 is FFFF FFFF FADE 0D8A
        public const int ENTRY_CODE_BLOCK = 0;
        public const int EXIT_CODE_BLOCK = 1;

        public ActionsParser(PsaFile psaFile, int dataSectionLocation, string movesetBaseName)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            PsaCommandParser = new PsaCommandParser(PsaFile);
        }

        public int GetNumberOfSpecialActions()
        {
            //Console.WriteLine(String.Format("Number of Special Actions: {0}", (PsaFile.FileContent[DataSectionLocation + 10] - PsaFile.FileContent[DataSectionLocation + 9]) / 4));
            return (PsaFile.FileContent[DataSectionLocation + 10] - PsaFile.FileContent[DataSectionLocation + 9]) / 4;
        }

        public int GetTotalNumberOfActions()
        {
            return 274 + GetNumberOfSpecialActions();
        }

        /// <summary>
        /// Gets starting location in data section where new actions can be placed
        /// </summary>
        /// <returns>location in data section -- "stf" in psa-c</returns>
        public int GetOpenAreaStartLocation() // stf
        {
            return 2014 + GetNumberOfSpecialActions() * 2;
        }

        // this is the offset where action code starts (displayed in PSAC)
        public int GetActionCodeBlockCommandsLocation(int actionId, int codeBlockId)
        {
            int actionsCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);

            int actionCodeBlockCommandsLocation = PsaFile.FileContent[actionsCodeBlockLocation];

            return actionCodeBlockCommandsLocation;
        }

        public int GetActionCodeBlockLocation(int actionId, int codeBlockId)
        {
            int actionCodeBlockStartingLocation;

            switch(codeBlockId)
            {
                case ENTRY_CODE_BLOCK:
                    actionCodeBlockStartingLocation = PsaFile.FileContent[DataSectionLocation + 9] / 4;
                    break;
                case EXIT_CODE_BLOCK:
                    actionCodeBlockStartingLocation = PsaFile.FileContent[DataSectionLocation + 10] / 4;
                    break;
                default:
                    throw new ArgumentException("Invalid code block id -- only 0 (Entry) and 1 (Exit) are valid");
            }

            return actionCodeBlockStartingLocation + actionId;
        }

        public int GetActionCodeBlockCommandLocation(int actionId, int codeBlockId, int commandIndex)
        {
            int actionCodeBlockCommandsStartLocation = GetActionCodeBlockCommandsLocation(actionId, codeBlockId);
            return actionCodeBlockCommandsStartLocation / 4 + commandIndex * 2;
        }

        public List<PsaCommand> GetPsaCommandsForActionCodeBlock(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetActionCodeBlockCommandsLocation(actionId, codeBlockId);
            return PsaCommandParser.GetPsaCommands(actionCodeBlockLocation);
        }

        public PsaCommand GetPsaCommandForActionCodeBlock(int actionId, int codeBlockId, int commandIndex)
        {
            int commandLocation = GetActionCodeBlockCommandLocation(actionId, codeBlockId, commandIndex);
            return PsaCommandParser.GetPsaCommand(commandLocation);
        }

        public int GetNumberOfPsaCommandsInActionCodeBlock(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetActionCodeBlockCommandsLocation(actionId, codeBlockId);
            return PsaCommandParser.GetNumberOfPsaCommands(actionCodeBlockLocation);
        }

        /********************************
         * ADDING NEW COMMAND TO ACTION *
         * ******************************/
        public void AddCommandToAction(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetActionCodeBlockCommandsLocation(actionId, codeBlockId); // h
            int openAreaStartLocation = GetOpenAreaStartLocation();

            if (actionCodeBlockLocation == 0 || (actionCodeBlockLocation >= openAreaStartLocation * 4 && actionCodeBlockLocation < PsaFile.DataSectionSize))
            {
                // no commands yet exist for action
                if (actionCodeBlockLocation == 0)
                {
                    CreateActionCodeBlock(actionId, codeBlockId, openAreaStartLocation);
                }

                // if there are already existing commands for action
                else
                {
                    AddCommandToExistingActionCodeBlock(actionId, codeBlockId, actionCodeBlockLocation);
                }

                ApplyFileUpdatesToAccountForActionChanges();
            }
            else
            {
                throw new DataMisalignedException("Action code block location is not valid");
            }
        }

        public void CreateActionCodeBlock(int actionId, int codeBlockId, int openAreaStartLocation)
        {
            // I'm pretty sure this section finds the next available location to put a psa command, but not sure...
            int stoppingPoint = openAreaStartLocation;
            int bitStoppingPoint = 0;

            while (stoppingPoint < PsaFile.DataSectionSizeBytes && bitStoppingPoint != stoppingPoint + 4)
            {
                if (PsaFile.FileContent[stoppingPoint] == FADEF00D)
                {
                    bitStoppingPoint = stoppingPoint + 1;
                    while (bitStoppingPoint <= stoppingPoint + 3 && PsaFile.FileContent[bitStoppingPoint] != FADEF00D)
                    {
                        if (PsaFile.FileContent[bitStoppingPoint] != FADEF00D)
                        {
                            stoppingPoint = bitStoppingPoint;
                            break;
                        }
                        bitStoppingPoint++;
                    }
                }
                stoppingPoint++;
            }

            // this increases the size of the data section, I guess this happens if you try to add a new command and there's no room left in the data section
            if (stoppingPoint >= PsaFile.DataSectionSizeBytes)
            {
                IncreaseDataSectionSizeForActionWithNoExistingCommands();
            }

            // this creates the first "nop" command to start off the new location for the action's commands
            SetFileLocationToNop(stoppingPoint);

            // this increases the offset entires table by 1 for the new action offset entry
            int codeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);

            PsaFile.FileContent[codeBlockLocation] = stoppingPoint * 4;
            PsaFile.OffsetInterlockTracker[PsaFile.NumberOfOffsetEntries] = codeBlockLocation * 4;
            PsaFile.NumberOfOffsetEntries++;

            int newOffsetLocation = stoppingPoint * 4;
        }

        public void AddCommandToExistingActionCodeBlock(int actionId, int codeBlockId, int actionCodeBlockLocation)
        {
            int commandStartLocation = actionCodeBlockLocation / 4;

            int numberOfCommandsAlreadyInAction = GetNumberOfPsaCommandsInActionCodeBlock(actionId, codeBlockId); // g

            // TODO: Is this needed if I already get the count above?
            if (PsaFile.FileContent[commandStartLocation] == FADEF00D) // -86052851 is FFFF FFFF FADE F00D
            {
                numberOfCommandsAlreadyInAction = 0;
            }

            // if data section size needs to be resized, first check if it doesn't by seeing if there's any free space at the end (FADE0D8As) and if so that's free real estate to add a new command to
            // I think this is relpacing FADE0D8As with FADEF00DS if possible, and if this happens it means there is enough space to add the new command without needing to relocate the action
            bool isFreeSpaceAvailable = false;
            if (commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 5 > PsaFile.DataSectionSizeBytes)
            {
                isFreeSpaceAvailable = CheckForDataSectionTrailingFreeSpace(commandStartLocation, numberOfCommandsAlreadyInAction);
            }

            // I think this part checks if there is room for new command, and if there is, it doesn't need to move the existing commands all over to a new location so it dips early and just adds the nop
            // basically if either of the above if statemetns are true, this will be true
            if (isFreeSpaceAvailable)
            {
                SetFileLocationToNop(commandStartLocation + numberOfCommandsAlreadyInAction * 2);

                int newOffsetLocation = actionCodeBlockLocation / 4;

            }

            // The below code actually moves the action and its existing commands to a new location in the file if there's no room to add the new one
            // this part actually adds the new nop command
            else
            {
                int newOffsetLocation = RelocateAction(actionId, codeBlockId, numberOfCommandsAlreadyInAction, commandStartLocation);
                ApplyOffsetInterlockLogic(actionCodeBlockLocation, numberOfCommandsAlreadyInAction, newOffsetLocation);
            }
        }

        /// <summary>
        /// relocates action to a new location in file
        /// this happens when trying to add a command to an action and there is no more room to add the command
        /// returns new offset location for commands in action
        /// </summary>
        /// <param name="actionId">id of action (0 = 112)</param>
        /// <param name="codeBlockId">id of code block (0 is Entry, 1 is Exit)</param>
        /// <param name="numberOfCommandsAlreadyInAction">number of commands already in action</param>
        /// <param name="commandStartLocation">location where action's commands start</param>
        /// <returns>new offset location for commands in action (relocated offset)</returns>
        public int RelocateAction(int actionId, int codeBlockId, int numberOfCommandsAlreadyInAction, int commandStartLocation)
        {
            int openAreaStartLocation = GetOpenAreaStartLocation();

            // I don't think "commandOffset" is the right name for this...
            int commandOffset = numberOfCommandsAlreadyInAction * 2 + 4; // k
            int stoppingPoint = openAreaStartLocation;
            int bitStoppingPoint = 0;

            while (stoppingPoint < PsaFile.DataSectionSizeBytes && bitStoppingPoint != stoppingPoint + commandOffset)
            {
                if (PsaFile.FileContent[stoppingPoint] == FADEF00D)
                {
                    bitStoppingPoint = stoppingPoint + 1;
                    while (bitStoppingPoint <= stoppingPoint && PsaFile.FileContent[bitStoppingPoint] != FADEF00D)
                    {
                        if (PsaFile.FileContent[bitStoppingPoint] != FADEF00D)
                        {
                            stoppingPoint = bitStoppingPoint;
                            break;
                        }
                        bitStoppingPoint++;
                    }
                }
                stoppingPoint++;
            }

            // this increases the size of the data section, I guess this happens if you try to add a new command and there's no room left in the data section
            // stopping point is modified here if necessary
            if (stoppingPoint >= PsaFile.DataSectionSizeBytes)
            {
                stoppingPoint = IncreaseDataSectionSizeForActionWithExistingCommand(commandOffset);
            }

            commandOffset = numberOfCommandsAlreadyInAction * 2;

            // commandOffset might be the number of bits (or bytes?) taken up by all of the commands' pointers currently in the action
            // also NGL I have NO clue what this is doing
            int relocatingOffset = 0;
            for (int i = 0; i < commandOffset; i += 2)
            {
                // asc stuff (PointerInterlock)
                // NO clue what this does exactly or how it works
                if (((PsaFile.FileContent[commandStartLocation + i] >> 8) & 0xFF) != 0)
                {
                    // psac uses rmv for this name, no idea what this variable means
                    int rmv = (commandStartLocation + i) * 4 + 4;
                    for (int j = 0; j < PsaFile.NumberOfOffsetEntries; j++)
                    {
                        if (PsaFile.OffsetInterlockTracker[j] == rmv)
                        {
                            PsaFile.OffsetInterlockTracker[j] = (stoppingPoint + i) * 4 + 4;
                            break;
                        }
                    }
                }
                relocatingOffset = i;
                PsaFile.FileContent[stoppingPoint + i] = PsaFile.FileContent[commandStartLocation + i];
                PsaFile.FileContent[stoppingPoint + i + 1] = PsaFile.FileContent[commandStartLocation + i + 1];
                PsaFile.FileContent[commandStartLocation + i] = FADEF00D; 
                PsaFile.FileContent[commandStartLocation + i + 1] = FADEF00D; 
                relocatingOffset += 2;
            }

            PsaFile.FileContent[commandStartLocation + relocatingOffset] = FADEF00D; 
            PsaFile.FileContent[commandStartLocation + relocatingOffset + 1] = FADEF00D;

            SetFileLocationToNop(stoppingPoint + relocatingOffset);

            int codeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            int newOffsetLocation = stoppingPoint * 4;

            PsaFile.FileContent[codeBlockLocation] = newOffsetLocation;

            return newOffsetLocation;
        }

        public void SetFileLocationToNop(int location)
        {
            PsaFile.FileContent[location] = NOP;
            PsaFile.FileContent[location + 1] = 0;
            PsaFile.FileContent[location + 2] = 0;
            PsaFile.FileContent[location + 3] = 0;
        }

        public void ApplyOffsetInterlockLogic(int actionCodeBlockLocation, int numberOfCommandsAlreadyInAction, int newOffsetLocation)
        {
            int openAreaStartLocation = GetOpenAreaStartLocation();

            // this is the "EvOffsetInterlock" stuff, not entirely sure what this does, maybe updates existing command offsets so they don't break (like gotos)?
            // actionCodeBlockEndLocation is probably not the right name for this variable...
            int actionCodeBlockEndLocation = actionCodeBlockLocation + numberOfCommandsAlreadyInAction * 8;
            for (int i = openAreaStartLocation; i < PsaFile.DataSectionSizeBytes; i++)
            {
                if (PsaFile.FileContent[i] >= actionCodeBlockLocation && PsaFile.FileContent[i] <= actionCodeBlockEndLocation)
                {
                    // DEFINITELY not a good name for this variable, NO idea what it's for
                    int locationRoot = i * 4;

                    for (int j = 0; j < PsaFile.NumberOfOffsetEntries; j++)
                    {
                        if (PsaFile.OffsetInterlockTracker[j] == locationRoot)
                        {
                            if (PsaFile.FileContent[i] == actionCodeBlockLocation)
                            {
                                PsaFile.FileContent[i] = newOffsetLocation;
                                break;
                            }
                            int offsetDifference = PsaFile.FileContent[i] - actionCodeBlockLocation;
                            if (offsetDifference % 8 == 0)
                            {
                                PsaFile.FileContent[i] = newOffsetLocation + offsetDifference;
                            }
                            break;
                        }
                    }
                }
            }
        }

        public int IncreaseDataSectionSizeForActionWithNoExistingCommands()
        {
            int stoppingPoint = PsaFile.DataSectionSizeBytes;
            if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == FADE0D8A)
            {
                PsaFile.FileContent[PsaFile.DataSectionSizeBytes + 2] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2];
                PsaFile.FileContent[PsaFile.DataSectionSizeBytes + 3] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                stoppingPoint -= 2;
            }
            PsaFile.DataSectionSizeBytes += 4;

            return stoppingPoint;
        }

        // this increases the size of the data section, I guess this happens if you try to add a new command and there's no room left in the data section
        public int IncreaseDataSectionSizeForActionWithExistingCommand(int commandOffset)
        {
            int stoppingPoint = PsaFile.DataSectionSizeBytes;
            if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == FADE0D8A)
            {
                stoppingPoint -= 2;
                PsaFile.FileContent[PsaFile.DataSectionSizeBytes + commandOffset - 2] = FADE0D8A;
                PsaFile.FileContent[PsaFile.DataSectionSizeBytes + commandOffset - 1] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
            }
            PsaFile.DataSectionSizeBytes += commandOffset;

            return stoppingPoint;
        }

        /// <summary>
        /// This will check if the data section has free space after it, and if so "claims" this free space as part of the data section
        /// returns if there is free space found after doing this process
        /// </summary>
        public bool CheckForDataSectionTrailingFreeSpace(int commandStartLocation, int numberOfCommandsAlreadyInAction)
        {
            if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == FADE0D8A)
            {
                PsaFile.FileContent[PsaFile.DataSectionSizeBytes] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2];
                PsaFile.FileContent[PsaFile.DataSectionSizeBytes + 1] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 2] = FADEF00D; 
                PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 3] = FADEF00D;
                PsaFile.DataSectionSizeBytes += 2;
                return true;
            }
            else if (commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 3 > PsaFile.DataSectionSizeBytes)
            {
                PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 2] = FADEF00D;
                PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 3] = FADEF00D;
                PsaFile.DataSectionSizeBytes += 2;
                return true;
            }

            return false;
        }

        /// <summary>
        /// fixam
        /// </summary>
        public void ApplyFileUpdatesToAccountForActionChanges()
        {
            // Fixam (in PSA-C) method here -- I think this is what updates the global headers like filesize and stuff
            // TODO: Fixam logic has to be added for adding new command to action that had no previous commands as well
            // I don't really understand this part yet
            // bad variable name...
            // last data section value is where FADE 0D8A is found?
            int lastDataSectionValidValue = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == FADE0D8A 
                ? PsaFile.DataSectionSizeBytes - 3 
                : PsaFile.DataSectionSizeBytes - 1;

            // decrease last data section value while no fade foods are found ??
            while (lastDataSectionValidValue >= PsaFile.DataSectionSizeBytes && PsaFile.FileContent[lastDataSectionValidValue] == FADEF00D)
            {
                lastDataSectionValidValue--;
            }

            // not a clue what this does, maybe moves all the FADE F00DS over if there's free space?
            int newDataSectionSize = lastDataSectionValidValue;
            while (newDataSectionSize < PsaFile.DataSectionSizeBytes)
            {
                if (PsaFile.FileContent[lastDataSectionValidValue] != FADEF00D)
                {
                    PsaFile.FileContent[newDataSectionSize] = PsaFile.FileContent[lastDataSectionValidValue];
                    newDataSectionSize++;
                }
            }

            // change size of data section to match new size, which did change since a new command was added
            PsaFile.DataSectionSize = newDataSectionSize * 4;

            // this just threw me for a loop, WHAT is going on
            Array.Sort(PsaFile.OffsetInterlockTracker);

            for (int i = 0; i < PsaFile.NumberOfOffsetEntries; i++)
            {
                PsaFile.FileContent[newDataSectionSize] = PsaFile.OffsetInterlockTracker[i];
                newDataSectionSize++;
            }

            int movesetFileSizeLeftoverSpace = PsaFile.MovesetFileSize % 4;
            if (movesetFileSizeLeftoverSpace == 0)
            {
                movesetFileSizeLeftoverSpace = 4;
            }

            for (int i = 0; i < PsaFile.CompressedSize; i++)
            {
                PsaFile.FileContent[newDataSectionSize] = PsaFile.CompressionTracker[i];
                newDataSectionSize++;
            }

            PsaFile.MovesetFileSize = (newDataSectionSize * 4) + movesetFileSizeLeftoverSpace + 28;

            // NO CLUE what FileHeader[17] is, this is the only place in the entire PSAC that it's used
            // I'm guessing it just always needs to be the same as the MovesetFileSize at FileHeader[24]
            PsaFile.FileHeader[17] = PsaFile.MovesetFileSize;

            // this checks if moveset is now over 544kb, a limitation of PSAC that I will later remove
            int newMovesetFileSizeBytes = (PsaFile.MovesetFileSize + 3) / 4;
            if (newMovesetFileSizeBytes % 8 != 0)
            {
                newDataSectionSize = 8 - newMovesetFileSizeBytes % 8;
                newMovesetFileSizeBytes += newDataSectionSize;
            }
            newMovesetFileSizeBytes += PsaFile.ExtraSpace - 8;
            if (newMovesetFileSizeBytes > 139264)
            {
                Console.WriteLine("Current data size over 544kb");
            }
        }

        /// <summary>
        /// Searches through data section for the desired amount of free space
        /// </summary>
        /// <param name="amountOfFreeSpace">amount of free space desired (as doubleword, e.g. 4 would look for 4 doublewords)</param>
        /// <returns>starting location where the desired amount of free space has been found</returns>
        public int FindLocationWithAmountOfFreeSpace(int amountOfFreeSpace)
        {
            int openAreaStartLocation = GetOpenAreaStartLocation();
            int stoppingPoint = openAreaStartLocation;

            while (stoppingPoint < PsaFile.DataSectionSizeBytes)
            {
                if (PsaFile.FileContent[stoppingPoint] == FADEF00D)
                {
                    bool hasEnoughSpace = true;
                    for (int i = stoppingPoint + 1; i < amountOfFreeSpace; i++)
                    {
                        if (PsaFile.FileContent[i] != FADEF00D)
                        {
                            hasEnoughSpace = false;
                            break;
                        }
                    }
                    if (hasEnoughSpace)
                    {
                        return stoppingPoint;
                    }
                }
                stoppingPoint++;
            }
            return stoppingPoint;
        }

        /********************************
         * MODIFYING COMMAND IN ACTION  *
         * ******************************/
        /// <summary>
        /// modifies command to a new command in action
        /// </summary>
        /// <param name="actionId">id of action (0 = 112)</param>
        /// <param name="codeBlockId">id of codeblock (0 for Entry, 1 for Exit)</param>
        /// <param name="commandIndex">index number of command in action, starts at 0 -- ex: the third command in the action would be index 2</param>
        /// <param name="newPsaCommand">the new psa command to replace the old one with</param>
        public void ModifyActionCommand(int actionId, int codeBlockId, int commandIndex, PsaCommand newPsaCommand)
        {
            int commandLocation = GetActionCodeBlockCommandLocation(actionId, codeBlockId, commandIndex); // j

            if (PsaFile.FileContent[commandLocation + 1] >= 0 && PsaFile.FileContent[commandLocation + 1] < PsaFile.DataSectionSize)
            {
                // event modify method
                PsaCommand oldPsaCommand = GetPsaCommandForActionCodeBlock(actionId, codeBlockId, commandIndex);

                int oldCommandParamsSize = oldPsaCommand.GetCommandParamsSize(); // k

                // show dialog (pauses event modify method execution)
                // for this method, that whole process is replaced with the PsaCommand object parameter

                // if there is not currently an existing command params location (e.g. you are replacing a nop command which as no params)

                // if there were no command params on the previous command
                if (oldCommandParamsSize == 0)
                {
                    int newCommandParamsSize = newPsaCommand.GetCommandParamsSize(); // m

                    // if there are no command params on new command, it's simply a swap out of command instructions with nothing else needed
                    if (newCommandParamsSize == 0)
                    {
                        PsaFile.FileContent[commandLocation] = newPsaCommand.Instruction;
                    }

                    // if there are command params on new command, create space for this new params location
                    else
                    {
                        // ParamsModify method

                        // determine stopping point, which is where new command params will be added (finds room where number of params can all fit)
                        int stoppingPoint = FindLocationWithAmountOfFreeSpace(newCommandParamsSize);

                        // if adding command params location causes data to go beyond data section limit, increase data section size
                        if (stoppingPoint >= PsaFile.DataSectionSizeBytes)
                        {
                            stoppingPoint = PsaFile.DataSectionSizeBytes;
                            if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == FADE0D8A)
                            {
                                stoppingPoint -= 2;
                                PsaFile.FileContent[PsaFile.DataSectionSizeBytes + newCommandParamsSize - 2] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2];
                                PsaFile.FileContent[PsaFile.DataSectionSizeBytes + newCommandParamsSize - 1] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                            }
                            PsaFile.DataSectionSizeBytes += newCommandParamsSize;
                        }

                        int paramsIndex = 0;
                        for (int i = 0; i < newCommandParamsSize; i += 2)
                        {
                            // if command param type is "Pointer" and the param value is greater than 0 (meaning it points to something)
                            if (newPsaCommand.Parameters[paramsIndex].Type == 2 && newPsaCommand.Parameters[paramsIndex].Value > 0)
                            {
                                int valuePointerOffset = (stoppingPoint + i) * 4 + 4;
                                PsaFile.OffsetInterlockTracker[PsaFile.NumberOfOffsetEntries] = valuePointerOffset;
                                PsaFile.NumberOfOffsetEntries++;
                            }
                            PsaFile.FileContent[stoppingPoint + i] = newPsaCommand.Parameters[paramsIndex].Type;
                            PsaFile.FileContent[stoppingPoint + i + 1] = newPsaCommand.Parameters[paramsIndex].Value;
                            paramsIndex++;
                        }

                        // end ParamsModify method

                        PsaFile.FileContent[commandLocation] = newPsaCommand.Instruction;
                        int newCommandParamsLocation = stoppingPoint * 4;
                        PsaFile.FileContent[commandLocation + 1] = newCommandParamsLocation;
                        PsaFile.OffsetInterlockTracker[PsaFile.NumberOfOffsetEntries] = commandLocation * 4 + 4;
                        PsaFile.NumberOfOffsetEntries++;
                        ApplyFileUpdatesToAccountForActionChanges();
                    }
                }

                // if there are existing command params, they need to be overwritten, or the entire action may need to be relocated
                else
                {
                    for (int i = 0; i < oldCommandParamsSize; i += 2)
                    {
                        if (PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation + i] == 2)
                        {
                            // I believe this is the location of the actual value of a particular command param
                            int commandParamValueLocation = (oldPsaCommand.CommandParametersValuesLocation + i) * 4 + 4; // rmv

                            // Delasc method -- this checks if an offset already exists I think? Not quite sure
                            int iterator = 0;
                            bool existingOffsetFound = false;

                            while (iterator < PsaFile.NumberOfOffsetEntries)
                            {
                                if (PsaFile.OffsetInterlockTracker[iterator] == commandParamValueLocation)
                                {
                                    existingOffsetFound = true;
                                    break;
                                }
                                iterator++;
                            }

                            if (existingOffsetFound)
                            {
                                PsaFile.OffsetInterlockTracker[iterator] = 16777216; // 100 0000
                                PsaFile.NumberOfOffsetEntries--;
                            }

                            // end delasc

                            // this part is a long series of nested if statements...
                            // I can't figure out exactly what it does and can't get it to consistently trigger
                            if (iterator >= PsaFile.NumberOfOffsetEntries)
                            {
                                // something to do with external subroutines
                                if (i == 0)
                                {
                                    for (int j = 0; j < PsaFile.NumberOfExternalSubRoutines; j++) // j is mov
                                    {
                                        i = PsaFile.CompressionTracker[(PsaFile.NumberOfDataTableEntries + j) * 2];
                                        if (i > 8096 && i < PsaFile.DataSectionSize)
                                        {
                                            if (commandParamValueLocation == i)
                                            {
                                                oldPsaCommand.CommandParametersLocation = PsaFile.FileContent[commandLocation + 1] / 4 + 1; // rmv
                                                int temp = (PsaFile.NumberOfDataTableEntries + j) * 2; // not entirely sure what this is yet  :/
                                                if (PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] >= 8096 && PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] < PsaFile.DataSectionSize)
                                                {
                                                    if (PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] % 4 == 0)
                                                    {
                                                        PsaFile.CompressionTracker[temp] = PsaFile.FileContent[oldPsaCommand.CommandParametersLocation];
                                                    }
                                                    else
                                                    {
                                                        PsaFile.CompressionTracker[temp] = -1;
                                                    }
                                                }
                                                else
                                                {
                                                    PsaFile.CompressionTracker[temp] = -1;
                                                }
                                                oldPsaCommand.CommandParametersLocation = 0;
                                                break;
                                            }
                                            if (i >= 8096 && i < PsaFile.DataSectionSize)
                                            {
                                                for (int k = 0; k < 100; k++) // k is an1
                                                {
                                                    // clearly I'm not sure what location this represents
                                                    int somethingLocation = i / 4;

                                                    i = PsaFile.FileContent[somethingLocation];
                                                    if (i < 8096 || i >= PsaFile.DataSectionSize)
                                                    {
                                                        break;
                                                    }
                                                    if (oldPsaCommand.CommandParametersLocation == i)
                                                    {
                                                        oldPsaCommand.CommandParametersLocation = PsaFile.FileContent[commandLocation + 1] / 4 + 1;
                                                        if (PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] >= 8096 && PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] < PsaFile.DataSectionSize)
                                                        {
                                                            if (PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] % 4 == 0)
                                                            {
                                                                PsaFile.CompressionTracker[somethingLocation] = PsaFile.FileContent[oldPsaCommand.CommandParametersLocation];
                                                            }
                                                            else
                                                            {
                                                                PsaFile.CompressionTracker[somethingLocation] = -1;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            PsaFile.CompressionTracker[somethingLocation] = -1;
                                                        }
                                                        oldPsaCommand.CommandParametersLocation = 0;
                                                        break;
                                                    }
                                                }
                                                if (oldPsaCommand.CommandParametersLocation == 0)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    i = 0;
                                }
                                else if (oldCommandParamsSize == 4 && i == 2)
                                {
                                    for (int j = 0; j < PsaFile.NumberOfExternalSubRoutines; j++) // j is mov
                                    {
                                        i = PsaFile.CompressionTracker[(PsaFile.NumberOfDataTableEntries + j) * 2];
                                        if (i > 8096 && i < PsaFile.DataSectionSize)
                                        {
                                            if (commandParamValueLocation == i)
                                            {
                                                oldPsaCommand.CommandParametersLocation = PsaFile.FileContent[commandLocation + 1] / 4 + 3; // rmv
                                                int temp = (PsaFile.NumberOfDataTableEntries + j) * 2; // not entirely sure what this is yet  :/
                                                if (PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] >= 8096 && PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] < PsaFile.DataSectionSize)
                                                {
                                                    if (PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] % 4 == 0)
                                                    {
                                                        PsaFile.CompressionTracker[temp] = PsaFile.FileContent[oldPsaCommand.CommandParametersLocation];
                                                    }
                                                    else
                                                    {
                                                        PsaFile.CompressionTracker[temp] = -1;
                                                    }
                                                }
                                                else
                                                {
                                                    PsaFile.CompressionTracker[temp] = -1;
                                                }
                                                oldPsaCommand.CommandParametersLocation = 0;
                                                break;
                                            }
                                            if (i >= 8096 && i < PsaFile.DataSectionSize)
                                            {
                                                for (int k = 0; k < 100; k++) // k is an1
                                                {
                                                    // clearly I'm not sure what location this represents
                                                    int somethingLocation = i / 4;

                                                    i = PsaFile.FileContent[somethingLocation];
                                                    if (i < 8096 || i >= PsaFile.DataSectionSize)
                                                    {
                                                        break;
                                                    }
                                                    if (oldPsaCommand.CommandParametersLocation == i)
                                                    {
                                                        oldPsaCommand.CommandParametersLocation = PsaFile.FileContent[commandLocation + 1] / 4 + 1;
                                                        if (PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] >= 8096 && PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] < PsaFile.DataSectionSize)
                                                        {
                                                            if (PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] % 4 == 0)
                                                            {
                                                                PsaFile.CompressionTracker[somethingLocation] = PsaFile.FileContent[oldPsaCommand.CommandParametersLocation];
                                                            }
                                                            else
                                                            {
                                                                PsaFile.CompressionTracker[somethingLocation] = -1;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            PsaFile.CompressionTracker[somethingLocation] = -1;
                                                        }
                                                        oldPsaCommand.CommandParametersLocation = 0;
                                                        break;
                                                    }
                                                }
                                                if (oldPsaCommand.CommandParametersLocation == 0)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    i = 2;
                                }
                            }

                        }
                        PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation + i] = FADEF00D;
                        PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation + i + 1] = FADEF00D;
                    }
                    int newCommandParamsSize = newPsaCommand.GetCommandParamsSize(); // m
                    if (newCommandParamsSize == 0)
                    {
                        PsaFile.FileContent[commandLocation] = newPsaCommand.Instruction;
                        PsaFile.FileContent[commandLocation + 1] = 0;

                        // I believe this is the location of the actual value of a particular command param
                        int commandParamValueLocation = commandLocation * 4 + 4; // rmv

                        // Delasc method -- this checks if an offset already exists I think? Not quite sure
                        int iterator = 0;
                        bool existingOffsetFound = false;

                        while (iterator < PsaFile.NumberOfOffsetEntries)
                        {
                            if (PsaFile.OffsetInterlockTracker[iterator] == commandParamValueLocation) // rmv
                            {
                                existingOffsetFound = true;
                                break;
                            }
                            iterator++;
                        }

                        if (existingOffsetFound)
                        {
                            PsaFile.OffsetInterlockTracker[iterator] = 16777216; // 100 0000
                            PsaFile.NumberOfOffsetEntries--;
                        }

                        // end delasc

                        // fixam
                        ApplyFileUpdatesToAccountForActionChanges();
                    }
                    // resize command params if new command has more params than the one it is replacing
                    else if (newCommandParamsSize > oldCommandParamsSize)
                    {
                        // figure out how much space is in the current location for command params (increases if it finds some trailing FADEF00D spots)
                        int currentCommandParamSize = oldCommandParamsSize; // i
                        while (currentCommandParamSize < newCommandParamsSize && PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation + currentCommandParamSize] == FADEF00D)
                        {
                            currentCommandParamSize++;
                        }
                        // resize data section if no more room for command params
                        if (oldPsaCommand.CommandParametersValuesLocation + oldCommandParamsSize + 5 > PsaFile.DataSectionSize)
                        {
                            int commandSizeDifference = newCommandParamsSize - oldCommandParamsSize;
                            if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == FADE0D8A)
                            {
                                PsaFile.FileContent[PsaFile.DataSectionSizeBytes + commandSizeDifference - 2] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2];
                                PsaFile.FileContent[PsaFile.DataSectionSizeBytes + commandSizeDifference - 1] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                                PsaFile.DataSectionSizeBytes += commandSizeDifference;
                                currentCommandParamSize = newCommandParamsSize;
                            }
                            else if (oldPsaCommand.CommandParametersValuesLocation + oldCommandParamsSize + 3 > PsaFile.DataSectionSizeBytes)
                            {
                                PsaFile.DataSectionSizeBytes += commandSizeDifference;
                                currentCommandParamSize = newCommandParamsSize;
                            }
                        }
                        // if new command params size is less than what's already there, create some FADEF00DS to represent free space that can later be used if needed
                        if (currentCommandParamSize < newCommandParamsSize)
                        {
                            int opeanAreaStartLocation = GetOpenAreaStartLocation();
                            oldPsaCommand.CommandParametersValuesLocation = opeanAreaStartLocation;
                            int bitStoppingPoint = 0;

                            while (oldPsaCommand.CommandParametersValuesLocation < PsaFile.DataSectionSizeBytes && bitStoppingPoint != oldPsaCommand.CommandParametersValuesLocation + newCommandParamsSize)
                            {
                                if (PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation] == FADEF00D)
                                {
                                    bitStoppingPoint = oldPsaCommand.CommandParametersValuesLocation + 1;
                                    while (bitStoppingPoint <= oldPsaCommand.CommandParametersValuesLocation + newCommandParamsSize && PsaFile.FileContent[bitStoppingPoint] != FADEF00D)
                                    {
                                        if (PsaFile.FileContent[bitStoppingPoint] != FADEF00D)
                                        {
                                            oldPsaCommand.CommandParametersValuesLocation = bitStoppingPoint;
                                            break;
                                        }
                                        bitStoppingPoint++;
                                    }
                                }
                                oldPsaCommand.CommandParametersValuesLocation++;
                            }
                            if (oldPsaCommand.CommandParametersValuesLocation >= PsaFile.DataSectionSizeBytes)
                            {
                                oldPsaCommand.CommandParametersValuesLocation = PsaFile.DataSectionSizeBytes;
                                if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == FADE0D8A)
                                {
                                    oldPsaCommand.CommandParametersValuesLocation -= 2;
                                    PsaFile.FileContent[PsaFile.DataSectionSizeBytes + newCommandParamsSize - 2] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2];
                                    PsaFile.FileContent[PsaFile.DataSectionSizeBytes + newCommandParamsSize - 1] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                                }
                                PsaFile.DataSectionSizeBytes += newCommandParamsSize;
                            }
                            PsaFile.FileContent[commandLocation + 1] = oldPsaCommand.CommandParametersValuesLocation * 4;
                        }
                    }
                    PsaFile.FileContent[commandLocation] = newPsaCommand.Instruction;
                    for (int i = 0; i < newCommandParamsSize; i += 2)
                    {
                        int paramsIndex = i / 2;
                        // if command param type is Pointer and it actually points to something
                        if (newPsaCommand.Parameters[paramsIndex].Type == 2 && newPsaCommand.Parameters[paramsIndex].Value > 0)
                        {
                            int something = (oldPsaCommand.CommandParametersValuesLocation + i) * 4 + 4;
                            PsaFile.OffsetInterlockTracker[PsaFile.NumberOfOffsetEntries] = something;
                            PsaFile.NumberOfOffsetEntries++;
                        }

                        PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation + i] = newPsaCommand.Parameters[paramsIndex].Type;
                        PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation + i + 1] = newPsaCommand.Parameters[paramsIndex].Value;
                    }

                    // in psac, this is only called if "fnt" is 1, which means something was changed -- will see if this will break things or not to just call every time
                    ApplyFileUpdatesToAccountForActionChanges();
                }

                
                // end EventModify method
                if (oldPsaCommand.CommandParametersLocation == -1)
                {
                    // I don't think I need any of this
                }
            }
            else
            {
                throw new ApplicationException("Cannot modify event...not sure why just can't okay");
            }

        }
    }
}
