﻿using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.Configs;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Views.MovesetEditorViews.Interfaces
{
    public interface ICodeBlockViewerListener
    {
        void OnCommandSelected(List<PsaCommand> psaCommands, List<int> commandIndexes, CodeBlockSelection codeBlockSelection);
    }
}
