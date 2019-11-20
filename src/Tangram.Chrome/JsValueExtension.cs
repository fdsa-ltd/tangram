using Tangram.Blink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Tangram.Utility;
using Tangram.Core;

namespace Tangram.Chrome
{

    internal static class JsValueExtension
    {
        public static string GetString(this JsValue[] value, IntPtr es, int index)
        {
            if (index > value.Length - 1)
            {
                return string.Empty;
            }
            return value[index].ToString(es);
        }
        public static int GetInt(this JsValue[] value, IntPtr es, int index)
        {
            if (index > value.Length - 1)
            {
                return 0;
            }
            return value[index].ToInt32(es);
        }
        public static Dictionary<string, string> GetObj(this JsValue[] value, IntPtr es, int index)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (index > value.Length - 1)
            {
                return dict;
            }
            var keys = value[index].GetKeys(es);
            if (keys == null)
            {
                return dict;
            }
            foreach (var item in keys)
            {
                dict.Add(item, value[index].GetProp(es, item).ToString(es));
            }
            return dict;
        }

    }
}