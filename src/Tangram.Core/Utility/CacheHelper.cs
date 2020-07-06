using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangram.Core
{
    public class CacheItem
    {
        public long TimeOut { get; set; }
        public string Content { get; set; }
    }
    public class CacheEntity : CacheItem
    {
        public string Name { get; set; }
    }
    /// <summary>
    /// 功能描述：CacheHelper  
    /// </summary>
    public class CacheHelper
    {
        static Dictionary<string, CacheItem> cache = new Dictionary<string, CacheItem>();
        static CacheHelper()
        {
            var list = JsonConvert.DeserializeObject<List<CacheEntity>>(File.ReadAllText("cache.json"));
            foreach (var item in list)
            {
                if (item.TimeOut > DateTime.Now.Ticks)
                {
                    cache.Add(item.Name, item);
                }
            }
        }
        public static string Get(string key)
        {
            try
            {
                if (!cache.ContainsKey(key))
                {
                    return string.Empty;
                }
                var item = cache[key];
                if (item.TimeOut < DateTime.Now.Ticks)
                {
                    return string.Empty;
                }
                return item.Content;
            }
            catch (Exception ex)
            {
                FileManager.Loger.WriteLog("error", ex);
                return string.Empty;
            }
        }
        public static void Set(string key, string value, double seconds = 600)
        {
            try
            {
                CacheEntity entity = new CacheEntity() { Name = key, Content = value, TimeOut = DateTime.Now.AddSeconds(seconds).Ticks };
                if (cache.ContainsKey(key))
                {
                    cache[key] = entity;
                }
                else
                {
                    cache.Add(key, entity);
                }
            }
            catch (Exception ex)
            {
                FileManager.Loger.WriteLog("error", ex);
            }
            save();
        }
        static Task save()
        {
            return Task.Run(() =>
            {
                var list = cache.Values;
                File.WriteAllText("cache.json", JsonConvert.SerializeObject(list));
                //foreach (var item in cache)
                //{
                //}
            });
        }

    }
}
