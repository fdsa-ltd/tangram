using CefSharp.WinForms;
using CefSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tangram.Core;
using Tangram.Utility;
using CefSharp.ModelBinding;
using CefSharp.Internals;
using CefSharp.Event;

namespace Tangram.CEF
{
    public partial class CEFBrowser : Form, IFormBrowser
    {
        public Form Form { get { return this; } }
        private ChromiumWebBrowser webBrowser;
        public event Events.EventCallback eventHandler;

        public CEFBrowser()
        {
            InitializeComponent();
        }

        private void CEFBrowser_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TransparencyKey = Color.Blue;
            this.BackColor = Color.Blue;
            this.FormBorderStyle = FormBorderStyle.None;

            //this.WindowState = FormWindowState.Maximized; 
        }
        //public class RenderProcessMessageHandler : IRenderProcessMessageHandler
        //{
        //    // Wait for the underlying JavaScript Context to be created. This is only called for the main frame.
        //    // If the page has no JavaScript, no context will be created.
        //    public void OnContextCreated(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        //    {
        //        //const string script = "document.addEventListener('DOMContentLoaded', function(){ alert('DomLoaded'); });";
        //        //frame.ExecuteJavaScriptAsync(script);
        //    }

        //    public void OnContextReleased(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        //    {

        //    }

        //    public void OnFocusedNodeChanged(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IDomNode node)
        //    {

        //    }

        //    public void OnUncaughtException(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, JavascriptException exception)
        //    {

        //    }
        //}

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.eventHandler != null)
            {
                this.eventHandler(this.Text, "close");
            }
            Cef.Shutdown();
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

        public void show(string parent)
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

        public void position(int x, int y)
        {
            this.Left = x;
            this.Top = y;
        }

        public void size(int width, int height)
        {
            this.Size = new Size(width, height);
        }
        public class MenuHandler : IContextMenuHandler
        {
            public void OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
            {
            }

            public bool OnContextMenuCommand(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
            {
                return true;
            }

            public void OnContextMenuDismissed(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
            {
            }

            public bool RunContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
            {
                return true;
            }
        } 

        public class Binder : IBinder
        {
            public object Bind(object obj, Type modelType)
            {
                throw new NotImplementedException();
            }
        }
        public void refresh(string url)
        {
            if (this.webBrowser == null)
            {
                this.webBrowser = new ChromiumWebBrowser(url) { Dock = DockStyle.Fill };
                this.webBrowser.RegisterJsObject("external", new GlobalForScript(this), new BindingOptions() { CamelCaseJavascriptNames = false,   Binder = new Binder() });
               
                //this.webBrowser.TransparencyKey = Color.Blue;
                this.webBrowser.BackColor = Color.Blue;
                this.Controls.Add(this.webBrowser);
                this.webBrowser.MenuHandler = new MenuHandler();
                //this.webBrowser.RenderProcessMessageHandler = new RenderProcessMessageHandler();
                //Wait for the page to finish loading (all resources will have been loaded, rendering is likely still happening)
                //this.webBrowser.LoadingStateChanged += (sender, args) =>
                //{
                //    Wait for the Page to finish loading
                //    if (args.IsLoading == false)
                //        {
                //            this.webBrowser.ExecuteScriptAsync("alert('All Resources Have Loaded');");
                //        }
                //};

                //Wait for the MainFrame to finish loading
                //this.webBrowser.FrameLoadEnd += (sender, args) =>
                //{
                //    Wait for the MainFrame to finish loading
                //    if (args.Frame.IsMain)
                //        {
                //            args.Frame.ExecuteJavaScriptAsync("alert('MainFrame finished loading');");
                //        }
                //};
                return;
            }
            if (!string.IsNullOrEmpty(url))
            {
                this.webBrowser.Load(url);
                return;
            }
            this.webBrowser.Refresh();
        }
        public void exec(string script, bool asyn = false)
        {
            try
            {
                if (asyn)
                {
                    PushJS(script);
                }
                else
                {
                    this.webBrowser.GetMainFrame().EvaluateScriptAsync(script).Wait();
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(string.Format("窗体：{0},调用方法：{1} 出错，具体如下：", this.Text, "IEBrowser.exec()"), ex.ToString());
                this.eventHandler(this.Text, "error");
                MessageBox.Show(ex.Message, "错误");
            }
        }

        public void mode(int status)
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