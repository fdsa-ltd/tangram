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
    public class SocketForScript : JsObject
    {
        private string url;
        private string name;
        private Dictionary<string, string> options;
        public SocketForScript(string name, string url, Dictionary<string, string> options)
        {
            this.name = name;
            this.options = options;
            var queryString = string.Empty;
            //if (options.Count > 0)
            //{
            //    SortedDictionary<string, string> sd = new SortedDictionary<string, string>(options);
            //    queryString = string.Join("&", sd.Select(m => string.Format("{0}={1}", m.Key, m.Value)));
            //    this.url = url + "?" + queryString;
            //}
            //else
            //{
            //    this.url = url;
            //}
            if (options.Count > 0)
            {
                SortedDictionary<string, string> sd = new SortedDictionary<string, string>(options);
                queryString = string.Join("&", sd.Select(m => string.Format("{0}={1}", m.Key, m.Value)));
            }
            this.url = url;
            SocketManager.External.Init(this.url, queryString);
        }

        public override JsValue jsGetProperty(IntPtr jsExecState, JsValue obj, string propertyName)
        {
            switch (propertyName.ToLower())
            {
                case "open":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.open));
                case "on":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.on));
                case "close":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.close));
                case "send":
                    return JsValue.FunctionValue(jsExecState, new jsCallAsFunction(this.send));
                default:
                    return base.jsGetProperty(jsExecState, obj, propertyName);
            }
        }
        long open(IntPtr jsExecState, long obj, JsValue[] args)
        {
            SocketManager.External.Open(this.url);

            return JsValue.ObjectValue(jsExecState, this);
        }
        long on(IntPtr jsExecState, long obj, JsValue[] args)
        {
            var eventName = args.GetString(jsExecState, 0);
            if (this.options.Count <= 0)
            {
                SocketManager.External.On(this.url, this.name, eventName, this.name);
            }
            else
            {
                SocketManager.External.On(this.url, this.name, eventName, this.options.Values.ToArray());
            }
            return JsValue.ObjectValue(jsExecState, this);
        }
        long close(IntPtr jsExecState, long obj, JsValue[] args)
        {
            SocketManager.External.Close(this.url);
            return JsValue.ObjectValue(jsExecState, this);
        }
        long send(IntPtr jsExecState, long obj, JsValue[] args)
        {
            var eventString = args.GetString(jsExecState, 0);
            var data = args.GetString(jsExecState, 1);
            SocketManager.External.Send(this.url, eventString, data);
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