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
    public class EventMessage
    {
        public string From;
        public MessageType Type;
        public object[] Data;
        public override string ToString()
        {
            try
            {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }
            catch (Exception ex)
            {
                FileManager.Loger.WriteLog("error", ex);
                return string.Empty;
            }
        } 
        public static EventMessage Parse(string content)
        {
            try
            {
                return JsonConvert.DeserializeObject<EventMessage>(content);
            }
            catch (Exception ex)
            {
                FileManager.Loger.WriteLog("error", ex);
                return null;
            }
        }
    }
    public enum MessageType
    {
        None = 0,
        //Inner Methods
        Exec,

        //Window Methods
        Show,
        Hide,
        Close,
        Site,
        Size,
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