using NUnit.Framework;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
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

            psaMovesetParser.ActionsHandler.InsertCommand(0, 1, 0, psaCommand);
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Insert/FitMarioOneCommandInsertedToEmptyCodeBlock.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Insert/FitMarioOneCommandInsertedToEmptyCodeBlock.pac", "./Tests/ActionsTests/Out/Insert/FitMarioOneCommandInsertedToEmptyCodeBlock.pac"));
        }
    }
}
