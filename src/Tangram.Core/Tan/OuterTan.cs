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
    public class OuterTan : ITan
    {
        private readonly Features features;
        public OuterTan(Features features)
        {
            this.features = features;
            this.OnMessage = new FormEventCallback(this.Window_EventCallback);
        }
        public string Text { get; set; }

        public IntPtr Handle => throw new NotImplementedException();

        public event FormEventCallback OnMessage;
        private void Window_EventCallback(FormMessage message)
        {
            switch (message.Type)
            {
                case FormMessageType.None:
                    break;
                case FormMessageType.Show:
                    RPCMessageManager.External.SendToAsync("", this.features.Get("name"));
                    break;
                case FormMessageType.Close:
                    RPCMessageManager.External.SendToAsync("", this.features.Get("name"));
                    break;
                case FormMessageType.Hide:
                    RPCMessageManager.External.SendToAsync("", this.features.Get("name"));
                    break;
                case FormMessageType.Size:
                    RPCMessageManager.External.SendToAsync("", this.features.Get("name"));
                    break;
                case FormMessageType.Site:
                    RPCMessageManager.External.SendToAsync("", this.features.Get("name"));
                    break;
                case FormMessageType.Mode:
                    break;
                case FormMessageType.Exec:
                    RPCMessageManager.External.SendToAsync("", this.features.Get("name"));
                    break;
                case FormMessageType.Refresh:
                    break;
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
