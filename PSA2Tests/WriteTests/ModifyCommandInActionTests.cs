using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using PSA2.src.FileProcessor.MovesetHandler;
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;

namespace PSA2Tests.WriteTests
{
    [TestFixture]
    public class ModifyCommandInActionTests
    {
        [Test]
        public void ModifyCommandInActionWithIdenticalCommand()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(6, 1)
            };
            psaMovesetParser.ActionsParser.ModifyActionCommand(0, 0, 0, new PsaCommand(33620480, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Modify/FitMarioModifyCommandWithIdenticalCommand.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Modify/FitMarioModifyCommandWithIdenticalCommand.pac", "./WriteTests/Out/Actions/Modify/FitMarioModifyCommandWithIdenticalCommand.pac"));
        }

        [Test]
        public void ModifyCommandInActionWithIdenticalCommandWithDifferentParameterValues()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 1),
                new PsaCommandParameter(6, 2)
            };
            psaMovesetParser.ActionsParser.ModifyActionCommand(0, 0, 0, new PsaCommand(33620480, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterValues.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterValues.pac", "./WriteTests/Out/Actions/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterValues.pac"));
        }

        [Test]
        public void ModifyCommandInActionWithIdenticalCommandWithDifferentParameterTypes()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(1, 0),
                new PsaCommandParameter(0, 1)
            };
            psaMovesetParser.ActionsParser.ModifyActionCommand(0, 0, 0, new PsaCommand(33620480, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterTypes.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterTypes.pac", "./WriteTests/Out/Actions/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterTypes.pac"));
        }

        [Test]
        public void ModifyCommandInActionWithDifferentCommandWithIdenticalParameterCount()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(3, 0)
            };
            psaMovesetParser.ActionsParser.ModifyActionCommand(0, 0, 0, new PsaCommand(67109376, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Modify/FitMarioModifyCommandWithDifferentCommandWithIdenticalParameterCount.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Modify/FitMarioModifyCommandWithDifferentCommandWithIdenticalParameterCount.pac", "./WriteTests/Out/Actions/Modify/FitMarioModifyCommandWithDifferentCommandWithIdenticalParameterCount.pac"));
        }

        [Test]
        public void ModifyCommandInActionWithDifferentCommandWithLargerParameterCount()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(6, 0),
                new PsaCommandParameter(5, 0)
            };
            psaMovesetParser.ActionsParser.ModifyActionCommand(0, 0, 0, new PsaCommand(33620736, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Modify/FitMarioModifyCommandWithDifferentCommandWithLargerParameterCount.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Modify/FitMarioModifyCommandWithDifferentCommandWithLargerParameterCount.pac", "./WriteTests/Out/Actions/Modify/FitMarioModifyCommandWithDifferentCommandWithLargerParameterCount.pac"));
        }

        [Test]
        public void ModifyCommandInActionWithDifferentCommandWithSmallerParameterCount()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0)
            };
            psaMovesetParser.ActionsParser.ModifyActionCommand(0, 0, 0, new PsaCommand(67109120, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Modify/FitMarioModifyCommandWithDifferentCommandWithSmallerParameterCount.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Modify/FitMarioModifyCommandWithDifferentCommandWithSmallerParameterCount.pac", "./WriteTests/Out/Actions/Modify/FitMarioModifyCommandWithDifferentCommandWithSmallerParameterCount.pac"));
        }

        [Test]
        public void ModifyCommandInActionWithCommandThatHasPointerParameter()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(2, 98464)
            };
            psaMovesetParser.ActionsParser.ModifyActionCommand(0, 0, 0, new PsaCommand(459008, 103152, parameters));
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Modify/FitMarioModifyCommandInActionWithCommandThatHasPointerParameter.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Modify/FitMarioModifyCommandInActionWithCommandThatHasPointerParameter.pac", "./WriteTests/Out/Actions/Modify/FitMarioModifyCommandInActionWithCommandThatHasPointerParameter.pac"));
        }

        [Test]
        public void ModifyCommandInActionWithNoParametersToCommandWithParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(6, 0)
            };
            psaMovesetParser.ActionsParser.ModifyActionCommand(0, 0, 10, new PsaCommand(33620480, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Modify/FitMarioModifyCommandWithNoParametersToCommandWithParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Modify/FitMarioModifyCommandWithNoParametersToCommandWithParameters.pac", "./WriteTests/Out/Actions/Modify/FitMarioModifyCommandWithNoParametersToCommandWithParameters.pac"));
        }

        [Test]
        public void ModifyCommandInActionWithParametersToCommandWithNoParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>();
            psaMovesetParser.ActionsParser.ModifyActionCommand(0, 0, 0, new PsaCommand(917504, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/Actions/Modify/FitMarioModifyCommandWithParametersToCommandWithNoParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Modify/FitMarioModifyCommandWithParametersToCommandWithNoParameters.pac", "./WriteTests/Out/Actions/Modify/FitMarioModifyCommandWithParametersToCommandWithNoParameters.pac"));
        }

        [Test]
        [TestCase(2, 0, 11, "FitMarioModifyCommandWithPointerParameter.pac")]
        [TestCase(5, 0, 16, "FitMarioModifyCommandWithPointerParameter2.pac")]
        public void ModifyCommandInActionWithPointerParameter(int actionId, int codeBlockId, int commandIndex, string comparisonFileName)
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(6, 0)
            };
            psaMovesetParser.ActionsParser.ModifyActionCommand(actionId, codeBlockId, commandIndex, new PsaCommand(33620480, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile($"./WriteTests/Out/Actions/Modify/{comparisonFileName}");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./WriteTests/ComparisonData/Actions/Modify/{comparisonFileName}", $"./WriteTests/Out/Actions/Modify/{comparisonFileName}"));
        }
    }
}
