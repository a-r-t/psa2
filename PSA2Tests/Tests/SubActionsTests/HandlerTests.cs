using NUnit.Framework;
using PSA2.src.FileProcessor.MovesetHandler;
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2.src.Models.Fighter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSA2Tests.Tests.SubActionsTests
{
    [TestFixture]
    public class HandlerTests
    {
        [Test]
        [Description("Get animation for subaction")]
        public void ReadPsaCommandsFromSubAction()
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
    }
}
