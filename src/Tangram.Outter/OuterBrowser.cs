using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Tangram.Core;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using Tangram.Core.Event;

namespace Tangram.Core
{
    public partial class OuterBrowser : Form, IFormBrowser
    {
        private static readonly string StartTime = DateTime.Now.Ticks.ToString();
        Features features;

        public OuterBrowser()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.Left = 10000;
            InitializeComponent();
        }
        private Process program = new Process();

        public void Init(Features features)
        {
            this.features = features;
        }

        private void PluginBrowser_Load(object sender, EventArgs e)
        {
            var name = this.features.Get("name");
            this.Text = name;
            var url = this.features.Get("url");
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show($"url不能为空");
                return;
            }
            var index = url.IndexOf('#');
            var tail = string.Empty;
            if (index > 0)
            {
                tail = url.Substring(index);
                url = url.Substring(0, index);
            }
            if (url.Contains("?"))
            {
                url += "&_title=" + this.Text + tail;
            }
            else
            {
                url += "?_title=" + this.Text + tail;
            }
            var fileName = this.features.Get("filename");
            var args = this.features.Get("args").Replace("{url}", url).Replace("{name}", this.Text + StartTime);
            var workspace = this.features.Get("workspace");
            if (string.IsNullOrEmpty(workspace))
            {
                workspace = Application.StartupPath;
            }
            //需要启动的程序
            program.StartInfo.FileName = fileName;
            program.StartInfo.Arguments = args;
            program.StartInfo.WorkingDirectory = workspace;
            program.Start();
            program.WaitForInputIdle();
            //加载当前窗口样式
            this.FormBorderStyle = FormBorderStyle.None;
            //this.TransparencyKey = Color.Blue;
            //this.BackColor = Color.Blue;
            var countDown = 10000;
            while (program.MainWindowHandle == IntPtr.Zero)
            {
                if (countDown < 0)
                {
                    break;
                }
                Thread.Sleep(countDown / 100);
                countDown -= 100;
            }
            //Thread.Sleep(1000);
            SetParent(program.MainWindowHandle, this.Handle);
            //改变尺寸
            ResizeControl();
        }
        //控制嵌入程序的位置和尺寸
        private void ResizeControl()
        {
            //SendMessage(program.MainWindowHandle, WM_COMMAND, WM_PAINT, 0);
            //PostMessage(program.MainWindowHandle, WM_QT_PAINT, 0, 0);

            //SetWindowPos(program.MainWindowHandle, HWND_TOP, 0, 0,
            //  (int)this.Width,
            //  (int)this.Height,
            //  SWP_FRAMECHANGED);
            //SendMessage(program.MainWindowHandle, WM_COMMAND, WM_SIZE, 0);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            ResizeControl();
        }

        private void PluginBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (this.OnMessage != null)
            //{
            //    this.OnMessage(this.Title, CallbackType.Close);
            //}
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
                    var result = ScreenManager.External.Find(title);
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
                        WindowMessageManager.WriteData(newForm.ToString());
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
            if (this.OnMessage != null)
            {
                this.OnMessage(this.Title, CallbackType.Close);
            }
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
                Log.WriteLog(string.Format("窗体：{0},调用方法：{1} 出错，具体如下：", this.Text, "IEBrowser.show()"), ex.ToString());
                this.OnMessage(this.Text, CallbackType.Error);

                MessageBox.Show(ex.Message, "出错");
            }
        }

        public void size(int width, int height)
        {
            this.Size = new Size(width, height);
            ResizeControl();
        }

        public void refresh(string url)
        {
            RPCMessageManager.External.SendToAsync("", this.Title);
        }


        public void site(int x, int y)
        {
            this.Left = x;
            this.Top = y;
            ResizeControl();
        }

        public void exec(string script)
        {
            RPCMessageManager.External.SendToAsync(script, this.Title);
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

        #region private
        [DllImport("user32.dll")]
        private static extern int SetParent(IntPtr hWndChild, IntPtr hWndParent);


        //[DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        //public static extern long GetWindowLong(IntPtr hwnd, int nIndex);
        //[DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        //public static extern long SetWindowLong(IntPtr hwnd, int nIndex, long dwNewLong);
        //[DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]
        //public static extern int SetLayeredWindowAttributes(IntPtr Handle, int crKey, byte bAlpha, int dwFlags);
        //const int GWL_EXSTYLE = -20;
        //const int WS_EX_TRANSPARENT = 0x20;
        //const int WS_EX_LAYERED = 0x80000;
        //const int LWA_ALPHA = 2;


        //const int WM_COPYDATA = 0x004A;
        //const int WM_SYSCOMMAND = 0x0112;
        //const int WM_NCHITTEST = 0x0084;
        //const int SC_MOVE = 0xF010;
        //const int HTCAPTION = 0x0002;
        //const int LEFT = 10;            //左边界
        //const int RIGHT = 11;           //右边界
        //const int TOP = 12;             //上边界
        //const int LEFTTOP = 13;         //左上角
        //const int RIGHTTOP = 14;        //右上角
        //const int BOTTOM = 15;          //下边界
        //const int LEFTBOTTOM = 16;      //左下角
        //const int RIGHTBOTTOM = 17;     //右下角

        ///// <summary>
        ///// 拖动无窗体的控件
        ///// </summary>
        ///// <returns></returns>
        //[DllImport("user32.dll")]
        //public static extern bool ReleaseCapture();
        //[DllImport("user32.dll")]
        //public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);


        //[DllImport("user32.dll")]
        //private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        //[DllImport("user32.dll", SetLastError = true)]
        //private static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        //[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        //private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        //[DllImport("user32.dll")]
        //private static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        //[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        //private static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint newLong);

        //private const int HWND_TOP = 0x0;
        //private const int WM_COMMAND = 0x0112;
        //private const int WM_QT_PAINT = 0xC2DC;
        //private const int WM_PAINT = 0x000F;
        //private const int WM_SIZE = 0x0005;
        //private const int SWP_FRAMECHANGED = 0x0020;



        public event EventCallback OnMessage;

        //protected override void WndProc(ref Message m)
        //{
        //    switch (m.Msg)
        //    {
        //        case WM_NCHITTEST:
        //            base.WndProc(ref m);
        //            Point vPoint = new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF);
        //            vPoint = PointToClient(vPoint);
        //            if (vPoint.X <= 5)
        //                if (vPoint.Y <= 5)
        //                    m.Result = (IntPtr)LEFTTOP;
        //                else if (vPoint.Y >= ClientSize.Height - 5)
        //                    m.Result = (IntPtr)LEFTBOTTOM;
        //                else m.Result = (IntPtr)LEFT;
        //            else if (vPoint.X >= ClientSize.Width - 5)
        //                if (vPoint.Y <= 5)
        //                    m.Result = (IntPtr)RIGHTTOP;
        //                else if (vPoint.Y >= ClientSize.Height - 5)
        //                    m.Result = (IntPtr)RIGHTBOTTOM;
        //                else m.Result = (IntPtr)RIGHT;
        //            else if (vPoint.Y <= 5)
        //                m.Result = (IntPtr)TOP;
        //            else if (vPoint.Y >= ClientSize.Height - 5)
        //                m.Result = (IntPtr)BOTTOM;
        //            break;
        //        case WM_COPYDATA:
        //            var message = WindowMessageManager.GetFormMessage(m);
        //            this.processMessage(message);

        //            m.Result = Marshal.StringToHGlobalAnsi("test");

        //            break;
        //        default:
        //            base.WndProc(ref m);
        //            break;
        //    }
        //}

        #endregion
    }
}
