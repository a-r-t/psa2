using NUnit.Framework;
using PSA2.src.FileProcessor.MovesetHandler;
using System;

namespace PSA2Tests.Tests.ActionsTests
{
    [TestFixture]
    public class RemoveCommandFromActionTests
    {
        [Test]
        [TestCase(0, 0, 0, "FitMarioRemoveOneCommandInAction.pac")]
        [TestCase(0, 0, 1, "FitMarioRemoveOneCommandInAction2.pac")]
        public void RemoveOneCommandFromAction(int actionId, int codeBlockId, int commandIndex, string comparisonFileName)
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.RemoveCommandFromAction(actionId, codeBlockId, commandIndex);
            psaMovesetParser.PsaFile.SaveFile($"./Tests/ActionsTests/Out/Remove/{comparisonFileName}");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./Tests/ActionsTests/ComparisonData/Remove/{comparisonFileName}", $"./Tests/ActionsTests/Out/Remove/{comparisonFileName}"));
        }

        [Test]
        public void RemoveAllCommandsFromAction()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.RemoveCommandFromAction(6, 1, 0);
            psaMovesetParser.ActionsHandler.RemoveCommandFromAction(6, 1, 0);
            psaMovesetParser.ActionsHandler.RemoveCommandFromAction(6, 1, 0);
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Remove/FitMarioRemoveAllCommandsInAction.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Remove/FitMarioRemoveAllCommandsInAction.pac", "./Tests/ActionsTests/Out/Remove/FitMarioRemoveAllCommandsInAction.pac"));
        }

        [Test]
        public void RemoveCommandWithPointerFromAction()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.RemoveCommandFromAction(2, 0, 11);
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Remove/FitMarioRemoveCommandWithPointerInAction.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Remove/FitMarioRemoveCommandWithPointerInAction.pac", "./Tests/ActionsTests/Out/Remove/FitMarioRemoveCommandWithPointerInAction.pac"));
        }

        [Test]
        public void RemoveCommandAtEndOfDataSection()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.AddCommandToAction(6, 1);
            psaMovesetParser.ActionsHandler.RemoveCommandFromAction(6, 1, 3);
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Remove/FitMarioRemoveCommandAtEndOfDataSection.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Remove/FitMarioRemoveCommandAtEndOfDataSection.pac", "./Tests/ActionsTests/Out/Remove/FitMarioRemoveCommandAtEndOfDataSection.pac"));
        }
    }
}
