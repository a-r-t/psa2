using PSA2.src.models.fighter.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.MiscParserHelpers
{
    public class MiscSectionParser
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public int MiscSectionLocation { get; private set; } // k

        public MiscSectionParser(PsaFile psaFile, int dataSectionLocation, int miscSectionLocation)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            MiscSectionLocation = miscSectionLocation;
        }

        public MiscSection GetMiscSection()
        {
            MiscSection miscSection = new MiscSection();
            if (PsaFile.FileContent[DataSectionLocation + 4] >= 8096 && PsaFile.FileContent[DataSectionLocation + 4] < PsaFile.DataSectionSize)
            {
                miscSection.Offset = PsaFile.FileContent[DataSectionLocation + 4];
                miscSection.MiscSection1Offset = PsaFile.FileContent[MiscSectionLocation];
                miscSection.FinalSmashAuraOffset = PsaFile.FileContent[MiscSectionLocation + 1];
                miscSection.FinalSmashAuraCount = PsaFile.FileContent[MiscSectionLocation + 2];
                miscSection.HurtBoxOffset = PsaFile.FileContent[MiscSectionLocation + 3];
                miscSection.HurtBoxCount = PsaFile.FileContent[MiscSectionLocation + 4];
                miscSection.LedgeGrabOffset = PsaFile.FileContent[MiscSectionLocation + 5];
                miscSection.LedgeGrabCount = PsaFile.FileContent[MiscSectionLocation + 6];
                miscSection.MiscSection2Offset = PsaFile.FileContent[MiscSectionLocation + 7];
                miscSection.MiscSection2Count = PsaFile.FileContent[MiscSectionLocation + 8];
                miscSection.BoneReferencesOffset = PsaFile.FileContent[MiscSectionLocation + 9];
                miscSection.ItemBonesOffset = PsaFile.FileContent[MiscSectionLocation + 10];
                miscSection.SoundDataOffset = PsaFile.FileContent[MiscSectionLocation + 11];
                miscSection.MiscSection5Offset = PsaFile.FileContent[MiscSectionLocation + 12];
                miscSection.MultiJumpOffset = PsaFile.FileContent[MiscSectionLocation + 13];
                miscSection.GlideOffset = PsaFile.FileContent[MiscSectionLocation + 14];
                miscSection.CrawlOffset = PsaFile.FileContent[MiscSectionLocation + 15];
                miscSection.CollisionDataOffset = PsaFile.FileContent[MiscSectionLocation + 16];
                miscSection.TetherOffset = PsaFile.FileContent[MiscSectionLocation + 17];
                miscSection.MiscSection12Offset = PsaFile.FileContent[MiscSectionLocation + 18];
            }
            Console.WriteLine(miscSection);
            return miscSection;
        }

        public MiscSection1 GetMiscSection1()
        {
            MiscSection1 miscSection1 = new MiscSection1();

            if (PsaFile.FileContent[MiscSectionLocation] >= 8096 && PsaFile.FileContent[MiscSectionLocation] < PsaFile.DataSectionSize)
            {
                miscSection1.Offset = PsaFile.FileContent[MiscSectionLocation];
                int miscSection1Location = PsaFile.FileContent[MiscSectionLocation] / 4;
                for (int i = 0; i < 7; i++)
                {
                    miscSection1.Params.Add(new MiscSection1Param($"Data{i + 1}", PsaFile.FileContent[miscSection1Location + i]));
                }
                miscSection1.Params.Add(new MiscSection1Param("Misc Section1 Offset", PsaFile.FileContent[MiscSectionLocation]));
            }
            Console.WriteLine(miscSection1);
            return miscSection1;
        }

        public FinalSmashAura GetFinalSmashAura()
        {
            FinalSmashAura finalSmashAura = new FinalSmashAura();
            if (PsaFile.FileContent[MiscSectionLocation + 1] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 1] < PsaFile.DataSectionSize)
            {
                finalSmashAura.Offset = PsaFile.FileContent[MiscSectionLocation + 1];
                int numberOfFinalSmashAuraEntries = PsaFile.FileContent[MiscSectionLocation + 2];

                if (numberOfFinalSmashAuraEntries > 0 && numberOfFinalSmashAuraEntries < 256)
                {
                    finalSmashAura.FinalSmashAuraEntryCount = numberOfFinalSmashAuraEntries;
                    for (int i = 0; i < numberOfFinalSmashAuraEntries; i++)
                    {
                        int finalSmashAuraEntryLocation = PsaFile.FileContent[MiscSectionLocation + 1] + i * 20;
                        int finalSmashAuraEntryValuesLocation = finalSmashAuraEntryLocation / 4;
                        int bone = PsaFile.FileContent[finalSmashAuraEntryValuesLocation];
                        int x = PsaFile.FileContent[finalSmashAuraEntryValuesLocation + 1];
                        int y = PsaFile.FileContent[finalSmashAuraEntryValuesLocation + 2];
                        int width = PsaFile.FileContent[finalSmashAuraEntryValuesLocation + 3];
                        int height = PsaFile.FileContent[finalSmashAuraEntryValuesLocation + 4];
                        finalSmashAura.Entries.Add(new FinalSmashAuraEntry(finalSmashAuraEntryLocation, bone, x, y, width, height));
                    }
                }
            }
            Console.WriteLine(finalSmashAura);
            return finalSmashAura;
        }

        public HurtBoxes GetHurtBoxes()
        {
            HurtBoxes hurtBoxes = new HurtBoxes();
            if (PsaFile.FileContent[MiscSectionLocation + 3] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 3] < PsaFile.DataSectionSize)
            {

                hurtBoxes.Offset = PsaFile.FileContent[MiscSectionLocation + 3];
                int numberOfHurtBoxEntries = PsaFile.FileContent[MiscSectionLocation + 4];

                if (numberOfHurtBoxEntries > 0 && numberOfHurtBoxEntries < 256)
                {
                    hurtBoxes.HurtBoxEntryCount = numberOfHurtBoxEntries;
                    for (int i = 0; i < numberOfHurtBoxEntries; i++)
                    {
                        int hurtBoxesEntryLocation = PsaFile.FileContent[MiscSectionLocation + 3] + i * 32;
                        int hurtBoxesEntryValuesLocation = hurtBoxesEntryLocation / 4;

                        int xOffset = PsaFile.FileContent[hurtBoxesEntryValuesLocation];
                        int yOffset = PsaFile.FileContent[hurtBoxesEntryValuesLocation + 1];
                        int zOffset = PsaFile.FileContent[hurtBoxesEntryValuesLocation + 2];
                        int xStretch = PsaFile.FileContent[hurtBoxesEntryValuesLocation + 3];
                        int yStretch = PsaFile.FileContent[hurtBoxesEntryValuesLocation + 4];
                        int zStretch = PsaFile.FileContent[hurtBoxesEntryValuesLocation + 5];
                        int radius = PsaFile.FileContent[hurtBoxesEntryValuesLocation + 6];
                        int data = PsaFile.FileContent[hurtBoxesEntryValuesLocation + 7];
                        int bone = (data >> 23) & 0x1FF;
                        int enabled = (data >> 16) & 1;
                        int zone = (data >> 19) & 3;
                        int region = (data >> 21) & 3;
                        hurtBoxes.Entries.Add(new HurtBoxEntry(hurtBoxesEntryLocation, xOffset, yOffset, zOffset, xStretch, yStretch, zStretch, radius, data, bone, enabled, zone, region));
                    }
                }

            }
            Console.WriteLine(hurtBoxes);
            return hurtBoxes;
        }

        public LedgeGrab GetLedgeGrab()
        {
            LedgeGrab ledgeGrab = new LedgeGrab();
            if (PsaFile.FileContent[MiscSectionLocation + 5] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 5] < PsaFile.DataSectionSize)
            {
                ledgeGrab.Offset = PsaFile.FileContent[MiscSectionLocation + 5];
                int numberOfLedgeGrabEntries = PsaFile.FileContent[MiscSectionLocation + 6];

                if (numberOfLedgeGrabEntries > 0 && numberOfLedgeGrabEntries < 256)
                {
                    ledgeGrab.LedgeGrabeEntriesCount = numberOfLedgeGrabEntries;
                    for (int i = 0; i < numberOfLedgeGrabEntries; i++)
                    {
                        int ledgeGrabEntryLocation = PsaFile.FileContent[MiscSectionLocation + 5] + i * 16;
                        int ledgeGrabEntryValuesLocation = ledgeGrabEntryLocation / 4;

                        int x = PsaFile.FileContent[ledgeGrabEntryValuesLocation];
                        int y = PsaFile.FileContent[ledgeGrabEntryValuesLocation + 1];
                        int width = PsaFile.FileContent[ledgeGrabEntryValuesLocation + 2];
                        int height = PsaFile.FileContent[ledgeGrabEntryValuesLocation + 3];
                        ledgeGrab.Entries.Add(new LedgeGrabEntry(ledgeGrabEntryLocation, x, y, width, height));
                    }
                }
            }
            Console.WriteLine(ledgeGrab);
            return ledgeGrab;
        }

        public MiscSection2 GetMiscSection2()
        {
            MiscSection2 miscSection2 = new MiscSection2();
            // counts misc section 2 entries
            if (PsaFile.FileContent[MiscSectionLocation + 7] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 7] < PsaFile.DataSectionSize)
            {
                miscSection2.Offset = PsaFile.FileContent[MiscSectionLocation + 7];
                int numberOfMiscSection2Entries = PsaFile.FileContent[MiscSectionLocation + 8];

                if (numberOfMiscSection2Entries > 0 && numberOfMiscSection2Entries < 256)
                {
                    miscSection2.EntriesCount = numberOfMiscSection2Entries;
                    for (int i = 0; i < numberOfMiscSection2Entries; i++)
                    {
                        int miscSection2EntryLocation = PsaFile.FileContent[MiscSectionLocation + 7] + i * 32;
                        int miscSection2EntryValuesLocation = miscSection2EntryLocation / 4;

                        int unknown0 = PsaFile.FileContent[miscSection2EntryValuesLocation];
                        int unknown1 = PsaFile.FileContent[miscSection2EntryValuesLocation + 1];
                        int unknown2 = PsaFile.FileContent[miscSection2EntryValuesLocation + 2];
                        int unknown3 = PsaFile.FileContent[miscSection2EntryValuesLocation + 3];
                        int unknown4 = PsaFile.FileContent[miscSection2EntryValuesLocation + 4];
                        int unknown5 = PsaFile.FileContent[miscSection2EntryValuesLocation + 5];
                        int unknown6 = PsaFile.FileContent[miscSection2EntryValuesLocation + 6];
                        int unknown7 = PsaFile.FileContent[miscSection2EntryValuesLocation + 7];
                        miscSection2.Entries.Add(new MiscSection2Entry(miscSection2EntryLocation, unknown0, unknown1, unknown2, unknown3, unknown4, unknown5, unknown6, unknown7));
                    }
                }
            }
            Console.WriteLine(miscSection2);
            return miscSection2;
        }

        public BoneReferences GetBoneReferences()
        {
            BoneReferences boneReferences = new BoneReferences();
            // checks if bone references section exists (inside misc section) but does not count them
            if (PsaFile.FileContent[MiscSectionLocation + 9] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 9] < PsaFile.DataSectionSize)
            {
                Console.WriteLine("Bone References Section (In Misc Section) exists");
            }
            else
            {
                Console.WriteLine("Bone References Section (In Misc Section) does NOT exist");
            }
            return boneReferences;
        }

        public ItemBones GetItemBones()
        {
            ItemBones itemBones = new ItemBones();
            // counts item bones entries
            if (PsaFile.FileContent[MiscSectionLocation + 10] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 10] < PsaFile.DataSectionSize)
            {
                int itemBonesEntriesLocation = PsaFile.FileContent[MiscSectionLocation + 10] / 4;
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
            return itemBones;
        }

        public SoundLists GetSoundLists()
        {
            SoundLists soundLists = new SoundLists();
            // counts sound data section
            if (PsaFile.FileContent[MiscSectionLocation + 11] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 11] < PsaFile.DataSectionSize)
            {
                int soundListsLocation = PsaFile.FileContent[MiscSectionLocation + 11] / 4;
                if (PsaFile.FileContent[soundListsLocation] >= 8096 && PsaFile.FileContent[soundListsLocation] < PsaFile.DataSectionSize)
                {
                    int soundListEntriesLocation = PsaFile.FileContent[soundListsLocation] / 4;
                    int numberOfSoundListEntries = PsaFile.FileContent[soundListsLocation + 1];
                    if (numberOfSoundListEntries > 0 && numberOfSoundListEntries < 256)
                    {
                        for (int i = 0; i < numberOfSoundListEntries; i++)
                        {
                            // how many sound datas there are
                            if (PsaFile.FileContent[soundListEntriesLocation + i * 2] >= 8096 && PsaFile.FileContent[soundListEntriesLocation + i * 2] < PsaFile.DataSectionSize)
                            {
                                soundLists.SoundListCount++;
                            }
                        }
                        Console.WriteLine(String.Format("Number of Sound List Entries: {0}", soundLists.SoundListCount));
                    }
                }
            }
            return soundLists;
        }

        public MiscSection5 GetMiscSection5()
        {
            MiscSection5 miscSection5 = new MiscSection5();
            // checks if there is a Misc Section 5
            if (PsaFile.FileContent[MiscSectionLocation + 12] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 12] < PsaFile.DataSectionSize)
            {
                Console.WriteLine("Misc Section 5 exists");
            }
            else
            {
                Console.WriteLine("Misc Section 5 does NOT exist");
            }
            return miscSection5;
        }

        public MultiJump GetMultiJump()
        {
            MultiJump multiJump = new MultiJump();
            // checks if multi jump section exists, checks if there is a hops section and multi jump unknown section
            if (PsaFile.FileContent[MiscSectionLocation + 13] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 13] < PsaFile.DataSectionSize)
            {
                Console.WriteLine("Mutli Jump Section exists");
                int multiJumpLocation = PsaFile.FileContent[MiscSectionLocation + 13] / 4;
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

            return multiJump;
        }

        public Glide GetGlide()
        {
            Glide glide = new Glide();
            // checks if glide section exists
            if (PsaFile.FileContent[MiscSectionLocation + 14] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 14] < PsaFile.DataSectionSize)
            {
                Console.WriteLine("Glide section exists");
            }
            else
            {
                Console.WriteLine("Glide section does NOT exist");
            }

            return glide;
        }

        public Crawl GetCrawl()
        {
            Crawl crawl = new Crawl();
            // checks if crawl section exists
            if (PsaFile.FileContent[MiscSectionLocation + 15] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 15] < PsaFile.DataSectionSize)
            {
                Console.WriteLine("Crawl section exists");
            }
            else
            {
                Console.WriteLine("Crawl section does NOT exist");
            }
            return crawl;
        }

        public CollisionData GetCollisionData()
        {
            CollisionData collisionData = new CollisionData();
            // checks if there is collision data
            if (PsaFile.FileContent[MiscSectionLocation + 16] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 16] < PsaFile.DataSectionSize)
            {
                int collisionDataLocation = PsaFile.FileContent[MiscSectionLocation + 16] / 4;
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
            return collisionData;
        }

        public Tether GetTether()
        {
            Tether tether = new Tether();
            // checks if tether exists
            if (PsaFile.FileContent[MiscSectionLocation + 17] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 17] < PsaFile.DataSectionSize)
            {
                Console.WriteLine("Tether section exists");
            }
            else
            {
                Console.WriteLine("Tether section does NOT exist");
            }
            return tether;
        }

        public MiscSection12 GetMiscSection12()
        {
            MiscSection12 miscSection12 = new MiscSection12();
            // gets count of entries in Misc Section 12
            if (PsaFile.FileContent[MiscSectionLocation + 18] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 18] < PsaFile.DataSectionSize)
            {
                int miscSection12Location = PsaFile.FileContent[MiscSectionLocation + 18] / 4;

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
            return miscSection12;
        }
    }
}
