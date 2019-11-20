using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Tangram.Core;

namespace Tangram.BuiltinBrowser
{
    public class StoreForScript
    {
        public string get(string key)
        {
            var cacheValue = CacheHelper.Get(key);
            if (null == cacheValue)
            {
                return "undefined";
            }
            return CacheHelper.Get(key);
        }
        public void set(string key, string value, int timeout)
        {
            CacheHelper.Set(key, value, timeout);
        }


    }

}