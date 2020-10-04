using NUnit.Framework;
using PSA2.src.FileProcessor.MovesetHandler;
using System;

namespace PSA2Tests.WriteTests
{
    [TestFixture]
    public class RemoveCommandFromActionTests
    {
        [Test]
        [TestCase(0, 0, 0, "FitMarioRemoveOneCommandInAction.pac")]
        [TestCase(0, 0, 1, "FitMarioRemoveOneCommandInAction2.pac")]
        public void RemoveOneCommandFromAction(int actionId, int codeBlockId, int commandIndex, string comparisonFileName)
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.ActionsParser.RemoveCommandFromAction(actionId, codeBlockId, commandIndex);
            psaMovesetParser.PsaFile.SaveFile($"./WriteTests/Out/Actions/Remove/{comparisonFileName}");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./WriteTests/ComparisonData/Actions/Remove/{comparisonFileName}", $"./WriteTests/Out/Actions/Remove/{comparisonFileName}"));
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
    }
}
