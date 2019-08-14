using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter
{
    public class Animation
    {
        public string AnimationName { get; set; }
        public int InTransitionFrames { get; set; }
        public AnimationFlag[] AnimationFlags { get; private set; }

        public Animation(string animationName)
        {
            AnimationName = animationName;
            InTransitionFrames = 0;
            AnimationFlags = new AnimationFlag[]
            {
                new AnimationFlag("No Out Transition"),
                new AnimationFlag("Loop"),
                new AnimationFlag("Moves Character"),
                new AnimationFlag("Unknown3"),
                new AnimationFlag("Unknown4"),
                new AnimationFlag("Unknown5"),
                new AnimationFlag("TransitionOutFromStart"),
                new AnimationFlag("Unknown7")
            };
        }
    }
}
