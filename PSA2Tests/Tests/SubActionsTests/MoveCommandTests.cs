using NUnit.Framework;
using PSA2.src.FileProcessor.MovesetHandler;
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using System;

namespace PSA2Tests.Tests.SubActionsTests
{
    [TestFixture]
    public class MoveCommandTests
    {
        [Test]
        [Description("Move Command Upwards: Swap commands that both have parameters")]
        public void MoveCommandUpwards()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.MoveCommandUp(72, 0, 1);
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Move/FitMarioMoveCommandUpwards-BothCommandsHaveParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Move/FitMarioMoveCommandUpwards-BothCommandsHaveParameters.pac", "./Tests/SubActionsTests/Out/Move/FitMarioMoveCommandUpwards-BothCommandsHaveParameters.pac"));
        }

        [Test]
        [Description("Move Command Upwards: Swap command that has no parameters with a command that has parameters")]
        public void MoveCommandUpwardsThatHasNoParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.MoveCommandUp(72, 0, 5);
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Move/FitMarioMoveCommandUpwards-CommandWithNoParametersSwapWithCommandWithParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Move/FitMarioMoveCommandUpwards-CommandWithNoParametersSwapWithCommandWithParameters.pac", "./Tests/SubActionsTests/Out/Move/FitMarioMoveCommandUpwards-CommandWithNoParametersSwapWithCommandWithParameters.pac"));
        }

        [Test]
        [Description("Move Command Upwards: Swap command that has parameters with a command that has no parameters")]
        public void MoveCommandUpwardsThatHasParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.MoveCommandUp(72, 0, 6);
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Move/FitMarioMoveCommandUpwards-CommandWithParametersSwapWithCommandWithNoParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Move/FitMarioMoveCommandUpwards-CommandWithParametersSwapWithCommandWithNoParameters.pac", "./Tests/SubActionsTests/Out/Move/FitMarioMoveCommandUpwards-CommandWithParametersSwapWithCommandWithNoParameters.pac"));
        }

        [Test]
        [Description("Move Command Downwards: Swap commands that both have parameters")]
        public void MoveCommandDownwards()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.MoveCommandDown(72, 0, 0);
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Move/FitMarioMoveCommandDownwards-BothCommandsHaveParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Move/FitMarioMoveCommandDownwards-BothCommandsHaveParameters.pac", "./Tests/SubActionsTests/Out/Move/FitMarioMoveCommandDownwards-BothCommandsHaveParameters.pac"));
        }

        [Test]
        [Description("Move Command Downwards: Swap command that has no parameters with a command that has parameters")]
        public void MoveCommandDownwardsThatHasNoParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.MoveCommandDown(72, 0, 5);
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Move/FitMarioMoveCommandDownwards-CommandWithNoParametersSwapWithCommandWithParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Move/FitMarioMoveCommandDownwards-CommandWithNoParametersSwapWithCommandWithParameters.pac", "./Tests/SubActionsTests/Out/Move/FitMarioMoveCommandDownwards-CommandWithNoParametersSwapWithCommandWithParameters.pac"));
        }

        [Test]
        [Description("Move Command Downwards: Swap command that has parameters with a command that has no parameters")]
        public void MoveCommandDownwardsThatHasParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.MoveCommandDown(72, 0, 4);
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Move/FitMarioMoveCommandDownwards-CommandWithParametersSwapWithCommandWithNoParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Move/FitMarioMoveCommandDownwards-CommandWithParametersSwapWithCommandWithNoParameters.pac", "./Tests/SubActionsTests/Out/Move/FitMarioMoveCommandDownwards-CommandWithParametersSwapWithCommandWithNoParameters.pac"));
        }
    }
}
