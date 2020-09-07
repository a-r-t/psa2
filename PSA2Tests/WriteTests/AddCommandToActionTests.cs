using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSA2.src.FileProcessor.MovesetParser;
using System;

namespace PSA2Tests.WriteTests
{
    [TestClass]
    public class AddCommandToActionTests
    {
        [TestMethod]
        public void AddOneCommandToActionWithExistingCommands()
        {
            PsaMovesetParser psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.AddCommandToAction(0, 0);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioOneCommandAdded.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/FitMarioOneCommandAdded.pac", "./WriteTests/Out/FitMarioOneCommandAdded.pac"));
        }

        [TestMethod]
        public void AddTwoCommandsToActionWithExistingCommands()
        {
            PsaMovesetParser psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.AddCommandToAction(0, 0);
            psaMovesetParser.ActionsParser.AddCommandToAction(0, 0);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioTwoCommandsAdded.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/FitMarioTwoCommandsAdded.pac", "./WriteTests/Out/FitMarioTwoCommandsAdded.pac"));
        }

        [TestMethod]
        public void AddOneCommandToActionWithNoExistingCommands()
        {
            PsaMovesetParser psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.AddCommandToAction(0, 1);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioOneCommandAddedWithNoExistingCommands.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/FitMarioOneCommandAddedWithNoExistingCommands.pac", "./WriteTests/Out/FitMarioOneCommandAddedWithNoExistingCommands.pac"));
        }
    }
}
