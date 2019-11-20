using CefSharp.WinForms;
using CefSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tangram.CEF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private ChromiumWebBrowser chromiumWebBrowser;
        private void Form1_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.chromiumWebBrowser = new ChromiumWebBrowser("https://www.baidu.com/") { Dock = DockStyle.Fill };
            this.Controls.Add(this.chromiumWebBrowser);

        }
    }
}
