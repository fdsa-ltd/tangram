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
        private IFormBrowser form;
        public GlobalForScript(IFormBrowser form)
        {
            this.form = form;
        }
        public FormForScript open(string url, string formName = "", string type = "ie", string features = "", string parent = "")
        {
            var newForm = ScreenManager.External.Open(url, formName, type, features, parent);
            if (newForm == null)
            {
                return null;
            }
            return new FormForScript(newForm);
        }
        public FormForScript find(string formName)
        {
            if (string.IsNullOrEmpty(formName))
            {
                return new FormForScript(this.form);
            }
            var form = ScreenManager.External.Find(formName);
            if (form == null)
            {
                return null;
            }
            return new FormForScript(form);
        }

        public void exec(string name, string script)
        {
            ScreenManager.External.Exec(name, script);

        }
        public void invoke(string name, string method, params string[] args)
        {
            ScreenManager.External.Invoke(name, method, args);
        }
        private StoreForScript s = new StoreForScript();
        public StoreForScript store
        {
            get
            {
                return s;
            }
            set
            {
                this.s = value;
            }
        }
        public StateForScript state()
        {
            if (this.form == null)
            {
                return null;
            }
            return new StateForScript(this.form);
        }
        public SocketForScript socket(string url, string options)
        {
            if (this.form == null)
            {
                return null;
            }
            return new SocketForScript(this.form.Title, url, SocketOptions.options(options));
        }


    }

}