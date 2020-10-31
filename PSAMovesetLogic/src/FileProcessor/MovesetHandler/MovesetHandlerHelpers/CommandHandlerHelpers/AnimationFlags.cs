using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers
{
    public class AnimationFlags
    {
        public int InTransition { get; set; }
        public int NoOutTransition { get; set; }
        public int Loop { get; set; }
        public int MovesCharacter { get; set; }
        public int Unknown3 { get; set; }
        public int Unknown4 { get; set; }
        public int Unknown5 { get; set; }
        public int TransitionOutFromStart { get; set; }
        public int Unknown7 { get; set; }

        public AnimationFlags(int inTransition, int noOutTransition, int loop, int movesCharacter, int unknown3, int unknown4, int unknown5, int transitionOutFromStart, int unknown7)
        {
            InTransition = inTransition;
            NoOutTransition = noOutTransition;
            Loop = loop;
            MovesCharacter = movesCharacter;
            Unknown3 = unknown3;
            Unknown4 = unknown4;
            Unknown5 = unknown5;
            TransitionOutFromStart = transitionOutFromStart;
            Unknown7 = unknown7;
        }

        public override string ToString()
        {
            return $"In Transition: {InTransition}\nNo Out Transition: {Convert.ToBoolean(NoOutTransition).ToString()}\nLoop: {Convert.ToBoolean(Loop).ToString()}\n" +
                $"Moves Character: {Convert.ToBoolean(MovesCharacter).ToString()}\nUnknown3: {Convert.ToBoolean(Unknown3).ToString()}\nUnknown4: {Convert.ToBoolean(Unknown4).ToString()}\n" +
                $"Unknown5: {Convert.ToBoolean(Unknown5).ToString()}\nTransition Out From Start: {Convert.ToBoolean(TransitionOutFromStart).ToString()}\nUnknown7: {Convert.ToBoolean(Unknown7).ToString()}\n";
        }
    }
}
