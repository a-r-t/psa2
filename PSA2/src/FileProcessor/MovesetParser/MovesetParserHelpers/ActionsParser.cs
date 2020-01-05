using PSA2.src.FileProcessor.MovesetParser.Configs;
using PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.ActionsParserHelpers;
using PSA2.src.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers
{
    public class ActionsParser
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public Dictionary<string, PsaCommandConfig> PsaCommands { get; private set; }
        public PsaCommandParser PsaCommandParser { get; private set; }
        public CharacterSpecificParametersConfig CharacterSpecificParametersConfig { get; private set; }

        public ActionsParser(PsaFile psaFile, int dataSectionLocation, string movesetBaseName)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            PsaCommandsConfig PsaCommandsConfig = Utils.LoadJson<PsaCommandsConfig>("data/psa_command_data.json");
            PsaCommands = PsaCommandsConfig.PsaCommands.ToDictionary(command => command.Instruction, command => command);
            PsaCommandParser = new PsaCommandParser(PsaFile);
            CharacterSpecificParametersConfig = Utils.LoadJson<CharacterSpecificParametersConfig>($"data/char_specific/{movesetBaseName}.json");
        }

        public int GetNumberOfSpecialActions()
        {
            Console.WriteLine(String.Format("Number of Special Actions: {0}", (PsaFile.FileContent[DataSectionLocation + 10] - PsaFile.FileContent[DataSectionLocation + 9]) / 4));
            return (PsaFile.FileContent[DataSectionLocation + 10] - PsaFile.FileContent[DataSectionLocation + 9]) / 4;
        }

        public int GetNumberOfSubActions()
        {
            Console.WriteLine(String.Format("Number of Sub Actions: {0}", (PsaFile.FileContent[DataSectionLocation + 13] - PsaFile.FileContent[DataSectionLocation + 12]) / 4));
            return (PsaFile.FileContent[DataSectionLocation + 13] - PsaFile.FileContent[DataSectionLocation + 12]) / 4;
        }

        public int GetTotalNumberOfActions()
        {
            return 274 + GetNumberOfSpecialActions();
        }

        // this is the offset where action code starts (displayed in PSAC)
        public int GetActionCodeBlockLocation(int actionId, int codeBlockId)
        {
            int actionsCodeBlockStartingLocation = codeBlockId == 0 ?
                PsaFile.FileContent[DataSectionLocation + 9] / 4 :
                PsaFile.FileContent[DataSectionLocation + 10] / 4;

            int actionCodeBlockLocation = PsaFile.FileContent[actionsCodeBlockStartingLocation + actionId];

            return actionCodeBlockLocation;
        }


        public List<PsaCommand> GetPsaCommandsForAction(int actionId, int codeBlockId)
        {
            List <PsaCommand> psaCommands = new List<PsaCommand>();
            int actionCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            int actionCodeBlockInstructionsStartLocation = actionCodeBlockLocation / 4;
            if (actionCodeBlockInstructionsStartLocation < PsaFile.DataSectionSize) // and greater than "stf" whatever that means"
            {
                int nextActionCodeBlockInstructionLocation = actionCodeBlockInstructionsStartLocation;
                while (PsaFile.FileContent[nextActionCodeBlockInstructionLocation] != 0 && 
                    nextActionCodeBlockInstructionLocation < PsaFile.DataSectionSize)
                {
                    psaCommands.Add(PsaCommandParser.GetPsaInstruction(nextActionCodeBlockInstructionLocation));
                    nextActionCodeBlockInstructionLocation += 2;
                }
            }


            return psaCommands;
        }

        // this is the offset where subaction code starts (displayed in PSAC)
        public int GetSubActionCodeBlockLocation(int subActionId, int codeBlockId)
        {
            int subActionsCodeBlockStartingLocation = PsaFile.FileContent[DataSectionLocation + 12 + codeBlockId] / 4; // n
            int subActionCodeBlockLocation = PsaFile.FileContent[subActionsCodeBlockStartingLocation + subActionId];

            return subActionCodeBlockLocation;
        }


        public List<PsaCommand> GetPsaCommandsForSubAction(int subActionId, int codeBlockId)
        {
            List<PsaCommand> psaCommands = new List<PsaCommand>();
            int subActionCodeBlockLocation = GetSubActionCodeBlockLocation(subActionId, codeBlockId); // i
            int subActionCodeBlockInstructionsStartLocation = subActionCodeBlockLocation / 4; // j
            if (subActionCodeBlockInstructionsStartLocation < PsaFile.DataSectionSize) // and greater than "stf" whatever that means"
            {
                int nextSubActionCodeBlockInstructionLocation = subActionCodeBlockInstructionsStartLocation;
                while (PsaFile.FileContent[nextSubActionCodeBlockInstructionLocation] != 0 &&
                    nextSubActionCodeBlockInstructionLocation < PsaFile.DataSectionSize)
                {
                    psaCommands.Add(PsaCommandParser.GetPsaInstruction(nextSubActionCodeBlockInstructionLocation));
                    nextSubActionCodeBlockInstructionLocation += 2;
                }
            }

            return psaCommands;
        }

        public string GetSubActionAnimationName(int subActionId)
        {
            // h = subActionId
            // not 100% sure why this chain works but it does
            int animationLocation = PsaFile.FileContent[DataSectionLocation] / 4 + 1 + subActionId * 2;
            if (PsaFile.FileContent[animationLocation] == 0)
            {
                return "NONE";
            }
            else
            {
                int animationNameLocation = PsaFile.FileContent[animationLocation] / 4; // j

                if (animationNameLocation < PsaFile.DataSectionSize) // and animationNameLocation >= stf whatever that means
                {
                    StringBuilder animationName = new StringBuilder();
                    int nameEndByteIndex = 0;
                    while (true)
                    {
                        string nextStringData = Utils.ConvertWordToString(PsaFile.FileContent[animationNameLocation + nameEndByteIndex]);
                        animationName.Append(nextStringData);
                        if (nextStringData.Length == 4)
                        {
                            nameEndByteIndex++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    Console.WriteLine(animationName.ToString());
                    return animationName.ToString();
                }

                return "ERROR";
            }

        }

        public AnimationFlags GetSubActionAnimationFlags(int subActionId)
        {
            // will need to look at this later to figure out why it works
            int animationFlagsLocation = PsaFile.FileContent[DataSectionLocation] / 4 + subActionId * 2;
            int animationFlagsValue = PsaFile.FileContent[animationFlagsLocation];
            int inTransition = (animationFlagsValue >> 24) & 0xFF;
            int noOutTransition = animationFlagsValue & 0x1;
            int loop = ((animationFlagsValue >> 16) & 0xFF) & 0x2;
            int movesCharacter = ((animationFlagsValue >> 16) & 0xFF) & 0x4;
            int unknown3 = ((animationFlagsValue >> 16) & 0xFF) & 0x8;
            int unknown4 = ((animationFlagsValue >> 16) & 0xFF) & 0x10;
            int unknown5 = ((animationFlagsValue >> 16) & 0xFF) & 0x20;
            int transitionOutFromStart = ((animationFlagsValue >> 16) & 0xFF) & 0x40;
            int unknown7 = ((animationFlagsValue >> 16) & 0xFF) & 0x80;
            return new AnimationFlags(inTransition, noOutTransition, loop, movesCharacter, unknown3, unknown4, unknown5, transitionOutFromStart, unknown7);
        }

        public List<PsaCommand> GetPsaCommandsForSubRoutine(int subRoutineLocation)
        {
            List<PsaCommand> psaCommands = new List<PsaCommand>();
            int subRoutineCodeStartLocation = subRoutineLocation / 4;
            if (subRoutineCodeStartLocation > 0 && subRoutineCodeStartLocation < PsaFile.DataSectionSize) // and greater than "stf" whatever that means"
            {
                int nextSubRoutineInstructionLocation = subRoutineCodeStartLocation;
                while (PsaFile.FileContent[nextSubRoutineInstructionLocation] != 0 &&
                    nextSubRoutineInstructionLocation < PsaFile.DataSectionSize)
                {
                    psaCommands.Add(PsaCommandParser.GetPsaInstruction(nextSubRoutineInstructionLocation));
                    nextSubRoutineInstructionLocation += 2;
                }
            }
            return psaCommands;
        }

        public List<int> GetAllSubRoutines()
        {
            HashSet<int> subRoutines = new HashSet<int>();
            int numberOfSpecialActions = GetNumberOfSpecialActions();
            int numberOfSubActions = GetNumberOfSubActions();

            // find all subroutines in all actions
            for (int i = 0; i < numberOfSpecialActions; i++)
            {
                for (int j = 0; j < 2; j++) {
                    List<PsaCommand> commands = GetPsaCommandsForAction(i, j);
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
                    List<PsaCommand> commands = GetPsaCommandsForSubAction(i, j);
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
                                subRoutinesList.Add(command.Parameters[i].Value);
                            }
                        }
                    }
                }
            }

            HashSet<int> subRoutinesNested = subRoutinesList.ToHashSet<int>();
            subRoutinesNested.RemoveWhere(emptySubRoutines.Contains);
            //subRoutinesNested.ToList().OrderBy(x => x.ToString("X8")).ToList().ForEach(s => Console.WriteLine(s.ToString("X8")));
            return subRoutinesNested.ToList().OrderBy(x => x.ToString("X8")).ToList();
        }

        public List<int> GetAllActionOverrides(int codeBlockId)
        {
            List<int> actionOverrideIds = new List<int>();
            if (codeBlockId == 3)
            {
                int totalNumberOfActions = GetTotalNumberOfActions();
                for (int i = 0; i < totalNumberOfActions; i++)
                {
                    actionOverrideIds.Add(i);
                }
                // pre action overrides
                return actionOverrideIds;
            }
            else
            {

                int actionOverridesLocation;
                if (codeBlockId == 0)
                {
                    actionOverridesLocation = PsaFile.FileContent[DataSectionLocation + 20] / 4;
                }
                else // if (codeBlockId == 1)
                {
                    actionOverridesLocation = PsaFile.FileContent[DataSectionLocation + 21] / 4;
                }

                int nextActionOverridesLocation = actionOverridesLocation;
                while (PsaFile.FileContent[nextActionOverridesLocation] >= 0)
                {
                    actionOverrideIds.Add(PsaFile.FileContent[nextActionOverridesLocation]);
                    nextActionOverridesLocation += 2;
                }
                actionOverrideIds.ForEach(x => Console.WriteLine(x.ToString("X")));
                return actionOverrideIds;
            }

        }

        // this is the offset where action override code starts (displayed in PSAC)
        public int GetActionOverrideCodeBlockLocation(int actionOverrideId, int codeBlockId) // issue -- actionOverrideId needs to be in order of existence
        {
            int actionOverridesCodeBlockStartingLocation = codeBlockId == 0 ?
                PsaFile.FileContent[DataSectionLocation + 20] / 4 :
                PsaFile.FileContent[DataSectionLocation + 21] / 4;

            int actionOverrideCodeBlockLocation = PsaFile.FileContent[actionOverridesCodeBlockStartingLocation + actionOverrideId * 2 + 1];

            return actionOverrideCodeBlockLocation;
        }

        public List<PsaCommand> GetPsaCommandsForActionOverride(int codeBlockId, int actionOverrideId)
        {
            List<PsaCommand> psaCommands = new List<PsaCommand>();
            int actionOverrideCodeBlockLocation = GetActionOverrideCodeBlockLocation(actionOverrideId, codeBlockId);
            int actionOverrideCodeBlockInstructionsStartLocation = actionOverrideCodeBlockLocation / 4;
            if (actionOverrideCodeBlockInstructionsStartLocation < PsaFile.DataSectionSize) // and greater than "stf" whatever that means"
            {
                int nextActionOverrideCodeBlockInstructionLocation = actionOverrideCodeBlockInstructionsStartLocation;
                while (PsaFile.FileContent[nextActionOverrideCodeBlockInstructionLocation] != 0 &&
                    nextActionOverrideCodeBlockInstructionLocation < PsaFile.DataSectionSize)
                {
                    psaCommands.Add(PsaCommandParser.GetPsaInstruction(nextActionOverrideCodeBlockInstructionLocation));
                    nextActionOverrideCodeBlockInstructionLocation += 2;
                }
            }
            return psaCommands;
        }

        public int GetNumberOfArticles()
        {
            return CharacterSpecificParametersConfig.Articles.Count;
        }

        public int GetArticleDataLocation(int articleId)
        {
            int articleLocation = Convert.ToInt32(CharacterSpecificParametersConfig.Articles[articleId].Location, 16);
            return PsaFile.FileContent[DataSectionLocation + articleLocation / 4] / 4;

            // to get article offset, multiple the result of this by 4
            // ArtOffset.Text = "0x" + (artdt * 4).ToString("X");

        }

        public int GetNumberOfArticleActions(int articleId)
        {
            int articleDataLocation = GetArticleDataLocation(articleId);
            if (PsaFile.FileContent[articleDataLocation + 5] > 8096 && PsaFile.FileContent[articleDataLocation + 5] < PsaFile.DataSectionSize)
            {
                return CharacterSpecificParametersConfig.Articles[articleId].NumberOfActions;
            }
            else
            {
                return 0;
            }
        }

        public int GetNumberOfArticleSubActions(int articleId)
        {
            int articleDataLocation = GetArticleDataLocation(articleId);
            if (PsaFile.FileContent[articleDataLocation + 4] > 8096 && PsaFile.FileContent[articleDataLocation + 4] < PsaFile.DataSectionSize)
            {
                return CharacterSpecificParametersConfig.Articles[articleId].NumberOfSubActions;
            }
            else
            {
                return 0;
            }
        }

        // the psac code for this showed that technically a "Main" and "SFX" code block can exist without a "GFX",
        // which is weird and I've never seen it, but better to stay safe and match it for now
        // also subactions in an article share the same codeblocks
        public List<int> GetCodeBlockIdsForArticleSubActions(int articleId)
        {
            List<int> codeBlockIds = new List<int>();
            int articleDataLocation = GetArticleDataLocation(articleId);
            for (int i = 0; i < 3; i++)
            {
                if (PsaFile.FileContent[articleDataLocation + i + 6] > 8096 && PsaFile.FileContent[articleDataLocation + i + 6] < PsaFile.DataSectionSize)
                {
                    codeBlockIds.Add(i);
                }
            }
            codeBlockIds.ForEach(Console.WriteLine);
            return codeBlockIds;

            /*
               Original Code:

                if (h >= 1 && alm[artdt + 4] > 8096 && alm[artdt + 4] < tds[25])
                {
                    for (k = 0; k < h; k++)
                    {
                        ArtSubaId.Items.Add(k.ToString("X"));
                    }
                    if (alm[artdt + 6] > 8096 && alm[artdt + 6] < tds[25])
                    {
                        ArtSubaCbList.Items.Add("Main");
                    }
                    if (alm[artdt + 7] > 8096 && alm[artdt + 7] < tds[25])
                    {
                        ArtSubaCbList.Items.Add("GFX");
                    }
                    if (alm[artdt + 8] > 8096 && alm[artdt + 8] < tds[25])
                    {
                        ArtSubaCbList.Items.Add("SFX");
                    }
                }

            */
        }

        // starts at 0x000 and goes up from there -- 0x004, 0x008, etc
        public Dictionary<string, string> GetArticleOffsets(int articleId)
        {
            List<string> offsetNames = new List<string>
            {
                "ArticleGroupId",
                "ARCEntryGroup",
                "Bone",
                "ActionFlags",
                "SubActionFlags",
                "Actions",
                "SubActionMain",
                "SubActionGFX",
                "SubActionSFX",
                "ModelVisibility",
                "CollisionData",
                "Data2",
            };
            Dictionary<string, string> articleOffsets = new Dictionary<string, string>();
            int articleDataLocation = GetArticleDataLocation(articleId);
            for (int i = 0; i < offsetNames.Count; i++)
            {
                articleOffsets.Add(offsetNames[i], Utils.ConvertIntToOffset(PsaFile.FileContent[articleDataLocation + i]));
            }
            foreach (KeyValuePair<string, string> pair in articleOffsets)
            {
                Console.WriteLine(String.Format("{0}:{1}", pair.Key, pair.Value));
            }
            return articleOffsets;
        }

        public List<int> GetArticleParameterValues(int articleId, int paramsId)
        {
            List<int> articleParameterValues = new List<int>();
            int articleDataLocation = GetArticleDataLocation(articleId);
            int paramsLocation = Convert.ToInt32(CharacterSpecificParametersConfig.Articles[articleId].ArticleParameters[paramsId].Location, 16);
            int articleParametersDataLocation = PsaFile.FileContent[articleDataLocation + paramsLocation / 4] / 4; // artpo

            /*
            int m = articleParametersLocation + 3;
            int n = articleParametersLocation + 1023;
            if (n > PsaFile.DataSectionSize)
            {
                n = PsaFile.DataSectionSize;
            }
            */

            int numberOfParamValues = CharacterSpecificParametersConfig.Articles[articleId].ArticleParameters[paramsId].Values.Count;
            for (int i = 0; i < numberOfParamValues; i++)
            {
                articleParameterValues.Add(PsaFile.FileContent[articleParametersDataLocation + i]);
            }
            articleParameterValues.ForEach(x => Console.WriteLine(x.ToString("X")));
            return articleParameterValues;

        }

        // this is the offset where article action code starts (displayed in PSAC)
        public int GetArticleActionCodeLocation(int articleId, int actionId)
        {
            int articleDataLocation = GetArticleDataLocation(articleId);
            int articleActionCodeStartingLocation = PsaFile.FileContent[articleDataLocation + 5] / 4; // n -- also in one rare case for Pikmin, + 13 is its exit code block...worry about that later
            int articleActionCodeLocation = PsaFile.FileContent[articleActionCodeStartingLocation + actionId];
            return articleActionCodeLocation;
        }

        public List<PsaCommand> GetPsaCommandsForArticleAction(int articleId, int actionId)
        {
            List<PsaCommand> psaCommands = new List<PsaCommand>();
            int articleActionCodeLocation = GetArticleActionCodeLocation(articleId, actionId);
            int articleCommandsStartLocation = articleActionCodeLocation / 4; // j
            if (articleCommandsStartLocation < PsaFile.DataSectionSize) // and greater than "stf" whatever that means"
            {
                int nextArticleCommandsLocation = articleCommandsStartLocation;
                while (PsaFile.FileContent[nextArticleCommandsLocation] != 0 &&
                    nextArticleCommandsLocation < PsaFile.DataSectionSize)
                {
                    psaCommands.Add(PsaCommandParser.GetPsaInstruction(nextArticleCommandsLocation));
                    nextArticleCommandsLocation += 2;
                }
            }
            return psaCommands;
        }

        // this is the offset where article subaction code starts (displayed in PSAC)
        public int GetArticleSubActionCodeBlockLocation(int articleId, int subActionId, int codeBlockId)
        {
            int articleDataLocation = GetArticleDataLocation(articleId);
            int articleSubActionsCodeBlockStartingLocation = PsaFile.FileContent[articleDataLocation + 6 + codeBlockId] / 4; // k
            int articleSubActionCodeBlockLocation = PsaFile.FileContent[articleSubActionsCodeBlockStartingLocation + subActionId];

            return articleSubActionCodeBlockLocation;
        }

        public List<PsaCommand> GetPsaCommandsForArticleSubAction(int articleId, int subActionId, int codeBlockId)
        {
            List<PsaCommand> psaCommands = new List<PsaCommand>();
            int articleSubActionCodeBlockLocation = GetArticleSubActionCodeBlockLocation(articleId, subActionId, codeBlockId); // i
            int articleSubActionCodeBlockInstructionsStartLocation = articleSubActionCodeBlockLocation / 4; // j
            if (articleSubActionCodeBlockInstructionsStartLocation < PsaFile.DataSectionSize) // and greater than "stf" whatever that means"
            {
                int nextarticleSubActionCodeBlockInstructionLocation = articleSubActionCodeBlockInstructionsStartLocation;
                while (PsaFile.FileContent[nextarticleSubActionCodeBlockInstructionLocation] != 0 &&
                    nextarticleSubActionCodeBlockInstructionLocation < PsaFile.DataSectionSize)
                {
                    psaCommands.Add(PsaCommandParser.GetPsaInstruction(nextarticleSubActionCodeBlockInstructionLocation));
                    nextarticleSubActionCodeBlockInstructionLocation += 2;
                }
            }

            return psaCommands;
        }

        public string GetArticleSubActionAnimationName(int articleId, int subActionId)
        {
            // i = subActionId
            // not 100% sure why this chain works but it does
            int articleDataLocation = GetArticleDataLocation(articleId);
            int animationLocation = PsaFile.FileContent[articleDataLocation + 4] / 4 + 1 + subActionId * 2;
            if (PsaFile.FileContent[animationLocation] == 0)
            {
                return "NONE";
            }
            else
            {
                int animationNameLocation = PsaFile.FileContent[animationLocation] / 4; // j

                if (animationNameLocation < PsaFile.DataSectionSize) // and animationNameLocation >= stf whatever that means
                {
                    StringBuilder animationName = new StringBuilder();
                    int nameEndByteIndex = 0;
                    while (true)
                    {
                        string nextStringData = Utils.ConvertWordToString(PsaFile.FileContent[animationNameLocation + nameEndByteIndex]);
                        animationName.Append(nextStringData);
                        if (nextStringData.Length == 4)
                        {
                            nameEndByteIndex++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    Console.WriteLine(animationName.ToString());
                    return animationName.ToString();
                }

                return "ERROR";
            }

        }

        public AnimationFlags GetArticleSubActionAnimationFlags(int articleId, int subActionId)
        {
            // will need to look at this later to figure out why it works
            int articleDataLocation = GetArticleDataLocation(articleId);
            int animationFlagsLocation = PsaFile.FileContent[articleDataLocation + 4] / 4 + subActionId * 2;
            int animationFlagsValue = PsaFile.FileContent[animationFlagsLocation];
            int inTransition = (animationFlagsValue >> 24) & 0xFF;
            int noOutTransition = animationFlagsValue & 0x1;
            int loop = ((animationFlagsValue >> 16) & 0xFF) & 0x2;
            int movesCharacter = ((animationFlagsValue >> 16) & 0xFF) & 0x4;
            int unknown3 = ((animationFlagsValue >> 16) & 0xFF) & 0x8;
            int unknown4 = ((animationFlagsValue >> 16) & 0xFF) & 0x10;
            int unknown5 = ((animationFlagsValue >> 16) & 0xFF) & 0x20;
            int transitionOutFromStart = ((animationFlagsValue >> 16) & 0xFF) & 0x40;
            int unknown7 = ((animationFlagsValue >> 16) & 0xFF) & 0x80;
            return new AnimationFlags(inTransition, noOutTransition, loop, movesCharacter, unknown3, unknown4, unknown5, transitionOutFromStart, unknown7);
        }

        // the result of this * 4 is the offset for the params that shows in PSA when you go to No Select and look at Data Table
        public int GetCharacterParametersDataLocation(int paramsId)
        {
            int paramsLocation = Convert.ToInt32(CharacterSpecificParametersConfig.Parameters[paramsId].Location, 16);
            return PsaFile.FileContent[DataSectionLocation + paramsLocation / 4] / 4;
        }

        // formally known as the "No Selects", I don't believe these are article related at all
        // or at least related to an instance of an article or anything
        // I think it's parameters set for a character as a whole
        public List<int> GetCharacterParameterValues(int paramsId)
        {
            List<int> characterParameterValues = new List<int>();
            int characterParametersDataLocation = GetCharacterParametersDataLocation(paramsId);
            Console.WriteLine(characterParametersDataLocation.ToString("X"));

            int numberOfParamValues = CharacterSpecificParametersConfig.Parameters[paramsId].Values.Count;
            for (int i = 0; i < numberOfParamValues; i++)
            {
                characterParameterValues.Add(PsaFile.FileContent[characterParametersDataLocation + i]);
            }
            characterParameterValues.ForEach(x => Console.WriteLine(x.ToString("X")));
            return characterParameterValues;
        }

        // I guess a series of locations need to be followed to get to these "extra" params, and some characters (like Kirby) have A LOT of them and may have even more locations?
        // idk, it works though
        // also the result of this * 4 is the offset for the params that shows in PSA when you go to No Select and look at Data Table
        public int GetCharacterExtraParametersDataLocation(int paramsId)
        {
            int numberOfLocations = CharacterSpecificParametersConfig.ExtraParameters[paramsId].ParameterLocationPath.Count;
            int nextStartLocation = DataSectionLocation;
            int valuesDataLocation = 0;
            for (int i = 0; i < numberOfLocations; i++)
            {
                int nextValuesLocation = Convert.ToInt32(CharacterSpecificParametersConfig.ExtraParameters[paramsId].ParameterLocationPath[i], 16);
                valuesDataLocation = PsaFile.FileContent[nextStartLocation + nextValuesLocation] / 4;
                nextStartLocation = valuesDataLocation;
            }

            Console.WriteLine(valuesDataLocation);
            return valuesDataLocation;
        }

        public List<int> GetCharacterExtraParameterValues(int paramsId)
        {
            List<int> characterExtraParameterValues = new List<int>();
            int characterExtraParametersDataLocation = GetCharacterExtraParametersDataLocation(paramsId);
            Console.WriteLine((characterExtraParametersDataLocation * 4).ToString("X"));
            int numberOfParamValues = CharacterSpecificParametersConfig.Parameters[paramsId].Values.Count;
            for (int i = 0; i < numberOfParamValues; i++)
            {
                characterExtraParameterValues.Add(PsaFile.FileContent[characterExtraParametersDataLocation + i]);
            }
            characterExtraParameterValues.ForEach(x => Console.WriteLine(x.ToString("X")));
            return characterExtraParameterValues;
        }
    }
}
