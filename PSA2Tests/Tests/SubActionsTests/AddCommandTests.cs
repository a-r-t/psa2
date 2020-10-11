using NUnit.Framework;
using PSA2.src.FileProcessor.MovesetHandler;
using System;

namespace PSA2Tests.Tests.SubActionsTests
{
    [TestFixture]
    public class AddCommandTests
    {
        [Test]
        [Description("Add command which will relocate code block")]
        [TestCase(72, 0, "FitMarioOneCommandAdded.pac")]
        [TestCase(78, 1, "FitMarioOneCommandAdded2.pac")]
        [TestCase(1, 2, "FitMarioOneCommandAdded3.pac")]
        [TestCase(12, 3, "FitMarioOneCommandAdded4.pac")]
        public void AddOneCommandToSubActionWithExistingCommands(int subActionId, int codeBlockId, string comparisonFileName)
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.AddCommand(subActionId, codeBlockId);
            psaMovesetParser.PsaFile.SaveFile($"./Tests/SubActionsTests/Out/Add/{comparisonFileName}");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./Tests/SubActionsTests/ComparisonData/Add/{comparisonFileName}", $"./Tests/SubActionsTests/Out/Add/{comparisonFileName}"));
        }

        /*
        [Test]
        public void AddTwoCommandsToActionWithExistingCommands()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.AddCommandToAction(0, 0);
            psaMovesetParser.ActionsHandler.AddCommandToAction(0, 0);
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Add/FitMarioTwoCommandsAdded.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Add/FitMarioTwoCommandsAdded.pac", "./Tests/ActionsTests/Out/Add/FitMarioTwoCommandsAdded.pac"));
        }

        [Test]
        [Description("Add command which will create code block")]
        [TestCase(0, 1, "FitMarioOneCommandAddedWithNoExistingCommands.pac")]
        [TestCase(1, 1, "FitMarioOneCommandAddedWithNoExistingCommands2.pac")]
        public void AddOneCommandToActionWithNoExistingCommands(int actionId, int codeBlockId, string comparisonFileName)
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.AddCommandToAction(actionId, codeBlockId);
            psaMovesetParser.PsaFile.SaveFile($"./Tests/ActionsTests/Out/Add/{comparisonFileName}");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./Tests/ActionsTests/ComparisonData/Add/{comparisonFileName}", $"./Tests/ActionsTests/Out/Add/{comparisonFileName}"));
        }


        [Test]
        public void AddCommandsToActionWithFreeSpaceInMiddleOfDataSection()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.RemoveCommandFromAction(6, 1, 0);
            psaMovesetParser.ActionsHandler.RemoveCommandFromAction(6, 1, 0);
            psaMovesetParser.ActionsHandler.RemoveCommandFromAction(6, 1, 0);
            psaMovesetParser.ActionsHandler.AddCommandToAction(6, 1);
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Add/FitMarioOneCommandAddedWithFreeSpaceInMiddleOfDataSection.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Add/FitMarioOneCommandAddedWithFreeSpaceInMiddleOfDataSection.pac", "./Tests/ActionsTests/Out/Add/FitMarioOneCommandAddedWithFreeSpaceInMiddleOfDataSection.pac"));
        }
        */
    }
}
