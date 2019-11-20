using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tangram.CEF
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            CefSettings settings = new CefSettings()
            {
                Locale = "zh-CN",
                IgnoreCertificateErrors = true,
                //BrowserSubprocessPath = @"x86\CefSharp.BrowserSubprocess.exe"
            };
            CefSharp.Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);

            Application.Run(new CEFBrowser());
        }
    }
}
