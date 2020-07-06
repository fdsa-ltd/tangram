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
using Tangram.Builtin;

namespace Tangram.IE
{
    [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]
    [ComVisibleAttribute(true)]
    public class IEBrowser : BuiltinForm
    {

        private readonly WebBrowser webBrowser;
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
        public IEBrowser()
        {
            this.webBrowser = new WebBrowser();
            this.FormClosing += Form_FormClosing;
            this.webBrowser.Dock = DockStyle.Fill;
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.AllowWebBrowserDrop = false;
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser.WebBrowserShortcutsEnabled = false;
            this.webBrowser.ScrollBarsEnabled = false;
            this.webBrowser.ObjectForScripting = new GlobalForScript(this);
            this.panel1.Controls.Add(webBrowser);

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
                        this.webBrowser.Document.InvokeScript("eval", new object[] { script });
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
            //if (this.OnMessage != null)
            //{
            //    this.OnMessage(this.Title, CallbackType.Close);
            //}
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
                    this.Show();
                    return;
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
                Log.WriteLog(string.Format("窗体：{0},调用方法：{1} 出错，具体如下：", this.Text, "IEBrowser.show()"), ex.ToString());
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
                this.webBrowser.Navigate(url);
                return;
            }
            this.webBrowser.Refresh();
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
                    this.webBrowser.Document.InvokeScript("eval", new object[] { script });
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(string.Format("窗体：{0},调用方法：{1} 出错，具体如下：", this.Text, "IEBrowser.exec()"), ex.ToString());
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