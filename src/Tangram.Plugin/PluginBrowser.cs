using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Tangram.Core;
using Tangram.Utility;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Tangram.Plugin
{
    public partial class PluginBrowser : Form, IFormBrowser
    {
        static string FILEPATH = Path.Combine(Application.StartupPath, "Plugins\\IE64.exe");
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
        public event EventCallback OnMessage;
        Process subProcess;


        public PluginBrowser()
        {
            InitializeComponent();
            //this.Show();
            this.subProcess = Process.GetProcessesByName("IE64").FirstOrDefault();
            if (this.subProcess != null)
            {
                WindowMessageManager.Send(this.subProcess.MainWindowHandle, WindowMessageType.None, this.subProcess.MainWindowTitle, this.Handle.ToInt64().ToString());
            }
        }



        private void Button1_Click(object sender, EventArgs e)
        {
            var type = (WindowMessageType)Enum.Parse(typeof(WindowMessageType), this.textBox1.Text, true);
            var args = this.textBox2.Text.Split(',', ';').Select(m => m.Trim()).ToArray();
            WindowMessageManager.Send(this.subProcess.MainWindowHandle, type, this.subProcess.MainWindowTitle, args);
        }
        private void PluginBrowser_Load(object sender, EventArgs e)
        {

        }
        private void PluginBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        private void processMessage(WindowMessage message)
        {
            if (message == null)
            {
                return;
            }
            switch (message.Type)
            {
                case WindowMessageType.GlobalFind:
                    string title = message.Data.GetString(0);
                    var form = ScreenManager.External.Find(title);
                    FormInfo result = new FormInfo() { Handle = form.Handle, Name = form.Title };
                    if (result == null)
                    {
                        WindowMessageManager.WriteData(null);
                    }
                    else
                    {
                        WindowMessageManager.WriteData(result.ToString());
                    }
                    break;
                case WindowMessageType.GlobalOpen:
                    string url = message.Data.GetString(0);
                    string name = message.Data.GetString(1);
                    string type = message.Data.GetString(2);
                    string features = message.Data.GetString(3);
                    string parent = message.Data.GetString(4);
                    var newForm = ScreenManager.External.Open(url, name, type, features, parent);

                    if (newForm == null)
                    {
                        WindowMessageManager.WriteData(null);
                    }
                    else
                    {
                        FormInfo resultForm = new FormInfo() { Handle = newForm.Handle, Name = newForm.Title };
                        WindowMessageManager.WriteData(resultForm.ToString());
                    }
                    break;
                case WindowMessageType.GlobalExec:
                    var script = message.Data.GetString(0);
                    bool asyn = message.Data.GetBool(1);
                    ScreenManager.External.Exec(message.To, script, asyn);
                    break;
                case WindowMessageType.GlobalInvoke:
                    string method = message.Data.GetString(0);
                    var args = message.Data.Skip(1).Select(m => m.ToString()).ToArray();
                    ScreenManager.External.Invoke(message.To, method, args);
                    break;
                default:
                    break;
            }
        }

        #region Implement Form Method
        public void close()
        {
            this.subProcess.Kill();
            this.Close();
        }

        public void hide()
        {
            WindowMessageManager.Send(this.subProcess.MainWindowHandle, WindowMessageType.Hide, this.subProcess.MainWindowTitle);
        }

        public void show(string parent)
        {
            try
            {
                if (this.subProcess == null)
                {
                    var title = this.Text;
                    this.Text += "_" + title;
                    var args = string.Join(" ", title, this.Handle.ToInt64());
                    this.subProcess = Process.Start(FILEPATH, args);
                    Thread.Sleep(1000);
                }
                if (this.subProcess != null)
                {
                    WindowMessageManager.Send(this.subProcess.MainWindowHandle, WindowMessageType.None, this.subProcess.MainWindowTitle, this.Handle.ToInt64().ToString());
                    WindowMessageManager.Send(this.subProcess.MainWindowHandle, WindowMessageType.Show, this.subProcess.MainWindowTitle, parent);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(string.Format("窗体：{0},调用方法：{1} 出错，具体如下：", this.Text, "IEBrowser.show()"), ex.ToString());
                this.OnMessage(this.Text, CallbackType.Error);
                MessageBox.Show(ex.Message, "出错");
            }
        }

        public void position(int x, int y)
        {
            WindowMessageManager.Send(this.subProcess.MainWindowHandle, WindowMessageType.Site, this.subProcess.MainWindowTitle, x, y);
        }

        public void size(int width, int height)
        {
            WindowMessageManager.Send(this.subProcess.MainWindowHandle, WindowMessageType.Size, this.subProcess.MainWindowTitle, width, height);
        }

        public void refresh(string url)
        {
            if (this.subProcess == null)
            {
                var title = this.Text;
                this.Text += "_" + title;
                var args = string.Join(" ", title, this.Handle.ToInt64());
                this.subProcess = Process.Start(FILEPATH, args);
            }
            if (this.subProcess != null)
            {
                WindowMessageManager.Send(this.subProcess.MainWindowHandle, WindowMessageType.None, this.subProcess.MainWindowTitle, this.Handle.ToInt64().ToString());
                WindowMessageManager.Send(this.subProcess.MainWindowHandle, WindowMessageType.Refresh, this.subProcess.MainWindowTitle, url);
            }
        }
        public void exec(string script, bool asyn = false)
        {
            WindowMessageManager.Send(this.subProcess.MainWindowHandle, WindowMessageType.Exec, this.subProcess.MainWindowTitle, script, asyn);
        }

        public void mode(int status)
        {
            WindowMessageManager.Send(this.subProcess.MainWindowHandle, WindowMessageType.Mode, this.subProcess.MainWindowTitle, status);
        }
        #endregion

        #region private
        /// <summary>
        /// 拖动无窗体的控件
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        const int WM_COPYDATA = 0x004A;
        const int WM_SYSCOMMAND = 0x0112;
        const int WM_NCHITTEST = 0x0084;
        const int SC_MOVE = 0xF010;
        const int HTCAPTION = 0x0002;
        const int LEFT = 10;            //左边界
        const int RIGHT = 11;           //右边界
        const int TOP = 12;             //上边界
        const int LEFTTOP = 13;         //左上角
        const int RIGHTTOP = 14;        //右上角
        const int BOTTOM = 15;          //下边界
        const int LEFTBOTTOM = 16;      //左下角
        const int RIGHTBOTTOM = 17;     //右下角


        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    Point vPoint = new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF);
                    vPoint = PointToClient(vPoint);
                    if (vPoint.X <= 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)LEFTTOP;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)LEFTBOTTOM;
                        else m.Result = (IntPtr)LEFT;
                    else if (vPoint.X >= ClientSize.Width - 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)RIGHTTOP;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)RIGHTBOTTOM;
                        else m.Result = (IntPtr)RIGHT;
                    else if (vPoint.Y <= 5)
                        m.Result = (IntPtr)TOP;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)BOTTOM;
                    break;
                case WM_COPYDATA:
                    var message = WindowMessageManager.GetFormMessage(m);
                    this.processMessage(message);

                    m.Result = Marshal.StringToHGlobalAnsi("test");

                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion
    }
}
