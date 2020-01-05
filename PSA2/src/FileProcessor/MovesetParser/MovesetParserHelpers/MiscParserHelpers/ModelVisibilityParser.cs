using PSA2.src.models.fighter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.MiscParserHelpers
{
    public class ModelVisibilityParser
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }

        public ModelVisibilityParser(PsaFile psaFile, int dataSectionLocation)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
        }

        public ModelVisibility GetModelVisibility()
        {
            ModelVisibility modelVisibility = new ModelVisibility();

            // checks if model visbility section exists
            if (PsaFile.FileContent[DataSectionLocation + 1] >= 8096 && PsaFile.FileContent[DataSectionLocation + 1] < PsaFile.DataSectionSize)
            {
                modelVisibility.Offset = PsaFile.FileContent[DataSectionLocation + 1];
                int modelVisibilityStartLocation = PsaFile.FileContent[DataSectionLocation + 1] / 4; // k
                modelVisibility.EntryOffset = PsaFile.FileContent[modelVisibilityStartLocation];
                modelVisibility.BoneSwitchCount = PsaFile.FileContent[modelVisibilityStartLocation + 1];
                modelVisibility.DataOffset = PsaFile.FileContent[modelVisibilityStartLocation + 2];
                modelVisibility.DataCount = PsaFile.FileContent[modelVisibilityStartLocation + 3];

                // checks if model visibility section data exists
                if (PsaFile.FileContent[modelVisibilityStartLocation] >= 8096 && PsaFile.FileContent[modelVisibilityStartLocation] < PsaFile.DataSectionSize)
                {
                    // these are the two model visibility sections
                    // hidden is for the model, visible is for shadow -- these names will be changed in the future to better represent what they are
                    List<string> modelVisibilitySectionNames = new List<string> { "Hidden", "Visible" };

                    int modelVisibilitySectionStartLocation = PsaFile.FileContent[modelVisibilityStartLocation] / 4;
                    for (int i = 0; i < modelVisibilitySectionNames.Count; i++)
                    {
                        if (PsaFile.FileContent[modelVisibilitySectionStartLocation + i] >= 8096 && PsaFile.FileContent[modelVisibilitySectionStartLocation + i] < PsaFile.DataSectionSize)
                        {
                            ModelVisibilitySection modelVisibilitySection = new ModelVisibilitySection();
                            modelVisibilitySection.Offset = PsaFile.FileContent[modelVisibilityStartLocation] + (i * 4);
                            modelVisibilitySection.DataOffset = PsaFile.FileContent[modelVisibilitySectionStartLocation + i];
                            modelVisibilitySection.Name = modelVisibilitySectionNames[i];
                            int numberOfBoneSwitches = PsaFile.FileContent[modelVisibilityStartLocation + 1];

                            if (numberOfBoneSwitches > 0 && numberOfBoneSwitches < 256) // makes sure value is only one byte
                            {
                                for (int j = 0; j < numberOfBoneSwitches; j++)
                                {
                                    // checks that bone switch data exists
                                    if (PsaFile.FileContent[modelVisibilityStartLocation] / 4 + j >= 8096 && PsaFile.FileContent[modelVisibilityStartLocation] / 4 + j < PsaFile.DataSectionSize)
                                    {
                                        int boneSwitchStartLocation = PsaFile.FileContent[modelVisibilityStartLocation] / 4 + i + j; // h
                                        List<BoneSwitch> boneSwitches = GetModelVisibilitySectionBoneSwitches(boneSwitchStartLocation, numberOfBoneSwitches);
                                        modelVisibilitySection.BoneSwitches = boneSwitches;
                                        modelVisibility.Sections.Add(modelVisibilitySection);
                                    }
                                }
                            }
                        }
                    }

                }
                modelVisibility.SectionsData = GetModelVisbilitySectionData(modelVisibilityStartLocation);
            }

            Console.WriteLine(modelVisibility);
            foreach (ModelVisibilitySection section in modelVisibility.Sections)
            {
                Console.WriteLine(section);
                foreach (BoneSwitch boneSwitch in section.BoneSwitches)
                {
                    Console.WriteLine(boneSwitch);
                    foreach (BoneGroup boneGroup in boneSwitch.BoneGroups)
                    {
                        Console.WriteLine(boneGroup);
                        Console.WriteLine(boneGroup.BoneList);
                    }
                }
            }
            foreach (ModelVisibilitySectionData sectionData in modelVisibility.SectionsData)
            {
                Console.WriteLine(sectionData);
            }
            return modelVisibility;
        }

        private List<BoneSwitch> GetModelVisibilitySectionBoneSwitches(int boneSwitchStartLocation, int numberOfBoneSwitches)
        {
            List<BoneSwitch> boneSwitches = new List<BoneSwitch>();
            for (int boneSwitchIndex = 0; boneSwitchIndex < numberOfBoneSwitches; boneSwitchIndex++)
            {
                BoneSwitch boneSwitch = new BoneSwitch();
                boneSwitches.Add(boneSwitch);
                boneSwitch.Offset = PsaFile.FileContent[boneSwitchStartLocation] + boneSwitchIndex * 8;
                int boneGroupStartLocation = PsaFile.FileContent[boneSwitchStartLocation] / 4 + boneSwitchIndex * 2;
                boneSwitch.DataOffset = PsaFile.FileContent[boneGroupStartLocation];
                boneSwitch.Count = PsaFile.FileContent[boneGroupStartLocation + 1];

                if (PsaFile.FileContent[boneSwitchStartLocation + boneSwitchIndex * 2] >= 8096 && PsaFile.FileContent[boneSwitchStartLocation + boneSwitchIndex * 2] < PsaFile.DataSectionSize)
                {
                    int numberOfBoneGroups = PsaFile.FileContent[boneGroupStartLocation + boneSwitchIndex * 2 + 1];
                    if (numberOfBoneGroups > 0 && numberOfBoneGroups < 256)
                    {
                        for (int boneGroupIndex = 0; boneGroupIndex < numberOfBoneGroups; boneGroupIndex++)
                        {
                            BoneGroup boneGroup = new BoneGroup();
                            boneSwitch.BoneGroups.Add(boneGroup);
                            boneGroup.Offset = PsaFile.FileContent[boneGroupStartLocation] + boneGroupIndex * 8;
                            int bonesStartLocation = PsaFile.FileContent[boneGroupStartLocation] / 4 + boneGroupIndex * 2;
                            boneGroup.DataOffset = PsaFile.FileContent[bonesStartLocation];
                            boneGroup.Count = PsaFile.FileContent[bonesStartLocation + 1];

                            if (PsaFile.FileContent[bonesStartLocation] >= 8096 && PsaFile.FileContent[bonesStartLocation] < PsaFile.DataSectionSize)
                            {
                                int numberOfBones = PsaFile.FileContent[bonesStartLocation + 1];
                                boneGroup.BoneList.Offset = PsaFile.FileContent[bonesStartLocation];
                                int something = PsaFile.FileContent[bonesStartLocation] / 4;
                                if (numberOfBones > 0 && numberOfBones < 256)
                                {
                                    for (int boneIndex = 0; boneIndex < numberOfBones; boneIndex++)
                                    {
                                        int bone = PsaFile.FileContent[something + boneIndex];
                                        boneGroup.BoneList.Bones.Add(bone);
                                    }
                                }
                            }
                        }

                    }
                }

            }
            return boneSwitches;
        }

        private List<ModelVisibilitySectionData> GetModelVisbilitySectionData(int modelVisibilityStartLocation)
        {
            List<ModelVisibilitySectionData> sectionsData = new List<ModelVisibilitySectionData>();
            if (PsaFile.FileContent[modelVisibilityStartLocation + 2] >= 8096 && PsaFile.FileContent[modelVisibilityStartLocation + 2] < PsaFile.DataSectionSize)
            {
                int numberOfDataSections = PsaFile.FileContent[modelVisibilityStartLocation + 3];
                if (numberOfDataSections > 0 && numberOfDataSections < 256)
                {
                    for (int i = 0; i < numberOfDataSections; i++)
                    {
                        ModelVisibilitySectionData sectionData = new ModelVisibilitySectionData();
                        sectionData.Offset = PsaFile.FileContent[modelVisibilityStartLocation + 2] + i * 8;
                        int sectionDataValuesLocation = PsaFile.FileContent[modelVisibilityStartLocation + 2] / 4 + i * 2;
                        sectionData.BoneSwitchIndex = PsaFile.FileContent[sectionDataValuesLocation];
                        sectionData.BoneGroupIndex = PsaFile.FileContent[sectionDataValuesLocation + 1];
                        sectionsData.Add(sectionData);
                    }
                }
            }
            return sectionsData;
        }
    }
}
