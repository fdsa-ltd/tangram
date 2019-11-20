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
using Tangram.Core;

namespace Tangram.Chrome
{
    public class GlobalForScript : JsObject
    {
        private IFormBrowser form;
        public GlobalForScript(IFormBrowser form)
        {
            this.form = form;

        }
        public long js_load(IntPtr es, IntPtr param)
        {
            return JsValue.ObjectValue(es, this);
        }
        public override JsValue jsGetProperty(IntPtr jsExecState, JsValue obj, string propertyName)
        {
            switch (propertyName.ToLower())
            {
                case "find":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.find_form));
                case "open":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.open_form));
                case "socket":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.socket));
                case "state":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.state));
                case "store":
                    return JsValue.ObjectValue(jsExecState, new StoreForScript());
                case "exec":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.exec_script));
                case "invoke":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.invoke_script));
                default:
                    return base.jsGetProperty(jsExecState, obj, propertyName);
            }
        }
        long find_form(IntPtr jsExecState, long obj, JsValue[] args)
        {
            var formName = args.GetString(jsExecState, 0);
            if (string.IsNullOrEmpty(formName))
            {
                return JsValue.ObjectValue(jsExecState, new FormForScript(this.form));
            }
            if (ScreenManager.External.Find(formName) == null)
            {
                return JsValue.ObjectValue(jsExecState, null);
            }
            return JsValue.ObjectValue(jsExecState, new FormForScript(ScreenManager.External.Find(formName)));
        }
        long open_form(IntPtr jsExecState, long obj, JsValue[] args)
        {
            string url = args.GetString(jsExecState, 0);
            string name = args.GetString(jsExecState, 1);
            string type = args.GetString(jsExecState, 2);
            string features = args.GetString(jsExecState, 3);
            string parent = args.GetString(jsExecState, 4);
            ScreenManager.External.Open(url, name, type, features, parent);
            return JsValue.ObjectValue(jsExecState, new FormForScript(form));
        }


        long socket(IntPtr jsExecState, long obj, JsValue[] args)
        {
            string name = this.form.Title;
            string url = args.GetString(jsExecState, 0);
            Dictionary<string, string> options = SocketOptions.options(args.GetString(jsExecState, 1));
            return JsValue.ObjectValue(jsExecState, new SocketForScript(name, url, options));
        }

        long state(IntPtr jsExecState, long obj, JsValue[] args)
        {
            string name = this.form.Title;
            return JsValue.ObjectValue(jsExecState, new StateForScript(name));
        }
        long exec_script(IntPtr jsExecState, long obj, JsValue[] args)
        {
            var name = args.GetString(jsExecState, 0);
            var script = args.GetString(jsExecState, 1);
            ScreenManager.External.Exec(name, script);
            return 0;
        }

        long invoke_script(IntPtr jsExecState, long obj, JsValue[] args)
        {
            var name = args.GetString(jsExecState, 0);
            if (string.IsNullOrEmpty(name))
            {
                name = this.form.Title;
            }
            var method = args.GetString(jsExecState, 1);
            ScreenManager.External.Invoke(name, method, args.Skip(2).Select(m => m.ToString(jsExecState)).ToArray());
            return 0;
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