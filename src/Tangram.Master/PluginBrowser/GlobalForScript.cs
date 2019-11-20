using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using Tangram.Core; 

namespace Tangram.PluginBrowser
{
    public class GlobalForScript
    {
        private readonly string formName;
        public GlobalForScript(string formName)
        {
            this.formName = formName;
        }
        public FormForScript open(string url, string features = "")
        {
            var newForm = ScreenManager.External.Open(url, features);
            return new FormForScript(newForm.Text);
        }
        public FormForScript find(string formName = "")
        {
            if (string.IsNullOrEmpty(formName))
            {
                return new FormForScript(this.formName);
            }
            if (ScreenManager.External.Find(formName) == null)
            {
                return null;
            }
            return new FormForScript(formName);
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



    }

}