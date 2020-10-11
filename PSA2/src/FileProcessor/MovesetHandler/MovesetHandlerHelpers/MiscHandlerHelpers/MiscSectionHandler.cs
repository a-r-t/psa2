using PSA2.src.Models.Fighter.Misc;
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
            if (PsaFile.DataSection[DataSectionLocation + 4] >= 8096 && PsaFile.DataSection[DataSectionLocation + 4] < PsaFile.DataSectionSize)
            {
                miscSection.Offset = PsaFile.DataSection[DataSectionLocation + 4];
                miscSection.MiscSection1Offset = PsaFile.DataSection[MiscSectionLocation];
                miscSection.FinalSmashAuraOffset = PsaFile.DataSection[MiscSectionLocation + 1];
                miscSection.FinalSmashAuraCount = PsaFile.DataSection[MiscSectionLocation + 2];
                miscSection.HurtBoxOffset = PsaFile.DataSection[MiscSectionLocation + 3];
                miscSection.HurtBoxCount = PsaFile.DataSection[MiscSectionLocation + 4];
                miscSection.LedgeGrabOffset = PsaFile.DataSection[MiscSectionLocation + 5];
                miscSection.LedgeGrabCount = PsaFile.DataSection[MiscSectionLocation + 6];
                miscSection.MiscSection2Offset = PsaFile.DataSection[MiscSectionLocation + 7];
                miscSection.MiscSection2Count = PsaFile.DataSection[MiscSectionLocation + 8];
                miscSection.BoneReferencesOffset = PsaFile.DataSection[MiscSectionLocation + 9];
                miscSection.ItemBonesOffset = PsaFile.DataSection[MiscSectionLocation + 10];
                miscSection.SoundDataOffset = PsaFile.DataSection[MiscSectionLocation + 11];
                miscSection.MiscSection5Offset = PsaFile.DataSection[MiscSectionLocation + 12];
                miscSection.MultiJumpOffset = PsaFile.DataSection[MiscSectionLocation + 13];
                miscSection.GlideOffset = PsaFile.DataSection[MiscSectionLocation + 14];
                miscSection.CrawlOffset = PsaFile.DataSection[MiscSectionLocation + 15];
                miscSection.CollisionDataOffset = PsaFile.DataSection[MiscSectionLocation + 16];
                miscSection.TetherOffset = PsaFile.DataSection[MiscSectionLocation + 17];
                miscSection.MiscSection12Offset = PsaFile.DataSection[MiscSectionLocation + 18];
            }
            Console.WriteLine(miscSection);
            return miscSection;
        }

        public MiscSection1 GetMiscSection1()
        {
            MiscSection1 miscSection1 = new MiscSection1();

            if (PsaFile.DataSection[MiscSectionLocation] >= 8096 && PsaFile.DataSection[MiscSectionLocation] < PsaFile.DataSectionSize)
            {
                miscSection1.Offset = PsaFile.DataSection[MiscSectionLocation];
                int miscSection1Location = PsaFile.DataSection[MiscSectionLocation] / 4;
                for (int i = 0; i < 7; i++) // currently hardcoded to 7, there was code in PSAC for getting a count of them but all chars seem to have 7 and the code was ridiculous
                {
                    miscSection1.Params.Add(new MiscSection1Param($"Data{i + 1}", PsaFile.DataSection[miscSection1Location + i]));
                }
                miscSection1.Params.Add(new MiscSection1Param("Misc Section1 Offset", PsaFile.DataSection[MiscSectionLocation]));
            }
            Console.WriteLine(miscSection1);
            return miscSection1;
        }

        public FinalSmashAura GetFinalSmashAura()
        {
            FinalSmashAura finalSmashAura = new FinalSmashAura();
            if (PsaFile.DataSection[MiscSectionLocation + 1] >= 8096 && PsaFile.DataSection[MiscSectionLocation + 1] < PsaFile.DataSectionSize)
            {
                finalSmashAura.Offset = PsaFile.DataSection[MiscSectionLocation + 1];
                int numberOfFinalSmashAuraEntries = PsaFile.DataSection[MiscSectionLocation + 2];

                if (numberOfFinalSmashAuraEntries > 0 && numberOfFinalSmashAuraEntries < 256)
                {
                    finalSmashAura.FinalSmashAuraEntryCount = numberOfFinalSmashAuraEntries;
                    for (int i = 0; i < numberOfFinalSmashAuraEntries; i++)
                    {
                        int finalSmashAuraEntryLocation = PsaFile.DataSection[MiscSectionLocation + 1] + i * 20;
                        int finalSmashAuraEntryValuesLocation = finalSmashAuraEntryLocation / 4;
                        int bone = PsaFile.DataSection[finalSmashAuraEntryValuesLocation];
                        int x = PsaFile.DataSection[finalSmashAuraEntryValuesLocation + 1];
                        int y = PsaFile.DataSection[finalSmashAuraEntryValuesLocation + 2];
                        int width = PsaFile.DataSection[finalSmashAuraEntryValuesLocation + 3];
                        int height = PsaFile.DataSection[finalSmashAuraEntryValuesLocation + 4];
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
            if (PsaFile.DataSection[MiscSectionLocation + 3] >= 8096 && PsaFile.DataSection[MiscSectionLocation + 3] < PsaFile.DataSectionSize)
            {

                hurtBoxes.Offset = PsaFile.DataSection[MiscSectionLocation + 3];
                int numberOfHurtBoxEntries = PsaFile.DataSection[MiscSectionLocation + 4];

                if (numberOfHurtBoxEntries > 0 && numberOfHurtBoxEntries < 256)
                {
                    hurtBoxes.HurtBoxEntryCount = numberOfHurtBoxEntries;
                    for (int i = 0; i < numberOfHurtBoxEntries; i++)
                    {
                        int hurtBoxesEntryLocation = PsaFile.DataSection[MiscSectionLocation + 3] + i * 32;
                        int hurtBoxesEntryValuesLocation = hurtBoxesEntryLocation / 4;

                        int xOffset = PsaFile.DataSection[hurtBoxesEntryValuesLocation];
                        int yOffset = PsaFile.DataSection[hurtBoxesEntryValuesLocation + 1];
                        int zOffset = PsaFile.DataSection[hurtBoxesEntryValuesLocation + 2];
                        int xStretch = PsaFile.DataSection[hurtBoxesEntryValuesLocation + 3];
                        int yStretch = PsaFile.DataSection[hurtBoxesEntryValuesLocation + 4];
                        int zStretch = PsaFile.DataSection[hurtBoxesEntryValuesLocation + 5];
                        int radius = PsaFile.DataSection[hurtBoxesEntryValuesLocation + 6];
                        int data = PsaFile.DataSection[hurtBoxesEntryValuesLocation + 7];
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
            if (PsaFile.DataSection[MiscSectionLocation + 5] >= 8096 && PsaFile.DataSection[MiscSectionLocation + 5] < PsaFile.DataSectionSize)
            {
                ledgeGrab.Offset = PsaFile.DataSection[MiscSectionLocation + 5];
                int numberOfLedgeGrabEntries = PsaFile.DataSection[MiscSectionLocation + 6];

                if (numberOfLedgeGrabEntries > 0 && numberOfLedgeGrabEntries < 256)
                {
                    ledgeGrab.LedgeGrabeEntriesCount = numberOfLedgeGrabEntries;
                    for (int i = 0; i < numberOfLedgeGrabEntries; i++)
                    {
                        int ledgeGrabEntryLocation = PsaFile.DataSection[MiscSectionLocation + 5] + i * 16;
                        int ledgeGrabEntryValuesLocation = ledgeGrabEntryLocation / 4;

                        int x = PsaFile.DataSection[ledgeGrabEntryValuesLocation];
                        int y = PsaFile.DataSection[ledgeGrabEntryValuesLocation + 1];
                        int width = PsaFile.DataSection[ledgeGrabEntryValuesLocation + 2];
                        int height = PsaFile.DataSection[ledgeGrabEntryValuesLocation + 3];
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
            if (PsaFile.DataSection[MiscSectionLocation + 7] >= 8096 && PsaFile.DataSection[MiscSectionLocation + 7] < PsaFile.DataSectionSize)
            {
                miscSection2.Offset = PsaFile.DataSection[MiscSectionLocation + 7];
                int numberOfMiscSection2Entries = PsaFile.DataSection[MiscSectionLocation + 8];

                if (numberOfMiscSection2Entries > 0 && numberOfMiscSection2Entries < 256)
                {
                    miscSection2.EntriesCount = numberOfMiscSection2Entries;
                    for (int i = 0; i < numberOfMiscSection2Entries; i++)
                    {
                        int miscSection2EntryLocation = PsaFile.DataSection[MiscSectionLocation + 7] + i * 32;
                        int miscSection2EntryValuesLocation = miscSection2EntryLocation / 4;

                        int unknown0 = PsaFile.DataSection[miscSection2EntryValuesLocation];
                        int unknown1 = PsaFile.DataSection[miscSection2EntryValuesLocation + 1];
                        int unknown2 = PsaFile.DataSection[miscSection2EntryValuesLocation + 2];
                        int unknown3 = PsaFile.DataSection[miscSection2EntryValuesLocation + 3];
                        int unknown4 = PsaFile.DataSection[miscSection2EntryValuesLocation + 4];
                        int unknown5 = PsaFile.DataSection[miscSection2EntryValuesLocation + 5];
                        int unknown6 = PsaFile.DataSection[miscSection2EntryValuesLocation + 6];
                        int unknown7 = PsaFile.DataSection[miscSection2EntryValuesLocation + 7];
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
            if (PsaFile.DataSection[MiscSectionLocation + 9] >= 8096 && PsaFile.DataSection[MiscSectionLocation + 9] < PsaFile.DataSectionSize)
            {
                int boneReferencesLocation = PsaFile.DataSection[MiscSectionLocation + 9];
                boneReferences.Offset = boneReferencesLocation;
                int boneReferencesValuesLocation = PsaFile.DataSection[MiscSectionLocation + 9] / 4;
                for (int i = 0; i < 10; i++)
                {
                    boneReferences.Bones.Add(PsaFile.DataSection[boneReferencesValuesLocation + i]);
                }
            }
            Console.WriteLine(boneReferences);
            return boneReferences;
        }

        public ItemBones GetItemBones()
        {
            ItemBones itemBones = new ItemBones();
            if (PsaFile.DataSection[MiscSectionLocation + 10] >= 8096 && PsaFile.DataSection[MiscSectionLocation + 10] < PsaFile.DataSectionSize)
            {
                itemBones.Offset = PsaFile.DataSection[MiscSectionLocation + 10];
                int itemBonesLocation = PsaFile.DataSection[MiscSectionLocation + 10] / 4;
                itemBones.HaveNBoneIndex0 = PsaFile.DataSection[itemBonesLocation];
                itemBones.HaveNBoneIndex1 = PsaFile.DataSection[itemBonesLocation + 1];
                itemBones.ThrowNBoneIndex = PsaFile.DataSection[itemBonesLocation + 2];
                itemBones.DataCount = PsaFile.DataSection[itemBonesLocation + 3];
                itemBones.DataOffset = PsaFile.DataSection[itemBonesLocation + 4];
                itemBones.Pad = PsaFile.DataSection[itemBonesLocation + 5];

                if (PsaFile.DataSection[itemBonesLocation + 4] >= 8096 && PsaFile.DataSection[itemBonesLocation + 4] < PsaFile.DataSectionSize)
                {
                    int numberOfItemBonesEntries = PsaFile.DataSection[itemBonesLocation + 3];
                    itemBones.EntriesCount = numberOfItemBonesEntries;
                    if (numberOfItemBonesEntries > 0 && numberOfItemBonesEntries < 256)
                    {
                        for (int i = 0; i < numberOfItemBonesEntries; i++)
                        {
                            int itemBonesEntriesLocation = PsaFile.DataSection[itemBonesLocation + 4] + i * 16;
                            int itemBonesEntriesValuesLocation = itemBonesEntriesLocation / 4;
                            int unknown0 = PsaFile.DataSection[itemBonesEntriesValuesLocation];
                            int unknown1 = PsaFile.DataSection[itemBonesEntriesValuesLocation + 1];
                            int pad0 = PsaFile.DataSection[itemBonesEntriesValuesLocation + 2];
                            int pad1 = PsaFile.DataSection[itemBonesEntriesValuesLocation + 3];
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
            if (PsaFile.DataSection[MiscSectionLocation + 11] >= 8096 && PsaFile.DataSection[MiscSectionLocation + 11] < PsaFile.DataSectionSize)
            {
                soundLists.Offset = PsaFile.DataSection[MiscSectionLocation + 11];
                int soundListsLocation = PsaFile.DataSection[MiscSectionLocation + 11] / 4;
                soundLists.SoundListOffset = PsaFile.DataSection[soundListsLocation];
                soundLists.SoundListCount = PsaFile.DataSection[soundListsLocation + 1];
                if (PsaFile.DataSection[soundListsLocation] >= 8096 && PsaFile.DataSection[soundListsLocation] < PsaFile.DataSectionSize)
                {
                    int numberOfSoundListData = PsaFile.DataSection[soundListsLocation + 1];
                    if (numberOfSoundListData > 0 && numberOfSoundListData < 256)
                    {
                        for (int i = 0; i < numberOfSoundListData; i++)
                        {
                            int soundListDataEntryLocation = PsaFile.DataSection[soundListsLocation] + i * 8;
                            int soundListDataEntrySfxLocation = PsaFile.DataSection[soundListsLocation] / 4 + i * 2;

                            int soundListOffset = PsaFile.DataSection[soundListDataEntrySfxLocation];
                            int soundListCount = PsaFile.DataSection[soundListDataEntrySfxLocation + 1];
                            SoundDataEntry soundDataEntry = new SoundDataEntry(soundListDataEntryLocation, soundListOffset, soundListCount);
                            soundLists.Entries.Add(soundDataEntry);

                            int soundListDataEntrySfxValuesLocation = PsaFile.DataSection[soundListDataEntrySfxLocation] / 4;
                            int numberOfSoundListDataEntrySfxValues = PsaFile.DataSection[soundListDataEntrySfxLocation + 1];
                            if (numberOfSoundListDataEntrySfxValues > 0 && numberOfSoundListDataEntrySfxValues < 128)
                            {
                                for (int j = 0; j < numberOfSoundListDataEntrySfxValues; j++)
                                {
                                    soundDataEntry.SfxIds.Add(PsaFile.DataSection[soundListDataEntrySfxValuesLocation + j]);
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
            if (PsaFile.DataSection[MiscSectionLocation + 12] >= 8096 && PsaFile.DataSection[MiscSectionLocation + 12] < PsaFile.DataSectionSize)
            {
                int miscSection5EntriesLocation = PsaFile.DataSection[MiscSectionLocation + 11] / 4;

                int numberOfMiscSection5Entries;
                // not a clue what's going on here
                if (PsaFile.DataSection[MiscSectionLocation + 11] >= 8096 && PsaFile.DataSection[MiscSectionLocation + 11] < PsaFile.DataSectionSize
                    && PsaFile.DataSection[MiscSectionLocation + 12] + 16 != PsaFile.DataSection[miscSection5EntriesLocation])
                {
                    numberOfMiscSection5Entries = 6;
                }
                else
                {
                    numberOfMiscSection5Entries = 4;
                }

                int miscSection5Location = PsaFile.DataSection[MiscSectionLocation + 12];
                miscSection5.Offset = miscSection5Location;
                int miscSection5EntriesValuesLocation = miscSection5Location / 4;
                for (int i = 0; i < numberOfMiscSection5Entries; i++)
                {
                    miscSection5.Entries.Add(PsaFile.DataSection[miscSection5EntriesValuesLocation + i]);
                }
            }
            Console.WriteLine(miscSection5);
            return miscSection5;
        }

        public MultiJump GetMultiJump()
        {
            MultiJump multiJump = new MultiJump();
            if (PsaFile.DataSection[MiscSectionLocation + 13] >= 8096 && PsaFile.DataSection[MiscSectionLocation + 13] < PsaFile.DataSectionSize)
            {
                Console.WriteLine("Mutli Jump Section exists");
                multiJump.Offset = PsaFile.DataSection[MiscSectionLocation + 13];
                int multiJumpLocation = PsaFile.DataSection[MiscSectionLocation + 13] / 4;
                multiJump.Unknown0 = PsaFile.DataSection[multiJumpLocation];
                multiJump.Unknown1 = PsaFile.DataSection[multiJumpLocation + 1];
                multiJump.Unknown2 = PsaFile.DataSection[multiJumpLocation + 2];
                multiJump.HorizontalBoost = PsaFile.DataSection[multiJumpLocation + 3];
                multiJump.HopsOffset = PsaFile.DataSection[multiJumpLocation + 4];
                multiJump.UnknownDatasOffset = PsaFile.DataSection[multiJumpLocation + 5];
                multiJump.TurnFrames = PsaFile.DataSection[multiJumpLocation + 6];

                if (PsaFile.DataSection[multiJumpLocation + 4] >= 8096 && PsaFile.DataSection[multiJumpLocation + 4] < PsaFile.DataSectionSize)
                {
                    multiJump.Hops.Offset = PsaFile.DataSection[multiJumpLocation + 4];
                    int hopsLocation = PsaFile.DataSection[multiJumpLocation + 4] / 4;

                    int numberOfHopsValues = (PsaFile.DataSection[MiscSectionLocation + 13] - PsaFile.DataSection[multiJumpLocation + 4]) / 4;
                    for (int i = 0; i < numberOfHopsValues; i++)
                    {
                        multiJump.Hops.HopVelocities.Add(PsaFile.DataSection[hopsLocation + i]);
                    }
                }

                if (PsaFile.DataSection[multiJumpLocation + 5] >= 8096 && PsaFile.DataSection[multiJumpLocation + 5] < PsaFile.DataSectionSize)
                {
                    multiJump.MultiJumpUnknown.Offset = PsaFile.DataSection[multiJumpLocation + 5];
                    int multiJumpUnknownLocation = PsaFile.DataSection[multiJumpLocation + 5] / 4;
                    for (int i = 0; i < 12; i++)
                    {
                        multiJump.MultiJumpUnknown.Unknowns.Add(PsaFile.DataSection[multiJumpUnknownLocation + i]);
                    }
                }
            }
            Console.WriteLine(multiJump);
            return multiJump;
        }

        public Glide GetGlide()
        {
            Glide glide = new Glide();
            if (PsaFile.DataSection[MiscSectionLocation + 14] >= 8096 && PsaFile.DataSection[MiscSectionLocation + 14] < PsaFile.DataSectionSize)
            {
                glide.Offset = PsaFile.DataSection[MiscSectionLocation + 14];
                int glideLocation = PsaFile.DataSection[MiscSectionLocation + 14] / 4;
                for (int i = 0; i < 21; i++)
                {
                    glide.Datas.Add(PsaFile.DataSection[glideLocation + i]);
                }
            }
            Console.WriteLine(glide);
            return glide;
        }

        public Crawl GetCrawl()
        {
            Crawl crawl = new Crawl();
            // checks if crawl section exists
            if (PsaFile.DataSection[MiscSectionLocation + 15] >= 8096 && PsaFile.DataSection[MiscSectionLocation + 15] < PsaFile.DataSectionSize)
            {
                crawl.Offset = PsaFile.DataSection[MiscSectionLocation + 15];
                int crawlLocation = PsaFile.DataSection[MiscSectionLocation + 15] / 4;
                crawl.Forward = PsaFile.DataSection[crawlLocation];
                crawl.Backward = PsaFile.DataSection[crawlLocation + 1];
            }
            Console.WriteLine(crawl);
            return crawl;
        }

        public CollisionData GetCollisionData()
        {
            CollisionData collisionData = new CollisionData();
            if (PsaFile.DataSection[MiscSectionLocation + 16] >= 8096 && PsaFile.DataSection[MiscSectionLocation + 16] < PsaFile.DataSectionSize)
            {
                collisionData.Offset = PsaFile.DataSection[MiscSectionLocation + 16];
                int collisionDataLocation = PsaFile.DataSection[MiscSectionLocation + 16] / 4; // k
                collisionData.CollisionDataOffset = PsaFile.DataSection[MiscSectionLocation + 16];
                collisionData.EntryOffset = PsaFile.DataSection[collisionDataLocation];
                collisionData.Count = PsaFile.DataSection[collisionDataLocation + 1];
                if (PsaFile.DataSection[collisionDataLocation] >= 8096 && PsaFile.DataSection[collisionDataLocation] < PsaFile.DataSectionSize)
                {

                    int collisionDataEntryLocation = PsaFile.DataSection[collisionDataLocation] / 4; // n
                    collisionData.DataOffset = PsaFile.DataSection[collisionDataEntryLocation];
                    if (PsaFile.DataSection[collisionDataEntryLocation] >= 8096 && PsaFile.DataSection[collisionDataEntryLocation] < PsaFile.DataSectionSize)
                    {
                        int collisionDataEntryBoneDataLocation = PsaFile.DataSection[collisionDataEntryLocation] / 4; // j
                        collisionData.CollisionDataEntry.Offset = PsaFile.DataSection[collisionDataEntryLocation];
                        collisionData.CollisionDataEntry.Type = PsaFile.DataSection[collisionDataEntryBoneDataLocation];

                        // so if there is extra data for bone data offset and count, then the unknowns get pushed down a few indexes (which is why the 1, 3 goes to 3, 6)
                        int unknownsStartIndex;
                        int unknownsEndIndex;
                        if (PsaFile.DataSection[collisionDataEntryLocation] + 16 == PsaFile.DataSection[collisionDataLocation])
                        {
                            unknownsStartIndex = 1;
                            unknownsEndIndex = 3;
                        }
                        else
                        {
                            collisionData.CollisionDataEntry.BoneDataOffset = PsaFile.DataSection[collisionDataEntryBoneDataLocation + 1];
                            collisionData.CollisionDataEntry.Count = PsaFile.DataSection[collisionDataEntryBoneDataLocation + 2];
                            unknownsStartIndex = 3;
                            unknownsEndIndex = 6;
                        }
                        for (int i = unknownsStartIndex; i < unknownsEndIndex; i++)
                        {
                            collisionData.CollisionDataEntry.Unknowns.Add(PsaFile.DataSection[collisionDataEntryBoneDataLocation + i]);
                        }

                        if (PsaFile.DataSection[collisionDataEntryBoneDataLocation + 1] >= 8096 && PsaFile.DataSection[collisionDataEntryBoneDataLocation + 1] < PsaFile.DataSectionSize)
                        {
                            collisionData.CollisionDataEntry.BonesListOffset = PsaFile.DataSection[collisionDataEntryBoneDataLocation + 1];
                            int collisionDataEntryBoneDataValuesLocation = PsaFile.DataSection[collisionDataEntryBoneDataLocation + 1] / 4;
                            int collisionDataBoneDataCount = PsaFile.DataSection[collisionDataEntryBoneDataLocation + 2];
                            if (collisionDataBoneDataCount > 0 && collisionDataBoneDataCount < 256) // psac has this at 128 in one place and 256 in another...256 should be correct though
                            {
                                for (int i = 0; i < collisionDataBoneDataCount; i++)
                                {
                                    collisionData.CollisionDataEntry.Bones.Add(PsaFile.DataSection[collisionDataEntryBoneDataValuesLocation + i]);
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
            if (PsaFile.DataSection[MiscSectionLocation + 17] >= 8096 && PsaFile.DataSection[MiscSectionLocation + 17] < PsaFile.DataSectionSize)
            {
                tether.Offset = PsaFile.DataSection[MiscSectionLocation + 17];
                int tetherLocation = PsaFile.DataSection[MiscSectionLocation + 17] / 4;
                tether.HangFrameCount = PsaFile.DataSection[tetherLocation];
                tether.Unknown = PsaFile.DataSection[tetherLocation + 1];
            }
            Console.WriteLine(tether);
            return tether;
        }

        public MiscSection12 GetMiscSection12()
        {
            MiscSection12 miscSection12 = new MiscSection12();
            if (PsaFile.DataSection[MiscSectionLocation + 18] >= 8096 && PsaFile.DataSection[MiscSectionLocation + 18] < PsaFile.DataSectionSize)
            {
                miscSection12.Offset = PsaFile.DataSection[MiscSectionLocation + 18];
                int miscSection12Location = PsaFile.DataSection[MiscSectionLocation + 18] / 4;
                miscSection12.DataOffset = PsaFile.DataSection[miscSection12Location];
                miscSection12.DataCount = PsaFile.DataSection[miscSection12Location + 1];

                if (PsaFile.DataSection[miscSection12Location] >= 8096 && PsaFile.DataSection[miscSection12Location] < PsaFile.DataSectionSize)
                {
                    int numberOfMiscSection12Entries = PsaFile.DataSection[miscSection12Location + 1];
                    if (numberOfMiscSection12Entries > 0 && numberOfMiscSection12Entries < 256) // psa-c has this at 100...but 256 should be fine?
                    {
                        miscSection12.ItemsListOffset = PsaFile.DataSection[miscSection12Location]; // seems not correct, come back to this
                        int miscSection12EntriesValuesLocation = PsaFile.DataSection[miscSection12Location] / 4;
                        for (int i = 0; i < numberOfMiscSection12Entries; i++)
                        {
                            miscSection12.Items.Add(PsaFile.DataSection[miscSection12EntriesValuesLocation + i]);
                        }
                    }
                }
            }
            Console.WriteLine(miscSection12);
            return miscSection12;
        }
    }
}
