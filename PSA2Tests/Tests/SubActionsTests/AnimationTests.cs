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
        [Description("Modify animation name for subaction with identical length")]
        public void ModifyAnimationName()
        {
            PsaMovesetHandler psaMovesetParser = WriteTestsHelper.GetPsaMovesetParser("./Tests/Data/FitMario.pac");
            psaMovesetParser.SubActionsHandler.SetAnimationName(0, "TEST1");
            psaMovesetParser.PsaFile.SaveFile($"./Tests/SubActionsTests/Out/Animation/FitMarioChangedAnimationNameIdenticalLength.pac");
            Assert.IsTrue(WriteTestsHelper.AreFilesIdentical($"./Tests/SubActionsTests/ComparisonData/Animation/FitMarioChangedAnimationNameIdenticalLength.pac", $"./Tests/SubActionsTests/Out/Animation/FitMarioChangedAnimationNameIdenticalLength.pac"));
        }
    }
}
