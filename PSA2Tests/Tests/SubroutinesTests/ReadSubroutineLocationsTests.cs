using NUnit.Framework;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSA2Tests.Tests.SubroutineTests
{
    [TestFixture]
    public class ReadSubroutineLocationsTests
    {
        [Test]
        [Description("Gets all valid subroutines found in moveset")]
        public void GetAllSubroutines()
        {
            // TODO: This test will need to be updated in the future once articles/action overrides are added to moveset logic
            // TODO: This test includes external subroutines -- in the future this will go somewhere else
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<int> subroutineLocations = psaMovesetParser.SubRoutinesHandler.GetAllSubroutineLocations();
            List<int> orderedSubroutineLocations = subroutineLocations.OrderBy(x => x.ToString("X8")).ToList();
            Assert.IsTrue(orderedSubroutineLocations[0].ToString("X") == "6188");
            Assert.IsTrue(orderedSubroutineLocations[1].ToString("X") == "CCB0");
            Assert.IsTrue(orderedSubroutineLocations[2].ToString("X") == "105F8");
            Assert.IsTrue(orderedSubroutineLocations[3].ToString("X") == "16AF8");
            Assert.IsTrue(orderedSubroutineLocations[4].ToString("X") == "16B50");
            Assert.IsTrue(orderedSubroutineLocations[5].ToString("X") == "16C58");
            Assert.IsTrue(orderedSubroutineLocations[6].ToString("X") == "16C80");
            Assert.IsTrue(orderedSubroutineLocations[7].ToString("X") == "16D78");
            Assert.IsTrue(orderedSubroutineLocations[8].ToString("X") == "185F0");
        }
    }
}
