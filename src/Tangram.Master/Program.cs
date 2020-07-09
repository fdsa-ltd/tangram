using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Pkcs;
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
            if (args == null || args.Length == 0)
            {
                args = new string[] { "" };
            }
            //if (!SoftRegister.IsRegister())
            //{
            //    FormRegister register = new FormRegister();
            //    register.ShowDialog();
            //    return;
            //}

            var current = Process.GetCurrentProcess().Id;
            foreach (var item in Process.GetProcessesByName("Tangram"))
            {
                if (current != item.Id)
                {
                    var info = item.MainModule.FileName;
                    item.Kill();
                }
            }

            var path = Path.Combine(Application.StartupPath, "pid.txt");
            if (File.Exists(path))
            {
                var result = File.ReadAllLines(path).Select(m =>
                {
                    var pid = int.Parse(m);
                    var p = Process.GetProcessById(pid);

                    if (p != null)
                    {
                        p.Kill();
                    }
                    return pid;
                });
                File.Delete(path);

            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain(args.FirstOrDefault()));
        }
    }
}