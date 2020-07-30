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
using Gecko;
using Gecko.JQuery;

namespace Tangram.Gecko
{
    [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]
    [ComVisibleAttribute(true)]
    public class GeckoBrowser : BrowserForm, IFormBrowser
    {

        public Form Form { get { return this; } }
        private GeckoWebBrowser webBrowser = new GeckoWebBrowser();
        public event Events.EventCallback eventHandler;

        public GeckoBrowser()
        {
            this.FormClosing += Form_FormClosing;
            this.webBrowser.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(webBrowser);
        }
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.eventHandler != null)
            {
                this.eventHandler(this.Text, "close");
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
                this.eventHandler(this.Text, "error");
            }
        }

        public override void position(int x, int y)
        {
            this.Left = x;
            this.Top = y;
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
                    this.webBrowser.ExecuteJQuery("alert('test')");
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(string.Format("窗体：{0},调用方法：{1} 出错，具体如下：", this.Text, "IEBrowser.exec()"), ex.ToString());
                this.eventHandler(this.Text, "error");
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

        #region Implement Queue
        Queue<String> mq = new Queue<string>();
        Timer timer = new Timer();
        private void PushJS(string script)
        {

            this.mq.Enqueue(script);
            this.timer.Start();
        }
        #endregion

    }
}