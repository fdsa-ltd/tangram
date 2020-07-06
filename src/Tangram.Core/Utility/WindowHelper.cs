using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tangram.Core
{
    public class WindowHelper : IWin32Window
    {

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool ShowWindow(IntPtr hWnd, short State);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, Int32 msg, Int32 wParam, Int32 lParam);



        [DllImport("user32.dll", SetLastError = true)]
        public static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, UInt32 dwNewLong);



        [DllImport("user32", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetFocus(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        const UInt32 SWP_SHOWWINDOW = 0x0040;
        const UInt32 SWP_HIDEWINDOW = 0x0080;
        public static void Taskbar(bool show = true)
        {
            var hWnd = FindWindow("Shell_traywnd", "");
            if (show)
            {

                SetWindowPos(hWnd, 0, 0, 0, 0, 0, SWP_SHOWWINDOW);
                return;
            }
            SetWindowPos(hWnd, 0, 0, 0, 0, 0, SWP_HIDEWINDOW);
        }
        public WindowHelper(int handle)
        {
            this.Handle = new IntPtr(handle);
        }
        public WindowHelper(IntPtr handle)
        {
            this.Handle = handle;
        }
        public WindowHelper(string title)
        {
            this.Handle = FindWindow(null, title);
        }
        public IntPtr Handle { get; private set; }

        public void ShowChild(Form form)
        {
            form.Hide();
            form.Show(this);
        }
    }
}