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

namespace Tangram.Plugin
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
            this.form.show(parent);
            return new FormForScript(form);
        }
        public FormForScript close()
        {
            this.form.close();
            return new FormForScript(form);
        }
        public FormForScript hide()
        {
            this.form.hide();
            return new FormForScript(form);
        }
        public FormForScript size(int width, int height)
        {
            this.form.size(width, height);
            return new FormForScript(form);
        }
        public FormForScript position(int left, int top)
        {
            this.position(left, top);
            return new FormForScript(form);
        }
        public FormForScript refresh(string url = "")
        {
            this.form.refresh(url);
            return new FormForScript(form);
        }
        public FormForScript mode(int  status)
        {
            this.mode(status);
            return new FormForScript(form);
        }
    }
}