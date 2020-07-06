using Newtonsoft.Json;
using System;
namespace Tangram.Core
{
    public class WebMessage
    {
        public string to { get; set; }
        public string from { get; set; }
        public string type { get; set; }
        public object[] data { get; set; }
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
        public static WebMessage Parse(string content)
        {
            try
            {
                return JsonConvert.DeserializeObject<WebMessage>(content);
            }
            catch (Exception ex)
            {
                FileManager.Loger.WriteLog("error", ex);
                return null;
            }
        }
    }
}
