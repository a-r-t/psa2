using NUnit.Framework;
using PSA2.src.FileProcessor.MovesetHandler;
using System;

namespace PSA2Tests.WriteTests
{
    [TestFixture]
    public class RemoveCommandFromActionTests
    {
        [Test]
        public void RemoveOneCommandFromAction()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.RemoveCommandFromAction(0, 0, 0);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Remove/FitMarioRemoveOneCommandInAction.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Remove/FitMarioRemoveOneCommandInAction.pac", "./WriteTests/Out/Actions/Remove/FitMarioRemoveOneCommandInAction.pac"));
        }

        [Test]
        public void RemoveAllCommandsFromAction()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.RemoveCommandFromAction(6, 1, 0);
            psaMovesetParser.ActionsParser.RemoveCommandFromAction(6, 1, 0);
            psaMovesetParser.ActionsParser.RemoveCommandFromAction(6, 1, 0);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Remove/FitMarioRemoveAllCommandsInAction.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Remove/FitMarioRemoveAllCommandsInAction.pac", "./WriteTests/Out/Actions/Remove/FitMarioRemoveAllCommandsInAction.pac"));
        }

        [Test]
        public void RemoveCommandWithPointerFromAction()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.RemoveCommandFromAction(2, 0, 11);
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Remove/FitMarioRemoveCommandWithPointerInAction.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Remove/FitMarioRemoveCommandWithPointerInAction.pac", "./WriteTests/Out/Actions/Remove/FitMarioRemoveCommandWithPointerInAction.pac"));
        }

        // remove last command that is a pointer
    }
}
