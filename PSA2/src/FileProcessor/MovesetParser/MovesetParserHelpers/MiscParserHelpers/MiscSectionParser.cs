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
        public int MiscSectionLocation { get; private set; }

        public MiscSectionParser(PsaFile psaFile, int dataSectionLocation, int miscSectionLocation)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            MiscSectionLocation = miscSectionLocation;
        }

        public MiscSection1 GetMiscSection1()
        {
            MiscSection1 miscSection1 = new MiscSection1();

            // checks if there is a misc section 1
            if (PsaFile.FileContent[MiscSectionLocation] >= 8096 && PsaFile.FileContent[MiscSectionLocation] < PsaFile.DataSectionSize)
            {
                Console.WriteLine("Misc Section 1 exists");
            }
            else
            {
                Console.WriteLine("Misc Section 1 does NOT exist");
            }

            return miscSection1;
        }

        public FinalSmashAura GetFinalSmashAura()
        {
            FinalSmashAura finalSmashAura = new FinalSmashAura();
            // counts final smash aura entries
            if (PsaFile.FileContent[MiscSectionLocation + 1] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 1] < PsaFile.DataSectionSize)
            {
                int numberOfFinalSmashAuraEntries = PsaFile.FileContent[MiscSectionLocation + 2];
                if (numberOfFinalSmashAuraEntries > 0 && numberOfFinalSmashAuraEntries < 256)
                {
                    // Idk where this is used...
                    // int n = PsaFile.FileContent[miscSectionLocation + 1] / 4;

                    finalSmashAura.FinalSmashAuraEntryCount = numberOfFinalSmashAuraEntries;
                }
            }
            Console.WriteLine(String.Format("Number Of Final Smash Aura Entries: {0}", finalSmashAura.FinalSmashAuraEntryCount));
            return finalSmashAura;
        }

        public HurtBoxes GetHurtBoxes()
        {
            HurtBoxes hurtBoxes = new HurtBoxes();
            // counts hurtboxes entries
            if (PsaFile.FileContent[MiscSectionLocation + 3] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 3] < PsaFile.DataSectionSize)
            {
                int numberOfHurtBoxEntries = PsaFile.FileContent[MiscSectionLocation + 4];
                if (numberOfHurtBoxEntries > 0 && numberOfHurtBoxEntries < 256)
                {

                    // Idk where this is used...
                    //int n = PsaFile.FileContent[miscSectionLocation + 3] / 4 + 7;

                    hurtBoxes.HurtBoxEntryCount = numberOfHurtBoxEntries;
                }
            }
            Console.WriteLine(String.Format("Number Of Hurt Box Entries: {0}", hurtBoxes.HurtBoxEntryCount));
            return hurtBoxes;
        }

        public LedgeGrab GetLedgeGrab()
        {
            LedgeGrab ledgeGrab = new LedgeGrab();
            // counts ledge grab
            if (PsaFile.FileContent[MiscSectionLocation + 5] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 5] < PsaFile.DataSectionSize)
            {
                int numberOfLedgeGrabEntries = PsaFile.FileContent[MiscSectionLocation + 6];
                if (numberOfLedgeGrabEntries > 0 && numberOfLedgeGrabEntries < 256)
                {
                    ledgeGrab.LedgeGrabeEntriesCount = numberOfLedgeGrabEntries;
                }
            }
            Console.WriteLine(String.Format("Number Of Ledge Grab Entries: {0}", ledgeGrab.LedgeGrabeEntriesCount));
            return ledgeGrab;
        }

        public MiscSection2 GetMiscSection2()
        {
            MiscSection2 miscSection2 = new MiscSection2();
            // counts misc section 2 entries
            if (PsaFile.FileContent[MiscSectionLocation + 7] >= 8096 && PsaFile.FileContent[MiscSectionLocation + 7] < PsaFile.DataSectionSize)
            {
                int numberOfMiscSection2Entries = PsaFile.FileContent[MiscSectionLocation + 8];
                if (numberOfMiscSection2Entries > 0 && numberOfMiscSection2Entries < 256)
                {
                    miscSection2.EntriesCount = numberOfMiscSection2Entries;
                }
            }
            Console.WriteLine(String.Format("Number Of Misc Section 2 Entries: {0}", miscSection2.EntriesCount));
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
