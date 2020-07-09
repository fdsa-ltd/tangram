using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms.VisualStyles;
using Tangram.Core.Event;

namespace Tangram.Core
{
    public class BuiltinTan : ITan
    {
        private readonly Features features;
        public BuiltinTan(Features features)
        {
            this.features = features;
            this.OnMessage = new FormEventCallback(this.Window_EventCallback);
        }
        public string Text { get; set; }

        public IntPtr Handle => IntPtr.Zero;

        public event FormEventCallback OnMessage;
        private void Window_EventCallback(FormMessage message)
        {
            switch (message.Type)
            {
                case FormMessageType.None:
                    break;
                case FormMessageType.Show:
                case FormMessageType.Close:
                case FormMessageType.Hide:
                case FormMessageType.Size:
                case FormMessageType.Site:
                case FormMessageType.Mode:
                case FormMessageType.Exec:
                case FormMessageType.Refresh:
                default:
                    break;
            }
        }

        public int  Init()
        {
            return 0;
        }

        public void Invoke(FormMessage message)
        {
            this.OnMessage(message);
        }
    }
}
