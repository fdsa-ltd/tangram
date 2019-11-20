using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Tangram.Core
{
    public class FormInfo
    {
        public string Name { get; set; }
        public IntPtr Handle { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static FormInfo FromString(string data)
        {
            return JsonConvert.DeserializeObject<FormInfo>(data);
        }
    }
}
