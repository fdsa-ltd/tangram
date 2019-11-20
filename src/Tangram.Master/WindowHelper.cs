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

namespace Tangram.Master
{
    public class WindowHelper : IWin32Window
    {

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);
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