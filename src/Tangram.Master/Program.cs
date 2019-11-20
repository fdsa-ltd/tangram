using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Tangram.Core;

namespace Tangram.Master
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!SoftRegister.IsRegister())
            {
                FormRegister register = new FormRegister();
                register.ShowDialog();
                return;
            }
            
            var current = Process.GetCurrentProcess().Id;
            foreach (var item in Process.GetProcessesByName("Tangram"))
            {
                if (current != item.Id)
                {
                    item.Kill();
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain(args.FirstOrDefault()));
        }
    }
}