using Tangram.Blink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Tangram.Utility;
using Tangram.Core;

namespace Tangram.Chrome
{
    public class StateForScript : JsObject
    {
        private string formName;
        private List<string> eventList = new List<string>();
        public StateForScript(string formName)
        {
            this.formName = formName;
        }
        public override JsValue jsGetProperty(IntPtr jsExecState, JsValue obj, string propertyName)
        {
            switch (propertyName.ToLower())
            {
                case "send":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.send));
                case "on":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.on));
                default:
                    return base.jsGetProperty(jsExecState, obj, propertyName);
            }
        }
        long send(IntPtr jsExecState, long obj, JsValue[] args)
        {
            var formName = args.GetString(jsExecState, 0);
            var eventName = args.GetString(jsExecState, 1);
            var data = args.GetString(jsExecState, 2);
            LocalEventManager.StateEventManager.Send(formName, eventName, data);            
            return JsValue.ObjectValue(jsExecState, this);
        }
        long on(IntPtr jsExecState, long obj, JsValue[] args)
        {            
            var eventName = args.GetString(jsExecState, 0);            
            LocalEventManager.StateEventManager.On(this.formName, eventName);
            return JsValue.ObjectValue(jsExecState, this);
        }

        public override void jsFinalize()
        {
            base.jsFinalize();
        }
        public override bool jsSetProperty(IntPtr jsExecState, JsValue obj, string propertyName, JsValue value)
        {
            return base.jsSetProperty(jsExecState, obj, propertyName, value);
        }
    }
}