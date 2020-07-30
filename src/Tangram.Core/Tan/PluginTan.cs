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
using System.Windows.Forms.VisualStyles;
using Tangram.Core.Event;

namespace Tangram.Core
{
    public class PluginTan : ITan
    {
        private readonly Features features;
        private readonly Process process;

        private readonly Form form;
        public PluginTan(Features features, Form form)
        {
            this.features = features;
            this.process = new Process();
            this.OnMessage = new FormEventCallback(this.Window_EventCallback);
            this.form = new Form();
            this.form.FormBorderStyle = FormBorderStyle.None;
            this.form.StartPosition = FormStartPosition.Manual;
            this.form.WindowState = FormWindowState.Normal;
            this.form.FormClosed += Form_FormClosed;
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
            this.form.Text = name;

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
            this.process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
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
            //加载当前窗口样式
 
            SetParent(this.Handle, this.form.Handle);
            var parent = this.features.Get("parent");
            if (parent != null)
            {
                WindowHelper window = new WindowHelper(parent);
                window.ShowChild(this.form);
            }
            else
            {
                this.form.Show();
            }
            //改变尺寸
            this.form.Size = new Size(this.features.GetInt("width"), this.features.GetInt("height"));
            this.form.Location = new Point(this.features.GetInt("left"), this.features.GetInt("top"));
            //WindowHelper.SetWindowPos(this.Handle, 0, 0, 0, 0, 0, SWP_SHOWWINDOW);

            return this.process.Id;
        }
        private void Window_EventCallback(FormMessage message)
        {
            switch (message.Type)
            {
                case FormMessageType.None:
                    break;
                case FormMessageType.Show:
                    var parent = message.Data.GetString(0);
                    if (string.IsNullOrEmpty(parent))
                    {
                        this.form.Hide();
                        this.form.Show(null);
                    }
                    else
                    {
                        WindowHelper window = new WindowHelper(parent);
                        window.ShowChild(this.form);
                    }
                    break;
                case FormMessageType.Close:
                    this.process.Kill(true);
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
                    var status = message.Data.GetInt(0);
                    switch (status)
                    {
                        case 0:
                            this.form.Show();
                            break;
                        case 1:
                            this.form.ShowDialog();
                            break;
                        default:
                            this.form.TopLevel = true;
                            this.form.TopMost = true;
                            break;
                    }
                    break;
                case FormMessageType.Exec:
                    RPCMessageManager.External.Send(message.To, MessageType.Exec, message.Data.GetString(0));
                    break;
                case FormMessageType.Refresh:
                    RPCMessageManager.External.Send(message.To, MessageType.Refresh, message.Data.GetString(0));
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
        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            var message = new FormMessage()
            {
                From = this.features.Get("name"),
                To = this.features.Get("name"),
                Type = FormMessageType.Close,
            };
            //this.OnMessage.Invoke(fm);
        }

    }
}
