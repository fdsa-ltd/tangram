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
        public string To;
        public GlobalMessageType Type;
        public object[] Data;

        public static GlobalMessage Parse(string content)
        {
            try
            {
                return JsonConvert.DeserializeObject<GlobalMessage>(content);
            }
            catch (Exception ex)
            {
                FileManager.Loger.WriteLog("error", ex);
                return null;
            }
        }
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
    }
    public enum GlobalMessageType
    {
        Open,
        Find,
        Invoke,
        StoreGet,
        StoreSet,
        NQ,
        MQ,
    } 
}