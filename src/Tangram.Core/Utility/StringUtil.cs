using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tangram.Core
{
    public static class StringUtil
    {
        public static int GetInt(this object[] value, int index)
        {
            if (index > value.Length - 1)
            {
                return 0;
            }
            if (value[index]==null)
            {
                return 0;
            }
            if (value[index] is int)
            {
                return (int)value[index];
            }
            int result;
            if (int.TryParse(value[index].ToString(), out result))
            {
                return result;
            }
            return 0;
        }
        public static bool GetBool(this object[] value, int index)
        {
            if (index > value.Length - 1)
            {
                return false;
            }
            if (value[index] is bool)
            {
                return (bool)value[index];
            }
            bool result;
            if (bool.TryParse(value[index].ToString(), out result))
            {
                return result;
            }
            return false;
        }
        public static string GetString(this object[] value, int index)
        {
            if (index > value.Length - 1)
            {
                return string.Empty;
            }
            if (value[index]==null)
            {
                return string.Empty;
            }
            return value[index].ToString();
        }
        public static Dictionary<string, string> SplitString(string settingString)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string item in settingString.Split(',', ';').Select(m => m.Trim()).Where(m => !string.IsNullOrEmpty(m)))
            {
                var kv = item.Split('=', ':').Select(m => m.Trim()).Where(m => !string.IsNullOrEmpty(m)).ToArray();
                if (kv != null && kv.Length == 2)
                {
                    if (dict.ContainsKey(kv[0]))
                    {
                        dict[kv[0]] = kv[1];
                    }
                    else
                    {
                        dict.Add(kv[0], kv[1]);
                    }
                }
            }
            return dict;
        }
        /// <summary>
        /// 获得字符串中括号中间得值
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetMidValue(string text)
        {
            Regex rg = new Regex(@"\(([^)]*)\)");
            return rg.Match(text).Result("$1");
        }

        public static string GetQueryString(string url, string name)
        {
            var reg = new Regex(@"(^|&)?(" + name + ")=([^&]+)(&|$)?", RegexOptions.Compiled);
            foreach (Match item in reg.Matches(url))
            {
                return item.Result("$3");
            }
            return string.Empty;
        }
        /// <summary>
        /// 分析 url 字符串中的参数信息
        /// </summary>
        /// <param name="url">输入的 URL</param>
        /// <param name="baseUrl">输出 URL 的基础部分</param>
        /// <param name="nvc">输出分析后得到的 (参数名,参数值) 的集合</param>
        private static NameValueCollection ParseUrl(string url)
        {
            NameValueCollection collection = new NameValueCollection();
            var questionMarkIndex = url.IndexOf('?');
            string ps = url.Substring(questionMarkIndex + 1);
            // 开始分析参数对  
            Regex re = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
            MatchCollection mc = re.Matches(ps);
            foreach (Match m in mc)
            {
                collection.Add(m.Result("$2").ToLower(), m.Result("$3"));
            }
            return collection;
        }
        public static DateTime ParseByDefault(this string input, DateTime defaultvalue)
        {
            return input.ParseStringToType<DateTime>(delegate (string e)
            {
                return Convert.ToDateTime(input);
            }, defaultvalue);
        }

        public static decimal ParseByDefault(this string input, decimal defaultvalue)
        {
            return input.ParseStringToType<decimal>(delegate (string e)
            {
                return Convert.ToDecimal(input);
            }, defaultvalue);
        }

        public static double ParseByDefault(this string input, double defaultvalue)
        {
            return input.ParseStringToType<double>(delegate (string e)
            {
                return Convert.ToDouble(input);
            }, defaultvalue);
        }

        public static int ParseByDefault(this string input, int defaultvalue)
        {
            return input.ParseStringToType<int>(delegate (string e)
            {
                return Convert.ToInt32(input);
            }, defaultvalue);
        }

        public static long ParseByDefault(this string input, long defaultvalue)
        {
            return input.ParseStringToType<long>(delegate (string e)
            {
                return Convert.ToInt64(input);
            }, defaultvalue);
        }

        public static float ParseByDefault(this string input, float defaultvalue)
        {
            return input.ParseStringToType<float>(delegate (string e)
            {
                return Convert.ToSingle(input);
            }, defaultvalue);
        }

        public static float ParseByDefault(this string input, short defaultvalue)
        {
            return input.ParseStringToType<short>(delegate (string e)
            {
                return Convert.ToInt16(input);
            }, defaultvalue);
        }

        public static string ParseByDefault(this string input, string defaultvalue)
        {
            if (string.IsNullOrEmpty(input))
            {
                return defaultvalue;
            }
            return input;
        }
        public static string CheckSum(this string input)
        {
            long h = 0;
            foreach (var item in input)
            {
                //h = 31 * h + item;
                h = (h << 5) - h + item;
            }
            var code = Convert.ToString(h, 16).ToUpper();

            while (code.Length < 16)
            {
                code = "0" + code;
            }
            return code;
        }
        public static string Fragment(this string code, int step = 0)
        {
            if (step == 0)
            {
                return code;
            }
            var length = code.Length / step;
            var left = code.Length % step;
            List<string> list = new List<string>(length + 1);
            for (int i = 0; i < length; i++)
            {
                list.Add(code.Substring(i * step, step));
            }
            if (left > 0)
            {
                list.Add(code.Substring(length * step));
            }
            return string.Join("-", list);
        }

        private static T ParseStringToType<T>(this string input, Func<string, T> action, T defaultvalue) where T : struct
        {
            if (string.IsNullOrEmpty(input))
            {
                return defaultvalue;
            }
            try
            {
                return action(input);
            }
            catch
            {
                return defaultvalue;
            }
        }
    }
}
