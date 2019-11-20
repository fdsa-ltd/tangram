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

    public class FormMessage
    {
        public string From;
        public FormMessageType Type;
        public object[] Data;
        public static FormMessage Get(string data)
        {
            return JsonConvert.DeserializeObject<FormMessage>(data);
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public enum FormMessageType
    {
        None = 0,
        Show,
        Close,
        Hide,
        Size,
        Site,
        Mode,
        Exec,
        Refresh,
    }
}