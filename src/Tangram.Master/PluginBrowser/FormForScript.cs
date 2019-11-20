using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Tangram.Core;

namespace Tangram.PluginBrowser
{
    public class FormForScript
    {
        private readonly string formName;
        public FormForScript(string formName)
        {
            this.formName = formName;
        }
        public FormForScript show(string parent)
        {
            ScreenManager.External.Invoke(formName, "show", parent);
            return new FormForScript(formName);
        }
        public FormForScript close()
        {
            ScreenManager.External.Invoke(formName, "close");
            return new FormForScript(formName);
        }
        public FormForScript hide()
        {
            ScreenManager.External.Invoke(formName, "hide");
            return new FormForScript(formName);
        }
        public FormForScript size(int width, int height)
        {
            ScreenManager.External.Invoke(formName, "size", new string[] { width.ToString(), height.ToString() });
            return new FormForScript(formName);
        }
        public FormForScript position(int left, int top)
        {
            ScreenManager.External.Invoke(formName, "position", new string[] { left.ToString(), top.ToString() });
            return new FormForScript(formName);
        }
        public FormForScript refresh(string url)
        {
            ScreenManager.External.Invoke(formName, "refresh", url);
            return new FormForScript(formName);
        }
        public FormForScript mode(string status)
        {
            ScreenManager.External.Invoke(formName, "mode", status);
            return new FormForScript(formName);
        }
    }

}