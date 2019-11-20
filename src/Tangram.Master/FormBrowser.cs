using Tangram.Core;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;
using Tangram.Master;

namespace Tangram.IEBrowser
{
    public partial class FormBrowser : Form, IFormBrowser
    {
        #region ctor
        protected Features features;
        public FormBrowser()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }
        public virtual void Init(string name, string url, Features features)
        {
            this.Text = this.Name = name;
            this.features = features;
        }
        protected void RaiseEvent(CallbackType type)
        {
            if (this.OnMessage != null)
            {
                this.OnMessage(this.Text, type);
            }
        }
        public event EventCallback OnMessage;

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
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion

        #region Form Event
        private void Form_Load(object sender, EventArgs e)
        {
            this.TransparencyKey = Color.Blue;
            this.BackColor = Color.Blue;
            //this.Opacity = 0.99;
            //this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void Form_SizeChanged(object sender, EventArgs e)
        {
            ResizeControl();

        }
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (this.OnMessage != null)
            {
                this.OnMessage(this.Text, CallbackType.Close);
            }
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
        private void processMessage(WindowMessage message)
        {
            if (message == null)
            {
                return;
            }
            switch (message.Type)
            {
                case WindowMessageType.Show:
                    string parent = message.Data.GetString(0);
                    this.show(parent);
                    break;
                case WindowMessageType.Hide:
                    this.Hide();
                    break;
                case WindowMessageType.Close:
                    this.Close();
                    break;
                case WindowMessageType.Site:
                    var left = message.Data.GetInt(0);
                    var top = message.Data.GetInt(1);
                    this.site(left, top);
                    break;
                case WindowMessageType.Size:
                    var width = message.Data.GetInt(0);
                    var height = message.Data.GetInt(1);
                    this.size(width, height);
                    break;
                case WindowMessageType.Refresh:
                    var url = message.Data.GetString(0);
                    this.refresh(url);
                    break;
                case WindowMessageType.Mode:
                    var status = message.Data.GetInt(0);
                    this.mode(status);
                    break;
                case WindowMessageType.Exec:
                    var script = message.Data.GetString(0);
                    this.exec(script);
                    break;
                case WindowMessageType.None:
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Implement Form Method
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

        public virtual void site(int x, int y)
        {
            this.Left = x;
            this.Top = y;
            ResizeControl();
        }

        public virtual void size(int width, int height)
        {
            this.Size = new Size(width, height);
            ResizeControl();
        }

        public virtual void refresh(string url)
        {

        }

        public virtual void exec(string script)
        {

        }

        public virtual void mode(int status)
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

        public virtual void close()
        {
            if (this.OnMessage != null)
            {
                this.OnMessage(this.Text, CallbackType.Close);
            }
            this.Close();
        }

        public void hide()
        {
            this.Hide();
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