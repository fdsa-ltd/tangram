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
    public class FormForScript : JsObject
    {
        private IFormBrowser form;
        public FormForScript(IFormBrowser form)
        {
            this.form = form;

        }
        public override JsValue jsGetProperty(IntPtr jsExecState, JsValue obj, string propertyName)
        {
            switch (propertyName.ToLower())
            {
                case "show":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.show));
                case "size":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.size));
                case "close":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.close));
                case "hide":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.hide));
                case "position":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.position));
                case "refresh":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.refresh));
                case "mode":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.mode));
                case "exec":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.exec));
                default:
                    return base.jsGetProperty(jsExecState, obj, propertyName);
            }
        }

        long show(IntPtr jsExecState, long obj, JsValue[] args)
        {
            this.form.show(args.GetString(jsExecState, 0));
            return JsValue.ObjectValue(jsExecState, this);
        }
        long size(IntPtr jsExecState, long obj, JsValue[] args)
        {
            this.form.size(args.GetInt(jsExecState, 0), args.GetInt(jsExecState, 1));
            return JsValue.ObjectValue(jsExecState, this);
        }
        long close(IntPtr jsExecState, long obj, JsValue[] args)
        {
            this.form.close();
            return JsValue.ObjectValue(jsExecState, this);
        }
        long hide(IntPtr jsExecState, long obj, JsValue[] args)
        {
            this.form.hide();
            return JsValue.ObjectValue(jsExecState, this);
        }
        long position(IntPtr jsExecState, long obj, JsValue[] args)
        {
            this.form.position(args.GetInt(jsExecState, 0), args.GetInt(jsExecState, 1));
            return JsValue.ObjectValue(jsExecState, this);
        }
        long refresh(IntPtr jsExecState, long obj, JsValue[] args)
        {
            this.form.refresh(args.GetString(jsExecState, 0));
            return JsValue.ObjectValue(jsExecState, this);
        }

        private long exec(IntPtr jsExecState, long obj, JsValue[] args)
        {
            var script = args.GetString(jsExecState, 0);
            this.form.exec(script);
            return JsValue.ObjectValue(jsExecState, this);
        }
        private long mode(IntPtr jsExecState, long obj, JsValue[] args)
        {
            this.form.mode(args.GetInt(jsExecState, 0));
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