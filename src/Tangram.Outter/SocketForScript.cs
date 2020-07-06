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
    public class SocketForScript
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
        public void open()
        {
            SocketManager.External.Open(this.url);
        }
        public void on(string eventName)
        {
            if (this.options.Count <= 0)
            {
                SocketManager.External.On(this.url, this.name, eventName, this.name);
            }
            else
            {
                SocketManager.External.On(this.url, this.name, eventName, this.options.Values.ToArray());
            }
        }
        public void close()
        {
            SocketManager.External.Close(this.url);
        }
        public void send(string eventString, string data)
        {
            SocketManager.External.Send(this.url, eventString, data);
        }
    }
  
}