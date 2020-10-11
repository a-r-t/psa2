using NUnit.Framework;
using PSA2.src.FileProcessor.MovesetHandler;
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSA2Tests.Tests.ActionsTests
{
    [TestFixture]
    public class ReadCommandTests
    {
        [Test]
        [Description("Read psa commands from action code block")]
        public void ReadPsaCommandsFromAction()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommand> psaCommands = psaMovesetParser.ActionsHandler.GetPsaCommandsForActionCodeBlock(0, 0);
            Assert.AreEqual(28, psaCommands.Count);
            Assert.AreEqual(33620480, psaCommands[0].Instruction);
            Assert.AreEqual(2, psaCommands[0].Parameters.Count);
            Assert.AreEqual(0, psaCommands[0].Parameters[0].Type);
            Assert.AreEqual(0, psaCommands[0].Parameters[0].Value);
        }

        [Test]
        [Description("Read psa commands from action code block")]
        public void ReadPsaCommandsFromAction2()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommand> psaCommands = psaMovesetParser.ActionsHandler.GetPsaCommandsForActionCodeBlock(1, 0);
            Assert.AreEqual(33, psaCommands.Count);
            Assert.AreEqual(33620480, psaCommands[0].Instruction);
            Assert.AreEqual(2, psaCommands[0].Parameters.Count);
            Assert.AreEqual(0, psaCommands[0].Parameters[0].Type);
            Assert.AreEqual(0, psaCommands[0].Parameters[0].Value);
        }

        [Test]
        [Description("Read psa commands from action code block")]
        public void ReadPsaCommandsFromAction3()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommand> psaCommands = psaMovesetParser.ActionsHandler.GetPsaCommandsForActionCodeBlock(3, 1);
            Assert.AreEqual(5, psaCommands.Count);
            Assert.AreEqual(656384, psaCommands[0].Instruction);
            Assert.AreEqual(4, psaCommands[0].Parameters.Count);
            Assert.AreEqual(6, psaCommands[0].Parameters[0].Type);
            Assert.AreEqual(7, psaCommands[0].Parameters[0].Value);
        }
    }
}
