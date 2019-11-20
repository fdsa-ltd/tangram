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
    public class GlobalForScript
    {
        private IntPtr controller;
        private IntPtr handle;
        private string title;
        public GlobalForScript(IntPtr controller, IntPtr handle, string title)
        {
            this.controller = controller;
            this.handle = handle;
            this.title = title;
        }
        public FormForScript open(string url, string formName = "", string type = "ie", string features = "", string parent = "")
        {
            var result = WindowMessageManager.Send(controller, WindowMessageType.GlobalOpen, string.Empty, url, formName, type, features, parent);
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            var form = FormInfo.FromString(result);
            return new FormForScript(this.controller, form.Handle, form.Name);
        }
        public FormForScript find(string formName = "")
        {

            if (string.IsNullOrEmpty(formName))
            {
                return new FormForScript(this.controller, this.controller, this.title);
            }
            var result = WindowMessageManager.Send(controller, WindowMessageType.GlobalFind, string.Empty, formName);
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            var form = FormInfo.FromString(result);
            return new FormForScript(this.controller, form.Handle, form.Name);
        }

        public void exec(string name, string script)
        {
            WindowMessageManager.Send(controller, WindowMessageType.GlobalExec, name, script);

        }
        public void invoke(string name, string method, params string[] args)
        {
            WindowMessageManager.Send(controller, WindowMessageType.GlobalInvoke, name, method, args);
        }
        public StoreForScript store
        {
            get
            {
                return new StoreForScript();
            }
            set
            {
            }
        }
        public StateForScript state()
        {
            return new StateForScript(this.controller, this.handle, this.title);
        }
        public SocketForScript socket(string url, string options)
        {
            return new SocketForScript(this.title, url, SocketOptions.options(options));
        }


    }

}