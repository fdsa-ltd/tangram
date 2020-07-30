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
    public class DefaultTan : ITan
    {
        readonly Form form;
        public DefaultTan(Form form)
        {
            this.form = form;
            this.OnMessage = new FormEventCallback(this.Window_EventCallback);
            this.form.FormClosed += Form_FormClosed;
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            //todo
        }

        public int InitProcess()
        {
            return Process.GetCurrentProcess().Id;
        }
        private void Window_EventCallback(FormMessage message)
        {
            switch (message.Type)
            {
                case FormMessageType.None:
                    break;
                case FormMessageType.Show:
                    this.form.Show();
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
                    MessageBox.Show("当前窗口不支持exec方法");
                    break;
                case FormMessageType.Refresh:
                    MessageBox.Show("当前窗口不支持refresh方法");
                    break;
                default:
                    break;
            }
        }


        public IntPtr Handle => this.form.Handle;

        public event FormEventCallback OnMessage;

        public void Invoke(FormMessage message)
        {
            this.OnMessage.Invoke(message);
        }
    }
}
