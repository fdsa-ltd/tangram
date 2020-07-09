using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tangram.Core.Event;

namespace Tangram.Core
{
    public class PluginTan : ITan
    {
        private readonly Form form;
        private readonly Form mainForm;
        private readonly Features features;

        private readonly System.Diagnostics.Process program;
        public PluginTan(Features features, Form form)
        {
            this.form = new Form();
            this.mainForm = form;
            this.OnMessage = new FormEventCallback(this.Window_EventCallback);
            this.features = features;
            this.program = new System.Diagnostics.Process();
            this.form.FormClosed += Form_FormClosed;
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            var fm = new FormMessage()
            {
                From = this.features.Get("name"),
                To = this.features.Get("name"),
                Type = FormMessageType.Close,
            };
            //this.OnMessage.Invoke(fm);
        }
        const UInt32 SWP_SHOWWINDOW = 0x0040;
        const UInt32 SWP_HIDEWINDOW = 0x0080;
        public int Init()
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
            this.program.StartInfo.FileName = fileName;
            this.program.StartInfo.Arguments = args;
            this.program.StartInfo.WorkingDirectory = workspace;
        this.program.StartInfo.WindowStyle =  ProcessWindowStyle.Maximized;
    
            this.program.Start();
            //this.program.WaitForInputIdle();
            //加载当前窗口样式
            this.form.FormBorderStyle = FormBorderStyle.None;
            //this.TransparencyKey = Color.Blue;
            //this.BackColor = Color.Blue;
            var countDown = 1000;
            var pid = this.program.Id;
            var hWnd = this.program.MainWindowHandle;
            while (hWnd == IntPtr.Zero)
            {
                if (countDown < 0)
                {
                    break;
                }
                countDown--;
                Thread.Sleep(100);
                hWnd = System.Diagnostics.Process.GetProcessById(pid).MainWindowHandle;
            }
            SetParent(hWnd, this.form.Handle);

            //改变尺寸
            this.form.Size = new Size(this.features.GetInt("width"), this.features.GetInt("height"));
            this.form.Location = new Point(this.features.GetInt("left"), this.features.GetInt("top"));
            this.form.Text = name;
            this.form.BackColor = Color.Red;
            WindowHelper.SetWindowPos(hWnd, 0, this.form.Location.X, this.form.Location.Y,this.form.Size.Width,this.form.Size.Height, SWP_SHOWWINDOW);
            var parent = ScreenManager.External.Find(this.features.Get("parent"));
            if (parent != null)
            {
                WindowHelper window = new WindowHelper(parent.Handle);
                window.ShowChild(this.form);
            }
            else
            {
                this.form.Show();
            }


            return pid;
        }
        private void Window_EventCallback(FormMessage message)
        {
            switch (message.Type)
            {
                case FormMessageType.None:
                    break;
                case FormMessageType.Show:
                    var parent = ScreenManager.External.Find(message.Data.GetString(0));
                    if (parent != null)
                    {
                        WindowHelper window = new WindowHelper(parent.Handle);
                        window.ShowChild(this.form);
                    }
                    else
                    {
                        this.form.Hide();
                        this.form.Show(null);
                    }
                    break;
                case FormMessageType.Close:
                    this.form.Close();
                    break;
                case FormMessageType.Hide:
                    this.form.Hide();
                    break;
                case FormMessageType.Size:
                    var width = message.Data.GetInt(0);
                    var height = message.Data.GetInt(1);
                    this.form.Size = new Size(width, height);
                    break;
                case FormMessageType.Site:
                    var left = message.Data.GetInt(0);
                    var right = message.Data.GetInt(1);
                    this.form.Location = new Point(left, right);
                    break;
                case FormMessageType.Mode:
                    break;
                case FormMessageType.Exec:
                    var channel = this.features.Get("channel", "rpc");
                    switch (channel)
                    {
                        case "rpc":
                            RPCMessageManager.External.SendToAsync(message.Data.GetString(0), message.From);
                            break;
                        case "ipc":
                            IPCMessageManager.Send(this.program.MainWindowHandle, MessageType.Exec, "", message.Data);
                            break;
                        default:

                            break;
                    }
                    break;
                case FormMessageType.Refresh:
                    break;
                default:
                    break;
            }
        }


        public IntPtr Handle => this.form.Handle;

        public event FormEventCallback OnMessage;

        [DllImport("user32.dll")]
        private static extern int SetParent(IntPtr hWndChild, IntPtr hWndParent);

        public void Invoke(FormMessage message)
        {
            this.OnMessage.Invoke(message);
        }
    }
}
