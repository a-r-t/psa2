using Newtonsoft.Json;
using PSA2.src.Models.Fighter;
using Attribute = PSA2.src.Models.Fighter.Attribute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSA2.src.Utility;
using PSA2.src.Models.Fighter.Misc;
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers;

namespace PSA2.src.FileProcessor.MovesetHandler
{
    public class PsaMovesetHandler
    {
        public PsaFile PsaFile { get; private set; }
        public AttributesParser AttributesParser { get; private set; }
        public DataTableHandler DataTableParser { get; private set; }
        public ExternalDataHandler ExternalDataParser { get; private set; }
        public ActionsHandler ActionsParser { get; private set; }
        public SubActionsHandler SubActionsParser { get; private set; }
        public SubRoutinesHandler SubRoutinesParser { get; private set; }
        public ActionOverridesHandler ActionOverridesParser { get; private set; }
        public ArticlesHandler ArticlesParser { get; private set; }
        public CharacterParamsHandler CharacterParamsParser { get; private set; }
        public MiscHandler MiscParser { get; private set; }

        public PsaMovesetHandler(PsaFile psaFile)
        {
            PsaFile = psaFile;
            AttributesParser = new AttributesParser(PsaFile);
            DataTableParser = new DataTableHandler(PsaFile);
            ExternalDataParser = new ExternalDataHandler(PsaFile);
            int dataSectionLocation = DataTableParser.GetDataTableEntryByName("data").Location;
            string movesetBaseName = GetMovesetBaseName();

            int numberOfSpecialActions = (PsaFile.FileContent[dataSectionLocation + 10] - PsaFile.FileContent[dataSectionLocation + 9]) / 4;
            int codeBlockDataStartLocation = 2014 + numberOfSpecialActions * 2;
            PsaCommandHandler psaCommandHandler = new PsaCommandHandler(psaFile, dataSectionLocation, codeBlockDataStartLocation);

            ActionsParser = new ActionsHandler(PsaFile, dataSectionLocation, psaCommandHandler);
            SubActionsParser = new SubActionsHandler(PsaFile, dataSectionLocation, psaCommandHandler);
            SubRoutinesParser = new SubRoutinesHandler(PsaFile, dataSectionLocation, ActionsParser, SubActionsParser, psaCommandHandler);
            ActionOverridesParser = new ActionOverridesHandler(PsaFile, dataSectionLocation, ActionsParser, psaCommandHandler);
            ArticlesParser = new ArticlesHandler(PsaFile, dataSectionLocation, movesetBaseName, psaCommandHandler);
            CharacterParamsParser = new CharacterParamsHandler(PsaFile, dataSectionLocation, movesetBaseName, psaCommandHandler);
            MiscParser = new MiscHandler(PsaFile, dataSectionLocation, movesetBaseName, numberOfSpecialActions);

            //PsaCommand psaCommand = ActionsParser.GetPsaCommandsForActionCodeBlock(0, 0)[0];
            //Console.WriteLine(psaCommand);
            //Console.WriteLine(psaCommand.Instruction);

            //bool isMovesetParsable = IsMovesetParsable();
            //ActionsParser.AddCommandToAction(0, 0);
            //ActionsParser.AddCommandToAction(0, 0);
            //ActionsParser.AddCommandToAction(0, 1);
            //PsaFile.SaveFile("results.pac");

            //ActionsParser.MoveActionCommand(0, 0, 1, MoveDirection.UP);
            //Console.WriteLine(ActionsParser.GetAction(0));

            /*
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(6, 0)
            };
            ActionsParser.ModifyActionCommand(2, 0, 11, new PsaCommand(33620480, 25788, parameters));
            */
        }

        public bool IsMovesetParsable()
        {

            //List<Attribute> attributes = AttributesParser.GetAttributes();
            //List<DataEntry> dataTableEntries = DataTableParser.GetDataTableEntries();
            //List<DataEntry> externalDataEntries = ExternalDataParser.GetExternalDataEntries();
            //ModelVisibility modelVisibility = MiscParser.ModelVisibilityParser.GetModelVisibility();
            //MiscSection miscSection = MiscParser.MiscSectionParser.GetMiscSection();
            //MiscSection1 miscSection1 = MiscParser.MiscSectionParser.GetMiscSection1();
            //FinalSmashAura finalSmashAura = MiscParser.MiscSectionParser.GetFinalSmashAura();
            //HurtBoxes hurtBoxes = MiscParser.MiscSectionParser.GetHurtBoxes();
            //LedgeGrab ledgeGrab = MiscParser.MiscSectionParser.GetLedgeGrab();
            //MiscSection2 miscSection2 = MiscParser.MiscSectionParser.GetMiscSection2();
            //BoneReferences miscSectionBoneReferences = MiscParser.MiscSectionParser.GetBoneReferences();
            //ItemBones itemBones = MiscParser.MiscSectionParser.GetItemBones();
            //SoundLists soundLists = MiscParser.MiscSectionParser.GetSoundLists();
            //MiscSection5 miscSection5 = MiscParser.MiscSectionParser.GetMiscSection5();
            //MultiJump multiJump = MiscParser.MiscSectionParser.GetMultiJump();
            //Glide glide = MiscParser.MiscSectionParser.GetGlide();
            //Crawl crawl = MiscParser.MiscSectionParser.GetCrawl();
            //CollisionData collisionData = MiscParser.MiscSectionParser.GetCollisionData();
            //Tether tether = MiscParser.MiscSectionParser.GetTether();
            //MiscSection12 miscSection12 = MiscParser.MiscSectionParser.GetMiscSection12();
            //CommonActionFlags commonActionFlags = MiscParser.GetCommonActionFlags();
            //SpecialActionFlags specialActionFlags = MiscParser.GetSpecialActionFlags();
            //ExtraActionFlags extraActionFlags = MiscParser.GetExtraActionFlags();
            //ActionInterrupts actionInterrupts = MiscParser.GetActionInterrupts();
            //BoneFloats1 boneFloats1 = MiscParser.GetBoneFloats1();
            //BoneFloats2 boneFloats2 = MiscParser.GetBoneFloats2();
            //BoneFloats3 boneFloats3 = MiscParser.GetBoneFloats3();
            //BoneReferences boneReferences = MiscParser.GetBoneReferences();
            //HandBones handBones = MiscParser.GetHandBones();
            //ExtraActionInterrupts extraActionInterrupts = MiscParser.GetExtraActionInterrupts();
            //Unknown24 unknown24 = MiscParser.GetUnknown24();
            //StaticArticles staticArticles = MiscParser.ArticleDataParser.GetStaticArticles();
            //EntryArticle entryArticle = MiscParser.ArticleDataParser.GetEntryArticle();
            //ArticleExtraDatas articleExtraDatas = MiscParser.ArticleDataParser.GetArticleExtraDatas();
            //DataFlags dataFlags = MiscParser.GetDataFlags();

            //List<PsaCommand> actionPsaCommands = ActionsParser.GetPsaCommandsForAction(0, 0);
            //List<PsaCommand> subActionPsaCommands = SubActionsParser.GetPsaCommandsForSubAction(73, 1);
            //string animationName = SubActionsParser.GetSubActionAnimationName(73);
            //AnimationFlags animationFlags = SubActionsParser.GetSubActionAnimationFlags(11);
            //List<PsaCommand> subRoutineCode = SubRoutinesParser.GetPsaCommandsForSubRoutine(53288);
            //List<int> subRoutines = SubRoutinesParser.GetAllSubRoutines();
            //List<int> actionOverrideIdsEntries = ActionOverridesParser.GetAllActionOverrides(0);
            //List<PsaCommand> actionOverideCommands = ActionOverridesParser.GetPsaCommandsForActionOverride(0, 0);
            //int numberOfArticleActions = ArticlesParser.GetNumberOfArticleActions(0);
            //int numberOfArticleSubActions = ArticlesParser.GetNumberOfArticleActions(0);
            //List<int> codeBlockIdsForArticleSubActions = ArticlesParser.GetCodeBlockIdsForArticleSubActions(0);
            //Dictionary<string, string> articleOffsets = ArticlesParser.GetArticleOffsets(0);
            //List<int> articleParameterValues = ArticlesParser.GetArticleParameterValues(0, 0);
            //List<int> characterParameterValues = CharacterParamsParser.GetCharacterParameterValues(1);
            //List<int> characterExtraParameterValues = CharacterParamsParser.GetCharacterExtraParameterValues(0);
            //List<PsaCommand> articleActionCommands = ArticlesParser.GetPsaCommandsForArticleAction(0, 2);
            //List<PsaCommand> articleSubActionCommands = ArticlesParser.GetPsaCommandsForArticleSubAction(0, 1, 0);
            //string articleAnimationName = ArticlesParser.GetArticleSubActionAnimationName(0, 3);
            //AnimationFlags articleAnimationFlags = ArticlesParser.GetArticleSubActionAnimationFlags(0, 0);



            return true;
        }

        public string GetMovesetBaseName()
        {
            StringBuilder movesetBaseName = new StringBuilder();
            int nameEndByteIndex = 4;
            while (true)
            {
                string nextStringData = Utils.ConvertDoubleWordToString(PsaFile.FileHeader[nameEndByteIndex]);
                movesetBaseName.Append(nextStringData);
                if (nextStringData.Length == 4)
                {
                    nameEndByteIndex++;
                }
                else
                {
                    break;
                }
            }
            return movesetBaseName.ToString();
        }
    }
}
