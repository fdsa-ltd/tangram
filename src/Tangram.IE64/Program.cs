using System;
using System.Windows.Forms;
using System.Linq;
using Tangram.Utility;
using System.Diagnostics;

namespace Tangram.IE
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                IEVersionHelper.BrowserEmulationSet();
                return;
            }
            //var current = Process.GetCurrentProcess().Id;
            //foreach (var item in Process.GetProcessesByName("IE64"))
            //{
            //    if (current != item.Id)
            //    {
            //        item.Kill();
            //    }
            //}

            long hWnd;
            if (long.TryParse(args[1], out hWnd))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Browser(args[0], hWnd));
            }
        }
    }
}