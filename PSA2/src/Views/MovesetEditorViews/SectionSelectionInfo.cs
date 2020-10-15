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

        public SectionSelectionInfo Clone()
        {
            SectionSelectionInfo clone = new SectionSelectionInfo(psaMovesetHandler);
            clone.SectionType = SectionType;
            clone.SectionIndex = SectionIndex;
            clone.CodeBlockIndex = CodeBlockIndex;
            clone.CommandIndex = CommandIndex;
            return clone;
        }
    }
}
