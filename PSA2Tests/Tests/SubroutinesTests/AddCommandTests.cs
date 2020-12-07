using NUnit.Framework;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSA2Tests.Tests.SubroutineTests
{
    [TestFixture]
    public class AddCommandTests
    {
        [Test]
        [Description("Add command which will relocate subroutine")]
        [TestCase(99824, 125540, "FitMarioOneCommandAdded.pac")]
        [TestCase(103064, 125540, "FitMarioOneCommandAdded2.pac")]
        public void AddCommandToSubroutine(int subroutineLocation, int relocatedCommandsPointerLocation, string comparisonFileName)
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            int commandsPointerLocation = psaMovesetParser.SubRoutinesHandler.AddCommand(subroutineLocation);
            Assert.IsTrue(commandsPointerLocation == relocatedCommandsPointerLocation);
            psaMovesetParser.PsaFile.SaveFile($"./Tests/SubroutinesTests/Out/Add/{comparisonFileName}");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./Tests/SubroutinesTests/ComparisonData/Add/{comparisonFileName}", $"./Tests/SubroutinesTests/Out/Add/{comparisonFileName}"));
        }
    }
}
