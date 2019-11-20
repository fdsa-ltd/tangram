using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tangram.Core;

namespace Tangram.Core
{
    public class Features : Dictionary<string, string>
    {
        private Dictionary<string, string> defaultFeatures = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase) {
            { "top","0"},
            { "left","0"},
        };
        public Features(IDictionary<string, string> dictionary) : base(dictionary, StringComparer.CurrentCultureIgnoreCase)
        { }
        public Features(string features) : this(StringUtil.SplitString(features))
        { }
        public string Get(string key, string defaultValue = "")
        {
            if (this.ContainsKey(key))
            {
                return this[key];
            }
            if (this.defaultFeatures.ContainsKey(key))
            {
                return this.defaultFeatures[key];
            }
            return defaultValue;
        }
        public int GetInt(string key, int defaultValue = 0)
        {
            var value = this.Get(key);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            int result;
            if (int.TryParse(value, out result))
            {
                return result;
            }
            return defaultValue;
        }
    }
}
