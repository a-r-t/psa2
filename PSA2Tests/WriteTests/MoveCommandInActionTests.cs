using NUnit.Framework;
using PSA2.src.FileProcessor.MovesetHandler;
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using System;

namespace PSA2Tests.WriteTests
{
    [TestFixture]
    public class MoveCommandInActionTests
    {
        [Test]
        [Description("Move Command Upwards: Swap commands that both have parameters")]
        public void MoveCommandUpwards()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.MoveActionCommand(0, 0, 1, MoveDirection.UP);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Move/FitMarioMoveCommandUpwards-BothCommandsHaveParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Move/FitMarioMoveCommandUpwards-BothCommandsHaveParameters.pac", "./WriteTests/Out/Actions/Move/FitMarioMoveCommandUpwards-BothCommandsHaveParameters.pac"));
        }

        [Test]
        [Description("Move Command Upwards: Swap command that has no parameters with a command that has parameters")]
        public void MoveCommandUpwardsThatHasNoParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.MoveActionCommand(0, 0, 10, MoveDirection.UP);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Move/FitMarioMoveCommandUpwards-CommandWithNoParametersSwapWithCommandWithParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Move/FitMarioMoveCommandUpwards-CommandWithNoParametersSwapWithCommandWithParameters.pac", "./WriteTests/Out/Actions/Move/FitMarioMoveCommandUpwards-CommandWithNoParametersSwapWithCommandWithParameters.pac"));
        }

        [Test]
        [Description("Move Command Upwards: Swap command that has parameters with a command that has no parameters")]
        public void MoveCommandUpwardsThatHasParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.MoveActionCommand(0, 0, 11, MoveDirection.UP);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Move/FitMarioMoveCommandUpwards-CommandWithParametersSwapWithCommandWithNoParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Move/FitMarioMoveCommandUpwards-CommandWithParametersSwapWithCommandWithNoParameters.pac", "./WriteTests/Out/Actions/Move/FitMarioMoveCommandUpwards-CommandWithParametersSwapWithCommandWithNoParameters.pac"));
        }

        [Test]
        [Description("Move Command Downwards: Swap commands that both have parameters")]
        public void MoveCommandDownwards()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.MoveActionCommand(0, 0, 0, MoveDirection.DOWN);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Move/FitMarioMoveCommandDownwards-BothCommandsHaveParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Move/FitMarioMoveCommandDownwards-BothCommandsHaveParameters.pac", "./WriteTests/Out/Actions/Move/FitMarioMoveCommandDownwards-BothCommandsHaveParameters.pac"));
        }

        [Test]
        [Description("Move Command Downwards: Swap command that has no parameters with a command that has parameters")]
        public void MoveCommandDownwardsThatHasNoParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.MoveActionCommand(0, 0, 10, MoveDirection.DOWN);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Move/FitMarioMoveCommandDownwards-CommandWithNoParametersSwapWithCommandWithParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Move/FitMarioMoveCommandDownwards-CommandWithNoParametersSwapWithCommandWithParameters.pac", "./WriteTests/Out/Actions/Move/FitMarioMoveCommandDownwards-CommandWithNoParametersSwapWithCommandWithParameters.pac"));
        }

        [Test]
        [Description("Move Command Downwards: Swap command that has parameters with a command that has no parameters")]
        public void MoveCommandDownwardsThatHasParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.MoveActionCommand(0, 0, 9, MoveDirection.DOWN);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Move/FitMarioMoveCommandDownwards-CommandWithParametersSwapWithCommandWithNoParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Move/FitMarioMoveCommandDownwards-CommandWithParametersSwapWithCommandWithNoParameters.pac", "./WriteTests/Out/Actions/Move/FitMarioMoveCommandDownwards-CommandWithParametersSwapWithCommandWithNoParameters.pac"));
        }
    }
}
