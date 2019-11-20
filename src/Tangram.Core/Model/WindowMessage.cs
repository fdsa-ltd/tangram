using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks; 

namespace Tangram.Core
{
    public class WindowMessage
    {
        public string To;
        public WindowMessageType Type;
        public object[] Data;
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        public static WindowMessage Parse(string content)
        {
            try
            {
                return JsonConvert.DeserializeObject<WindowMessage>(content);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
    public enum WindowMessageType
    {
        None = 0,
        Show,
        Hide,
        Close,
        Site,
        Size,
        Exec,
        Refresh,
        Mode,
        //Global methods
        GlobalOpen,
        GlobalFind,
        GlobalExec,
        GlobalInvoke,
        GlobalMQ,
        GlobalIO,

    }
}