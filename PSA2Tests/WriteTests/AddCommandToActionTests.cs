using NUnit.Framework;
using PSA2.src.FileProcessor.MovesetHandler;
using System;

namespace PSA2Tests.WriteTests
{
    [TestFixture]
    public class AddCommandToActionTests
    {
        [Test]
        [Description("Add command which will relocate code block")]
        [TestCase(0, 0, "FitMarioOneCommandAdded.pac")]
        [TestCase(6, 1, "FitMarioOneCommandAdded2.pac")]
        public void AddOneCommandToActionWithExistingCommands(int actionId, int codeBlockId, string comparisonFileName)
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.AddCommandToAction(actionId, codeBlockId);
            psaMovesetParser.PsaFile.SaveFile($"./WriteTests/Out/Actions/Add/{comparisonFileName}");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./WriteTests/ComparisonData/Actions/Add/{comparisonFileName}", $"./WriteTests/Out/Actions/Add/{comparisonFileName}"));
        }

        [Test]
        public void AddTwoCommandsToActionWithExistingCommands()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.AddCommandToAction(0, 0);
            psaMovesetParser.ActionsHandler.AddCommandToAction(0, 0);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Add/FitMarioTwoCommandsAdded.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Add/FitMarioTwoCommandsAdded.pac", "./WriteTests/Out/Actions/Add/FitMarioTwoCommandsAdded.pac"));
        }

        [Test]
        [Description("Add command which will create code block")]
        [TestCase(0, 1, "FitMarioOneCommandAddedWithNoExistingCommands.pac")]
        [TestCase(1, 1, "FitMarioOneCommandAddedWithNoExistingCommands2.pac")]
        public void AddOneCommandToActionWithNoExistingCommands(int actionId, int codeBlockId, string comparisonFileName)
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.AddCommandToAction(actionId, codeBlockId);
            psaMovesetParser.PsaFile.SaveFile($"./WriteTests/Out/Actions/Add/{comparisonFileName}");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./WriteTests/ComparisonData/Actions/Add/{comparisonFileName}", $"./WriteTests/Out/Actions/Add/{comparisonFileName}"));
        }


        [Test]
        public void AddCommandsToActionWithFreeSpaceInMiddleOfDataSection()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.RemoveCommandFromAction(6, 1, 0);
            psaMovesetParser.ActionsHandler.RemoveCommandFromAction(6, 1, 0);
            psaMovesetParser.ActionsHandler.RemoveCommandFromAction(6, 1, 0);
            psaMovesetParser.ActionsHandler.AddCommandToAction(6, 1);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Add/FitMarioOneCommandAddedWithFreeSpaceInMiddleOfDataSection.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Add/FitMarioOneCommandAddedWithFreeSpaceInMiddleOfDataSection.pac", "./WriteTests/Out/Actions/Add/FitMarioOneCommandAddedWithFreeSpaceInMiddleOfDataSection.pac"));
        }
    }
}
