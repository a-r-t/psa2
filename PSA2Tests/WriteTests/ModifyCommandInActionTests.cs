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
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioModifyCommandWithIdenticalCommand.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Modify/FitMarioModifyCommandWithIdenticalCommand.pac", "./WriteTests/Out/FitMarioModifyCommandWithIdenticalCommand.pac"));
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
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterValues.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterValues.pac", "./WriteTests/Out/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterValues.pac"));
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
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterTypes.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterTypes.pac", "./WriteTests/Out/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterTypes.pac"));
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
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioModifyCommandWithDifferentCommandWithIdenticalParameterCount.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Modify/FitMarioModifyCommandWithDifferentCommandWithIdenticalParameterCount.pac", "./WriteTests/Out/FitMarioModifyCommandWithDifferentCommandWithIdenticalParameterCount.pac"));
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
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioModifyCommandWithDifferentCommandWithLargerParameterCount.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Modify/FitMarioModifyCommandWithDifferentCommandWithLargerParameterCount.pac", "./WriteTests/Out/FitMarioModifyCommandWithDifferentCommandWithLargerParameterCount.pac"));
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
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioModifyCommandWithDifferentCommandWithSmallerParameterCount.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Modify/FitMarioModifyCommandWithDifferentCommandWithSmallerParameterCount.pac", "./WriteTests/Out/FitMarioModifyCommandWithDifferentCommandWithSmallerParameterCount.pac"));
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
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioModifyCommandWithNoParametersToCommandWithParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Modify/FitMarioModifyCommandWithNoParametersToCommandWithParameters.pac", "./WriteTests/Out/FitMarioModifyCommandWithNoParametersToCommandWithParameters.pac"));
        }

        [Test]
        public void ModifyCommandInActionWithParametersToCommandWithNoParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>();
            psaMovesetParser.ActionsParser.ModifyActionCommand(0, 0, 0, new PsaCommand(917504, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioModifyCommandWithParametersToCommandWithNoParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Modify/FitMarioModifyCommandWithParametersToCommandWithNoParameters.pac", "./WriteTests/Out/FitMarioModifyCommandWithParametersToCommandWithNoParameters.pac"));
        }

        [Test]
        public void ModifyCommandInActionWithPointerParameter()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./WriteTests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(6, 0)
            };
            psaMovesetParser.ActionsParser.ModifyActionCommand(2, 0, 11, new PsaCommand(33620480, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile("./WriteTests/Out/FitMarioModifyCommandWithPointerParameter.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./WriteTests/ComparisonData/Actions/Modify/FitMarioModifyCommandWithPointerParameter.pac", "./WriteTests/Out/FitMarioModifyCommandWithPointerParameter.pac"));
        }
    }
}
