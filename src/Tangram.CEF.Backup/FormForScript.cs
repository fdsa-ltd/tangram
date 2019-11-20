using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Tangram.Core;
using Tangram.Utility;

namespace Tangram.CEF
{

    [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisibleAttribute(true)]
    public class FormForScript
    {
        private Form form;
        public FormForScript(Form form)
        {
            this.form = form;
        }
        public FormForScript show(string parent)
        {
            ScreenManager.External.Invoke(form.Text, "show", parent);
            return new FormForScript(form);
        }
        public FormForScript close()
        {
            ScreenManager.External.Invoke(form.Text, "close");
            return new FormForScript(form);
        }
        public FormForScript hide()
        {
            ScreenManager.External.Invoke(form.Text, "hide");
            return new FormForScript(form);
        }
        public FormForScript size(int width, int height)
        {
            ScreenManager.External.Invoke(form.Text, "size", new string[] { width.ToString(), height.ToString() });
            return new FormForScript(form);
        }
        public FormForScript position(int left, int top)
        {
            ScreenManager.External.Invoke(form.Text, "position", new string[] { left.ToString(), top.ToString() });
            return new FormForScript(form);
        }
        public FormForScript refresh(string url)
        {
            ScreenManager.External.Invoke(form.Text, "refresh", url);
            return new FormForScript(form);
        }
        public FormForScript exec(string script, bool asyn = false)
        {
            ScreenManager.External.Exec(form.Text, script, asyn);
            return new FormForScript(form);
        }
        public FormForScript mode(string status)
        {
            ScreenManager.External.Invoke(form.Text, "mode", status);
            return new FormForScript(form);
        }
    }

}