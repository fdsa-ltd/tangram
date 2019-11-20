using Newtonsoft.Json;
using System;
namespace Tangram.Core
{
    public class WebMessage
    {
        public string from { get; set; }
        public string type { get; set; }
        public object[] data { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        public static WebMessage GetWebMessage(string content)
        {
            try
            {
                return JsonConvert.DeserializeObject<WebMessage>(content);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
