using PSA2.src.FileProcessor.MovesetHandler;
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Views.MovesetEditorViews
{
    public class SectionSelectionInfo
    {
        public SectionType SectionType { get; set; }
        public int SectionIndex { get; set; }
        public int CodeBlockIndex { get; set; }
        public int CommandIndex { get; set; }
        protected PsaMovesetHandler psaMovesetHandler;

        public SectionSelectionInfo(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            SectionIndex = -1;
            CodeBlockIndex = -1;
            CommandIndex = -1;
        }

        public List<PsaCommand> PsaCommands 
        {
            get
            {
                return GetPsaCommands();
            } 
        }

        public PsaCommand PsaCommand
        {
            get
            {
                return GetPsaCommand();
            }
        }

        public int NumberOfCommands
        {
            get
            {
                return GetNumberOfPsaCommands();
            }
        }

        private List<PsaCommand> GetPsaCommands()
        {
            switch (SectionType)
            {
                case SectionType.ACTION:
                    return psaMovesetHandler.ActionsHandler.GetPsaCommandsInCodeBlock(SectionIndex, CodeBlockIndex);
                case SectionType.SUBACTION:
                    return psaMovesetHandler.SubActionsHandler.GetPsaCommandsForSubAction(SectionIndex, CodeBlockIndex);
                default:
                    return new List<PsaCommand>();
            }
        }

        private PsaCommand GetPsaCommand()
        {
            switch (SectionType)
            {
                case SectionType.ACTION:
                    return psaMovesetHandler.ActionsHandler.GetPsaCommandInCodeBlock(SectionIndex, CodeBlockIndex, CommandIndex);
                case SectionType.SUBACTION:
                    return psaMovesetHandler.SubActionsHandler.GetPsaCommandForSubActionCodeBlock(SectionIndex, CodeBlockIndex, CommandIndex);
                default:
                    return null;
            }
        }

        private int GetNumberOfPsaCommands()
        {
            switch (SectionType)
            {
                case SectionType.ACTION:
                    return psaMovesetHandler.ActionsHandler.GetNumberOfPsaCommandsInCodeBlock(SectionIndex, CodeBlockIndex);
                case SectionType.SUBACTION:
                    return psaMovesetHandler.SubActionsHandler.GetNumberOfPsaCommandsInSubActionCodeBlock(SectionIndex, CodeBlockIndex);
                default:
                    return 0;
            }
        }
    }
}
