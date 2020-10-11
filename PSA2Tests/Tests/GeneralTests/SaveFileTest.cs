using NUnit.Framework;
using PSA2.src.FileProcessor.MovesetHandler;
using System;

namespace PSA2Tests.Tests
{
    [TestFixture]
    public class SaveFileTest
    {
        [Test]
        public void SaveFile()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.PsaFile.SaveFile("./Tests/GeneralTests/Out/SaveFile/FitMario.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/Data/FitMario.pac", "./Tests/GeneralTests/Out/SaveFile/FitMario.pac"));
        }
    }
}
