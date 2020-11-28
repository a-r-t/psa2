using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PSA2.src.ExtentionMethods;
using PSA2.src.Configuration;

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
                    return psaMovesetHandler.SubActionsHandler.GetPsaCommandsInCodeBlock(SectionIndex, CodeBlockIndex);
                default:
                    throw new ArgumentException($"Section Type not yet implemented: {SectionType}");
            }
        }

        public PsaCommand GetPsaCommandInCodeBlock(int commandIndex)
        {
            switch (SectionType)
            {
                case SectionType.ACTION:
                    return psaMovesetHandler.ActionsHandler.GetPsaCommandInCodeBlock(SectionIndex, CodeBlockIndex, commandIndex);
                case SectionType.SUBACTION:
                    return psaMovesetHandler.SubActionsHandler.GetPsaCommandInCodeBlock(SectionIndex, CodeBlockIndex, commandIndex);
                default:
                    throw new ArgumentException($"Section Type not yet implemented: {SectionType}");
            }
        }

        public int GetNumberOfPsaCommandsInCodeBlock()
        {
            switch (SectionType)
            {
                case SectionType.ACTION:
                    return psaMovesetHandler.ActionsHandler.GetNumberOfPsaCommandsInCodeBlock(SectionIndex, CodeBlockIndex);
                case SectionType.SUBACTION:
                    return psaMovesetHandler.SubActionsHandler.GetNumberOfPsaCommandsInCodeBlock(SectionIndex, CodeBlockIndex);
                default:
                    throw new ArgumentException($"Section Type not yet implemented: {SectionType}");
            }
        }

        public void InsertCommand(int commandIndex, PsaCommand psaCommand)
        {
            switch (SectionType)
            {
                case SectionType.ACTION:
                    psaMovesetHandler.ActionsHandler.InsertCommand(SectionIndex, CodeBlockIndex, commandIndex, psaCommand);
                    break;
                case SectionType.SUBACTION:
                    psaMovesetHandler.SubActionsHandler.InsertCommand(SectionIndex, CodeBlockIndex, commandIndex, psaCommand);
                    break;
                default:
                    throw new ArgumentException($"Section Type not yet implemented: {SectionType}");
            }
        }

        public void ModifyCommand(int commandIndex, PsaCommand psaCommand)
        {
            switch (SectionType)
            {
                case SectionType.ACTION:
                    psaMovesetHandler.ActionsHandler.ModifyCommand(SectionIndex, CodeBlockIndex, commandIndex, psaCommand);
                    break;
                case SectionType.SUBACTION:
                    psaMovesetHandler.SubActionsHandler.ModifyCommand(SectionIndex, CodeBlockIndex, commandIndex, psaCommand);
                    break;
                default:
                    throw new ArgumentException($"Section Type not yet implemented: {SectionType}");
            }
        }

        public void MoveCommandUp(int commandIndex)
        {
            switch (SectionType)
            {
                case SectionType.ACTION:
                    psaMovesetHandler.ActionsHandler.MoveCommandUp(SectionIndex, CodeBlockIndex, commandIndex);
                    break;
                case SectionType.SUBACTION:
                    psaMovesetHandler.SubActionsHandler.MoveCommandUp(SectionIndex, CodeBlockIndex, commandIndex);
                    break;
                default:
                    throw new ArgumentException($"Section Type not yet implemented: {SectionType}");
            }
        }

        public void MoveCommandDown(int commandIndex)
        {
            switch (SectionType)
            {
                case SectionType.ACTION:
                    psaMovesetHandler.ActionsHandler.MoveCommandDown(SectionIndex, CodeBlockIndex, commandIndex);
                    break;
                case SectionType.SUBACTION:
                    psaMovesetHandler.SubActionsHandler.MoveCommandDown(SectionIndex, CodeBlockIndex, commandIndex);
                    break;
                default:
                    throw new ArgumentException($"Section Type not yet implemented: {SectionType}");
            }
        }

        public void RemoveCommand(int commandIndex)
        {
            switch (SectionType)
            {
                case SectionType.ACTION:
                    psaMovesetHandler.ActionsHandler.RemoveCommand(SectionIndex, CodeBlockIndex, commandIndex);
                    break;
                case SectionType.SUBACTION:
                    psaMovesetHandler.SubActionsHandler.RemoveCommand(SectionIndex, CodeBlockIndex, commandIndex);
                    break;
                default:
                    throw new ArgumentException($"Section Type not yet implemented: {SectionType}");
            }
        }

        public string ToSectionString()
        {
            string sectionIndexHex = "";
            string alias = "";
            switch (SectionType)
            {
                case SectionType.ACTION:
                    sectionIndexHex = (SectionIndex + 274).ToString("X");
                    alias = Config.ActionAliasesConfig.GetActionAlias(SectionIndex);
                    break;
                case SectionType.SUBACTION:
                    sectionIndexHex = SectionIndex.ToString("X");
                    alias = psaMovesetHandler.SubActionsHandler.GetAnimationName(SectionIndex);
                    break;
            }
            return $"{SectionType.ToString().ToTitleCase()} {sectionIndexHex}{FormatAlias(alias)}";
        }

        private string FormatAlias(string alias)
        {
            return alias != ""
                ? $" - {alias}"
                : "";
        }

        public string ToCodeBlockString()
        {
            string sectionIndexHex = "";
            switch(SectionType)
            {
                case SectionType.ACTION:
                    sectionIndexHex = (SectionIndex + 274).ToString("X");
                    break;
                case SectionType.SUBACTION:
                    sectionIndexHex = SectionIndex.ToString("X");
                    break;
            }
            return $"{SectionType.ToString().ToTitleCase()} {sectionIndexHex} - {CodeBlockIndex}";
        }
    }
}
