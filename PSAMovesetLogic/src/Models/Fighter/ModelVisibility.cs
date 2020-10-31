using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Models.Fighter
{
    public class ModelVisibility
    {
        public int Offset { get; set; }
        public int EntryOffset { get; set; }
        public int BoneSwitchCount { get; set; }
        public int DataOffset { get; set; }
        public int DataCount { get; set; }
        public List<ModelVisibilitySection> Sections { get; set; }
        public int SectionsDataCount { get; set; }
        public List<ModelVisibilitySectionData> SectionsData { get; set; }

        public ModelVisibility()
        {
            Sections = new List<ModelVisibilitySection>();
            SectionsData = new List<ModelVisibilitySectionData>();
        }

        public override string ToString()
        {
            return $"Model Visibility:\n\tOffset: {Offset.ToString("X")}\n\tEntry Offset: {EntryOffset.ToString("X")}\n\tBone Switch Count: {BoneSwitchCount.ToString("X")}\n\t" +
                $"Data Offset: {DataOffset.ToString("X")}\n\tData Count: {DataCount.ToString("X")}\n\tNumber Of Sections: {Sections.Count}";
        }

    }

    public class ModelVisibilitySection
    {
        public int Offset { get; set; }
        public string Name { get; set; }
        public int DataOffset { get; set; }
        public List<BoneSwitch> BoneSwitches { get; set; }

        public ModelVisibilitySection()
        {
            BoneSwitches = new List<BoneSwitch>();
        }

        public override string ToString()
        {
            return $"Model Visbility Section:\n\tOffset: {Offset.ToString("X")}\n\tName: {Name}\n\tData Offset: {DataOffset.ToString("X")}\n\tNumber Of BoneSwitches: {BoneSwitches.Count}";
        }

    }

    public class BoneSwitch
    {
        public int Offset { get; set; }
        public int DataOffset { get; set; }
        public int Count { get; set; }
        public List<BoneGroup> BoneGroups { get; set; }

        public BoneSwitch()
        {
            BoneGroups = new List<BoneGroup>();
        }

        public override string ToString()
        {
            return $"Bone Switch:\n\tOffset: {Offset.ToString("X")}\n\tData Offset: {DataOffset.ToString("X")}\n\tCount: {Count.ToString("X")}, Number Of Bone Groups: {BoneGroups.Count}";
        }
    }

    public class BoneGroup
    {
        public int Offset { get; set; }
        public int DataOffset { get; set; }
        public int Count { get; set; }
        public BoneList BoneList { get; set; }

        // won't need this later on, need it for testing purposes
        // since the count of the bones is gotten before bone data is loaded in...for some reason
        public int numberOfBones { get; set; }


        public BoneGroup()
        {
            BoneList = new BoneList();
        }

        public override string ToString()
        {
            return $"Bone Group:\n\tOffset: {Offset.ToString("X")}\n\tData Offset: {DataOffset.ToString("X")}\n\tCount: {Count.ToString("X")}";
        }
    }

    public class BoneList
    {
        public int Offset { get; set; }
        public List<int> Bones { get; set; }

        public BoneList()
        {
            Bones = new List<int>();
        }

        public override string ToString()
        {
            return $"Bone List:\n\tOffset: {Offset.ToString("X")}\n\tBones: {string.Join(",", Bones)}";
        }
    }

    public class ModelVisibilitySectionData
    {
        public int Offset { get; set; }
        public int BoneSwitchIndex { get; set; }
        public int BoneGroupIndex { get; set; }

        public ModelVisibilitySectionData()
        {

        }

        public override string ToString()
        {
            return $"ModelVisibilitySectionData:\n\tOffset: {Offset.ToString("X")}\n\tBone Switch Index: {BoneSwitchIndex}\n\tBone Group Index: {BoneGroupIndex}";
        }
    }
}
