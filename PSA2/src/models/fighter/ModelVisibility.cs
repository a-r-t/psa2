using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter
{
    public class ModelVisibility
    {
        public string EntryOffset { get; set; }
        public string DataOffset { get; set; }
        public List<Section> Sections { get; set; }
        public List<SectionData> SectionsData { get; set; }

        public ModelVisibility()
        {
            Sections = new List<Section>();
            SectionsData = new List<SectionData>();
        }

        public class Section
        {
            public string Name { get; set; }
            public string DataOffset { get; set; }
            public List<BoneSwitch> BoneSwitches { get; set; }

            public Section()
            {
                BoneSwitches = new List<BoneSwitch>();
            }

        }

        public class BoneSwitch
        {
            public string DataOffset { get; set; }
            public List<BoneGroup> BoneGroups { get; set; }

            public BoneSwitch()
            {
                BoneGroups = new List<BoneGroup>();
            }
        }

        public class BoneGroup
        {
            public string DataOffset { get; set; }
            
            // won't need this later on, need it for testing purposes
            // since the count of the bones is gotten before bone data is loaded in...for some reason
            public int numberOfBones { get; set; }

            public List<Bone> Bones { get; set; }

            public BoneGroup()
            {
                Bones = new List<Bone>();
            }
        }

        public class SectionData
        {
            public int BoneSwitchIndex { get; set; }
            public int BoneGroupIndex { get; set; }

            public SectionData()
            {

            }
        }

    }
}
