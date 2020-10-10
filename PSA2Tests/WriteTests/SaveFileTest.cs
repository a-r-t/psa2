using NUnit.Framework;
using PSA2.src.FileProcessor.MovesetHandler;
using System;

namespace PSA2Tests.WriteTests
{
    [TestFixture]
    public class SaveFileTest
    {
        [Test]
        public void SaveFile()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/NoChanges/FitMario.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/Data/FitMario.pac", "./WriteTests/Out/NoChanges/FitMario.pac"));
        }
    }
}
