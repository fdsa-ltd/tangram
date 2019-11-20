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

namespace Tangram.IE
{
    public class Browser : BrowserForm
    {

        private WebBrowser webBrowser = new WebBrowser();

        public Browser(string name, long hWnd)
        {
            this.controller = new IntPtr(hWnd);
            this.Text = name;
            webBrowser.Dock = DockStyle.Fill;
            webBrowser.ScriptErrorsSuppressed = true;
            //webBrowser.AllowWebBrowserDrop = false;
            webBrowser.IsWebBrowserContextMenuEnabled = false;
            //webBrowser.WebBrowserShortcutsEnabled = false;
            //webBrowser.ObjectForScripting = new GlobalForScript(this.controller,this.Handle,this.Text);
            this.panel1.Controls.Add(webBrowser);
            timer.Tick += (sender, e) =>
            {
                if (mq.Count > 0)
                {
                    var script = mq.Dequeue();
                    this.webBrowser.Document.InvokeScript("eval", new object[] { script });
                }
                this.timer.Stop();
            };
            timer.Interval = 100;
            //this.ShowInTaskbar = false;
        }

        #region Implement Form Method
        public override void refresh(string url)
        {
            webBrowser.ObjectForScripting = new GlobalForScript(this.controller, this.Handle, this.Text);
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