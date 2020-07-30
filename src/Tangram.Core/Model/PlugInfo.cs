using Newtonsoft.Json;
using System;
namespace Tangram.Core
{
    public class PlugInfo
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string[] Arguments { get; set; }
        public string[] Settings { get; set; }
        public PlugType Type { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        public static PlugInfo GetPlugInfo(string content)
        {
            try
            {
                return JsonConvert.DeserializeObject<PlugInfo>(content);
            }
            catch (Exception ex)
            {
                FileManager.Loger.WriteLog("error", ex);
                return null;
            }
        }
    }
    public enum PlugType
    {
        /// <summary>
        /// 默认的主应用程序，负责全局的事件侦听
        /// </summary>
        Default = -100,
        /// <summary>
        /// 内置受控窗口使用windows 消息通知
        /// </summary>
        Builtin = -1,
        /// <summary>
        /// 外置使用受控窗口的应用程序
        /// </summary>
        Plugin = 0,
        /// <summary>
        /// 外部的不受控窗口的应用程序
        /// </summary>
        Outer = 1,
    }
}
