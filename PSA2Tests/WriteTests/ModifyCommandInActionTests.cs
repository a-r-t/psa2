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
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>();
            parameters.Add(new PsaCommandParameter(0, 0));
            parameters.Add(new PsaCommandParameter(6, 1));
            psaMovesetParser.ActionsParser.ModifyActionCommand(0, 0, 0, new PsaCommand(33620480, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioModifyCommandWithIdenticalCommand.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Modify/FitMarioModifyCommandWithIdenticalCommand.pac", "./WriteTests/Out/FitMarioModifyCommandWithIdenticalCommand.pac"));
        }

        [TestMethod]
        public void ModifyCommandInActionWithIdenticalCommandWithDifferentParameterValues()
        {
            PsaMovesetParser psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>();
            parameters.Add(new PsaCommandParameter(0, 1));
            parameters.Add(new PsaCommandParameter(6, 2));
            psaMovesetParser.ActionsParser.ModifyActionCommand(0, 0, 0, new PsaCommand(33620480, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterValues.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterValues.pac", "./WriteTests/Out/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterValues.pac"));
        }
    }
}
