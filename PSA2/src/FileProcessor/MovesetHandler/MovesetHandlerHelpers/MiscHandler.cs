using PSA2.src.FileProcessor.MovesetHandler.Configs;
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.MiscHandlerHelpers;
using PSA2.src.models.fighter.Misc;
using PSA2.src.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    public class MiscHandler
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public string MovesetBaseName { get; private set; }
        public int NumberOfSpecialActions { get; private set; }
        public ModelVisibilityHandler ModelVisibilityHandler { get; private set; }
        public MiscSectionHandler MiscSectionHandler { get; private set; }
        public ArticleDataHandler ArticleDataHandler { get; private set; }

        public MiscHandler(PsaFile psaFile, int dataSectionLocation, string movesetBaseName, int numberOfSpecialActions)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            MovesetBaseName = movesetBaseName;
            NumberOfSpecialActions = numberOfSpecialActions;

            ModelVisibilityHandler = new ModelVisibilityHandler(PsaFile, DataSectionLocation);

            // if misc section exists
            int miscSectionLocation = PsaFile.FileContent[DataSectionLocation + 4] / 4;
            if (PsaFile.IsValidDataSectionLocation(miscSectionLocation))
            {
                MiscSectionHandler = new MiscSectionHandler(PsaFile, DataSectionLocation, miscSectionLocation);
            }

            ArticleDataHandler = new ArticleDataHandler(PsaFile, DataSectionLocation, GetCharacterSpecificParameters());
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
                Console.WriteLine(string.Format("{0}:{1}", pair.Key, pair.Value));
            }
            return dataOffsets;
        }

        public CharacterSpecificParametersConfig GetCharacterSpecificParameters()
        {
            return Utils.LoadJson<CharacterSpecificParametersConfig>(string.Format("data/char_specific/{0}.json", MovesetBaseName));
        }

        public CommonActionFlags GetCommonActionFlags()
        {
            CommonActionFlags commonActionFlags = new CommonActionFlags();
            // counts number of common action flags
            if (PsaFile.FileContent[DataSectionLocation + 5] >= 1480 && PsaFile.FileContent[DataSectionLocation + 5] < PsaFile.DataSectionSize)
            {
                commonActionFlags.Offset = PsaFile.FileContent[DataSectionLocation + 5];
                commonActionFlags.ActionFlagsCount = 274;
                for (int i = 0; i < 274; i++)
                {
                    if (PsaFile.FileContent[DataSectionLocation + 5] + i * 16 > 1479 && PsaFile.FileContent[DataSectionLocation + 5] + i * 16 < PsaFile.DataSectionSize)
                    {
                        int commonActionFlagActionLocation = PsaFile.FileContent[DataSectionLocation + 5] + i * 16;
                        int commonActionFlagActionValuesLocation = commonActionFlagActionLocation / 4;
                        int[] flagValues = new int[4]; // each flag has four values
                        for (int j = 0; j < 4; j++)
                        {
                            flagValues[j] = PsaFile.FileContent[commonActionFlagActionValuesLocation + j];
                        }
                        ActionFlag actionFlag = new ActionFlag(commonActionFlagActionLocation, flagValues);
                        commonActionFlags.ActionFlags.Add(actionFlag);

                    }

                }
            }
            Console.WriteLine(commonActionFlags);
            return commonActionFlags;
        }

        public SpecialActionFlags GetSpecialActionFlags()
        {
            SpecialActionFlags specialActionFlags = new SpecialActionFlags();
            // counts number of special action flags
            if (PsaFile.FileContent[DataSectionLocation + 6] >= 1480 && PsaFile.FileContent[DataSectionLocation + 6] < PsaFile.DataSectionSize)
            {
                specialActionFlags.Offset = PsaFile.FileContent[DataSectionLocation + 6];
                specialActionFlags.ActionFlagsCount = 274 + NumberOfSpecialActions;
                for (int i = 0; i < NumberOfSpecialActions; i++)
                {
                    if (PsaFile.FileContent[DataSectionLocation + 6] + i * 16 > 1479 && PsaFile.FileContent[DataSectionLocation + 6] + i * 16 < PsaFile.DataSectionSize)
                    {
                        int specialActionFlagActionLocation = PsaFile.FileContent[DataSectionLocation + 6] + i * 16;
                        int specialActionFlagActionValuesLocation = specialActionFlagActionLocation / 4;
                        int[] flagValues = new int[4]; // each flag has four values
                        for (int j = 0; j < 4; j++)
                        {
                            flagValues[j] = PsaFile.FileContent[specialActionFlagActionValuesLocation + j];
                        }
                        ActionFlag actionFlag = new ActionFlag(specialActionFlagActionLocation, flagValues);
                        specialActionFlags.ActionFlags.Add(actionFlag);
                    }

                }
            }
            Console.WriteLine(specialActionFlags);
            return specialActionFlags;
        }

        public ExtraActionFlags GetExtraActionFlags()
        {
            ExtraActionFlags extraActionFlags = new ExtraActionFlags();
            // counts number of extra action flags
            if (PsaFile.FileContent[DataSectionLocation + 7] >= 1480 && PsaFile.FileContent[DataSectionLocation + 7] < PsaFile.DataSectionSize)
            {
                extraActionFlags.Offset = PsaFile.FileContent[DataSectionLocation + 6];
                extraActionFlags.ActionFlagsCount = 274 + NumberOfSpecialActions;
                for (int i = 0; i < 274 + NumberOfSpecialActions; i++)
                {
                    if (PsaFile.FileContent[DataSectionLocation + 7] + i * 8 > 1479 && PsaFile.FileContent[DataSectionLocation + 7] + i * 8 < PsaFile.DataSectionSize)
                    {
                        int extraActionFlagActionLocation = PsaFile.FileContent[DataSectionLocation + 7] + i * 8;
                        int extraActionFlagActionValuesLocation = extraActionFlagActionLocation / 4;
                        int[] flagValues = new int[2]; // each flag has two values
                        for (int j = 0; j < 2; j++)
                        {
                            flagValues[j] = PsaFile.FileContent[extraActionFlagActionValuesLocation + j];
                        }
                        ActionFlag actionFlag = new ActionFlag(extraActionFlagActionLocation, flagValues);
                        extraActionFlags.ActionFlags.Add(actionFlag);
                    }

                }
            }
            Console.WriteLine(extraActionFlags);
            return extraActionFlags;
        }

        // TODO: I definitely flubbed this one and I should come back to it
        public ActionInterrupts GetActionInterrupts()
        {
            ActionInterrupts actionInterrupts = new ActionInterrupts();
            // check if action interrupt section exists
            if (PsaFile.FileContent[DataSectionLocation + 8] >= 8096 && PsaFile.FileContent[DataSectionLocation + 8] < PsaFile.DataSectionSize)
            {
                actionInterrupts.Offset = PsaFile.FileContent[DataSectionLocation + 8];
                int actionInterruptsLocation = PsaFile.FileContent[DataSectionLocation + 8] / 4; // k
                actionInterrupts.ActionInterruptsOffset = PsaFile.FileContent[DataSectionLocation + 8];
                actionInterrupts.DataOffset = PsaFile.FileContent[actionInterruptsLocation];
                actionInterrupts.DataCount = PsaFile.FileContent[actionInterruptsLocation + 1];

                if (PsaFile.FileContent[actionInterruptsLocation] >= 8096 && PsaFile.FileContent[actionInterruptsLocation] < PsaFile.DataSectionSize)
                {
                    int numberOfActionInterrupts = PsaFile.FileContent[actionInterruptsLocation + 1];
                    if (numberOfActionInterrupts > 0 && numberOfActionInterrupts < 100) // why 100 and not 256? 100 in hex is 256 so maybe an accident?
                    {
                        int actionInterruptEntryValuesLocation = PsaFile.FileContent[actionInterruptsLocation] / 4; // h
                        for (int i = 0; i < numberOfActionInterrupts; i++)
                        {
                            ActionInterruptEntry actionInterruptEntry = new ActionInterruptEntry();
                            actionInterruptEntry.Value = PsaFile.FileContent[actionInterruptEntryValuesLocation + i];
                            // TODO: PSAC has actual names for which external data it references
                            actionInterrupts.ActionInterruptEntries.Add(actionInterruptEntry);
                        }
                    }
                }
            }
            Console.WriteLine(actionInterrupts);
            return actionInterrupts;
        }

        public BoneFloats1 GetBoneFloats1()
        {
            BoneFloats1 boneFloats1 = new BoneFloats1();
            if (PsaFile.FileContent[DataSectionLocation + 16] >= 8096 && PsaFile.FileContent[DataSectionLocation + 16] < PsaFile.DataSectionSize)
            {
                int boneFloats1Location = PsaFile.FileContent[DataSectionLocation + 16] / 4;
                boneFloats1.Offset = boneFloats1Location * 4;
                boneFloats1.EntriesCount = 3;
                for (int i = 0; i < 3; i++) // there's always three bone floats 1 data (not the same for the other bone floats)
                {
                    int boneFloats1DataLocation = PsaFile.FileContent[DataSectionLocation + 16] + i * 28;
                    int boneFloats1DataValueLocation = boneFloats1DataLocation / 4;
                    int bone = PsaFile.FileContent[boneFloats1DataValueLocation];
                    int[] data = new int[6];
                    for (int j = 0; j < 6; j++)
                    {
                        data[j] = PsaFile.FileContent[boneFloats1DataValueLocation + j + 1];
                    }
                    boneFloats1.BoneFloatEntries.Add(new BoneFloatEntry(boneFloats1DataLocation, bone, data));
                }
            }
            Console.WriteLine(boneFloats1);
            return boneFloats1;
        }

        public BoneFloats2 GetBoneFloats2()
        {
            BoneFloats2 boneFloats2 = new BoneFloats2();
            if (PsaFile.FileContent[DataSectionLocation + 17] >= 8096 && PsaFile.FileContent[DataSectionLocation + 17] < PsaFile.DataSectionSize)
            {
                int boneFloats2Location = PsaFile.FileContent[DataSectionLocation + 17] / 4;
                boneFloats2.Offset = boneFloats2Location * 4;
                int numberOfBoneFloats2 = (PsaFile.FileContent[DataSectionLocation + 18] - PsaFile.FileContent[DataSectionLocation + 17]) / 28;
                boneFloats2.EntriesCount = numberOfBoneFloats2;
                if (numberOfBoneFloats2 > 0 && numberOfBoneFloats2 < 256)
                {
                    for (int i = 0; i < numberOfBoneFloats2; i++)
                    {
                        int boneFloats2DataLocation = PsaFile.FileContent[DataSectionLocation + 17] + i * 28;
                        int boneFloats2DataValueLocation = boneFloats2DataLocation / 4;
                        int bone = PsaFile.FileContent[boneFloats2DataValueLocation];
                        int[] data = new int[6];
                        for (int j = 0; j < 6; j++)
                        {
                            data[j] = PsaFile.FileContent[boneFloats2DataValueLocation + j + 1];
                        }
                        boneFloats2.BoneFloatEntries.Add(new BoneFloatEntry(boneFloats2DataLocation, bone, data));
                    }
                }
            }
            Console.WriteLine(boneFloats2);
            return boneFloats2;
        }

        // link/toon link has this
        public BoneFloats3 GetBoneFloats3()
        {
            BoneFloats3 boneFloats3 = new BoneFloats3();
            if (PsaFile.FileContent[DataSectionLocation + 23] >= 8096 && PsaFile.FileContent[DataSectionLocation + 23] < PsaFile.DataSectionSize)
            {
                int boneFloats3Location = PsaFile.FileContent[DataSectionLocation + 23] / 4;
                boneFloats3.Offset = boneFloats3Location * 4;
                int numberOfBoneFloats3 = (PsaFile.FileContent[DataSectionLocation + 18] - PsaFile.FileContent[DataSectionLocation + 23]) / 28;
                boneFloats3.EntriesCount = numberOfBoneFloats3;
                if (numberOfBoneFloats3 > 0 && numberOfBoneFloats3 < 256)
                {
                    for (int i = 0; i < numberOfBoneFloats3; i++)
                    {
                        int boneFloats3DataLocation = PsaFile.FileContent[DataSectionLocation + 23] + i * 28;
                        int boneFloats3DataValueLocation = boneFloats3DataLocation / 4;
                        int bone = PsaFile.FileContent[boneFloats3DataValueLocation];
                        int[] data = new int[6];
                        for (int j = 0; j < 6; j++)
                        {
                            data[j] = PsaFile.FileContent[boneFloats3DataValueLocation + j + 1];
                        }
                        boneFloats3.BoneFloatEntries.Add(new BoneFloatEntry(boneFloats3DataLocation, bone, data));
                    }
                }
            }
            Console.WriteLine(boneFloats3);
            return boneFloats3;
        }

        public BoneReferences GetBoneReferences()
        {
            BoneReferences boneReferences = new BoneReferences();
            // checks if bone references section exists (the other one, NOT the one in misc section)
            if (PsaFile.FileContent[DataSectionLocation + 18] >= 8096 && PsaFile.FileContent[DataSectionLocation + 18] < PsaFile.DataSectionSize)
            {
                boneReferences.Offset = PsaFile.FileContent[DataSectionLocation + 18];
                int boneReferencesLocation = PsaFile.FileContent[DataSectionLocation + 18] / 4; // j

                // this gets number of bone references...somehow
                // no idea why it needs this info from misc section's bone references...
                if (PsaFile.FileContent[DataSectionLocation + 4] >= 8096 && PsaFile.FileContent[DataSectionLocation + 4] < PsaFile.DataSectionSize) // if misc section exists...
                {
                    int miscSectionLocation = PsaFile.FileContent[DataSectionLocation + 4] / 4;
                    if (PsaFile.FileContent[miscSectionLocation + 9] >= 8096 && PsaFile.FileContent[miscSectionLocation + 9] < PsaFile.DataSectionSize) // if misc section's bone references exist
                    {
                        // I thinkkk this calcs the difference between the two locations, which would be how many bone references are in this section before it reaches misc section's bone references...maybe??
                        int numberOfBoneReferences = (PsaFile.FileContent[miscSectionLocation + 9] - PsaFile.FileContent[DataSectionLocation + 18]) / 4;

                        // I guess there has to at least be just one bone references at minimum
                        if (numberOfBoneReferences == 0)
                        {
                            numberOfBoneReferences = 1;
                        }
                        boneReferences.BonesCount = numberOfBoneReferences;
                        for (int i = 0; i < numberOfBoneReferences; i++)
                        {
                            boneReferences.Bones.Add(PsaFile.FileContent[boneReferencesLocation + i]);
                        }
                    }
                }
            }
            Console.WriteLine(boneReferences);
            return boneReferences;
        }

        public HandBones GetHandBones()
        {
            HandBones handBones = new HandBones();
            if (PsaFile.FileContent[DataSectionLocation + 19] >= 8096 && PsaFile.FileContent[DataSectionLocation + 19] < PsaFile.DataSectionSize)
            {
                handBones.Offset = PsaFile.FileContent[DataSectionLocation + 19];
                int handBonesLocation = PsaFile.FileContent[DataSectionLocation + 19] / 4; // k
                handBones.HandNBoneIndex0 = PsaFile.FileContent[handBonesLocation];
                handBones.HandNBoneIndex1 = PsaFile.FileContent[handBonesLocation + 1];
                handBones.HandNBoneIndex2 = PsaFile.FileContent[handBonesLocation + 2];
                handBones.HandNBoneIndex3 = PsaFile.FileContent[handBonesLocation + 3];
                handBones.DataCount = PsaFile.FileContent[handBonesLocation + 4];
                handBones.DataOffset = PsaFile.FileContent[handBonesLocation + 5];

                if (PsaFile.FileContent[handBonesLocation + 5] >= 8096 && PsaFile.FileContent[handBonesLocation + 5] < PsaFile.DataSectionSize)
                {
                    int numberOfHandBones = PsaFile.FileContent[handBonesLocation + 4];
                    if (numberOfHandBones > 0 && numberOfHandBones < 80) // I guess there can only be 80 handbones? ok...
                    {
                        handBones.BonesListDataOffset = PsaFile.FileContent[handBonesLocation + 5];
                        int handBonesValuesLocation = PsaFile.FileContent[handBonesLocation + 5] / 4; // j
                        for (int i = 0; i < numberOfHandBones; i++)
                        {
                            handBones.Bones.Add(PsaFile.FileContent[handBonesValuesLocation + i]);
                        }
                    }
                }
            }
            Console.WriteLine(handBones);
            return handBones;
        }

        public ExtraActionInterrupts GetExtraActionInterrupts()
        {
            ExtraActionInterrupts extraActionInterrupts = new ExtraActionInterrupts();
            if (PsaFile.FileContent[DataSectionLocation + 22] >= 8096 && PsaFile.FileContent[DataSectionLocation + 22] < PsaFile.DataSectionSize)
            {
                extraActionInterrupts.Offset = PsaFile.FileContent[DataSectionLocation + 22];
                int extraActionInterruptsLocation = PsaFile.FileContent[DataSectionLocation + 22] / 4;
                extraActionInterrupts.Unknown0 = PsaFile.FileContent[extraActionInterruptsLocation];
                extraActionInterrupts.Unknown1 = PsaFile.FileContent[extraActionInterruptsLocation + 1];
                extraActionInterrupts.DataOffset = PsaFile.FileContent[extraActionInterruptsLocation + 2];
            }
            Console.WriteLine(extraActionInterrupts);
            return extraActionInterrupts;
        }

        public Unknown24 GetUnknown24()
        {
            Unknown24 unknown24 = new Unknown24();
            if (PsaFile.FileContent[DataSectionLocation + 24] >= 8096 && PsaFile.FileContent[DataSectionLocation + 24] < PsaFile.DataSectionSize)
            {
                unknown24.Offset = PsaFile.FileContent[DataSectionLocation + 24];
                int unknown24Location = PsaFile.FileContent[DataSectionLocation + 24] / 4;
                unknown24.DataOffset = PsaFile.FileContent[unknown24Location];
                unknown24.DataCount = PsaFile.FileContent[unknown24Location + 1];
                if (PsaFile.FileContent[unknown24Location] >= 8096 && PsaFile.FileContent[unknown24Location] < PsaFile.DataSectionSize)
                {
                    int unknown24EntriesValuesLocation = PsaFile.FileContent[unknown24Location] / 4;
                    int numberOfUnknown24Entries = PsaFile.FileContent[unknown24Location + 1];
                    unknown24.BonesListOffset = PsaFile.FileContent[unknown24Location];
                    if (numberOfUnknown24Entries > 0 && numberOfUnknown24Entries < 256)
                    {
                        for (int i = 0; i < numberOfUnknown24Entries; i++)
                        {
                            unknown24.Bones.Add(PsaFile.FileContent[unknown24EntriesValuesLocation + i]);
                        }
                    }
                }
            }
            Console.WriteLine(unknown24);
            return unknown24;
        }

        public DataFlags GetDataFlags()
        {
            DataFlags dataFlags = new DataFlags();
            dataFlags.Offset = DataSectionLocation * 4 + 108;
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
