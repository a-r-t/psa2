using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSA2.src.FileProcessor.MovesetHandler;
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using System;

namespace PSA2Tests.WriteTests
{
    [TestClass]
    public class MoveCommandInActionTests
    {
        [TestMethod]
        [Description("Move Command Upwards: Swap commands that both have parameters")]
        public void MoveCommandUpwards()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.MoveActionCommand(0, 0, 1, MoveDirection.UP);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioMoveCommandUpwards-BothCommandsHaveParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Move/FitMarioMoveCommandUpwards-BothCommandsHaveParameters.pac", "./WriteTests/Out/FitMarioMoveCommandUpwards-BothCommandsHaveParameters.pac"));
        }

        [TestMethod]
        [Description("Move Command Upwards: Swap command that has no parameters with a command that has parameters")]
        public void MoveCommandUpwardsThatHasNoParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.MoveActionCommand(0, 0, 10, MoveDirection.UP);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioMoveCommandUpwards-CommandWithNoParametersSwapWithCommandWithParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Move/FitMarioMoveCommandUpwards-CommandWithNoParametersSwapWithCommandWithParameters.pac", "./WriteTests/Out/FitMarioMoveCommandUpwards-CommandWithNoParametersSwapWithCommandWithParameters.pac"));
        }

        [TestMethod]
        [Description("Move Command Upwards: Swap command that has parameters with a command that has no parameters")]
        public void MoveCommandUpwardsThatHasParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.MoveActionCommand(0, 0, 11, MoveDirection.UP);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioMoveCommandUpwards-CommandWithParametersSwapWithCommandWithNoParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Move/FitMarioMoveCommandUpwards-CommandWithParametersSwapWithCommandWithNoParameters.pac", "./WriteTests/Out/FitMarioMoveCommandUpwards-CommandWithParametersSwapWithCommandWithNoParameters.pac"));
        }

        [TestMethod]
        [Description("Move Command Downwards: Swap commands that both have parameters")]
        public void MoveCommandDownwards()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.MoveActionCommand(0, 0, 0, MoveDirection.DOWN);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioMoveCommandDownwards-BothCommandsHaveParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Move/FitMarioMoveCommandDownwards-BothCommandsHaveParameters.pac", "./WriteTests/Out/FitMarioMoveCommandDownwards-BothCommandsHaveParameters.pac"));
        }

        [TestMethod]
        [Description("Move Command Downwards: Swap command that has no parameters with a command that has parameters")]
        public void MoveCommandDownwardsThatHasNoParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.MoveActionCommand(0, 0, 10, MoveDirection.DOWN);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioMoveCommandDownwards-CommandWithNoParametersSwapWithCommandWithParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Move/FitMarioMoveCommandDownwards-CommandWithNoParametersSwapWithCommandWithParameters.pac", "./WriteTests/Out/FitMarioMoveCommandDownwards-CommandWithNoParametersSwapWithCommandWithParameters.pac"));
        }

        [TestMethod]
        [Description("Move Command Downwards: Swap command that has parameters with a command that has no parameters")]
        public void MoveCommandDownwardsThatHasParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.MoveActionCommand(0, 0, 9, MoveDirection.DOWN);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioMoveCommandDownwards-CommandWithParametersSwapWithCommandWithNoParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Move/FitMarioMoveCommandDownwards-CommandWithParametersSwapWithCommandWithNoParameters.pac", "./WriteTests/Out/FitMarioMoveCommandDownwards-CommandWithParametersSwapWithCommandWithNoParameters.pac"));
        }
    }
}
