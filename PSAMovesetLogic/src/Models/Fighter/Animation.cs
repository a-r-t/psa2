using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Models.Fighter
{
    public class Animation
    {
        public string AnimationName { get; set; }
        public AnimationFlags AnimationFlags { get; private set; }

        public Animation(string animationName, AnimationFlags animationFlags)
        {
            AnimationName = animationName;
            AnimationFlags = animationFlags;
        }
    }
}
