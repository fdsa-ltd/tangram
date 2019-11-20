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
        private IntPtr controller;
        private IntPtr handle;
        private string title;
        public FormForScript(IntPtr controller, IntPtr handle, string title)
        {
            this.controller = controller;
            this.handle = handle;
            this.title = title;
        }
        public FormForScript show(string parent)
        {
            WindowMessageManager.Send(controller, WindowMessageType.GlobalInvoke, title, "show", parent);
            return new FormForScript(this.controller, this.handle, this.title);
        }
        public FormForScript close()
        {
            WindowMessageManager.Send(controller, WindowMessageType.GlobalInvoke, title, "close");
            return new FormForScript(this.controller, this.handle, this.title);
        }
        public FormForScript hide()
        {
            WindowMessageManager.Send(controller, WindowMessageType.GlobalInvoke, title, "hide");
            return new FormForScript(this.controller, this.handle, this.title);
        }
        public FormForScript size(int width, int height)
        {
            WindowMessageManager.Send(controller, WindowMessageType.GlobalInvoke, title, "size", width, height);
            return new FormForScript(this.controller, this.handle, this.title);
        }
        public FormForScript position(int left, int top)
        {

            WindowMessageManager.Send(controller, WindowMessageType.GlobalInvoke, title, "position", left, top);
            return new FormForScript(this.controller, this.handle, this.title);
        }
        public FormForScript refresh(string url)
        {
            WindowMessageManager.Send(controller, WindowMessageType.GlobalInvoke, title, "refresh", url);
            return new FormForScript(this.controller, this.handle, this.title);
        }
        public FormForScript mode(string status)
        {
            WindowMessageManager.Send(controller, WindowMessageType.GlobalInvoke, title, "mode", status);
            return new FormForScript(this.controller, this.handle, this.title);
        }
        public FormForScript exec(string script, bool asyn = false)
        {
            WindowMessageManager.Send(handle, WindowMessageType.Exec, title, "exec", script);
            return new FormForScript(this.controller, this.handle, this.title);
        }
    }
}