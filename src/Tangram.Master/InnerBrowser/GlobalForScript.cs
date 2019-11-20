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

namespace Tangram.IEBrowser
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
        public FormForScript open(string url, string features = "")
        {
            var newForm = ScreenManager.External.Open(url, features);
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
    }
}