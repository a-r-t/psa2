using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSA2.src.FileProcessor.MovesetParser;
using PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.CommandParserHelpers;

namespace PSA2Tests.WriteTests
{
    [TestClass]
    public class ModifyCommandInActionTests
    {
        [TestMethod]
        public void ModifyCommandInActionWithIdenticalCommand()
        {
            PsaMovesetParser psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(6, 1)
            };
            psaMovesetParser.ActionsParser.ModifyActionCommand(0, 0, 0, new PsaCommand(33620480, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioModifyCommandWithIdenticalCommand.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Modify/FitMarioModifyCommandWithIdenticalCommand.pac", "./WriteTests/Out/FitMarioModifyCommandWithIdenticalCommand.pac"));
        }

        [TestMethod]
        public void ModifyCommandInActionWithIdenticalCommandWithDifferentParameterValues()
        {
            PsaMovesetParser psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 1),
                new PsaCommandParameter(6, 2)
            };
            psaMovesetParser.ActionsParser.ModifyActionCommand(0, 0, 0, new PsaCommand(33620480, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterValues.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterValues.pac", "./WriteTests/Out/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterValues.pac"));
        }

        [TestMethod]
        public void ModifyCommandInActionWithIdenticalCommandWithDifferentParameterTypes()
        {
            PsaMovesetParser psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(1, 0),
                new PsaCommandParameter(0, 1)
            };
            psaMovesetParser.ActionsParser.ModifyActionCommand(0, 0, 0, new PsaCommand(33620480, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterTypes.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterTypes.pac", "./WriteTests/Out/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterTypes.pac"));
        }

        [TestMethod]
        public void ModifyCommandInActionWithDifferentCommandWithIdenticalParameterCount()
        {
            PsaMovesetParser psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(3, 0)
            };
            psaMovesetParser.ActionsParser.ModifyActionCommand(0, 0, 0, new PsaCommand(67109376, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioModifyCommandWithDifferentCommandWithIdenticalParameterCount.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Modify/FitMarioModifyCommandWithDifferentCommandWithIdenticalParameterCount.pac", "./WriteTests/Out/FitMarioModifyCommandWithDifferentCommandWithIdenticalParameterCount.pac"));
        }
    }
}
