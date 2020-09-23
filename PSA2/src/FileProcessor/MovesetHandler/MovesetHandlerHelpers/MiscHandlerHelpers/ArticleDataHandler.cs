using PSA2.src.FileProcessor.MovesetHandler.Configs;
using PSA2.src.Models.Fighter;
using PSA2.src.Models.Fighter.Misc;
using PSA2.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.MiscHandlerHelpers
{
    public class ArticleDataHandler
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public CharacterSpecificParametersConfig CharacterSpecificParametersConfig { get; private set; }

        public ArticleDataHandler(PsaFile psaFile, int dataSectionLocation, CharacterSpecificParametersConfig characterSpecificParametersConfig)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            CharacterSpecificParametersConfig = characterSpecificParametersConfig;
        }

        // TODO: Article Data in Misc Section

        /*
        // basicaly done besides the data3 element
        public StaticArticles GetStaticArticles()
        {
            StaticArticles staticArticles = new StaticArticles();
            // static articles count
            if (PsaFile.FileContent[DataSectionLocation + 25] >= 8096 && PsaFile.FileContent[DataSectionLocation + 25] < PsaFile.DataSectionSize)
            {
                staticArticles.Offset = PsaFile.FileContent[DataSectionLocation + 25];
                int staticArticleLocation = PsaFile.FileContent[DataSectionLocation + 25] / 4; // h
                staticArticles.ArticleListOffset = PsaFile.FileContent[staticArticleLocation];
                staticArticles.ArticleCount = PsaFile.FileContent[staticArticleLocation];
                if (PsaFile.FileContent[staticArticleLocation] >= 8096 && PsaFile.FileContent[staticArticleLocation] < PsaFile.DataSectionSize)
                {
                    int staticArticleDataLocation = PsaFile.FileContent[staticArticleLocation] / 4;
                    int numberOfStaticArticles = PsaFile.FileContent[staticArticleLocation + 1];
                    if (numberOfStaticArticles > 0 && numberOfStaticArticles < 21)
                    {
                        staticArticles.ArticleCount = numberOfStaticArticles;
                        for (int i = 0; i < numberOfStaticArticles; i++)
                        {
                            int staticArticlesDataLocation = PsaFile.FileContent[staticArticleLocation] / 4 + i * 14; // k
                            int articleGroupId = PsaFile.FileContent[staticArticlesDataLocation];
                            int arcEntryGroup = PsaFile.FileContent[staticArticlesDataLocation + 1];
                            int bone = PsaFile.FileContent[staticArticlesDataLocation + 2];
                            int actionFlags = PsaFile.FileContent[staticArticlesDataLocation + 3];
                            int subActionFlags = PsaFile.FileContent[staticArticlesDataLocation + 4];
                            int actions = PsaFile.FileContent[staticArticlesDataLocation + 5];
                            int subActionMain = PsaFile.FileContent[staticArticlesDataLocation + 6];
                            int subActionGfx = PsaFile.FileContent[staticArticlesDataLocation + 7];
                            int subActionSfx = PsaFile.FileContent[staticArticlesDataLocation + 8];
                            int modelVisibility = PsaFile.FileContent[staticArticlesDataLocation + 9];
                            int collisionData = PsaFile.FileContent[staticArticlesDataLocation + 10];
                            int data2 = PsaFile.FileContent[staticArticlesDataLocation + 11];
                            int data3 = PsaFile.FileContent[staticArticlesDataLocation + 12];
                            int subActionCount = PsaFile.FileContent[staticArticlesDataLocation + 13];

                            StaticArticleEntry staticArticleEntry = new StaticArticleEntry(
                                    staticArticlesDataLocation * 4, articleGroupId, arcEntryGroup, bone, actionFlags, subActionFlags, actions, subActionMain,
                                    subActionGfx, subActionSfx, modelVisibility, collisionData, data2, data3, subActionCount);
                            staticArticles.StaticArticleEntries.Add(staticArticleEntry);

                            int staticArticlesDataSubActionLocation = staticArticlesDataLocation + 4;

                            // checks if subaction data exists at all
                            if (PsaFile.FileContent[staticArticlesDataSubActionLocation] >= 8096 && PsaFile.FileContent[staticArticlesDataSubActionLocation] < PsaFile.DataSectionSize)
                            {
                                int numberOfSubActions = 0;
                                if (PsaFile.FileContent[staticArticlesDataSubActionLocation + 9] > 0 && PsaFile.FileContent[staticArticlesDataSubActionLocation + 9] < 11)
                                {
                                    numberOfSubActions = PsaFile.FileContent[staticArticlesDataSubActionLocation + 9];
                                }
                                else
                                {
                                    numberOfSubActions = 1;
                                }
                                int staticArticlesDataSubActionValuesLocation = PsaFile.FileContent[staticArticlesDataSubActionLocation] / 4 + 1; // n
                                
                                
                                 // NOTE: The subaction and subaction gfx code blocks are the same exact thing and can be combined
                                
                                
                                // IF SUBACTION GFX SECTION
                                if (PsaFile.FileContent[staticArticlesDataSubActionLocation + 3] >= 8096 && PsaFile.FileContent[staticArticlesDataSubActionLocation + 3] < PsaFile.DataSectionSize)
                                {
                                    Console.WriteLine(String.Format("Static Article {0} has a SubAction GFX Section", i)); // Wario

                                    for (int j = 0; j < numberOfSubActions; j++)
                                    {
                                        string subActionName = "";
                                        if (PsaFile.FileContent[staticArticlesDataSubActionValuesLocation] == 0)
                                        {
                                            subActionName = "<null>";
                                        }
                                        else
                                        {
                                            int subActionNameLocation = PsaFile.FileContent[staticArticlesDataSubActionValuesLocation + (j * 2)] / 4;
                                            if (subActionNameLocation < PsaFile.DataSectionSize) // and >= stf whatever that means
                                            {
                                                StringBuilder animationName = new StringBuilder();
                                                int nameEndByteIndex = 0;
                                                while (true) // originally i < 47 -- 48 char limit?
                                                {
                                                    string nextStringData = Utils.ConvertWordToString(PsaFile.FileContent[subActionNameLocation + nameEndByteIndex]);
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
                                                subActionName = animationName.ToString();
                                                Console.WriteLine(subActionName);
                                            }
                                        }
                                        staticArticleEntry.SubActionNames.Add(subActionName);

                                    }
                                }
                                // IF SUBACTION SECTION
                                else
                                {
                                    Console.WriteLine(String.Format("Static Article {0} has a SubAction Section", i)); // MetaKnight

                                    for (int j = 0; j < numberOfSubActions; j++)
                                    {
                                        string subActionName = "";
                                        if (PsaFile.FileContent[staticArticlesDataSubActionValuesLocation] == 0)
                                        {
                                            subActionName = "<null>";
                                        }
                                        else
                                        {
                                            int subActionNameLocation = PsaFile.FileContent[staticArticlesDataSubActionValuesLocation + (j * 2)] / 4;
                                            if (subActionNameLocation < PsaFile.DataSectionSize) // and >= stf whatever that means
                                            {
                                                StringBuilder animationName = new StringBuilder();
                                                int nameEndByteIndex = 0;
                                                while (true) // originally i < 47 -- 48 char limit?
                                                {
                                                    string nextStringData = Utils.ConvertWordToString(PsaFile.FileContent[subActionNameLocation + nameEndByteIndex]);
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
                                                subActionName = animationName.ToString();
                                                Console.WriteLine(subActionName);
                                            }
                                        }
                                        staticArticleEntry.SubActionNames.Add(subActionName);

                                    }

                                }
                            }

                            int staticArticlesData3Location = staticArticlesDataLocation + 12;
                            if (PsaFile.FileContent[staticArticlesData3Location] >= 8096 && PsaFile.FileContent[staticArticlesData3Location] < PsaFile.DataSectionSize)
                            {
                                Console.WriteLine(String.Format("Static Article {0} has a Data3 Section", i)); // kirby

                                // I don't care, this code is ridiculous for all that it accomplishes -- a section that we don't even know what it does
                                int staticArticlesData3ValuesLocation = PsaFile.FileContent[staticArticlesData3Location] / 4;

                            }
                        }
                    }
                }
            }
            Console.WriteLine(staticArticles);
            return staticArticles;
        }

        public EntryArticle GetEntryArticle()
        {
            EntryArticle entryArticle = new EntryArticle();
            // entry article existence (there appears to only be 1 entry article allowed)
            if (PsaFile.FileContent[DataSectionLocation + 26] >= 8096 && PsaFile.FileContent[DataSectionLocation + 26] < PsaFile.DataSectionSize)
            {
                Console.WriteLine("Entry Article Exists");
                entryArticle.Offset = PsaFile.FileContent[DataSectionLocation + 26];
                int entryArticleLocation = PsaFile.FileContent[DataSectionLocation + 26] / 4;
                entryArticle.ArticleGroupId = PsaFile.FileContent[entryArticleLocation];
                entryArticle.ArcEntryGroup = PsaFile.FileContent[entryArticleLocation + 1];
                entryArticle.Bone = PsaFile.FileContent[entryArticleLocation + 2];
                entryArticle.ActionFlags = PsaFile.FileContent[entryArticleLocation + 3];
                entryArticle.SubActionFlags = PsaFile.FileContent[entryArticleLocation + 4];
                entryArticle.Actions = PsaFile.FileContent[entryArticleLocation + 5];
                entryArticle.SubActionMain = PsaFile.FileContent[entryArticleLocation + 6];
                entryArticle.SubActionGfx = PsaFile.FileContent[entryArticleLocation + 7];
                entryArticle.SubActionSfx = PsaFile.FileContent[entryArticleLocation + 8];
                entryArticle.ModelVisibility = PsaFile.FileContent[entryArticleLocation + 9];
                entryArticle.CollisionData = PsaFile.FileContent[entryArticleLocation + 10];
                entryArticle.Data2 = PsaFile.FileContent[entryArticleLocation + 11];
                entryArticle.Data3 = PsaFile.FileContent[entryArticleLocation + 12];
                entryArticle.SubActionCount = PsaFile.FileContent[entryArticleLocation + 13];

                if (PsaFile.FileContent[entryArticleLocation + 4] >= 8096 && PsaFile.FileContent[entryArticleLocation + 4] < PsaFile.DataSectionSize)
                {
                    // checks if entry article has subaction
                    if (PsaFile.FileContent[entryArticleLocation + 7] >= 8096 && PsaFile.FileContent[entryArticleLocation + 7] < PsaFile.DataSectionSize)
                    {
                        Console.WriteLine("Entry Article has a SubAction GFX Section");
                    }
                    else
                    {
                        Console.WriteLine("Entry Article has a SubAction Section");
                    }


                }
                if (PsaFile.FileContent[entryArticleLocation + 12] >= 8096 && PsaFile.FileContent[entryArticleLocation + 12] < PsaFile.DataSectionSize)
                {
                    Console.WriteLine("Entry Article has a Data3 Section");
                }
            }
            return entryArticle;
        }

        public ArticleExtraDatas GetArticleExtraDatas()
        {
            ArticleExtraDatas articleExtraDatas = new ArticleExtraDatas();
            // article extra datas
            // might be able to redo this using my file parsing method
            Console.WriteLine("Article Extra Datas Section");
            foreach (Configs.Article article in CharacterSpecificParametersConfig.Articles)
            {
                Console.WriteLine(String.Format("Article {0} extra datas", article.Name));

                int articleLocation = Convert.ToInt32(article.Location, 16);
                int articleDataLocation = PsaFile.FileContent[DataSectionLocation + articleLocation / 4] / 4; // an4
                if (PsaFile.FileContent[articleDataLocation + 3] >= 8096 && PsaFile.FileContent[articleDataLocation + 3] < PsaFile.DataSectionSize)
                {
                    articleExtraDatas.ArticleExtraDataEntries.Add(GetArticleExtraDataEntryType1(article, articleDataLocation));
                }
                else
                {
                    articleExtraDatas.ArticleExtraDataEntries.Add(GetArticleExtraDataEntryType2(articleDataLocation));
                }

            }
            return articleExtraDatas;
        }

        private ArticleExtraDataEntry GetArticleExtraDataEntryType1(Configs.Article article, int articleDataLocation)
        {
            ArticleExtraDataEntry articleExtraDataEntry = new ArticleExtraDataEntry();
            Console.WriteLine(String.Format("Article {0} has {1} action flags", article.Name, article.NumberOfActions));
            ModelVisibility modelVisibility = new ModelVisibility();
            if (PsaFile.FileContent[articleDataLocation + 9] >= 8096 && PsaFile.FileContent[articleDataLocation + 9] < PsaFile.DataSectionSize)
            {
                int articleModelVisiblityLocation = PsaFile.FileContent[articleDataLocation + 9] / 4; // h
                // if sections exists in model visiblity
                if (PsaFile.FileContent[articleModelVisiblityLocation] >= 8096 && PsaFile.FileContent[articleModelVisiblityLocation] < PsaFile.DataSectionSize)
                {
                    int modelVisiblityHiddenSectionLocation = PsaFile.FileContent[articleModelVisiblityLocation] / 4;
                    int numberOfBoneSwitches = PsaFile.FileContent[articleModelVisiblityLocation + 1];
                    if (numberOfBoneSwitches > 0 && numberOfBoneSwitches < 20)
                    {
                        // if hidden section exists
                        if (PsaFile.FileContent[modelVisiblityHiddenSectionLocation] >= 8096 && PsaFile.FileContent[modelVisiblityHiddenSectionLocation] < PsaFile.DataSectionSize)
                        {
                            ModelVisibilitySection section = new ModelVisibilitySection();
                            modelVisibility.Sections.Add(section);
                            section.Name = "Hidden";
                            int sectionBoneSwitchLocation = PsaFile.FileContent[modelVisiblityHiddenSectionLocation] / 4;
                            for (int i = 0; i < numberOfBoneSwitches; i++)
                            {
                                BoneSwitch boneSwitch = new BoneSwitch();
                                section.BoneSwitches.Add(boneSwitch);
                                int boneGroupLocation = PsaFile.FileContent[sectionBoneSwitchLocation + i * 2];
                                if (boneGroupLocation >= 8096 && boneGroupLocation < PsaFile.DataSectionSize)
                                {
                                    int numberOfBoneGroups = PsaFile.FileContent[sectionBoneSwitchLocation + i * 2 + 1];
                                    if (numberOfBoneGroups > 0 && numberOfBoneGroups < 256)
                                    {
                                        int boneGroupBonesLocation = boneGroupLocation / 4;
                                        for (int j = 0; j < numberOfBoneGroups; j++)
                                        {
                                            BoneGroup boneGroup = new BoneGroup();
                                            boneSwitch.BoneGroups.Add(boneGroup);
                                            if (PsaFile.FileContent[boneGroupBonesLocation + j * 2] >= 8096 && PsaFile.FileContent[boneGroupBonesLocation + j * 2] < PsaFile.DataSectionSize)
                                            {
                                                int numberOfBones = PsaFile.FileContent[boneGroupBonesLocation + j * 2 + 1];
                                                if (numberOfBones > 0 && numberOfBones < 256)
                                                {
                                                    boneGroup.numberOfBones = numberOfBones;
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        // not a clue what this is for but not going to question it for now
                        // if notsure is equal to 4, there is no "Visible" model visibilty section for the article -- once again not sure why
                        int notsure = 0;
                        if (PsaFile.FileContent[articleModelVisiblityLocation + 2] >= 8096 && PsaFile.FileContent[articleModelVisiblityLocation + 2] < PsaFile.DataSectionSize)
                        {
                            notsure = PsaFile.FileContent[articleModelVisiblityLocation + 2] - PsaFile.FileContent[articleModelVisiblityLocation];
                        }
                        else
                        {
                            notsure = articleModelVisiblityLocation * 4 - PsaFile.FileContent[articleModelVisiblityLocation];
                        }

                        // if visible section exists
                        if (notsure != 4 && PsaFile.FileContent[modelVisiblityHiddenSectionLocation + 1] >= 8096 && PsaFile.FileContent[modelVisiblityHiddenSectionLocation + 1] < PsaFile.DataSectionSize)
                        {
                            ModelVisibilitySection section = new ModelVisibilitySection();
                            modelVisibility.Sections.Add(section);
                            section.Name = "Hidden";
                            int sectionBoneSwitchLocation = PsaFile.FileContent[modelVisiblityHiddenSectionLocation] / 4;
                            for (int i = 0; i < numberOfBoneSwitches; i++)
                            {
                                BoneSwitch boneSwitch = new BoneSwitch();
                                section.BoneSwitches.Add(boneSwitch);
                                int boneGroupLocation = PsaFile.FileContent[sectionBoneSwitchLocation + i * 2];
                                if (boneGroupLocation >= 8096 && boneGroupLocation < PsaFile.DataSectionSize)
                                {
                                    int numberOfBoneGroups = PsaFile.FileContent[sectionBoneSwitchLocation + i * 2 + 1];
                                    if (numberOfBoneGroups > 0 && numberOfBoneGroups < 256)
                                    {
                                        int boneGroupBonesLocation = boneGroupLocation / 4;
                                        for (int j = 0; j < numberOfBoneGroups; j++)
                                        {
                                            BoneGroup boneGroup = new BoneGroup();
                                            boneSwitch.BoneGroups.Add(boneGroup);
                                            if (PsaFile.FileContent[boneGroupBonesLocation + j * 2] >= 8096 && PsaFile.FileContent[boneGroupBonesLocation + j * 2] < PsaFile.DataSectionSize)
                                            {
                                                int numberOfBones = PsaFile.FileContent[boneGroupBonesLocation + j * 2 + 1];
                                                if (numberOfBones > 0 && numberOfBones < 256)
                                                {
                                                    boneGroup.numberOfBones = numberOfBones;
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }


                    // model visiblity data section
                    if (PsaFile.FileContent[articleModelVisiblityLocation + 2] >= 8096 && PsaFile.FileContent[articleModelVisiblityLocation + 2] < PsaFile.DataSectionSize)
                    {
                        int numberOfDataSections = PsaFile.FileContent[articleModelVisiblityLocation + 3];
                        if (numberOfDataSections > 0 && numberOfDataSections < 51)
                        {
                            modelVisibility.SectionsDataCount = numberOfDataSections;
                        }
                    }

                }
            }
            articleExtraDataEntry.ModelVisibility = modelVisibility;

            // check for collision data
            if (PsaFile.FileContent[articleDataLocation + 10] > 8096 && PsaFile.FileContent[articleDataLocation + 10] < PsaFile.DataSectionSize)
            {
                int collisionDataLocation = PsaFile.FileContent[articleDataLocation + 10] / 4;
                if (PsaFile.FileContent[collisionDataLocation] > 8096 && PsaFile.FileContent[collisionDataLocation] < PsaFile.DataSectionSize)
                {
                    Console.WriteLine(String.Format("Article {0} has CollisionData", article.Name));
                }
            }

            // check for data2 section
            if (PsaFile.FileContent[articleDataLocation + 11] > 8096 && PsaFile.FileContent[articleDataLocation + 11] < PsaFile.DataSectionSize)
            {
                int data2Location = PsaFile.FileContent[articleDataLocation + 11] / 4;
                if (PsaFile.FileContent[data2Location] > 8096 && PsaFile.FileContent[data2Location] < PsaFile.DataSectionSize)
                {
                    Console.WriteLine("Data2 Section exists");
                    int data2Count = PsaFile.FileContent[data2Location + 1];
                    Console.WriteLine(String.Format("Data2 Count: {0}", data2Count));
                }
            }

            return articleExtraDataEntry;
        }

        /// <summary>
        /// This is exactly the same as type1's article model visibility code on first glance...probs can easily combine these :)
        /// </summary>
        /// <param name="article"></param>
        /// <param name="dataSectionOffset"></param>
        /// <param name="articleDataLocation"></param>
        private ArticleExtraDataEntry GetArticleExtraDataEntryType2(int articleDataLocation)
        {
            ArticleExtraDataEntry articleExtraDataEntry = new ArticleExtraDataEntry();
            ModelVisibility modelVisibility = new ModelVisibility();
            if (PsaFile.FileContent[articleDataLocation + 9] >= 8096 && PsaFile.FileContent[articleDataLocation + 9] < PsaFile.DataSectionSize)
            {
                int articleModelVisiblityLocation = PsaFile.FileContent[articleDataLocation + 9] / 4; // h
                // if sections exists in model visiblity
                if (PsaFile.FileContent[articleModelVisiblityLocation] >= 8096 && PsaFile.FileContent[articleModelVisiblityLocation] < PsaFile.DataSectionSize)
                {
                    int modelVisiblityHiddenSectionLocation = PsaFile.FileContent[articleModelVisiblityLocation] / 4;
                    int numberOfBoneSwitches = PsaFile.FileContent[articleModelVisiblityLocation + 1];
                    if (numberOfBoneSwitches > 0 && numberOfBoneSwitches < 20)
                    {
                        // if hidden section exists
                        if (PsaFile.FileContent[modelVisiblityHiddenSectionLocation] >= 8096 && PsaFile.FileContent[modelVisiblityHiddenSectionLocation] < PsaFile.DataSectionSize)
                        {
                            ModelVisibilitySection section = new ModelVisibilitySection();
                            modelVisibility.Sections.Add(section);
                            section.Name = "Hidden";
                            int sectionBoneSwitchLocation = PsaFile.FileContent[modelVisiblityHiddenSectionLocation] / 4;
                            for (int i = 0; i < numberOfBoneSwitches; i++)
                            {
                                BoneSwitch boneSwitch = new BoneSwitch();
                                section.BoneSwitches.Add(boneSwitch);
                                int boneGroupLocation = PsaFile.FileContent[sectionBoneSwitchLocation + i * 2];
                                if (boneGroupLocation >= 8096 && boneGroupLocation < PsaFile.DataSectionSize)
                                {
                                    int numberOfBoneGroups = PsaFile.FileContent[sectionBoneSwitchLocation + i * 2 + 1];
                                    if (numberOfBoneGroups > 0 && numberOfBoneGroups < 256)
                                    {
                                        int boneGroupBonesLocation = boneGroupLocation / 4;
                                        for (int j = 0; j < numberOfBoneGroups; j++)
                                        {
                                            BoneGroup boneGroup = new BoneGroup();
                                            boneSwitch.BoneGroups.Add(boneGroup);
                                            if (PsaFile.FileContent[boneGroupBonesLocation + j * 2] >= 8096 && PsaFile.FileContent[boneGroupBonesLocation + j * 2] < PsaFile.DataSectionSize)
                                            {
                                                int numberOfBones = PsaFile.FileContent[boneGroupBonesLocation + j * 2 + 1];
                                                if (numberOfBones > 0 && numberOfBones < 256)
                                                {
                                                    boneGroup.numberOfBones = numberOfBones;
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        // not a clue what this is for but not going to question it for now
                        // if notsure is equal to 4, there is no "Visible" model visibilty section for the article -- once again not sure why
                        int notsure;
                        if (PsaFile.FileContent[articleModelVisiblityLocation + 2] >= 8096 && PsaFile.FileContent[articleModelVisiblityLocation + 2] < PsaFile.DataSectionSize)
                        {
                            notsure = PsaFile.FileContent[articleModelVisiblityLocation + 2] - PsaFile.FileContent[articleModelVisiblityLocation];
                        }
                        else
                        {
                            notsure = articleModelVisiblityLocation * 4 - PsaFile.FileContent[articleModelVisiblityLocation];
                        }

                        // if visible section exists
                        if (notsure != 4 && PsaFile.FileContent[modelVisiblityHiddenSectionLocation + 1] >= 8096 && PsaFile.FileContent[modelVisiblityHiddenSectionLocation + 1] < PsaFile.DataSectionSize)
                        {
                            ModelVisibilitySection section = new ModelVisibilitySection();
                            modelVisibility.Sections.Add(section);
                            section.Name = "Hidden";
                            int sectionBoneSwitchLocation = PsaFile.FileContent[modelVisiblityHiddenSectionLocation] / 4;
                            for (int i = 0; i < numberOfBoneSwitches; i++)
                            {
                                BoneSwitch boneSwitch = new BoneSwitch();
                                section.BoneSwitches.Add(boneSwitch);
                                int boneGroupLocation = PsaFile.FileContent[sectionBoneSwitchLocation + i * 2];
                                if (boneGroupLocation >= 8096 && boneGroupLocation < PsaFile.DataSectionSize)
                                {
                                    int numberOfBoneGroups = PsaFile.FileContent[sectionBoneSwitchLocation + i * 2 + 1];
                                    if (numberOfBoneGroups > 0 && numberOfBoneGroups < 256)
                                    {
                                        int boneGroupBonesLocation = boneGroupLocation / 4;
                                        for (int j = 0; j < numberOfBoneGroups; j++)
                                        {
                                            BoneGroup boneGroup = new BoneGroup();
                                            boneSwitch.BoneGroups.Add(boneGroup);
                                            if (PsaFile.FileContent[boneGroupBonesLocation + j * 2] >= 8096 && PsaFile.FileContent[boneGroupBonesLocation + j * 2] < PsaFile.DataSectionSize)
                                            {
                                                int numberOfBones = PsaFile.FileContent[boneGroupBonesLocation + j * 2 + 1];
                                                if (numberOfBones > 0 && numberOfBones < 256)
                                                {
                                                    boneGroup.numberOfBones = numberOfBones;
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }


                    // model visiblity data section
                    if (PsaFile.FileContent[articleModelVisiblityLocation + 2] >= 8096 && PsaFile.FileContent[articleModelVisiblityLocation + 2] < PsaFile.DataSectionSize)
                    {
                        int numberOfDataSections = PsaFile.FileContent[articleModelVisiblityLocation + 3];
                        if (numberOfDataSections > 0 && numberOfDataSections < 51)
                        {
                            modelVisibility.SectionsDataCount = numberOfDataSections;
                        }
                    }

                }
            }
            articleExtraDataEntry.ModelVisibility = modelVisibility;
            return articleExtraDataEntry;
        }
        */
    }
}
