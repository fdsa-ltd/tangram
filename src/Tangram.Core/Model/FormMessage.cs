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
        public string To;
        public FormMessageType Type;
        public object[] Data;
        public static FormMessage Parse(string content)
        {
            try
            {
                return JsonConvert.DeserializeObject<FormMessage>(content);
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