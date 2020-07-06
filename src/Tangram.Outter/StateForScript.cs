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

namespace Tangram.Core
{
    [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisibleAttribute(true)]
    public class StateForScript
    {
        private readonly string form;
        public StateForScript(string form)
        {
            this.form = form;
        }
        public void send(string formName, string eventName, string data)
        {
            LocalEventManager.StateEventManager.Send(formName, eventName, data);
        }
        public void on(string eventName, string callBack)
        {
            LocalEventManager.StateEventManager.On(this.form, eventName);
        }
    }


}