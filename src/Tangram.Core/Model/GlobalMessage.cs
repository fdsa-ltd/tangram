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
    public class GlobalMessage
    {
        public string From;
        public GlobalMessageType Type;
        public object[] Data;

        public static GlobalMessage Get(string data)
        {
            return JsonConvert.DeserializeObject<GlobalMessage>(data);
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public enum GlobalMessageType
    {
        None = 0,
        Exec,
        Open,
        Close,
        Find,
        StoreGet,
        StoreSet,
        NQ,
        MQ,
    }
 

}