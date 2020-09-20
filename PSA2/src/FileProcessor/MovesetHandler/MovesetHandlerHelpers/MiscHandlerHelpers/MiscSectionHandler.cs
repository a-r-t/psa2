using PSA2.src.models.fighter.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.MiscHandlerHelpers
{
    public class MiscSectionHandler
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public int MiscSectionLocation { get; private set; } // k

        public MiscSectionHandler(PsaFile psaFile, int dataSectionLocation, int miscSectionLocation)
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
                for (int i = 0; i < 7; i++) // currently hardcoded to 7, there was code in PSAC for getting a count of them but all chars seem to have 7 and the code was ridiculous
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
                        int bone = data >> 23 & 0x1FF;
                        int enabled = data >> 16 & 1;
                        int zone = data >> 19 & 3;
                        int region = data >> 21 & 3;
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
            if (PsaFile.FileContent[MiscSectionLocation + 9] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 9] < PsaFile.DataSectionSize)
            {
                int boneReferencesLocation = PsaFile.FileContent[MiscSectionLocation + 9];
                boneReferences.Offset = boneReferencesLocation;
                int boneReferencesValuesLocation = PsaFile.FileContent[MiscSectionLocation + 9] / 4;
                for (int i = 0; i < 10; i++)
                {
                    boneReferences.Bones.Add(PsaFile.FileContent[boneReferencesValuesLocation + i]);
                }
            }
            Console.WriteLine(boneReferences);
            return boneReferences;
        }

        public ItemBones GetItemBones()
        {
            ItemBones itemBones = new ItemBones();
            if (PsaFile.FileContent[MiscSectionLocation + 10] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 10] < PsaFile.DataSectionSize)
            {
                itemBones.Offset = PsaFile.FileContent[MiscSectionLocation + 10];
                int itemBonesLocation = PsaFile.FileContent[MiscSectionLocation + 10] / 4;
                itemBones.HaveNBoneIndex0 = PsaFile.FileContent[itemBonesLocation];
                itemBones.HaveNBoneIndex1 = PsaFile.FileContent[itemBonesLocation + 1];
                itemBones.ThrowNBoneIndex = PsaFile.FileContent[itemBonesLocation + 2];
                itemBones.DataCount = PsaFile.FileContent[itemBonesLocation + 3];
                itemBones.DataOffset = PsaFile.FileContent[itemBonesLocation + 4];
                itemBones.Pad = PsaFile.FileContent[itemBonesLocation + 5];

                if (PsaFile.FileContent[itemBonesLocation + 4] >= 8096 && PsaFile.FileContent[itemBonesLocation + 4] < PsaFile.DataSectionSize)
                {
                    int numberOfItemBonesEntries = PsaFile.FileContent[itemBonesLocation + 3];
                    itemBones.EntriesCount = numberOfItemBonesEntries;
                    if (numberOfItemBonesEntries > 0 && numberOfItemBonesEntries < 256)
                    {
                        for (int i = 0; i < numberOfItemBonesEntries; i++)
                        {
                            int itemBonesEntriesLocation = PsaFile.FileContent[itemBonesLocation + 4] + i * 16;
                            int itemBonesEntriesValuesLocation = itemBonesEntriesLocation / 4;
                            int unknown0 = PsaFile.FileContent[itemBonesEntriesValuesLocation];
                            int unknown1 = PsaFile.FileContent[itemBonesEntriesValuesLocation + 1];
                            int pad0 = PsaFile.FileContent[itemBonesEntriesValuesLocation + 2];
                            int pad1 = PsaFile.FileContent[itemBonesEntriesValuesLocation + 3];
                            itemBones.Entries.Add(new ItemBonesEntry(itemBonesEntriesLocation, unknown0, unknown1, pad0, pad1));
                        }
                    }
                }
            }
            Console.WriteLine(itemBones);
            return itemBones;
        }

        public SoundLists GetSoundLists()
        {
            SoundLists soundLists = new SoundLists();
            if (PsaFile.FileContent[MiscSectionLocation + 11] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 11] < PsaFile.DataSectionSize)
            {
                soundLists.Offset = PsaFile.FileContent[MiscSectionLocation + 11];
                int soundListsLocation = PsaFile.FileContent[MiscSectionLocation + 11] / 4;
                soundLists.SoundListOffset = PsaFile.FileContent[soundListsLocation];
                soundLists.SoundListCount = PsaFile.FileContent[soundListsLocation + 1];
                if (PsaFile.FileContent[soundListsLocation] >= 8096 && PsaFile.FileContent[soundListsLocation] < PsaFile.DataSectionSize)
                {
                    int numberOfSoundListData = PsaFile.FileContent[soundListsLocation + 1];
                    if (numberOfSoundListData > 0 && numberOfSoundListData < 256)
                    {
                        for (int i = 0; i < numberOfSoundListData; i++)
                        {
                            int soundListDataEntryLocation = PsaFile.FileContent[soundListsLocation] + i * 8;
                            int soundListDataEntrySfxLocation = PsaFile.FileContent[soundListsLocation] / 4 + i * 2;

                            int soundListOffset = PsaFile.FileContent[soundListDataEntrySfxLocation];
                            int soundListCount = PsaFile.FileContent[soundListDataEntrySfxLocation + 1];
                            SoundDataEntry soundDataEntry = new SoundDataEntry(soundListDataEntryLocation, soundListOffset, soundListCount);
                            soundLists.Entries.Add(soundDataEntry);

                            int soundListDataEntrySfxValuesLocation = PsaFile.FileContent[soundListDataEntrySfxLocation] / 4;
                            int numberOfSoundListDataEntrySfxValues = PsaFile.FileContent[soundListDataEntrySfxLocation + 1];
                            if (numberOfSoundListDataEntrySfxValues > 0 && numberOfSoundListDataEntrySfxValues < 128)
                            {
                                for (int j = 0; j < numberOfSoundListDataEntrySfxValues; j++)
                                {
                                    soundDataEntry.SfxIds.Add(PsaFile.FileContent[soundListDataEntrySfxValuesLocation + j]);
                                }
                            }

                        }
                    }
                }
            }
            Console.WriteLine(soundLists);
            return soundLists;
        }

        public MiscSection5 GetMiscSection5()
        {
            MiscSection5 miscSection5 = new MiscSection5();
            // checks if there is a Misc Section 5
            if (PsaFile.FileContent[MiscSectionLocation + 12] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 12] < PsaFile.DataSectionSize)
            {
                int miscSection5EntriesLocation = PsaFile.FileContent[MiscSectionLocation + 11] / 4;

                int numberOfMiscSection5Entries;
                // not a clue what's going on here
                if (PsaFile.FileContent[MiscSectionLocation + 11] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 11] < PsaFile.DataSectionSize
                    && PsaFile.FileContent[MiscSectionLocation + 12] + 16 != PsaFile.FileContent[miscSection5EntriesLocation])
                {
                    numberOfMiscSection5Entries = 6;
                }
                else
                {
                    numberOfMiscSection5Entries = 4;
                }

                int miscSection5Location = PsaFile.FileContent[MiscSectionLocation + 12];
                miscSection5.Offset = miscSection5Location;
                int miscSection5EntriesValuesLocation = miscSection5Location / 4;
                for (int i = 0; i < numberOfMiscSection5Entries; i++)
                {
                    miscSection5.Entries.Add(PsaFile.FileContent[miscSection5EntriesValuesLocation + i]);
                }
            }
            Console.WriteLine(miscSection5);
            return miscSection5;
        }

        public MultiJump GetMultiJump()
        {
            MultiJump multiJump = new MultiJump();
            if (PsaFile.FileContent[MiscSectionLocation + 13] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 13] < PsaFile.DataSectionSize)
            {
                Console.WriteLine("Mutli Jump Section exists");
                multiJump.Offset = PsaFile.FileContent[MiscSectionLocation + 13];
                int multiJumpLocation = PsaFile.FileContent[MiscSectionLocation + 13] / 4;
                multiJump.Unknown0 = PsaFile.FileContent[multiJumpLocation];
                multiJump.Unknown1 = PsaFile.FileContent[multiJumpLocation + 1];
                multiJump.Unknown2 = PsaFile.FileContent[multiJumpLocation + 2];
                multiJump.HorizontalBoost = PsaFile.FileContent[multiJumpLocation + 3];
                multiJump.HopsOffset = PsaFile.FileContent[multiJumpLocation + 4];
                multiJump.UnknownDatasOffset = PsaFile.FileContent[multiJumpLocation + 5];
                multiJump.TurnFrames = PsaFile.FileContent[multiJumpLocation + 6];

                if (PsaFile.FileContent[multiJumpLocation + 4] >= 8096 && PsaFile.FileContent[multiJumpLocation + 4] < PsaFile.DataSectionSize)
                {
                    multiJump.Hops.Offset = PsaFile.FileContent[multiJumpLocation + 4];
                    int hopsLocation = PsaFile.FileContent[multiJumpLocation + 4] / 4;

                    int numberOfHopsValues = (PsaFile.FileContent[MiscSectionLocation + 13] - PsaFile.FileContent[multiJumpLocation + 4]) / 4;
                    for (int i = 0; i < numberOfHopsValues; i++)
                    {
                        multiJump.Hops.HopVelocities.Add(PsaFile.FileContent[hopsLocation + i]);
                    }
                }

                if (PsaFile.FileContent[multiJumpLocation + 5] >= 8096 && PsaFile.FileContent[multiJumpLocation + 5] < PsaFile.DataSectionSize)
                {
                    multiJump.MultiJumpUnknown.Offset = PsaFile.FileContent[multiJumpLocation + 5];
                    int multiJumpUnknownLocation = PsaFile.FileContent[multiJumpLocation + 5] / 4;
                    for (int i = 0; i < 12; i++)
                    {
                        multiJump.MultiJumpUnknown.Unknowns.Add(PsaFile.FileContent[multiJumpUnknownLocation + i]);
                    }
                }
            }
            Console.WriteLine(multiJump);
            return multiJump;
        }

        public Glide GetGlide()
        {
            Glide glide = new Glide();
            if (PsaFile.FileContent[MiscSectionLocation + 14] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 14] < PsaFile.DataSectionSize)
            {
                glide.Offset = PsaFile.FileContent[MiscSectionLocation + 14];
                int glideLocation = PsaFile.FileContent[MiscSectionLocation + 14] / 4;
                for (int i = 0; i < 21; i++)
                {
                    glide.Datas.Add(PsaFile.FileContent[glideLocation + i]);
                }
            }
            Console.WriteLine(glide);
            return glide;
        }

        public Crawl GetCrawl()
        {
            Crawl crawl = new Crawl();
            // checks if crawl section exists
            if (PsaFile.FileContent[MiscSectionLocation + 15] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 15] < PsaFile.DataSectionSize)
            {
                crawl.Offset = PsaFile.FileContent[MiscSectionLocation + 15];
                int crawlLocation = PsaFile.FileContent[MiscSectionLocation + 15] / 4;
                crawl.Forward = PsaFile.FileContent[crawlLocation];
                crawl.Backward = PsaFile.FileContent[crawlLocation + 1];
            }
            Console.WriteLine(crawl);
            return crawl;
        }

        public CollisionData GetCollisionData()
        {
            CollisionData collisionData = new CollisionData();
            if (PsaFile.FileContent[MiscSectionLocation + 16] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 16] < PsaFile.DataSectionSize)
            {
                collisionData.Offset = PsaFile.FileContent[MiscSectionLocation + 16];
                int collisionDataLocation = PsaFile.FileContent[MiscSectionLocation + 16] / 4; // k
                collisionData.CollisionDataOffset = PsaFile.FileContent[MiscSectionLocation + 16];
                collisionData.EntryOffset = PsaFile.FileContent[collisionDataLocation];
                collisionData.Count = PsaFile.FileContent[collisionDataLocation + 1];
                if (PsaFile.FileContent[collisionDataLocation] >= 8096 && PsaFile.FileContent[collisionDataLocation] < PsaFile.DataSectionSize)
                {

                    int collisionDataEntryLocation = PsaFile.FileContent[collisionDataLocation] / 4; // n
                    collisionData.DataOffset = PsaFile.FileContent[collisionDataEntryLocation];
                    if (PsaFile.FileContent[collisionDataEntryLocation] >= 8096 && PsaFile.FileContent[collisionDataEntryLocation] < PsaFile.DataSectionSize)
                    {
                        int collisionDataEntryBoneDataLocation = PsaFile.FileContent[collisionDataEntryLocation] / 4; // j
                        collisionData.CollisionDataEntry.Offset = PsaFile.FileContent[collisionDataEntryLocation];
                        collisionData.CollisionDataEntry.Type = PsaFile.FileContent[collisionDataEntryBoneDataLocation];

                        // so if there is extra data for bone data offset and count, then the unknowns get pushed down a few indexes (which is why the 1, 3 goes to 3, 6)
                        int unknownsStartIndex;
                        int unknownsEndIndex;
                        if (PsaFile.FileContent[collisionDataEntryLocation] + 16 == PsaFile.FileContent[collisionDataLocation])
                        {
                            unknownsStartIndex = 1;
                            unknownsEndIndex = 3;
                        }
                        else
                        {
                            collisionData.CollisionDataEntry.BoneDataOffset = PsaFile.FileContent[collisionDataEntryBoneDataLocation + 1];
                            collisionData.CollisionDataEntry.Count = PsaFile.FileContent[collisionDataEntryBoneDataLocation + 2];
                            unknownsStartIndex = 3;
                            unknownsEndIndex = 6;
                        }
                        for (int i = unknownsStartIndex; i < unknownsEndIndex; i++)
                        {
                            collisionData.CollisionDataEntry.Unknowns.Add(PsaFile.FileContent[collisionDataEntryBoneDataLocation + i]);
                        }

                        if (PsaFile.FileContent[collisionDataEntryBoneDataLocation + 1] >= 8096 && PsaFile.FileContent[collisionDataEntryBoneDataLocation + 1] < PsaFile.DataSectionSize)
                        {
                            collisionData.CollisionDataEntry.BonesListOffset = PsaFile.FileContent[collisionDataEntryBoneDataLocation + 1];
                            int collisionDataEntryBoneDataValuesLocation = PsaFile.FileContent[collisionDataEntryBoneDataLocation + 1] / 4;
                            int collisionDataBoneDataCount = PsaFile.FileContent[collisionDataEntryBoneDataLocation + 2];
                            if (collisionDataBoneDataCount > 0 && collisionDataBoneDataCount < 256) // psac has this at 128 in one place and 256 in another...256 should be correct though
                            {
                                for (int i = 0; i < collisionDataBoneDataCount; i++)
                                {
                                    collisionData.CollisionDataEntry.Bones.Add(PsaFile.FileContent[collisionDataEntryBoneDataValuesLocation + i]);
                                }
                            }

                        }
                    }
                }
            }
            Console.WriteLine(collisionData);
            return collisionData;
        }

        public Tether GetTether()
        {
            Tether tether = new Tether();
            if (PsaFile.FileContent[MiscSectionLocation + 17] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 17] < PsaFile.DataSectionSize)
            {
                tether.Offset = PsaFile.FileContent[MiscSectionLocation + 17];
                int tetherLocation = PsaFile.FileContent[MiscSectionLocation + 17] / 4;
                tether.HangFrameCount = PsaFile.FileContent[tetherLocation];
                tether.Unknown = PsaFile.FileContent[tetherLocation + 1];
            }
            Console.WriteLine(tether);
            return tether;
        }

        public MiscSection12 GetMiscSection12()
        {
            MiscSection12 miscSection12 = new MiscSection12();
            if (PsaFile.FileContent[MiscSectionLocation + 18] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 18] < PsaFile.DataSectionSize)
            {
                miscSection12.Offset = PsaFile.FileContent[MiscSectionLocation + 18];
                int miscSection12Location = PsaFile.FileContent[MiscSectionLocation + 18] / 4;
                miscSection12.DataOffset = PsaFile.FileContent[miscSection12Location];
                miscSection12.DataCount = PsaFile.FileContent[miscSection12Location + 1];

                if (PsaFile.FileContent[miscSection12Location] >= 8096 && PsaFile.FileContent[miscSection12Location] < PsaFile.DataSectionSize)
                {
                    int numberOfMiscSection12Entries = PsaFile.FileContent[miscSection12Location + 1];
                    if (numberOfMiscSection12Entries > 0 && numberOfMiscSection12Entries < 256) // psa-c has this at 100...but 256 should be fine?
                    {
                        miscSection12.ItemsListOffset = PsaFile.FileContent[miscSection12Location]; // seems not correct, come back to this
                        int miscSection12EntriesValuesLocation = PsaFile.FileContent[miscSection12Location] / 4;
                        for (int i = 0; i < numberOfMiscSection12Entries; i++)
                        {
                            miscSection12.Items.Add(PsaFile.FileContent[miscSection12EntriesValuesLocation + i]);
                        }
                    }
                }
            }
            Console.WriteLine(miscSection12);
            return miscSection12;
        }
    }
}
