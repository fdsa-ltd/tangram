using Tangram.Blink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Tangram.Core;
using Tangram.Utility;

namespace Tangram.Chrome
{
    public class BlinkBrowser : BrowserForm, IFormBrowser
    {

        private readonly WebView webBrowser;
        public event EventCallback OnMessage;

        public string Title
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = this.Name = value;
            }
        }
        #region Implement Queue
        private readonly Queue<String> mq;
        private readonly Timer timer;
        private void PushJS(string script)
        {
            this.mq.Enqueue(script);
            this.timer.Enabled = true;
            this.timer.Start();
        }
        #endregion
        public BlinkBrowser()
        {
            this.webBrowser = new WebView();
            this.FormClosing += Form_FormClosing;
            this.webBrowser.Bind(this.panel1, false);
            GlobalForScript tg = new GlobalForScript(this);
            JsValue.BindGetter("external", new wkeJsNativeFunction(tg.js_load));
            this.webBrowser.DragDropEnable = false;
            this.webBrowser.DragEnable = false;
            this.webBrowser.CookieEnabled = false;
            this.webBrowser.NavigationToNewWindowEnable = true;
            this.webBrowser.TouchEnable = false;
            this.webBrowser.SetCspCheckEnable(false);
            this.webBrowser.NpapiPluginsEnabled = false;
            this.mq = new Queue<string>();
            this.timer = new Timer();
            this.timer.Interval = 100;
            this.timer.Enabled = true;

            this.timer.Tick += (sender, e) =>
            {
                try
                {
                    if (mq.Count > 0)
                    {
                        var script = mq.Dequeue();
                        this.webBrowser.RunJS(script);
                    }
                    //this.timer.Stop();
                }
                catch (Exception ex)
                {
                    Log.WriteLog("Error", ex.Message);
                }
            };
            this.timer.Start();
        }
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.OnMessage != null)
            {
                this.OnMessage(this.Title, CallbackType.Close);
            }
        }

        #region Implement Form Method
        public void close()
        {
            this.Close();
        }

        public void hide()
        {
            this.Hide();
        }

        public override void show(string parent)
        {
            try
            {
                if (string.IsNullOrEmpty(parent))
                {
                    //this.Show();
                    //return;
                    parent = "MainForm";
                }
                WindowHelper window = new WindowHelper(parent);
                if (window.Handle == IntPtr.Zero || window.Handle == this.Handle)
                {
                    this.Show();
                    return;
                }
                this.Hide();
                this.Show(window);
            }
            catch (Exception ex)
            {
                Log.WriteLog(string.Format("窗体：{0},调用方法：{1} 出错，具体如下：", this.Text, "ChromeBrowser.show()"), ex.ToString());
                this.OnMessage(this.Text, CallbackType.Error);
            }
        }

        public override void position(int x, int y)
        {
            try
            {
                this.Left = x;
                this.Top = y;
            }
            catch (Exception ex)
            {
                Log.WriteLog(string.Format("窗体：{0},调用方法：{1} 出错，具体如下：", this.Text, "ChromeBrowser.position()"), ex.ToString());
                this.OnMessage(this.Text, CallbackType.Error);
            }
        }

        public override void size(int width, int height)
        {
            this.Size = new Size(width, height);
        }

        public override void refresh(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                this.webBrowser.LoadURL(url);
                return;
            }
            this.webBrowser.Reload();
        }

        public override void exec(string script, bool asyn = false)
        {
            try
            {
                if (asyn)
                {
                    PushJS(script);
                }
                else
                {
                    this.webBrowser.RunJS(script);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(string.Format("窗体：{0},调用方法：{1} 出错，具体如下：", this.Text, "ChromeBrowser.exec()"), ex.ToString());
                this.OnMessage(this.Text, CallbackType.Error);
            }
        }


        public override void mode(int status)
        {
            switch (status)
            {
                case 0:
                    this.Visible = true;
                    this.TopMost = false;
                    this.Show();
                    break;
                case 1:
                    this.Visible = true;
                    this.TopMost = true;
                    this.Show();
                    break;
                case 2:
                    this.TopMost = true;
                    this.Visible = false;
                    this.ShowDialog();
                    break;
                default:
                    MessageBox.Show("请输入正确的窗体状态", "消息");
                    break;
            }
        }
        #endregion

    }
}