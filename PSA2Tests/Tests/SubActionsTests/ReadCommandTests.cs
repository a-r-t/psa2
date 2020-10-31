using NUnit.Framework;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSA2Tests.Tests.SubActionsTests
{
    [TestFixture]
    public class ReadCommandTests
    {
        [Test]
        [Description("Read psa commands from action code block")]
        public void ReadPsaCommandsFromSubAction()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommand> psaCommands = psaMovesetParser.SubActionsHandler.GetPsaCommandsForSubAction(72, 0);
            Assert.AreEqual(12, psaCommands.Count);
            Assert.AreEqual(131328, psaCommands[0].Instruction);
            Assert.AreEqual(1, psaCommands[0].Parameters.Count);
            Assert.AreEqual(1, psaCommands[0].Parameters[0].Type);
            Assert.AreEqual(60000, psaCommands[0].Parameters[0].Value);
        }

        [Test]
        [Description("Read psa commands from action code block")]
        public void ReadPsaCommandsFromSubAction2()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommand> psaCommands = psaMovesetParser.SubActionsHandler.GetPsaCommandsForSubAction(73, 2);
            Assert.AreEqual(2, psaCommands.Count);
            Assert.AreEqual(131328, psaCommands[0].Instruction);
            Assert.AreEqual(1, psaCommands[0].Parameters.Count);
            Assert.AreEqual(1, psaCommands[0].Parameters[0].Type);
            Assert.AreEqual(60000, psaCommands[0].Parameters[0].Value);
        }
    }
}
