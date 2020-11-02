using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Views.MovesetEditorViews.Interfaces
{
    public interface IParameterEditorListener
    {
        void OnParameterChange(bool isDirty);
    }
}
