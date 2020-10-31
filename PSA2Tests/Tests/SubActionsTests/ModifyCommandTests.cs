using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;

namespace PSA2Tests.Tests.SubActionsTests
{
    [TestFixture]
    public class ModifyCommandTests
    {
        [Test]
        public void ModifyCommandInSubActionWithIdenticalCommand()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 39),
                new PsaCommandParameter(0, 2)
            };
            psaMovesetParser.SubActionsHandler.ModifyCommand(93, 0, 3, new PsaCommand(101188096, 73880, parameters));
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Modify/FitMarioModifyCommandWithIdenticalCommand.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Modify/FitMarioModifyCommandWithIdenticalCommand.pac", "./Tests/SubActionsTests/Out/Modify/FitMarioModifyCommandWithIdenticalCommand.pac"));
        }

        [Test]
        public void ModifyCommandInSubActionWithIdenticalCommandWithDifferentParameterValues()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 295),
                new PsaCommandParameter(0, 0)
            };
            psaMovesetParser.SubActionsHandler.ModifyCommand(93, 0, 3, new PsaCommand(101188096, 73880, parameters));
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterValues.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterValues.pac", "./Tests/SubActionsTests/Out/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterValues.pac"));
        }

        [Test]
        public void ModifyCommandInSubActionWithIdenticalCommandWithDifferentParameterTypes()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(1, 39),
                new PsaCommandParameter(5, 2)
            };
            psaMovesetParser.SubActionsHandler.ModifyCommand(93, 0, 3, new PsaCommand(101188096, 73880, parameters));
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterTypes.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterTypes.pac", "./Tests/SubActionsTests/Out/Modify/FitMarioModifyCommandWithIdenticalCommandWithDifferentParameterTypes.pac"));
        }

        [Test]
        public void ModifyCommandInSubActionWithDifferentCommandWithIdenticalParameterCount()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(1, 0),
            };
            psaMovesetParser.SubActionsHandler.ModifyCommand(16, 0, 3, new PsaCommand(65792, 67784, parameters));
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Modify/FitMarioModifyCommandWithDifferentCommandWithIdenticalParameterCount.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Modify/FitMarioModifyCommandWithDifferentCommandWithIdenticalParameterCount.pac", "./Tests/SubActionsTests/Out/Modify/FitMarioModifyCommandWithDifferentCommandWithIdenticalParameterCount.pac"));
        }
        
        [Test]
        public void ModifyCommandInSubActionWithDifferentCommandWithLargerParameterCount()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(1, 0),
                new PsaCommandParameter(1, 0),
                new PsaCommandParameter(1, 0),
                new PsaCommandParameter(1, 0),
                new PsaCommandParameter(1, 0),
                new PsaCommandParameter(1, 0),
                new PsaCommandParameter(1, 0),
                new PsaCommandParameter(0, 0)
            };
            psaMovesetParser.SubActionsHandler.ModifyCommand(72, 0, 4, new PsaCommand(100666624, 68768, parameters));
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Modify/FitMarioModifyCommandWithDifferentCommandWithLargerParameterCount.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Modify/FitMarioModifyCommandWithDifferentCommandWithLargerParameterCount.pac", "./Tests/SubActionsTests/Out/Modify/FitMarioModifyCommandWithDifferentCommandWithLargerParameterCount.pac"));
        }
        
        [Test]
        public void ModifyCommandInSubActionWithDifferentCommandWithSmallerParameterCount()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(1, 2400000)
            };
            psaMovesetParser.SubActionsHandler.ModifyCommand(72, 0, 1, new PsaCommand(131328, 68456, parameters));
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Modify/FitMarioModifyCommandWithDifferentCommandWithSmallerParameterCount.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Modify/FitMarioModifyCommandWithDifferentCommandWithSmallerParameterCount.pac", "./Tests/SubActionsTests/Out/Modify/FitMarioModifyCommandWithDifferentCommandWithSmallerParameterCount.pac"));
        }
        
        [Test]
        public void ModifyCommandInSubActionWithCommandThatHasPointerParameter()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(2, 68816)
            };
            psaMovesetParser.SubActionsHandler.ModifyCommand(72, 0, 0, new PsaCommand(459008, 68448, parameters));
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Modify/FitMarioModifyCommandInSubActionWithCommandThatHasPointerParameter.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Modify/FitMarioModifyCommandInSubActionWithCommandThatHasPointerParameter.pac", "./Tests/SubActionsTests/Out/Modify/FitMarioModifyCommandInSubActionWithCommandThatHasPointerParameter.pac"));
        }
        
        [Test]
        public void ModifyCommandInSubActionWithNoParametersToCommandWithParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(6, 0)
            };
            psaMovesetParser.SubActionsHandler.ModifyCommand(72, 0, 5, new PsaCommand(33620480, 0, parameters));
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Modify/FitMarioModifyCommandWithNoParametersToCommandWithParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Modify/FitMarioModifyCommandWithNoParametersToCommandWithParameters.pac", "./Tests/SubActionsTests/Out/Modify/FitMarioModifyCommandWithNoParametersToCommandWithParameters.pac"));
        }
        
        [Test]
        public void ModifyCommandInSubActionWithParametersToCommandWithNoParameters()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>();
            psaMovesetParser.SubActionsHandler.ModifyCommand(0, 3, 0, new PsaCommand(524288, 8112, parameters));
            psaMovesetParser.PsaFile.SaveFile("./Tests/SubActionsTests/Out/Modify/FitMarioModifyCommandWithParametersToCommandWithNoParameters.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical("./Tests/SubActionsTests/ComparisonData/Modify/FitMarioModifyCommandWithParametersToCommandWithNoParameters.pac", "./Tests/SubActionsTests/Out/Modify/FitMarioModifyCommandWithParametersToCommandWithNoParameters.pac"));
        }
        
        [Test]
        [TestCase(15, 1, 6, "FitMarioModifyCommandWithPointerParameter.pac")]
        [TestCase(319, 1, 0, "FitMarioModifyCommandWithPointerParameter2.pac")]
        public void ModifyCommandInSubActionWithPointerParameter(int SubActionId, int codeBlockId, int commandIndex, string comparisonFileName)
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>
            {
                new PsaCommandParameter(0, 0),
                new PsaCommandParameter(6, 0)
            };
            psaMovesetParser.SubActionsHandler.ModifyCommand(SubActionId, codeBlockId, commandIndex, new PsaCommand(33620480, 25788, parameters));
            psaMovesetParser.PsaFile.SaveFile($"./Tests/SubActionsTests/Out/Modify/{comparisonFileName}");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./Tests/SubActionsTests/ComparisonData/Modify/{comparisonFileName}", $"./Tests/SubActionsTests/Out/Modify/{comparisonFileName}"));
        }
    }
}
