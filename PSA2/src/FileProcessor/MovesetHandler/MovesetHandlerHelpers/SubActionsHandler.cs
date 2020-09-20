using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2.src.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    public class SubActionsHandler
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public PsaCommandHandler PsaCommandHandler { get; private set; }

        public SubActionsHandler(PsaFile psaFile, int dataSectionLocation, PsaCommandHandler psaCommandHandler)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            PsaCommandHandler = psaCommandHandler;
        }

        public int GetNumberOfSubActions()
        {
            Console.WriteLine(string.Format("Number of Sub Actions: {0}", (PsaFile.FileContent[DataSectionLocation + 13] - PsaFile.FileContent[DataSectionLocation + 12]) / 4));
            return (PsaFile.FileContent[DataSectionLocation + 13] - PsaFile.FileContent[DataSectionLocation + 12]) / 4;
        }

        // this is the offset where subaction code starts (displayed in PSAC)
        public int GetSubActionCodeBlockLocation(int subActionId, int codeBlockId)
        {
            int subActionsCodeBlockStartingLocation = PsaFile.FileContent[DataSectionLocation + 12 + codeBlockId] / 4; // n
            int subActionCodeBlockLocation = PsaFile.FileContent[subActionsCodeBlockStartingLocation + subActionId];

            return subActionCodeBlockLocation;
        }


        public List<PsaCommand> GetPsaCommandsForSubAction(int subActionId, int codeBlockId)
        {
            int subActionCodeBlockLocation = GetSubActionCodeBlockLocation(subActionId, codeBlockId); // i
            return PsaCommandHandler.GetPsaCommands(subActionCodeBlockLocation);
        }

        public string GetSubActionAnimationName(int subActionId)
        {
            // h = subActionId
            // not 100% sure why this chain works but it does
            int animationLocation = PsaFile.FileContent[DataSectionLocation] / 4 + 1 + subActionId * 2;
            if (PsaFile.FileContent[animationLocation] == 0)
            {
                return "NONE";
            }
            else
            {
                int animationNameLocation = PsaFile.FileContent[animationLocation] / 4; // j

                if (animationNameLocation < PsaFile.DataSectionSize) // and animationNameLocation >= stf whatever that means
                {
                    StringBuilder animationName = new StringBuilder();
                    int nameEndByteIndex = 0;
                    while (true) // originally i < 47 -- 48 char limit?
                    {
                        string nextStringData = Utils.ConvertDoubleWordToString(PsaFile.FileContent[animationNameLocation + nameEndByteIndex]);
                        animationName.Append(nextStringData);
                        if (nextStringData.Length == 4)
                        {
                            nameEndByteIndex++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    Console.WriteLine(animationName.ToString());
                    return animationName.ToString();
                }

                return "ERROR";
            }

        }

        public AnimationFlags GetSubActionAnimationFlags(int subActionId)
        {
            // will need to look at this later to figure out why it works
            int animationFlagsLocation = PsaFile.FileContent[DataSectionLocation] / 4 + subActionId * 2;
            int animationFlagsValue = PsaFile.FileContent[animationFlagsLocation];
            int inTransition = animationFlagsValue >> 24 & 0xFF;
            int noOutTransition = animationFlagsValue & 0x1;
            int loop = animationFlagsValue >> 16 & 0xFF & 0x2;
            int movesCharacter = animationFlagsValue >> 16 & 0xFF & 0x4;
            int unknown3 = animationFlagsValue >> 16 & 0xFF & 0x8;
            int unknown4 = animationFlagsValue >> 16 & 0xFF & 0x10;
            int unknown5 = animationFlagsValue >> 16 & 0xFF & 0x20;
            int transitionOutFromStart = animationFlagsValue >> 16 & 0xFF & 0x40;
            int unknown7 = animationFlagsValue >> 16 & 0xFF & 0x80;
            return new AnimationFlags(inTransition, noOutTransition, loop, movesCharacter, unknown3, unknown4, unknown5, transitionOutFromStart, unknown7);
        }
    }
}
