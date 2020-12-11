using PSA2MovesetLogic.src.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    public class CommandLocationTracker
    {
        public BidirectionalDictionary<int, (SectionType, int, int, int)> Locations { get; private set; }

        public CommandLocationTracker()
        {
            Locations = new BidirectionalDictionary<int, (SectionType, int, int, int)>();
        }

        public enum SectionType
        {
            ACTION, SUBACTION, SUBROUTINE
        }
    }
}
