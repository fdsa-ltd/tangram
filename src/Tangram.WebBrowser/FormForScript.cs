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

namespace Tangram.IE
{

    [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisibleAttribute(true)]
    public class FormForScript
    {
        private IFormBrowser form;
        public FormForScript(IFormBrowser form)
        {
            this.form = form;
        }
        public FormForScript show(string parent)
        {
            ScreenManager.External.Invoke(form.Title, "show", parent);
            return new FormForScript(form);
        }
        public FormForScript close()
        {
            ScreenManager.External.Invoke(form.Title, "close");
            return new FormForScript(form);
        }
        public FormForScript hide()
        {
            ScreenManager.External.Invoke(form.Title, "hide");
            return new FormForScript(form);
        }
        public FormForScript size(int width, int height)
        {
            ScreenManager.External.Invoke(form.Title, "size", new string[] { width.ToString(), height.ToString() });
            return new FormForScript(form);
        }
        public FormForScript position(int left, int top)
        {
            ScreenManager.External.Invoke(form.Title, "position", new string[] { left.ToString(), top.ToString() });
            return new FormForScript(form);
        }
        public FormForScript refresh(string url)
        {
            ScreenManager.External.Invoke(form.Title, "refresh", url);
            return new FormForScript(form);
        }
        public FormForScript exec(string script, bool asyn = false)
        {
            ScreenManager.External.Exec(form.Title, script, asyn);
            return new FormForScript(form);
        }
        public FormForScript mode(string status)
        {
            ScreenManager.External.Invoke(form.Title, "mode", status);
            return new FormForScript(form);
        }
    }

}