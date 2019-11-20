using Tangram.Core;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using Tangram.IEBrowser;

namespace Tangram.BuiltinBrowser
{
    public partial class OuterBrowser : FormBrowser
    {
        #region ctor 
        private Process program = new Process();

        public override void Init(string name, string url, Features features)
        {
            base.Init(name, url, features);
           
            var type = this.features.Get("type");
            if (!ScreenManager.External.plugins.ContainsKey(type))
            {
                MessageBox.Show($"{type}没有注册");
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

            var plugin = ScreenManager.External.plugins[type];
            var args = string.Join(" ", ScreenManager.External.plugins[type].Arguments);

            //需要启动的程序

            program.StartInfo.FileName = plugin.FileName;
            program.StartInfo.Arguments = args.Replace("{url}", url).Replace("{name}", this.Text + DateTime.Now.Ticks.ToString());
            program.StartInfo.WorkingDirectory = Application.StartupPath;
            program.Start();
            program.WaitForInputIdle();
            var countDown = 3000;
            while (program.MainWindowHandle == IntPtr.Zero)
            {
                if (countDown < 0)
                {
                    break;
                }
                Thread.Sleep(countDown / 100);
                countDown -= 100;
            }
            SetParent(program.MainWindowHandle, this.Handle);
        }

        #endregion


        #region Implement Form Method

        public override void refresh(string url)
        {

        }

        public override void exec(string script)
        {
            WebMessage message = new WebMessage()
            {
                from = this.Text,
                type = "exec",
                data = new object[] { script }
            };
            ScreenManager.External.SendToAsync(message.ToString(), this.Text);

        }
        public override void close()
        {
            base.close();
        }
        #endregion

        #region 拖动窗体
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
        private void BrowserForm_MouseDown(object sender, MouseEventArgs e)
        {
            //拖动窗体
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        #endregion

        #region Windows API

        [DllImport("user32.dll")]
        private static extern int SetParent(IntPtr hWndChild, IntPtr hWndParent);
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern long GetWindowLong(IntPtr hwnd, int nIndex);
        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern long SetWindowLong(IntPtr hwnd, int nIndex, long dwNewLong);
        [DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]
        public static extern int SetLayeredWindowAttributes(IntPtr Handle, int crKey, byte bAlpha, int dwFlags);
        const int GWL_EXSTYLE = -20;
        const int WS_EX_TRANSPARENT = 0x20;
        const int WS_EX_LAYERED = 0x80000;
        const int LWA_ALPHA = 2;

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint newLong);

        private const int HWND_TOP = 0x0;
        private const int WM_COMMAND = 0x0112;
        private const int WM_QT_PAINT = 0xC2DC;
        private const int WM_PAINT = 0x000F;
        private const int WM_SIZE = 0x0005;
        private const int SWP_FRAMECHANGED = 0x0020;

        #endregion
    }
}
