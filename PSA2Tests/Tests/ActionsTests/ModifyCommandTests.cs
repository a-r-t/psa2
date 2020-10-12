using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using PSA2.src.FileProcessor.MovesetHandler;
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;

namespace PSA2Tests.Tests.ActionsTests
{
    [TestFixture]
    public class ModifyCommandTests
    {
        [Test]
        public void ModifyCommandInActionWithIdenticalCommand()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(6, 1)
            };
            psaMovesetParser.ActionsHandler.ModifyCommand(0, 0, 0, new PsaCommand(33620480, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Modify/FitMarioModifyCommandWithIdenticalCommand.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Modify/FitMarioModifyCommandWithIdenticalCommand.pac", "./Tests/ActionsTests/Out/Modify/FitMarioModifyCommandWithIdenticalCommand.pac"));
        }

        [Test]
        public void ModifyCommandInActionWithIdenticalCommandWithDifferentParameterValues()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 1),
                new PsaCommandParameter(6, 2)
            };
            psaMovesetParser.ActionsHandler.ModifyCommand(0, 0, 0, new PsaCommand(33620480, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterValues.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterValues.pac", "./Tests/ActionsTests/Out/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterValues.pac"));
        }

        [Test]
        public void ModifyCommandInActionWithIdenticalCommandWithDifferentParameterTypes()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(1, 0),
                new PsaCommandParameter(0, 1)
            };
            psaMovesetParser.ActionsHandler.ModifyCommand(0, 0, 0, new PsaCommand(33620480, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterTypes.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterTypes.pac", "./Tests/ActionsTests/Out/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterTypes.pac"));
        }

        [Test]
        public void ModifyCommandInActionWithDifferentCommandWithIdenticalParameterCount()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(3, 0)
            };
            psaMovesetParser.ActionsHandler.ModifyCommand(0, 0, 0, new PsaCommand(67109376, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Modify/FitMarioModifyCommandWithDifferentCommandWithIdenticalParameterCount.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Modify/FitMarioModifyCommandWithDifferentCommandWithIdenticalParameterCount.pac", "./Tests/ActionsTests/Out/Modify/FitMarioModifyCommandWithDifferentCommandWithIdenticalParameterCount.pac"));
        }

        [Test]
        public void ModifyCommandInActionWithDifferentCommandWithLargerParameterCount()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(6, 0),
                new PsaCommandParameter(5, 0)
            };
            psaMovesetParser.ActionsHandler.ModifyCommand(0, 0, 0, new PsaCommand(33620736, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Modify/FitMarioModifyCommandWithDifferentCommandWithLargerParameterCount.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Modify/FitMarioModifyCommandWithDifferentCommandWithLargerParameterCount.pac", "./Tests/ActionsTests/Out/Modify/FitMarioModifyCommandWithDifferentCommandWithLargerParameterCount.pac"));
        }

        [Test]
        public void ModifyCommandInActionWithDifferentCommandWithSmallerParameterCount()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0)
            };
            psaMovesetParser.ActionsHandler.ModifyCommand(0, 0, 0, new PsaCommand(67109120, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Modify/FitMarioModifyCommandWithDifferentCommandWithSmallerParameterCount.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Modify/FitMarioModifyCommandWithDifferentCommandWithSmallerParameterCount.pac", "./Tests/ActionsTests/Out/Modify/FitMarioModifyCommandWithDifferentCommandWithSmallerParameterCount.pac"));
        }

        [Test]
        public void ModifyCommandInActionWithCommandThatHasPointerParameter()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(2, 98464)
            };
            psaMovesetParser.ActionsHandler.ModifyCommand(0, 0, 0, new PsaCommand(459008, 103152, parameters));
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Modify/FitMarioModifyCommandInActionWithCommandThatHasPointerParameter.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Modify/FitMarioModifyCommandInActionWithCommandThatHasPointerParameter.pac", "./Tests/ActionsTests/Out/Modify/FitMarioModifyCommandInActionWithCommandThatHasPointerParameter.pac"));
        }

        [Test]
        public void ModifyCommandInActionWithNoParametersToCommandWithParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(6, 0)
            };
            psaMovesetParser.ActionsHandler.ModifyCommand(0, 0, 10, new PsaCommand(33620480, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Modify/FitMarioModifyCommandWithNoParametersToCommandWithParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Modify/FitMarioModifyCommandWithNoParametersToCommandWithParameters.pac", "./Tests/ActionsTests/Out/Modify/FitMarioModifyCommandWithNoParametersToCommandWithParameters.pac"));
        }

        [Test]
        public void ModifyCommandInActionWithParametersToCommandWithNoParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>();
            psaMovesetParser.ActionsHandler.ModifyCommand(0, 0, 0, new PsaCommand(917504, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./Tests/ActionsTests/Out/Modify/FitMarioModifyCommandWithParametersToCommandWithNoParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/ActionsTests/ComparisonData/Modify/FitMarioModifyCommandWithParametersToCommandWithNoParameters.pac", "./Tests/ActionsTests/Out/Modify/FitMarioModifyCommandWithParametersToCommandWithNoParameters.pac"));
        }

        [Test]
        [TestCase(2, 0, 11, "FitMarioModifyCommandWithPointerParameter.pac")]
        [TestCase(5, 0, 16, "FitMarioModifyCommandWithPointerParameter2.pac")]
        public void ModifyCommandInActionWithPointerParameter(int actionId, int codeBlockId, int commandIndex, string comparisonFileName)
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(6, 0)
            };
            psaMovesetParser.ActionsHandler.ModifyCommand(actionId, codeBlockId, commandIndex, new PsaCommand(33620480, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile($"./Tests/ActionsTests/Out/Modify/{comparisonFileName}");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./Tests/ActionsTests/ComparisonData/Modify/{comparisonFileName}", $"./Tests/ActionsTests/Out/Modify/{comparisonFileName}"));
        }
    }
}
