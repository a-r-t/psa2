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

        
        [Test]
        public void AddTwoCommandsToSubActionWithExistingCommands()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.AddCommand(72, 0);
            psaMovesetParser.SubActionsHandler.AddCommand(72, 0);
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Add/FitMarioTwoCommandsAdded.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Add/FitMarioTwoCommandsAdded.pac", "./Tests/SubActionsTests/Out/Add/FitMarioTwoCommandsAdded.pac"));
        }

        [Test]
        [Description("Add command which will create code block")]
        [TestCase(4, 0, "FitMarioOneCommandAddedWithNoExistingCommands.pac")]
        [TestCase(72, 1, "FitMarioOneCommandAddedWithNoExistingCommands2.pac")]
        public void AddOneCommandToSubActionWithNoExistingCommands(int subActionId, int codeBlockId, string comparisonFileName)
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.AddCommand(subActionId, codeBlockId);
            psaMovesetParser.PsaFile.SaveFile($"./Tests/SubActionsTests/Out/Add/{comparisonFileName}");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./Tests/SubActionsTests/ComparisonData/Add/{comparisonFileName}", $"./Tests/SubActionsTests/Out/Add/{comparisonFileName}"));
        }
        
        [Test]
        [Description("Add command to codeblock with no commands but still has an offset")]
        public void AddOneCommandToSubActionWithNoExistingCommandsWithExistingOffset()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.AddCommand(0, 0);
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Add/FitMarioOneCommandAddedWithNoExistingCommandsWithExistingOffset.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Add/FitMarioOneCommandAddedWithNoExistingCommandsWithExistingOffset.pac", "./Tests/SubActionsTests/Out/Add/FitMarioOneCommandAddedWithNoExistingCommandsWithExistingOffset.pac"));
        }

        [Test]
        public void AddCommandsToSubActionWithFreeSpaceInMiddleOfDataSection()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.RemoveCommand(19, 0, 0);
            psaMovesetParser.SubActionsHandler.AddCommand(19, 0);
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Add/FitMarioOneCommandAddedWithFreeSpaceInMiddleOfDataSection.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Add/FitMarioOneCommandAddedWithFreeSpaceInMiddleOfDataSection.pac", "./Tests/SubActionsTests/Out/Add/FitMarioOneCommandAddedWithFreeSpaceInMiddleOfDataSection.pac"));
        }
    }
}
