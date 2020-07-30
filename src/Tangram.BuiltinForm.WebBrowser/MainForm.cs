using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using Tangram.Core;

namespace Tangram.Builtin.WebBrowser
{
    [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]
    [ComVisibleAttribute(true)]
    public partial class MainForm :
        BuiltinForm,
        // Form,
        IFormBrowser
    {
        private readonly System.Windows.Forms.WebBrowser webBrowser;
        public MainForm()
        {
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.webBrowser.Dock = DockStyle.Fill;
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.AllowWebBrowserDrop = false;
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser.WebBrowserShortcutsEnabled = false;
            this.webBrowser.ScrollBarsEnabled = false;
            this.webBrowser.ObjectForScripting = new GlobalForScript(this.GlobalHandle,this.Handle);
            this.Controls.Add(this.webBrowser);
            this.webBrowser.Navigate("https://baidu.com");
        }

        public override void exec(string script)
        {
            this.webBrowser.Document.InvokeScript(script);
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
    }
}
