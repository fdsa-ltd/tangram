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
using System.Diagnostics;

namespace Tangram.Core
{
    [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisibleAttribute(true)]
    public class GlobalForScript
    {
        private readonly string formName;
        public GlobalForScript(string formName)
        {
            this.formName = formName;
        }
        public FormForScript open(string url, string formName, string type, string features = "", string parent = "")
        {
            var newForm = ScreenManager.External.Open(url, formName, type, features, parent);
            return new FormForScript(newForm.Title);
        }
        public FormForScript find(string formName = "")
        {
            if (string.IsNullOrEmpty(formName))
            {
                return new FormForScript(this.formName);
            }
            if (ScreenManager.External.Find(formName)==null)
            {
                return null;
            }
            return new FormForScript(formName);
        }
        public FormForScript run(string fileName, string arguments = "")
        {
            try
            {
                Process.Start(fileName, arguments);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return new FormForScript(this.formName);
        }

        public void exec(string name, string script)
        {
            ScreenManager.External.Exec(name, script);

        }
        public void invoke(string name, string method, params string[] args)
        {
            ScreenManager.External.Invoke(name, method, args);
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
            return new StateForScript(this.formName);
        }
        public SocketForScript socket(string url, string options)
        {
            return new SocketForScript(this.formName, url, SocketOptions.options(options));
        }


    }

}