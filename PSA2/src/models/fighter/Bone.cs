using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter
{
    public class Bone
    {
        public int BoneIndex { get; set; }
        public string BoneName { get; set; }

        public Bone(int boneIndex)
        {
            BoneIndex = boneIndex;
        }
    }
}
