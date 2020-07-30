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
using Tangram.Core.Event;

namespace Tangram.Builtin.WebBrowser
{

    [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisibleAttribute(true)]
    public class FormForScript
    {
        private IntPtr formHandle;
        public FormForScript(IntPtr formHandle)
        {
            this.formHandle = formHandle;
        }
        public FormForScript show(string parent)
        {
            IPCMessageManager.Send(this.formHandle, MessageType.Show, parent);
            return new FormForScript(formHandle);
        }
        public FormForScript close()
        {
            IPCMessageManager.Send(this.formHandle, MessageType.Close);

            return new FormForScript(formHandle);
        }
        public FormForScript hide()
        {
            IPCMessageManager.Send(this.formHandle, MessageType.Hide);
            return new FormForScript(formHandle);
        }
        public FormForScript size(int width, int height)
        {
            IPCMessageManager.Send(this.formHandle, MessageType.Size, width, height);
            return new FormForScript(formHandle);
        }
        public FormForScript site(int left, int top)
        {
            IPCMessageManager.Send(this.formHandle, MessageType.Site, left, top);
            return new FormForScript(formHandle);
        }
        public FormForScript refresh(string url)
        {
            IPCMessageManager.Send(this.formHandle, MessageType.Refresh, url);
            return new FormForScript(formHandle);
        }
        public FormForScript exec(string script, bool asyn = false)
        {
            IPCMessageManager.Send(this.formHandle, MessageType.Exec, script);
            return new FormForScript(formHandle);
        }
        public FormForScript mode(int status)
        {
            IPCMessageManager.Send(this.formHandle, MessageType.Mode, status);
            return new FormForScript(formHandle);
        }
    }

}