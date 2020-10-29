using NUnit.Framework;
using PSA2.src.FileProcessor.MovesetHandler;
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using System;

namespace PSA2Tests.Tests.ActionsTests
{
    [TestFixture]
    public class MoveCommandTests
    {
        [Test]
        [Description("Move Command Upwards: Swap commands that both have parameters")]
        public void MoveCommandUpwards()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.MoveCommandUp(0, 0, 1);
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Move/FitMarioMoveCommandUpwards-BothCommandsHaveParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Move/FitMarioMoveCommandUpwards-BothCommandsHaveParameters.pac", "./Tests/ActionsTests/Out/Move/FitMarioMoveCommandUpwards-BothCommandsHaveParameters.pac"));
        }

        [Test]
        [Description("Move Command Upwards: Swap command that has no parameters with a command that has parameters")]
        public void MoveCommandUpwardsThatHasNoParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.MoveCommandUp(0, 0, 10);
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Move/FitMarioMoveCommandUpwards-CommandWithNoParametersSwapWithCommandWithParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Move/FitMarioMoveCommandUpwards-CommandWithNoParametersSwapWithCommandWithParameters.pac", "./Tests/ActionsTests/Out/Move/FitMarioMoveCommandUpwards-CommandWithNoParametersSwapWithCommandWithParameters.pac"));
        }

        [Test]
        [Description("Move Command Upwards: Swap command that has parameters with a command that has no parameters")]
        public void MoveCommandUpwardsThatHasParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.MoveCommandUp(0, 0, 11);
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Move/FitMarioMoveCommandUpwards-CommandWithParametersSwapWithCommandWithNoParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Move/FitMarioMoveCommandUpwards-CommandWithParametersSwapWithCommandWithNoParameters.pac", "./Tests/ActionsTests/Out/Move/FitMarioMoveCommandUpwards-CommandWithParametersSwapWithCommandWithNoParameters.pac"));
        }

        [Test]
        [Description("Move Command Downwards: Swap commands that both have parameters")]
        public void MoveCommandDownwards()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.MoveCommandDown(0, 0, 0);
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Move/FitMarioMoveCommandDownwards-BothCommandsHaveParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Move/FitMarioMoveCommandDownwards-BothCommandsHaveParameters.pac", "./Tests/ActionsTests/Out/Move/FitMarioMoveCommandDownwards-BothCommandsHaveParameters.pac"));
        }

        [Test]
        [Description("Move Command Downwards: Swap command that has no parameters with a command that has parameters")]
        public void MoveCommandDownwardsThatHasNoParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.MoveCommandDown(0, 0, 10);
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Move/FitMarioMoveCommandDownwards-CommandWithNoParametersSwapWithCommandWithParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Move/FitMarioMoveCommandDownwards-CommandWithNoParametersSwapWithCommandWithParameters.pac", "./Tests/ActionsTests/Out/Move/FitMarioMoveCommandDownwards-CommandWithNoParametersSwapWithCommandWithParameters.pac"));
        }

        [Test]
        [Description("Move Command Downwards: Swap command that has parameters with a command that has no parameters")]
        public void MoveCommandDownwardsThatHasParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.ActionsHandler.MoveCommandDown(0, 0, 9);
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Move/FitMarioMoveCommandDownwards-CommandWithParametersSwapWithCommandWithNoParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Move/FitMarioMoveCommandDownwards-CommandWithParametersSwapWithCommandWithNoParameters.pac", "./Tests/ActionsTests/Out/Move/FitMarioMoveCommandDownwards-CommandWithParametersSwapWithCommandWithNoParameters.pac"));
        }
    }
}
