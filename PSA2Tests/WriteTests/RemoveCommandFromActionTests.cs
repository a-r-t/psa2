using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSA2.src.FileProcessor.MovesetParser;
using System;

namespace PSA2Tests.WriteTests
{
    [TestClass]
    public class RemoveCommandFromActionTests
    {
        [TestMethod]
        public void RemoveOneCommandFromAction()
        {
            PsaMovesetParser psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.RemoveCommandFromAction(0, 0, 0);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioRemoveOneCommandInAction.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Remove/FitMarioRemoveOneCommandInAction.pac", "./WriteTests/Out/FitMarioRemoveOneCommandInAction.pac"));
        }

        [TestMethod]
        public void RemoveAllCommandsFromAction()
        {
            PsaMovesetParser psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.RemoveCommandFromAction(6, 1, 0);
            psaMovesetParser.ActionsParser.RemoveCommandFromAction(6, 1, 0);
            psaMovesetParser.ActionsParser.RemoveCommandFromAction(6, 1, 0);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioRemoveAllCommandsInAction.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Remove/FitMarioRemoveAllCommandsInAction.pac", "./WriteTests/Out/FitMarioRemoveAllCommandsInAction.pac"));
        }

        [TestMethod]
        public void RemoveCommandWithPointerFromAction()
        {
            PsaMovesetParser psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.RemoveCommandFromAction(2, 0, 11);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioRemoveCommandWithPointerInAction.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Remove/FitMarioRemoveCommandWithPointerInAction.pac", "./WriteTests/Out/FitMarioRemoveCommandWithPointerInAction.pac"));
        }
    }
}
