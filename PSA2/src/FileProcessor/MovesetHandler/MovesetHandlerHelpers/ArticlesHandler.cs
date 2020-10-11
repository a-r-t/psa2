using PSA2.src.FileProcessor.MovesetHandler.Configs;
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    public class ArticlesHandler
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public PsaCommandHandler PsaCommandHandler { get; private set; }
        public CharacterSpecificParametersConfig CharacterSpecificParametersConfig { get; private set; }

        public ArticlesHandler(PsaFile psaFile, int dataSectionLocation, string movesetBaseName, PsaCommandHandler psaCommandHandler)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            PsaCommandHandler = psaCommandHandler;
            CharacterSpecificParametersConfig = Utils.LoadJson<CharacterSpecificParametersConfig>($"data/char_specific/{movesetBaseName}.json");
        }

        public int GetNumberOfArticles()
        {
            return CharacterSpecificParametersConfig.Articles.Count;
        }

        public int GetArticleDataLocation(int articleId)
        {
            int articleLocation = Convert.ToInt32(CharacterSpecificParametersConfig.Articles[articleId].Location, 16);
            return PsaFile.DataSection[DataSectionLocation + articleLocation / 4] / 4;

            // to get article offset, multiple the result of this by 4
            // ArtOffset.Text = "0x" + (artdt * 4).ToString("X");

        }

        public int GetNumberOfArticleActions(int articleId)
        {
            int articleDataLocation = GetArticleDataLocation(articleId);
            if (PsaFile.DataSection[articleDataLocation + 5] > 8096 && PsaFile.DataSection[articleDataLocation + 5] < PsaFile.DataSectionSize)
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
            if (PsaFile.DataSection[articleDataLocation + 4] > 8096 && PsaFile.DataSection[articleDataLocation + 4] < PsaFile.DataSectionSize)
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
                if (PsaFile.DataSection[articleDataLocation + i + 6] > 8096 && PsaFile.DataSection[articleDataLocation + i + 6] < PsaFile.DataSectionSize)
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
                articleOffsets.Add(offsetNames[i], Utils.ConvertIntToOffset(PsaFile.DataSection[articleDataLocation + i]));
            }
            foreach (KeyValuePair<string, string> pair in articleOffsets)
            {
                Console.WriteLine(string.Format("{0}:{1}", pair.Key, pair.Value));
            }
            return articleOffsets;
        }

        public List<int> GetArticleParameterValues(int articleId, int paramsId)
        {
            List<int> articleParameterValues = new List<int>();
            int articleDataLocation = GetArticleDataLocation(articleId);
            int paramsLocation = Convert.ToInt32(CharacterSpecificParametersConfig.Articles[articleId].ArticleParameters[paramsId].Location, 16);
            int articleParametersDataLocation = PsaFile.DataSection[articleDataLocation + paramsLocation / 4] / 4; // artpo

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
                articleParameterValues.Add(PsaFile.DataSection[articleParametersDataLocation + i]);
            }
            articleParameterValues.ForEach(x => Console.WriteLine(x.ToString("X")));
            return articleParameterValues;

        }

        // this is the offset where article action code starts (displayed in PSAC)
        public int GetArticleActionCodeLocation(int articleId, int actionId)
        {
            int articleDataLocation = GetArticleDataLocation(articleId);
            int articleActionCodeStartingLocation = PsaFile.DataSection[articleDataLocation + 5] / 4; // n -- also in one rare case for Pikmin, + 13 is its exit code block...worry about that later
            int articleActionCodeLocation = PsaFile.DataSection[articleActionCodeStartingLocation + actionId];
            return articleActionCodeLocation;
        }

        public List<PsaCommand> GetPsaCommandsForArticleAction(int articleId, int actionId)
        {
            int articleActionCodeLocation = GetArticleActionCodeLocation(articleId, actionId);
            return PsaCommandHandler.GetPsaCommands(articleActionCodeLocation);
        }

        // this is the offset where article subaction code starts (displayed in PSAC)
        public int GetArticleSubActionCodeBlockLocation(int articleId, int subActionId, int codeBlockId)
        {
            int articleDataLocation = GetArticleDataLocation(articleId);
            int articleSubActionsCodeBlockStartingLocation = PsaFile.DataSection[articleDataLocation + 6 + codeBlockId] / 4; // k
            int articleSubActionCodeBlockLocation = PsaFile.DataSection[articleSubActionsCodeBlockStartingLocation + subActionId];

            return articleSubActionCodeBlockLocation;
        }

        public List<PsaCommand> GetPsaCommandsForArticleSubAction(int articleId, int subActionId, int codeBlockId)
        {
            int articleSubActionCodeBlockLocation = GetArticleSubActionCodeBlockLocation(articleId, subActionId, codeBlockId); // i
            return PsaCommandHandler.GetPsaCommands(articleSubActionCodeBlockLocation);
        }

        public string GetArticleSubActionAnimationName(int articleId, int subActionId)
        {
            // i = subActionId
            // not 100% sure why this chain works but it does
            int articleDataLocation = GetArticleDataLocation(articleId);
            int animationLocation = PsaFile.DataSection[articleDataLocation + 4] / 4 + 1 + subActionId * 2;
            if (PsaFile.DataSection[animationLocation] == 0)
            {
                return "NONE";
            }
            else
            {
                int animationNameLocation = PsaFile.DataSection[animationLocation] / 4; // j

                if (animationNameLocation < PsaFile.DataSectionSize) // and animationNameLocation >= stf whatever that means
                {
                    StringBuilder animationName = new StringBuilder();
                    int nameEndByteIndex = 0;
                    while (true)
                    {
                        string nextStringData = Utils.ConvertDoubleWordToString(PsaFile.DataSection[animationNameLocation + nameEndByteIndex]);
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
            int animationFlagsLocation = PsaFile.DataSection[articleDataLocation + 4] / 4 + subActionId * 2;
            int animationFlagsValue = PsaFile.DataSection[animationFlagsLocation];
            int inTransition = animationFlagsValue >> 24 & 0xFF;
            int noOutTransition = animationFlagsValue & 0x1;
            int loop = animationFlagsValue >> 16 & 0xFF & 0x2;
            int movesCharacter = animationFlagsValue >> 16 & 0xFF & 0x4;
            int unknown3 = animationFlagsValue >> 16 & 0xFF & 0x8;
            int unknown4 = animationFlagsValue >> 16 & 0xFF & 0x10;
            int unknown5 = animationFlagsValue >> 16 & 0xFF & 0x20;
            int transitionOutFromStart = animationFlagsValue >> 16 & 0xFF & 0x40;
            int unknown7 = animationFlagsValue >> 16 & 0xFF & 0x80;
            return new AnimationFlags(inTransition, noOutTransition, loop, movesCharacter, unknown3, unknown4, unknown5, transitionOutFromStart, unknown7);
        }

    }
}
