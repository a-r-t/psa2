using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Utility
{
    public static class Constants
    {
        public const int NOP = 131072; // 131072 is the nop command in psa (20000)
        public const int FADEF00D = -86052851; // -86052851 is FFFF FFFF FADE F00D
        public const int FADE0D8A = -86110838; // -86110838 is FFFF FFFF FADE 0D8A    
    }
}