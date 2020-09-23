using NUnit.Framework;
using PSA2.src.FileProcessor.MovesetHandler;
using System;

namespace PSA2Tests.WriteTests
{
    [TestFixture]
    public class AddCommandToActionTests
    {
        [Test]
        public void AddOneCommandToActionWithExistingCommands()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.AddCommandToAction(0, 0);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioOneCommandAdded.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Add/FitMarioOneCommandAdded.pac", "./WriteTests/Out/FitMarioOneCommandAdded.pac"));
        }

        [Test]
        public void AddTwoCommandsToActionWithExistingCommands()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.AddCommandToAction(0, 0);
            psaMovesetParser.ActionsParser.AddCommandToAction(0, 0);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioTwoCommandsAdded.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Add/FitMarioTwoCommandsAdded.pac", "./WriteTests/Out/FitMarioTwoCommandsAdded.pac"));
        }

        [Test]
        [Description("Add command which will create code block")]
        [TestCase(0, 1, "FitMarioOneCommandAddedWithNoExistingCommands.pac")]
        [TestCase(1, 1, "FitMarioOneCommandAddedWithNoExistingCommands2.pac")]
        public void AddOneCommandToActionWithNoExistingCommands(int actionId, int codeBlockId, string comparisonFileName)
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.AddCommandToAction(actionId, codeBlockId);
            psaMovesetParser.PsaFile.SaveFile($"./WriteTests/Out/{comparisonFileName}");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./WriteTests/ComparisonData/Actions/Add/{comparisonFileName}", $"./WriteTests/Out/{comparisonFileName}"));
        }
    }
}
