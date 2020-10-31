using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Models.Fighter
{
    public class AnimationFlag
    {
        public string AnimationFlagName { get; set; }
        public bool IsSet { get; set; }

        public AnimationFlag(string animationFlagName)
        {
            AnimationFlagName = animationFlagName;
            IsSet = false;
        }

    }
}
