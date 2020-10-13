using PSA2.src.FileProcessor.MovesetHandler.Configs;
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Views.MovesetEditorViews
{
    public interface ICodeBlockViewerListener
    {
        void OnCommandSelected(PsaCommandConfig psaCommandConfig, PsaCommand psaCommand, SectionType sectionType, int sectionIndex, int codeBlockIndex, int commandIndex);
    }
}
