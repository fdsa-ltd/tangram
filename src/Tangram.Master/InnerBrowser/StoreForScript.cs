using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Tangram.Core;

namespace Tangram.IEBrowser
{
    [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisibleAttribute(true)]
    public class StoreForScript
    {
        public string get(string key)
        {
            var cacheValue = CacheHelper.Get(key);
            if (null == cacheValue)
            {
                return "undefined";
            }
            return cacheValue;
        }
        public void set(string key, string value, int timeout)
        {
            CacheHelper.Set(key, value, timeout);
        }
    }
}