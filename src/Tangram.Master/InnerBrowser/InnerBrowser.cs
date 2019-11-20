using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tangram.Core;
using Tangram.Master;

namespace Tangram.IEBrowser
{
    public class InnerBrowser : FormBrowser, IFormBrowser
    {
        private readonly WebBrowser webBrowser;
        public InnerBrowser()
        {
            this.webBrowser = new WebBrowser();
            this.webBrowser.Dock = DockStyle.Fill;
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.AllowWebBrowserDrop = false;
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser.WebBrowserShortcutsEnabled = false;
            this.webBrowser.ScrollBarsEnabled = false;
            this.webBrowser.ObjectForScripting = new GlobalForScript(this);
            this.Controls.Add(webBrowser);
        }

        #region Implement Form Method 
        public override void refresh(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                this.webBrowser.Navigate(url);
                return;
            }
            this.webBrowser.Refresh();
        }
        public override void exec(string script)
        {
            try
            {
                this.webBrowser.Document.InvokeScript("eval", new object[] { script });
            }
            catch (Exception ex)
            {
                Log.WriteLog(string.Format("窗体：{0},调用方法：{1} 出错，具体如下：", this.Text, "IEBrowser.exec()"), ex.ToString());
                base.RaiseEvent(CallbackType.Error);
            }
        }
        public override void Init(string name, string url, Features features)
        {
            base.Init(name, url, features);
            this.webBrowser.Navigate(url);
        }
        #endregion
    }
}