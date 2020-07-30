using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Tangram.Core.Event;

namespace Tangram.Core
{
    public class BuiltinTan : ITan
    {
        private readonly Features features;
        private readonly Process process;
        public BuiltinTan(Features features)
        {
            this.features = features;
            this.process = new Process();
            this.OnMessage = new FormEventCallback(this.Window_EventCallback);
        }
        public IntPtr Handle { get; set; }

        public event FormEventCallback OnMessage;

        public int InitProcess()
        {
            var fileName = this.features.Get("fileName");
            var url = this.features.Get("url");
            var name = this.features.Get("name");
            var args = this.features.Get("args").Replace("{url}", url).Replace("{name}", name);
            var workspace = this.features.Get("workspace", Application.StartupPath);
            this.process.StartInfo.FileName = fileName;
            this.process.StartInfo.Arguments = args;
            this.process.StartInfo.WorkingDirectory = workspace;
            this.process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            this.process.Start();
            this.process.WaitForInputIdle();
            this.Handle = this.process.MainWindowHandle;

            var countDown = 1000;
            while (this.Handle == IntPtr.Zero)
            {
                if (countDown < 0)
                {
                    break;
                }
                countDown--;
                Thread.Sleep(100);
                this.Handle = Process.GetProcessById(this.process.Id).MainWindowHandle;
            }
            //改变尺寸
            IPCMessageManager.Send(this.Handle, MessageType.Size, this.features.Get("width"), this.features.Get("height"));
            IPCMessageManager.Send(this.Handle, MessageType.Site, this.features.GetInt("left"), this.features.GetInt("top"));
            IPCMessageManager.Send(this.Handle, MessageType.Show, this.features.Get("parent"));
            return this.process.Id;
        }
        private void Window_EventCallback(FormMessage message)
        {
            switch (message.Type)
            {
                case FormMessageType.None:
                    break;
                case FormMessageType.Show:
                    IPCMessageManager.Send(this.Handle, MessageType.Show, this.features.Get("name"), message.Data.GetString(0));
                    break;
                case FormMessageType.Close:
                    IPCMessageManager.Send(this.Handle, MessageType.Close, this.features.Get("name"));
                    break;
                case FormMessageType.Hide:
                    IPCMessageManager.Send(this.Handle, MessageType.Hide, this.features.Get("name"));
                    break;
                case FormMessageType.Size:
                    IPCMessageManager.Send(this.Handle, MessageType.Size, this.features.Get("name"), message.Data.GetString(0), message.Data.GetString(1));
                    break;
                case FormMessageType.Site:
                    IPCMessageManager.Send(this.Handle, MessageType.Site, this.features.Get("name"), message.Data.GetString(0), message.Data.GetString(1));
                    break;
                case FormMessageType.Mode:
                    IPCMessageManager.Send(this.Handle, MessageType.Mode, this.features.Get("name"), message.Data.GetString(0));
                    break;
                case FormMessageType.Exec:
                    IPCMessageManager.Send(this.Handle, MessageType.Exec, this.features.Get("name"), message.Data.GetString(0));
                    break;
                case FormMessageType.Refresh:
                    IPCMessageManager.Send(this.Handle, MessageType.Refresh, this.features.Get("name"), message.Data.GetString(0));
                    break;
                default:
                    break;
            }
        }


        public void Invoke(FormMessage message)
        {
            this.OnMessage(message);
        }
    }
}
