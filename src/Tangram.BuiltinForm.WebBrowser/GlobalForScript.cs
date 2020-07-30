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
using System.Windows.Forms.VisualStyles;

namespace Tangram.Builtin.WebBrowser
{
    [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisibleAttribute(true)]
    public class GlobalForScript
    {
        private readonly IntPtr formHandle;
        private readonly IntPtr globalHandle;
        public GlobalForScript(IntPtr formHandle, IntPtr globalHandle)
        {
            this.formHandle = formHandle;
            this.globalHandle = globalHandle;
        }
        public FormForScript open(string url, string features = "")
        {
            var handle = IPCMessageManager.Send(this.globalHandle, MessageType.GlobalOpen, url, features);

            if (string.IsNullOrEmpty(handle))
            {
                return null;
            }
            return new FormForScript(Marshal.StringToHGlobalAnsi(handle));
        }
        public FormForScript find(string formName)
        {
            if (string.IsNullOrEmpty(formName))
            {
                return new FormForScript(this.formHandle);
            }
            var handle = IPCMessageManager.Send(this.globalHandle, MessageType.GlobalFind, formName);

            if (string.IsNullOrEmpty(handle))
            {
                return null;
            }
            return new FormForScript(Marshal.StringToHGlobalAnsi(handle));
        }

        public void exec(string name, string script)
        {
            IPCMessageManager.Send(this.globalHandle, MessageType.GlobalExec, name, script);

        }
        public void invoke(string name, string method, params string[] args)
        {
            IPCMessageManager.Send(this.globalHandle, MessageType.GlobalInvoke, name, method, args);
        }
        //private StoreForScript s = new StoreForScript();
        //public StoreForScript store
        //{
        //    get
        //    {
        //        return s;
        //    }
        //    set
        //    {
        //        this.s = value;
        //    }
        //}
        //public StateForScript state()
        //{
        //    if (this.form == null)
        //    {
        //        return null;
        //    }
        //    return new StateForScript(this.form);
        //}
        //public SocketForScript socket(string url, string options)
        //{
        //    if (this.form == null)
        //    {
        //        return null;
        //    }
        //    return new SocketForScript(this.form.Title, url, SocketOptions.options(options));
        //}


    }

}