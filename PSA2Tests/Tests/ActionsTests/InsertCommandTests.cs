using NUnit.Framework;
using PSA2.src.FileProcessor.MovesetHandler;
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using System;
using System.Collections.Generic;

namespace PSA2Tests.Tests.ActionsTests
{
    [TestFixture]
    public class InsertCommandTests
    {
        [Test]
        [Description("Insert command which will relocate code block")]
        [TestCase(0, 0, 0, "FitMarioOneCommandInserted.pac")]
        [TestCase(0, 0, 5, "FitMarioOneCommandInserted2.pac")]
        public void InsertOneCommandInActionWithExistingCommands(int actionId, int codeBlockId, int commandIndex, string comparisonFileName)
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");

            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(6, 0)
            };
            PsaCommand psaCommand = new PsaCommand(33620480, 0, parameters);

            psaMovesetParser.ActionsHandler.InsertCommand(actionId, codeBlockId, commandIndex, psaCommand);
            psaMovesetParser.PsaFile.SaveFile($"./Tests/ActionsTests/Out/Insert/{comparisonFileName}");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./Tests/ActionsTests/ComparisonData/Insert/{comparisonFileName}", $"./Tests/ActionsTests/Out/Insert/{comparisonFileName}"));
        }

        /*
        [Test]
        [Description("Add command which will relocate code block")]
        [TestCase(0, 0, "FitMarioOneCommandAdded.pac")]
        [TestCase(6, 1, "FitMarioOneCommandAdded2.pac")]
        public void AddOneCommandToActionWithExistingCommands(int actionId, int codeBlockId, string comparisonFileName)
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.AddCommand(actionId, codeBlockId);
            psaMovesetParser.PsaFile.SaveFile($"./Tests/ActionsTests/Out/Add/{comparisonFileName}");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./Tests/ActionsTests/ComparisonData/Add/{comparisonFileName}", $"./Tests/ActionsTests/Out/Add/{comparisonFileName}"));
        }

        [Test]
        public void AddTwoCommandsToActionWithExistingCommands()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.AddCommand(0, 0);
            psaMovesetParser.ActionsHandler.AddCommand(0, 0);
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
            psaMovesetParser.ActionsHandler.AddCommand(actionId, codeBlockId);
            psaMovesetParser.PsaFile.SaveFile($"./Tests/ActionsTests/Out/Add/{comparisonFileName}");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./Tests/ActionsTests/ComparisonData/Add/{comparisonFileName}", $"./Tests/ActionsTests/Out/Add/{comparisonFileName}"));
        }


        [Test]
        public void AddCommandsToActionWithFreeSpaceInMiddleOfDataSection()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.RemoveCommand(6, 1, 0);
            psaMovesetParser.ActionsHandler.RemoveCommand(6, 1, 0);
            psaMovesetParser.ActionsHandler.RemoveCommand(6, 1, 0);
            psaMovesetParser.ActionsHandler.AddCommand(6, 1);
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Add/FitMarioOneCommandAddedWithFreeSpaceInMiddleOfDataSection.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Add/FitMarioOneCommandAddedWithFreeSpaceInMiddleOfDataSection.pac", "./Tests/ActionsTests/Out/Add/FitMarioOneCommandAddedWithFreeSpaceInMiddleOfDataSection.pac"));
        }
        */
    }
}
