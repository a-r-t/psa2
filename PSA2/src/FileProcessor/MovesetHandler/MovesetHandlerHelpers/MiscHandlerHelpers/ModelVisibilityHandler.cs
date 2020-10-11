using PSA2.src.Models.Fighter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.MiscHandlerHelpers
{
    public class ModelVisibilityHandler
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }

        public ModelVisibilityHandler(PsaFile psaFile, int dataSectionLocation)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
        }

        public ModelVisibility GetModelVisibility()
        {
            ModelVisibility modelVisibility = new ModelVisibility();

            // checks if model visbility section exists
            if (PsaFile.DataSection[DataSectionLocation + 1] >= 8096 && PsaFile.DataSection[DataSectionLocation + 1] < PsaFile.DataSectionSize)
            {
                modelVisibility.Offset = PsaFile.DataSection[DataSectionLocation + 1];
                int modelVisibilityStartLocation = PsaFile.DataSection[DataSectionLocation + 1] / 4; // k
                modelVisibility.EntryOffset = PsaFile.DataSection[modelVisibilityStartLocation];
                modelVisibility.BoneSwitchCount = PsaFile.DataSection[modelVisibilityStartLocation + 1];
                modelVisibility.DataOffset = PsaFile.DataSection[modelVisibilityStartLocation + 2];
                modelVisibility.DataCount = PsaFile.DataSection[modelVisibilityStartLocation + 3];

                // checks if model visibility section data exists
                if (PsaFile.DataSection[modelVisibilityStartLocation] >= 8096 && PsaFile.DataSection[modelVisibilityStartLocation] < PsaFile.DataSectionSize)
                {
                    // these are the two model visibility sections
                    // hidden is for the model, visible is for shadow -- these names will be changed in the future to better represent what they are
                    List<string> modelVisibilitySectionNames = new List<string> { "Hidden", "Visible" };

                    int modelVisibilitySectionStartLocation = PsaFile.DataSection[modelVisibilityStartLocation] / 4;
                    for (int i = 0; i < modelVisibilitySectionNames.Count; i++)
                    {
                        if (PsaFile.DataSection[modelVisibilitySectionStartLocation + i] >= 8096 && PsaFile.DataSection[modelVisibilitySectionStartLocation + i] < PsaFile.DataSectionSize)
                        {
                            ModelVisibilitySection modelVisibilitySection = new ModelVisibilitySection();
                            modelVisibilitySection.Offset = PsaFile.DataSection[modelVisibilityStartLocation] + i * 4;
                            modelVisibilitySection.DataOffset = PsaFile.DataSection[modelVisibilitySectionStartLocation + i];
                            modelVisibilitySection.Name = modelVisibilitySectionNames[i];
                            int numberOfBoneSwitches = PsaFile.DataSection[modelVisibilityStartLocation + 1];

                            if (numberOfBoneSwitches > 0 && numberOfBoneSwitches < 256) // makes sure value is only one byte
                            {
                                for (int j = 0; j < numberOfBoneSwitches; j++)
                                {
                                    // checks that bone switch data exists
                                    if (PsaFile.DataSection[modelVisibilityStartLocation] / 4 + j >= 8096 && PsaFile.DataSection[modelVisibilityStartLocation] / 4 + j < PsaFile.DataSectionSize)
                                    {
                                        int boneSwitchStartLocation = PsaFile.DataSection[modelVisibilityStartLocation] / 4 + i + j; // h
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
                boneSwitch.Offset = PsaFile.DataSection[boneSwitchStartLocation] + boneSwitchIndex * 8;
                int boneGroupStartLocation = PsaFile.DataSection[boneSwitchStartLocation] / 4 + boneSwitchIndex * 2;
                boneSwitch.DataOffset = PsaFile.DataSection[boneGroupStartLocation];
                boneSwitch.Count = PsaFile.DataSection[boneGroupStartLocation + 1];

                if (PsaFile.DataSection[boneSwitchStartLocation + boneSwitchIndex * 2] >= 8096 && PsaFile.DataSection[boneSwitchStartLocation + boneSwitchIndex * 2] < PsaFile.DataSectionSize)
                {
                    int numberOfBoneGroups = PsaFile.DataSection[boneGroupStartLocation + boneSwitchIndex * 2 + 1];
                    if (numberOfBoneGroups > 0 && numberOfBoneGroups < 256)
                    {
                        for (int boneGroupIndex = 0; boneGroupIndex < numberOfBoneGroups; boneGroupIndex++)
                        {
                            BoneGroup boneGroup = new BoneGroup();
                            boneSwitch.BoneGroups.Add(boneGroup);
                            boneGroup.Offset = PsaFile.DataSection[boneGroupStartLocation] + boneGroupIndex * 8;
                            int bonesStartLocation = PsaFile.DataSection[boneGroupStartLocation] / 4 + boneGroupIndex * 2;
                            boneGroup.DataOffset = PsaFile.DataSection[bonesStartLocation];
                            boneGroup.Count = PsaFile.DataSection[bonesStartLocation + 1];

                            if (PsaFile.DataSection[bonesStartLocation] >= 8096 && PsaFile.DataSection[bonesStartLocation] < PsaFile.DataSectionSize)
                            {
                                int numberOfBones = PsaFile.DataSection[bonesStartLocation + 1];
                                boneGroup.BoneList.Offset = PsaFile.DataSection[bonesStartLocation];
                                int boneValuesLocation = PsaFile.DataSection[bonesStartLocation] / 4;
                                if (numberOfBones > 0 && numberOfBones < 256)
                                {
                                    for (int boneIndex = 0; boneIndex < numberOfBones; boneIndex++)
                                    {
                                        int bone = PsaFile.DataSection[boneValuesLocation + boneIndex];
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
            if (PsaFile.DataSection[modelVisibilityStartLocation + 2] >= 8096 && PsaFile.DataSection[modelVisibilityStartLocation + 2] < PsaFile.DataSectionSize)
            {
                int numberOfDataSections = PsaFile.DataSection[modelVisibilityStartLocation + 3];
                if (numberOfDataSections > 0 && numberOfDataSections < 256)
                {
                    for (int i = 0; i < numberOfDataSections; i++)
                    {
                        ModelVisibilitySectionData sectionData = new ModelVisibilitySectionData();
                        sectionData.Offset = PsaFile.DataSection[modelVisibilityStartLocation + 2] + i * 8;
                        int sectionDataValuesLocation = PsaFile.DataSection[modelVisibilityStartLocation + 2] / 4 + i * 2;
                        sectionData.BoneSwitchIndex = PsaFile.DataSection[sectionDataValuesLocation];
                        sectionData.BoneGroupIndex = PsaFile.DataSection[sectionDataValuesLocation + 1];
                        sectionsData.Add(sectionData);
                    }
                }
            }
            return sectionsData;
        }
    }
}
