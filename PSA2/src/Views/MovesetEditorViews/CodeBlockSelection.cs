using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Views.MovesetEditorViews
{
    public class CodeBlockSelection
    {
        public SectionType SectionType { get; set; }
        public int SectionIndex { get; set; }
        public int CodeBlockIndex { get; set; }
        protected PsaMovesetHandler psaMovesetHandler;

        public CodeBlockSelection(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            SectionIndex = -1;
            CodeBlockIndex = -1;
        }

        public List<PsaCommand> GetPsaCommandsInCodeBlock()
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

        public PsaCommand GetPsaCommandInCodeBlock(int commandIndex)
        {
            switch (SectionType)
            {
                case SectionType.ACTION:
                    return psaMovesetHandler.ActionsHandler.GetPsaCommandInCodeBlock(SectionIndex, CodeBlockIndex, commandIndex);
                case SectionType.SUBACTION:
                    return psaMovesetHandler.SubActionsHandler.GetPsaCommandForSubActionCodeBlock(SectionIndex, CodeBlockIndex, commandIndex);
                default:
                    return null;
            }
        }

        public int GetNumberOfPsaCommandsInCodeBlock()
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
