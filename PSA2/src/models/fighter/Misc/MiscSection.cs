using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class MiscSection
    {
        public int Offset { get; set; }
        public int MiscSection1Offset { get; set; }
        public int FinalSmashAuraOffset { get; set; }
        public int FinalSmashAuraCount { get; set; }
        public int HurtBoxOffset { get; set; }
        public int HurtBoxCount { get; set; }
        public int LedgeGrabOffset { get; set; }
        public int LedgeGrabCount { get; set; }
        public int MiscSection2Offset { get; set; }
        public int MiscSection2Count { get; set; }
        public int BoneReferencesOffset { get; set; }
        public int ItemBonesOffset { get; set; }
        public int SoundDataOffset { get; set; }
        public int MiscSection5Offset { get; set; }
        public int MultiJumpOffset { get; set; }
        public int GlideOffset { get; set; }
        public int CrawlOffset { get; set; }
        public int CollisionDataOffset { get; set; }
        public int TetherOffset { get; set; }
        public int MiscSection12Offset { get; set; }

        public MiscSection1 MiscSection1 { get; set; }
        public FinalSmashAura FinalSmashAura { get; set; }
        public HurtBoxes HurtBoxes { get; set; }
        public LedgeGrab LedgeGrab { get; set; }
        public MiscSection2 MiscSection2 { get; set; }
        public BoneReferences BoneReferences { get; set; }
        public ItemBones ItemBones { get; set; }
        public SoundLists SoundLists { get; set; }
        public MiscSection5 MiscSection5 { get; set; }
        public Crawl Crawl { get; set; }
        public MultiJump MultiJump { get; set; }
        public Glide Glide { get; set; }
        public Tether Tether { get; set; }
        public CollisionData CollisionData { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(MiscSection1Offset)}={MiscSection1Offset.ToString("X")}, {nameof(FinalSmashAuraOffset)}={FinalSmashAuraOffset.ToString("X")}, {nameof(FinalSmashAuraCount)}={FinalSmashAuraCount.ToString("X")}, {nameof(HurtBoxOffset)}={HurtBoxOffset.ToString("X")}, {nameof(HurtBoxCount)}={HurtBoxCount.ToString("X")}, {nameof(LedgeGrabOffset)}={LedgeGrabOffset.ToString("X")}, {nameof(LedgeGrabCount)}={LedgeGrabCount.ToString("X")}, {nameof(MiscSection2Offset)}={MiscSection2Offset.ToString("X")}, {nameof(MiscSection2Count)}={MiscSection2Count.ToString("X")}, {nameof(BoneReferencesOffset)}={BoneReferencesOffset.ToString("X")}, {nameof(ItemBonesOffset)}={ItemBonesOffset.ToString("X")}, {nameof(SoundDataOffset)}={SoundDataOffset.ToString("X")}, {nameof(MiscSection5Offset)}={MiscSection5Offset.ToString("X")}, {nameof(MultiJumpOffset)}={MultiJumpOffset.ToString("X")}, {nameof(GlideOffset)}={GlideOffset.ToString("X")}, {nameof(CrawlOffset)}={CrawlOffset.ToString("X")}, {nameof(CollisionDataOffset)}={CollisionDataOffset.ToString("X")}, {nameof(TetherOffset)}={TetherOffset.ToString("X")}, {nameof(MiscSection12Offset)}={MiscSection12Offset.ToString("X")}, {nameof(MiscSection1)}={MiscSection1}, {nameof(FinalSmashAura)}={FinalSmashAura}, {nameof(HurtBoxes)}={HurtBoxes}, {nameof(LedgeGrab)}={LedgeGrab}, {nameof(MiscSection2)}={MiscSection2}, {nameof(BoneReferences)}={BoneReferences}, {nameof(ItemBones)}={ItemBones}, {nameof(SoundLists)}={SoundLists}, {nameof(MiscSection5)}={MiscSection5}, {nameof(Crawl)}={Crawl}, {nameof(MultiJump)}={MultiJump}, {nameof(Glide)}={Glide}, {nameof(Tether)}={Tether}, {nameof(CollisionData)}={CollisionData}}}";
        }
    }
}
