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

    public class StoreForScript : JsObject
    {
        public override JsValue jsGetProperty(IntPtr jsExecState, JsValue obj, string propertyName)
        {
            switch (propertyName)
            {
                case "get":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.get));
                case "set":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.set));
                default:
                    return base.jsGetProperty(jsExecState, obj, propertyName);

            }
        }
        long set(IntPtr jsExecState, long obj, JsValue[] args)
        {
            CacheHelper.Set(args.GetString(jsExecState, 0), args.GetString(jsExecState, 1), ParseUtil.ParseByDefault(args.GetString(jsExecState, 2), 7200));
            return JsValue.BoolValue(true);
        }
        long get(IntPtr jsExecState, long obj, JsValue[] args)
        {
            var cacheValue = CacheHelper.Get(args.GetString(jsExecState, 0));
            if (null == cacheValue)
            {                
                return JsValue.StringValue(jsExecState, "undefined");
            }            
            return JsValue.StringValue(jsExecState, cacheValue);
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