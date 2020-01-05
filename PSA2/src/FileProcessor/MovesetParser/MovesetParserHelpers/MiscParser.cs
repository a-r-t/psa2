using PSA2.src.FileProcessor.MovesetParser.Configs;
using PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.MiscParserHelpers;
using PSA2.src.models.fighter.Misc;
using PSA2.src.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers
{
    public class MiscParser
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public string MovesetBaseName { get; private set; }
        public int NumberOfSpecialActions { get; private set; }
        public ModelVisibilityParser ModelVisibilityParser { get; private set; }
        public MiscSectionParser MiscSectionParser { get; private set; }
        public ArticleDataParser ArticleDataParser { get; private set; }

        public MiscParser(PsaFile psaFile, int dataSectionLocation, string movesetBaseName, int numberOfSpecialActions)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            MovesetBaseName = movesetBaseName;
            NumberOfSpecialActions = numberOfSpecialActions;

            ModelVisibilityParser = new ModelVisibilityParser(PsaFile, DataSectionLocation);

            // if misc section exists
            int miscSectionLocation = PsaFile.FileContent[DataSectionLocation + 4] / 4;
            if (PsaFile.IsValidDataSectionLocation(miscSectionLocation))
            {
                MiscSectionParser = new MiscSectionParser(PsaFile, DataSectionLocation, miscSectionLocation);
            }

            ArticleDataParser = new ArticleDataParser(PsaFile, DataSectionLocation, GetCharacterSpecificParameters());
        }

        public Dictionary<string, string> GetDataOffsets(int dataSectionOffset)
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

        public CharacterSpecificParametersConfig GetCharacterSpecificParameters()
        {
            return Utils.LoadJson<CharacterSpecificParametersConfig>(String.Format("data/char_specific/{0}.json", MovesetBaseName));
        }

        public CommonActionFlags GetCommonActionFlags()
        {
            CommonActionFlags commonActionFlags = new CommonActionFlags();
            // counts number of common action flags
            if (PsaFile.FileContent[DataSectionLocation + 5] >= 1480 && PsaFile.FileContent[DataSectionLocation + 5] < PsaFile.DataSectionSize)
            {
                commonActionFlags.ActionFlagsCount = 274;
            }
            Console.WriteLine(String.Format("Common Action Flags count: {0}", commonActionFlags.ActionFlagsCount));
            return commonActionFlags;
        }

        public SpecialActionFlags GetSpecialActionFlags()
        {
            SpecialActionFlags specialActionFlags = new SpecialActionFlags();
            // counts number of special action flags
            if (PsaFile.FileContent[DataSectionLocation + 6] >= 1480 && PsaFile.FileContent[DataSectionLocation + 6] < PsaFile.DataSectionSize)
            {
                specialActionFlags.ActionFlagsCount = 274 + NumberOfSpecialActions;
            }
            Console.WriteLine(String.Format("Special Action Flags count: {0}", specialActionFlags.ActionFlagsCount));
            return specialActionFlags;
        }

        public ExtraActionFlags GetExtraActionFlags()
        {
            ExtraActionFlags extraActionFlags = new ExtraActionFlags();
            // counts number of extra action flags
            if (PsaFile.FileContent[DataSectionLocation + 7] >= 1480 && PsaFile.FileContent[DataSectionLocation + 7] < PsaFile.DataSectionSize)
            {
                extraActionFlags.ActionFlagsCount = 274 + NumberOfSpecialActions;
            }
            Console.WriteLine(String.Format("Extra Action Flags count: {0}", extraActionFlags.ActionFlagsCount));
            return extraActionFlags;
        }

        public ActionInterrupts GetActionInterrupts()
        {
            ActionInterrupts actionInterrupts = new ActionInterrupts();
            // check if action interrupt section exists
            if (PsaFile.FileContent[DataSectionLocation + 8] >= 8096 && PsaFile.FileContent[DataSectionLocation + 8] < PsaFile.DataSectionSize)
            {
                Console.WriteLine("Action Interrupt section exists");
            }
            else
            {
                Console.WriteLine("Action Interrupt section does NOT exist");
            }
            return actionInterrupts;
        }

        public BoneFloats1 GetBoneFloats1()
        {
            BoneFloats1 boneFloats1 = new BoneFloats1();
            // counts number of bone floats 1
            if (PsaFile.FileContent[DataSectionLocation + 16] >= 8096 && PsaFile.FileContent[DataSectionLocation + 16] < PsaFile.DataSectionSize)
            {
                int boneFloats1Location = PsaFile.FileContent[DataSectionLocation + 16] / 4;
                boneFloats1.EntriesCount = 3;
            }
            Console.WriteLine(String.Format("Bone Floats 1 count: {0}", boneFloats1.EntriesCount));
            return boneFloats1;
        }

        public BoneFloats2 GetBoneFloats2()
        {
            BoneFloats2 boneFloats2 = new BoneFloats2();
            // counts number of bone floats 2
            // idk why this one is so different
            if (PsaFile.FileContent[DataSectionLocation + 17] >= 8096 && PsaFile.FileContent[DataSectionLocation + 17] < PsaFile.DataSectionSize)
            {
                int numberOfBoneFloats2 = (PsaFile.FileContent[DataSectionLocation + 18] - PsaFile.FileContent[DataSectionLocation + 17]) / 28;
                if (numberOfBoneFloats2 > 0 && numberOfBoneFloats2 <= 25)
                {
                    boneFloats2.EntriesCount = numberOfBoneFloats2;
                }
            }
            Console.WriteLine(String.Format("Bone Floats 2 count: {0}", boneFloats2.EntriesCount));
            return boneFloats2;
        }

        public BoneFloats3 GetBoneFloats3()
        {
            BoneFloats3 boneFloats3 = new BoneFloats3();
            // counts number of bone floats 3
            if (PsaFile.FileContent[DataSectionLocation + 23] >= 8096 && PsaFile.FileContent[DataSectionLocation + 23] < PsaFile.DataSectionSize)
            {
                int boneFloats3Location = PsaFile.FileContent[DataSectionLocation + 23] / 4;
                if (PsaFile.FileContent[DataSectionLocation + 23] < PsaFile.FileContent[DataSectionLocation + 18] && PsaFile.FileContent[DataSectionLocation + 18] < PsaFile.DataSectionSize)
                {
                    int numberOfBoneFloats3 = (PsaFile.FileContent[DataSectionLocation + 18] - PsaFile.FileContent[DataSectionLocation + 23]) / 28;
                    if (numberOfBoneFloats3 > 0 && numberOfBoneFloats3 <= 25)
                    {
                        boneFloats3.EntriesCount = numberOfBoneFloats3;
                    }
                }
                Console.WriteLine(String.Format("Bone Floats 3 count: {0}", boneFloats3.EntriesCount));
            }
            return boneFloats3;
        }

        public BoneReferences GetBoneReferences()
        {
            BoneReferences boneReferences = new BoneReferences();
            // checks if bone references section exists (the other one, NOT the one in misc section)
            if (PsaFile.FileContent[DataSectionLocation + 18] >= 8096 && PsaFile.FileContent[DataSectionLocation + 18] < PsaFile.DataSectionSize)
            {
                Console.WriteLine("Bone References Section exists");
            }
            else
            {
                Console.WriteLine("Bone References Section does NOT exist");
            }
            return boneReferences;
        }

        public HandBones GetHandBones()
        {
            HandBones handBones = new HandBones();
            // gets hand bones count
            if (PsaFile.FileContent[DataSectionLocation + 19] >= 8096 && PsaFile.FileContent[DataSectionLocation + 19] < PsaFile.DataSectionSize)
            {
                int handBonesLocation = PsaFile.FileContent[DataSectionLocation + 19] / 4 + 4; // why the plus 4?
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
            return handBones;
        }

        public ExtraActionInterrupts GetExtraActionInterrupts()
        {
            ExtraActionInterrupts extraActionInterrupts = new ExtraActionInterrupts();
            // apparently extra action interrupts is added automatically?
            Console.WriteLine("Extra Action Interrupts exists...by default?");
            return extraActionInterrupts;
        }

        public Unknown24 GetUnknown24()
        {
            Unknown24 unknown24 = new Unknown24();
            // gets unknown24 count
            if (PsaFile.FileContent[DataSectionLocation + 24] >= 8096 && PsaFile.FileContent[DataSectionLocation + 24] < PsaFile.DataSectionSize)
            {
                int unknown24Location = PsaFile.FileContent[DataSectionLocation + 24] / 4;
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
            return unknown24;
        }

        // this does NOT look right at all but it's what PSA-C has...
        public int GetDataFlagsLocation()
        {
            return DataSectionLocation * 4 + 108;
        }

        public DataFlags GetDataFlags()
        {
            DataFlags dataFlags = new DataFlags();
            dataFlags.Offset = GetDataFlagsLocation();
            for (int i = 0; i < 4; i++)
            {
                dataFlags.ActionFlags.Add(PsaFile.FileContent[DataSectionLocation + 27 + i]);
            }
            Console.WriteLine(dataFlags.Offset.ToString("X"));
            dataFlags.ActionFlags.ForEach(x => Console.WriteLine(x.ToString("X8")));
            return dataFlags;
        }

    }
}
