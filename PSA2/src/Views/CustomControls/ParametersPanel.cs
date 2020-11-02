﻿using PSA2.src.Views.MovesetEditorViews.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.Views.CustomControls
{
    public partial class ParametersPanel : Panel
    {
        public List<ParameterEntry> ParameterEntries { get; }
        private const int startY = 5;
        private const int spacing = 10;
        private List<ParameterEntryUserControl> parameterEntryUserControls;
        private int virtualizationCount = 15;
        private List<IParameterEntryUserControlListener> parameterEntryUserControlListeners = new List<IParameterEntryUserControlListener>();

        public ParametersPanel()
        {
            ParameterEntries = new List<ParameterEntry>();
            parameterEntryUserControls = new List<ParameterEntryUserControl>();
            SetUpVirtualization();
        }

        public void SetUpVirtualization()
        {
            if (virtualizationCount > parameterEntryUserControls.Count)
            {
                int difference = virtualizationCount - parameterEntryUserControls.Count;
                int currentCount = parameterEntryUserControls.Count;
                for (int i = currentCount; i < currentCount + difference; i++)
                {
                    ParameterEntryUserControl parameterEntryUserControl = new ParameterEntryUserControl();
                    parameterEntryUserControl.Visible = false;
                    parameterEntryUserControl.Location = new Point(0, startY + (i * (parameterEntryUserControl.Height + spacing)));
                    parameterEntryUserControl.Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left);
                    parameterEntryUserControl.Width = Width - 10;
                    parameterEntryUserControls.Add(parameterEntryUserControl);

                    foreach (IParameterEntryUserControlListener listener in parameterEntryUserControlListeners)
                    {
                        parameterEntryUserControl.AddListener(listener);
                    }

                    Controls.Add(parameterEntryUserControl);
                }
            }
        }

        public void AddOnChangeListener(IParameterEntryUserControlListener listener)
        {
            parameterEntryUserControlListeners.Add(listener);
            for (int i = 0; i < parameterEntryUserControls.Count; i++)
            {
                parameterEntryUserControls[i].AddListener(listener);
            }
        }

        public void AddParameterEntry(ParameterEntry parameterEntry)
        {
            ParameterEntries.Add(parameterEntry);
        }

        public void ClearParameterEntries()
        {
            ParameterEntries.Clear();
        }

        public void Reload()
        {
            SuspendLayout();

            if (ParameterEntries.Count > parameterEntryUserControls.Count)
            {
                virtualizationCount += (ParameterEntries.Count - parameterEntryUserControls.Count);
                SetUpVirtualization();
            }

            for (int i = 0; i < ParameterEntries.Count; i++)
            {
                ParameterEntry parameterEntry = ParameterEntries[i];
                ParameterEntryUserControl parameterEntryUserControl = parameterEntryUserControls[i];
                parameterEntryUserControl.ParameterEntry = parameterEntry;
                parameterEntryUserControl.Width = Width - 10;
                parameterEntryUserControl.Visible = true;
            }

            for (int i = ParameterEntries.Count; i < parameterEntryUserControls.Count; i++)
            {
                parameterEntryUserControls[i].Visible = false;
            }
            ResumeLayout();
        }
    }
}
