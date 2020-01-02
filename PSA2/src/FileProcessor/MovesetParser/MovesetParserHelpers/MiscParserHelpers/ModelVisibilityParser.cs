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
            // idk what this if statement is for -- maybe this assures that there is model visibility data?
            if (PsaFile.FileContent[DataSectionLocation + 1] >= 8096 && PsaFile.FileContent[DataSectionLocation + 1] < PsaFile.DataSectionSize)
            {

                int modelVisibilityStartLocation = PsaFile.FileContent[DataSectionLocation + 1] / 4;

                // idk what this if statement is for
                if (PsaFile.FileContent[modelVisibilityStartLocation] >= 8096 && PsaFile.FileContent[modelVisibilityStartLocation] < PsaFile.DataSectionSize)
                {
                    int modelVisibilitySectionStartLocation = PsaFile.FileContent[modelVisibilityStartLocation] / 4;
                    int numberOfBoneSwitches = PsaFile.FileContent[modelVisibilityStartLocation + 1];

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
                            // this checks if a section exists
                            if (PsaFile.FileContent[modelVisibilitySectionStartLocation + i] >= 8096 && PsaFile.FileContent[modelVisibilitySectionStartLocation + i] < PsaFile.DataSectionSize)
                            {
                                int boneSwitchStartLocation = PsaFile.FileContent[modelVisibilitySectionStartLocation + i] / 4;
                                modelVisibility.Sections.Add(GetModelVisibilitySection(boneSwitchStartLocation, modelVisibilitySectionNames[i], numberOfBoneSwitches));
                            }
                        }
                        modelVisibility.SectionsData = GetModelVisibilitySectionsData(modelVisibilityStartLocation);
                    }
                }
            }
            return modelVisibility;
        }

        private ModelVisibility.Section GetModelVisibilitySection(int boneSwitchStartLocation, string modelVisibilitySectionName, int numberOfBoneSwitches)
        {
            ModelVisibility.Section modelVisibilitySection = new ModelVisibility.Section();
            modelVisibilitySection.Name = modelVisibilitySectionName;

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
            return modelVisibilitySection;
        }

        private List<ModelVisibility.SectionData> GetModelVisibilitySectionsData(int modelVisibilityStartLocation)
        {
            List<ModelVisibility.SectionData> sectionsData = new List<ModelVisibility.SectionData>();
            int numberOfDataSections = PsaFile.FileContent[modelVisibilityStartLocation + 3];
            if (PsaFile.FileContent[modelVisibilityStartLocation + 2] >= 8096 && PsaFile.FileContent[modelVisibilityStartLocation + 2] < PsaFile.DataSectionSize &&
                numberOfDataSections > 0 && numberOfDataSections < 256)
            {
                for (int i = 0; i < numberOfDataSections; i++)
                {
                    sectionsData.Add(new ModelVisibility.SectionData());
                }
            }
            return sectionsData;
        }

        public void PrintModelVisibility()
        {
            ModelVisibility modelVisibility = GetModelVisibility();
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
            if (modelVisibility.Sections.Count > 0)
            {
                Console.WriteLine("Number of data sections: {0}", modelVisibility.SectionsData.Count);
            }
        }
    }
}
