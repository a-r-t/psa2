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

namespace PSA2.src.FileProcessor.MovesetParser
{
    public class PsaMovesetParser
    {
        public PsaFile PsaFile { get; set; }
        public Fighter Fighter { get; set; }

        public PsaMovesetParser(PsaFile psaFile)
        {
            PsaFile = psaFile;
            Fighter = new Fighter();
            ParseFighter();
        }

        public void ParseFighter()
        {
            string movesetBase = GetMovesetBase();
            List<Attribute> attributes = GetAttributes();
            List<DataEntry> dataTableEntries = GetDataTableEntries();
            int dataSectionOffset = dataTableEntries.Find(dte => dte.Name == "data").Offset;
            Console.WriteLine(String.Format("Data Section Offset: {0}", dataSectionOffset));
            List<DataEntry> externalDataEntries = GetExternalDataEntries();
            int numberOfSpecialActions = GetNumberOfSpecialActions(dataSectionOffset);
            int numberOfSubActions = GetNumberOfSubActions(dataSectionOffset);
            //LoadCharacterSpecificParameters();
            Dictionary<string, string> dataOffsets = GetDataOffsets(dataSectionOffset);
            ModelVisibility modelVisibility = GetModelVisibilityData(dataSectionOffset);
        }


        private string GetMovesetBase()
        {
            StringBuilder movesetBase = new StringBuilder();
            int nameEndByteIndex = 4;
            while (true)
            {
                string nextStringData = Utils.ConvertWordToString(PsaFile.FileHeader[nameEndByteIndex]);
                movesetBase.Append(nextStringData);
                if (nextStringData.Length == 4)
                {
                    nameEndByteIndex++;
                }
                else
                {
                    break;
                }
            }
            Console.WriteLine(movesetBase.ToString());
            return movesetBase.ToString();
        }

        private List<DataEntry> GetDataTableEntries()
        {
            List<DataEntry> dataTableEntries = new List<DataEntry>();
            int dataElementNameEntriesStartLocation = PsaFile.ExternalDataSectionStartLocation + PsaFile.NumberOfExternalSubRoutines * 2;
            for (int i = 0; i < PsaFile.NumberOfDataTableEntries; i++)
            {
                int dataOffset = PsaFile.FileContent[PsaFile.DataTableSectionStartLocation + (i * 2)] / 4;
                int nameStringOffset = PsaFile.FileContent[PsaFile.DataTableSectionStartLocation + 1 + (i * 2)];
                int startBit = nameStringOffset;
                StringBuilder dataEntryName = new StringBuilder();
                while (true)
                {
                    string nextStringData = Utils.ConvertWordToString(PsaFile.FileContent[dataElementNameEntriesStartLocation + startBit / 4], startByte: startBit % 4);

                    if (nextStringData.Length != 0)
                    {
                        dataEntryName.Append(nextStringData);
                        startBit += nextStringData.Length;
                    }
                    else
                    {
                        break;
                    }
                }
                dataTableEntries.Add(new DataEntry(dataOffset, dataEntryName.ToString()));
            }

            foreach (DataEntry dte in dataTableEntries)
            {
                Console.WriteLine(String.Format("Name: {0}, Offset: {1}", dte.Name, dte.Offset));
            }

            return dataTableEntries;
        }

        private List<DataEntry> GetExternalDataEntries()
        {
            List<DataEntry> externalDataEntries = new List<DataEntry>();
            int dataElementNameEntriesStartLocation = PsaFile.ExternalDataSectionStartLocation + PsaFile.NumberOfExternalSubRoutines * 2;
            for (int i = 0; i < PsaFile.NumberOfExternalSubRoutines; i++)
            {
                int dataOffset = PsaFile.FileContent[PsaFile.ExternalDataSectionStartLocation + (i * 2)] / 4;
                int nameStringOffset = PsaFile.FileContent[PsaFile.ExternalDataSectionStartLocation + 1 + (i * 2)];
                int startBit = nameStringOffset;
                StringBuilder dataEntryName = new StringBuilder();
                while (true)
                {
                    string nextStringData = Utils.ConvertWordToString(PsaFile.FileContent[dataElementNameEntriesStartLocation + startBit / 4], startByte: startBit % 4);

                    if (nextStringData.Length != 0)
                    {
                        dataEntryName.Append(nextStringData);
                        startBit += nextStringData.Length;
                    }
                    else
                    {
                        break;
                    }
                }
                externalDataEntries.Add(new DataEntry(dataOffset, dataEntryName.ToString()));
            }

            foreach (DataEntry dte in externalDataEntries)
            {
                Console.WriteLine(String.Format("Name: {0}, Offset: {1}", dte.Name, dte.Offset));
            }
            return externalDataEntries;
        }

        private int GetNumberOfSpecialActions(int dataSectionOffset)
        {
            Console.WriteLine(String.Format("Number of Special Actions: {0}", (PsaFile.FileContent[dataSectionOffset + 10] - PsaFile.FileContent[dataSectionOffset + 9]) / 4));
            return (PsaFile.FileContent[dataSectionOffset + 10] - PsaFile.FileContent[dataSectionOffset + 9]) / 4;
        }

        private int GetNumberOfSubActions(int dataSectionOffset)
        {
            Console.WriteLine(String.Format("Number of Sub Actions: {0}", (PsaFile.FileContent[dataSectionOffset + 13] - PsaFile.FileContent[dataSectionOffset + 12]) / 4));
            return (PsaFile.FileContent[dataSectionOffset + 13] - PsaFile.FileContent[dataSectionOffset + 12]) / 4;
        }

        private Dictionary<string, string> GetDataOffsets(int dataSectionOffset)
        {
            List<string> offsetNames = new List<string>
            {
                "SubActionFlags",
                "ModelVisibility",
                "Attributes",
                "SSEAttributes",
                "MiscSection",
                "CommonActionFlags",
                "SpecialActionFlags",
                "ExtraActionFlags",
                "ActionInterrupts",
                "EntrySpecials",
                "ExitSpecials",
                "ActionPre",
                "SubActionMain",
                "SubActionGFX",
                "SubActionSFX",
                "SubActionOther",
                "BoneFloats1",
                "BoneFloats2",
                "BoneReferences",
                "HandBones",
                "EntryActionOverride",
                "ExitActionOverride",
                "ExtraActionInterrupts",
                "BoneFloats3",
                "Unknown24",
                "StaticArticles",
                "EntryArticles",
                "DataFlags0",
                "DataFlags1",
                "DataFlags2",
                "DataFlags3"
            };
            Dictionary<string, string> dataOffsets = new Dictionary<string, string>();
            for (int i = 0; i < offsetNames.Count; i++)
            {
                dataOffsets.Add(offsetNames[i], Utils.ConvertIntToOffset(PsaFile.FileContent[dataSectionOffset + i]));
            }
            foreach (KeyValuePair<string, string> pair in dataOffsets)
            {
                Console.WriteLine(String.Format("{0}:{1}", pair.Key, pair.Value));
            }
            return dataOffsets;
        }

        private void LoadCharacterSpecificParameters()
        {
            CharacterSpecificParametersConfig characterSpecificParametersConfig = Utils.LoadJson<CharacterSpecificParametersConfig>("data/char_specific/FitLucario.json");
            //Console.WriteLine(characterSpecificParametersConfig.Articles[0].ArticleParameters[0].Name);
        }

        private List<Attribute> GetAttributes()
        {
            const int TOTAL_NUMBER_OF_ATTRIBUTES = 185;
            AttributeConfig attributesConfig = Utils.LoadJson<AttributeConfig>("data/attribute_data.json");
            List<Attribute> attributes = new List<Attribute>();

            for (int i = 0; i < TOTAL_NUMBER_OF_ATTRIBUTES; i++)
            {
                AttributeConfig.AttributeData attributeData = attributesConfig.Attributes[i];
                if (attributesConfig.Attributes[i].Type == "int")
                {
                    attributes.Add(
                        new IntAttribute(
                            attributeData.Name, 
                            attributeData.Description, 
                            attributeData.Location, 
                            PsaFile.FileContent[i], 
                            PsaFile.FileContent[i + TOTAL_NUMBER_OF_ATTRIBUTES]));
                }
                else
                {
                    attributes.Add(
                        new FloatAttribute(
                            attributeData.Name,
                            attributeData.Description,
                            attributeData.Location,
                            Utils.ConvertBytesToFloat(PsaFile.FileContent[i]),
                            Utils.ConvertBytesToFloat(PsaFile.FileContent[i + TOTAL_NUMBER_OF_ATTRIBUTES])));
                }
            }

            foreach (Attribute attribute in attributes)
            {
                if (attribute is IntAttribute)
                {
                    IntAttribute intAttribute = (IntAttribute)attribute;
                    Console.WriteLine(String.Format("Name: {0}, Value: {1}, SSE Value: {2}", intAttribute.Name, intAttribute.Value, intAttribute.SseValue));
                }
                else
                {
                    FloatAttribute floatAttribute = (FloatAttribute)attribute;
                    Console.WriteLine(String.Format("Name: {0}, Value: {1}, SSE Value: {2}", floatAttribute.Name, floatAttribute.Value, floatAttribute.SseValue));
                }
            }

            return attributes;
        }

        private ModelVisibility GetModelVisibilityData(int dataSectionOffset)
        {
            ModelVisibility modelVisibility = new ModelVisibility();
            // idk what this if statement is for -- maybe this assures that there is model visibility data?
            if (PsaFile.FileContent[dataSectionOffset + 1] >= 8096 && PsaFile.FileContent[dataSectionOffset + 1] < PsaFile.DataSectionSize)
            {

                int modelVisiblityStartLocation = PsaFile.FileContent[dataSectionOffset + 1] / 4;

                // idk what this if statement is for
                if (PsaFile.FileContent[modelVisiblityStartLocation] >= 8096 && PsaFile.FileContent[modelVisiblityStartLocation] < PsaFile.DataSectionSize)
                {
                    int modelVisibilitySectionStartLocation = PsaFile.FileContent[modelVisiblityStartLocation] / 4;
                    int numberOfBoneSwitches = PsaFile.FileContent[modelVisiblityStartLocation + 1];

                    // if g is between 01 and FF?? -- actually might be how many bone switches there are in a section...
                    if (numberOfBoneSwitches > 0 && numberOfBoneSwitches < 256)
                    {
                        // these are the two model visibility sections
                        // hidden is for the model, visible is for shadow -- these names will be changed in the future to better represent what they are
                        List<string> modelVisibilitySectionNames = new List<string>
                        {
                            "Hidden",
                            "Visible"
                        };

                        // gets both model changers for "hidden" and "visible" sections
                        for (int i = 0; i < modelVisibilitySectionNames.Count; i++)
                        {
                            // this checks if a hidden section exists?
                            if (PsaFile.FileContent[modelVisibilitySectionStartLocation + i] >= 8096 && PsaFile.FileContent[modelVisibilitySectionStartLocation + i] < PsaFile.DataSectionSize)
                            {
                                ModelVisibility.Section modelVisibilitySection = new ModelVisibility.Section();
                                modelVisibility.Sections.Add(modelVisibilitySection);
                                modelVisibilitySection.Name = modelVisibilitySectionNames[i];

                                int boneSwitchStartLocation = PsaFile.FileContent[modelVisibilitySectionStartLocation + i] / 4;
                                for (int j = 0; j < numberOfBoneSwitches; j++)
                                {
                                    ModelVisibility.BoneSwitch boneSwitch = new ModelVisibility.BoneSwitch();
                                    modelVisibilitySection.BoneSwitches.Add(boneSwitch);
                                    int numberOfBoneGroups = PsaFile.FileContent[boneSwitchStartLocation + j * 2 + 1];

                                    // if there's bone groups for bone switch maybe?
                                    if (numberOfBoneGroups > 0 && numberOfBoneGroups < 256 && PsaFile.FileContent[boneSwitchStartLocation + j * 2] >= 8096 && PsaFile.FileContent[boneSwitchStartLocation + j * 2] < PsaFile.DataSectionSize)
                                    {
                                        int boneGroupStartLocation = PsaFile.FileContent[boneSwitchStartLocation + j * 2] / 4;

                                        // looping through the bone groups
                                        for (int k = 0; k < numberOfBoneGroups; k++)
                                        {
                                            ModelVisibility.BoneGroup boneGroup = new ModelVisibility.BoneGroup();
                                            boneSwitch.BoneGroups.Add(boneGroup);

                                            int numberOfBones = PsaFile.FileContent[boneGroupStartLocation + k * 2 + 1];
                                            // this checks if bone group has any bone data? holy f
                                            if (PsaFile.FileContent[boneGroupStartLocation + k * 2] >= 8096 && PsaFile.FileContent[boneGroupStartLocation + k * 2] < PsaFile.DataSectionSize &&
                                                numberOfBones > 0 && numberOfBones < 256)
                                            {
                                                // bones exist
                                                //Console.WriteLine("Number of bones: " + numberOfBones);
                                                boneGroup.numberOfBones = numberOfBones;
                                            }

                                        }
                                    }
                                }
                            }
                        }

                        // gets data sections (specify which bone group in a bone switch to start on -- optional, otherwise I believe a bone switch defaults to starting on bone group 0)
                        int numberOfDataSections = PsaFile.FileContent[modelVisiblityStartLocation + 3];
                        if (PsaFile.FileContent[modelVisiblityStartLocation + 2] >= 8096 && PsaFile.FileContent[modelVisiblityStartLocation + 2] < PsaFile.DataSectionSize &&
                            numberOfDataSections > 0 && numberOfDataSections < 256)
                        {
                            for (int i = 0; i < numberOfDataSections; i++)
                            {
                                modelVisibility.SectionsData.Add(new ModelVisibility.SectionData());
                            }
                        }
                    }
                }
            }

            Console.WriteLine("ModelVisibility Sections");
            foreach (ModelVisibility.Section section in modelVisibility.Sections)
            {
                Console.WriteLine(String.Format("Section Name: {0}", section.Name));
                Console.WriteLine(String.Format("Number of bone switches: {0}", section.BoneSwitches.Count));
                foreach (ModelVisibility.BoneSwitch boneSwitch in section.BoneSwitches)
                {
                    Console.WriteLine(String.Format("Number of bone groups: {0}", boneSwitch.BoneGroups.Count));
                    foreach (ModelVisibility.BoneGroup boneGroup in boneSwitch.BoneGroups)
                    {
                        Console.WriteLine(String.Format("Bone count: {0}", boneGroup.numberOfBones));
                    }
                }
            }
            Console.WriteLine("Number of data sections: {0}", modelVisibility.SectionsData.Count);
            return modelVisibility;
        }
    }
}
