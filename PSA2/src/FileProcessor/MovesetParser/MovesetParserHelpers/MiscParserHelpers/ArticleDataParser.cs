using PSA2.src.FileProcessor.MovesetParser.Configs;
using PSA2.src.models.fighter;
using PSA2.src.models.fighter.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.MiscParserHelpers
{
    public class ArticleDataParser
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public CharacterSpecificParametersConfig CharacterSpecificParametersConfig { get; private set; }

        public ArticleDataParser(PsaFile psaFile, int dataSectionLocation, CharacterSpecificParametersConfig characterSpecificParametersConfig)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            CharacterSpecificParametersConfig = characterSpecificParametersConfig;
        }

        public StaticArticles GetStaticArticles()
        {
            StaticArticles staticArticles = new StaticArticles();
            // static articles count
            if (PsaFile.FileContent[DataSectionLocation + 25] >= 8096 && PsaFile.FileContent[DataSectionLocation + 25] < PsaFile.DataSectionSize)
            {
                int staticArticleLocation = PsaFile.FileContent[DataSectionLocation + 25] / 4;
                if (PsaFile.FileContent[staticArticleLocation] >= 8096 && PsaFile.FileContent[staticArticleLocation] < PsaFile.DataSectionSize)
                {
                    int staticArticleDataLocation = PsaFile.FileContent[staticArticleLocation] / 4;
                    int numberOfStaticArticles = PsaFile.FileContent[staticArticleLocation + 1];
                    if (numberOfStaticArticles > 0 && numberOfStaticArticles < 21)
                    {
                        staticArticles.ArticleCount = numberOfStaticArticles;
                        Console.WriteLine(String.Format("Static Articles count: {0}", staticArticles.ArticleCount));
                        for (int i = 0; i < numberOfStaticArticles; i++)
                        {
                            // checks if subaction data exists at all
                            if (PsaFile.FileContent[staticArticleDataLocation + 4] >= 8096 && PsaFile.FileContent[staticArticleDataLocation + 4] < PsaFile.DataSectionSize)
                            {
                                if (PsaFile.FileContent[staticArticleDataLocation + 7] >= 8096 && PsaFile.FileContent[staticArticleDataLocation + 7] < PsaFile.DataSectionSize)
                                {
                                    Console.WriteLine(String.Format("Static Article {0} has a SubAction GFX Section", i));
                                }
                                else
                                {
                                    Console.WriteLine(String.Format("Static Article {0} has a SubAction Section", i));
                                }
                            }
                            if (PsaFile.FileContent[staticArticleDataLocation + 12] >= 8096 && PsaFile.FileContent[staticArticleDataLocation + 12] < PsaFile.DataSectionSize)
                            {
                                Console.WriteLine(String.Format("Static Article {0} has a Data3 Section", i));
                            }
                        }
                    }
                }
            }
            return staticArticles;
        }

        public EntryArticle GetEntryArticle()
        {
            EntryArticle entryArticle = new EntryArticle();
            // entry article existence (there appears to only be 1 entry article allowed)
            if (PsaFile.FileContent[DataSectionLocation + 26] >= 8096 && PsaFile.FileContent[DataSectionLocation + 26] < PsaFile.DataSectionSize)
            {
                Console.WriteLine("Entry Article Exists");
                int entryArticleLocation = PsaFile.FileContent[DataSectionLocation + 26] / 4;
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
    }
}
