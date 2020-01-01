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
            List<DataEntry> dataTableEntries = GetDataTableEntries();
            int dataSectionOffset = dataTableEntries.Find(dte => dte.Name == "data").Offset;

            /*
            string movesetBase = GetMovesetBase();
            List<Attribute> attributes = GetAttributes();
            Console.WriteLine(String.Format("Data Section Offset: {0}", dataSectionOffset));
            List<DataEntry> externalDataEntries = GetExternalDataEntries();
            int numberOfSpecialActions = GetNumberOfSpecialActions(dataSectionOffset);
            int numberOfSubActions = GetNumberOfSubActions(dataSectionOffset);
            LoadCharacterSpecificParameters();
            Dictionary<string, string> dataOffsets = GetDataOffsets(dataSectionOffset);
            ModelVisibility modelVisibility = GetModelVisibilityData(dataSectionOffset);
            */

            LoadMiscSection(dataSectionOffset);
            Console.WriteLine("MovesetSize=0x" + PsaFile.MovesetFileSize.ToString("X"));
            Console.WriteLine("TotalFileSize=0x" + (PsaFile.FileSize + 128).ToString("X"));
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

        private void LoadMiscSection(int dataSectionOffset)
        {
            FinalSmashAura finalSmashAura = new FinalSmashAura();
            HurtBoxes hurtBoxes = new HurtBoxes();
            LedgeGrab ledgeGrab = new LedgeGrab();
            MiscSection2 miscSection2 = new MiscSection2();
            BoneReferences miscSectionBoneReferences = new BoneReferences();
            ItemBones itemBones = new ItemBones();
            SoundLists soundList = new SoundLists();
            MiscSection5 miscSection5 = new MiscSection5();
            MultiJump multiJump = new MultiJump();
            Glide glide = new Glide();
            Crawl crawl = new Crawl();
            CollisionData collisionData = new CollisionData();
            Tether tether = new Tether();
            MiscSection12 miscSection12 = new MiscSection12();
            CommonActionFlags commonActionFlags = new CommonActionFlags();
            SpecialActionFlags specialActionFlags = new SpecialActionFlags();
            ExtraActionFlags extraActionFlags = new ExtraActionFlags();
            Unknown24 unknown24 = new Unknown24();
            StaticArticles staticArticles = new StaticArticles();
            EntryArticle entryArticle = new EntryArticle();
            BoneFloats1 boneFloats1 = new BoneFloats1();
            BoneFloats2 boneFloats2 = new BoneFloats2();
            BoneFloats3 boneFloats3 = new BoneFloats3();
            BoneReferences boneReferences = new BoneReferences();
            HandBones handBones = new HandBones();
            ExtraActionInterrupts extraActionInterrupts = new ExtraActionInterrupts();
            ArticleExtraDatas articleExtraDatas = new ArticleExtraDatas();
            DataFlags dataFlags = new DataFlags();

            // idk what this is for -- check if there's a misc section at all maybe?
            if (PsaFile.FileContent[dataSectionOffset + 4] >= 8096 && PsaFile.FileContent[dataSectionOffset + 4] < PsaFile.DataSectionSize)
            {
                int miscSectionLocation = PsaFile.FileContent[dataSectionOffset + 4] / 4;
                // checks if there is a misc section 1
                if (PsaFile.FileContent[miscSectionLocation] >= 8096 && PsaFile.FileContent[miscSectionLocation] < PsaFile.DataSectionSize)
                {
                    // counts final smash aura entries
                    if (PsaFile.FileContent[miscSectionLocation + 1] >= 8096 && PsaFile.FileContent[miscSectionLocation + 1] < PsaFile.DataSectionSize)
                    {
                        int numberOfFinalSmashAuraEntries = PsaFile.FileContent[miscSectionLocation + 2];
                        if (numberOfFinalSmashAuraEntries > 0 && numberOfFinalSmashAuraEntries < 256)
                        {
                            // Idk where this is used...
                            // int n = PsaFile.FileContent[miscSectionLocation + 1] / 4;

                            finalSmashAura.FinalSmashAuraEntryCount = numberOfFinalSmashAuraEntries;
                        }
                    }
                    Console.WriteLine(String.Format("Number Of Final Smash Aura Entries: {0}", finalSmashAura.FinalSmashAuraEntryCount));


                    // counts hurtboxes entries
                    if (PsaFile.FileContent[miscSectionLocation + 3] >= 8096 && PsaFile.FileContent[miscSectionLocation + 3] < PsaFile.DataSectionSize)
                    {
                        int numberOfHurtBoxEntries = PsaFile.FileContent[miscSectionLocation + 4];
                        if (numberOfHurtBoxEntries > 0 && numberOfHurtBoxEntries < 256)
                        {

                            // Idk where this is used...
                            //int n = PsaFile.FileContent[miscSectionLocation + 3] / 4 + 7;

                            hurtBoxes.HurtBoxEntryCount = numberOfHurtBoxEntries;
                        }
                    }
                    Console.WriteLine(String.Format("Number Of Hurt Box Entries: {0}", hurtBoxes.HurtBoxEntryCount));


                    // counts ledge grab
                    if (PsaFile.FileContent[miscSectionLocation + 5] >= 8096 && PsaFile.FileContent[miscSectionLocation + 5] < PsaFile.DataSectionSize)
                    {
                        int numberOfLedgeGrabEntries = PsaFile.FileContent[miscSectionLocation + 6];
                        if (numberOfLedgeGrabEntries > 0 && numberOfLedgeGrabEntries < 256)
                        {
                            ledgeGrab.LedgeGrabeEntriesCount = numberOfLedgeGrabEntries;
                        }
                    }
                    Console.WriteLine(String.Format("Number Of Ledge Grab Entries: {0}", ledgeGrab.LedgeGrabeEntriesCount));


                    // counts misc section 2 entries
                    if (PsaFile.FileContent[miscSectionLocation + 7] >= 8096 && PsaFile.FileContent[miscSectionLocation + 7] < PsaFile.DataSectionSize)
                    {
                        int numberOfMiscSection2Entries = PsaFile.FileContent[miscSectionLocation + 8];
                        if (numberOfMiscSection2Entries > 0 && numberOfMiscSection2Entries < 256)
                        {
                            miscSection2.EntriesCount = numberOfMiscSection2Entries;
                        }
                    }
                    Console.WriteLine(String.Format("Number Of Misc Section 2 Entries: {0}", miscSection2.EntriesCount));


                    // checks if bone references section exists (inside misc section) but does not count them
                    if (PsaFile.FileContent[miscSectionLocation + 9] >= 8096 && PsaFile.FileContent[miscSectionLocation + 9] < PsaFile.DataSectionSize)
                    {
                        Console.WriteLine("Bone References Section (In Misc Section) exists");
                    } else
                    {
                        Console.WriteLine("Bone References Section (In Misc Section) does NOT exist");
                    }

                    // counts item bones entries
                    if (PsaFile.FileContent[miscSectionLocation + 10] >= 8096 && PsaFile.FileContent[miscSectionLocation + 10] < PsaFile.DataSectionSize)
                    {
                        int itemBonesEntriesLocation = PsaFile.FileContent[miscSectionLocation + 10] / 4;
                        if (PsaFile.FileContent[itemBonesEntriesLocation + 4] >= 8096 && PsaFile.FileContent[itemBonesEntriesLocation + 4] < PsaFile.DataSectionSize)
                        {
                            int numberOfItemBonesEntries = PsaFile.FileContent[itemBonesEntriesLocation + 3];
                            if (numberOfItemBonesEntries > 0 && numberOfItemBonesEntries < 256)
                            {
                                itemBones.EntriesCount = numberOfItemBonesEntries;
                            }
                        }
                    }
                    Console.WriteLine(String.Format("Number Of Item Bone Entries: {0}", itemBones.EntriesCount));

                    // counts sound data section
                    if (PsaFile.FileContent[miscSectionLocation + 11] >= 8096 && PsaFile.FileContent[miscSectionLocation + 11] < PsaFile.DataSectionSize)
                    {
                        int soundListsLocation = PsaFile.FileContent[miscSectionLocation + 11] / 4;
                        if (PsaFile.FileContent[soundListsLocation] >= 8096 && PsaFile.FileContent[soundListsLocation] < PsaFile.DataSectionSize) {
                            int soundListEntriesLocation = PsaFile.FileContent[soundListsLocation] / 4;
                            int numberOfSoundListEntries = PsaFile.FileContent[soundListsLocation + 1];
                            if (numberOfSoundListEntries > 0 && numberOfSoundListEntries < 256)
                            {
                                for (int i = 0; i < numberOfSoundListEntries; i++)
                                {
                                    // how many sound datas there are
                                    if (PsaFile.FileContent[soundListEntriesLocation + i * 2] >= 8096 && PsaFile.FileContent[soundListEntriesLocation + i * 2] < PsaFile.DataSectionSize)
                                    {
                                        soundList.SoundListCount++;
                                    }
                                }
                                Console.WriteLine(String.Format("Number of Sound List Entries: {0}", soundList.SoundListCount));
                            }
                        }
                    }

                    // checks if there is a Misc Section 5
                    if (PsaFile.FileContent[miscSectionLocation + 12] >= 8096 && PsaFile.FileContent[miscSectionLocation + 12] < PsaFile.DataSectionSize)
                    {
                        Console.WriteLine("Misc Section 5 exists");
                    }
                    else
                    {
                        Console.WriteLine("Misc Section 5 does NOT exist");
                    }

                    // checks if multi jump section exists, checks if there is a hops section and multi jump unknown section
                    if (PsaFile.FileContent[miscSectionLocation + 13] >= 8096 && PsaFile.FileContent[miscSectionLocation + 13] < PsaFile.DataSectionSize)
                    {
                        Console.WriteLine("Mutli Jump Section exists");
                        int multiJumpLocation = PsaFile.FileContent[miscSectionLocation + 13] / 4;
                        if (PsaFile.FileContent[multiJumpLocation + 4] >= 8096 && PsaFile.FileContent[multiJumpLocation + 4] < PsaFile.DataSectionSize)
                        {
                            Console.WriteLine("Hops section exists");
                        }

                        if (PsaFile.FileContent[multiJumpLocation + 5] >= 8096 && PsaFile.FileContent[multiJumpLocation + 5] < PsaFile.DataSectionSize)
                        {
                            Console.WriteLine("Muli Jump Unknown section exists");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Mutli Jump Section does NOT exist");
                    }

                    // checks if glide section exists
                    if (PsaFile.FileContent[miscSectionLocation + 14] >= 8096 && PsaFile.FileContent[miscSectionLocation + 14] < PsaFile.DataSectionSize)
                    {
                        Console.WriteLine("Glide section exists");
                    }
                    else
                    {
                        Console.WriteLine("Glide section does NOT exist");
                    }

                    // checks if crawl section exists
                    if (PsaFile.FileContent[miscSectionLocation + 15] >= 8096 && PsaFile.FileContent[miscSectionLocation + 15] < PsaFile.DataSectionSize)
                    {
                        Console.WriteLine("Crawl section exists");
                    }
                    else
                    {
                        Console.WriteLine("Crawl section does NOT exist");
                    }

                    // checks if there is collision data
                    if (PsaFile.FileContent[miscSectionLocation + 16] >= 8096 && PsaFile.FileContent[miscSectionLocation + 16] < PsaFile.DataSectionSize)
                    {
                        int collisionDataLocation = PsaFile.FileContent[miscSectionLocation + 16] / 4;
                        if (PsaFile.FileContent[collisionDataLocation] >= 8096 && PsaFile.FileContent[collisionDataLocation] < PsaFile.DataSectionSize)
                        {
                            int collisionEntryLocation = PsaFile.FileContent[collisionDataLocation] / 4;
                            if (PsaFile.FileContent[collisionEntryLocation] >= 8096 && PsaFile.FileContent[collisionEntryLocation] < PsaFile.DataSectionSize)
                            {
                                int collisionBoneDataLocation = PsaFile.FileContent[collisionEntryLocation] / 4;
                                if (PsaFile.FileContent[collisionBoneDataLocation + 1] >= 8096 && PsaFile.FileContent[collisionBoneDataLocation + 1] < PsaFile.DataSectionSize)
                                {
                                    // guessing but I think this is right
                                    int collisionBoneDataCount = PsaFile.FileContent[collisionBoneDataLocation + 2];
                                    if (collisionBoneDataCount > 0 && collisionBoneDataCount < 256)
                                    {
                                        collisionData.Count = collisionBoneDataCount;
                                    }

                                }
                            }
                        }
                    }
                    Console.WriteLine(String.Format("Collision Data bone count: {0}", collisionData.Count));

                    // checks if tether exists
                    if (PsaFile.FileContent[miscSectionLocation + 17] >= 8096 && PsaFile.FileContent[miscSectionLocation + 17] < PsaFile.DataSectionSize)
                    {
                        Console.WriteLine("Tether section exists");
                    }
                    else
                    {
                        Console.WriteLine("Tether section does NOT exist");
                    }

                    // gets count of entries in Misc Section 12
                    if (PsaFile.FileContent[miscSectionLocation + 18] >= 8096 && PsaFile.FileContent[miscSectionLocation + 18] < PsaFile.DataSectionSize)
                    {
                        int miscSection12Location = PsaFile.FileContent[miscSectionLocation + 18] / 4;

                        if (PsaFile.FileContent[miscSection12Location] >= 8096 && PsaFile.FileContent[miscSection12Location] < PsaFile.DataSectionSize)
                        {
                            int numberOfMiscSection12Entries = PsaFile.FileContent[miscSection12Location + 1];
                            if (numberOfMiscSection12Entries > 0 && numberOfMiscSection12Entries < 256)
                            {
                                miscSection12.DataCount = numberOfMiscSection12Entries;
                            }
                        }
                    }
                    Console.WriteLine(String.Format("Misc Section 12 entries count: {0}", miscSection12.DataCount));

                    // counts number of common action flags
                    if (PsaFile.FileContent[dataSectionOffset + 5] >= 1480 && PsaFile.FileContent[dataSectionOffset + 5] < PsaFile.DataSectionSize)
                    {
                        commonActionFlags.ActionFlagsCount = 274;
                    }
                    Console.WriteLine(String.Format("Common Action Flags count: {0}", commonActionFlags.ActionFlagsCount));

                    // counts number of special action flags
                    if (PsaFile.FileContent[dataSectionOffset + 6] >= 1480 && PsaFile.FileContent[dataSectionOffset + 6] < PsaFile.DataSectionSize)
                    {
                        int numberOfSpecialActions = GetNumberOfSpecialActions(dataSectionOffset);

                        specialActionFlags.ActionFlagsCount = 274 + numberOfSpecialActions;
                    }
                    Console.WriteLine(String.Format("Special Action Flags count: {0}", specialActionFlags.ActionFlagsCount));

                    // counts number of extra action flags
                    if (PsaFile.FileContent[dataSectionOffset + 7] >= 1480 && PsaFile.FileContent[dataSectionOffset + 7] < PsaFile.DataSectionSize)
                    {
                        int numberOfSpecialActions = GetNumberOfSpecialActions(dataSectionOffset);

                        extraActionFlags.ActionFlagsCount = 274 + numberOfSpecialActions;
                    }
                    Console.WriteLine(String.Format("Extra Action Flags count: {0}", extraActionFlags.ActionFlagsCount));

                    // check if action interrupt section exists
                    if (PsaFile.FileContent[dataSectionOffset + 8] >= 8096 && PsaFile.FileContent[dataSectionOffset + 8] < PsaFile.DataSectionSize)
                    {
                        Console.WriteLine("Action Interrupt section exists");
                    }
                    else
                    {
                        Console.WriteLine("Action Interrupt section does NOT exist");
                    }

                    // counts number of bone floats 1
                    if (PsaFile.FileContent[dataSectionOffset + 16] >= 8096 && PsaFile.FileContent[dataSectionOffset + 16] < PsaFile.DataSectionSize)
                    {
                        int boneFloats1Location = PsaFile.FileContent[dataSectionOffset + 16] / 4;
                        boneFloats1.EntriesCount = 3;
                    }
                    Console.WriteLine(String.Format("Bone Floats 1 count: {0}", boneFloats1.EntriesCount));

                    // counts number of bone floats 2
                    // idk why this one is so different
                    if (PsaFile.FileContent[dataSectionOffset + 17] >= 8096 && PsaFile.FileContent[dataSectionOffset + 17] < PsaFile.DataSectionSize)
                    {
                        int numberOfBoneFloats2 = (PsaFile.FileContent[dataSectionOffset + 18] - PsaFile.FileContent[dataSectionOffset + 17]) / 28;
                        if (numberOfBoneFloats2 > 0 && numberOfBoneFloats2 <= 25)
                        {
                            boneFloats2.EntriesCount = numberOfBoneFloats2;
                        }
                    }
                    Console.WriteLine(String.Format("Bone Floats 2 count: {0}", boneFloats2.EntriesCount));

                    // counts number of bone floats 3
                    if (PsaFile.FileContent[dataSectionOffset + 23] >= 8096 && PsaFile.FileContent[dataSectionOffset + 23] < PsaFile.DataSectionSize)
                    {
                        int boneFloats3Location = PsaFile.FileContent[dataSectionOffset + 23] / 4;
                        if (PsaFile.FileContent[dataSectionOffset + 23] < PsaFile.FileContent[dataSectionOffset + 18] && PsaFile.FileContent[dataSectionOffset + 18] < PsaFile.DataSectionSize)
                        {
                            int numberOfBoneFloats3 = (PsaFile.FileContent[dataSectionOffset + 18] - PsaFile.FileContent[dataSectionOffset + 23]) / 28;
                            if (numberOfBoneFloats3 > 0 && numberOfBoneFloats3 <= 25)
                            {
                                boneFloats3.EntriesCount = numberOfBoneFloats3;
                            }
                        }
                        Console.WriteLine(String.Format("Bone Floats 3 count: {0}", boneFloats3.EntriesCount));
                    }

                    // checks if bone references section exists (the other one, NOT the one in misc section)
                    if (PsaFile.FileContent[dataSectionOffset + 18] >= 8096 && PsaFile.FileContent[dataSectionOffset + 18] < PsaFile.DataSectionSize)
                    {
                        Console.WriteLine("Bone References Section exists");
                    }
                    else
                    {
                        Console.WriteLine("Bone References Section does NOT exist");
                    }

                    // gets hand bones count
                    if (PsaFile.FileContent[dataSectionOffset + 19] >= 8096 && PsaFile.FileContent[dataSectionOffset + 19] < PsaFile.DataSectionSize)
                    {
                        int handBonesLocation = PsaFile.FileContent[dataSectionOffset + 19] / 4 + 4; // why the plus 4?
                        if (PsaFile.FileContent[handBonesLocation + 1] >= 8096 && PsaFile.FileContent[handBonesLocation + 1] < PsaFile.DataSectionSize)
                        {
                            int numberOfHandBones = PsaFile.FileContent[handBonesLocation];
                            if (numberOfHandBones > 0 && numberOfHandBones < 256)
                            {
                                handBones.DataCount = numberOfHandBones;
                            }
                        }
                    }
                    Console.WriteLine(String.Format("Hand Bones count: {0}", handBones.DataCount));

                    // apparently extra action interrupts is added automatically?
                    Console.WriteLine("Extra Action Interrupts exists...by default?");

                    // gets unknown24 count
                    if (PsaFile.FileContent[dataSectionOffset + 24] >= 8096 && PsaFile.FileContent[dataSectionOffset + 24] < PsaFile.DataSectionSize)
                    {
                        int unknown24Location = PsaFile.FileContent[dataSectionOffset + 24] / 4;
                        if (PsaFile.FileContent[unknown24Location] >= 8096 && PsaFile.FileContent[unknown24Location] < PsaFile.DataSectionSize)
                        {
                            int numberOfUnknown24Entries = PsaFile.FileContent[unknown24Location + 1];
                            if (numberOfUnknown24Entries > 0 && numberOfUnknown24Entries < 256)
                            {
                                unknown24.DataCount = numberOfUnknown24Entries;
                            }
                        }
                    }
                    Console.WriteLine(String.Format("Unknown24 count: {0}", unknown24.DataCount));

                    // static articles count
                    if (PsaFile.FileContent[dataSectionOffset + 25] >= 8096 && PsaFile.FileContent[dataSectionOffset + 25] < PsaFile.DataSectionSize)
                    {
                        int staticArticleLocation = PsaFile.FileContent[dataSectionOffset + 25] / 4;
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

                    // entry article existence (there appears to only be 1 entry article allowed)
                    if (PsaFile.FileContent[dataSectionOffset + 26] >= 8096 && PsaFile.FileContent[dataSectionOffset + 26] < PsaFile.DataSectionSize)
                    {
                        Console.WriteLine("Entry Article Exists");
                        int entryArticleLocation = PsaFile.FileContent[dataSectionOffset + 26] / 4;
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

                    // article extra datas
                    // might be able to redo this using my file parsing method
                    Console.WriteLine("Article Extra Datas Section");
                    CharacterSpecificParametersConfig characterSpecificParametersConfig = GetCharacterSpecificParameters();
                    foreach (CharacterSpecificParametersConfig.Article article in characterSpecificParametersConfig.Articles)
                    {
                        Console.WriteLine(String.Format("Article {0} extra datas", article.Name));

                        int articleLocation = Convert.ToInt32(article.Location, 16);
                        int articleDataLocation = PsaFile.FileContent[dataSectionOffset + articleLocation / 4] / 4; // an4
                        if (PsaFile.FileContent[articleDataLocation + 3] >= 8096 && PsaFile.FileContent[articleDataLocation + 3] < PsaFile.DataSectionSize)
                        {
                            ArticleExtraDatasType1(article, dataSectionOffset, articleDataLocation);
                        }
                        else
                        {
                            ArticleExtraDatasType2(article, dataSectionOffset, articleDataLocation);
                        }

                    }

                    // Data Flags auto exists
                    Console.WriteLine("Data Flags exists");
                }
            }
        }

        private void ArticleExtraDatasType1(CharacterSpecificParametersConfig.Article article, int dataSectionOffset, int articleDataLocation)
        {
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
                            ModelVisibility.Section section = new ModelVisibility.Section();
                            modelVisibility.Sections.Add(section);
                            section.Name = "Hidden";
                            int sectionBoneSwitchLocation = PsaFile.FileContent[modelVisiblityHiddenSectionLocation] / 4;
                            for (int i = 0; i < numberOfBoneSwitches; i++)
                            {
                                ModelVisibility.BoneSwitch boneSwitch = new ModelVisibility.BoneSwitch();
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
                                            ModelVisibility.BoneGroup boneGroup = new ModelVisibility.BoneGroup();
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
                            ModelVisibility.Section section = new ModelVisibility.Section();
                            modelVisibility.Sections.Add(section);
                            section.Name = "Hidden";
                            int sectionBoneSwitchLocation = PsaFile.FileContent[modelVisiblityHiddenSectionLocation] / 4;
                            for (int i = 0; i < numberOfBoneSwitches; i++)
                            {
                                ModelVisibility.BoneSwitch boneSwitch = new ModelVisibility.BoneSwitch();
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
                                            ModelVisibility.BoneGroup boneGroup = new ModelVisibility.BoneGroup();
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

        }

        /// <summary>
        /// This is exactly the same as type1's article model visibility code on first glance...probs can easily combine these :)
        /// </summary>
        /// <param name="article"></param>
        /// <param name="dataSectionOffset"></param>
        /// <param name="articleDataLocation"></param>
        private void ArticleExtraDatasType2(CharacterSpecificParametersConfig.Article article, int dataSectionOffset, int articleDataLocation)
        {
            // this was originally:
            /*
              if (alm[an4 + 9] <= 8096 || alm[an4 + 9] >= tds[25])
              {
                  break;
              }
            */
            // idk why considering none of the other code looks like that
            // ignoring for now and doing the "regular" if statement
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
                            ModelVisibility.Section section = new ModelVisibility.Section();
                            modelVisibility.Sections.Add(section);
                            section.Name = "Hidden";
                            int sectionBoneSwitchLocation = PsaFile.FileContent[modelVisiblityHiddenSectionLocation] / 4;
                            for (int i = 0; i < numberOfBoneSwitches; i++)
                            {
                                ModelVisibility.BoneSwitch boneSwitch = new ModelVisibility.BoneSwitch();
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
                                            ModelVisibility.BoneGroup boneGroup = new ModelVisibility.BoneGroup();
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
                            ModelVisibility.Section section = new ModelVisibility.Section();
                            modelVisibility.Sections.Add(section);
                            section.Name = "Hidden";
                            int sectionBoneSwitchLocation = PsaFile.FileContent[modelVisiblityHiddenSectionLocation] / 4;
                            for (int i = 0; i < numberOfBoneSwitches; i++)
                            {
                                ModelVisibility.BoneSwitch boneSwitch = new ModelVisibility.BoneSwitch();
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
                                            ModelVisibility.BoneGroup boneGroup = new ModelVisibility.BoneGroup();
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
        }

        private CharacterSpecificParametersConfig GetCharacterSpecificParameters()
        {
            // at the moment hardcoded for Lucario for testing
            return Utils.LoadJson<CharacterSpecificParametersConfig>("data/char_specific/FitLucario.json");
        }
    }
}
