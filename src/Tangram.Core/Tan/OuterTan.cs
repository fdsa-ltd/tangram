using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Tangram.Core.Event;

namespace Tangram.Core
{
    public class OuterTan : ITan
    {
        private readonly Features features;
        private readonly Process process;
        public OuterTan(Features features)
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
            var index = url.IndexOf('#');
            var tail = string.Empty;

            var name = this.features.Get("name");
            if (index > 0)
            {
                tail = url.Substring(index);
                url = url.Substring(0, index);
            }
            if (url.Contains("?"))
            {
                url += "&_title=" + name + tail;
            }
            else
            {
                url += "?_title=" + name + tail;
            }

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
          
            WindowHelper.SetWindowPos(this.Handle, 0, this.features.GetInt("left"), this.features.GetInt("top"), this.features.GetInt("width")
                , this.features.GetInt("height"), SWP_SHOWWINDOW);
            var parent = this.features.Get("parent");
            if (parent != null)
            {
                WindowHelper window = new WindowHelper(parent);
                //window.ShowChild(this);
            } 
            return this.process.Id;
        }
        private void Window_EventCallback(FormMessage message)
        {
            switch (message.Type)
            {
                case FormMessageType.None:
                    break;
                case FormMessageType.Show:
                    WindowHelper.SetFocus(this.Handle);
                    break;
                case FormMessageType.Close:
                    WindowHelper.SendMessage(this.Handle, 0, 0, 0);
                     break;
                case FormMessageType.Hide:
                    WindowHelper.SendMessage(this.Handle, 0, 0, 0);
                    break;
                case FormMessageType.Size:
                    WindowHelper.SendMessage(this.Handle, 0, 0, 0);
                    break;
                case FormMessageType.Site:
                    WindowHelper.SendMessage(this.Handle, 0, 0, 0);
                    break;
                case FormMessageType.Mode:
                    WindowHelper.SendMessage(this.Handle, 0, 0, 0);
                    break;
                case FormMessageType.Exec:
                    MessageBox.Show("");
                    break;
                case FormMessageType.Refresh:
                    MessageBox.Show("");
                    break;
                default:
                    break;
            }
        }


        public void Invoke(FormMessage message)
        {
            this.OnMessage(message);
        }
        const UInt32 SWP_SHOWWINDOW = 0x0040;
        const UInt32 SWP_HIDEWINDOW = 0x0080;
        [DllImport("user32.dll")]
        private static extern int SetParent(IntPtr hWndChild, IntPtr hWndParent);
    }
}
