using NUnit.Framework;
using PSA2.src.FileProcessor.MovesetHandler;
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using System;
using System.Collections.Generic;

namespace PSA2Tests.Tests.SubActionsTests
{
    [TestFixture]
    public class InsertCommandTests
    {
        [Test]
        [Description("Insert command which will relocate code block")]
        [TestCase(72, 0, 0, "FitMarioOneCommandInserted.pac")]
        [TestCase(72, 0, 5, "FitMarioOneCommandInserted2.pac")]
        public void InsertOneCommandInActionWithExistingCommands(int subActionId, int codeBlockId, int commandIndex, string comparisonFileName)
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");

            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(6, 0)
            };
            PsaCommand psaCommand = new PsaCommand(33620480, 0, parameters);

            psaMovesetParser.SubActionsHandler.InsertCommand(subActionId, codeBlockId, commandIndex, psaCommand);
            psaMovesetParser.PsaFile.SaveFile($"./Tests/SubActionsTests/Out/Insert/{comparisonFileName}");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./Tests/SubActionsTests/ComparisonData/Insert/{comparisonFileName}", $"./Tests/SubActionsTests/Out/Insert/{comparisonFileName}"));
        }

        [Test]
        [Description("Insert command to empty code block")]
        public void InsertOneCommandInActionWithNoExistingCommands()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");

            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(6, 0)
            };
            PsaCommand psaCommand = new PsaCommand(33620480, 0, parameters);

            psaMovesetParser.SubActionsHandler.InsertCommand(0, 0, 0, psaCommand);
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Insert/FitMarioOneCommandInsertedToEmptyCodeBlock.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Insert/FitMarioOneCommandInsertedToEmptyCodeBlock.pac", "./Tests/SubActionsTests/Out/Insert/FitMarioOneCommandInsertedToEmptyCodeBlock.pac"));
        }
    }
}
