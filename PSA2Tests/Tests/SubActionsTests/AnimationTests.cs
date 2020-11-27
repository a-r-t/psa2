using NUnit.Framework;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2MovesetLogic.src.Models.Fighter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSA2Tests.Tests.SubActionsTests
{
    [TestFixture]
    public class AnimationTests
    {
        [Test]
        [Description("Get animation for subaction")]
        public void ReadAnimationDetailsFromSubAction()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            Animation animation = psaMovesetParser.SubActionsHandler.GetSubActionAnimationData(80);
            Assert.AreEqual("AttackS3S", animation.AnimationName);
            Assert.AreEqual(0, animation.AnimationFlags.InTransition);
            Assert.AreEqual(0, animation.AnimationFlags.NoOutTransition);
            Assert.AreEqual(0, animation.AnimationFlags.Loop);
            Assert.AreEqual(4, animation.AnimationFlags.MovesCharacter);
            Assert.AreEqual(0, animation.AnimationFlags.Unknown3);
            Assert.AreEqual(0, animation.AnimationFlags.Unknown4);
            Assert.AreEqual(0, animation.AnimationFlags.Unknown5);
            Assert.AreEqual(0, animation.AnimationFlags.TransitionOutFromStart);
            Assert.AreEqual(0, animation.AnimationFlags.Unknown7);
        }

        [Test]
        [TestCase(0, "TEST1", "FitMarioChangedAnimationNameIdenticalLength.pac")]
        [TestCase(1, "TEST2", "FitMarioChangedAnimationNameIdenticalLength2.pac")]
        [Description("Modify animation name for subaction with smaller length")]
        public void ModifyAnimationNameWithIdenticalLength(int subActionId, string animationName, string comparisonFile)
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.ModifyAnimationName(subActionId, animationName);
            psaMovesetParser.PsaFile.SaveFile($"./Tests/SubActionsTests/Out/Animation/{comparisonFile}");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./Tests/SubActionsTests/ComparisonData/Animation/{comparisonFile}", $"./Tests/SubActionsTests/Out/Animation/{comparisonFile}"));
        }

        [Test]
        [TestCase(0, "TESTING", "FitMarioChangedAnimationNameLargerLength.pac")]
        [TestCase(1, "TESTING", "FitMarioChangedAnimationNameLargerLength2.pac")]
        [Description("Modify animation name for subaction with larger length")]
        public void ModifyAnimationNameWithLargerLength(int subActionId, string animationName, string comparisonFile)
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.ModifyAnimationName(subActionId, animationName);
            psaMovesetParser.PsaFile.SaveFile($"./Tests/SubActionsTests/Out/Animation/{comparisonFile}");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./Tests/SubActionsTests/ComparisonData/Animation/{comparisonFile}", $"./Tests/SubActionsTests/Out/Animation/{comparisonFile}"));
        }

        [Test]
        [Description("Modify animation name for subaction with no current animation name")]
        public void AddNewAnimationName()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.ModifyAnimationName(3, "TEST");
            psaMovesetParser.PsaFile.SaveFile($"./Tests/SubActionsTests/Out/Animation/FitMarioAddNewAnimationName.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./Tests/SubActionsTests/ComparisonData/Animation/FitMarioAddNewAnimationName.pac", $"./Tests/SubActionsTests/Out/Animation/FitMarioAddNewAnimationName.pac"));
        }


        [Test]
        [Description("Modify animation name for subaction that uses a shared animation name")]
        public void ModifyAnimationNameFromSharedToUnique()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.ModifyAnimationName(21, "TEST");
            psaMovesetParser.PsaFile.SaveFile($"./Tests/SubActionsTests/Out/Animation/FitMarioChangedAnimationNameFromSharedToUnique.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./Tests/SubActionsTests/ComparisonData/Animation/FitMarioChangedAnimationNameFromSharedToUnique.pac", $"./Tests/SubActionsTests/Out/Animation/FitMarioChangedAnimationNameFromSharedToUnique.pac"));
        }

        
        [Test]
        [Description("Modify animation name for subaction to an animation name that already exists")]
        public void ModifyAnimationToAnAlreadyExistingAnimationName()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.ModifyAnimationName(1, "Wait1");
            psaMovesetParser.PsaFile.SaveFile($"./Tests/SubActionsTests/Out/Animation/FitMarioChangedAnimationNameFromUniqueToShared.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./Tests/SubActionsTests/ComparisonData/Animation/FitMarioChangedAnimationNameFromUniqueToShared.pac", $"./Tests/SubActionsTests/Out/Animation/FitMarioChangedAnimationNameFromUniqueToShared.pac"));
        }

        [Test]
        [Description("Modify animation name for subaction where location goes past data section intially and needs to be moved")]
        public void ModifyAnimationWhereLocationGoesPastDataSection()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.ModifyAnimationName(477, "abcdefghijklmnopqrstuvwxyz");
            psaMovesetParser.PsaFile.SaveFile($"./Tests/SubActionsTests/Out/Animation/FitMarioChangedAnimationNameWhereLocationGoesPastDataSection.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./Tests/SubActionsTests/ComparisonData/Animation/FitMarioChangedAnimationNameWhereLocationGoesPastDataSection.pac", $"./Tests/SubActionsTests/Out/Animation/FitMarioChangedAnimationNameWhereLocationGoesPastDataSection.pac"));
        }
    }
}
