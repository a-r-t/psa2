using NUnit.Framework;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;
using System;

namespace PSA2Tests.Tests.SubActionsTests
{
    [TestFixture]
    public class RemoveCommandTests
    {
        [Test]
        [TestCase(72, 0, 0, "FitMarioRemoveOneCommandInSubAction.pac")]
        [TestCase(72, 0, 1, "FitMarioRemoveOneCommandInSubAction2.pac")]
        public void RemoveOneCommandFromSubAction(int SubActionId, int codeBlockId, int commandIndex, string comparisonFileName)
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.RemoveCommand(SubActionId, codeBlockId, commandIndex);
            psaMovesetParser.PsaFile.SaveFile($"./Tests/SubActionsTests/Out/Remove/{comparisonFileName}");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./Tests/SubActionsTests/ComparisonData/Remove/{comparisonFileName}", $"./Tests/SubActionsTests/Out/Remove/{comparisonFileName}"));
        }

        [Test]
        public void RemoveAllCommandsFromSubAction()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            for (int i = 0; i < 5; i++)
            {
                psaMovesetParser.SubActionsHandler.RemoveCommand(70, 0, 0);
            }
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Remove/FitMarioRemoveAllCommandsInSubAction.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Remove/FitMarioRemoveAllCommandsInSubAction.pac", "./Tests/SubActionsTests/Out/Remove/FitMarioRemoveAllCommandsInSubAction.pac"));
        }

        [Test]
        public void RemoveCommandWithPointerFromSubAction()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.RemoveCommand(319, 1, 0);
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Remove/FitMarioRemoveCommandWithPointerInSubAction.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Remove/FitMarioRemoveCommandWithPointerInSubAction.pac", "./Tests/SubActionsTests/Out/Remove/FitMarioRemoveCommandWithPointerInSubAction.pac"));
        }

        [Test]
        public void RemoveCommandAtEndOfDataSection()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.AddCommand(72, 0);
            psaMovesetParser.SubActionsHandler.RemoveCommand(72, 0, 12);
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Remove/FitMarioRemoveCommandAtEndOfDataSection.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Remove/FitMarioRemoveCommandAtEndOfDataSection.pac", "./Tests/SubActionsTests/Out/Remove/FitMarioRemoveCommandAtEndOfDataSection.pac"));
        }
    }
}
