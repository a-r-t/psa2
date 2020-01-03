using Newtonsoft.Json;
using PSA2.src.models.fighter;
using Attribute = PSA2.src.models.fighter.Attribute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSA2.src.utility;
using PSA2.src.models.fighter.Misc;
using PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers;

namespace PSA2.src.FileProcessor.MovesetParser
{
    public class PsaMovesetParser
    {
        public PsaFile PsaFile { get; private set; }
        public AttributesParser AttributesParser { get; private set; }
        public DataTableParser DataTableParser { get; private set; }
        public ExternalDataParser ExternalDataParser { get; private set; }
        public ActionsParser ActionsParser { get; private set; }
        public MiscParser MiscParser { get; private set; }

        public PsaMovesetParser(PsaFile psaFile)
        {
            PsaFile = psaFile;
            AttributesParser = new AttributesParser(PsaFile);
            DataTableParser = new DataTableParser(PsaFile);
            ExternalDataParser = new ExternalDataParser(PsaFile);
            int dataSectionLocation = DataTableParser.GetDataTableEntryByName("data").Location;
            ActionsParser = new ActionsParser(PsaFile, dataSectionLocation);
            int numberOfSpecialActions = ActionsParser.GetNumberOfSpecialActions();
            string movesetBaseName = GetMovesetBaseName();
            MiscParser = new MiscParser(PsaFile, dataSectionLocation, movesetBaseName, numberOfSpecialActions);
            //bool isMovesetParsable = IsMovesetParsable();
            List<PsaInstruction> psaInstruction = ActionsParser.GetPsaInstructionsForAction(0, 0);
        }

        public bool IsMovesetParsable()
        {
            try
            {
                List<Attribute> attributes = AttributesParser.GetAttributes();
                List<DataEntry> dataTableEntries = DataTableParser.GetDataTableEntries();
                List<DataEntry> externalDataEntries = ExternalDataParser.GetExternalDataEntries();
                ModelVisibility modelVisibility = MiscParser.ModelVisibilityParser.GetModelVisibility();
                MiscSection1 miscSection1 = MiscParser.MiscSectionParser.GetMiscSection1();
                FinalSmashAura finalSmashAura = MiscParser.MiscSectionParser.GetFinalSmashAura();
                HurtBoxes hurtBoxes = MiscParser.MiscSectionParser.GetHurtBoxes();
                LedgeGrab ledgeGrab = MiscParser.MiscSectionParser.GetLedgeGrab();
                MiscSection2 miscSection2 = MiscParser.MiscSectionParser.GetMiscSection2();
                BoneReferences miscSectionBoneReferences = MiscParser.MiscSectionParser.GetBoneReferences();
                ItemBones itemBones = MiscParser.MiscSectionParser.GetItemBones();
                SoundLists soundLists = MiscParser.MiscSectionParser.GetSoundLists();
                MiscSection5 miscSection5 = MiscParser.MiscSectionParser.GetMiscSection5();
                MultiJump multiJump = MiscParser.MiscSectionParser.GetMultiJump();
                Glide glide = MiscParser.MiscSectionParser.GetGlide();
                Crawl crawl = MiscParser.MiscSectionParser.GetCrawl();
                CollisionData collisionData = MiscParser.MiscSectionParser.GetCollisionData();
                Tether tether = MiscParser.MiscSectionParser.GetTether();
                MiscSection12 miscSection12 = MiscParser.MiscSectionParser.GetMiscSection12();
                CommonActionFlags commonActionFlags = MiscParser.GetCommonActionFlags();
                SpecialActionFlags specialActionFlags = MiscParser.GetSpecialActionFlags();
                ExtraActionFlags extraActionFlags = MiscParser.GetExtraActionFlags();
                ActionInterrupts actionInterrupts = MiscParser.GetActionInterrupts();
                BoneFloats1 boneFloats1 = MiscParser.GetBoneFloats1();
                BoneFloats2 boneFloats2 = MiscParser.GetBoneFloats2();
                BoneFloats3 boneFloats3 = MiscParser.GetBoneFloats3();
                BoneReferences boneReferences = MiscParser.GetBoneReferences();
                HandBones handBones = MiscParser.GetHandBones();
                ExtraActionInterrupts extraActionInterrupts = MiscParser.GetExtraActionInterrupts();
                Unknown24 unknown24 = MiscParser.GetUnknown24();
                StaticArticles staticArticles = MiscParser.ArticleDataParser.GetStaticArticles();
                EntryArticle entryArticle = MiscParser.ArticleDataParser.GetEntryArticle();
                ArticleExtraDatas articleExtraDatas = MiscParser.ArticleDataParser.GetArticleExtraDatas();
                DataFlags dataFlags = MiscParser.GetDataFlags();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
            return true;
        }

        public string GetMovesetBaseName()
        {
            StringBuilder movesetBaseName = new StringBuilder();
            int nameEndByteIndex = 4;
            while (true)
            {
                string nextStringData = Utils.ConvertWordToString(PsaFile.FileHeader[nameEndByteIndex]);
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
